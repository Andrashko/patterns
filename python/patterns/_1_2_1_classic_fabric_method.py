from typing import Protocol


class Document(Protocol):
    def print_info(self) -> None:
        ...


class PDFDocument (Document):
    def print_info(self) -> None:
        print("This is a PDF document.")


class WordDocument (Document):
    def print_info(self) -> None:
        print("This is a Word document.")


# Фабричні методи
class Application(Protocol):
    def create_document(self) -> Document:
        ...


class PDFApp (Application):
    def create_document(self) -> Document:
        return PDFDocument()

    def open_document(self) -> None:
        doc = self.create_document()
        doc.print_info()


class WordApp (Application):
    def create_document(self) -> Document:
        return WordDocument()

    def open_document(self) -> None:
        doc = self.create_document()
        doc.print_info()
