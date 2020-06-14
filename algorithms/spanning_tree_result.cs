using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithms {
    using Graphs;

    public class SpanningTreeResult : Result {
        public SpanningTreeResult(string initial) : base(initial) {}

        public override bool buildResult() {
            if (initial == "unreachable") return false;
            buildSpanningTree();
            return true;
        }

        void buildSpanningTree() {
            foreach (string pair in initial.Trim().Split(';')) {
                if (pair != "") {
                    string[] vertices = pair.Trim().Split(null);
                    int v1 = Int32.Parse(vertices[0].ToString());
                    int v2 = Int32.Parse(vertices[1].ToString());
                    edges.Add((v1, v2));
                }
            }
        }
    }
}
