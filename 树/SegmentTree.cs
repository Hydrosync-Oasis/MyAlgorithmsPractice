using System.Reflection;

namespace 树 {
    internal class SegmentTree {
        int[] sum;
        int[] data;
        int[] lazy;//lazy有值的地方代表该节点sum值虽然已经累加，但是子节点的sum值都没有加过

        public SegmentTree(int[] data) {
            this.data = new int[data.Length + 1];
            Array.Copy(data, 0, this.data, 1, data.Length);
            sum = new int[this.data.Length << 2]; //四倍空间肯定够用，用于存放[x..y]求和的值
            lazy = new int[this.data.Length << 2]; //四倍空间肯定够用
        }

        public void Build(int from, int to, int position) {
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
            Add2(left, right, 0, sum.Length - 1, val, 0);
        }

        private void Add2(int left, int right, int l, int r, int val, int rt) {
            if (left > r || right < l) {
                return;
            }
            if (left <= l && right >= r) { //l r是实际的线段树中的区间
                lazy[rt] += val;//只有这个大区间有懒信息，下面的子节点都没加过
                sum[rt] += val * (r - l + 1);
                return;
            }
            if (lazy[rt] != 0) {
                LazyPush(rt, l, r);
            }
            Add2(left, right, l, r >> 1, val, rt >> 1);
            Add2(left, right, r >> 1 | 1, r, val, rt >> 1 | 1);
        }

        private void LazyPush(int rt, int l, int r) {
            //本节点的sum值已经加过lazy了，子节点没加过lazy
            lazy[rt >> 1] += lazy[rt];
            lazy[rt >> 1 | 1] += lazy[rt];
            sum[rt >> 1] += lazy[rt] * (r - l - 1) / 2;
            sum[rt >> 1 | 1] += lazy[rt] * (r - l - 1) / 2;
            lazy[rt] = 0;//本节点lazy已下放完毕，清空lazy值
        }

        public int GetSum(int left, int right) {
            throw new NotImplementedException();
        }
    }
}
