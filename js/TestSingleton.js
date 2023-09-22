const { Singleton } = require("./1_singleton");

const firstCopy = Singleton.getInstance();
const secondCopy = Singleton.getInstance();
firstCopy.print();
firstCopy.incCounter();
secondCopy.print();
secondCopy.incCounter();
firstCopy.incCounter();
console.log(firstCopy === secondCopy);
try {
    const constructorCallError = new Singleton();
} catch (e) {
    console.log(e);
}
try {
    Singleton._instance = {};
} catch (e) {
    console.log(e);
}
