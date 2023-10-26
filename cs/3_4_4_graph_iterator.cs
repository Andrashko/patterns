using System;
using System.Collections;
using System.Collections.Generic;

namespace Behavioral.Iterator
{
    /// <summary>
    /// Вершина графу 
    /// </summary>
    class Node
    {
        /// <summary>
        /// Мітка вершини
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Список вхідних дуг
        /// </summary>
        public List<Edge> IncomeEdges = new List<Edge>();
        /// <summary>
        /// Список вихідних дуг
        /// </summary>
        public List<Edge> OutcomeEdges = new List<Edge>();

        public override string ToString()
        {
            return $"Node {Label}";
        }
    }

    /// <summary>
    /// дуга графу
    /// </summary>
    class Edge
    {
        /// <summary>
        /// початок дуги
        /// </summary>
        public Node From;
        /// <summary>
        /// кінець дуги
        /// </summary>
        public Node To;
        /// <summary>
        /// значення дуги
        /// </summary>
        public int Value;

        public override string ToString()
        {
            return $"Edge ({From})-({To})";
        }
    }
    /// <summary>
    /// Граф
    /// </summary>
    class Graph : IEnumerable<Node>
    {   /// <summary>
        /// стратегія обходу графу, приватне поле
        /// </summary>
        private GraphIteratorStrategy searchStrategy;
        /// <summary>
        /// публічне поле для визначення обходу графу
        /// /// </summary>
        public GraphIteratorStrategy SearchStrategy
        {
            get { return searchStrategy; }
            set
            {
                searchStrategy = value;
                // стратегія обходу буде працювати із цим графом
                searchStrategy.Graph = this;
            }
        }
        /// <summary>
        /// Вершини графу
        /// </summary>
        public List<Node> Nodes = new List<Node>();
        /// <summary>
        /// Конструктор графу з інцидентності
        /// </summary>
        /// <param name="IdentityMatrix"> квадратна матриця, додатнє значення IdentityMatrix[row, col] > 0
        /// визначає дугу із вершини row у вершину col
        /// </param>
        public Graph(int[,] IdentityMatrix)
        {

            int Count = IdentityMatrix.GetLength(0);
            for (int row = 0; row < Count; row++)
            {
                var node = new Node()
                {
                    Label = row.ToString()
                };
                Nodes.Add(node);
            }
            for (int row = 0; row < Count; row++)
                for (int col = 0; col < Count; col++)
                    if (IdentityMatrix[row, col] > 0)
                    {
                        var edge = new Edge()
                        {
                            From = Nodes[row],
                            To = Nodes[col],
                            Value = IdentityMatrix[row, col]
                        };
                        Nodes[row].OutcomeEdges.Add(edge);
                        Nodes[col].IncomeEdges.Add(edge);
                    }
        }
        /// <summary>
        /// Повертає ітераторі із відповідної стратегії
        /// </summary>
        /// <returns>IEnumerator<Node></returns>
        public IEnumerator<Node> GetEnumerator()
        {
            return SearchStrategy.GetEnumerator();
        }
        /// <summary>
        /// заглушка для сумысності з інтерфейсом
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return SearchStrategy.GetEnumerator();
        }
    }
    /// <summary>
    /// Стратегія обходу графу за замовчуванням 
    /// </summary>
    class GraphIteratorStrategy
    {
        public Graph Graph;
        /// <summary>
        /// Ітератор вершин графу в порядку за замовчуванням (в якому вони були додані до списку вершин)
        /// </summary>
        /// <returns>IEnumerator<Node></returns>
        public virtual IEnumerator<Node> GetEnumerator()
        {
            foreach (var node in Graph.Nodes)
                yield return node;
        }
    }
    /// <summary>
    /// Обхід гафа в ширину
    /// </summary>
    class BreadthFirstSearch : GraphIteratorStrategy
    {
        /// <summary>
        /// Вершина, з якої починається обхід графу. Якщо не вказати, то обхід почнеться з [0] вершини в списку
        /// </summary>
        public Node StartNode;
        /// <summary>
        /// Черга вершин, які слід відвідати
        /// </summary>
        private Queue<Node> nodesToVisit = new Queue<Node>();
        /// <summary>
        /// список уже відвіданих вершин
        /// </summary>
        private List<Node> visitedNodes = new List<Node>();
        /// <summary>
        ///  скидання ітератора до початкового стану
        /// </summary>
        public void Reset()
        {
            nodesToVisit.Clear();
            visitedNodes.Clear();
        }
        /// <summary>
        /// Реалізація обходу в ширину 
        /// https://uk.wikipedia.org/wiki/%D0%9F%D0%BE%D1%88%D1%83%D0%BA_%D1%83_%D1%88%D0%B8%D1%80%D0%B8%D0%BD%D1%83
        /// </summary>
        /// <returns>IEnumerator<Node></returns>
        public override IEnumerator<Node> GetEnumerator()
        {
            if (StartNode == null)
                StartNode = Graph.Nodes[0];
            nodesToVisit.Enqueue(StartNode);
            while (nodesToVisit.Count > 0)
            {
                Node node = nodesToVisit.Dequeue();
                if (visitedNodes.Contains(node))
                    //якщо вершину вже відвідували раніше, то її не потрібно відвідувати ще раз. Запобігає циклічним блуканням
                    //але в деяких алгоритмах можливо потрібно буде yield return node
                    continue;
                visitedNodes.Add(node);
                foreach (Edge edge in node.OutcomeEdges)
                    nodesToVisit.Enqueue(edge.To);
                yield return node;
            }
            Reset();
        }
    }
    /// <summary>
    /// Обхід гафа в ширину
    /// </summary>
    class DepthFirstSearch : GraphIteratorStrategy
    {
        public Node StartNode;

        private Stack<Node> nodesToVisit = new Stack<Node>();
        private List<Node> visitedNodes = new List<Node>();

        public void Reset()
        {
            nodesToVisit.Clear();
            visitedNodes.Clear();
        }

        /// <summary>
        /// Реалізація обходу в глибину 
        /// https://uk.wikipedia.org/wiki/%D0%9F%D0%BE%D1%88%D1%83%D0%BA_%D1%83_%D0%B3%D0%BB%D0%B8%D0%B1%D0%B8%D0%BD%D1%83
        /// </summary>
        /// <returns>IEnumerator<Node></returns>
        public override IEnumerator<Node> GetEnumerator()
        {
            if (StartNode == null)
                StartNode = Graph.Nodes[0];
            nodesToVisit.Push(StartNode);
            while (nodesToVisit.Count > 0)
            {
                Node node = nodesToVisit.Pop();
                if (visitedNodes.Contains(node))
                    //якщо вершину вже відвідували раніше, то її не потрібно відвідувати ще раз. Запобігає циклічним блуканням
                    //але в деяких алгоритмах можливо потрібно буде yield return node
                    continue;
                visitedNodes.Add(node);
                foreach (Edge edge in node.OutcomeEdges)
                    nodesToVisit.Push(edge.To);
                yield return node;
            }
            // очищуємо стан, щоб можна було повторно зійснити обхід
            Reset();
        }
    }

}