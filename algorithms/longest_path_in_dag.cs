using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithms {
    using Graphs;

    class LongestPathInDAG : Algorithm {
        class DataTuple {
            public int ttl;
            public string str;
            public DataTuple(string s, int t) { str = s; ttl = t; }
        }

        const int MIN = -10000;

        Graph graph;
        SortedDictionary<int, bool> visited = new SortedDictionary<int, bool>();

        Stack<int> stack = new Stack<int>();
        SortedDictionary<int, DataTuple> total;

        List<(int, int)> _visitedEdges = new List<(int, int)>();
        List<(int, int)> _frontierEdges = new List<(int, int)>();

        bool searching = false;

        string path = "";
        int finResult = MIN;
        int currentVertex = -1;

        public LongestPathInDAG(Graph graph) {
            this.graph = graph;
            initValues();
        }

        public override void search() {
            while (stack.Count > 0) step();
            searching = false;
        }

        public override Result result() {
            if (finResult < 0) path = "unreachable";
            return new PathResult(path);
        }

        public override void executeSearchStep() {
            if (stack.Count > 0) {
                step();

                if (currentVertex > 0) {
                    if (_frontierEdges.Count() >= 1) {
                        foreach (var edge in _frontierEdges)
                            _visitedEdges.Add(edge);
                        _frontierEdges = new List<(int, int)>();
                    }
                    foreach(var vertex in graph.neighbors(currentVertex))
                        _frontierEdges.Add((currentVertex, vertex));
                }
            }
            else searching = false;
        }

        public override IEnumerable<int> visitedVertices() {
            List<int> vertices = new List<int>();
            foreach (var vertex in graph.vertices())
                if (!stack.Contains(vertex)) vertices.Add(vertex);
            return vertices;
        }

        public override IEnumerable<int> frontierVertices() { return null; }

        public override IEnumerable<(int, int)> visitedEdges() {
            return _visitedEdges;
        }

        public override IEnumerable<(int, int)> frontierEdges() {
            return _frontierEdges;
        }

        public override bool running() { return searching; }

        void initValues() {
            foreach (var vertex in graph.vertices()) visited[vertex] = false;

            sort();

            total = new SortedDictionary<int, DataTuple>();
            foreach(var vertex in graph.vertices())
                total[vertex] = new DataTuple("", MIN);

            if (stack.Count > 0) {
                int start = stack.Peek();
                setVertexSortingData(start);
            }

            searching = true;
        }

        void sort() {
            List<int> keys = visited.Keys.ToList();
            for (int i = 0; i < keys.Count; i++) {
                if (!visited[keys[i]]) _sort(keys[i], visited);
            }
        }

        void _sort(int key, SortedDictionary<int, bool> visited) {
            visited[key] = true;
            foreach (var vertex in graph.neighbors(key))
                if (!visited[vertex]) _sort(vertex, visited);
            stack.Push(key);
        }

        void step() {
            currentVertex = stack.Pop();

            if (total[currentVertex].ttl == MIN)
                setVertexSortingData(currentVertex);

            foreach(var vertex in graph.neighbors(currentVertex)) {
                int weight = graph.getWeight(vertex);

                if (total[vertex].ttl <= total[currentVertex].ttl + weight) {
                    total[vertex].ttl = total[currentVertex].ttl + weight;
                    total[vertex].str = total[currentVertex].str + vertex + " ";

                    if (total[vertex].ttl > finResult) {
                        finResult = total[vertex].ttl;
                        path = total[vertex].str;
                    }
                }
            }
        }

        void setVertexSortingData(int vertex) {
            total[vertex].ttl = graph.getWeight(vertex);
            total[vertex].str = vertex + " ";
        }
    }
}
