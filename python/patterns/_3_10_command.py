from typing import Protocol, Any, Optional


class ICommand (Protocol):
    def execute(self, *args: Any, **kwargs: Any) -> None: ...


class SimpleCommand:  # ICommand
    def __init__(self, payload: str) -> None:
        self._payload: str = payload

    def execute(self) -> None:
        print(f"Simple command with: {self._payload}")


class ComplexCommand:  # ICommand
    def __init__(self, receiver: ICommand, *args: Any, **kwargs: Any) -> None:
        self._receiver = receiver
        self._args = args
        self._kwargs = kwargs

    def execute(self) -> None:
        print(f"Complex command. mplex stuff should be done by a receiver object.")
        self._receiver.execute(*self._args, **self._kwargs)


class Invoker:
    def __init__(self) -> None:
        self._on_start: Optional[ICommand] = None
        self._on_finish: Optional[ICommand] = None

    @property
    def on_start(self) -> Optional[ICommand]:
        return self._on_start

    @on_start.setter
    def on_start(self, value: Optional[ICommand]) -> None:
        self._on_start = value

    @property
    def on_finish(self) -> Optional[ICommand]:
        return self._on_finish

    @on_finish.setter
    def on_finish(self, value: Optional[ICommand]) -> None:
        self._on_finish = value

    def business_logic(self) -> None:
        print("Invoker: before start")
        if self._on_start is not None:
            self._on_start.execute()

        print("Invoker: ...doing something really important...")

        print("Invoker:  after  finish")
        if self._on_finish is not None:
            self._on_finish.execute()


class Receiver:
    def execute(self, email: str, text: str) -> None:
        print(f"send {text} to {email}")
