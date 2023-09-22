const singleton = {
    count: 0,
    randomNumber: Math.random(),
    print: function () {
        console.log(`My random number is ${this.randomNumber}. Count is ${this.count}`);
    },
    incCount: function () {
        this.count++;
    }
}

module.exports =  singleton ;