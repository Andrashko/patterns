const { Human, IterableCollection } = require("./3_4_iterator");


let collection = new IterableCollection([
    new Human("Yurii", 32),
    new Human("Andrii", 42),
    new Human("Tetiana", 18),
    new Human("Olekandr", 62)
]);

console.log("Straight traversal:");

for (let element of collection) {
    console.log(element);
}

console.log("\nReverse traversal:");

collection.reverseDirection();

for (let element of collection) {
    console.log(element);
}