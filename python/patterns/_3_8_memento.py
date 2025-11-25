from __future__ import annotations
from typing import Protocol
from random import choice
from string import ascii_letters
from datetime import datetime


class IRestorable (Protocol):
    def save(self) -> IMemento: ...
    def restore(self, memento: IMemento): ...


class Originator:  # (IRestorable)
    def __init__(self, state: str) -> None:
        self._state: str = state
        print(f"Originator: My initial state is: {state}")

    def _generate_random_string(self, length: int) -> str:
        return "".join(choice(ascii_letters) for _ in range(length))

    def change_state(self) -> None:
        print("Originator: I'm doing something important.")
        self._state = self._generate_random_string(30)
        print(f"Originator: and my state has changed to: {self._state}")

    def save(self) -> IMemento:
        return ConcreteMemento(self._state)

    def restore(self, memento: IMemento):
        self._state = memento.state
        print(f"Originator: My state has changed to: {self._state}")


class IMemento(Protocol):
    @property
    def name(self) -> str: ...
    @property
    def date(self) -> datetime: ...
    @property
    def state(self) -> str: ...


class ConcreteMemento:  # (IMemento)
    _MAX_LENGTH: int = 10

    def __init__(self, state: str) -> None:
        super().__init__()
        self._state: str = state
        self._date: datetime = datetime.now()

    @property
    def name(self) -> str:
        if len(self._state) > self._MAX_LENGTH:
            return f"{self._date} / ({self._state[0:self._MAX_LENGTH]})..."
        return f"{self._date} / ({self._state})"

    @property
    def date(self) -> datetime:
        return self._date

    @property
    def state(self) -> str:
        return self._state


class Caretaker:
    def __init__(self, originator: Originator) -> None:
        self._originator: Originator = originator
        self._mementos: list[IMemento] = []

    def backup(self) -> None:
        self._mementos.append(self._originator.save())
        print("Caretaker: Saving Originator's state...")

    def undo(self) -> None:
        if not self._mementos:
            print("Caretaker: There is no history to undo")
            return

        memento = self._mementos.pop()
        self._originator.restore(memento)
        print(f"Caretaker: Restoring state to: {memento.name}")
