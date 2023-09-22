const { SongSmallWidget, SongMiddleWidget, SongBigWidget, BookSmallWidget, BookMiddleWidget, BookBigWidget, Book, Song } = require("./2_5_1_no_bridge");
const { SmallWidgetAbstraction, MiddleWidgetAbstraction, BigWidgetAbstraction, SongWidgetData, BookWidgetData } = require("./2_5_2_bridge");

const song = new Song("Вставай!",
    "Вставай! Пий чай з молоком, Молися на теплий душ!");
const book = new Book("Шаблони проєктування: Елементи повторно використовуваного об'єктно-орієнтованого програмного забезпечення",
    "Книга 1994 року з програмної інженерії, в якій запропоновані і описані архітектурні рішення деяких частих проблем у проєктуванні ПЗ");

function testNoBridge() {
    const widgets = [
        new BookSmallWidget(book),
        new BookMiddleWidget(book),
        new BookBigWidget(book),
        new SongSmallWidget(song),
        new SongMiddleWidget(song),
        new SongBigWidget(song)
    ];
    for (let widget of widgets)
        console.log(widget.render());
}

function testBridge() {
    const widgets = [
        new SmallWidgetAbstraction(),
        new MiddleWidgetAbstraction(),
        new BigWidgetAbstraction()
    ];
    const widgetData = [
        new SongWidgetData(song),
        new BookWidgetData(book)
    ];
    for (let widget of widgets)
        for (let data of widgetData)
            console.log(widget.render(data));
}
testNoBridge();
testBridge();