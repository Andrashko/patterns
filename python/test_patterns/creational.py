from os import write
from patterns._1_1_1_classic_singleton import ClassicSingleton
from patterns._1_1_2_pythonic_singleton import pythonic_singleton, PythonicSingleton
from patterns._1_1_3_metaclass_singleton import MetaclassSingleton
from patterns._1_1_4_logger_singleton import LoggerSingleton
from patterns._1_2_1_classic_fabric_method import Application, PDFApp, WordApp
from patterns._1_2_2_new_fabric_method import application_fabric_method
from patterns._1_2_3_auto_register_fabric_method import AutoRegisterApplicationFabricMethod


def test_classic_singleton() -> None:
    instance1: ClassicSingleton = ClassicSingleton()
    instance1.inc_counter()
    instance2: ClassicSingleton = ClassicSingleton()
    instance1.print_self()
    instance2.print_self()
    instance1.inc_counter()
    instance2.inc_counter()
    instance1.print_self()
    instance2.print_self()


def test_pythonic_singleton() -> None:
    instance1: PythonicSingleton = pythonic_singleton
    instance1.inc_counter()
    instance2: PythonicSingleton = pythonic_singleton
    instance1.print_self()
    instance2.print_self()
    instance1.inc_counter()
    instance2.inc_counter()
    instance1.print_self()
    instance2.print_self()


def test_metaclass_singleton() -> None:
    instance1: MetaclassSingleton = MetaclassSingleton()
    instance1.inc_counter()
    instance2: MetaclassSingleton = MetaclassSingleton()
    instance1.print_self()
    instance2.print_self()
    instance1.inc_counter()
    instance2.inc_counter()
    instance1.print_self()
    instance2.print_self()


def test_logger_singleton() -> None:
    logger1: LoggerSingleton = LoggerSingleton("log.txt")
    logger1.log("message to logger 1")
    logger2: LoggerSingleton = LoggerSingleton("other_log.txt")
    logger2.log("message to logger 2")
    logger1.log("other message to logger 1")
    logger1.show_log()
    logger2.show_log()

# спільна для декількох  тестів


def test_document(app: Application) -> None:
    app.open_document()


def test_classic_fabric_method() -> None:
    app: Application
    choice: str = input("Choose word or pdf:")
    if choice == "word":
        app = WordApp()
    elif choice == "pdf":
        app = PDFApp()
    else:
        print("Error")
        return
    test_document(app)


def test_new_fabric_method() -> None:

    choice: str = input("Choose word or pdf:")
    app: Application = application_fabric_method(choice)
    test_document(app)


def test_auto_register_fabric_method() -> None:
    choice: str = input("Choose word or pdf:")
    app: Application = AutoRegisterApplicationFabricMethod.create(choice)
    test_document(app)
