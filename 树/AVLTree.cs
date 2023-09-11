using System.Collections;
using System.Runtime.CompilerServices;

namespace 树 {
    internal class AVLTree<T> : IEnumerator<T> where T : IComparable {
        public class Node {
            public T value;
            public Node? leftChild;
            public Node? rightChild;
            public (Node, bool) parent;//是否是左树
            public int height;

            public Node(T value) {
                this.value = value;
            }
        }

        Node root;//虚拟节点
        public int Count { get => _count; }

        private int _count;

        public int Height => root.leftChild is null ? 0 : root.leftChild.height;

        public T Current => _current.value;

        private Node _current;

        object IEnumerator.Current => _current;

        public AVLTree() {
            root = new(default!);

            traversalStack = new();
            traversalStack.Push((root, 0));
        }

        private bool Less(Node a, Node b) {
            if (a == root) {
                return false;
            }
            if (b == root) {
                return true;
            }
            return a.value.CompareTo(b.value) < 0;
        }

        private int Compare(Node a, Node b) {
            if (a == b) {
                return 0;
            }
            if (a == root) {
                return 1;
            }
            if (b == root) {
                return -1;
            }
            return a.value.CompareTo(b.value);
        }

        public void Add(T value) {
            Node cur = root!;
            Node newNode = new(value);
            while (true) {
                if (Less(cur, newNode)) {
                    if (cur.rightChild is null) {
                        cur.rightChild = newNode;
                        newNode.parent = (cur, false);
                        cur = cur.rightChild;
                        break;
                    }
                    cur = cur.rightChild;
                } else {
                    if (cur.leftChild is null) {
                        cur.leftChild = newNode;
                        newNode.parent = (cur, true);
                        cur = cur.leftChild;
                        break;
                    }
                    cur = cur.leftChild;
                }
            }
            _count++;
            //cur是新节点
            UpdateHeight(cur, 1);//设置底部高度是1
        }

        public Node? FindNode(T value) {
            Node? cur = root;
            Node tmp = new(value);
            while (cur is not null && Compare(cur, tmp) != 0) {
                if (Compare(tmp, cur) > 0) {
                    cur = cur.rightChild;
                } else {
                    cur = cur.leftChild;
                }
            }

            return cur;
        }

        public bool Contains(T value) => FindNode(value) is not null;

        public void Remove(T value) {
            //分情况：无子节点、有左树、有右树和左右树均有
            Node? cur = FindNode(value) ?? throw new InvalidOperationException("不存在节点");
            //找到了
            var parent = cur.parent;
            if (cur.leftChild is null && cur.rightChild is null) { //无子树
                if (parent.Item2) {
                    parent.Item1.leftChild = null;
                } else {
                    parent.Item1.rightChild = null;
                }
                UpdateHeight(parent.Item1, Math.Max(parent.Item1.leftChild is null ? 0 : parent.Item1.leftChild.height,
                    parent.Item1.rightChild is null ? 0 : parent.Item1.rightChild.height) + 1);
            } else if (cur.leftChild is not null && cur.rightChild is null) { //仅有左树
                if (parent.Item2) {
                    cur.parent.Item1.leftChild = cur.leftChild;
                    cur = cur.parent.Item1;
                    cur.leftChild!.parent = (cur, true);
                    cur = cur.leftChild;
                } else {
                    cur.parent.Item1.rightChild = cur.leftChild;
                    cur = cur.parent.Item1;
                    cur.rightChild!.parent = (cur, false);
                    cur = cur.rightChild;
                }
                UpdateHeight(cur, cur.height);
            } else { //有右树
                var mostLeft = cur.rightChild;
                while (mostLeft!.leftChild is not null) { mostLeft = mostLeft.leftChild; }
                (cur.value, mostLeft.value) = (mostLeft.value, cur.value);
                parent = mostLeft.parent;

                if (parent.Item2) { //删除后继节点
                    //后继节点的右树找到后继节点的父
                    Node? tmp = mostLeft.rightChild;
                    mostLeft.parent.Item1.leftChild = tmp;
                    if (tmp is not null) {
                        tmp.parent = (mostLeft.parent.Item1, true);
                    }
                    //parent.Item1.leftChild = null;
                } else {
                    Node? tmp = mostLeft.rightChild;
                    mostLeft.parent.Item1.rightChild = tmp;
                    if (tmp is not null) {
                        tmp.parent = (mostLeft.parent.Item1, false);
                    }
                    //parent.Item1.rightChild = null;
                }

                UpdateHeight(mostLeft.parent.Item1, Math.Max(
                    mostLeft.parent.Item1.leftChild is null ? 0 : mostLeft.parent.Item1.leftChild.height,
                    mostLeft.parent.Item1.rightChild is null ? 0 :
                    mostLeft.parent.Item1.rightChild.height) + 1);

            }
            _count--;
        }

        private void UpdateHeight(Node? node, int setValue) {
            if (node is null || node == root) {
                return;
            }
            Node cur = node.parent.Item1;
            node.height = setValue;
            while (cur is not null && cur != root) {
                //cur.height = Math.Max(cur.leftChild == null ? 0 : cur.leftChild.height, cur.rightChild == null ? 0 : cur.rightChild.height) + 1;

                int leftH = cur.leftChild is null ? 0 : cur.leftChild.height;
                int rightH = cur.rightChild is null ? 0 : cur.rightChild.height;

                if (leftH - rightH == 2) {
                    if ((cur.leftChild!.leftChild is null ? 0 : cur.leftChild!.leftChild.height) >
                        (cur.leftChild!.rightChild is null ? 0 : cur.leftChild!.rightChild.height)) { //LL
                        AVLTree<T>.RightRotate(cur!.leftChild!);
                    } else { //LR
                        AVLTree<T>.LeftRotate(cur.leftChild!.rightChild!);
                        AVLTree<T>.RightRotate(cur!.leftChild!);
                    }

                } else if (rightH - leftH == 2) {
                    if ((cur.rightChild!.leftChild is null ? 0 : cur.rightChild!.leftChild.height) <
                        (cur.rightChild!.rightChild is null ? 0 : cur.rightChild!.rightChild.height)) { //RR
                        AVLTree<T>.LeftRotate(cur.rightChild!);
                    } else { //RL
                        AVLTree<T>.RightRotate(cur.rightChild!.leftChild!);
                        AVLTree<T>.LeftRotate(cur!.rightChild!);
                    }
                }

                if (cur.height != Math.Max(cur.leftChild is null ? 0 : cur.leftChild.height, cur.rightChild is null ? 0 : cur.rightChild.height) + 1) 
                    {

                    cur.height = Math.Max(cur.leftChild is null ? 0 : cur.leftChild.height,
                        cur.rightChild is null ? 0 : cur.rightChild.height) + 1;
                    cur = cur.parent.Item1;//不断往上
                } else {
                    break;
                }

            }
        }

        private static void RightRotate(Node node) {
            Node? right = node.rightChild;
            var par = node.parent.Item1;
            if (par is null) {
                throw new InvalidOperationException("旋转的子树无父节点");
            }
            var backup = par.parent;
            node.rightChild = par;
            par.parent = (node, false);
            par.leftChild = right;
            if (right is not null) {
                right.parent = (par, true);
            }

            if (backup.Item1 is not null) {
                if (backup.Item2) {
                    backup.Item1.leftChild = node;
                    node.parent = (backup.Item1, true);
                } else {
                    backup.Item1.rightChild = node;
                    node.parent = (backup.Item1, false);
                }
            }

            node.rightChild.height =
                Math.Max(node.rightChild.leftChild is null ? 0 : node.rightChild.leftChild.height,
                node.rightChild.rightChild is null ? 0 : node.rightChild.rightChild.height) + 1;

        }

        private static void LeftRotate(Node node) {
            Node par = node.parent.Item1;
            if (par is null) {
                throw new InvalidOperationException("旋转的子树无父节点");
            }
            Node? left = node.leftChild;
            var backup = node.parent.Item1.parent;
            node.leftChild = par;
            par.parent = (node, true);
            par.rightChild = left;
            if (left is not null) {
                left.parent = (par, false);
            }
            if (backup.Item1 is not null) {
                if (backup.Item2) {
                    backup.Item1.leftChild = node;
                    node.parent = (backup.Item1, true);
                } else { //3987110
                    backup.Item1.rightChild = node;
                    node.parent = (backup.Item1, false);
                }
            }

            node.leftChild.height =
                Math.Max(node.leftChild.leftChild is null ? 0 : node.leftChild.leftChild.height,
                node.leftChild.rightChild is null ? 0 : node.leftChild.rightChild.height) + 1;
        }

        public void Clear() {
            _count = 0;
            root.leftChild = null;
        }

        private Stack<(Node, int)> traversalStack;//左树是否被遍历完，0=左树未完，1=左树遍历完，2=右树遍历完

        public bool MoveNext() {
            while (traversalStack.Count > 0) {
                var cur = traversalStack.Pop();
                if (cur.Item2 == 0) {
                    traversalStack.Push(new(cur.Item1, 1));
                    if (cur.Item1.leftChild is not null) {
                        traversalStack.Push((cur.Item1.leftChild, 0));
                    }
                } else if (cur.Item2 == 1) {
                    traversalStack.Push(new(cur.Item1, 2));
                    if (cur.Item1.rightChild is not null) {
                        traversalStack.Push((cur.Item1.rightChild, 0));
                    }
                    _current = cur.Item1;//更新了现存值
                    break;
                }
            }
            return traversalStack.Count > 0 && _current != root;
        }

        public void Reset() {
            traversalStack = new();
            traversalStack.Clear();

            traversalStack.Push((root, 0));
        }

        public void Dispose() {
            Reset();
        }

        public IEnumerator<T> GetEnumerator() {
            return this;
        }
    }
}
