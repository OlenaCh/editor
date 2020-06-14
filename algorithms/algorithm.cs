using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithms {
    using Graphs;

    public abstract class Algorithm {
        public abstract void search();
        public abstract Result result();

        public abstract void executeSearchStep();

        public abstract IEnumerable<int> visitedVertices();
        public abstract IEnumerable<int> frontierVertices();

        public abstract IEnumerable<(int, int)> visitedEdges();
        public abstract IEnumerable<(int, int)> frontierEdges();

        public abstract bool running();
    }
}