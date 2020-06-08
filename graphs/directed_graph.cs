using System;
using System.Collections.Generic;
using System.IO;

namespace Graphs {
    public class DirectedGraph : GraphWithInterface {
        public override void connect(int i, int j) {
            vertex[i].Add(j, 0);
            onChange(Events.EDGE_ADDED, j);
        }

        public override void disconnect(int i, int j) {
            vertex[i].Remove(j);
            onChange(Events.EDGE_DELETED, j);
        }

        public override void deleteVertex(int val) {
            if (vertex.ContainsKey(val)) {
                vertex.Remove(val);
                weight.Remove(val);
                onChange(Events.VERTEX_DELETED, val);
            }
        }

        protected override void onChange(Graph.Events evnt, int id) {
            base.onChange(evnt, id);
        }
    }
}
