const s1 = require("./1_2_module_singleton");
const s2 = require("./1_2_module_singleton");
console.log(s1);
s1.print();
s1.incCount();
s2.incCount()
s1.print();
s2.print();