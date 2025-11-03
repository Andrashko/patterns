from __future__ import annotations
from typing import Protocol, overload, Any, Callable, Type, Optional
from dataclasses import dataclass
from functools import singledispatchmethod


class IVisitable (Protocol):
    def accept(self, visitor: IVisitor) -> None: ...


class IVisitor (Protocol):
    def visit(self, target: Student | Professor) -> None: ...


@dataclass
class Person:  # (IVisitable)
    name: str
    surname: str

    def accept(self, visitor: IVisitor) -> None: ...


@dataclass
class Student (Person):
    course: int

    def accept(self, visitor: IVisitor) -> None:
        visitor.visit(self)


@dataclass
class Professor (Person):
    secondname: str
    cathedra: str

    def accept(self, visitor: IVisitor) -> None:
        visitor.visit(self)


class AbstractVisitor:
    @overload
    def visit(self, target: Student) -> None: ...
    @overload
    def visit(self, target: Professor) -> None: ...
    @overload
    def visit(self, target: object) -> None: ...

    @singledispatchmethod
    def visit(self, target: object) -> None:
        raise TypeError(f"Unsupported type: {type(target).__name__}")

    # --- декоратор-«маркер» для підкласів ---
    @staticmethod
    def register(tp: Type[Any]) -> Callable[[Callable[..., Any]], Callable[..., Any]]:
        """
        Позначає метод підкласу як обробник для типу `tp`.
        Реєстрацію виконаємо пізніше, у __init_subclass__.
        """
        def deco(fn: Callable[..., Any]) -> Callable[..., Any]:
            setattr(fn, "__visit_type__", tp)
            return fn
        return deco

    def __init_subclass__(cls) -> None:
        """
        Для КОЖНОГО підкласу:
        1) створюємо НОВИЙ singledispatchmethod `visit`;
        2) знаходимо в його словнику методи, позначені @AbstractVisitor.register(T);
        3) реєструємо їх у підкласовому диспетчері.
        """
        # 1) свій диспетчер для підкласу

        def _default(self, target: object) -> None:
            raise TypeError(f"Unsupported type: {type(target).__name__}")

        cls.visit = singledispatchmethod(_default)  # type: ignore[assignment]

        # 2) знайти помічені методи
        for name, obj in cls.__dict__.items():
            tp: Optional[Type[Any]] = getattr(
                obj, "__visit_type__", None)  # type: ignore[arg-type]
            if tp is not None and callable(obj):
                # 3) зареєструвати у підкласовому диспетчері
                cls.visit.register(tp)(obj)  # type: ignore[misc]

        super().__init_subclass__()


# ---- Підклас 1: свій набір гілок ----
class PrintVisitor(AbstractVisitor):
    @AbstractVisitor.register(Student)
    def visit_student(self, target: Student) -> None:
        print("Друкую студента")
        print(f"Курс {target.course}, {target.name} {target.surname}")

    @AbstractVisitor.register(Professor)
    def visit_professor(self, target: Professor) -> None:
        print("Друкую професора")
        print(f"{target.surname} {target.name} {target.secondname}, {target.cathedra}")


# ---- Підклас 2: інший набір гілок (НЕ перезатирає перший) ----
class SayHiVisitor(AbstractVisitor):
    @AbstractVisitor.register(Student)
    def hi_student(self, target: Student) -> None:
        print(f"Привіт, {target.name}!")

    @AbstractVisitor.register(Professor)
    def hi_professor(self, target: Professor) -> None:
        print(f"Доброго дня, {target.name} {target.secondname}.")
