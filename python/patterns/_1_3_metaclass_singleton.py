from __future__ import annotations
from typing import Any
from random import randint


class SingletonMeta(type):
    _instances: dict[type, Any] = {}

    def __call__(cls, *args: Any, **kwargs: Any) -> Any:
        if cls not in cls._instances:
            cls._instances[cls] = super().__call__(*args, **kwargs)
        return cls._instances[cls]


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
