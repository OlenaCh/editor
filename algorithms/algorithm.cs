using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithms {
    using Graphs;

    public class Algorithm {
        protected Graph graph;
        protected SortedDictionary<int, bool> visited =
            new SortedDictionary<int, bool>();
        protected bool searching = false;

        public Algorithm(Graph graph) { this.graph = graph; }

        public virtual void search() {}
        public virtual string result() { return ""; }

        public virtual void executeSearchStep() {}

        public virtual IEnumerable<int> visitedVertices() { return null; }
        public virtual IEnumerable<int> frontierVertices() { return null; }

        public virtual IEnumerable<string> visitedEdges() { return null; }
        public virtual string frontierEdge() { return null; }

        public bool running() { return searching; }
    }
}