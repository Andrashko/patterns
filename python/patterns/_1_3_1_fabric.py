from os import write
from typing import Protocol, TypeVar, Generic

T = TypeVar("T")


class Connector (Protocol):
    def __init__(self, options: dict):
        self._options = options

    def connect(self) -> bool:
        ...

    def disconnect(self) -> bool:
        ...


class SQLConnector (Connector):
    def connect(self) -> bool:
        print("Connection to SQL database...")
        return True

    def disconnect(self) -> bool:
        print("Disconnection from SQL database...")
        return True


class AWSConnector (Connector):
    def connect(self) -> bool:
        print("Connection to AWS...")
        return True

    def disconnect(self) -> bool:
        print("Disconnection from AWS...")
        return True


class Cursor (Protocol[T]):
    def __init__(self, connector: Connector):
        super().__init__()
        self._connector = connector

    def read(self) -> T:
        ...

    def write(self, data: T):
        ...


class SQLCursor (Cursor):
    def read(self) -> str:
        print("SELECT * FROM table")
        return "some data"

    def write(self, data: str):
        print(f"INSERT {data} INTO table")


class AWSCursor (Cursor):
    def read(self) -> str:
        print("Loading data")
        return "some data"

    def write(self, data: str):
        print(f"Sending {data}")


class DatabaseFabric (Protocol):
    def create_connector(self, options: dict) -> Connector:
        ...

    def create_cursor(self, connector: Connector) -> Cursor:
        ...


class SQLDatabaseFabric (DatabaseFabric):
    def create_connector(self, options: dict) -> SQLConnector:
        return SQLConnector(options)

    def create_cursor(self, connector: Connector) -> SQLCursor:
        return SQLCursor(connector)


class AWSDatabaseFabric (DatabaseFabric):
    def create_connector(self, options: dict) -> AWSConnector:
        return AWSConnector(options)

    def create_cursor(self, connector: Connector) -> AWSCursor:
        return AWSCursor(connector)
