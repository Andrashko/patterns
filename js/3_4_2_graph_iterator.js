class Node {
    constructor(label, incomeEdges = [], outcomeEdges = []) {
        this.label = label;
        this.incomeEdges = incomeEdges;
        this.outcomeEdges = outcomeEdges;
    }
}

class Edge {
    constructor(from, to, value) {
        this.from = from;
        this.to = to;
        this.value = value;
    }
}

class Graph {
    constructor(identityMatrix) {
        this.iteratorStrategy = new GraphIteratorStrategy();
        this.nodes = [];
        const count = identityMatrix.length;
        for (let row = 0; row < count; row++) {
            let node = new Node(
                row.toString()
            );
            this.nodes.push(node);
        }
        for (let row = 0; row < count; row++)
            for (let col = 0; col < count; col++)
                if (identityMatrix[row][col] > 0) {
                    let edge = new Edge(
                        this.nodes[row],
                        this.nodes[col],
                        identityMatrix[row][col]
                    );
                    this.nodes[row].outcomeEdges.push(edge);
                    this.nodes[col].incomeEdges.push(edge);
                }
    }
    get iteratorStrategy() {
        return this._iteratorStrategy;
    }
    set iteratorStrategy(value) {
        this._iteratorStrategy = value;
        this._iteratorStrategy.graph = this;
    }

    [Symbol.iterator]() {
        return this.iteratorStrategy.iterator();
    }
}

class GraphIteratorStrategy {
    constructor(graph) {
        this.graph = graph;
    }

    *iterator() {
        for (let node of this.graph.nodes)
            yield node;
    }
}

class BreadthFirstSearch {
    constructor(graph, startNode) {
        this.graph = graph;
        this.startNode = startNode;
        this.reset();
    }
    reset() {
        this.nodesToVisit = [];
        this.visitedNodes = [];
    }
    *iterator() {
        this.reset();
        if (this.startNode == null)
            this.startNode = this.graph.nodes[0];
        this.nodesToVisit.push(this.startNode);
        while (this.nodesToVisit.length > 0) {
            let node = this.nodesToVisit.shift();
            if (this.visitedNodes.includes(node))
                //якщо вершину вже відвідували раніше, то її не потрібно відвідувати ще раз. Запобігає циклічним блуканням
                //але в деяких алгоритмах можливо потрібно буде yield node
                continue;
            this.visitedNodes.push(node);
            for (let edge of node.outcomeEdges)
                this.nodesToVisit.push(edge.to);
            yield node;
        }
    }
}

class DepthFirstSearch {
    constructor(graph, startNode) {
        this.graph = graph;
        this.startNode = startNode;
        this.reset();
    }
    reset() {
        this.nodesToVisit = [];
        this.visitedNodes = [];
    }
    *iterator() {
        this.reset();
        if (this.startNode == null)
            this.startNode = this.graph.nodes[0];
        this.nodesToVisit.push(this.startNode);
        while (this.nodesToVisit.length > 0) {
            let node = this.nodesToVisit.pop();
            if (this.visitedNodes.includes(node))
                //якщо вершину вже відвідували раніше, то її не потрібно відвідувати ще раз. Запобігає циклічним блуканням
                //але в деяких алгоритмах можливо потрібно буде yield node
                continue;
            this.visitedNodes.push(node);
            for (let edge of node.outcomeEdges)
                this.nodesToVisit.push(edge.to);
            yield node;
        }
    }
}


module.exports = { Graph, GraphIteratorStrategy, BreadthFirstSearch, DepthFirstSearch }