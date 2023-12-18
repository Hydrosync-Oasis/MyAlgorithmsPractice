using ServiceStack;
using System.Text;
using 树;

namespace Algorithm {

    internal class MyCircularQueue {
        int[] data;
        int N;
        int first = 0, last = 0;

        public MyCircularQueue(int k) {
            data = new int[k];
            //N = k;
        }

        public bool EnQueue(int value) {
            if (N == data.Length) {
                return false;
            }
            data[last] = value;
            last = (last + 1) % this.data.Length;
            N++;
            return true;
        }

        public bool DeQueue() {
            if (N == 0) {
                return false;
            }
            data[first] = default;
            first = (first + 1) % data.Length;
            N--;

            return true;
        }

        public int Front() {
            if (N == 0) {
                return -1;
            } else {
                return data[first];
            }
        }

        public int Rear() {
            if (this.N == 0) {
                return -1;
            } else {
                return data[(last - 1 + data.Length) % data.Length];
            }
        }

        public bool IsEmpty() => first == last;

        public bool IsFull() => (last + 1) % data.Length == first;
    }

    internal class MyCircularDeque {
        int[] data;

        int start = 0;//先设置再移动
        int end = 0;//先移动再设置
        int N = 0;

        public MyCircularDeque(int k) => data = new int[k];

        public bool InsertFront(int value) {
            if (N == data.Length) {
                return false;
            }
            data[start] = value;
            start = (start + data.Length - 1) % data.Length;//此时start处的数组成员空
            N++;
            return true;
        }

        public bool InsertLast(int value) {
            if (N == data.Length) {
                return false;
            }
            end = (end + data.Length + 1) % data.Length;//此时start处的数组成员空
            data[end] = value;
            N++;
            return true;
        }

        public bool DeleteFront() {
            if (N == 0) {
                return false;
            }
            start = (start + data.Length + 1) % data.Length;
            data[start] = default;
            N--;
            return true;
        }

        public bool DeleteLast() {
            if (N == 0) {
                return false;
            }
            data[end] = default;
            end = (end + data.Length - 1) % data.Length;
            N--;
            return true;
        }

        public int GetFront() {
            if (N == 0) {
                return -1;
            } else {
                return data[(start + data.Length + 1) % data.Length];
            }
        }

        public int GetRear() {
            if (N == 0) {
                return -1;
            } else {
                return data[end];
            }
        }

        public bool IsEmpty() => N == 0;

        public bool IsFull() => N == data.Length;
    }

    internal class MyLinkedListNoDummy {
        private class Node {
            public int val;
            public Node? next;

            public Node(int val, Node? next = null) {
                this.val = val;
                this.next = next;
            }
        }

        private Node? head;

        int N = 0;

        public MyLinkedListNoDummy() {

        }

        public int Get(int index) {
            Node? cur = head;
            for (int i = 0; i < index; i++) {
                cur = cur?.next;
            }
            return cur is null ? -1 : cur.val;
        }

        public void AddAtHead(int val) {
            if (N == 0) {
                head = new(val);
            } else {
                head = new(val, head);
            }
            N++;
        }

        public void AddAtTail(int val) {
            Node cur = head;

            if (N == 0) {
                head = new(val);
                N++;
                return;
            }
            while (true) {
                if (cur.next is null) {
                    cur.next = new(val);
                    N++;
                    return;
                }

                cur = cur.next;
            }
        }

        public void AddAtIndex(int index, int val) {
            if (index > N) {
                return;
            }
            if (index <= 0) {
                AddAtHead(val);
            } else if (index == N) {
                AddAtTail(val);
            } else {
                Node? cur = head;
                for (int i = 0; i < index - 1; i++) {
                    cur = cur?.next;
                }

                if (cur is null) {
                    return;
                } else {
                    Node? tmp = cur.next;
                    cur.next = new(val, tmp);
                }

                N++;
            }
        }

        public void DeleteAtIndex(int index) {
            if (index >= N || index < 0) {
                return;
            }

            if (index == 0) {
                head = head?.next;
                N--;
                return;
            }

            Node? cur = head;
            for (int i = 0; i < index - 1; i++) {
                cur = cur?.next;
            }
            if (cur is null) {
                return;
            }

            cur.next = cur.next?.next;
            N--;
        }
    }

    internal class MyLinkedList {
        private class Node {
            public int val;
            public Node? next;

            public Node(int val, Node? next = null) {
                this.val = val;
                this.next = next;
            }
        }

        private Node dummyhead;

        int N = 0;

        public MyLinkedList() {
            dummyhead = new(-1, null);//dummy
        }

        public int Get(int index) {
            Node? cur = dummyhead;
            for (int i = 0; i < index + 1; i++) {
                cur = cur?.next;
            }
            return cur is null ? -1 : cur.val;
        }

        public void AddAtHead(int val) {
            dummyhead.next = new(val, dummyhead.next);

            N++;
        }

        public void AddAtTail(int val) {
            Node cur = dummyhead;

            while (true) {
                if (cur.next is null) {
                    cur.next = new(val);
                    N++;
                    return;
                }

                cur = cur.next;
            }
        }

        public void AddAtIndex(int index, int val) {
            Node? cur = dummyhead;
            for (int i = 0; i < index; i++) {
                cur = cur?.next;
            }

            if (cur is null) {
                return;
            } else {
                Node? tmp = cur?.next;
                cur.next = new(val, tmp);
                N++;
            }

        }

        public void DeleteAtIndex(int index) {
            if (index >= N || index < 0) {
                return;
            }

            Node? cur = dummyhead;
            for (int i = 0; i < index; i++) {
                cur = cur?.next;
            }

            cur.next = cur.next?.next;
            N--;
        }
    }

    internal class TreeNode {
        public int val;
        public TreeNode? left;
        public TreeNode? right;
        public TreeNode(int val = 0, TreeNode? left = null, TreeNode? right = null) {
            this.val = val;
            this.left = left;
            this.right = right;
        }
    }

    internal class ListNode {
        public int val;
        public ListNode? next;
        public ListNode(int x) {
            val = x;
            next = null;
        }

    }

    public class Node {
        public int val;
        public Node? next;
        public Node? random;

        public Node(int _val) {
            val = _val;
            next = null;
            random = null;
        }
    }

    public class NodeNext {
        public int val;
        public NodeNext left;
        public NodeNext right;
        public NodeNext? next;

        public NodeNext() { }

        public NodeNext(int _val) {
            val = _val;
        }

        public NodeNext(int _val, NodeNext _left, NodeNext _right, NodeNext _next) {
            val = _val;
            left = _left;
            right = _right;
            next = _next;
        }
    }


    internal class MyStack {

        private Queue<int> que;
        int N;

        public MyStack() {
            que = new();
            N = 0;
        }

        public void Push(int x) {
            que.Enqueue(x);
            N++;
        }

        public int Pop() {
            for (int i = 0; i < N - 1; i++) {
                que.Enqueue(que.Dequeue());
            }
            N--;
            return que.Dequeue();
        }

        public int Top() {
            int tmp = Pop();
            que.Enqueue(tmp);
            return tmp;
        }

        public bool Empty() => N == 0;
    }

    internal class StockSpanner {
        List<int> dp;
        List<int> data;

        public StockSpanner() {
            dp = new() {
                int.MaxValue
            };
            data = new() {
                int.MaxValue,
            };
        }

        public int Next(int price) {
            data.Add(price);
            dp.Add(1);
            if (price >= data[^1]) {
                int idx = dp.Count - 1;
                do {
                    idx -= dp[idx];
                    if (data[idx] <= price) {
                        dp[^1] += dp[idx];
                    } else {
                        break;
                    }
                } while (true);
            }
            return dp[^1]; // return the last
        }
    }

    internal class Trie {
        PrefixTree tree;
        public Trie() {
            tree = new();
        }

        public void Insert(string word) {
            tree.Add(word);
        }

        public bool Search(string word) {
            return tree.Contains(word);
        }

        public bool StartsWith(string prefix) {
            return tree.StartsWith(prefix) > 0;
        }
    }

    internal class Codec {
        List<string> data = new();
        // Encodes a tree to a single string.
        public string SerializePreO(TreeNode root) {
            data.Clear();
            RecursionPreO(root);
            return string.Join(',', data);
        }

        public string SerializePostO(TreeNode root) {
            data.Clear();
            RecursionPostO(root);
            return string.Join(',', data);
        }

        public string SerializeLevelO(TreeNode root) {
            data.Clear();
            Queue<TreeNode> que = new();
            que.Enqueue(root);
            while (que.Count > 0) {
                var tmp = que.Dequeue();
                data.Add(tmp.val.ToString());
                if (tmp.left is null) {
                    data.Add("null");
                } else {
                    que.Enqueue(tmp.left);
                    data.Add(tmp.val.ToString());
                }

                if (tmp.right is null) {
                    data.Add("null");
                } else {
                    que.Enqueue(tmp.right);
                    data.Add(tmp.val.ToString());
                }
            }

            return string.Join(',', data);
        }

        private void RecursionPreO(TreeNode? node) {
            if (node is null) {
                data.Add("null");
            } else {
                data.Add(node.val.ToString());
                RecursionPreO(node.left);
                RecursionPreO(node.right);
            }
        }

        private void RecursionPostO(TreeNode? node) {
            if (node is null) {
                data.Add("null");
            } else {
                RecursionPostO(node.left);
                RecursionPostO(node.right);
                data.Add(node.val.ToString());
            }
        }

        // Decodes your encoded data to tree.
        public static TreeNode? DeserializePreO(string data) {
            var stringArr = data.Split(new char[] { ',' }, options: StringSplitOptions.RemoveEmptyEntries);
            Queue<string> que = new(stringArr);
            if (que.Peek() == "null") {
                return null;
            }
            TreeNode root = new(int.Parse(que.Dequeue()));
            RecursionPreDeserialize(root, que);
            return root;
        }

        public static TreeNode? DeserializePostO(string data) {
            var stringArr = data.Split(new char[] { ',' }, options: StringSplitOptions.RemoveEmptyEntries);
            Stack<string> st = new(stringArr);
            if (st.Peek() == "null") {
                return null;
            }
            TreeNode root = new(int.Parse(st.Pop()));//左右中 中右左
            RecursionPostDeserialize(root, st);
            return root;
        }

        public static TreeNode? DeserializeLevelO(string data) {
            Queue<string> strQue = new(data.Split(new char[] { ',', ' ' }, options: StringSplitOptions.RemoveEmptyEntries));
            if (strQue.Peek() == "null") {
                return null;
            }
            TreeNode root = new(int.Parse(strQue.Dequeue()));
            TreeNode cur1;
            Queue<TreeNode> level = new();
            level.Enqueue(root);
            while (strQue.Count != 0) {
                var tmp = strQue.Dequeue();
                cur1 = level.Dequeue();
                if (tmp == "null") {
                    cur1.left = null;
                } else {
                    cur1.left = new(int.Parse(tmp));
                    level.Enqueue(cur1.left);
                }
                if (strQue.Count == 0) {
                    break;
                }
                tmp = strQue.Dequeue();
                if (tmp == "null") {
                    cur1.right = null;
                } else {
                    cur1.right = new(int.Parse(tmp));
                    level.Enqueue(cur1.right);
                }
            }
            return root;
        }

        private static void RecursionPreDeserialize(TreeNode node, Queue<string> arr) {
            var tmp1 = arr.Dequeue();
            if (tmp1 == "null") {
                node.left = null;
            } else {
                node.left = new(int.Parse(tmp1));
                RecursionPreDeserialize(node.left, arr);
            }
            tmp1 = arr.Dequeue();
            if (tmp1 == "null") {
                node.right = null;
            } else {
                node.right = new(int.Parse(tmp1));
                RecursionPreDeserialize(node.right, arr);
            }
        }

        private static void RecursionPostDeserialize(TreeNode node, Stack<string> arr) {
            var tmp = arr.Pop();
            if (tmp == "null") {
                node.right = null;
            } else {
                node.right = new(int.Parse(tmp));
                RecursionPostDeserialize(node.right, arr);
            }

            tmp = arr.Pop();
            if (tmp == "null") {
                node.left = null;
            } else {
                node.left = new(int.Parse(tmp));
                RecursionPostDeserialize(node.left, arr);
            }
        }
    }

    public class LRUCache {
        private class DoubleLinkedList {
            DoubleNode? head;
            DoubleNode? tail;

            int _count;

            public int Count { get => _count; set => _count = value; }
            public DoubleNode Head { get => head; }
            public DoubleNode Tail { get => tail; }

            public DoubleNode Add(int val, int key) {
                DoubleNode ret;
                if (Count == 0) {
                    ret = head = tail = new(val, key);
                } else {
                    DoubleNode n = new(val, key);
                    ret = head!.next = n;
                    n.previous = head;
                    head = head.next;
                }
                Count++;
                return ret;
            }

            public void RemoveLast() {
                if (Count == 0) {
                    throw new InvalidOperationException("链表已空");
                }
                if (Count == 1) {
                    head = tail = null;
                } else {
                    var old = tail;
                    tail = tail!.next;
                    tail!.previous = null;
                    old!.next = null;
                }
                Count--;
            }

            public void MoveToFirst(DoubleNode node) {
                if (node is null) {
                    throw new ArgumentNullException(nameof(node));
                }
                if (node.next is null) {
                    return;
                } else if (node.previous is null) {
                    tail = node.next;
                    tail.previous = null;
                    node.next = null;
                    //移动到head
                    node.previous = head;
                    head.next = node;
                    head = node;
                } else {
                    node.previous.next = node.next;
                    node.next.previous = node.previous;
                    head.next = node;
                    node.previous = head;
                    node.next = null;
                    head = head.next;
                }
            }
        }

        private class DoubleNode {
            int _value;
            public DoubleNode? previous;
            public DoubleNode? next;
            int _key;

            public int Value { get => _value; set => _value = value; }
            public int Key { get => _key; set => _key = value; }

            public DoubleNode(int value, int key) {
                Value = value;
                Key = key;
            }
        }

        int _capacity;
        Dictionary<int, DoubleNode> dic;
        DoubleLinkedList root;

        public LRUCache(int capacity) {
            _capacity = capacity;
            dic = new();
            root = new();
        }


        public int Get(int key) {
            if (!dic.ContainsKey(key)) {
                return -1;
            }
            var node = dic[key];
            root.MoveToFirst(node);
            return node.Value;
        }

        public void Put(int key, int value) {
            if (dic.ContainsKey(key)) {
                dic[key].Value = value;
                root.MoveToFirst(dic[key]);
            } else {
                if (root.Count < _capacity) {
                    var tmp = root.Add(value, key);
                    dic.Add(key, tmp);
                } else {
                    dic.Remove(root.Tail.Key);
                    root.RemoveLast();
                    Put(key, value);
                }
            }
        }
    }

    public class MagicDictionary {
        private class Node {
            public char value;
            public int end;
            public Dictionary<char, Node> next;

            public Node(char val) {
                value = val;
                next = new Dictionary<char, Node>();
            }

            public override string ToString() => value.ToString();
        }

        Node root;

        public MagicDictionary() {
            root = new(' ');
        }

        public void BuildDict(string[] dictionary) {
            foreach (var word in dictionary) {
                Node cur = root;
                for (int i = 0; i < word.Length; i++) {
                    char c = word[i];
                    if (!cur.next.ContainsKey(c)) {
                        cur.next.Add(c, new(c));
                    }
                    cur = cur.next[c];
                    if (i == word.Length - 1) {
                        cur.end++;
                    }
                }
            }
        }

        public bool Search(string searchWord) {
            return DFS(searchWord, 0, 0, root);
        }

        private bool DFS(string s, int index, int wrong, Node node) {
            if (wrong > 1) { return false; }
            if (index == s.Length) {
                return wrong == 1 && node.end > 0;
            }
            Node cur = node;
            //不管包含还是不包含，都应DFS

            bool res = false;
            foreach (var item in cur.next.Keys) { //start searching
                int times = wrong;
                if (s[index] != item) {
                    times++;
                }
                res = res || DFS(s, index + 1, times, cur.next[item]);
                if (res) { break; }
            }
            return res;
        }
    }

    public class NumMatrix {
        int[,] prefixArea;

        public NumMatrix(int[][] matrix) {
            prefixArea = new int[matrix.Length, matrix[0].Length];
            if (prefixArea.GetLength(0) > 0) {
                prefixArea[0, 0] = matrix[0][0];
                for (int i = 1; i < matrix[0].Length; i++) {
                    prefixArea[0, i] = matrix[0][i] + prefixArea[0, i - 1];
                }
                for (int i = 1; i < matrix.Length; i++) {
                    for (int j = 0; j < matrix[i].Length; j++) {
                        if (j == 0) {
                            prefixArea[i, j] = matrix[i][j] + prefixArea[i - 1, j];
                        } else {
                            prefixArea[i, j] = matrix[i][j] + prefixArea[i, j - 1] + prefixArea[i - 1, j] - prefixArea[i - 1, j - 1];
                        }
                    }
                }
            }
        }

        public int SumRegion(int row1, int col1, int row2, int col2) {
            if (prefixArea.GetLength(0) == 0) {
                return 0;
            }
            if (row1 == 0 && col1 == 0) {
                return prefixArea[row2, col2];
            }
            if (row1 == 0) {
                return prefixArea[row2, col2] - prefixArea[row2, col1 - 1];
            } else if (col1 == 0) {
                return prefixArea[row2, col2] - prefixArea[row1 - 1, col2];
            } else {
                return prefixArea[row2, col2] - prefixArea[row1 - 1, col2] - prefixArea[row2, col1 - 1] + prefixArea[row1 - 1, col1 - 1];
            }
        }
    }

    public class StreamChecker {
        Trie tree;
        StringBuilder stream;

        public StreamChecker(string[] words) {
            tree = new();
            stream = new();
            foreach (var s in words) {
                tree.AddStr(string.Concat(s.Reverse())); //后缀是倒序
            }
        }

        public bool Query(char letter) {
            stream.Append(letter);
            return IsPrefix(tree, stream);
        }

        private bool IsPrefix(Trie tree, StringBuilder str) {
            Trie.Node cur = tree.root;
            for (int i = str.Length - 1; i >= 0; i--) { //倒序检查
                if (cur.child[str[i] - 'a'] is not null) {
                    cur = cur.child[str[i] - 'a'];
                } else if (cur.end == 0) {
                    return false;
                }
                if (cur.end > 0) {
                    return true;
                }
            }
            return cur.end > 0;
        }

        private class Trie {
            public class Node {
                public char val;
                public Node[] child; // 'a' -> child[0]
                public int pass;
                public int end;
                public Node(char val) {
                    this.val = val;
                    this.child = new Node[26];
                }
            }

            public Node root;

            public Trie() {
                this.root = new('\0');
            }

            public void AddStr(string s) {
                Node cur = root;

                for (int i = 0; i < s.Length; i++) {
                    cur.pass++;
                    if (cur.child[s[i] - 'a'] is null) {
                        cur.child[s[i] - 'a'] = new(s[i]);
                    }
                    cur = cur.child[s[i] - 'a'];
                }
                cur.pass++;
                cur.end++;
            }
        }
    }

    internal class MountainArray {
        int[] arr;

        public MountainArray(int[] arr) {
            this.arr = arr;
        }

        public int Get(int index) {
            return arr[index];
        }
        public int Length() { return arr.Length; }
    }

    internal class CBTInserter {
        Queue<TreeNode> que;
        TreeNode root;
        int h1, h2;
        public CBTInserter(TreeNode root) {
            (h1, h2) = Height(root);
            this.root = root;
            que = new();
            Queue<TreeNode> q = new();
            q.Enqueue(root);
            while (q.Count > 0) {
                var tmp = q.Dequeue();
                if (tmp.left is not null) {
                    q.Enqueue(tmp.left);
                    if (tmp.right is not null) {
                        q.Enqueue(tmp.right);
                    } else {
                        que.Enqueue(tmp);
                    }

                } else {
                    que.Enqueue(tmp);
                }
            }
        }

        private (int, int) Height(TreeNode node) {
            int res1 = 0;
            int res2 = 0;
            TreeNode? cur1 = node;
            while (cur1 is not null) {
                res1++;
                cur1 = cur1.left;
            }
            cur1 = node;
            while (cur1 is not null) {
                res2++;
                cur1 = cur1.right;
            }
            return (res1, res2);
        }

        public int Insert(int val) {
            var tmp = que.Peek();
            if (tmp.left is not null) {
                tmp.right = new(val);
                que.Enqueue(tmp.right);
                que.Dequeue();
            } else {
                tmp.left = new(val);
                que.Enqueue(tmp.left);
            }
            return tmp.val;
        }

        public TreeNode Get_root() => root;
    }

    public class LockingTree {
        int[] data;
        int[] user;
        List<int>[] graph;

        public LockingTree(int[] parent) {
            data = parent;
            user = new int[data.Length];
            graph = new List<int>[data.Length];
            for (int i = 0; i < graph.Length; i++) {
                graph[i] = new();
            }
            for (int i = 0; i < parent.Length; i++) {
                if (parent[i] >= 0) {
                    graph[parent[i]].Add(i);
                }
            }
        }

        public bool Lock(int num, int user) {
            if (this.user[num] == 0) {
                this.user[num] = user;
                return true;
            }
            return false;
        }

        public bool Unlock(int num, int user) {
            if (this.user[num] == user) {
                this.user[num] = 0;
                return true;
            }
            return false;
        }

        public bool Upgrade(int num, int user) {
            int cur = num;
            while (cur >= 0) {
                if (this.user[cur] != 0) {
                    return false;
                }
                cur = data[cur];
            }
            if (!Fill(num)) {
                return false;
            }
            this.user[num] = user;
            return true;
        }

        private bool Fill(int pos) {
            if (pos >= user.Length || pos < 0) {
                return false;
            }
            bool res = false;
            if (user[pos] != 0) {
                res = true;
            }
            user[pos] = 0;
            for (int i = 0; i < graph[pos].Count; i++) {
                res |= Fill(graph[pos][i]);
            }
            return res;
        }
    }

    class NumArray {
        SegmentTree tree;

        public NumArray(int[] nums) {
            tree = new(nums);
        }

        public void Update(int index, int val) {
            tree.SetNums(index, index, val);
        }

        public int SumRange(int left, int right) {
            return tree.GetSum(left, right);
        }
    }

    public class RangeModule {
        SegmentTreeDynamic tree;

        public RangeModule() {
            tree = new(1, (int)1e9);
        }

        public void AddRange(int left, int right) {
            tree.Update(left, right, 1);
        }

        public bool QueryRange(int left, int right) {
            return tree.Query(left, right) >= right - left + 1;
        }

        public void RemoveRange(int left, int right) {
            tree.Update(left, right, 0);
        }
    }

    public class CountIntervals {
        SegmentTreeDynamic tree;
        const int N = 1000000000;
        public CountIntervals() {
            tree = new(1, N);
        }

        public void Add(int left, int right) {
            tree.Update(left, right, 1);
        }

        public int Count() {
            return tree.Query(1, N);
        }
    }
}
