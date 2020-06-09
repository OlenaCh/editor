using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithms {
    using Graphs;

    class PrimsAlgorithm : Algorithm {
        SortedDictionary<int, int> parent = new SortedDictionary<int, int>();
        SortedDictionary<int, double> dist = new SortedDictionary<int, double>();

        List<string> edges = new List<string>();
        string edge = "";

        int currentVertex, totalCount = 0, adjacentCount = 0;
        List<int> adjacent;

        public PrimsAlgorithm(Graph graph) : base(graph) { initValues(); }

        public override void search() {
            int size = graph.vertices().Count();

            for (int i = 0; i < size; i++) {
                currentVertex = minKey();
                if (currentVertex > 0) visited[currentVertex] = true;
                foreach (var vertex in graph.neighbors(currentVertex))
                    step(currentVertex, vertex);
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
            if (totalCount < graph.vertices().Count()) {
                if (currentVertex <= 0) {
                    currentVertex = minKey();
                    if (currentVertex > 0) visited[currentVertex] = true;
                    adjacent = graph.neighbors(currentVertex).ToList();
                    totalCount++;
                }

                if (edge != "") edges.Add(edge);

                if (adjacentCount < adjacent.Count) {
                    step(currentVertex, adjacent[adjacentCount]);
                    edge = currentVertex + " " + adjacent[adjacentCount];
                    adjacentCount++;
                }
                else {
                    currentVertex = 0;
                    edge = "";
                    adjacentCount = 0;
                }
            }
            else searching = false;
        }

        public override IEnumerable<string> visitedEdges() { return edges; }
        public override string frontierEdge() { return edge; }

        void initValues() {
            int count = 0;

            foreach (var vertex in graph.vertices()) {
                visited[vertex] = false;
                parent[vertex] = -1;
                if (count == 0) dist[vertex] = 0.0;
                else dist[vertex] = double.PositiveInfinity;
                count++;
            }

            searching = true;
        }

        void step(int a, int b) {
            double d = graph.getDistance(a, b);
            if (d >= 0.0 && !visited[b] && d < dist[b]) {
                parent[b] = a;
                dist[b] = d;
            }
        }

        int minKey() {
            double min = double.PositiveInfinity;
            int ind = -1;

            foreach (var distance in dist) {
                if (!visited[distance.Key] && distance.Value < min) {
                    min = distance.Value;
                    ind = distance.Key;
                }
            }

            return ind;
        }
    }
}

