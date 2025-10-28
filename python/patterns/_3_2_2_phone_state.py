from __future__ import annotations
from typing import Protocol


class IPhoneState(Protocol):
    def press_button(self, phone: Phone): ...
    def dial_number(self,  phone: Phone, number: str): ...


class LockedPhoneState:  # IPhoneState
    def press_button(self, phone: Phone):
        phone.show_message("Phone is unlocked")
        phone.set_state(UnlockedPhoneState())

    def dial_number(self,  phone: Phone, number: str):
        phone.show_message("Cannot to Dial. Phone is Locked")


class UnlockedPhoneState:
    def press_button(self, phone: Phone):
        phone.show_message("Phone is locked")
        phone.set_state(LockedPhoneState())

    def dial_number(self,  phone: Phone, number: str):
        phone.show_message(f"Dialing {number}")


class Phone:
    def __init__(self) -> None:
        self._state = LockedPhoneState()

    def set_state(self, state: IPhoneState) -> None:
        self._state = state

    def show_message(self, message: str) -> None:
        print(f"Message: {message}")

    def press_button(self) -> None:
        self._state.press_button(self)

    def dial_number(self, number: str) -> None:
        self._state.dial_number(self, number)
