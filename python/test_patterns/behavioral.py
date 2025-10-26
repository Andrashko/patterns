from datetime import date
from decimal import Decimal
from patterns._3_1_strategy import SortStrategy, ReverseSortStrategy, CapitalizeStrategy, Context
from patterns._3_1_2_payment_strategy import Card, PaymentProcessor, Visa, MasterCard,  Bill, MasterCardPayment, VisaPayment


def test_strategy() -> None:
    context: Context = Context(SortStrategy())
    context.do_some_business_logic()
    context.set_strategy(ReverseSortStrategy())
    context.do_some_business_logic()
    context.set_strategy(CapitalizeStrategy())
    context.do_some_business_logic()


def test_payment_strategy() -> None:
    cards: list[Card] = [
        Visa("1234 5678 9012 3456", date(
            2032, 10, 1), Decimal("-1000")),
        MasterCard("2234 5678 9012 3477", date(
            2021, 1, 1), Decimal("5000")),
        MasterCard("3234 5678 9012 3000", date(
            2034, 12, 31), Decimal("500")),
        Visa("4234 5678 9012 3456", date(
            2033, 10, 1), Decimal("10000")),
    ]
    processor: PaymentProcessor = PaymentProcessor()
    processor.strategies = {
        "MASTER": MasterCardPayment(),
        "VISA": VisaPayment()
    }

    bill: Bill = Bill(Decimal("600"))

    for card in cards:
        if processor.checkout(bill, card):
            print(f"Paid by {card.number}")
        else:
            print(f"Not paid by {card.number}")
