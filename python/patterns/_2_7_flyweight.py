from dataclasses import dataclass
from typing import Optional, Self
import hashlib

# вихідний стан обєкту


@dataclass
class Car:
    owner: str = ""
    number: str = ""
    company: str = ""
    model: str = ""
    color: str = ""

#   Пристосуванець зберігає спільну частину стану (також відому як внутрішній стан), яка належеть декільком реальним обєктам.
#   Пристосуванець приймє решту стану (зовнішній стан, унікальний для кожного обєкту) через параемтри методу
#   Також пристосуванець  може повернути оригінальний об'єкт

# внутрішній стан


@dataclass
class CarSharedState:
    company: str = ""
    model: str = ""

    def __init__(self, car: Car) -> None:
        self.company = car.company
        self.model = car.model


# //зовнішній стан


@dataclass
class CarUniqueState:
    owner: str = ""
    number: str = ""
    color: str = ""

    def __init__(self, car: Car) -> None:
        self.owner = car.owner
        self.number = car.number
        self.color = car.color


class Flyweight:
    def __init__(self, car: Optional[Car] = None) -> None:
        if car is None:
            return
        self._shared_state: CarSharedState = CarSharedState(car)
        self._unique_state: CarUniqueState = CarUniqueState(car)

    def set_unique_state(self, car_unique_state: CarUniqueState) -> Self:
        self._unique_state = car_unique_state
        return self

    def set_shared_state(self, car_shared_state: CarSharedState) -> Self:
        self._shared_state = car_shared_state
        return self

    def __repr__(self) -> str:
        return f"Flyweight: shared {self._shared_state} and unique {self._unique_state} state"

    def to_object(self) -> Car:
        return Car(
            company=self._shared_state.company,
            model=self._shared_state.model,
            owner=self._unique_state.owner,
            number=self._unique_state.number,
            color=self._unique_state.color
        )
# Також пристосуванець може мати  властивості з гетерами та сетерами для зовнішнього стану
# і властивості з гетерами для внутрішнього стану. Сетери властивостей внутрішнього стану КАТЕГОРИЧНО ЗАБОРОНЕНІ

    @property
    def company(self) -> str:
        return self._shared_state.company

    @property
    def model(self) -> str:
        return self._shared_state.model

    @property
    def owner(self) -> str:
        return self._unique_state.owner

    @owner.setter
    def owner(self, value: str) -> None:
        self._unique_state.owner = value

    @property
    def number(self) -> str:
        return self._unique_state.number

    @number.setter
    def number(self, value: str) -> None:
        self._unique_state.number = value

    @property
    def color(self) -> str:
        return self._unique_state.color

    @color.setter
    def color(self, value: str) -> None:
        self._unique_state.color = value


#     Фабрика пристосуванців створює обєкти та керує ними.
#  Вона забезпечує правильний розподіл внутрішнього стану.
#     При створенні пристосуванця фабрика повертає існуючий спільний стан чи створює новий.
class FlyweightFactoryMethod:
    def __init__(self, *args: Car) -> None:
        self._shared_states: dict[str, CarSharedState] = {}
        for car in args:
            self._shared_states[self.get_key(car)] = CarSharedState(car)

    # Повертає текстовий геш для спільного стану
    def get_key(self, car: Car) -> str:
        raw = f"{car.company.strip().lower()}_{car.model.strip().lower()}"
        return hashlib.md5(raw.encode("utf-8")).hexdigest()

    # Повертає існуючий чи створює новий внутрішній стан пристосуванця
    def to_flyweight(self, car: Car) -> Flyweight:
        key: str = self.get_key(car)
        if not key in self._shared_states:
            self._shared_states[key] = CarSharedState(car)
        return Flyweight()\
            .set_shared_state(self._shared_states[key])\
            .set_unique_state(CarUniqueState(car))

    def print_shared_states(self) -> None:
        print(
            f"FlyweightFactory: I have {len(self._shared_states)} shared states:")
        for state in self._shared_states.values():
            print(state)
