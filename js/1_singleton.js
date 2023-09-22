const singleton = Symbol("singleton");
const singletonEnforcer = Symbol("singletonEnforcer");

class Singleton {

    constructor(enforcer) {
        //захист від прямого виклику конструктора 
        if (enforcer !== singletonEnforcer)
            throw "Instantiation failed: use Singleton.getInstance() instead of new.";
        //ініціалізація
        this.randomNumber = Math.random();
        this.counter = 0;
    }

    static get _instance() {
        if (!this[singleton])
            this[singleton] = new Singleton(singletonEnforcer);
        return this[singleton];
    }

    static set _instance(value) { throw "Instance is readonly property!" }

    static getInstance() { return this._instance; }

    //методи, що реалізують бізнес логіку
    print() {
        console.log(`My random number = ${this.randomNumber}`);
    }

    incCounter() {
        console.log(`Counter =${++this.counter}`);
    }

}

module.exports = { Singleton }