from __future__ import annotations
from typing import Iterator, List, Sequence, Any, Self
from patterns._2_6_composite import Folder, MyFile

class IterableMixin:
    def __iter__(self) -> Iterator[Any]:
        return _Iterator(self)


class _Iterator:
    def __init__(self, root: Any) -> None:
        self._stack: List[Any] = [root]

    def __iter__(self) -> Self:
        return self

    def __next__(self) -> Any:
        if not self._stack:
            raise StopIteration
        node = self._stack.pop()
        children = reversed(self._children_of(node))
        for ch in children:
            self._stack.append(ch)
        return node

    def _children_of(self, node: Any) -> Sequence[Any]:
        if hasattr(node, "components"):
            return getattr(node, "components")
        return ()


type IterableCompositeComponent = IterableFolder | IterableMyFile


class IterableFolder(IterableMixin, Folder):
    ...


class IterableMyFile(IterableMixin, MyFile):
    ...
