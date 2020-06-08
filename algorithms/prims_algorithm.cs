using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithms {
    using MyCollection;
    using Graphs;

    class PrimsAlgorithm : Algorithm {
        BinaryHeap<int> queue = new BinaryHeap<int>();

        SortedDictionary<int, int> parent = new SortedDictionary<int, int>();
        SortedDictionary<int, double> dist = new SortedDictionary<int, double>();

        List<string> edges = new List<string>();
        string edge = "";

        int currentVertex = -1, count = 0;
        List<int> adjacent;

        public PrimsAlgorithm(Graph graph) : base(graph) { initValues(); }

        public override void search() {
            while (queue.count > 0) {
                currentVertex = queue.extract_min().Item1;
                foreach (var vertex in graph.neighbors(currentVertex))
                    step(vertex);
                visited[currentVertex] = true;
            }
            searching = false;
        }

        public override string result() {
            string path = "";

            foreach (var vertex in parent.Keys) {
                if (parent[vertex] > 0) {
                    path += vertex.ToString() +
                            " " +
                            parent[vertex].ToString() +
                            ";";
                }
            }

            if (path == "") return "unreachable";
            else return path;
        }

        public override void executeSearchStep() {
            if (queue.count > 0) {
                if (currentVertex < 0) {
                    currentVertex = queue.extract_min().Item1;
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

        public override IEnumerable<string> visitedEdges() { return edges; }
        public override string frontierEdge() { return edge; }

        void initValues() {
            int count = 0;

            foreach (var vertex in graph.vertices()) {
                parent[vertex] = 0;
                if (count == 0) dist[vertex] = 0.0;
                else dist[vertex] = double.PositiveInfinity;
                queue.add(vertex, dist[vertex]);
                count++;
            }

            searching = true;
        }

        void step(int vertex) {
            double d = graph.getDistance(currentVertex, vertex);

            if (!visited[vertex] && d < dist[vertex]) {
                dist[vertex] = d;
                queue.decrease(vertex, dist[vertex]);
                parent[vertex] = currentVertex;
            }
        }
    }
}
