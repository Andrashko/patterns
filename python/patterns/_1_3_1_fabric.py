from typing import Protocol, TypeVar, Any

T = TypeVar("T")


class Connector (Protocol):
    def connect(self) -> bool: ...
    def disconnect(self) -> bool: ...


class BaseConnector(Connector):
    def __init__(self, options: dict[str, Any]):
        self._options: dict[str, Any] = options


class SQLConnector (BaseConnector):
    def connect(self) -> bool:
        print("Connection to SQL database...")
        return True

    def disconnect(self) -> bool:
        print("Disconnection from SQL database...")
        return True


class AWSConnector (BaseConnector):
    def connect(self) -> bool:
        print("Connection to AWS...")
        return True

    def disconnect(self) -> bool:
        print("Disconnection from AWS...")
        return True


class Cursor (Protocol[T]):
    def read(self) -> T: ...
    def write(self, data: T): ...


class BaseCursor(Cursor[str]):
    def __init__(self, connector: Connector):
        super().__init__()
        self._connector = connector


class SQLCursor (BaseCursor):
    def read(self) -> str:
        print("SELECT * FROM table")
        return "some data"

    def write(self, data: str):
        print(f"INSERT {data} INTO table")


class AWSCursor (BaseCursor):
    def read(self) -> str:
        print("Loading data")
        return "some data"

    def write(self, data: str):
        print(f"Sending {data}")


class DatabaseFabric (Protocol):
    def create_connector(self, options: dict[str, Any]) -> Connector: ...
    def create_cursor(self, connector: Connector) -> Cursor[Any]: ...


class SQLDatabaseFabric (DatabaseFabric):
    def create_connector(self, options: dict[str, Any]) -> SQLConnector:
        return SQLConnector(options)

    def create_cursor(self, connector: Connector) -> SQLCursor:
        return SQLCursor(connector)


class AWSDatabaseFabric (DatabaseFabric):
    def create_connector(self, options: dict[str, Any]) -> AWSConnector:
        return AWSConnector(options)

    def create_cursor(self, connector: Connector) -> AWSCursor:
        return AWSCursor(connector)
