from __future__ import annotations
from typing import Protocol, Any


class SupportsRichComparison(Protocol):
    def __lt__(self, other: Any, /) -> bool: ...
    def __le__(self, other: Any, /) -> bool: ...
    def __gt__(self, other: Any, /) -> bool: ...
    def __ge__(self, other: Any, /) -> bool: ...


class Context:
    def __init__(self, strategy: IStrategy[str]) -> None:
        self._strategy: IStrategy[str] = strategy

    def set_strategy(self, strategy: IStrategy[str]) -> None:
        self._strategy = strategy

    def do_some_business_logic(self) -> None:
        print("Context: Change data using the strategy")
        data: list[str] = ["a",  "c", "d", "b", "e",]
        result: list[str] = self._strategy.do_algorithm(data)
        print(f"Result: {','.join(result)}")


class IStrategy[T: SupportsRichComparison](Protocol):
    def do_algorithm(self, data: list[T]) -> list[T]: ...


class SortStrategy[T: SupportsRichComparison]:  # (IStrategy)
    def do_algorithm(self, data: list[T]) -> list[T]:
        return sorted(data)


class ReverseSortStrategy[T: SupportsRichComparison]:  # (IStrategy)
    def do_algorithm(self, data: list[T]) -> list[T]:
        return sorted(data, reverse=True)


class CapitalizeStrategy:  # (IStrategy)
    def do_algorithm(self, data: list[str]) -> list[str]:
        return [word.upper() for word in data]
