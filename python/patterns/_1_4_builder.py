from __future__ import annotations
from typing import Protocol, Self
from datetime import datetime, timezone


class BuildProduct:
    def __init__(self):
        self._parts: list[str] = []
        self.name: str = "No name"

    def __repr__(self) -> str:
        return f"Product {self.name}, parts:\n {','.join(self._parts)}"

    def add_part(self, part: str) -> None:
        self._parts.append(part)


class Builder(Protocol):
    def add_part(self, part: str) -> Self: ...
    def set_stamp(self) -> Self: ...
    def set_name(self, name: str) -> Self: ...
    def get_product(self) -> BuildProduct: ...
    def reset(self) -> None: ...


class SomeBuilder:#(Builder):
    def __init__(self) -> None:
        super().__init__()
        self._product: BuildProduct = BuildProduct()

    def add_part(self, part: str) -> Self:
        self._product.add_part(part)
        return self

    def set_stamp(self) -> Self:
        self._product.add_part(f"Date stamp: {datetime.now()}")
        return self

    def set_name(self, name: str) -> Self:
        self._product.name = name
        return self

    def get_product(self) -> BuildProduct:
        result: BuildProduct = self._product
        self.reset()
        return result

    def reset(self) -> None:
        self._product = BuildProduct()


class OtherBuilder(SomeBuilder):
    def set_stamp(self) -> Self:
        self._product.add_part(f"<{datetime.now(timezone.utc)}>")
        return self


class Director:
    def __init__(self, builder: Builder) -> None:
        self.builder: Builder = builder
        self.builder.reset()

    def build_empty_product(self) -> BuildProduct:
        self.builder.reset()
        return self.builder.get_product()

    def build_from_parts(self, parts: list[str]) -> BuildProduct:
        for part in parts:
            self.builder.add_part(part)
        return self.builder.get_product()

    def build_example(self) -> BuildProduct:
        return self.builder\
            .set_name("example")\
            .add_part("part one")\
            .add_part("part two")\
            .set_stamp()\
            .add_part("part three")\
            .get_product()
