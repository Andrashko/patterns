from __future__ import annotations
from typing import Protocol
from datetime import datetime


class BuildProduct:
    def __init__(self):
        self._parts: list[str] = []
        self.name: str = "No name"

    def __repr__(self) -> str:
        return f"Product {self.name}, parts:\n {','.join(self._parts)}"

    def add_part(self, part: str) -> None:
        self._parts.append(part)


class Builder(Protocol):
    def add_part(self, part: str) -> Builder:
        ...

    def set_stamp(self) -> Builder:
        ...

    def set_name(self, name: str) -> Builder:
        ...

    def get_product(self) -> BuildProduct:
        ...

    def reset(self) -> None:
        ...


class SomeBuilder(Builder):
    def __init__(self) -> None:
        super().__init__()
        self._product: BuildProduct = BuildProduct()

    def add_part(self, part: str) -> Builder:
        self._product.add_part(part)
        return self

    def set_stamp(self) -> Builder:
        self._product.add_part(f"Date stamp: {datetime.now()}")
        return self

    def set_name(self, name: str) -> Builder:
        self._product.name = name
        return self

    def get_product(self) -> BuildProduct:
        result: BuildProduct = self._product
        self.reset()
        return result

    def reset(self) -> None:
        self._product = BuildProduct()


class OtherBuilder(SomeBuilder):
    def set_stamp(self) -> Builder:
        self._product.add_part(f"<{datetime.utcnow()}>")
        return self


class Director:
    def __init__(self, builder: Builder) -> None:
        self._builder: Builder = builder
        self._builder.reset()

    def build_empty_product(self) -> BuildProduct:
        return self._builder.get_product()

    def build_from_parts(self, parts: list[str]) -> BuildProduct:
        for part in parts:
            self._builder.add_part(part)
        return self._builder.get_product()

    def build_example(self) -> BuildProduct:
        return self._builder\
            .set_name("example")\
            .add_part("part one")\
            .add_part("part two")\
            .set_stamp()\
            .add_part("part three")\
            .get_product()
