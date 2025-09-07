const { RealSubject, proxyRequestHandler } = require("./2_1_proxy");

const REPEAT_COUNT = 5;
const realSubject = new RealSubject("192.168.0.1");
console.log("Use real subject");
const response = realSubject.Request();
console.log(response);
realSubject.Request = new Proxy(realSubject.Request, proxyRequestHandler);
console.log(`Use proxy ${REPEAT_COUNT} times`);
for (let i = 1; i < REPEAT_COUNT; i++) {
    const response = realSubject.Request();
    console.log(response);
}
