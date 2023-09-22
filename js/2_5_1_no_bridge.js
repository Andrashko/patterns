class Book {
    constructor(title, abstracts) {
        this.title = title;
        this.abstracts = abstracts;
    }
}

class Song {
    constructor(name, text) {
        this.name = name;
        this.text = text;
    }
}

class Widget {
    constructor(title, description) {
        this.title = title;
        this.description = description;
    }

    render() {
        return this.template;
    }

    get template() {
        return `widget`;
    }

    cutString(str, len) {
        if (str.length <= len)
            return str;
        return `${str.substring(0, len - 3)}...`;
    }
}

class BookWidget extends Widget {
    constructor(book) {
        super(book.title, book.abstracts);
    }
}

class BookSmallWidget extends BookWidget {
    get template() {
        return `<h5 class="small-widget">${this.cutString(this.title, 10)}</h5>`;
    }
}

class BookMiddleWidget extends BookWidget {
    get template() {
        return `<div class="middle-widget"><h3>${this.title}</h3><p>${this.cutString(this.description, 20)}</p></div>`;
    }
}

class BookBigWidget extends BookWidget {
    get template() {
        return `<div class="big-widget"><h2>${this.title}</h2><p>${this.description}</p></div>`;
    }
}

class SongWidget extends Widget {
    constructor(song) {
        super(song.name, song.text);
    }
}

class SongSmallWidget extends SongWidget {
    get template() {
        return `<h5 class="small-widget">${this.cutString(this.title, 10)}</h5>`;
    }
}

class SongMiddleWidget extends SongWidget {
    get template() {
        return `<div class="middle-widget"><h3>${this.title}</h3><p>${this.cutString(this.description, 20)}</p></div>`;
    }
}

class SongBigWidget extends SongWidget {
    get template() {
        return `<div class="big-widget"><h2>${this.title}</h2><p>${this.description}</p></div>`;
    }
}


module.exports = { SongSmallWidget, SongMiddleWidget, SongBigWidget, BookSmallWidget, BookMiddleWidget, BookBigWidget, Book, Song, BookWidget, SongWidget };