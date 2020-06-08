using System;
using System.Collections.Generic;
using System.IO;

namespace Graphs {
    public class GraphWithInterface : Graph, GraphIOInterface {
        public void fromFile(string filename) {
            try {
                string[] data = File.ReadAllLines(filename);
                if (data.Length > 0) { build(data); }
            }
            catch (IOException e) {
                Console.WriteLine("The file could not be read: " + e.Message);
            }
        }

        public void toFile(string filename) {
            try { File.WriteAllLines(filename, store()); }
            catch (IOException e) {
                Console.WriteLine(
                    "The file could not be write to: " + e.Message
                );
            }
        }

        /*
         * Builds graph from .txt file. The following format is supported:
         * 3 4 12 56 9
         * 1 5
         * 2 5
         * 3 5
         * 4
         * 5 1 2 3
         * where the first line contains an array of weights for each vertex
         * (so, vertex #1 has weight 3, vertex #2 - 4 and so on).
         * If graph is unweighted then the first line contains an array of zeros.
         * The rest of lines provide details on adjacent vertices.
         * First number on each line is vertex id and the rest - ids of
         * adjacent vertices.
         */
        void build(string[] data) {
            if (data.Length == 0) return;

            int count = 0;

            foreach (string str in data) {
                string[] verticesData = str.Trim().Split(null);
                if (verticesData.Length > 0) {
                    if (count == 0) {
                        int verticesCount = 1;
                        foreach (var vertex in verticesData) {
                            weight.Add(verticesCount, Int32.Parse(vertex));
                            verticesCount++;
                        }
                        count++;
                    }
                    else {
                        int id = int.Parse(verticesData[0]);
                        SortedDictionary<int, int> edges =
                            new SortedDictionary<int, int>();
                        for (int j = 1; j < verticesData.Length; j++)
                            edges.Add(int.Parse(verticesData[j]), 0);
                        vertex.Add(id, edges);
                    }
                }
            }
        }

        /*
         * Builds graph to store in .txt file. The following format is supported:
         * 3 4 12 56 9
         * 1 5
         * 2 5
         * 3 5
         * 4
         * 5 1 2 3
         * where the first line contains an array of weights for each vertex
         * (so, vertex #1 has weight 3, vertex #2 - 4 and so on).
         * If graph is unweighted then the first line contains an array of zeros.
         * The rest of lines provide details on adjacent vertices.
         * First number on each line is vertex id and the rest - ids of
         * adjacent vertices.
         */
        string[] store() {
            if (vertex.Count == 0) return new string[0];

            string[] graph = new string[vertex.Count + 1];

            string verticesWeights = "";
            foreach (var vertex in weight) verticesWeights += vertex.Value.ToString() + " ";
            graph[0] = verticesWeights;

            int count = 1;
            foreach (var v in vertex) {
                string line = v.Key.ToString() + " ";
                foreach (var edge in v.Value)
                    line += edge.Key.ToString() + " ";
                graph[count] = line.Trim();
                count++;
            }

            return graph;
        }
    }
}
