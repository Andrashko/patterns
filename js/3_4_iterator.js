class Human {
    constructor(name, age) {
        this.name = name;
        this.age = age;
    }
}

class IterableCollection {
    constructor(array, reverse = false) {
        this.array = array;
        this.reverse = reverse;
        this.reset();
    }

    reset() {
        if (this.reverse)
            this.position = this.array.length - 1;
        else
            this.position = 0;
    }

    getChange() {
        if (this.reverse)
            return -1;
        return 1;
    }
    reverseDirection() {
        this.reverse = !this.reverse;
        this.reset();
    }

    [Symbol.iterator]() {
        return this;
    }

    next() {
        while (this.position < this.array.length && this.position >= 0) {
            let result = {
                value: this.array[this.position],
                done: false
            }
            this.position += this.getChange();
            return result;
        }
        this.reset();
        return {
            done: true
        }
    }
}

module.exports = { Human, IterableCollection }