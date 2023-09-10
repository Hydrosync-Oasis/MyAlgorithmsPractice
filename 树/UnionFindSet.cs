using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace 树 {
    public class UnionFindSet<T> where T : notnull {
        private class Node {
            public T val;
            public int Depth {
                get;
                set;
            }
            public Node(T value) {
                val = value;
                Depth = -1;
            }

            public Node(T val, UnionFindSet<T>.Node parent) : this(val) {
                this.parent = parent;
            }

            public Node parent;
        }

        Dictionary<T, Node> map = new();
        Dictionary<Node, int> sizeMap = new();//记录集合内有多少个元素

        public bool Add(T val) {
            if (map.ContainsKey(val)) {
                return false;
            } else {
                Node node = new(val);
                node.parent = node;
                node.Depth = 1;
                map.Add(val, node);
                sizeMap.Add(node, 1);
                return true;
            }
        }

        public void AddRange(IList<T> list) {
            foreach (var item in list) {
                Add(item);
            }
        }

        public bool Union(T val1, T val2) {
            if (!map.ContainsKey(val1) || !map.ContainsKey(val2)) {
                return false;
            }
            Node parent1 = GetParent(val1);
            Node parent2 = GetParent(val2);
            if (parent1 == parent2) {
                return false;
            } else {
                if (parent1.Depth > parent2.Depth) {
                    parent2.parent = parent1;
                    parent2.Depth = -1;
                    sizeMap[parent1] += sizeMap[parent2];
                    sizeMap.Remove(parent2);
                } else {
                    if (parent1.Depth == parent2.Depth) {
                        parent2.Depth++;
                    }
                    parent1.parent = parent2;
                    parent1.Depth = -1;
                    sizeMap[parent2] += sizeMap[parent1];
                    sizeMap.Remove(parent1);
                }
                return true;
            }
        }

        public bool IsSameSet(T val1, T val2) {
            if (!map.ContainsKey(val1) || !map.ContainsKey(val2)) { return false; }
            return GetParent(val1) == GetParent(val2);
        }

        private Node GetParent(T val) {
            Node cur = map[val];
            Stack<Node> st = new();
            while (cur.parent != cur) {
                st.Push(cur);
                cur.Depth = -1;
                cur = cur.parent;
            }
            while (st.Count != 0) {
                Node cur2 = st.Pop();
                cur2.parent = cur;
            }
            cur.Depth = 2;
            return cur;
        }

        public int GetSetNum() => sizeMap.Count;

        public int[] GetEachSetCount() => sizeMap.Values.ToArray();
    }
}
