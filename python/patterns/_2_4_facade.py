from dataclasses import dataclass
from typing import TypeVar, Callable, Optional
import xml.etree.ElementTree as ET

T = TypeVar("T")


def find_unique(items: list[T], predicate: Callable[[T], bool]) -> Optional[T]:
    """
    Шукає єдиний об'єкт у списку, який задовольняє предикат.

    :param items: список об'єктів
    :param predicate: функція T -> bool
    :return: знайдений об'єкт або None, якщо не знайдено
    """
    for item in items:
        if predicate(item):
            return item
    return None


@dataclass
class ArtItem:
    title: str
    id: int


class FetchMusic:
    _resources: list[ArtItem] = [
        ArtItem(id=1, title="Californication"),
        ArtItem(id=2, title="It's my life"),
        ArtItem(id=3, title="Sweet dreams")
    ]

    @classmethod
    def fetch(cls, id: int) -> Optional[ArtItem]:
        return find_unique(cls._resources, lambda item: item.id == id)


class GetMovie:
    def __init__(self, id: int) -> None:
        self._id = id
        self._resources: list[ArtItem] = [
            ArtItem(id=1, title="Apocalypse Now"),
            ArtItem(id=2, title="Terminator"),
            ArtItem(id=3, title="Titanic")
        ]

    @property
    def value(self) -> Optional[ArtItem]:
        return find_unique(self._resources, lambda item: item.id == self._id)


class TvShowResource:
    @classmethod
    def get(cls) -> list[ArtItem]:
        return [
            ArtItem(id=1, title="Twin Peaks"),
            ArtItem(id=2, title="Supernatural"),
            ArtItem(id=3, title="Dexter")
        ]


class ArtFacade:
    def _get_book_by_id(self, id: int) -> Optional[ArtItem]:
        tree = ET.parse("Books.xml")
        root = tree.getroot()
        for book in root.findall(".//Book"):
            book_id: int = int(book.findtext("Id", default="-1"))
            if book_id == id:
                return ArtItem(
                    id=book_id,
                    title=book.findtext("Title", default="")
                )
        return None

    def get_item(self, type: str, id: int) -> Optional[ArtItem]:
        if type == "music":
            return FetchMusic.fetch(id)
        elif type == "movie":
            return GetMovie(id).value
        elif type == "tvshow":
            return find_unique(TvShowResource.get(), lambda item: item.id == id)
        elif type == "book":
            return self._get_book_by_id(id)
        else:
            raise ValueError("Unknown type")
