from os import write
from tkinter import NO
from patterns._1_1_1_classic_singleton import ClassicSingleton
from patterns._1_1_2_pythonic_singleton import pythonic_singleton, PythonicSingleton
from patterns._1_1_3_metaclass_singleton import MetaclassSingleton
from patterns._1_1_4_logger_singleton import LoggerSingleton
from patterns._1_2_1_classic_fabric_method import Application, PDFApp, WordApp
from patterns._1_2_2_new_fabric_method import application_fabric_method
from patterns._1_2_3_auto_register_fabric_method import AutoRegisterApplicationFabricMethod
from patterns._1_4_builder import Director, SomeBuilder, OtherBuilder
from patterns._1_5_prototype import ObjectToCopy, InnerObjectToCopy
from copy import copy, deepcopy
from time import sleep


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


def test_builder() -> None:
    director: Director = Director(SomeBuilder())
    print(director.build_empty_product())
    print(director.build_example())
    parts: list[str] = ["one", "two"]
    print(director.build_from_parts(parts))
    director._builder = OtherBuilder()
    print(director.build_example())


def test_prototype() -> None:
    print("====Assign===")
    original_object: ObjectToCopy = ObjectToCopy()

    bad_copy: ObjectToCopy = original_object
    bad_copy.id = -1
    print(original_object, bad_copy)

    print("====shallow copy===")
    original_object = ObjectToCopy()
    shallow_copy: ObjectToCopy = copy(original_object)
    shallow_copy.id = -1
    shallow_copy.inner_object.name = "new name"
    print(original_object, shallow_copy)

    print("====Deep copy===")
    original_object = ObjectToCopy()
    sleep(1)
    shallow_copy: ObjectToCopy = deepcopy(original_object)
    shallow_copy.id = -1
    shallow_copy.inner_object.name = "new name"
    print(original_object, shallow_copy)
