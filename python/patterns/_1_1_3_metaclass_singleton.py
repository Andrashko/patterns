from __future__ import annotations
from typing import Any, TypeVar
from random import randint

T = TypeVar("T")


class SingletonMeta(type):
    _instances: dict[type[Any], Any] = {}

    def __call__(cls: type[T], *args: Any, **kwargs: Any) -> T:
        if cls not in SingletonMeta._instances:
            SingletonMeta._instances[cls] = super().__call__(*args, **kwargs)
        return SingletonMeta._instances[cls]


class MetaclassSingleton (metaclass=SingletonMeta):
    def __init__(self) -> None:
        # ініціалізація бізнес логіки
        self._random_number: int = randint(0, 100)
        self._counter: int = 0
    # бізнес логіка

    def print_self(self) -> None:
        print(
            f"My random number =  {self._random_number} \n Counter = {self._counter}")

    def inc_counter(self) -> None:
        self._counter += 1
