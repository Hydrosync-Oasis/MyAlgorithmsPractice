using System.Diagnostics;
using 堆;
using 树;

namespace 图 {
    internal class Graph<T> where T : notnull {
        public Dictionary<T, Node<T>> Nodes { get; set; }
        public List<Edge<T>> Edges { get; set; }

        public Graph() {
            Nodes = new();
            Edges = new();
        }

        /// <summary>
        /// 注意：非邻接矩阵！！！矩阵为N * 2大小，记录了from to，可将矩阵转换为图类型
        /// </summary>
        /// <param name="mat">N * 2的矩阵 [from, to]</param>
        /// <param name="weights">每个连接的权重</param>
        public Graph(T[][] mat, int[] weights) : this() {
            //weight from to
            int i = 0;
            foreach (var item in mat) {
                Node<T> from = Nodes.ContainsKey(item[0]) ? Nodes[item[0]] : new(item[0]);
                Node<T> to = Nodes.ContainsKey(item[1]) ? Nodes[item[1]] : new(item[1]);
                from.Nexts.Add(to);

                Nodes.TryAdd(item[0], from);
                Nodes.TryAdd(item[1], to);

                from.Out++;
                to.In++;

                Edge<T> edge = new(weights[i++], from, to);
                Edges.Add(edge);
                from.Edges.Add(edge);
            }
        }

        public static IList<T> BFS(Node<T> root) {
            Queue<Node<T>> que = new();//队列中仅有新进元素
            HashSet<Node<T>> set = new();
            que.Enqueue(root);
            set.Add(root);
            List<T> res = new();
            while (que.Count != 0) {
                var cur = que.Dequeue();
                res.Add(cur.Val);
                foreach (var item in cur.Nexts) {
                    if (set.Add(item)) {
                        que.Enqueue(item);
                    }
                }
            }
            return res;
        }

        public static IList<T> DFS(Node<T> root) {
            Stack<Node<T>> st = new();//只有新节点
            HashSet<Node<T>> set = new();
            List<T> res = new();
            st.Push(root);
            set.Add(root);
            res.Add(root.Val);//不能在pop时加，因为有可能pop后还会push导致重复
            while (st.Count != 0) {
                var cur = st.Pop();
                foreach (var item in cur.Nexts) {
                    if (set.Add(item)) {
                        st.Push(cur);
                        st.Push(item);
                        res.Add(item.Val);
                        break;
                    }
                }
            }
            return res;
        }

        public IList<T> TopologicalSort() {
            Queue<Node<T>> que = new();
            var nodes = Nodes.Values;
            Dictionary<Node<T>, int> inMap = new();
            foreach (var item in nodes) {
                inMap[item] = item.In;
            }
            foreach (Node<T>? item in nodes) {
                if (inMap[item] == 0) {
                    que.Enqueue(item);
                }
            }
            List<T> res = new();
            while (que.Count != 0) {
                var cur = que.Dequeue();
                res.Add(cur.Val);
                foreach (var item in cur.Nexts) {
                    inMap[item]--;
                    if (inMap[item] == 0) {
                        que.Enqueue(item);
                    }
                }
            }
            foreach (var item in inMap) {
                if (item.Value != 0) {
                    throw new InvalidOperationException("检测到循环");
                }
            }
            return res;
        }

        public IList<Edge<T>> KruskalTree() {
            UnionFindSet<Node<T>> set = new();
            List<Edge<T>> edges = Edges.ToList();
            edges.Sort((a, b) => {
                return a.Weight.CompareTo(b.Weight);
            });
            set.AddRange(Nodes.Values.ToList());
            List<Edge<T>> res = new();
            foreach (Edge<T> e in edges) {
                var from = e.From;
                var to = e.To;
                if (set.Union(from, to)) {
                    res.Add(e);
                }
            }
            return res;
        }

        public IList<Edge<T>> PrimTree() {
            PriorityQueue<Edge<T>, int> edgeQue = new();//int 是权重
            HashSet<Node<T>> nodeSet = new();
            HashSet<Edge<T>> edgeSet = new();
            List<Edge<T>> res = new();

            foreach (var item in Nodes) {//随机选择一个点
                if (nodeSet.Add(item.Value)) {
                    foreach (var e in item.Value.Edges) {
                        edgeQue.Enqueue(e, e.Weight);
                        //edgeSet.Add(e);
                    }
                    while (edgeQue.Count != 0) {
                        var minE = edgeQue.Dequeue();
                        if (edgeSet.Add(minE)) {
                            if (nodeSet.Add(minE.To)) {
                                res.Add(minE);//找到了最优的边
                                //解锁To节点的边
                                foreach (var e in minE.To.Edges) {
                                    edgeQue.Enqueue(e, e.Weight);
                                }
                            }
                        }
                    }
                }
                break;
            }
            return res;
        }

        public Dictionary<Node<T>, long> DijkstraMinEdge(T startNode) {
            Node<T> cur = Nodes[startNode];
            const long Infinity = long.MaxValue;
            HashSet<Node<T>> history = new();
            Heap<Node<T>, long> heap = new((a, b) => b.CompareTo(a));
            foreach (var item in Nodes) {
                heap.Push(item.Value, Infinity);
            }
            heap[cur] = 0;
            //history.Add(cur);
            while (true) {
                foreach (var item in cur.Edges) {
                    var to = item.To;
                    heap[to] = Math.Min(heap[to], item.Weight + heap[item.From]);
                }
                if (heap.Count > 0) {
                    cur = heap.Pop();//选出来了最近的边
                } else {
                    break;
                }
                //history.Add(cur);
            }
            return heap.ToDictionary();
        }

        /// <summary>
        /// 生成一个二元关系对应的邻接矩阵，并返回矩阵坐标和集合元素的对应关系
        /// </summary>
        /// <typeparam name="Type">集合元素的类型</typeparam>
        /// <param name="connectedPoint">二元关系</param>
        /// <returns>返回一个邻接矩阵，和坐标与点的map</returns>
        public static (int[,], Type[]) GenerateAdjacentMat<Type>(Type[][] connectedPoint) where Type : notnull {
            Dictionary<Type, int> mapIdx = new();

            int count = 0;
            for (int i = 0; i < connectedPoint.Length; i++) { //建立元素和坐标的对应关系
                if (mapIdx.TryAdd(connectedPoint[i][0], count)) {
                    count++;
                }
                if (mapIdx.TryAdd(connectedPoint[i][1], count)) {
                    count++;
                }
            }

            int[,] adjc = new int[mapIdx.Count, mapIdx.Count]; //生成矩阵
            for (int i = 0; i < connectedPoint.Length; i++) {
                adjc[mapIdx[connectedPoint[i][0]], mapIdx[connectedPoint[i][1]]] = 1;
            }

            Type[] res = new Type[mapIdx.Count];
            foreach (var item in mapIdx) {
                res[item.Value] = item.Key;
            }
            return (adjc, res);
        }

        public static int[,] GenerateAdjacentMat(int[][] connectedPoint) {
            int N = 0;
            for (int i = 0; i < connectedPoint.Length; i++) {
                N = Math.Max(N, connectedPoint[i][0]);
                N = Math.Max(N, connectedPoint[i][1]);
            }

            int[,] adjc = new int[N + 1, N + 1]; //生成矩阵
            for (int i = 0; i < connectedPoint.Length; i++) {
                adjc[connectedPoint[i][0], connectedPoint[i][1]] = 1;
            }

            return adjc;
        }

        public static Graph<int> GenGraphByAdjMat(int[,] mat) {
            Graph<int> graph = new Graph<int>();
            for (int i = 0; i < mat.GetLength(0); i++) {
                for (int j = 0; j < mat.GetLength(1); j++) {
                    if (mat[i, j] == 1) {
                        Node<int> from = new(i);
                        Node<int> to = new(i);
                        graph.Nodes.TryAdd(i, from);
                        from.Nexts.Add(to);
                        graph.Nodes.TryAdd(j, to);

                        Edge<int> edge = new(1, from, to);
                        graph.Edges.Add(edge);
                        from.Edges.Add(edge);

                        from.Out++;
                        to.In++;
                    }
                }
            }
            return graph;
        }
    }

    class Node<T> {
        public List<Node<T>> Nexts { get; set; }
        public List<Edge<T>> Edges { get; set; }
        public T Val { get; set; }
        public int In { get; set; }
        public int Out { get; set; }

        public Node(T val) {
            Val = val;
            Nexts = new();
            Edges = new();
        }

        public override string ToString() => $"{In} == {Val} => {Out}";
    }

    class Edge<T> {
        public int Weight { get; set; }
        public Node<T> From { get; set; }
        public Node<T> To { get; set; }

        public Edge(int weight, Node<T> from, Node<T> to) {
            Weight = weight;
            From = from;
            To = to;
        }

        public override string ToString() => $"{From.Val} ----({Weight})----> {To.Val}";
    }
}
