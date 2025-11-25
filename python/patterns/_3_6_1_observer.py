from __future__ import annotations
from typing import Protocol, Callable
from random import randint


class IObserver [T](Protocol):
    def update(self, new_value: T) -> None: ...


class IObservable [T](Protocol):
    def attach(self, observer: IObserver[T]) -> None: ...
    def detach(self, observer: IObserver[T]) -> None: ...
    def notify(self) -> None: ...


class Subject:  # (IObservable[int])
    def __init__(self) -> None:
        self._state: int = 0
        self._observers: list[IObserver[int]] = []

    @property
    def state(self) -> int:
        return self._state

    @state.setter
    def state(self, value: int) -> None:
        self._state = value
        self.notify()

    def set_random_state(self) -> None:
        self.state = randint(0, 10)

    def attach(self, observer: IObserver[int]) -> None:
        self._observers.append(observer)

    def detach(self, observer: IObserver[int]) -> None:
        if observer in self._observers:
            self._observers.remove(observer)

    def notify(self) -> None:
        for observer in self._observers:
            observer.update(self._state)


class ConsoleLogObserver:  # IObserver[int]
    def update(self, new_value: int) -> None:
        print(f"New value: {new_value}")


class EvenObserver:  # IObserver[int]
    def update(self, new_value: int) -> None:
        if new_value % 2 == 0:
            print(f"New even value: {new_value}")


type Predicate = Callable[[int], bool]


class CounterObserver:  # IObserver[int]
    def __init__(self, condition: Predicate) -> None:
        self.count: int = 0
        self.condition: Predicate = condition

    def update(self, new_value: int) -> None:
        if self.condition(new_value):
            self.count += 1
            print(f"satisfying the condition {self.count} times")
