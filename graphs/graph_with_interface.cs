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
         * 1 4 5
         * 2 0 5
         * 3 3 5
         * 4 6
         * 5 8 1 2 3
         * where the first number on each line is vertex id, second - weight
         * and the rest - ids of adjacent vertices.
         */
        void build(string[] data) {
            if (data.Length == 0) return;

            foreach (string str in data) {
                string[] verticesData = str.Trim().Split(null);
                if (verticesData.Length > 1) {
                    int id = int.Parse(verticesData[0]);
                    SortedDictionary<int, double> edges =
                        new SortedDictionary<int, double>();

                    weight.Add(id, Int32.Parse(verticesData[1]));
                    for (int j = 2; j < verticesData.Length; j++)
                        edges.Add(int.Parse(verticesData[j]), 1.0);

                    vertex.Add(id, edges);
                }
            }
        }

        /*
         * Builds graph to store in .txt file. The following format is supported:
         * 1 4 5
         * 2 0 5
         * 3 3 5
         * 4 6
         * 5 8 1 2 3
         * where the first number on each line is vertex id, second - weight
         * and the rest - ids of adjacent vertices.
         */
        string[] store() {
            string[] graph = new string[vertex.Count + 1];
            int count = -1;

            foreach (var v in vertex) {
                count++;
                string line = v.Key.ToString() + " " + weight[v.Key] + " ";
                foreach (var edge in v.Value)
                    line += edge.Key.ToString() + " ";
                graph[count] = line.Trim();
            }

            return graph;
        }
    }
}
