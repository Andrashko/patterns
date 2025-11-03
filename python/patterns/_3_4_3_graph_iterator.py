from __future__ import annotations
from typing import Generator, Iterator, Optional, Self


class Node:
    def __init__(self, label: str = "") -> None:
        self.label: str = label
        self.income_edges: list[Edge] = []
        self.outcome_edges: list[Edge] = []

    def __str__(self) -> str:
        return f"Node {self.label}"


class Edge:
    def __init__(self, from_node: Node, to_node: Node, value: int = 0) -> None:
        self.from_node: Node = from_node
        self.to_node: Node = to_node
        self.value: int = value

    def __str__(self) -> str:
        return f"Edge ({self.from_node})-({self.to_node})"


class Graph:
    def __init__(self, identity_matrix: list[list[int]]) -> None:
        self._iteration_strategy: GraphIteratorStrategy = GraphIteratorStrategy(
            self)

        self.nodes: list[Node] = []
        for i in range(len(identity_matrix)):
            self.nodes.append(Node(str(i)))
        for row, row_elements in enumerate(identity_matrix):
            for col, element in enumerate(row_elements):
                if element > 0:
                    edge: Edge = Edge(
                        from_node=self.nodes[row],
                        to_node=self.nodes[col],
                        value=element
                    )
                    self.nodes[row].outcome_edges.append(edge)
                    self.nodes[col].income_edges.append(edge)

    @property
    def iteration_strategy(self) -> GraphIteratorStrategy:
        return self._iteration_strategy

    @iteration_strategy.setter
    def iteration_strategy(self, value: GraphIteratorStrategy) -> None:
        self._iteration_strategy = value
        self._iteration_strategy.graph = self

    def __iter__(self) -> Iterator[Node]:
        return iter(self._iteration_strategy)


class GraphIteratorStrategy:
    def __init__(self, graph: Optional[Graph] = None) -> None:
        self.graph: Optional[Graph] = graph

    def __iter__(self) -> Iterator[Node]:
        return self.order()

    def order(self) -> Generator[Node]:
        if self.graph is None:
            return
        for node in self.graph.nodes:
            yield node


class BreadthFirstSearchStrategy (GraphIteratorStrategy):
    def __init__(self, graph: Optional[Graph] = None, start_node: Optional[Node] = None) -> None:
        super().__init__(graph)
        self._nodes_to_visit_queque: list[Node] = []
        self._visited_nodes: list[Node] = []
        self._start_node: Optional[Node] = start_node

    def reset(self) -> None:
        self._nodes_to_visit_queque.clear()
        self._visited_nodes.clear()

    def order(self) -> Generator[Node]:
        if self.graph is None:
            return
        if self._start_node is None:
            self._start_node = self.graph.nodes[0]
        self._nodes_to_visit_queque.append(self._start_node)
        while self._nodes_to_visit_queque:
            node: Node = self._nodes_to_visit_queque.pop(0)
            if node in self._visited_nodes:
                continue
            self._visited_nodes.append(node)
            for edge in node.outcome_edges:
                self._nodes_to_visit_queque.append(edge.to_node)
            yield node
        self.reset()


class DepthFirstSearchStrategy (GraphIteratorStrategy):
    def __init__(self, graph: Optional[Graph] = None, start_node: Optional[Node] = None) -> None:
        super().__init__(graph)
        self._nodes_to_visit_stack: list[Node] = []
        self._visited_nodes: list[Node] = []
        self._start_node: Optional[Node] = start_node

    def reset(self) -> None:
        self._nodes_to_visit_stack.clear()
        self._visited_nodes.clear()

    def order(self) -> Generator[Node]:
        if self.graph is None:
            return
        if self._start_node is None:
            self._start_node = self.graph.nodes[0]
        self._nodes_to_visit_stack.append(self._start_node)
        while self._nodes_to_visit_stack:
            node: Node = self._nodes_to_visit_stack.pop()
            if node in self._visited_nodes:
                continue
            self._visited_nodes.append(node)
            for edge in node.outcome_edges:
                self._nodes_to_visit_stack.append(edge.to_node)
            yield node
        self.reset()
