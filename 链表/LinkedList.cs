using System.Text;

namespace 链表 {
    public class LinkedList1<T> {
        private class Node//节点
        {
            public T element;
            public Node? next;
            public Node(T e, Node next) {
                this.element = e;
                this.next = next;
            }

            public Node(T e) {
                this.element = e;
                this.next = null;
            }

            public Node() {
                this.element = default;
                this.next = null;
            }

            public override string ToString() => element.ToString();
        }

        Node root, current;

        public LinkedList1(T element) {
            root = new Node(element);
            current = root;
            N = 1;
        }

        public LinkedList1() {
            this.root = new Node();
        }

        int N = 0;
        public int Count {
            get => N;
        }

        public bool IsEmpty { get => Count == 0; }

        public void AddLast(T element) {
            if (N == 0) {
                root = new Node(element);
                current = root;
            } else {
                current.next = new Node(element);
                current = current.next;
            }
            N++;
        }

        public void AddFirst(T element) {
            if (N == 0) {
                root = new Node(element);
                current = root;
            } else {
                Node node = root;
                Node newNode = new(element);
                newNode.next = root;
                root = newNode;
            }
            N++;
        }

        public void RemoveAt(int index) {
            if (index <= 0 || index > N - 1) {
                throw new ArgumentException("索引越界，index最小值只能为1");
            }
            Node newNode = root;
            for (int i = 0; i < index - 1; i++) {
                newNode = newNode.next;
            }
            newNode.next = newNode.next.next;
            if (index == N - 1) {
                current = newNode;
            }
            N--;
        }

        public void RemoveFirst() {
            if (N == 0) {
                throw new Exception("空链表不能删除节点");
            }
            root = root.next;
            N--;
        }

        public int IndexOf(T element) {
            if (N == 0) {
                throw new NullReferenceException();
            }
            Node newNode = root;
            for (int i = 0; i < N; i++) {
                if (newNode.element.Equals(element)) {
                    return i;
                }
                newNode = newNode.next;
            }
            return -1;
        }

        public void Insert(T element, int index) {
            if (index <= 0 || index > N - 1) {
                throw new ArgumentException("索引越界，index最小值只能为1");
            }
            Node node = root;
            Node newNode = new Node(element);//新节点
            //先找到index上一级的node
            for (int i = 0; i < index - 1; i++) {
                node = node.next;
            }
            newNode.next = node.next;
            node.next = newNode;
            if (index == N - 1) {
                current = node.next;
            }
            N++;
        }

        public void Set(int index, T element) {
            if (index < 0 || index > N - 1) {
                throw new ArgumentException("索引越界");
            }

            Node node = root;
            for (int i = 0; i < index; i++) {
                node = node.next;
            }

            node.element = element;
        }

        public T GetValue(int index) {
            if (index < 0 || index > N - 1) {
                throw new ArgumentException("索引越界");
            }

            Node node = root;
            for (int i = 0; i < index; i++) {
                node = node.next;
            }
            return node.element;
        }

        public T GetLast() => this.current.element;

        public T GetFirst() => this.root.element;

        public bool Contains(T element) => this.IndexOf(element) != -1;

        public void Reverse() {
            if (N == 1) {
                return;
            }
            Node head, mid, next;
            head = root;
            mid = root.next;
            Node tmp = head;//指向反转后的最后一位
            next = mid.next;//-1 -2 9 8 8
            //开始
            head.next = null;//将head变一般化，即与下一位不相连
            for (int i = 0; i < N - 1; i++) {
                mid.next = head;

                head = mid;
                mid = next;
                next = next?.next;
            }
            current = tmp;
            root = head;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder(String.Format("此链表信息：Count = {0}\n", Count));
            Node node = root;
            for (int i = 0; i < N - 1; i++) {
                sb.Append(node.element.ToString() + ", ");
                node = node.next;
            }
            sb.Append(node.element.ToString());
            return sb.ToString();
        }
    }
}
