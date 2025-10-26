from typing import Optional
import gc
import psutil
import os
from flask import Flask
from patterns._2_1_proxy import IRequest, RequestManager, RequestManagerProxy
from patterns._2_2_1_game_decorator import AttackBuff, IDamageActor, Character,  DefensiveBuff
from patterns._2_2_2_pythonic_proxy_decorator import RequestManagerPythonicProxy
from patterns._2_2_3_singleton_decorator import LoggerSingletonDecorator
from patterns._2_3_2_winapi_adapter import WinApiAdapter, PROCESS_INFORMATION
from patterns._2_3_1_adapter import Adaptee, Adapter
from patterns._2_4_facade import ArtItem, ArtFacade
from patterns._2_5_1_no_bridge import Widget, Book, BookSmallWidget, BookMiddleWidget, BookBigWidget, Song, SongBigWidget, SongMiddleWidget, SongSmallWidget
from patterns._2_5_2_bridge import BigWidgetAbstraction, MiddleWidgetAbstraction, SmallWidgetAbstraction, SongWidgetData, BookWidgetData, WidgetAbstraction, WidgetDataRealisation
from patterns._2_6_composite import CompositeComponent, MyFile, Folder
from patterns._2_7_flyweight import Car, Flyweight, FlyweightFactoryMethod


def test_bridge() -> None:
    book: Book = Book(
        title="Шаблони проєктування: Елементи повторно використовуваного об'єктно-орієнтованого програмного забезпечення",
        abstracts="Книга 1994 року з програмної інженерії, в якій запропоновані і описані архітектурні рішення деяких частих проблем у проєктуванні ПЗ",
    )
    song: Song = Song(
        name="Вставай!",
        text="Вставай! Пий чай з молоком, Молися на теплий душ!"
    )
    widgets: list[Widget] = [
        BookSmallWidget(book),
        BookMiddleWidget(book),
        BookBigWidget(book),
        SongSmallWidget(song),
        SongMiddleWidget(song),
        SongBigWidget(song)
    ]

    bridge_widgets: list[WidgetAbstraction] = [
        SmallWidgetAbstraction(),
        MiddleWidgetAbstraction(),
        BigWidgetAbstraction(),
    ]
    widget_data: list[WidgetDataRealisation] = [
        BookWidgetData(book),
        SongWidgetData(song)
    ]

    app: Flask = Flask("test", static_folder="./python/static")

    @app.get("/")
    def home() -> str:
        return f"""
        <html>
            <head>
                 <link rel="stylesheet" href="static/style.css">
            </head>
            <body>
                <h2>No Bridge</h2>
                {
            "\n".join([
                widget.render() for widget in widgets
            ])
        }
                <h2>with Bridge</h2>
                {
            "\n".join([widget.render(data)
                      for data in widget_data for widget in bridge_widgets ])
        }
            <body>
        </html>    
        """
    app.run()


def test_proxy() -> None:
    request_manager: IRequest = RequestManager("8.8.8.8")
    print(request_manager.request())
    request_manager = RequestManagerProxy(request_manager)
    print(request_manager.request())
    print(request_manager.request())
    print(request_manager.request())


def test_game_decorator() -> None:
    def battle(first: IDamageActor, second: IDamageActor) -> None:
        while not (first.is_dead() or second.is_dead()):
            first.hit(second)
            second.hit(first)

    human: IDamageActor = Character("Human", 300, 50)
    orc: IDamageActor = Character("Orc", 350, 75)
    battle(human, orc)

    print("Rematch")
    human = Character("Human", 300, 50)
    orc = Character("Orc", 350, 75)
    buffed_human: IDamageActor = AttackBuff(DefensiveBuff(human, 25), 50)
    battle(buffed_human, orc)


def test_proxy_decorator() -> None:
    request_manager = RequestManagerPythonicProxy("8.8.8.8")
    print(request_manager.request())
    print(request_manager.request())
    print(request_manager.request())


def test_logger_singleton_decorator() -> None:
    logger1 = LoggerSingletonDecorator("log.txt")
    logger1.log("message to logger 1")
    logger2 = LoggerSingletonDecorator("other.txt")
    logger2.log("message to logger 2")
    logger1.log("other message to logger 1")
    logger1.show_log()
    logger2.show_log()


def test_winapi_adapter() -> None:
    process: str = "notepad.exe"
    adapter = WinApiAdapter()
    pi: Optional[PROCESS_INFORMATION] = adapter.create_process(process)
    if pi:
        print(f"{process} was lunched with id  {pi.dwProcessId}")
    adapter.create_process("calc.exe")


def test_adapter() -> None:
    old_lib: Adaptee = Adaptee()
    print(old_lib.get_response("Test", 4, True))
    new_lib: Adapter = Adapter(old_lib)
    print(new_lib.get_response("Test"))


def test_facade() -> None:
    TYPES: list[str] = ["music", "movie", "tvshow", "book"]
    facade: ArtFacade = ArtFacade()
    for item_type in TYPES:
        item: Optional[ArtItem] = facade.get_item(item_type, 1)
        if item is None:
            raise ValueError(f"{item_type} with id {id} not found")
        print(f"{item_type} : {item.title}")


def test_composite() -> None:
    file: CompositeComponent = MyFile("new", "cs")
    folder: CompositeComponent = Folder("Project")
    folder\
        .add(MyFile("Project", "csproj"))\
        .add(MyFile("Program", "cs"))\
        .add(Folder("bin")
             .add(MyFile("Program", "exe"))
             .add(MyFile("config", "json"))
             .add(file)
             )\
        .add(MyFile("", "gitignore"))\
        .add(MyFile("README", "md"))\
        .add(Folder("git"))\
        .add(file)

    print(folder.to_string(0))
    folder.remove(file)
    print(f"File {file} was removed")
    print(folder.to_string(0))
    folder.sort()
    print("order by name")
    print(folder.to_string(0))


def test_flyweight() -> None:
    factory: FlyweightFactoryMethod = FlyweightFactoryMethod(
        Car(company="Chevrolet", model="Camaro"),
        Car(company="Mercedes Benz", model="C300"),
        Car(company="BMW", model="M5"),
        Car(company="BMW", model="X6")
    )
    factory.print_shared_states()
    cars: list[Flyweight] = []

    def add_car_to_list(factory: FlyweightFactoryMethod, car: Car) -> None:
        cars.append(factory.to_flyweight(car))

    add_car_to_list(factory,  Car
                    (
                        number="CL234IR",
                        owner="Jon Snow",
                        company="BMW",
                        model="M5",
                        color="Red"
                    ))

    add_car_to_list(factory,  Car
                    (
                        number="CL234IR",
                        owner="James Doe",
                        company="Skoda",
                        model="Octavia",
                        color="Red"
                    ))

    add_car_to_list(factory,  Car
                    (
                        number="CR123IR",
                        owner="Jon Doe",
                        company="Skoda",
                        model="Octavia",
                        color="Black"
                    ))
    factory.print_shared_states()
    print("List of cars:")
    for car in cars:
        print(car.to_object())


def test_flyweight_memory_usage() -> None:
    def memory_usage_mb() -> float:
        process = psutil.Process(os.getpid())
        return process.memory_info().rss / 1024**2

    def fresh_str(string: str) -> str:
        return string.encode().decode()

    print(f"No data structure use {memory_usage_mb()} Mb ")
    COUNT: int = 1_000_000
    cars: list[Car] = []
    factory = FlyweightFactoryMethod()
    car_flyweights: list[Flyweight] = []
    for i in range(COUNT):
        cars.append(
            Car(
                number=f"AB{i}CD",
                owner="Jon Doe",
                company="Skoda",
                model="Fabia",
                color="Black"
            )
        )
    print(f"list of {COUNT} cars use {memory_usage_mb()} Mb ")
    cars = []
    gc.collect()
    print(f"after clear use {memory_usage_mb()} Mb ")
    for i in range(COUNT):
        car_flyweights.append(
            factory.to_flyweight(
                Car(
                    number=f"AB{i}CD",
                    owner="Jon Doe",
                    company="Skoda",
                    model="Fabia",
                    color="Black"
                )
            )
        )
    print(
        f"list of {COUNT} cars flyweights use {memory_usage_mb()} Mb ")
    car_flyweights = []
    gc.collect()
    print(f"after clear use {memory_usage_mb()} Mb ")

    TIMES: int = 50
    company: str = "Skoda"*TIMES
    model: str = "Fabia"*TIMES
    for i in range(COUNT):
        cars.append(
            Car(
                number=f"AB{i}CD",
                owner="Jon Doe",
                company=company,
                model=model,
                color="Black"
            )
        )
    print(f"list of {COUNT} cars use {memory_usage_mb()} Mb ")
    cars = []
    gc.collect()
    print(f"after clear use {memory_usage_mb()} Mb ")
    for i in range(COUNT):
        car_flyweights.append(
            factory.to_flyweight(
                Car(
                    number=f"AB{i}CD",
                    owner="Jon Doe",
                    company=company,
                    model=model,
                    color="Black"
                )
            )
        )
    print(
        f"list of {COUNT} cars flyweights use {memory_usage_mb()} Mb ")
    car_flyweights = []
