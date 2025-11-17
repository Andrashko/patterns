from __future__ import annotations
from abc import ABC, abstractmethod
from os import name
from typing import Optional


class ChatMediator:
    def __init__(self) -> None:
        self._users: list[ChatUser] = []

    def add_user(self, user: ChatUser) -> None:
        self._users.append(user)

    def send_public_message(self, sender: ChatUser, message: str) -> None:
        for user in self._users:
            if user is not sender:
                user.receive(message, sender)

    def get_user(self, name: str) -> Optional[ChatUser]:
        for user in self._users:
            if user.name == name:
                return user

    def send_private_message(self, sender: AdminUser, target_name: str, message: str) -> None:
        if user := self.get_user(target_name):
            user.receive(f"[PRIVATE] {message}", sender)


class ChatUser(ABC):
    def __init__(self, name: str, mediator: ChatMediator) -> None:
        self.name = name
        self.mediator = mediator

    def send(self, message: str) -> None:
        print(f"{self.name} (send): {message}")
        self.mediator.send_public_message(self, message)

    @abstractmethod
    def receive(self, message: str, sender: ChatUser) -> None: ...


class RegularUser(ChatUser):
    def receive(self, message: str, sender: ChatUser) -> None:
        print(f"{self.name} (receive from {sender.name}): {message}")


class AdminUser(ChatUser):
    def receive(self, message: str, sender: ChatUser) -> None:
        print(f"[ADMIN] {self.name} receive from  {sender.name}: {message}")

    def ban_user(self, target_name: str, reason: str):
        self.mediator.send_public_message(self, f"{target_name} was baned!")
        self.mediator.send_private_message(self, target_name, f"You were baned for {reason}")
