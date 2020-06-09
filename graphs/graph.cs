using System;
using System.Collections.Generic;
using System.IO;

namespace Graphs {
    public class Graph {
        // Events handlers
        public enum Events {
            VERTEX_ADDED,
            VERTEX_DELETED,
            EDGE_ADDED,
            EDGE_DELETED,
            WEIGHT_CHANGED,
            DISTANCE_CHANGED
        };

        public delegate void Notify(Graph.Events evnt, int id);
        public event Notify changed;

        protected virtual void onChange(Graph.Events evnt, int id) {
            changed?.Invoke(evnt, id);
        }

        // Data containers
        protected SortedDictionary<int, SortedDictionary<int, double>> vertex =
            new SortedDictionary<int, SortedDictionary<int, double>>();
        protected SortedDictionary<int, int> weight =
            new SortedDictionary<int, int>();

        public Graph() {}

        // Main methods dealing with vertices
        public int addVertex() {
            int id = 1;
            while (vertex.ContainsKey(id)) { ++id; }
            vertex[id] = new SortedDictionary<int, double>();
            weight[id] = 0;
            onChange(Events.VERTEX_ADDED, id);
            return id;
        }

        public virtual void connect(int i, int j) {
            vertex[i].Add(j, -1.0);
            vertex[j].Add(i, -1.0);
            onChange(Events.EDGE_ADDED, j);
        }

        public virtual void disconnect(int i, int j) {
            vertex[i].Remove(j);
            vertex[j].Remove(i);
            onChange(Events.EDGE_DELETED, j);
        }

        public bool areConnected(int i, int j) {
            return vertex[i].ContainsKey(j) && vertex[j].ContainsKey(i);
        }

        public IEnumerable<int> vertices() { return vertex.Keys; }

        public IEnumerable<int> neighbors(int id) { return vertex[id].Keys; }

        public virtual void deleteVertex(int val) {
            if (vertex.ContainsKey(val)) {
                foreach (var v in vertex[val].Keys)
                    vertex[v].Remove(val);
                vertex.Remove(val);
                weight.Remove(val);
                onChange(Events.VERTEX_DELETED, val);
            }
        }

        // Methods dealing with weights
        public void setWeight(int val, int wght) {
            if (weight.ContainsKey(val)) {
                weight[val] = wght;
                onChange(Events.WEIGHT_CHANGED, val);
            }
        }

        public int getWeight(int val) {
            if (weight.ContainsKey(val)) return weight[val];
            return -1;
        }

        // Methods dealing with distance
        public virtual void setDistance(int a, int b, double dist) {
            vertex[a][b] = dist;
            vertex[b][a] = dist;
            onChange(Events.DISTANCE_CHANGED, a);
        }

        public double getDistance(int a, int b) {
            if (vertex.ContainsKey(a) && vertex[a].ContainsKey(b))
                return vertex[a][b];
            return -1.0;
        }
    }
}
