from __future__ import annotations
from typing import Any

from patterns._1_3_1_fabric import DatabaseFabric, SQLDatabaseFabric, AWSDatabaseFabric


class AutoRegisterDatabaseFabric:
    _registry: dict[str, DatabaseFabric] = {}

    def __init_subclass__(cls, **kwargs: Any) -> None:
        super().__init_subclass__(**kwargs)
        AutoRegisterDatabaseFabric._registry[cls.kind] = cls

    @classmethod
    def create(cls, kind: str, **kwargs: Any) -> AutoRegisterDatabaseFabric:
        try:
            return cls._registry[kind](**kwargs)
        except KeyError:
            raise ValueError(f"Unknown kind: {kind}")


class AutoRegisterSQLDatabaseFabric(SQLDatabaseFabric, AutoRegisterDatabaseFabric):
    kind = "sql"


class AutoRegisterAWSDatabaseFabric(AWSDatabaseFabric, AutoRegisterDatabaseFabric):
    kind = "aws"
