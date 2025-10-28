from datetime import date
from decimal import Decimal
from patterns._3_1_strategy import SortStrategy, ReverseSortStrategy, CapitalizeStrategy, Context
from patterns._3_1_2_payment_strategy import Card, PaymentProcessor, Visa, MasterCard,  Bill, MasterCardPayment, VisaPayment
from patterns._3_2_1_state import SubjectMark
from patterns._3_2_2_phone_state import Phone
from patterns._3_3_1_chain import IHandler, Request, IncHandler, LogHandler, ResponseHandler, RoleHandler, AuthorizeHandler
from patterns._3_3_2_pipline import PipelineAuthorizeHandler, PipelineIncHandler, PipelineLogHandler, PipelineManager, PipelineResponseHandler, PipelineRoleHandler


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


def test_state() -> None:
    subjects: list[SubjectMark] = [
        SubjectMark("Design patterns", 95),
        SubjectMark("Algebra", 42),
        SubjectMark("History", 0)
    ]
    for subject in subjects:
        subject.pass_exam()
    subjects[2].rating = 50
    subjects[2].pass_exam()


def test_phone_state() -> None:
    phone: Phone = Phone()
    phone.dial_number("911")
    phone.press_button()
    phone.dial_number("911")
    phone.press_button()


def test_chain_of_responsibility() -> None:
    chain: IHandler = LogHandler()\
        .set_next(AuthorizeHandler()) \
        .set_next(RoleHandler()) \
        .set_next(IncHandler())\
        .set_next(LogHandler())\
        .set_next(ResponseHandler())
    print(chain.handle(Request("Noname", "")).value)
    print(chain.handle(Request("admin", "admin", "admin")).value)


def test_pipeline() -> None:
    pipeline = PipelineManager(8)
    pipeline.set_handler(0,  PipelineLogHandler())
    pipeline.set_handler(2, PipelineAuthorizeHandler())
    pipeline.set_handler(4, PipelineLogHandler())
    pipeline.set_handler(6, PipelineResponseHandler())

    pipeline.set_handler(3,  PipelineIncHandler())
    print(pipeline.handle(Request("Noname", "")).value)
    print(pipeline.handle(Request("admin", "admin")).value)
