from __future__ import annotations
from typing import Any, ClassVar

from patterns._1_3_1_fabric import DatabaseFabric, SQLDatabaseFabric, AWSDatabaseFabric


class AutoRegisterDatabaseFabric(DatabaseFabric):
    _registry: dict[str, type[DatabaseFabric]] = {}
    kind: ClassVar[str]

    def __init_subclass__(cls, **kwargs: Any) -> None:
        super().__init_subclass__(**kwargs)
        AutoRegisterDatabaseFabric._registry[cls.kind] = cls

    @classmethod
    def create(cls, kind: str, **kwargs: Any) -> DatabaseFabric:
        try:
            app_cls: type[DatabaseFabric] = cls._registry[kind]
            return app_cls(**kwargs)

        except KeyError:
            raise ValueError(f"Unknown kind: {kind}")


class AutoRegisterSQLDatabaseFabric(SQLDatabaseFabric, AutoRegisterDatabaseFabric):
    kind = "sql"


class AutoRegisterAWSDatabaseFabric(AWSDatabaseFabric, AutoRegisterDatabaseFabric):
    kind = "aws"
