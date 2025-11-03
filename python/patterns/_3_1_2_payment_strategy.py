from __future__ import annotations
from datetime import date
from decimal import Decimal
from typing import Protocol
from dataclasses import dataclass, field


@dataclass
class Card:
    number: str
    system: str
    expires: date
    balance: Decimal = Decimal("0")


@dataclass
class MasterCard (Card):
    system: str = field(init=False, default="MASTER")

@dataclass
class Visa (Card):
    system: str = field(init=False, default="VISA")


@dataclass
class Bill:
    sum: Decimal


class IPayment(Protocol):
    def pay(self, pay_sum: Decimal, credit_card: Card) -> bool: ...


class PaymentProcessor:
    def __init__(self) -> None:
        self.strategies: dict[str, IPayment] = {}
    

    def checkout(self, bill: Bill, card: Card) -> bool:
        if not card.system in self.strategies:
            return False
        return self.strategies[card.system].pay(bill.sum, card)


class VisaPayment:  # (IPayment)
    def pay(self, pay_sum: Decimal, credit_card: Card) -> bool:
        if credit_card.system != "VISA":
            print("Wrong System")
            return False

        if date.today() > credit_card.expires:
            print("Expired")
            return False

        if pay_sum > credit_card.balance:
            print("Not enough money")
            return False

        credit_card.balance -= pay_sum
        print("Payment by Visa")
        return True


class MasterCardPayment:  # (IPayment)
    def pay(self, pay_sum: Decimal, credit_card: Card) -> bool:
        if credit_card.system != "MASTER":
            print("Wrong System")
            return False

        if date.today() > credit_card.expires:
            print("Expired")
            return False

        if pay_sum > credit_card.balance:
            print("Not enough money")
            return False

        credit_card.balance -= pay_sum
        print("Payment by MasterCard")
        return True
