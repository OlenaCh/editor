using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithms {
    using Graphs;

    public class Result {
        protected string initial;
        protected List<(int, int)> edges = new List<(int, int)>();

        public Result(string initial) { this.initial = initial; }
        public virtual bool buildResult() { return false; }
        public virtual List<(int, int)> result() { return edges; }
    }
}