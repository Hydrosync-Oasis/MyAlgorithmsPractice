namespace 树 {
    public class PrefixTree {
        Dictionary<char, int> dic = new();
        public class Node {
            public char val;
            public Dictionary<char, Node> children = new();
            public int pass;
            public int end;
            public Node(char val) {
                this.val = val;
            }

            public Node(char val, int pass) {
                this.val = val;
                this.pass = pass;
            }
        }

        public Node root;

        public PrefixTree() {
            root = new Node(' ');
        }

        public int Count => root.pass;

        public Dictionary<char, Node> Children => root.children;

        public void Add(string str) {
            root.pass++;
            Node cur = root;
            for (int i = 0; i < str.Length; i++) {
                if (!cur.children.ContainsKey(str[i])) {
                    cur.children.Add(str[i], new(str[i]));
                    cur.children[str[i]].pass++;
                }
                cur = cur.children[str[i]];
                cur.pass++;
                if (i == str.Length - 1) {
                    cur.end++;
                }
            }
        }

        public void Remove(string str) {
            Node cur = root;
            cur.pass--;
            for (int i = 0; i < str.Length; i++) {
                char c = str[i];
                if (cur.children.ContainsKey(c)) {
                    if (--cur.children[c].pass == 0) {
                        cur.children.Remove(c);
                        return;
                    }
                    cur = cur.children[c];
                    if (i == str.Length - 1) {
                        cur.end--;
                    }
                } else {
                    throw new InvalidOperationException($"前缀树中不包含 {str}。");
                }
            }
        }

        public bool Contains(string str) {
            Node cur = root;
            for (int i = 0; i < str.Length; i++) {
                char c = str[i];
                if (!cur.children.ContainsKey(c)) {
                    return false;
                } else {
                    cur = cur.children[c];
                }
            }
            return cur.end > 0;
        }

        public int StartsWith(string prefix) {
            Node cur = root;
            for (int i = 0; i < prefix.Length; i++) {
                if (!cur.children.ContainsKey(prefix[i])) {
                    return 0;
                }
                cur = cur.children[prefix[i]];
            }
            return cur.pass;
        }
    }
}
