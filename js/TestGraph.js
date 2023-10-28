const { Graph, BreadthFirstSearch, DepthFirstSearch } = require("./3_4_2_graph_iterator");

const identityMatrix =
    // https://i.ytimg.com/vi/oDqjPvD54Ss/maxresdefault.jpg
    //   0  1  2  3  4  5  6  7  8  9  10 11 12    
    [
        [0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0], //0
        [0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0], //1
        [0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1], //2
        [0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0], //3
        [0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0], //4
        [0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0], //5
        [0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0], //6
        [0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0], //7
        [0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1], //8
        [0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0], //9
        [0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0], //10
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0], //11
        [0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0], //12
    ];
let graph = new Graph(identityMatrix);

graph.iteratorStrategy = new DepthFirstSearch();

for (let node of graph)
    console.log(node);