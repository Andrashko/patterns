from __future__ import annotations
from io import TextIOWrapper
from datetime import datetime
from typing import TypeVar, Callable, Dict, Any

T = TypeVar("T")


def singleton(cls: type[T]) -> Callable[..., T]:
    instances: Dict[type, T] = {}

    def get_instance(*args: Any, **kwargs: Any) -> T:
        if cls not in instances:
            instances[cls] = cls(*args, **kwargs)
        return instances[cls]

    return get_instance


@singleton
class LoggerSingletonDecorator:
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
