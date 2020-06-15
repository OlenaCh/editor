using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithms {
    using Graphs;

    class PrimsAlgorithm : Algorithm {
        Graph graph;

        SortedDictionary<int, bool> visited = new SortedDictionary<int, bool>();
        SortedDictionary<int, int> parent = new SortedDictionary<int, int>();
        SortedDictionary<int, double> dist = new SortedDictionary<int, double>();

        List<(int, int)> _visitedEdges = new List<(int, int)>();
        List<(int, int)> _frontierEdges = new List<(int, int)>();

        bool searching = false;

        int currentVertex, totalCount = 0;

        public PrimsAlgorithm(Graph graph) {
            this.graph = graph;
            initValues();
        }

        public override void search() {
            int size = graph.vertices().Count();
            for (int i = 0; i < size; i++) step();
            searching = false;
        }

        public override Result result() {
            string path = "";

            foreach (var vertex in parent.Keys) {
                if (parent[vertex] > 0) {
                    path += vertex.ToString() +
                            " " +
                            parent[vertex].ToString() +
                            ";";
                }
            }

            if (path == "") path = "unreachable";
            return new SpanningTreeResult(path);
        }

        public override void executeSearchStep() {
            if (totalCount < graph.vertices().Count()) {
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

                totalCount++;
            }
            else searching = false;
        }

        public override IEnumerable<int> visitedVertices() { return null; }
        public override IEnumerable<int> frontierVertices() { return null; }

        public override IEnumerable<(int, int)> visitedEdges() {
            return _visitedEdges;
        }

        public override IEnumerable<(int, int)> frontierEdges() {
            return _frontierEdges;
        }

        public override bool running() { return searching; }

        void initValues() {
            int count = 0;

            foreach (var vertex in graph.vertices()) {
                visited[vertex] = false;
                parent[vertex] = -1;
                if (count == 0) dist[vertex] = 1.0;
                else dist[vertex] = double.PositiveInfinity;
                count++;
            }

            searching = true;
        }

        void step() {
            currentVertex = minKey();
            if (currentVertex > 0) {
                visited[currentVertex] = true;
                foreach (var vertex in graph.neighbors(currentVertex)) {
                    double d = graph.getEdgeWeight(currentVertex, vertex);
                    if (d > 0.0 && !visited[vertex] && d < dist[vertex]) {
                        parent[vertex] = currentVertex;
                        dist[vertex] = d;
                    }
                }
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
