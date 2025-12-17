from datetime import date
from decimal import Decimal
from patterns._3_1_strategy import SortStrategy, ReverseSortStrategy, CapitalizeStrategy, Context
from patterns._3_1_2_payment_strategy import Card, PaymentProcessor, Visa, MasterCard,  Bill, MasterCardPayment, VisaPayment
from patterns._3_2_1_state import SubjectMark
from patterns._3_2_2_phone_state import Phone
from patterns._3_3_1_chain import IHandler, Request, IncHandler, LogHandler, ResponseHandler, RoleHandler, AuthorizeHandler
from patterns._3_3_2_pipline import PipelineAuthorizeHandler, PipelineIncHandler, PipelineLogHandler, PipelineManager, PipelineResponseHandler, PipelineRoleHandler
from patterns._3_4_1_iterator import IterableFolder, IterableMyFile, IterableCompositeComponent
from patterns._3_4_2_yield_iterator import YieldIterableFolder, YieldIterableMyFile, YieldIterableCompositeComponent
from patterns._3_4_3_graph_iterator import Graph, BreadthFirstSearchStrategy, DepthFirstSearchStrategy
from patterns._3_5_1_visitor import IVisitor, Person, PrintVisitor, Student, Professor, SayHiVisitor
from patterns._3_5_2_pythonic_visiter import PrintPythonicVisitor, SayHiPythonicVisitor
from patterns._3_6_1_observer import IObserver, ConsoleLogObserver, EvenObserver, CounterObserver, Subject
from patterns._3_6_2_event import IEventHandler, event_system, ConsoleLogEventHandler, CounterEventHandler, EvenEventHandler, EventSubject
from patterns._3_7_mediator import ChatMediator, RegularUser, AdminUser
from patterns._3_8_memento import Originator, Caretaker
from patterns._3_10_command import SimpleCommand, ComplexCommand, Receiver, Invoker
from patterns._3_10_1_command_memento import CartHistory, ShoppingCart
from patterns._3_9_template import User, FakePostgresConnection, InMemoryDB, InMemoryTransaction, PostgresTransaction


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
        "VISA": VisaPayment(),
    }

    bill: Bill = Bill(Decimal("600"))

    for card in cards:
        if processor.checkout(bill, card):
            print(f"Paid by [card.number]")
        else:
            print(f"Not paid by [card.number]")


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
    pipeline.set_handler(0, PipelineLogHandler())
    pipeline.set_handler(2, PipelineAuthorizeHandler())
    pipeline.set_handler(4, PipelineLogHandler())
    pipeline.set_handler(6, PipelineRoleHandler())
    pipeline.set_handler(8, PipelineResponseHandler())

    pipeline.set_handler(3,  PipelineIncHandler())
    print(pipeline.handle(Request("Noname", "")).value)
    print(pipeline.handle(Request("admin", "admin")).value)


def test_iterator() -> None:
    file: IterableCompositeComponent = IterableMyFile("new", "cs")
    folder: IterableCompositeComponent = IterableFolder("Project")\
        .add(IterableMyFile("Project", "csproj"))\
        .add(IterableMyFile("Program", "cs"))\
        .add(IterableFolder("bin")
             .add(IterableMyFile("Program", "exe"))
             .add(IterableMyFile("config", "json"))
             .add(file)
             )\
        .add(IterableMyFile("", "gitignore"))\
        .add(IterableMyFile("README", "md"))\
        .add(IterableFolder("git"))\
        .add(file)

    for element in folder:
        print(element.to_string(0))


def test_yield_iterator() -> None:
    file: YieldIterableCompositeComponent = YieldIterableMyFile("new", "cs")
    folder: YieldIterableCompositeComponent = YieldIterableFolder("Project")\
        .add(YieldIterableMyFile("Project", "csproj"))\
        .add(YieldIterableMyFile("Program", "cs"))\
        .add(YieldIterableFolder("bin")
             .add(YieldIterableMyFile("Program", "exe"))
             .add(YieldIterableMyFile("config", "json"))
             .add(file)
             )\
        .add(YieldIterableMyFile("", "gitignore"))\
        .add(YieldIterableMyFile("README", "md"))\
        .add(YieldIterableFolder("git"))\
        .add(file)

    for element in folder:
        print(element.to_string(0))


def test_graph_iterator() -> None:
    identity_matrix: list[list[int]] = [
        # https://i.ytimg.com/vi/oDqjPvD54Ss/maxresdefault.jpg
        # 0   1   2   3   4   5   6   7   8   9   10  11 12
        [0,  0,  0,  0,  0,  0,  0,  1,  0,  1,  0,  1,  0],  # 0
        [0,  0,  0,  0,  0,  0,  0,  0,  1,  0,  1,  0,  0],  # 1
        [0,  0,  0,  1,  0,  0,  0,  0,  0,  0,  0,  0,  1],  # 2
        [0,  0,  1,  0,  1,  0,  0,  1,  0,  0,  0,  0,  0],  # 3
        [0,  0,  0,  1,  0,  0,  0,  0,  0,  0,  0,  0,  0],  # 4
        [0,  0,  0,  0,  0,  0,  1,  0,  0,  0,  0,  0,  0],  # 5
        [0,  0,  0,  0,  0,  1,  0,  0,  0,  0,  0,  0,  0],  # 6
        [0,  0,  0,  1,  0,  0,  1,  0,  0,  0,  0,  1,  0],  # 7
        [0,  1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1],  # 8
        [0,  0,  0,  0,  0,  0,  0,  0,  1,  0,  1,  0,  0],  # 9
        [0,  1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0],  # 10
        [0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0],  # 11
        [0,  0,  1,  0,  0,  0,  0,  0,  1,  0,  0,  0,  0],  # 12
    ]
    graph: Graph = Graph(identity_matrix)
    print("Звичайний обхід")
    for node in graph:
        print(node)
    print("обхід в ширину")
    graph.iteration_strategy = BreadthFirstSearchStrategy()
    for node in graph:
        print(node)
    print("обхід в глибину")
    graph.iteration_strategy = DepthFirstSearchStrategy()
    for node in graph:
        print(node)


def test_visitor() -> None:
    persons: list[Person] = [
        Student(name="Iван", surname="Iваненко", course=2),
        Professor(surname="Маляр", name="Микола",
                  secondname="Миколайович", cathedra="Кiбернетики i прикладної математики")
    ]

    visitors: list[IVisitor] = [
        PrintVisitor(),
        SayHiVisitor(),

        PrintPythonicVisitor(),
        SayHiPythonicVisitor()
    ]
    for person in persons:
        for visitor in visitors:
            person.accept(visitor)


def test_observer() -> None:
    subject: Subject = Subject()
    logger: IObserver[int] = ConsoleLogObserver()
    even: IObserver[int] = EvenObserver()
    counter: IObserver[int] = CounterObserver(lambda number: number < 5)
    subject.attach(logger)
    subject.attach(even)
    subject.attach(counter)
    for _ in range(5):
        subject.set_random_state()
    print("Detach even observer")
    subject.detach(even)
    for _ in range(5):
        subject.set_random_state()


def test_event() -> None:
    subject: EventSubject = EventSubject()
    EVENT_NAME: str = "update_value"
    logger: IEventHandler = ConsoleLogEventHandler()
    even: IEventHandler = EvenEventHandler()
    counter: IEventHandler = CounterEventHandler(lambda value: value < 5)
    event_system.subscribe(EVENT_NAME, logger)
    event_system.subscribe(EVENT_NAME, even)
    event_system.subscribe(EVENT_NAME, counter)
    for _ in range(5):
        subject.set_random_state()
    print("Detach even observer")
    event_system.unsubscribe(EVENT_NAME, even)
    for _ in range(5):
        subject.set_random_state()


def test_mediator() -> None:
    mediator = ChatMediator()

    alice = RegularUser("Alice", mediator)
    bob = RegularUser("Bob", mediator)
    admin = AdminUser("Admin", mediator)

    mediator.add_user(alice)
    mediator.add_user(bob)
    mediator.add_user(admin)

    alice.send("Hello world!")
    bob.send("Hello, Alice!")
    admin.ban_user("Bob", "Spam")


def test_memento() -> None:
    originator = Originator("Init state")
    caretaker = Caretaker(originator)

    caretaker.backup()
    originator.change_state()

    caretaker.backup()
    originator.change_state()

    print("\nClient: Now, let's rollback!\n")
    caretaker.undo()

    print("\n\nClient: Once more!\n")
    caretaker.undo()

    print("\n\nClient: Once more!\n")
    caretaker.undo()


def test_command() -> None:
    invoker = Invoker()
    # invoker.on_start = SimpleCommand("start logging")
    invoker.on_finish = ComplexCommand(
        Receiver(), email="test@mail.com", text="test message")
    invoker.business_logic()


def test_memento_command() -> None:
    cart = ShoppingCart()
    history = CartHistory(cart)
    history.add_item(2, "Asus TUF Gaming F15 FX506HM Laptop", 1)
    history.add_item(1, "IPhone 16 pro max 1Gb", 2)
    print(cart)
    history.remove_item(1, 2)
    history.add_item(1, "IPhone 17 pro max 1Gb", 1)
    print(cart)
    history.undo()
    print(cart)
    history.undo()
    print(cart)
    history.undo()
    print(cart)
    history.undo()
    print(cart)
    history.undo()
    print(cart)


def test_template() -> None:
    def create_user_operation_pg(conn: FakePostgresConnection) -> User:
        user: User = {"id": 1, "name": "Yurii"}
        conn.execute("INSERT INTO users(id, name) VALUES (%s, %s)",
                     (user["id"], user["name"]))
        return user

    def create_user_operation_mem(conn: InMemoryDB) -> User:
        user: User = {"id": 1, "name": "Yurii"}
        conn.set("user:1", user)
        return user

    pg_tx: PostgresTransaction[User] = PostgresTransaction()
    user_pg = pg_tx.run_in_transaction(create_user_operation_pg)
    print("PG user:", user_pg)

    mem_db = InMemoryDB()
    mem_tx: InMemoryTransaction[User] = InMemoryTransaction(mem_db)
    user_mem = mem_tx.run_in_transaction(create_user_operation_mem)
    print("InMemory user:", user_mem)
    print("InMemory data:", mem_db.get_all())
