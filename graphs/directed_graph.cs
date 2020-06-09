using System;
using System.Collections.Generic;
using System.IO;

namespace Graphs {
    public class DirectedGraph : GraphWithInterface {
        public override void connect(int i, int j) {
            vertex[i].Add(j, -1.0);
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

        public override void setDistance(int a, int b, double dist) {
            vertex[a][b] = dist;
            onChange(Events.DISTANCE_CHANGED, a);
        }

        protected override void onChange(Graph.Events evnt, int id) {
            base.onChange(evnt, id);
        }
    }
}
