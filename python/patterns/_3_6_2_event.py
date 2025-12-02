
from typing import Protocol, Any
from patterns._3_6_1_observer import Predicate
from random import randint


class IEventHandler(Protocol):
    def handle(self, *args: Any, **kwargs: Any) -> None: ...


class EventSystem:
    def __init__(self):
        self._subscribers: dict[str, list[IEventHandler]] = {}

    def subscribe(self, event_name: str, handler: IEventHandler) -> None:
        if not self._subscribers.get(event_name):
            self._subscribers[event_name] = []
        self._subscribers[event_name].append(handler)

    def unsubscribe(self, event_name: str, handler: IEventHandler) -> None:
        handlers = self._subscribers.get(event_name, [])
        if handler in handlers:
            handlers.remove(handler)

    def publish(self, event_name: str, *args: Any, **kwargs: Any) -> None:
        for handler in self._subscribers.get(event_name, []):
            handler.handle(*args, **kwargs)


event_system: EventSystem = EventSystem()


class ConsoleLogEventHandler:  # IEventHandler
    def handle(self, new_value: int) -> None:
        print(f"New value: {new_value}")


class EvenEventHandler:  # IEventHandler
    def handle(self, new_value: int) -> None:
        if new_value % 2 == 0:
            print(f"New even value: {new_value}")


class CounterEventHandler:  # IEventHandler
    def __init__(self, condition: Predicate) -> None:
        self.count: int = 0
        self.condition: Predicate = condition

    def handle(self, new_value: int) -> None:
        if self.condition(new_value):
            self.count += 1
            print(f"satisfying the condition {self.count} times")


class EventSubject:  # (IObservable)
    def __init__(self) -> None:
        self._state: int = 0

    @property
    def state(self) -> int:
        return self._state

    @state.setter
    def state(self, value: int) -> None:
        self._state = value
        event_system.publish("update_value", value)
        if value == 0:
            event_system.publish("zero_value", self)

    def set_random_state(self) -> None:
        self.state = randint(0, 10)
