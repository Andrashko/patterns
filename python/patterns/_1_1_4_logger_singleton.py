from __future__ import annotations
from io import TextIOWrapper
from datetime import datetime
from typing import Any


class SingletonMeta(type):
    _instances: dict[type, Any] = {}

    def __call__(cls, *args: Any, **kwargs: Any) -> Any:
        if cls not in cls._instances:
            cls._instances[cls] = super().__call__(*args, **kwargs)
        return cls._instances[cls]


class LoggerSingleton (metaclass=SingletonMeta):
    def __init__(self, file_name: str) -> None:
        self._file_name: str = file_name
        self._file: TextIOWrapper = open(
            self._file_name, "r+", encoding="utf-8")

    def __del__(self) -> None:
        self._file.close()

    def log(self, message: str) -> None:
        self._file.write(f"{datetime.now()} : {message}\n")
        self._file.flush()

    def show_log(self) -> None:
        self._file.seek(0)
        for line in self._file.readlines():
            print(line)
        
