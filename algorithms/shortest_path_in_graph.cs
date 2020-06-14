using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithms {
    using Graphs;

    class ShortestPathInGraph : Algorithm {
        class Node<T> where T : IComparable<T> {
            public T val;
            public Node<T> parent, next;

            public Node(T v, Node<T> p = null, Node<T> n = null) {
                val = v;
                parent = p;
                next = n;
            }
        }

        Graph graph;
        int start, end;

        SortedDictionary<int, bool> visited = new SortedDictionary<int, bool>();

        Queue<Node<int>> queue = new Queue<Node<int>>();
        Node<int> curr;

        bool searching = false;

        public ShortestPathInGraph(Graph graph, int start, int end) {
            this.graph = graph;
            this.start = start;
            this.end = end;
            initValues();
        }

        public override void search() {
            if (curr.val == end) { searching = false; return; }
            while (curr != null && searching) step();
            searching = false;
        }

        public override Result result() {
            string path = (curr != null) ? shortestPath() : "unreachable";
            return new PathResult(path);
        }

        public override void executeSearchStep() {
            if (curr == null || curr.val == end) { searching = false; return; }
            step();
        }

        public override IEnumerable<int> visitedVertices() {
            List<int> vertices = new List<int>();
            foreach (var vertex in visited.Keys)
                if (visited[vertex]) vertices.Add(vertex);
            return vertices;
        }

        public override IEnumerable<int> frontierVertices() {
            List<int> vertices = new List<int>();
            foreach (var vertex in queue) vertices.Add(vertex.val);
            return vertices;
        }

        public override IEnumerable<(int, int)> visitedEdges() { return null; }
        public override IEnumerable<(int, int)> frontierEdges() { return null; }

        public override bool running() { return searching; }

        void initValues() {
            foreach (var vertex in graph.vertices()) visited[vertex] = false;

            curr = new Node<int>(start);
            visited[curr.val] = true;

            searching = true;
        }

        string shortestPath() {
            string result = " " + curr.val;
            Node<int> p = curr.parent;

            while (p != null) {
                result = " " + p.val + result;
                p = p.parent;
            }

            return result.Trim();
        }

        void step() {
            foreach (var neighbor in graph.neighbors(curr.val)) {
                if (!visited[neighbor]) {
                    visited[neighbor] = true;

                    Node<int> node = new Node<int>(neighbor, curr);
                    queue.Enqueue(node);

                    if (neighbor == end) {
                        curr = node;
                        searching = false;
                        return;
                    }
                }
            }

            curr = queue.Count > 0 ? queue.Dequeue() : null;
        }
    }
}