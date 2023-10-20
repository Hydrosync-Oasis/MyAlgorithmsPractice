using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 树 {
    internal class HuffmanTree<TItem> {
        [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
        private class Node<T>(T item, ulong weight) {
            /// <summary>
            /// 数据项
            /// </summary>
            public T Item { get; private set; } = item;
            /// <summary>
            /// 实际权重值
            /// </summary>
            public ulong Weight { get; private set; } = weight;
            public Node<T> Parent { get; set; }

            public Node<T>? LeftChild { get; set; }
            public Node<T>? RightChild { get; set; }

            public static bool operator <(Node<T> a, Node<T> b) => a.Weight.CompareTo(b.Weight) < 0;
            public static bool operator >(Node<T> a, Node<T> b) => a.Weight.CompareTo(b.Weight) > 0;
            public static bool operator >=(Node<T> a, Node<T> b) => a.Weight.CompareTo(b.Weight) >= 0;
            public static bool operator <=(Node<T> a, Node<T> b) => a.Weight.CompareTo(b.Weight) <= 0;

            public static bool operator ==(Node<T> a, Node<T> b) =>
                a.Weight.CompareTo(b.Weight) == 0;
            public static bool operator !=(Node<T> a, Node<T> b) =>
                a.Weight.CompareTo(b.Weight) != 0;

            private string GetDebuggerDisplay() => $"权重：{Weight}, 数据：{Item}";
        }

        Node<TItem> root;

        public HuffmanTree(IList<TItem> items, IList<ulong> weights) {
            if (items.Count != weights.Count) {
                throw new ArgumentException("两个表大小应相等。");
            }

            PriorityQueue<Node<TItem>, ulong> pq = new();
            for (int i = 0; i < items.Count; i++) {
                pq.Enqueue(new(items[i], weights[i]), weights[i]);
            }

            while (pq.Count > 1) {
                var a = pq.Dequeue();
                var b = pq.Dequeue();
                Node<TItem> pa = new(default, a.Weight + b.Weight) {
                    LeftChild = a,
                    RightChild = b
                };
                pq.Enqueue(pa, pa.Weight);
            }
            root = pq.Dequeue();
        }
    }
}
