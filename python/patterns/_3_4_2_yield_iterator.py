from __future__ import annotations
from typing import Iterator,  Sequence, Any
from patterns._2_6_composite import Folder, MyFile


class YieldIterableMixin:
    def __iter__(self) -> Iterator[Any]:
        return _yield_generator(self)


def _yield_generator(node: Any) -> Iterator[Any]:
    yield node
    for child_node in _children_of(node):
        yield from _yield_generator(child_node)


def _children_of(node: Any) -> Sequence[Any]:
    if hasattr(node, "components"):
        return getattr(node, "components")
    return ()


type YieldIterableCompositeComponent = YieldIterableFolder | YieldIterableMyFile


class YieldIterableFolder(YieldIterableMixin, Folder):
    ...


class YieldIterableMyFile(YieldIterableMixin, MyFile):
    ...
