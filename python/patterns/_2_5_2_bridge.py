from __future__ import annotations
from patterns._2_5_1_no_bridge import Book, Song


class WidgetAbstraction:
    def __init__(self) -> None:
        self.title: str = ""
        self.description: str = ""
        self.url: str = ""

    def render(self, widget_data: WidgetDataRealisation) -> str:
        self.title = widget_data.title
        self.description = widget_data.description
        self.url = widget_data.url
        return self.template

    @property
    def template(self) -> str:
        return f'<div class="widget"></div>'

    def cut_string(self, string: str, length: int) -> str:
        if len(string) <= length:
            return string
        return f"{string[0:length-3]}..."


class SmallWidgetAbstraction (WidgetAbstraction):
    @property
    def template(self) -> str:
        return f"""
        <div class="small-widget">
            <h5>{self.cut_string(self.title, 10)}</h5>
        </div>"""


class MiddleWidgetAbstraction (WidgetAbstraction):
    @property
    def template(self) -> str:
        return f"""
        <div class="middle-widget">
            <h3>{self.title}</h3>
            <p>{self.cut_string(self.description, 20)}</p>
            <img src="{self.url}">
        </div>"""


class BigWidgetAbstraction (WidgetAbstraction):
    @property
    def template(self) -> str:
        return f"""
        <div class="big-widget">
            <h2>{self.title}</h2>
            <p>{self.description}</p>
            <img src="{self.url}">
        </div>"""


class WidgetDataRealisation:
    @property
    def title(self) -> str:
        return "title"

    @property
    def description(self) -> str:
        return "description"

    @property
    def url(self) -> str:
        return "http://localhost"


class BookWidgetData (WidgetDataRealisation):
    def __init__(self, book: Book) -> None:
        super().__init__()
        self._book:  Book = book

    @property
    def title(self) -> str:
        return self._book.title

    @property
    def description(self) -> str:
        return self._book.abstracts

    @property
    def url(self) -> str:
        return self._book.cover


class SongWidgetData (WidgetDataRealisation):
    def __init__(self, song: Song) -> None:
        super().__init__()
        self._song: Song = song

    @property
    def title(self) -> str:
        return self._song.name

    @property
    def description(self) -> str:
        return self._song.text

    @property
    def url(self) -> str:
        return self._song.album_cover
