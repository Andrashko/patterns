from __future__ import annotations
from typing import Protocol
from math import floor


class IDamageActor(Protocol):
    def take_damage(self, damage: int) -> None:
        ...

    def hit(self, other: IDamageActor) -> None:
        ...

    def is_dead(self) -> bool:
        ...


class Character  (IDamageActor):
    def __init__(self, name: str, health_points: int, attack_damage: int) -> None:
        self.name: str = name
        self.health_points: int = health_points
        self.attack_damage: int = attack_damage

    def take_damage(self, damage: int) -> None:
        self.health_points -= damage
        print(
            f"{self.name} take a hit {damage}. {self.health_points} healthpoits left")
        if self.is_dead():
            self.die()

    def hit(self, other: IDamageActor) -> None:
        other.take_damage(self.attack_damage)

    def is_dead(self) -> bool:
        return self.health_points <= 0

    def die(self) -> None:
        print(f"{self.name}  is dead!")


class CharacterBuff (IDamageActor):
    def __init__(self, damage_actor: IDamageActor) -> None:
        self._damage_actor: IDamageActor = damage_actor

    def take_damage(self, damage: int) -> None:
        self._damage_actor.take_damage(damage)

    def hit(self, other: IDamageActor) -> None:
        self._damage_actor.hit(other)

    def is_dead(self) -> bool:
        return self._damage_actor.is_dead()

    def undecorate(self) -> IDamageActor:
        return self._damage_actor


class DefenceBuff(CharacterBuff):
    def __init__(self, damage_actor: IDamageActor, defence_percent: int) -> None:
        super().__init__(damage_actor)
        self._reduce_damage_coefficient: float = 1 - defence_percent / 100.0

    def take_damage(self, damage: int) -> None:
        reduced_damage: int = floor(damage * self._reduce_damage_coefficient)
        super().take_damage(reduced_damage)
