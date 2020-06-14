using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithms {
    using Graphs;

    public class PathResult : Result {
        public PathResult(string initial) : base(initial) {}

        public override bool buildResult() {
            if (initial == "unreachable") return false;
            buildFoundPath();
            return true;
        }

        void buildFoundPath() {
            string[] tmp = initial.Trim().Split();
            for (int i = 0; i < tmp.Length - 1; i++) {
                int v1 = Int32.Parse(tmp[i]);
                int v2 = Int32.Parse(tmp[i + 1]);
                edges.Add((v1, v2));
            }
        }
    }
}
