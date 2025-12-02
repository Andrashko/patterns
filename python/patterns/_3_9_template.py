from __future__ import annotations

from abc import ABC, abstractmethod
from typing import TypeVar,  Callable, Protocol, TypedDict

# # Типи для generic-шаблону
ConnT = TypeVar("ConnT")      # тип "з'єднання"
ResultT = TypeVar("ResultT")  # тип результату операції


class TransactionTemplate [ConnT, ResultT](ABC):
    """
    Узагальнений Template Method для транзакцій.
    ConnT  – тип з'єднання (connection/session/handle)
    ResultT – тип результату, який повертає operation
    """

    def run_in_transaction(self, operation: Callable[[ConnT], ResultT]) -> ResultT:
        """
        Template Method:
        1. begin() -> ConnT
        2. operation(conn) -> ResultT
        3. commit/rollback
        4. cleanup
        """
        conn: ConnT | None = None
        try:
            conn = self.begin()
            result: ResultT = operation(conn)
            self.commit(conn)
            return result
        except Exception:
            if conn is not None:
                self.rollback(conn)
            raise
        finally:
            if conn is not None:
                self.cleanup(conn)

    # --- абстрактні кроки, які реалізують підкласи ---

    @abstractmethod
    def begin(self) -> ConnT:
        """Старт транзакції, повертає об'єкт з'єднання."""
        ...

    @abstractmethod
    def commit(self, conn: ConnT) -> None:
        ...

    @abstractmethod
    def rollback(self, conn: ConnT) -> None:
        ...

    @abstractmethod
    def cleanup(self, conn: ConnT) -> None:
        """Закриття/повернення в пул/очищення ресурсу."""
        ...


class DBConnection(Protocol):
    def execute(self, sql: str, params: tuple | dict | None = None) -> None:
        ...


class FakePostgresConnection:
    def __init__(self) -> None:
        self.in_tx: bool = False

    def execute(self, sql: str, params: tuple | dict | None = None) -> None:
        print(f"[Postgres] EXECUTE: {sql} {params or ''}")


class PostgresTransaction(TransactionTemplate[FakePostgresConnection, ResultT]):
    def begin(self) -> FakePostgresConnection:
        print("[Postgres] open connection")
        conn = FakePostgresConnection()
        conn.in_tx = True
        print("[Postgres] BEGIN")
        return conn

    def commit(self, conn: FakePostgresConnection) -> None:
        if conn.in_tx:
            print("[Postgres] COMMIT")
            conn.in_tx = False

    def rollback(self, conn: FakePostgresConnection) -> None:
        if conn.in_tx:
            print("[Postgres] ROLLBACK")
            conn.in_tx = False

    def cleanup(self, conn: FakePostgresConnection) -> None:
        print("[Postgres] close connection")


class InMemoryDB:
    def __init__(self) -> None:
        self._data: dict[str, object] = {}
        self._snapshot: dict[str, object] | None = None

    def set(self, key: str, value: object) -> None:
        self._data[key] = value

    def get_all(self) -> dict[str, object]:
        return dict(self._data)

    def begin_tx(self) -> None:
        self._snapshot = dict(self._data)

    def commit_tx(self) -> None:
        self._snapshot = None

    def rollback_tx(self) -> None:
        if self._snapshot is not None:
            self._data = self._snapshot
            self._snapshot = None


class InMemoryTransaction(TransactionTemplate[InMemoryDB, ResultT]):
    def __init__(self, db: InMemoryDB) -> None:
        self.db = db

    def begin(self) -> InMemoryDB:
        print("[InMemory] BEGIN")
        self.db.begin_tx()
        return self.db

    def commit(self, conn: InMemoryDB) -> None:
        print("[InMemory] COMMIT")
        conn.commit_tx()

    def rollback(self, conn: InMemoryDB) -> None:
        print("[InMemory] ROLLBACK")
        conn.rollback_tx()

    def cleanup(self, conn: InMemoryDB) -> None:
        print("[InMemory] CLEANUP")


class User(TypedDict):
    id: int
    name: str
