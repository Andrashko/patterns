from dataclasses import dataclass


@dataclass
class Book:
    title: str
    abstracts: str
    cover: str = "http://localhos/no_image.jpg"


@dataclass
class Song:
    name: str
    text: str
    album_cover: str = "http://localhos/no_image.jpg"


class Widget:
    def __init__(self, title: str, description: str, img_url: str) -> None:
        self.title: str = title
        self.description: str = description
        self.img_url: str = img_url

    def render(self) -> str:
        return self.template

    @property
    def template(self) -> str:
        return f'<div class="widget"></div>'

    def cut_string(self, string: str, length: int) -> str:
        if len(string) <= length:
            return string
        return f"{string[0:length-3]}..."


class BookWidget (Widget):
    def __init__(self, book: Book) -> None:
        super().__init__(book.title, book.abstracts, book.cover)


class BookSmallWidget(BookWidget):
    @property
    def template(self) -> str:
        return f"""
        <div class="small-widget">
            <h5>{self.cut_string(self.title, 10)}</h5>
        </div>"""


class BookMiddleWidget (BookWidget):
    @property
    def template(self) -> str:
        return f"""
        <div class="middle-widget">
            <h3>{self.title}</h3>
            <p>{self.cut_string(self.description, 20)}</p>
            <img src="{self.img_url}">
        </div>"""


class BookBigWidget (BookWidget):
    @property
    def template(self) -> str:
        return f"""
        <div class="big-widget">
            <h2>{self.title}</h2>
            <p>{self.description}</p>
            <img src="{self.img_url}">
        </div>"""


class SongWidget (Widget):
    def __init__(self, song: Song) -> None:
        super().__init__(song.name, song.text, song.album_cover)


class SongSmallWidget(SongWidget):
    @property
    def template(self) -> str:
        return f"""
        <div class="small-widget">
            <h5>{self.cut_string(self.title, 10)}</h5>
        </div>"""


class SongMiddleWidget (SongWidget):
    @property
    def template(self) -> str:
        return f"""
        <div class="middle-widget">
            <h3>{self.title}</h3>
            <p>{self.cut_string(self.description, 20)}</p>
            <img src="{self.img_url}">
        </div>"""


class SongBigWidget (SongWidget):
    @property
    def template(self) -> str:
        return f"""
        <div class="big-widget">
            <h2>{self.title}</h2>
            <p>{self.description}</p>
            <img src="{self.img_url}">
        </div>"""
