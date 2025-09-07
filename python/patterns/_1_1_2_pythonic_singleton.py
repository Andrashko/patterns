from __future__ import annotations
from random import randint


class PythonicSingleton:
    def __init__(self, value: object = None) -> None:
        # ініціалізація бізнес логіки
        self._random_number: int = randint(0, 100)
        self._counter: int = 0
    # бізнес логіка

    def print_self(self) -> None:
        print(
            f"My random number =  {self._random_number} \n Counter = {self._counter}")

    def inc_counter(self) -> None:
        self._counter += 1

#тут відбується єдина ініцалізація, тому нема проблем з багатопотоковістю
pythonic_singleton = PythonicSingleton()