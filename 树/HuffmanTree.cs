using System.Diagnostics;
using System.Text;

namespace 树 {
    internal class HuffmanTree<TItem> where TItem : notnull {
        [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
        private class Node<T>(T? item, ulong weight) {
            /// <summary>
            /// 数据项
            /// </summary>
            public T? Item { get; private set; } = item;
            /// <summary>
            /// 实际权重值
            /// </summary>
            public ulong Weight { get; private set; } = weight;
            public Node<T>? Parent { get; set; }

            public Node<T>? LeftChild { get; set; }
            public Node<T>? RightChild { get; set; }

            /// <summary>
            /// 返回该节点是否是叶子节点，叶子节点Item存储真正的值
            /// </summary>
            public bool IsLeaf => LeftChild is null && RightChild is null;

            private string GetDebuggerDisplay() => $"权重：{Weight}, 数据：{Item}";
        }

        Node<TItem> root;
        Dictionary<TItem, Node<TItem>> map;

        public HuffmanTree(IList<TItem> items, IList<ulong> weights) {
            if (items.Count != weights.Count) {
                throw new ArgumentException("两个表大小应相等。");
            }
            map = new();

            PriorityQueue<Node<TItem>, ulong> pq = new();
            for (int i = 0; i < items.Count; i++) {
                map.Add(items[i], new(items[i], weights[i]));
                pq.Enqueue(map[items[i]], weights[i]);
            }

            while (pq.Count > 1) {
                var a = pq.Dequeue();
                var b = pq.Dequeue();
                Node<TItem> pa = new(default, a.Weight + b.Weight) {
                    LeftChild = a,
                    RightChild = b
                };
                a.Parent = pa;
                b.Parent = pa;
                pq.Enqueue(pa, pa.Weight);
            }
            root = pq.Dequeue();
        }

        public string GetCode(TItem item) {
            StringBuilder sb = new();
            var node = map[item];
            while (true) {
                var pa = node.Parent;
                if (pa is null) {
                    break;
                }
                if (pa.LeftChild! == node) {
                    sb.Append('0');
                } else {
                    sb.Append('1');
                }
                node = pa;
            }
            for (int i = 0; i < sb.Length / 2; i++) {
                (sb[i], sb[^(i + 1)]) = (sb[^(i + 1)], sb[i]);
            }
            return sb.ToString();
        }



        public static HuffmanTree<char> CreateFromText(string txt, out string coded) {
            Dictionary<char, ulong> dic = new();
            for (int i = 0; i < txt.Length; i++) {
                if (!dic.ContainsKey(txt[i])) {
                    dic[txt[i]] = 0;
                }
                dic[txt[i]]++;
            }
            List<char> lKey = dic.Keys.ToList();
            List<ulong> lVal = dic.Values.ToList();

            HuffmanTree<char> t = new(lKey, lVal);
            StringBuilder sb = new();
            for (int i = 0; i < txt.Length; i++) {
                sb.Append(t.GetCode(txt[i]));
            }
            coded = sb.ToString();
            return t;
        }

        /// <summary>
        /// 返回二进制字符串密文对应的明文
        /// </summary>
        /// <param name="code">二进制字符串，仅包含0和1</param>
        /// <returns></returns>
        public TItem GetItem(string code) {
            Node<TItem> t = root;
            for (int i = 0; i < code.Length; i++) {
                if (code[i] == '0') {
                    t = t.LeftChild ?? throw new NotInTreeException(nameof(code));
                } else {
                    t = t.RightChild ?? throw new NotInTreeException(nameof(code));
                }
            }
            if (!t.IsLeaf) {
                throw new NotInTreeException(nameof(code));
            }
            return t.Item!;
        }

        public IList<TItem> Decode(string code) {
            IList<TItem> list = new List<TItem>();
            Node<TItem> t = root;
            for (int i = 0; i < code.Length; i++) {
                if (code[i] == '0') {
                    t = t.LeftChild ?? throw new NotInTreeException(nameof(code));
                } else {
                    t = t.RightChild ?? throw new NotInTreeException(nameof(code));
                }

                if (t.IsLeaf) {
                    list.Add(t.Item!);
                    t = root;
                }
            }
            return list;
        }
    }

    public class NotInTreeException : ArgumentException {
        public NotInTreeException(string paramName) : base("密文不在该树中", paramName) { }
    }
}
