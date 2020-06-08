using System;
using System.Collections.Generic;

namespace MyCollection {
    class Node<T> where T : IComparable<T> {
        public T val;
        public Node<T> parent, next;

        public Node(T v, Node<T> p = null, Node<T> n = null) {
            val = v;
            parent = p;
            next = n;
        }
    }

    class NodeWithPrio<T> {
        public T elem;
        public double prio;

        public NodeWithPrio(T elem, double prio) {
            this.elem = elem;
            this.prio = prio;
        }
    }

    class DataTuple {
        public int ttl;
        public string str;
        public DataTuple(string s, int t) { str = s; ttl = t; }
    }

    interface PriorityQueue<T> {
        int count { get; }
        void add(T elem, double priority);
        (T, double) extract_min();
        void decrease(T elem, double priority);
    }

    class BinaryHeap<T> : PriorityQueue<T> {
        List<NodeWithPrio<T>> a = new List<NodeWithPrio<T>>();
        Dictionary<T, int> pos = new Dictionary<T, int>();

        static int parent(int i) => (i - 1) / 2;
        static int left(int i) => 2 * i + 1;     // index of left child
        static int right(int i) => 2 * i + 2;    // index of right child

        public int count => a.Count;

        public void add(T elem, double prio) {
            a.Add(new NodeWithPrio<T>(elem, prio));
            pos[elem] = count - 1;
            up_heap(count - 1);
        }

        public (T, double) extract_min() {
            if (count <= 0)
                throw new System.InvalidOperationException("Queue is empty");

            NodeWithPrio<T> root = a[0];
            a[0] = a[count - 1];
            pos[a[0].elem] = 0;
            a.RemoveAt(count - 1);

            if (count > 1) { down_heap(0); }

            return (root.elem, root.prio);
        }

        public void decrease(T elem, double prio) {
            if (!pos.ContainsKey(elem))
                throw new System.InvalidOperationException("Not found");

            int i = pos[elem];
            a[i].prio = prio;
            up_heap(i);
        }

        void down_heap(int i) {
            int l = left(i);
            int r = right(i);
            int smallest = i;

            if (l < count && a[l].prio < a[smallest].prio) { smallest = l; }
            if (r < count && a[r].prio < a[smallest].prio) { smallest = r; }
            if (smallest != i) { swap(i, smallest); down_heap(smallest); }
        }

        void up_heap(int i) {
            while (i != 0 && a[parent(i)].prio > a[i].prio) {
                swap(i, parent(i));
                i = parent(i);
            }
        }

        void swap(int i, int j) {
            (a[i], a[j]) = (a[j], a[i]);
            pos[a[i].elem] = i;
            pos[a[j].elem] = j;
        }
    }
}
