from __future__ import annotations
from typing import Iterator,  Sequence, Any
from patterns._2_6_composite import Folder, MyFile


class YieldIterableMixin:
    def __iter__(self) -> Iterator[Any]:
        # return _yield_generator(self)
        yield self
        for child_node in self._children_of(self):
            yield from child_node

    def _children_of(self, node: Any) -> Sequence[Any]:
        if hasattr(node, "components"):
            return getattr(node, "components")
        return ()


type YieldIterableCompositeComponent = YieldIterableFolder | YieldIterableMyFile


class YieldIterableFolder(YieldIterableMixin, Folder):
    ...


class YieldIterableMyFile(YieldIterableMixin, MyFile):
    ...
