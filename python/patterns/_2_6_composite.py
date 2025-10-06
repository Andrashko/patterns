from __future__ import annotations
from typing import Self


class CompositeComponent:
    def __init__(self, name: str) -> None:
        self.name: str = name

    def add(self, component: CompositeComponent) -> Self:
        raise NotImplemented

    def remove(self, component: CompositeComponent) -> Self:
        raise NotImplemented

    @property
    def is_composite(self) -> bool:
        return False

    def to_string(self, level: int = 0) -> str:
        return f"{'...'*level} {self}\n"

    def __eq__(self, value: object) -> bool:
        return (type(value) is CompositeComponent) and (self.name == value.name)

    def sort(self) -> Self:
        return self


class MyFile (CompositeComponent):
    def __init__(self, name: str, ext: str) -> None:
        super().__init__(name)
        self.ext: str = ext

    def __str__(self) -> str:
        return f"{self.name}.{self.ext}"

    def __eq__(self, value: object) -> bool:
        return (type(value) is MyFile) and (self.name == value.name) and (self.ext == value.ext)


class Folder(CompositeComponent):
    def __init__(self, name: str) -> None:
        super().__init__(name)
        self.components: list[CompositeComponent] = []

    def add(self, component: CompositeComponent) -> Self:
        self.components.append(component)
        return self

    def remove(self, component: CompositeComponent) -> Self:
        for temp_component in self.components:
            if temp_component == component:
                self.components.remove(temp_component)
                continue
            if temp_component.is_composite:
                temp_component.remove(component)

        return self

    @property
    def is_composite(self) -> bool:
        return True

    def __str__(self) -> str:
        return self.name

    def to_string(self, level: int = 0) -> str:
        result: str = f"{'...'*level} > {self.name} \n"
        for temp_component in self.components:
            result += temp_component.to_string(level+1)
        return result

    def sort(self) -> Self:
        for temp_component in self.components:
            if temp_component.is_composite:
                temp_component.sort()
        self.components = sorted(
            self.components, key=lambda component: component.name.lower())
        return self
