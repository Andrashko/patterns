const { Book, Song } = require("./2_5_1_no_bridge");

class WidgetAbstraction {

    render(widgetData) {
        this.title = widgetData.title;
        this.description = widgetData.description;
        return this.temlate;
    }

    get temlate() {
        return `widget`;
    }

    cutString(str, len) {
        if (str.length <= len)
            return str;
        return `${str.substring(0, len - 3)}...`;
    }
}

class SmallWidgetAbstraction extends WidgetAbstraction {
    get temlate() {
        return `<h5 class="small-widget">${this.cutString(this.title, 10)}</h5>`;
    }
}

class MiddleWidgetAbstraction extends WidgetAbstraction {
    get temlate() {
        return `<div class="middle-widget"><h3>${this.title}</h3><p>${this.cutString(this.description, 20)}</p></div>`;
    }
}

class BigWidgetAbstraction extends WidgetAbstraction {
    get temlate() {
        return `<div class="big-widget"><h2>${this.title}</h2><p>${this.description}</p></div>`;
    }
}

class WidgetDataRealisation {
    get title() {
        return `title`;
    }

    get description() {
        return `description`;
    }
}

class SongWidgetData extends WidgetDataRealisation {
    constructor(song) {
        super();
        this.song = song;
    }
    get title() {
        return this.song.name;
    }
    get description() {
        return this.song.text;
    }
}

class BookWidgetData extends WidgetDataRealisation {
    constructor(book) {
        super();
        this.book = book;
    }
    get title() {
        return this.book.title;
    }
    get description() {
        return this.book.abstracts;
    }
}

module.exports = { SmallWidgetAbstraction, MiddleWidgetAbstraction, BigWidgetAbstraction, SongWidgetData, BookWidgetData };