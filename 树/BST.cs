namespace 树
{
    public class BST<T> where T : IComparable
    {
        private class Node
        {
            public T val;
            public Node left;
            public Node right;

            public Node(T element, Node left, Node right)
            {
                this.val = element;
                this.left = left;
                this.right = right;
            }

            public Node(T element) : this(element, null, null)
            {

            }

            public bool IsLeaf() => left is null && right is null;

            public override string ToString()
            {
                string? left, right;
                left = this.left is null ? "" : this.left.val.ToString();
                right = this.right is null ? "" : this.right.val.ToString();
                return string.Format("{0}<-{1}->{2}", left, this.val, right);
            }
        }

        Node root;
        int N = 0;

        public BST(T element)
        {
            this.root = new Node(element);
            N++;
        }

        public void Add(T element)
        {
            RecursionAdd(new Node(element), root);
            N++;
        }

        public void Add(T[] element)
        {
            foreach (var item in element)
            {
                RecursionAdd(new Node(item), root);
                N++;
            }
        }

        public BST(params T[] values)
        {
            root = new(values[0]);
            N++;
            for (int i = 1; i < values.Length; i++)
            {
                Add(values[i]);
            }
        }

        private void RecursionAdd(Node newNode, Node currentNode)
        {
            if (newNode == null)
            {
                return;
            }
            if (newNode.val.CompareTo(currentNode.val) <= 0)
            {
                if (currentNode.left is null)
                {
                    currentNode.left = newNode;
                }
                else
                {
                    RecursionAdd(newNode, currentNode.left);
                }
            }
            else if (newNode.val.CompareTo(currentNode.val) > 0)
            {
                if (currentNode.right is null)
                {
                    currentNode.right = newNode;
                }
                else
                {
                    RecursionAdd(newNode, currentNode.right);
                }
            }
        }

        public int Count => N;

        public bool IsEmpty => N == 0;

        public void Remove(T element)
        {
            if (N < 1)
            {
                throw new InvalidOperationException("二叉树是空的.");
            }
            if (element.Equals(root.val))
            {//判断是否删除根节点
                Node leftNode = root.left;
                Node rightNode = root.right;
                root = rightNode;
                RecursionAdd(leftNode, root);
                return;
            }

            RecursionRemove(element, root);

            N--;
        }

        private void RecursionRemove(T element, Node currentNode)
        {
            try
            {
                Node? wannaDelete = null;
                bool isLeft = false;
                if (currentNode.left.val.Equals(element))//目的是提前看自己的左右孩子是否是目标值，这样保存上一次的节点"node"方便令其孩子为null
                {
                    wannaDelete = currentNode.left;
                    isLeft = true;
                }
                else if (currentNode.right.val.Equals(element))
                {
                    wannaDelete = currentNode.right;
                    isLeft = false;
                }
                //如果左右孩子不是目标值那么下面的代码也不执行
                if (wannaDelete is not null)
                {//找到目标值并已存储了该目标值的父亲节点
                    if (isLeft)//开始删除节点
                    {
                        Node tmp = currentNode.left.left;
                        currentNode.left = currentNode.left.right;
                        RecursionAdd(tmp, currentNode.left);
                    }
                    else
                    {
                        Node tmp = currentNode.right.right;
                        currentNode.right = currentNode.right.left;
                        RecursionAdd(tmp, currentNode.right);
                    }
                    //删除完节点，返回
                    return;

                }
                //递归开始
                if (currentNode.val.CompareTo(element) >= 0)
                {
                    RecursionRemove(element, currentNode.left);
                }
                else
                {
                    RecursionRemove(element, currentNode.right);
                }
            }
            catch (Exception)//截获异常等价于元素未能找到
            {
                throw new InvalidOperationException("二叉树中无该元素");
            }

        }

        public bool Contains(T element)
        {
            RecursionFind(element, root);
            return _returnValue;
        }

        private bool _returnValue;

        private void RecursionFind(T element, Node currentNode)
        {
            if (currentNode is null)
            {
                _returnValue = false;
                return;
            }
            if (currentNode.val.Equals(element))
            {
                _returnValue = true;
                return;
            }
            if (element.CompareTo(currentNode.val) <= 0)
            {
                RecursionFind(element, currentNode.left);
            }
            else
            {
                RecursionFind(element, currentNode.right);
            }
        }

        public void LoopAdd(T element)
        {
            Node father = root;
            //Node child;//此变量存在的意义是判断当前节点的孩子节点是否为空
            while (true)
            {
                if (element.CompareTo(father.val) <= 0)
                {
                    if (father.left is null)//找不动了，添加元素
                    {
                        father.left = new Node(element);
                        return;
                    }
                    father = father.left;
                }
                else
                {
                    if (father.right is null)//找不动了，添加元素
                    {
                        father.right = new Node(element);
                        return;
                    }
                    father = father.right;
                }
            }
        }

        public bool LoopFind(T element)
        {
            Node father = root;
            //Node child;//此变量存在的意义是判断当前节点的孩子节点是否为空
            //bool isLeft = false;
            while (true)
            {
                if (element.CompareTo(father.val) <= 0)
                {
                    father = father.left;
                    //isLeft = true;
                }
                else
                {
                    father = father.right;
                    //isLeft = false;
                }

                if (father is null)//空即不存在该值
                {
                    return false;
                }
                if (father.val.Equals(element))
                {
                    return true;
                }
            }

        }

        public int GetLevel()
        {
            if (IsEmpty)
            {
                _level = 0;
            }
            else
            {
                _level = 1;
                TraverseDown(root);
            }

            return _level;
        }

        private int _level;

        private void TraverseDown(Node node)
        {
            if (node.left is not null)
            {
                TraverseDown(node.left);
                _level++;
            }
            else if (node.right is not null)
            {
                TraverseDown(node.right);
                _level++;
            }
            else
            {
                return;
            }
        }

        public int MethodCount() => RecursionGetCount(root);

        private int RecursionGetCount(Node node)
        {
            //终止条件
            if (node is null)
            {
                return 0;
            }
            else if (node.IsLeaf())
            {
                return 1;
            }
            else
            {//不要忘加自己
                return RecursionGetCount(node.left) + RecursionGetCount(node.right) + 1;
            }
        }
        /// <summary>
        /// 迭代法前序遍历
        /// </summary>
        /// <returns></returns>
        public T[] IteratePreOrder()
        {
            Node thisNode = root;
            Stack<Node> stck = new();
            stck.Push(thisNode);
            List<T> values = new(N);

            while (stck.Count != 0)
            {
                thisNode = stck.Pop();
                if (thisNode is null)
                {
                    //跳过，再弹node
                    continue;
                }
                values.Add(thisNode.val);
                stck.Push(thisNode.right);
                stck.Push(thisNode.left);
            }

            return values.ToArray();
        }
        /// <summary>
        /// 递归法前序遍历
        /// </summary>
        /// <returns></returns>
        public T[] PreOrder()
        {
            preorder.Clear();
            RecursionPreOrder(root);
            return preorder.ToArray();
        }

        public T[] InOrder()
        {
            preorder.Clear();
            RecursionPreOrder(root);
            return preorder.ToArray();
        }

        List<T> preorder = new();

        private void RecursionPreOrder(Node node)
        {
            if (node is null)
            {
                return;
            }
            preorder.Add(node.val);
            RecursionPreOrder(node.left);
            RecursionPreOrder(node.right);
        }

        private void RecursionInOrder(Node node)
        {
            preorder.Clear();
            if (node is null)
            {
                return;
            }
            RecursionInOrder(node.left);
            preorder.Add(node.val);
            RecursionInOrder(node.right);
        }

        public T[] IterateSeqOrder()
        {
            Queue<Node> nodes = new();
            Node thisNode = root;
            nodes.Enqueue(thisNode);
            List<T> values = new();
            while (true)
            {
                thisNode = nodes.Dequeue();
                values.Add(thisNode.val);

                if (thisNode.left is not null)
                {
                    nodes.Enqueue(thisNode.left);
                }
                if (thisNode.right is not null)
                {
                    nodes.Enqueue(thisNode.right);
                }
                if (nodes.Count == 0)
                {
                    break;
                }
            }
            return values.ToArray();
        }
        /// <summary>
        /// 取二叉树的最小层数
        /// </summary>
        /// <returns></returns>
        public int GetMinLevel()
        {
            Queue<Node> nodes = new();
            nodes.Enqueue(root);
            List<Node> levelNodes = new();
            int i = 0;
            while (true)
            {
                while (nodes.Count != 0)
                {
                    levelNodes.Add(nodes.Dequeue());//一层的节点
                }
                i++;
                foreach (var item in levelNodes)
                {
                    if (item.IsLeaf())
                    {
                        return i;
                    }
                    if (item.left is not null)
                    {
                        nodes.Enqueue(item.left);
                    }
                    if (item.right is not null)
                    {
                        nodes.Enqueue(item.right);
                    }
                }
            }
        }

        //全新的挑战属于是
        public T[] IterateInOrder()
        {
            Node thsNode = root;
            Stack<Node> stck = new();
            List<T> values = new(N);
            while (stck.Count != 0 || thsNode is not null)
            {
                if (thsNode is not null)
                {
                    stck.Push(thsNode);
                    thsNode = thsNode.left;
                }
                else//null
                {
                    thsNode = stck.Pop();
                    values.Add(thsNode.val);
                    thsNode = thsNode.right;
                }
            }

            return values.ToArray();
        }

        /// <summary>
        /// 一通百通，二叉树非递归遍历通解，此为前序遍历
        /// </summary>
        /// <returns></returns>
        public T[] IteratePreOrder3()
        {
            Node thsNode = root;
            Stack<Node> stck = new();
            List<T> values = new(N);
            stck.Push(thsNode);
            HashSet<Node> used = new();//记录是否被遍历过一次
            while (stck.Count != 0)
            {
                thsNode = stck.Pop();
                if (thsNode is null)
                {
                    continue;
                }
                if (thsNode.IsLeaf())
                {
                    values.Add(thsNode.val);
                    continue;
                }

                if (!used.Contains(thsNode))
                {//首次遍历到一个节点先把节点的左右孩子全部加入栈中
                    stck.Push(thsNode.right);
                    
                    stck.Push(thsNode.left);
                    stck.Push(thsNode);
                    used.Add(thsNode);
                }
                else
                {
                    values.Add(thsNode.val);
                }
            }

            return values.ToArray();
        }
    }
}
