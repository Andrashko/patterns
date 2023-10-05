class RealSubject {
    constructor(ip) {
        this.ip = ip;
    }

    Request() {
        return `Real subject response from ${this.ip}`;
    }
}

function CheckAccess() {
    return Math.random() < 0.5;
}

function LogAccess(message) {
    console.log(`Request was handle by proxy: ${message}`);
}

const proxyRequestHandler = {
    apply: function (target, thisArg, argumentsList) {
        if (CheckAccess()) {
            let response = target.apply(thisArg, argumentsList);
            LogAccess(response);
            return response;
        }
        LogAccess("Connection has failed");
        return "Proxy response";
    },
}

module.exports = {
    RealSubject,
    proxyRequestHandler
}