from __future__ import annotations
from typing import Any
from random import randint


class ClassicSingleton:
    _instance: ClassicSingleton | None = None

    def __new__(cls, *args: Any, **kwargs: Any) -> ClassicSingleton:
        if cls._instance is None:
            cls._instance = super().__new__(cls)
        return cls._instance

    def __init__(self) -> None:
        if not hasattr(self, "_initialized"):
            self._initialized = True
            # ініціалізація бізнес логіки
            self._random_number: int = randint(0, 100)
            self._counter: int = 0
    # бізнес логіка

    def print_self(self) -> None:
        print(
            f"My random number =  {self._random_number} \n Counter = {self._counter}")

    def inc_counter(self) -> None:
        self._counter += 1
