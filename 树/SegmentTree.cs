using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace 树 {
    public class SegmentTree {
        int[] sum;
        int[] data;
        int[] lazy;// lazy信息为单位每个元素增加多少数值，需要乘以对应范围长度
        int[] update; // 某个范围内的元素全部更新为一个定值
        // update 和 sum独立，lazy和sum需要相互搭配
        bool[] hasUpdt;

        public int Count { get; private set; }

        public SegmentTree(int[] data) {
            this.data = new int[data.Length + 1];
            Array.Copy(data, 0, this.data, 1, data.Length);
            sum = new int[this.data.Length << 2 | 1]; //四倍空间肯定够用，用于存放[x..y]求和的值
            lazy = new int[sum.Length]; //四倍空间肯定够用
            update = new int[sum.Length]; //四倍空间肯定够用
            hasUpdt = new bool[update.Length];
            Count = data.Length;

            Build(1, data.Length, 1);
        }

        private void Build(int from, int to, int position) {
            if (from == to) { //[n..n]单节点直接统计
                if (from < data.Length) {
                    sum[position] = data[from];
                }
                return;
            }
            int mid = (from + to) >> 1;
            Build(from, mid, position << 1);
            Build(mid + 1, to, position << 1 | 1);
            sum[position] = sum[position << 1] + sum[position << 1 | 1];
        }

        public void Add(int left, int right, int val) {
            Add2(left + 1, right + 1, 1, Count, val, 1);
        }

        private void Add2(int left, int right, int l, int r, int val, int rt) {
            if (left > r || right < l) {
                return;
            }
            if (left <= l && right >= r) { //l r是实际的线段树中的区间
                lazy[rt] += val;//只有这个大区间有懒信息，下面的子节点都没加过
                return;
            }
            if (lazy[rt] != 0) {
                LazyPush(rt, l, r);
            }
            int m = (l + r) >> 1;
            Add2(left, right, l, m, val, rt << 1);
            Add2(left, right, m + 1, r, val, rt << 1 | 1);
        }

        private void LazyPush(int rt, int l, int r) {
            //本节点的sum值已经加过lazy了，子节点没加过lazy
            if (l == r) {
                return;
            }
            lazy[rt << 1] += lazy[rt];
            lazy[(rt << 1) | 1] += lazy[rt];
            int m = (l + r) >> 1;
            sum[rt] += lazy[rt] * (r - l + 1);
            lazy[rt] = 0;//本节点lazy已下放完毕，清空lazy值

            if (hasUpdt[rt]) {
                update[rt << 1] = update[rt];
                update[(rt << 1) | 1] = update[rt];
                sum[rt] = (r - l + 1) * update[rt];
                sum[rt << 1] = (m - l + 1) * update[rt];
                sum[(rt << 1) | 1] = (r - m + 1) * update[rt];
                lazy[rt << 1] = 0;
                lazy[(rt << 1) | 1] = 0;
                update[rt] = 0;
                hasUpdt[rt] = false;
                hasUpdt[rt << 1] = true;
                hasUpdt[rt << 1 | 1] = true;
            }
        }

        public int GetSum(int left, int right) {
            return Query(left + 1, right + 1, 1, Count, 1);
        }

        private int Query(int left, int right, int l, int r, int rt) {
            // rt: 节点索引
            if (left > r || right < l) {
                return 0;
            }

            if (left <= l && right >= r) {
                return sum[rt] + lazy[rt] * (r - l + 1);
            }
            LazyPush(rt, l, r);
            int m = (l + r) >> 1;
            return Query(left, right, l, m, rt << 1) +
                Query(left, right, m + 1, r, rt << 1 | 1);
        }

        public void SetNums(int left, int right, int val) {
            Update(left + 1, right + 1, 1, Count, val, 1);
        }

        private void Update(int left, int right, int l, int r, int value, int rt) {
            if (left > r || right < l) {
                return;
            }

            if (left <= l && right >= r) {
                update[rt] = value;
                sum[rt] = value * (r - l + 1);
                lazy[rt] = 0;
                hasUpdt[rt] = true;
                Up(rt >> 1);
                return;
            }
            LazyPush(rt, l, r);
            int m = (l + r) >> 1;
            Update(left, right, l, m, value, rt << 1);
            Update(left, right, m + 1, r, value, rt << 1 | 1);
        }

        private void Up(int rt) {
            if (rt == 0) {
                return;
            }
            sum[rt] = sum[rt << 1] + sum[rt << 1 | 1];
            Up(rt >> 1);
        }
    }

    public class SegmentTreeDynamic(int l, int r) {
        private class Node {
            public int Left = -1;
            public int Right;

            public int Length { get => Right - Left + 1; }
            /// <summary>
            /// 懒更新，将区间的所有数替换为某个固定的数，必须使update和sum同时被维护
            /// </summary>
            public int LazyUpdate;
            /// <summary>
            /// 是否被覆盖，必须使update和sum同时被维护
            /// </summary>
            public bool HasUpdate = false;
            /// <summary>
            /// 标记每个元素增加了多少，LazySum与Sum互相搭配，可以不同时维护
            /// </summary>
            public int LazySum;
            public int Sum;

            public Node? LeftNode { get; set; }

            public Node? RightNode { get; set; }

            public bool IsLeaf => LeftNode is null && RightNode is null;

            public Node(int l, int r) {
                Left = l;
                Right = r;
            }
        }

        Node root = new(l, r);

        public void Update(int l, int r, int val) {
            Update2(l, r, val, root);
        }

        private static void PushDown(Node cur) {
            AddChild(cur);
            if (cur.HasUpdate) {
                cur.HasUpdate = false;
                cur.Sum = cur.LazyUpdate * cur.Length;

                cur.LeftNode!.HasUpdate = cur.RightNode!.HasUpdate = true;
                cur.LeftNode.LazyUpdate = cur.RightNode.LazyUpdate = cur.LazyUpdate;
                cur.LeftNode.Sum = cur.LeftNode.LazyUpdate * cur.LeftNode.Length;
                cur.RightNode.Sum = cur.RightNode.LazyUpdate * cur.RightNode.Length;
            }
            // 有懒更新信息
            cur.Sum += cur.LazySum * cur.Length;
            cur.LeftNode!.LazySum += cur.LazySum;
            cur.RightNode!.LazySum += cur.LazySum;
            cur.LazySum = 0;
        }

        private static void PushUp(Node node) {
            if (!node.IsLeaf) {
                node.Sum = node.LeftNode!.Sum + node.RightNode!.Sum
                    + node.LeftNode!.LazySum * node.LeftNode.Length
                    + node.RightNode!.LazySum * node.RightNode.Length;
            }
        }

        public void Add(int l, int r, int val) {
            Add2(l, r, val, root);
        }

        private static void Add2(int l, int r, int val, Node cur) {
            if (l > cur.Right || r < cur.Left) {
                return;
            }
            if (l <= cur.Left && r >= cur.Right) {
                cur.LazySum += val;
            } else {
                PushDown(cur);
                Add2(l, r, val, cur.LeftNode!);
                Add2(l, r, val, cur.RightNode!);
                PushUp(cur);
            }
        }

        private static void Update2(int l, int r, int val, Node cur) {
            if (l > cur.Right || r < cur.Left) {
                return;
            }

            PushDown(cur);
            if (l <= cur.Left && r >= cur.Right) {
                cur.HasUpdate = true;
                cur.LazyUpdate = val;
                cur.Sum = val * cur.Length;
                // 终止条件，全部都揽住了
            } else {
                AddChild(cur);
                Update2(l, r, val, cur.LeftNode!);
                Update2(l, r, val, cur.RightNode!);
                PushUp(cur);
            }
        }

        private static void AddChild(Node cur) {
            int mid = (cur.Left + cur.Right) >> 1;
            if (cur.IsLeaf) {
                cur.LeftNode = new(cur.Left, mid);
                cur.RightNode = new(mid + 1, cur.Right);
            }
        }

        public int Query(int l, int r) {
            return Query2(l, r, root);
        }

        private static int Query2(int l, int r, Node cur) {
            if (l > cur.Right || r < cur.Left) {
                return 0;
            }
            PushDown(cur);

            if (l <= cur.Left && r >= cur.Right) {
                return cur.Sum;
            } else {
                if (cur.IsLeaf) {
                    return cur.Sum;
                }
                return Query2(l, r, cur.LeftNode!) + Query2(l, r, cur.RightNode!);
            }
        }
    }
}



















