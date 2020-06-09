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

        Stack<int> stack = new Stack<int>();
        SortedDictionary<int, DataTuple> total;

        List<string> edges = new List<string>();
        string edge = "";

        string path = "";
        int finResult = MIN;

        int currentVertex = -1, count = 0;
        List<int> adjacent;

        public LongestPathInDAG(Graph graph) : base(graph) { initValues(); }

        public override void search() {
            while (stack.Count > 0) {
                currentVertex = stack.Pop();

                if (total[currentVertex].ttl != MIN)
                    foreach(var vertex in graph.neighbors(currentVertex))
                        step(vertex);
            }

            searching = false;
        }

        public override string result() {
            if (finResult < 0) return "unreachable";
            else return path;
        }

        public override void executeSearchStep() {
            if (stack.Count > 0) {
                if (currentVertex < 0) {
                    currentVertex = stack.Pop();
                    if (total[currentVertex].ttl != MIN)
                        adjacent = graph.neighbors(currentVertex).ToList();
                }

                if (edge != "") edges.Add(edge);

                if (count < adjacent.Count) {
                    step(adjacent[count]);
                    edge = currentVertex + " " + adjacent[count];
                    count++;
                }
                else {
                    currentVertex = -1;
                    edge = "";
                    count = 0;
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

        public override IEnumerable<string> visitedEdges() { return edges; }
        public override string frontierEdge() { return edge; }

        void initValues() {
            foreach (var vertex in graph.vertices()) visited[vertex] = false;

            sort();

            total = new SortedDictionary<int, DataTuple>();
            foreach(var vertex in graph.vertices())
                total[vertex] = new DataTuple("", MIN);

            if (stack.Count > 0) {
                int start = stack.Peek();
                total[start].ttl = graph.getWeight(start);
                total[start].str = start + " ";
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

        void step(int vertex) {
            int weight = graph.getWeight(vertex);

            if (total[vertex].ttl < total[currentVertex].ttl + weight) {
                total[vertex].ttl = total[currentVertex].ttl + weight;
                total[vertex].str = total[currentVertex].str + vertex + " ";

                if (total[vertex].ttl > finResult) {
                    finResult = total[vertex].ttl;
                    path = total[vertex].str;
                }
            }
        }
    }
}