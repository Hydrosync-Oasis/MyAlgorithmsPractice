using ServiceStack;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Xml.Linq;
using 树;
using 队列;

namespace Algorithm {
    internal static partial class Exercises {
        public static IList<bool> CheckArithmeticSubarrays(int[] nums, int[] l, int[] r) {
            bool[] res = new bool[l.Length];
            for (int i = 0; i < l.Length; i++) { //查询次数
                int min = int.MaxValue;
                int max = int.MinValue;
                HashSet<int> set = new();
                for (int j = l[i]; j <= r[i]; j++) {
                    min = Math.Min(min, nums[j]);
                    max = Math.Max(max, nums[j]);
                }
                //生成等差数列
                for (int j = 0; j < r[i] - l[i] + 1; j++) {
                    int item = min + (max - min) / (r[i] - l[i]) * j;
                    if (!set.Add(item)) {
                        set.Remove(item);
                    }
                }
                for (int j = l[i]; j <= r[i]; j++) {
                    if (!set.Add(nums[j])) {
                        set.Remove(nums[j]);
                    }
                }
                res[i] = set.Count == 0;
            }
            return res;
        }

        public static int FindLengthOfShortestSubarray(int[] arr) {
            int l = 0, r = arr.Length - 1;
            while (l < arr.Length - 1 && arr[l] <= arr[l + 1]) {
                l++;
            }
            while (r > 0 && arr[r] >= arr[r - 1]) {
                r--;
            }
            //找到了两侧的单调段，从最大值开始枚举子数组长度
            int c1 = l;
            int c2 = arr.Length - 1;
            int res = int.MaxValue;
            while (c1 >= 0 && c2 >= r) {
                if (arr[c1] <= arr[c2]) {
                    res = Math.Min(res, c2 - c1 - 1);
                }

                if (arr[c1] > arr[c2]) {
                    c1--;
                } else {
                    c2--;
                }

            }
            return Math.Max(0, Math.Min(arr.Length - Math.Max(l + 1, arr.Length - r), res));
        }

        public static int MaxValue(int n, int index, int maxSum) {
            if (index >= n / 2) {
                index = n - index - 1;
            }
            int res;
            if (Math.Sqrt(maxSum - n) <= index && Math.Sqrt(maxSum - n) - 1 <= n - index - 1) {
                res = (int)Math.Sqrt(maxSum - n) + 1;
            } else if (maxSum <= 0.5 * n * n) {
                res = Quadratic(0.5, index - 0.5, 0.5 * (-index * index - 3 * index + 2 * n) - maxSum);
            } else {
                res = (int)((-2.0 * index * n + 2 * index * index + n * n + 2 * index - n + 2 * maxSum) / (2 * n));
            }
            return res;

            static int Quadratic(double a, double b, double c) {
                double sqrt = Math.Sqrt(b * b - 4 * a * c);
                double numerator = sqrt - b;
                return (int)(numerator / (2 * a));
            }
        }

        public static void MoveZeroes2(int[] nums) {
            int c1 = 0;
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] != 0) {
                    nums[c1++] = nums[i];
                }
            }
            for (int i = c1; i < nums.Length; i++) {
                nums[i] = 0;
            }
        }

        public static int RemoveDuplicates2(int[] nums, int k) {
            if (nums.Length <= k) { return nums.Length; }
            int from = 0;
            int to = 0;
            while (from < nums.Length && to < nums.Length) {
                int tmp = to;
                while (to < nums.Length - 1 && nums[to] == nums[to + 1]) {
                    to++;
                }
                if (to - tmp + 1 < k) { //to长度不满足2，是1
                    int n = to - tmp + 1;//实际长度，小于k
                    for (int i = 0; i < n; i++) {
                        nums[from + i] = nums[to];//把to复制了n遍
                    }
                    //nums[from] = nums[to];
                    from += n;
                    to++;
                } else {
                    for (int i = 0; i < k; i++) {
                        nums[from + i] = nums[to - k + 1 + i];
                    }
                    //nums[from] = nums[to - 1];
                    //nums[from + 1] = nums[to];

                    from += k; //太多了，顶头了，加k代表把数组长度删到k这么小后往右移动
                    to++;
                }
            }
            return from;//每一轮结束，from都会来到这一组被删除后的最后一个位置
        }

        public static bool FindSubarrays(int[] nums) {
            HashSet<int> set = new();
            for (int i = 0; i < nums.Length - 1; i++) {
                if (!set.Add(nums[i] + nums[i + 1])) {
                    return true;
                }
            }
            return false;
        }

        public static int NumFactoredBinaryTrees(int[] arr) {
            Array.Sort(arr);
            const int MOD = 1000000007;
            Dictionary<int, long> map = new();
            for (int i = 0; i < arr.Length; i++) {
                map[arr[i]] = 1;
            }
            for (int i = 0; i < arr.Length; i++) {
                HashSet<int> dont = new();
                for (int j = 0; j <= i; j++) {
                    if (dont.Add(arr[j]) && arr[i] % arr[j] == 0 && map.ContainsKey(arr[i] / arr[j])) {
                        map[arr[i]] = (map[arr[i]] % MOD +
                            (map[arr[j]] % MOD * (map[arr[i] / arr[j]] % MOD) % MOD)
                            ) % MOD;
                    }
                }
            }
            long res = 0;
            foreach (var item in map) {
                res = (res % MOD + item.Value % MOD) % MOD;
            }
            return (int)res;
        }

        public static int MaxSumAfterPartitioning(int[] arr, int k) {
            return DFS(arr, 0, k, new());

            static int DFS(int[] arr, int startIdx, int k, Dictionary<int, int> map) {
                if (startIdx == arr.Length - 1) {
                    return arr[^1];
                }
                if (map.ContainsKey(startIdx)) {
                    return map[startIdx];
                }
                int max = 0;
                int sum = 0;
                for (int i = startIdx; i < Math.Min(arr.Length, startIdx + k); i++) {
                    max = Math.Max(max, arr[i]);
                    sum = Math.Max(sum, max * (i - startIdx + 1) + DFS(arr, i + 1, k, map));
                }
                map.Add(startIdx, sum);
                return sum;
            }
        }

        public static int OpenLock(string[] deadends, string target) {
            HashSet<string> dead = new(deadends);
            HashSet<string> history = new();
            string start = "0000";
            if (dead.Contains(start)) {
                return -1;
            }
            int min = -1;
            Queue<(string, int)> que = new(); //(cur, count)
            que.Enqueue((start, 0));
            history.Add(start);
            while (que.Count > 0) {
                var tmp = que.Dequeue();
                if (tmp.Item1 == target) {
                    min = tmp.Item2;
                    break;
                }
                for (int i = 0; i < tmp.Item1.Length; i++) {
                    string s1 = tmp.Item1[0..i] +
                        ((char)((tmp.Item1[i] - '0' - 1 + 10) % 10 + '0')).ToString() +
                        tmp.Item1[(i + 1)..];
                    string s2 = tmp.Item1[0..i] +
                        ((char)((tmp.Item1[i] - '0' + 1 + 10) % 10 + '0')).ToString() +
                        tmp.Item1[(i + 1)..];
                    if (history.Add(s1) && !dead.Contains(s1)) {
                        que.Enqueue((s1, tmp.Item2 + 1));
                    }
                    if (history.Add(s2) && !dead.Contains(s2)) {
                        que.Enqueue((s2, tmp.Item2 + 1));
                    }
                }
            }
            return min;
        }

        public static int CountSubstrings(string s, string t) {
            int[,] dp = new int[t.Length + 1, s.Length + 1];
            int[,] help = new int[t.Length + 1, s.Length + 1];

            dp[1, 1] = s[0] != t[0] ? 1 : 0;
            help[1, 1] = s[0] == t[0] ? 1 : 0;
            int sum = dp[1, 1];
            for (int i = 2; i <= s.Length; i++) {
                dp[1, i] = s[i - 1] != t[0] ? 1 : 0;
                sum += dp[1, i];

                if (s[i - 1] == t[0]) {
                    help[1, i] = 1;
                } else {
                    help[1, i] = 0;
                }
            }
            for (int i = 2; i <= t.Length; i++) {
                dp[i, 1] = s[0] != t[i - 1] ? 1 : 0;
                sum += dp[i, 1];

                if (s[0] == t[i - 1]) {
                    help[i, 1] = 1;
                } else {
                    help[i, 1] = 0;
                }
            }
            for (int i = 2; i <= t.Length; i++) {
                for (int j = 2; j <= s.Length; j++) {
                    if (t[i - 1] == s[j - 1]) {
                        help[i, j] = help[i - 1, j - 1] + 1;
                        dp[i, j] = dp[i - 1, j - 1];
                    } else {
                        help[i, j] = 0;
                        dp[i, j] = 1 + help[i - 1, j - 1];
                    }
                    sum += dp[i, j];
                }
            }
            return sum;
        }

        public static string ShortestCommonSupersequence(string str1, string str2) {
            //LCS
            int[,] dpLCS = new int[str2.Length + 1, str1.Length + 1];
            for (int i = 1; i <= str1.Length; i++) {
                dpLCS[1, i] = str1[i - 1] == str2[0] ? 1 : dpLCS[1, i - 1];
            }
            for (int i = 1; i <= str2.Length; i++) {
                dpLCS[i, 1] = str2[i - 1] == str1[0] ? 1 : dpLCS[i - 1, 1];
            }
            for (int i = 1; i <= str2.Length; i++) {
                for (int j = 1; j <= str1.Length; j++) {
                    if (str1[j - 1] == str2[i - 1]) {
                        dpLCS[i, j] = dpLCS[i - 1, j - 1] + 1;
                    } else {
                        dpLCS[i, j] = Math.Max(dpLCS[i - 1, j], dpLCS[i, j - 1]);
                    }
                }
            }
            StringBuilder LCS = new();
            //逆序根据表来判断LCS到底是哪个串 
            int m = str2.Length, n = str1.Length;
            while (true) {
                if (m < 1 || n < 1) {
                    break;
                }
                if (str1[n - 1] == str2[m - 1]) {
                    LCS.Append(str1[n - 1]);
                    m--;
                    n--;
                } else {
                    if (dpLCS[m - 1, n] >= dpLCS[m, n - 1]) {
                        m--;
                    } else {
                        n--;
                    }
                }
            }

            LCS = new(string.Concat(LCS.ToString().Reverse()));//算出来了LCS
                                                               //dpLCS = default!;
            int c1 = 0, c2 = 0, c = 0;
            StringBuilder res = new();
            while (c < LCS.Length && c1 < str1.Length && c2 < str2.Length) {
                while (str1[c1] != str2[c2] || str1[c1] != LCS[c]) {
                    if (str1[c1] != LCS[c]) {
                        res.Append(str1[c1++]);
                    }
                    if (str2[c2] != LCS[c]) {
                        res.Append(str2[c2++]);
                    }
                }
                res.Append(LCS[c++]);
                c1++;
                c2++;
            }
            if (c1 < str1.Length) {
                res.Append(str1[c1..]);
            }
            if (c2 < str2.Length) {
                res.Append(str2[c2..]);
            }
            return res.ToString();
        }

        public static int CountVowelStrings(int n) {
            // a e i o u
            if (n == 1) {
                return 5;
            }
            List<char> order = new() { 'a', 'e', 'i', 'o', 'u' };
            int[,] map = new int[n + 1, order.Count];
            int res = 0;
            for (int i = 0; i < order.Count; i++) {
                res += DFS(n - 1, order[i], order, map);
            }
            return res;

            static int DFS(int n, char last, List<char> order, int[,] map) {
                int startIdx = order.IndexOf(last);
                if (n == 1) {
                    return order.Count - startIdx; //包括自己
                }
                if (map[n, startIdx] != 0) {
                    return map[n, startIdx];
                }
                int res = 0;
                for (int i = startIdx; i < order.Count; i++) {
                    res += DFS(n - 1, order[i], order, map);
                }
                map[n, startIdx] = res;
                return res;
            }
        }

        public static int Reverse2(int x) {
            if (x == int.MinValue) {
                return 0;
            }
            bool neg = false;
            if (x < 0) {
                x = -x;
                neg = true;
            }

            int res = 0;
            int digit = 1;
            int n = 0;
            while (true) {
                n++;
                res *= 10;
                res += x / digit % 10;
                if (x / digit >= 10) {
                    digit *= 10;
                } else {
                    break;
                }
            }
            if (n == 10 && x % 10 > 2) {
                return 0;
            } else if (n == 10 && x % 10 == 2) {
                if (Reverse2(x / 10) > 147483647) {
                    return 0;
                }
            }
            return neg ? -res : res;
        }

        public static int MaxWidthOfVerticalArea(int[][] points) {
            Array.Sort(points, (a, b) => a[0].CompareTo(b[0]));
            int res = 0;
            for (int i = 1; i < points.Length; i++) {
                if (points[i][0] - points[i - 1][0] > res) {
                    res = points[i][0] - points[i - 1][0];
                }
            }
            return res;
        }

        public static int LargestComponentSize(int[] nums) {
            //生成1 ~ max的质数表
            int max = nums.Max();
            bool[] prime = new bool[max + 1];//true：非质数；false：质数
            prime[1] = true;
            for (int i = 2; i <= Math.Sqrt(max); i++) {
                while (prime[i]) { i++; }
                for (int j = i * i; j <= max; j += i) {
                    prime[j] = true;
                }
            }

            Dictionary<int, int> dic = new();
            UnionFindSet<int> ufo = new();

            ufo.AddRange(nums);
            for (int i = 0; i < nums.Length; i++) {
                int n = (int)Math.Sqrt(nums[i]);
                for (int j = 2; j <= n; j++) {
                    if (!prime[j] && nums[i] % j == 0) {
                        if (!dic.TryAdd(j, i)) {
                            ufo.Union(nums[i], dic[j]);
                        }
                    }
                }
            }

            return ufo.GetEachSetCount().Max();
        }

        public static string MaskPII(string s) {
            string[] email = s.Split('@');
            StringBuilder res = new();
            if (email.Length == 2) {
                //是邮件
                res.Append(email[0][0].ToString().ToLower());
                res.Append(new string('*', 5));
                res.Append(email[0][^1].ToString().ToLower());

                res.Append('@');

                res.Append(email[1].ToLower());
            } else {
                //手机号
                StringBuilder sb = new();
                for (int i = 0; i < s.Length; i++) {
                    if (char.IsDigit(s[i])) {
                        sb.Append(s[i]);
                    }
                }
                int numLen = sb.Length;
                switch (numLen) {
                    case 10:
                        res.Append("***-***-");
                        res.Append(sb.ToString()[^4..]);
                        break;
                    case >= 11 and <= 13:
                        res.Append('+');
                        res.Append(new string('*', numLen - 10));
                        res.Append("-***-***-");
                        res.Append(sb.ToString()[^4..]);
                        break;
                }
            }

            return res.ToString();
        }

        public static IList<IList<string>> GroupAnagrams(string[] strs) {
            List<int> sorted = new();
            for (int i = 0; i < strs.Length; i++) {
                var arr = strs[i].ToCharArray();
                Array.Sort(arr);
                sorted.Add(new string(arr).GetHashCode());
            }
            Dictionary<int, List<string>> map = new();
            for (int i = 0; i < strs.Length; i++) {
                if (map.ContainsKey(sorted[i])) {
                    map[sorted[i]].Add(strs[i]);
                } else {
                    map.Add(sorted[i], new() { strs[i] });
                }
            }

            List<IList<string>> res = new();
            foreach (var item in map) {
                res.Add(item.Value);
            }

            return res;
        }

        public static int[][] RestoreMatrix(int[] rowSum, int[] colSum) {
            int[][] mat = new int[rowSum.Length][];
            for (int i = 0; i < rowSum.Length; i++) {
                mat[i] = new int[colSum.Length];
                for (int j = 0; j < colSum.Length; j++) {
                    mat[i][j] = Math.Min(rowSum[i], colSum[j]);
                    rowSum[i] -= mat[i][j];
                    colSum[j] -= mat[i][j];
                }
            }
            return mat;
        }

        public static int MinScoreTriangulationSlow(int[] values) { //超级阶乘复杂度暴力搜索，无法优化
            return DFS(values, new());

            static int DFS(int[] values, HashSet<int> chose) {
                int res = int.MaxValue;

                for (int i = 0; i < values.Length; i++) {
                    //选择后两个
                    int cur = i;
                    while (chose.Contains(cur)) {
                        cur = (cur + 1) % values.Length;
                        if (cur == i) {
                            return res == int.MaxValue ? 0 : res;
                        }
                    }
                    chose.Add(cur); //防止后面选择到前面的 
                    int j = cur;
                    while (chose.Contains((j + 1) % values.Length)) {
                        if ((j + 1) % values.Length == cur) {
                            chose.Remove(cur);
                            return res == int.MaxValue ? 0 : res;
                        }
                        j++;
                    }
                    int second = (j + 1) % values.Length;
                    j = second;
                    chose.Add(second);
                    while (chose.Contains((j + 1) % values.Length)) {
                        if ((j + 1) % values.Length == second) {
                            chose.Remove(second);
                            chose.Remove(cur);
                            return res == int.MaxValue ? 0 : res;
                        }
                        j++;
                    }
                    int third = (j + 1) % values.Length;
                    chose.Remove(cur);
                    res = Math.Min(res, values[cur] * values[second] * values[third] + DFS(values, chose));
                    chose.Remove(second);
                }
                return res;
            }
        }

        public static int MinScoreTriangulation(int[] values) {
            return DFS(values, 0, values.Length - 1, new());

            static int DFS(int[] values, int left, int right, Dictionary<(int, int), int> map) {
                if (right - left + 1 <= 2) {
                    return 0;//选不了
                }
                if (right - left + 1 == 3) {
                    return values[left] * values[left + 1] * values[left + 2];
                }
                if (map.ContainsKey((left, right))) {
                    return map[(left, right)];
                }
                int res = int.MaxValue;
                //固定选择left, right这两个端点
                for (int i = left + 1; i < right; i++) {
                    res = Math.Min(res, values[left] * values[right] * values[i] + DFS(values, left, i, map) + DFS(values, i, right, map));
                }
                map[(left, right)] = res;
                return res;
            }
        }

        public static int[] PrevPermOpt1(int[] arr) {
            int i = arr.Length - 1;
            while (i > 0 && arr[i - 1] <= arr[i]) {
                i--;
            }
            //i此时在递增段的最左侧
            i--;
            if (i < 0) {
                return arr;
            }

            int j = arr.Length - 1;
            while (i < j && arr[j] >= arr[i]) {
                j--;
            }
            while (j > i && arr[j - 1] == arr[j]) {
                j--;//j尽量靠前保证字典序最大
            }

            (arr[i], arr[j]) = (arr[j], arr[i]);
            return arr;
        }

        public static bool ReachingPoints(int sx, int sy, int tx, int ty) {
            if (tx < sx || ty < sy) {
                return false;
            }
            if (sx == tx) {
                return (ty - sy) % sx == 0;
            }
            if (sy == ty) {
                return (tx - sx) % sy == 0;
            }
            if (ty > tx) {
                return ReachingPoints(sx, sy, tx, ty % tx);
            } else {
                return ReachingPoints(sx, sy, tx % ty, ty);
            }
        }

        public static int MergeStones(int[] stones, int k) {
            if (stones.Length % (k - 1) != 1 && k - 1 != 1) {
                return -1;
            }
            return DFS(stones, k, 0, stones.Length - 1);

            static int DFS(int[] values, int k, int left, int right) {
                int[] prefix = new int[values.Length];
                prefix[0] = values[0];
                for (int i = 1; i < values.Length; i++) {
                    prefix[i] = prefix[i - 1] + values[i];
                }

                if (right - left + 1 < k) {
                    return 0;
                }
                if (right - left + 1 == k) {
                    return prefix[right] - (left > 0 ? prefix[left - 1] : 0);
                }

                int res = int.MaxValue;
                for (int i = left; i < right - k + 2; i++) {
                    //有2个选择，即：把[i..i+k)合并，再与左区间合并；与右区间合并
                    int mid = prefix[i + k - 1] - (i > 0 ? prefix[i - 1] : 0);
                    //先合并左边
                    int l, r, case1 = int.MaxValue, case2 = int.MaxValue;
                    if (i + 1 >= k) {
                        int tmp = values[i];
                        values[i] = mid;
                        l = DFS(values, k, left, i);
                        int tmp2 = values[i + k - 1];
                        values[i + k - 1] = prefix[i + k - 1] - (left > 0 ? prefix[left - 1] : 0);
                        r = DFS(values, k, i + k - 1, right);
                        values[i + k - 1] = tmp2;

                        case1 = mid + l + r;// + prefix[right] - (left > 0 ? prefix[left - 1] : 0);

                        values[i] = tmp;
                    }
                    //先合并右边
                    if (right - (i + k - 1) >= k) {
                        int tmp = values[i + k - 1];
                        values[i + k - 1] = mid;
                        r = DFS(values, k, i + k - 1, right);
                        int tmp2 = values[i];
                        values[i] = prefix[right] - (i > 0 ? prefix[i - 1] : 0);
                        l = DFS(values, k, left, i);
                        values[i] = tmp2;

                        case2 = mid + l + r;// + prefix[right] - (left > 0 ? prefix[left - 1] : 0);

                        values[i + k - 1] = tmp;
                    }
                    res = Math.Min(res, Math.Min(case1, case2));
                }
                return res;

            }
        }

        public static string BaseNeg2(int n) {
            if (n == 0) { return "0"; }

            StringBuilder sb = new();
            long check = 2;
            long remain = n;
            bool neg = false;
            while (remain != 0) {
                if ((remain & (check - 1)) == 0) {
                    sb.Append('0');
                } else {
                    sb.Append('1');
                    remain -= neg ? -(check >> 1) : (check >> 1);
                }
                check <<= 1;
                neg = !neg;
            }
            for (int i = 0; i < sb.Length / 2; i++) {
                (sb[i], sb[^(i + 1)]) = (sb[^(i + 1)], sb[i]);
            }
            return sb.ToString();
        }

        public static void ReverseStack(Stack<int> st) {
            for (int i = 0; i < st.Count - 1; i++) {
                f(st, 0, st.Count - i, true);
            }

            static void f(Stack<int> st, int wannaPush, int n, bool first) { //还n次操作（包括当前的这一次）
                var i = st.Pop();

                if (first) {
                    f(st, i, n - 1, false);
                } else if (n == 1) { //所以1次 = 这一次进行完，就没了，直接结束递归
                    st.Push(wannaPush);
                    st.Push(i);
                } else {
                    f(st, wannaPush, n - 1, false);
                    st.Push(i);
                }
            }
        }

        public static bool IsRobotBounded(string instructions) {
            int direction = 0; //N
            (int, int) p = (0, 0);
            foreach (char c in instructions) {
                if (c == 'L') {
                    direction = (direction + 3) & 3;
                } else if (c == 'R') {
                    direction = (direction + 1) & 3;
                } else {
                    switch (direction & 3) {
                        case 0:
                            p.Item2++;
                            break;
                        case 1:
                            p.Item1++;
                            break;
                        case 2:
                            p.Item2--;
                            break;
                        case 3:
                            p.Item1--;
                            break;
                    }
                }
            }
            if (p == (0, 0)) {
                return true;
            }
            if (direction == 0) {
                return false;
            }

            return true;
        }

        public static int IntegerReplacement(int n) {
            return DFS(n);

            static int DFS(int n) {
                if (n <= 3) { return n - 1; }

                if (n == int.MaxValue) {
                    return DFS(1073741824) + 2;
                }
                if ((n & 1) == 0) {
                    return DFS(n >> 1) + 1;
                } else {
                    //奇数，分情况讨论
                    int res = int.MaxValue;
                    if (((n - 1) & 3) == 0) {
                        res = Math.Min(res, DFS(n - 1));
                    } else if (((n + 1) & 3) == 0) {
                        res = Math.Min(res, DFS(n + 1));
                    } else { //不可能来到这个分支
                             //res = Math.Min(DFS(n + 1), DFS(n - 1));
                    }
                    return res + 1;
                }
            }
        }

        public static bool CanIWin(int maxChoosableInteger, int desiredTotal) {
            if ((maxChoosableInteger + 1) * maxChoosableInteger / 2 < desiredTotal) {
                return false;
            }
            int mask = (1 << maxChoosableInteger) - 1; //2^N - 1
            return DFS(mask, desiredTotal, maxChoosableInteger, 0, new());

            static bool DFS(int mask, int goal, int max, int cur, Dictionary<int, bool> map) {
                //定义函数：可选范围是mask，已经选了cur这么多数，再挑任意一个数，返回对于当前局面的先手做出的最佳策略是否是赢
                if (map.ContainsKey(mask)) {
                    return map[mask];
                }

                bool ifWin = false;
                for (int i = 0; i < max; i++) {
                    if (((mask >> i) & 1) == 1) { //如果可以取这个数
                        if ((mask ^ (1 << i)) == 0) { //空了
                            ifWin = cur + i + 1 >= goal;
                            break;//完了
                        }
                        if (cur + i + 1 >= goal) {
                            ifWin = true; //直接就停止了，没后面的事了
                            break;
                        } else {
                            bool nextPlayer = DFS(mask ^ (1 << i), goal, max, cur + i + 1, map);
                            if (!nextPlayer) { //有一种必胜方案
                                ifWin = true;
                                break;
                            }
                        }
                    }
                }

                map[mask] = ifWin;
                return map[mask];
            }
        }

        public static int[] GardenNoAdj(int n, int[][] paths) {
            if (paths.Length == 0) {
                int[] res = new int[n];
                Array.Fill(res, 1);
                return res;
            }

            int[] color = new int[n]; //Node -->> a color, 1 2 3 4
            List<int>[] next = new List<int>[n + 1];
            for (int i = 0; i < paths.Length; i++) {
                if (next[paths[i][0]] is null) {
                    next[paths[i][0]] = new();
                }
                next[paths[i][0]].Add(paths[i][1]);

                if (next[paths[i][1]] is null) {
                    next[paths[i][1]] = new();
                }
                next[paths[i][1]].Add(paths[i][0]);
            } // 构建了一张图

            for (int i = 1; i <= n; i++) {
                if (color[i - 1] == 0) {
                    DFS(color, next, i);
                }
            }

            return color;

            static void DFS(int[] color, List<int>[] next, int startNode) {
                Queue<int> que = new();
                que.Enqueue(startNode);
                while (que.Count > 0) {
                    var tmp = que.Dequeue();
                    HashSet<int> used = new() { 1, 2, 3, 4 };
                    if (next[tmp] is null) {
                        // 说明该点是孤立点，随便弄个值对其他的节点都无影响
                        color[tmp - 1] = 1;
                        continue;
                    }

                    foreach (var item in next[tmp]) {
                        if (used.Contains(color[item - 1])) {
                            used.Remove(color[item - 1]);
                        }
                        if (color[item - 1] == 0) {
                            que.Enqueue(item);
                        }
                    }

                    // 剩下的种类一定可以种植
                    foreach (var item in used) {
                        color[tmp - 1] = item;
                        break;
                    }
                }
            }
        }

        public static bool BackspaceCompare(string s, string t) {
            char[] a = s.ToCharArray();
            char[] b = t.ToCharArray();
            int lenA = f(a);
            int lenB = f(b);
            if (lenA != lenB) {
                return false;
            }
            for (int i = 0; i < lenA; i++) {
                if (a[i] != b[i]) {
                    return false;
                }
            }
            return true;

            static int f(char[] s) {
                int cur1 = 0;
                int cur2 = 0;
                while (cur2 < s.Length) {
                    while (cur2 < s.Length && s[cur2] != '#') {
                        s[cur1] = s[cur2];
                        cur1++;
                        cur2++;
                    } //结束后cur1来到了删除后长度的下一个位置
                    int skip = 0;
                    while (cur2 < s.Length && s[cur2] == '#') {
                        skip++;
                        cur2++;
                    }
                    cur1 = Math.Max(0, cur1 - skip);
                }

                return cur1;
            }
        }

        public static int IslandPerimeter(int[][] grid) {
            int[,] mat = new int[grid.Length + 2, grid[0].Length + 2];
            for (int i = 0; i < grid.Length; i++) {
                for (int j = 0; j < grid[i].Length; j++) {
                    mat[i + 1, j + 1] = grid[i][j];
                }
            }

            int res = 0;
            for (int i = 1; i <= grid.Length; i++) {
                for (int j = 1; j <= grid[0].Length; j++) {
                    if (mat[i, j] == 1) {
                        int exposed = 0;
                        if (mat[i - 1, j] == 0) {
                            exposed++;
                        }
                        if (mat[i, j - 1] == 0) {
                            exposed++;
                        }
                        if (mat[i + 1, j] == 0) {
                            exposed++;
                        }
                        if (mat[i, j + 1] == 0) {
                            exposed++;
                        }
                        res += exposed;
                    }
                }
            }

            return res;
        }

        public static int LadderLength(string beginWord, string endWord, IList<string> wordList) {
            HashSet<string> set = new(wordList);
            set.Add(beginWord);
            Dictionary<string, List<string>> relation = new();
            foreach (var item in set) { //建立二元关系，时间复杂度O(N * C * Sigma)
                StringBuilder tmp = new(item);
                for (int i = 0; i < item.Length; i++) {
                    //对item的每一位进行检查
                    var ch = tmp[i];
                    for (int j = 'a'; j <= 'z'; j++) {
                        tmp[i] = (char)j;
                        if (set.Contains(tmp.ToString())) {
                            if (!relation.ContainsKey(item)) {
                                relation[item] = new();
                            }
                            relation[item].Add(tmp.ToString());
                        }
                    }
                    tmp[i] = ch;
                }
            }
            Queue<(string, int)> que = new();
            que.Enqueue((beginWord, 0));
            HashSet<string> history = new() { beginWord };
            int n = 0;
            while (que.Count > 0) {
                var tmp = que.Dequeue();
                if (tmp.Item1 == endWord) {
                    n = tmp.Item2;
                    break;
                }
                foreach (var item in relation[tmp.Item1]) {
                    if (history.Add(item)) {
                        que.Enqueue((item, tmp.Item2 + 1));
                    }
                }
            }
            return n == 0 ? 0 : n + 1;
        }

        public static int ClearLastOne(int num) {
            return (num ^ ~(num + 1)) & num;
        }

        public static int FindNumberOfLIS(int[] nums) {
            int[] dpLIS = new int[nums.Length];
            Array.Fill(dpLIS, 1);
            dpLIS[^1] = 1;
            int max = 1;
            for (int i = nums.Length - 2; i >= 0; i--) {
                for (int j = i + 1; j < nums.Length; j++) {
                    if (nums[j] > nums[i]) {
                        dpLIS[i] = Math.Max(dpLIS[i], 1 + dpLIS[j]);
                        max = Math.Max(max, dpLIS[i]);
                    }
                }
            }

            int[] dp = new int[nums.Length]; //有几个最长递增子序列
            dp[^1] = 1;
            for (int i = nums.Length - 2; i >= 0; i--) {
                if (dpLIS[i] == 1) { //如果最长只有1，那么自己是一种方案，不需要找后面
                    dp[i] = 1;
                } else {
                    for (int j = i + 1; j < nums.Length; j++) {
                        if (nums[j] > nums[i] && dpLIS[j] == dpLIS[i] - 1) {
                            dp[i] += dp[j];
                        }
                    }
                }
            }

            int res = 0;
            for (int i = 0; i < dpLIS.Length; i++) {
                if (dpLIS[i] == max) {
                    res += dp[i];
                }
            }
            return res;
        }

        public static int ThreeSumClosest(int[] nums, int target) {
            Array.Sort(nums);
            int res = 0;
            int minus = int.MaxValue;
            for (int i = 0; i < nums.Length - 2; i++) {
                int twoSumTarget = target - nums[i];
                int cur1 = i + 1, cur2 = nums.Length - 1;

                while (cur1 < cur2) {
                    if (Math.Abs(twoSumTarget - nums[cur1] - nums[cur2]) < minus) {
                        minus = Math.Abs(twoSumTarget - nums[cur1] - nums[cur2]);
                        res = nums[i] + nums[cur1] + nums[cur2];
                    }

                    if (nums[i] + nums[cur1] + nums[cur2] < target) {
                        cur1++;
                    } else {
                        cur2--;
                    }

                }
            }
            return res;
        }

        public static int LongestArithSeqLength(int[] nums) {
            Dictionary<int, int>[] dp = new Dictionary<int, int>[nums.Length]; //从idx开始，间隔为key的等差数列
            dp[^1] = new(); //每个元素都是一个间隔任意，长度最大为1的等差数列
            int res = 0;
            for (int i = nums.Length - 2; i >= 0; i--) {
                //填充dp[i]
                dp[i] = new();
                for (int j = i + 1; j < nums.Length; j++) {
                    //先考虑间隔任意，长度为1
                    int gap = nums[j] - nums[i];
                    if (!dp[i].ContainsKey(gap)) {
                        dp[i][gap] = 2;
                    }
                    dp[i][gap] = Math.Max(dp[i][gap], 2);

                    if (dp[j].ContainsKey(gap)) {
                        dp[i][gap] = Math.Max(dp[i][gap], 1 + dp[j][gap]);
                    }
                }
            }

            for (int i = 0; i < dp.Length; i++) {
                foreach (var item in dp[i].Values) {
                    res = Math.Max(res, item);
                }
            }

            return res;
        }

        public static int MinHeightShelves(int[][] books, int shelfWidth) {
            //split the array
            //find the first interval
            return dfs(0, books, shelfWidth, new());

            static int dfs(int startIdx, int[][] books, int shelfWidth, Dictionary<int, int> map) {
                if (startIdx == books.Length - 1) {
                    if (books[^1][0] > shelfWidth) {
                        return -1;
                    } else {
                        return books[^1][1];
                    }
                } else if (startIdx == books.Length) {
                    return 0;
                }
                if (map.ContainsKey(startIdx)) {
                    return map[startIdx];
                }

                int res = int.MaxValue;
                int thickness = 0;
                int choseHeight = 0;
                for (int i = startIdx; i < books.Length; i++) {
                    choseHeight = Math.Max(choseHeight, books[i][1]);
                    thickness += books[i][0];
                    if (thickness > shelfWidth) {
                        break;
                    }

                    int next = dfs(i + 1, books, shelfWidth, map);
                    if (next != -1) {
                        res = Math.Min(res, choseHeight + next);
                    }
                }
                map[startIdx] = res;
                return res == int.MaxValue ? -1 : res;
            }
        }

        public static string[] SortPeople(string[] names, int[] heights) {
            Array.Sort(heights, names);
            Array.Reverse(names);
            return names;
        }

        public static int[][] Merge(int[][] intervals) {
            List<int[]> res = new();
            Array.Sort(intervals, delegate (int[] a, int[] b) {
                if (a[0].CompareTo(b[0]) != 0) {
                    return a[0].CompareTo(b[0]);
                } else {
                    return a[1].CompareTo(b[1]);
                }
            });
            for (int i = 0; i < intervals.Length;) {
                //对于下标是i的元素来说，往后有可能会出现重叠的区间
                int j = i + 1;
                int maxRight = intervals[i][1];
                while (j < intervals.Length && intervals[j][0] <= intervals[i][1]) {
                    if (intervals[j][1] > maxRight) {
                        maxRight = intervals[j][1];
                    }
                    j++;
                }
                if (j == i + 1) {
                    if (res.Count > 0 && res[^1][1] >= intervals[i][0]) {
                        res[^1][1] = Math.Max(res[^1][1], intervals[i][1]);
                        //res.Add(intervals[i]);
                    } else {
                        res.Add(intervals[i]);
                    }
                } else {
                    j--;
                    if (res.Count > 0 && res[^1][1] >= intervals[i][0]) {
                        res[^1][1] = Math.Max(res[^1][1], maxRight);
                    } else {
                        res.Add(new int[] { intervals[i][0], maxRight });

                        intervals[j][0] = intervals[i][0];
                        intervals[j][1] = maxRight;
                    }
                }
                i = j;
            }

            return res.ToArray();
        }

        public static int MaxSumTwoNoOverlap(int[] nums, int firstLen, int secondLen) {
            int n = nums.Length;
            return Math.Max(f(firstLen, secondLen), f(secondLen, firstLen));

            int f(int len1, int len2) {
                int[] rightMax = new int[n];
                int sum = 0;

                for (int i = n - len2; i < n; i++) {
                    sum += nums[i];
                }
                rightMax[n - len2] = sum;
                for (int i = n - len2 - 1; i >= 0; i--) {
                    sum = sum + nums[i] - nums[i + len2];
                    rightMax[i] = Math.Max(sum, rightMax[i + 1]);
                }
                int res = 0;
                sum = 0;
                for (int i = 0; i < len1; i++) {
                    sum += nums[i];
                }
                for (int i = len1 - 1; i + 1 < n; i++) {
                    res = Math.Max(res, sum + rightMax[i + 1]);
                    sum += nums[i + 1] - nums[i - len1 + 1];
                }
                return res;

            }
        }

        public static string LargestNumber(int[] nums) {
            string[] arr = new string[nums.Length];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = nums[i].ToString();
            }

            Array.Sort(arr, delegate (string a, string b) {
                for (int i = 0; i < Math.Min(a.Length, b.Length); i++) {
                    if (a[i] != b[i]) {
                        return a[i] - b[i];
                    }
                }
                //说明ab前几个字符相同
                return (a + b).CompareTo(b + a);
            });
            Array.Reverse(arr);
            StringBuilder sb = new();
            for (int i = 0; i < arr.Length; i++) {
                if (sb.Length > 0 && sb[0] == '0') {
                    sb.Clear();
                }
                sb.Append(arr[i]);
            }
            return sb.ToString();
        }

        public static int LongestStrChain(string[] words) {
            Array.Sort(words, (a, b) => a.Length.CompareTo(b.Length));
            int[] dp = new int[words.Length];
            Array.Fill(dp, 1);
            int res = 1;
            for (int i = words.Length - 2; i >= 0; i--) {
                for (int j = i + 1; j < words.Length; j++) {
                    if (check(words, i, j)) {
                        dp[i] = Math.Max(dp[i], dp[j] + 1);
                    }
                }
                res = Math.Max(res, dp[i]);
            }
            return res;

            static bool check(string[] words, int a, int b) {
                if (words[b].Length != words[a].Length + 1) {
                    return false;
                }

                int cur1 = 0, cur2 = 0;
                bool found = false;
                while (cur1 < words[a].Length) {
                    if (words[a][cur1] == words[b][cur2]) {
                        cur1++;
                        cur2++;
                    } else {
                        if (found) {
                            return false;
                        }
                        found = true;
                        cur2++;
                    }
                }
                if (!found && cur1 == cur2 && cur1 == words[a].Length) {
                    return true;
                } else if (found) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        public static float LargestSumOfAverages(int[] nums, int k) {
            return dfs(nums, 0, k, new());

            float dfs(int[] nums, int startIdx, int remainK, Dictionary<(int, int), float> map) {
                float res = 0f;
                int sum = 0;
                if (map.ContainsKey((startIdx, remainK))) {
                    return map[(startIdx, remainK)];
                }
                if (remainK == 1) {
                    for (int i = startIdx; i < nums.Length; i++) {
                        res += nums[i];
                    }
                    return (res * 1f) / (nums.Length - startIdx);
                }

                for (int i = startIdx; i <= nums.Length - remainK; i++) {
                    sum += nums[i];
                    res = Math.Max(res, (sum * 1f) / (i - startIdx + 1) + dfs(nums, i + 1, remainK - 1, map));
                }

                map[(startIdx, remainK)] = res;
                return res;
            }
        }

        public static IList<int> EventualSafeNodes(int[][] graph) {
            HashSet<int> ends = new();
            for (int i = 0; i < graph.Length; i++) {
                int[] item = graph[i];
                if (item.Length == 0) {
                    ends.Add(i);
                    ;
                }
            }

            Dictionary<int, bool> map = new();
            HashSet<int> history = new();
            for (int i = 0; i < graph.Length; i++) {
                history.Add(i);
                dfs(graph, i, map, ends, history);
                history.Remove(i);
            }
            List<int> res = new();
            foreach (var item in map.Keys) {
                if (map[item]) {
                    res.Add(item);
                }
            }
            res.Sort();
            return res;

            static bool dfs(int[][] graph, int startNode, Dictionary<int, bool> map, HashSet<int> ends, HashSet<int> history) {

                if (map.ContainsKey(startNode)) {
                    return map[startNode];
                }
                bool flag = false;
                for (int i = 0; i < graph[startNode].Length; i++) {
                    if (!ends.Contains(graph[startNode][i])) {
                        flag = true;
                        break;
                    }
                }
                if (!flag) {
                    map[startNode] = true;
                    return true;
                }

                for (int i = 0; i < graph[startNode].Length; i++) {
                    if (history.Add(graph[startNode][i])) {
                        if (!dfs(graph, graph[startNode][i], map, ends, history)) {
                            map[startNode] = false;
                            return false;
                        }
                        history.Remove(graph[startNode][i]);
                    } else {
                        map[startNode] = false;
                        return false;
                    }
                }

                map[startNode] = true;
                return true;
            }
        }

        public static bool EqualFrequency(string word) {
            int[] dict = new int[26];
            for (int i = 0; i < word.Length; i++) {
                dict[word[i] - 'a']++;
            }
            // 全部等于1是可以过的
            // 只有一个最大值，其他的值都比最大值小1也可以过
            // 只有一个种类的字符也可以过
            // 有只出现一次的字符也可以过
            int max = 0, min = int.MaxValue;
            int countNonzero = 0;
            for (int i = 0; i < dict.Length; i++) {
                if (dict[i] != 0) {
                    max = Math.Max(max, dict[i]);
                    min = Math.Min(min, dict[i]);
                    countNonzero++;
                }
            }
            if (countNonzero == 1) {
                return true;
            }

            if (max == min && min == 1) {
                return true;
            }

            int countMax = 0;
            for (int i = 0; i < dict.Length; i++) {
                if (dict[i] == max) {
                    countMax++;
                }
            }
            if (countMax == 1 && min == max - 1) {
                return true;
            }
            if (min == 1 && countMax == countNonzero - 1) {
                return true;
            }

            return false;
        }

        public static bool SplitArraySameAverage(int[] nums) {
            if (nums.Length == 1) {
                return false;
            }
            // average = nums / len
            int desiredAveDenominator = nums.Length;
            int desiredAveNumerator = nums.Sum();
            if (desiredAveNumerator == 0) {
                return true;
            }
            int div = gcd(desiredAveDenominator, desiredAveNumerator);
            desiredAveDenominator /= div;
            desiredAveNumerator /= div;
            bool res = false;
            Dictionary<(int, int, int), bool> map = new();
            for (int i = 1; i < nums.Length; i++) {
                if (i % desiredAveDenominator == 0) {
                    res |= dfs(nums, 0, (i / desiredAveDenominator * desiredAveNumerator), i, map);
                }
            }
            return res;

            static bool dfs(int[] nums, int startIdx, int desiredSum, int remainN, Dictionary<(int, int, int), bool> map) {
                if (map.ContainsKey((startIdx, desiredSum, remainN))) {
                    return map[(startIdx, desiredSum, remainN)];
                }

                if (remainN == 1) {
                    for (int i = startIdx; i < nums.Length; i++) {
                        if (nums[i] == desiredSum) {
                            return true;
                        }
                    }
                    return false;
                }
                if (remainN > nums.Length - startIdx) {
                    return false;
                }
                //不选当前数
                bool res = false;
                res |= dfs(nums, startIdx + 1, desiredSum, remainN, map);
                //选当前数
                res |= dfs(nums, startIdx + 1, desiredSum - nums[startIdx], remainN - 1, map);

                map[(startIdx, desiredSum, remainN)] = res;
                return res;
            }

            static int gcd(int a, int b) {
                if (a < b) {
                    return gcd(b, a);
                }
                if (a % b == 0) {
                    return b;
                } else {
                    return gcd(a % b, b);
                }
            }
        }

        public static int[] NumMovesStones(int a, int b, int c) {
            int max = Math.Max(a, Math.Max(b, c));
            int min = Math.Min(a, Math.Min(b, c));
            int mid = a + b + c - max - min;
            if (max - mid == mid - mid && mid == 1) {
                return new int[] { 0, 0 };
            }
            int maxStep = max - min - 2;
            //有两个间隔，mid - min - 1, max - mid - 1
            int minStep;
            if (mid - min - 1 != 0 && max - mid - 1 != 0) {
                minStep = Math.Min(mid - min - 1, max - mid - 1);
                minStep = Math.Min(minStep, 2);
            } else {
                minStep = 1;
            }
            return new int[] { minStep, maxStep };
        }

        public static int NumOfMinutes(int n, int headID, int[] manager, int[] informTime) {
            List<int>[] next = new List<int>[n];
            for (int i = 0; i < manager.Length; i++) {
                if (manager[i] == -1) {
                    continue;
                }
                if (next[manager[i]] is null) {
                    next[manager[i]] = new();
                }
                next[manager[i]].Add(i);
            }
            //从headID开始
            Queue<(int, int)> que = new(); //(val, time)
            que.Enqueue((headID, 0));
            int time = 0;
            while (que.Count > 0) {
                var tmp = que.Dequeue();
                time = Math.Max(time, tmp.Item2);
                if (next[tmp.Item1] is not null) {
                    foreach (var item in next[tmp.Item1]) {
                        que.Enqueue((item, tmp.Item2 + informTime[tmp.Item1]));
                    }
                }
            }
            return time;
        }

        public static IList<int> PowerfulIntegers(int x, int y, int bound) {
            if (x == y && x == 1) {
                return bound switch {
                    >= 2 => new List<int>() { 2 },
                    _ => Array.Empty<int>(),
                };
            }

            PriorityQueue<(int, int, int), int> que = new();
            HashSet<(int, int)> set = new();
            que.Enqueue((0, 0, 2), 2);
            set.Add((0, 0));
            List<int> res = new();
            while (true) {
                var tmp = que.Dequeue();
                if (tmp.Item3 <= bound) {
                    if (res.Count == 0 || res[^1] != tmp.Item3) {
                        res.Add(tmp.Item3);
                    }
                    if (x != 1 && set.Add((tmp.Item1 + 1, tmp.Item2))) {
                        int cur = tmp.Item3 + (int)Math.Pow(x, tmp.Item1) * (x - 1);
                        que.Enqueue((tmp.Item1 + 1, tmp.Item2, cur), cur);
                    }
                    if (y != 1 && set.Add((tmp.Item1, tmp.Item2 + 1))) {
                        int cur = tmp.Item3 + (int)Math.Pow(y, tmp.Item2) * (y - 1);
                        que.Enqueue((tmp.Item1, tmp.Item2 + 1, cur), cur);
                    }
                } else {
                    break;
                }
            }
            return res;
        }

        public static ListNode? ReverseKGroup(ListNode head, int k) {
            if (k == 1 || head is null || head.next is null) {
                return head;
            }
            ListNode start = head;
            ListNode? last = null;
            ListNode? root = null;
            while (start is not null) {
                var tmp = ReverseK(start, k);
                if (tmp.Item1 is null) {
                    if (last != null) {
                        last.next = start;
                        break;
                    }
                }
                if (last is not null) {
                    last.next = tmp.Item1;
                } else {
                    root = tmp.Item1;
                }
                //start.next = tmp.Item2; loop
                last = start;
                tmp.Item1 = tmp.Item2;
                //start.next = tmp.Item2;
                start = tmp.Item2!;
            }
            return root;

            //(tail, next)
            static (ListNode?, ListNode?) ReverseK(ListNode head, int k) {
                ListNode tail = head;

                for (int i = 1; i < k; i++) {
                    if (tail.next is not null) {
                        tail = tail.next;
                    } else {
                        return (null, null);
                    }
                }
                ListNode? n = tail.next;

                ListNode cur1 = head;
                ListNode? cur2 = cur1.next;
                cur1.next = null;
                while (cur1 != tail) {
                    ListNode? next = cur2!.next;
                    cur2.next = cur1;
                    cur1 = cur2;
                    cur2 = next;
                }
                return (tail, n);
            }
        }

        public static bool IsValidABC(string s) {
            string cur = s.Replace("abc", "");
            while (cur != s) {
                s = cur;
                cur = s.Replace("abc", "");
            }
            return string.IsNullOrEmpty(cur);
        }

        public static bool IsValidABC2(string s) {
            StringBuilder sb = new();
            for (int i = 0; i < s.Length; i++) {
                if (sb.Length < 2) {
                    sb.Append(s[i]);
                } else {
                    if (s[i] != 'c') {
                        sb.Append(s[i]);
                    } else {
                        if (Check(sb, sb.Length - 1)) {
                            sb.Remove(sb.Length - 2, 2);
                        } else {
                            return false;
                        }
                    }
                }
            }

            return sb.Length == 0;

            static bool Check(StringBuilder sb, int idx) {
                return sb[idx] == 'b' && sb[idx - 1] == 'a';
            }
        }

        public static int MaxTotalFruits(int[][] fruits, int startPos, int k) {
            bool twice = false;
            if (k < 0) {
                k = -k;
                twice = true;
            }
            int[] prefix = new int[fruits.Length];
            prefix[0] = fruits[0][1];
            for (int i = 1; i < fruits.Length; i++) {
                prefix[i] = fruits[i][1] + prefix[i - 1];
            }
            int start = Find(fruits, startPos);
            int res = 0;
            bool overlap = start != -1 && fruits[start][0] == startPos;
            if (k == 0) {
                return overlap ? fruits[start][1] : 0;
            }
            for (int i = start; i < fruits.Length; i++) {
                if (i >= 0 && fruits[i][0] - startPos <= k && fruits[i][0] >= startPos) { //向右找
                    int right = prefix[i] - (start == -1 ? 0 : prefix[start]);
                    if (overlap) {
                        right += fruits[start][1];
                    }
                    res = Math.Max(res, right);
                    //再向左找
                    if (start != -1) {
                        int remain = k - 2 * (fruits[i][0] - startPos);
                        if (remain > 0) {
                            //从开始点出发
                            //从始点向左，还可以走remain步，最多能走到：
                            (int to, bool contains) = FindLeft(fruits, overlap ? start : start + 1, remain, startPos);
                            //判断区间是否包含to
                            int left = 0;
                            if (overlap) {
                                left += start < 1 ? 0 : prefix[start - 1] -
                                    (to >= 1 ? prefix[to - 1] : 0);
                            } else {
                                left += prefix[start] -
                                    (to >= 1 ? prefix[to - 1] : 0);
                            }
                            res = Math.Max(res, left + right);
                        }
                    }
                }
            }
            if (!twice) {
                int max = fruits[^1][0];
                for (int i = 0; i < fruits.Length; i++) {
                    fruits[i][0] = max - fruits[i][0];
                }
                for (int i = 0; i < fruits.Length / 2; i++) {
                    (fruits[i][0], fruits[^(i + 1)][0]) = (fruits[^(i + 1)][0], fruits[i][0]);
                    (fruits[i][1], fruits[^(i + 1)][1]) = (fruits[^(i + 1)][1], fruits[i][1]);
                }
                res = Math.Max(res, MaxTotalFruits(fruits, max - startPos, -k));
            }

            return res;

            static int Find(int[][] fruits, int start) {
                int l = -1, r = fruits.Length - 1;
                while (l < r) {
                    int m = (l + r + 1) >> 1;
                    if (fruits[m][0] > start) {
                        r = m - 1;
                    } else {
                        l = m;
                    }
                }
                return l;
            }

            static (int, bool) FindLeft(int[][] fruits, int r, int remainK, int startPos) { //找到能被步数覆盖的最右
                int l = 0;
                int val = startPos - remainK;
                while (l < r) {
                    int m = (l + r) >> 1;
                    if (fruits[m][0] >= val) {
                        r = m;
                    } else {
                        l = m + 1;
                    }
                }
                return (l, fruits[l][0] == val);
            }
        }

        public static int HardestWorker(int n, int[][] logs) {
            for (int i = logs.Length - 1; i >= 1; i--) {
                logs[i][1] -= logs[i - 1][1];
            }

            Array.Sort(logs, delegate (int[] a, int[] b) {
                if (a[1] == b[1]) {
                    return -a[0].CompareTo(b[0]);
                } else {
                    return a[1].CompareTo(b[1]);
                }
            });
            return logs[^1][0];
        }

        public static int PeakIndexInMountainArray(int[] arr) {
            int l = 0, r = arr.Length;
            while (l < r) {
                int m = (l + r) >> 1;
                if (check(arr, m)) {
                    r = m;
                } else {
                    l = m + 1;
                }
            }

            return l;

            bool check(int[] arr, int idx) => idx < arr.Length - 1 && arr[idx] > arr[idx + 1];
        }

        public static int MinNumberOfFrogs(string croakOfFrogs) {
            if (croakOfFrogs.Length < 5 ||
                croakOfFrogs[0] != 'c' && croakOfFrogs[^1] != 'k') {
                return -1;
            }
            Dictionary<char, int> dic = new() { ['c'] = 0, ['r'] = 0, ['o'] = 0, ['a'] = 0, ['k'] = 0 };
            int res = 0;
            for (int i = 0; i < croakOfFrogs.Length; i++) {
                if (croakOfFrogs[i] == 'c') {
                    dic['c']++;
                    dic['k'] = 0;
                    int sum = 0;
                    foreach (var item in dic) {
                        sum += item.Value;
                    }
                    res = Math.Max(res, sum);
                } else {
                    switch (croakOfFrogs[i]) {
                        case 'r':
                            dic['r']++;
                            dic['c']--;
                            if (dic['c'] < 0) {
                                return -1;
                            }
                            break;
                        case 'o':
                            dic['o']++;
                            dic['r']--;
                            if (dic['r'] < 0) {
                                return -1;
                            }
                            break;
                        case 'a':
                            dic['a']++;
                            dic['o']--;
                            if (dic['o'] < 0) {
                                return -1;
                            }
                            break;
                        case 'k':
                            dic['k']++;
                            dic['a']--;
                            if (dic['a'] < 0) {
                                return -1;
                            }
                            break;
                    }
                }
            }
            foreach (var item in dic) {
                if (item.Value != 0 && item.Key != 'k') {
                    return -1;
                }
            }
            return res;
        }

        public static int NumPairsDivisibleBy60(int[] time) {
            int[] count = new int[60];
            int res = 0;
            for (int i = 0; i < time.Length; i++) {
                res += count[(60 - time[i] % 60) % 60];
                count[time[i] % 60]++;
            }
            return res;
        }

        public static int NumIdenticalPairs(int[] nums) {
            Array.Sort(nums);
            int cur1 = 0, cur2 = 0;
            int res = 0;
            while (cur1 < nums.Length && cur2 < nums.Length) {
                while (cur2 < nums.Length && nums[cur2] == nums[cur1]) {
                    cur2++;
                }
                res += (cur2 - cur1) * (cur2 - cur1 - 1) / 2;
                cur1 = cur2;
            }
            return res;
        }

        public static int NumberOfWays(string corridor) {
            if (corridor.Length < 2) {
                return 0;
            }
            const int MOD = 1000000007;
            //我草你妈一直写不出来
            long[] map = new long[corridor.Length];
            Array.Fill(map, -1);
            return (int)(dfs(corridor, 0, map) % MOD);

            static long dfs(string s, int idx, long[] map) {
                //曹尼玛
                // 定义idx：从第idx号开始分成一区，idx元素前有一隔板
                if (map[idx] != -1) {
                    return map[idx];
                }
                int count = 0;
                int i;
                for (i = idx; i < s.Length; i++) {
                    if (s[i] == 'S') {
                        count++;
                        if (count == 2) {
                            break;
                        }
                    }
                }

                if (count == 2) {
                    // 找到了一个座位数是2的有效分区
                    i++;
                    if (i == s.Length) {
                        map[idx] = 1;
                        return 1; // base case
                    }

                    int plants = 0;
                    while (i < s.Length && s[i] != 'S') {
                        plants++;
                        i++;
                    }
                    // 如果往后全tm都是植物
                    if (i == s.Length) {
                        map[idx] = 1;
                        return 1;
                    } else { // 循环跳出时i一定指向一个座位
                        long res = (((plants + 1) % MOD) * (dfs(s, i, map) % MOD)) % MOD;
                        map[idx] = res;
                        return res;
                    }
                } else {
                    return 0;
                    // 从idx开始向后不能有效地分区
                }
            }
        }

        public static int MaximalSquare(char[][] matrix) {
            int[,] dp = new int[matrix.Length, matrix[0].Length];
            //记录最大正方形的面积
            int res = 0;
            for (int i = 0; i < matrix.Length; i++) {
                dp[i, 0] = matrix[i][0] - '0';
                res = Math.Max(res, dp[i, 0]);
            }
            for (int i = 1; i < matrix[0].Length; i++) {
                dp[0, i] = matrix[0][i] - '0';
                res = Math.Max(res, dp[0, i]);
            }
            for (int i = 1; i < matrix.Length; i++) {
                for (int j = 1; j < matrix[0].Length; j++) {
                    if (matrix[i][j] == '0') {
                        dp[i, j] = 0;
                    } else {
                        if (dp[i - 1, j - 1] > 0) {
                            dp[i, j] =
                                (dp[i - 1, j] == dp[i, j - 1] && dp[i, j - 1] == dp[i - 1, j - 1]) ?
                                 (int)(Math.Pow((int)Math.Sqrt(dp[i - 1, j - 1]) + 1, 2)) :
                                 (int)Math.Pow(Math.Min(
                                     (int)Math.Sqrt(dp[i - 1, j - 1]),
                                     Math.Min((int)Math.Sqrt(dp[i - 1, j]), (int)Math.Sqrt(dp[i, j - 1]))) + 1, 2);
                        } else {
                            dp[i, j] = 1;
                        }
                    }

                    res = Math.Max(res, dp[i, j]);
                }
            }
            return res;
        }

        public static int MatchstickEquation(int n) {
            //根据火柴棒数得到有哪些数
            int[] map = { 6, 2, 5, 5, 4, 5, 6, 3, 7, 6 };
            n -= 4;
            int[][] map2 = new int[n + 1][];
            for (int i = 2; i < map2.Length; i++) {
                map2[i] = solve(i, map).ToArray();
            }
            int res = 0;
            for (int i = 2; i < n; i++) { //枚举第一个数的火柴棒数量
                for (int j = 2; j < n - i; j++) {

                    for (int m = 0; m < map2[i].Length; m++) {
                        for (int p = 0; p < map2[j].Length; p++) {
                            if (getCnt(map2[i][m] + map2[j][p], map) + i + j == n) {
                                Console.WriteLine($"{map2[i][m]} + {map2[j][p]} = {map2[i][m] + map2[j][p]}");
                                res++;
                            }
                        }
                    }

                }
            }
            return res;
            static List<int> solve(int sticks, int[] map) { //多少根棒能摆成哪些数字
                if (sticks < 2) {
                    return new();
                }
                if (sticks == 2) {
                    return new() { 1 };
                }
                List<int> res = new();
                if (sticks == map[0]) {
                    res.Add(0);
                }
                for (int i = 1; i < map.Length; i++) {
                    if (map[i] < sticks) {
                        var tmp = solve(sticks - map[i], map);
                        foreach (var item in tmp) {
                            res.Add(item + i * (int)Math.Pow(10, digit(item)));
                        }
                    } else if (map[i] == sticks) {
                        res.Add(i);
                    }
                }
                return res;
            }

            static int digit(int n) { //取在屏幕上显示的位数
                if (n == 0)
                    return 1;
                int res = 0;
                while (n > 0) {
                    n /= 10;
                    res++;
                }
                return res;
            }

            static int getCnt(int num, int[] map) { //需要用多少根棒才能摆出这个数
                if (num == 0) {
                    return map[0];
                }
                int res = 0;
                while (num > 0) {
                    res += map[num % 10];
                    num /= 10;
                }
                return res;
            }
        }

        public static int MaxProfitIII(int[] prices) {
            if (prices.Length == 2) {
                return Math.Max(0, prices[1] - prices[0]);
            }
            if (prices.Length == 1) {
                return 0;
            }
            int bug = prices[^1] - prices[^2];
            int dp4 = prices[^1];
            int dp3 = Math.Max(0, -prices[^2] + dp4); // 第二次交易净亏不如不买
            int dp2 = prices[^2] + dp3;
            int dp1 = -prices[^3] + dp2;
            for (int i = prices.Length - 4; i >= 0; i--) {
                dp4 = Math.Max(dp4, prices[i + 2]);
                dp3 = Math.Max(dp3, -prices[i + 1] + dp4);
                dp2 = Math.Max(dp2, prices[i + 1] + dp3);
                dp1 = Math.Max(dp1, -prices[i] + dp2);
            }
            return Math.Max(0, Math.Max(bug, dp1));
        }

        public static bool QueryString(string s, int n) {
            if (n == 1) {
                return s.Contains('1');
            } else if (n == 2) {
                return s.Contains("10");
            }

            int m = s.Length;                                   // 先查询2^k到k这个区间的数是否存在，
            int digit = (int)Math.Ceiling(Math.Log2(n + 1));     // 再查下一个完整的指数范围的区间的数是否存在，如果都存在那么一定所有二进制数都存在
            if (m - digit + 1 < n - (1 << (digit - 1)) + 1 ||
                m - (digit - 1) + 1 < (1 << digit - 2)) {
                return false;
            }
            return check(s, 1 << (digit - 1), n, digit) &&
                check(s, 1 << (digit - 2), (1 << (digit - 1)) - 1, digit - 1);

            static bool check(string s, int l, int r, int len) {
                HashSet<int> set = new();
                int cur = System.Convert.ToInt32(s[..len], 2);
                int mask = (1 << len) - 1;
                if (cur >= l && cur <= r) {
                    set.Add(cur);
                }
                for (int i = 1; i < s.Length - len + 1; i++) {
                    cur <<= 1;
                    cur |= s[i + len - 1] - '0';
                    cur &= mask;
                    if (cur >= l && cur <= r) {
                        set.Add(cur);
                    }
                }
                return set.Count == r - l + 1;
            }
        }

        public static bool QueryString2(string s, int n) {
            if (n == 1) {
                return s.Contains('1');
            } else if (n == 2) {
                return s.Contains("10");
            }

            int m = s.Length;                                   // 先查询2^k到k这个区间的数是否存在，
            int digit = (int)Math.Ceiling(Math.Log2(n + 1));     // 再查下一个完整的指数范围的区间的数是否存在，如果都存在那么一定所有二进制数都存在
            if (m - digit + 1 < n - (1 << (digit - 1)) + 1 ||
                m - (digit - 1) + 1 < (1 << digit - 2)) {
                //return false;
            }
            return check(s, 1 << (digit - 1), n, digit) &&
                check(s, 1 << (digit - 2), (1 << (digit - 1)) - 1, digit - 1);

            static bool check(string s, int l, int r, int len) {
                HashSet<int> set = new();
                int cur = System.Convert.ToInt32(s[..len], 2);
                int mask = (1 << len) - 1;
                if (cur >= l && cur <= r) {
                    set.Add(cur);
                }
                for (int i = 1; i < s.Length - len + 1; i++) {
                    cur <<= 1;
                    cur |= s[i + len - 1] - '0';
                    cur &= mask;
                    if (cur >= l && cur <= r) {
                        set.Add(cur);
                    }
                }
                return set.Count == r - l + 1;
            }
        }

        public static int NumSubarrayBoundedMax(int[] nums, int left, int right) {
            int start = 0;// 从这个位置开始不存在大于right的数
            int maxIdx = -1; // 最后一个满足条件的数的索引
            // 统计以i作为结尾的子数组数量
            int res = 0;
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] > right) {
                    start = i + 1;
                } else if (nums[i] < left) {
                    if (maxIdx != -1) {
                        res += Math.Max(0, maxIdx - start + 1);
                    }
                } else {
                    res += i - start + 1;
                    maxIdx = i;
                }
            }
            return res;
        }

        public static int FindMaxK(int[] nums) {
            if (nums.Length == 1) {
                return -1;
            }
            Array.Sort(nums);
            int j = nums.Length - 1;
            for (int i = 0; i < nums.Length; i++) {
                while (j > i && nums[j] > -nums[i]) {
                    j--;
                }
                if (nums[j] + nums[i] == 0) {
                    return nums[j];
                }
            }
            return -1;
        }

        public static int MaximumScore(int[] nums, int k) {
            Stack<int> st = new(); // idx, val
            int[] right = new int[nums.Length];
            int[] left = new int[nums.Length];
            Array.Fill(right, nums.Length);
            Array.Fill(left, -1);
            int res = 0;
            // 递增栈
            for (int i = 0; i < nums.Length; i++) {
                while (st.Count > 0 && nums[st.Peek()] > nums[i]) {
                    var tmp = st.Pop();
                    right[tmp] = i; // 找到了第一个比自己小的元素
                }

                st.Push(i);
            }
            st.Clear();
            for (int i = nums.Length - 1; i >= 0; i--) {
                while (st.Count > 0 && nums[st.Peek()] > nums[i]) {
                    var tmp = st.Pop();
                    left[tmp] = i;
                }

                st.Push(i);
            }
            for (int i = 0; i < nums.Length; i++) {
                if (left[i] + 1 <= k && (right[i] - 1 >= k)) {
                    res = Math.Max(res, nums[i] * (right[i] - left[i] - 1));

                }
            }
            return res;
        }

        public static int MaximumScore2(int[] nums, int k) {
            int cur1 = k, cur2 = k;
            int min = nums[k];
            int res = nums[k];
            while (cur1 > 0 || cur2 < nums.Length - 1) {
                res = Math.Max(res, min * (cur2 - cur1 + 1));
                //bool left = cur1 <= 0 ? int.MaxValue : nums[cur1 - 1];
                //bool right = cur2 >= nums.Length - 1 ? int.MaxValue : nums[cur2 + 1];
                bool moveLeft = cur2 >= nums.Length - 1 || cur1 > 0 && nums[cur1 - 1] > nums[cur2 + 1];
                if (moveLeft) {
                    cur1--;
                    min = Math.Min(min, nums[cur1]);
                } else {
                    cur2++;
                    min = Math.Min(min, nums[cur2]);
                }
            }
            res = Math.Max(res, min * nums.Length);
            return res;
        }

        public static int[] RearrangeBarcodes(int[] barcodes) {
            Dictionary<int, int> count = new();
            for (int i = 0; i < barcodes.Length; i++) {
                if (!count.ContainsKey(barcodes[i])) {
                    count[barcodes[i]] = 0;
                }
                count[barcodes[i]]++;
            }
            List<int> record = new(barcodes);
            record.Sort(delegate (int a, int b) {
                if (count[a] == count[b]) {
                    return -a.CompareTo(b);
                }
                return count[b].CompareTo(count[a]);
            });
            List<int> res = new();
            for (int i = 0; i < barcodes.Length / 2; i++) {
                res.Add(record[i]);
                res.Add(record[i + (barcodes.Length + 1) / 2]);
            }
            if ((record.Count & 1) == 1) {
                res.Add(record[record.Count / 2]);
            }

            return res.ToArray();
        }

        public static int OrderOfLargestPlusSign(int n, int[][] mines) {
            HashSet<(int, int)> set = new();
            foreach (var item in mines) {
                set.Add((item[0], item[1]));
            }
            // mines[i][j] == 1 等价于 !set.Contains((i, j))
            static int get(int i, int j, HashSet<(int, int)> set) {
                return set.Contains((i, j)) ? 0 : 1;
            }

            (int, int)[,] dp = new (int, int)[n, n]; // left, up
            dp[0, 0] = (get(0, 0, set), get(0, 0, set));
            (int, int)[,] dp2 = new (int, int)[n, n]; // right, down
            dp[n - 1, n - 1] = (get(n - 1, n - 1, set), get(n - 1, n - 1, set));
            for (int i = 1; i < n; i++) {
                dp[0, i] = (get(0, i, set) == 0 ? 0 : 1 + dp[0, i - 1].Item1, get(0, i, set));
                dp2[n - 1, n - i - 1] =
                    (get(n - 1, n - i - 1, set) == 0 ? 0 : 1 + dp2[n - 1, n - i].Item1, get(n - 1, n - i - 1, set));
            }
            for (int i = 1; i < n; i++) {
                dp[i, 0] = (get(i, 0, set), get(i, 0, set) == 0 ? 0 : 1 + dp[i - 1, 0].Item2);
                dp2[n - i - 1, n - 1] =
                    (get(n - i - 1, n - 1, set), get(n - i - 1, n - 1, set) == 0 ? 0 : 1 + dp2[n - i, n - 1].Item2);
            }
            for (int i = 1; i < n; i++) {
                for (int j = 1; j < n; j++) {
                    if (get(i, j, set) == 0) {
                        dp[i, j] = (0, 0);
                    } else {
                        dp[i, j] = (dp[i, j - 1].Item1 + 1, dp[i - 1, j].Item2 + 1);
                    }

                    if (get(n - i - 1, n - j - 1, set) == 0) {
                        dp2[n - i - 1, n - j - 1] = (0, 0);
                    } else {
                        dp2[n - i - 1, n - j - 1] =
                            (dp2[n - i - 1, n - j].Item1 + 1, dp2[n - i, n - j - 1].Item2 + 1);
                    }
                }
            }
            int res = 0;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    if (get(i, j, set) == 1) {
                        res = Math.Max(res, 1);
                    }
                    if (i > 0 && j > 0 && get(i, j, set) == 1) {
                        int minProbableLen = Math.Min(Math.Min(dp[i, j].Item1, dp[i, j].Item2),
                            Math.Min(dp2[i, j].Item1, dp2[i, j].Item2));
                        res = Math.Max(res, minProbableLen);
                    }
                }
            }

            return res;
        }

        public static int MaxChunksToSortedII(int[] arr) {
            Stack<(int, int)> st = new();
            st.Push((-1, int.MinValue));// i, max
            for (int i = 0; i < arr.Length; i++) {
                if (arr[i] > st.Peek().Item2) {
                    st.Push((i, arr[i]));
                } else {
                    int max = arr[i];
                    while (st.Count > 0 && st.Peek().Item2 > arr[i]) {
                        var tmp = st.Pop();
                        max = Math.Max(max, tmp.Item2);
                    }
                    st.Push((i, max));
                }
            }
            return st.Count - 1;
        }

        public static int MaxEqualRowsAfterFlips(int[][] matrix) {
            Dictionary<string, int> count = new();
            int max = 0;
            for (int i = 0; i < matrix.Length; i++) {
                StringBuilder sb = new();
                int j = 0;
                while (j < matrix[i].Length) {
                    int c = 0;
                    while (j + 1 < matrix[i].Length && matrix[i][j] == matrix[i][j + 1]) {
                        j++;
                        c++;
                    }
                    j++;
                    sb.Append($"{c},");
                }
                string s = sb.ToString();
                if (!count.ContainsKey(s)) {
                    count[s] = 0;
                }
                count[s]++;
                max = Math.Max(max, count[s]);
            }
            return max;
        }

        public static int MinDifficulty(int[] jobDifficulty, int d) {
            Dictionary<(int, int), int> map = new();
            return dfs(jobDifficulty, d, 0, map);

            static int dfs(int[] nums, int d, int startIdx, Dictionary<(int, int), int> map) {
                if (d == nums.Length) {
                    return nums.Sum();
                }
                if (d > nums.Length) {
                    return -1;
                }
                if (map.ContainsKey((d, startIdx))) {
                    return map[(d, startIdx)];
                }
                if (d == 1) {
                    int tmp = 0;
                    for (int i = startIdx; i < nums.Length; i++) {
                        tmp = Math.Max(tmp, nums[i]);
                    }
                    return tmp;
                }

                int max = nums[startIdx];
                int res = int.MaxValue;
                for (int i = startIdx; i < nums.Length - 1; i++) {
                    max = Math.Max(max, nums[i]);
                    int next = dfs(nums, d - 1, i + 1, map);
                    if (next != -1) {
                        res = Math.Min(res, max + next);
                    }
                }
                map[(d, startIdx)] = res == int.MaxValue ? -1 : res;
                return map[(d, startIdx)];
            }
        }

        public static int SlidingPuzzle(int[][] board) {
            // 1 2 3
            // 4 0 5 -> 123405
            const int digit = 6;
            return bfs();

            int bfs() {
                Queue<(int, int)> que = new();
                Queue<(int, int)> que2 = new();
                HashSet<int> visited = new();
                Dictionary<int, int> visited2 = new();
                int bd =
                    9 * 1000000 + board[0][0] * 100000 + board[0][1] * 10000 + board[0][2] * 1000 + board[1][0]
                    * 100 + board[1][1] * 10 + board[1][2];
                visited.Add(bd);
                visited2.Add(9123450, 0);
                if (bd == 9123450) {
                    return 0;
                }
                que.Enqueue((bd, 0));
                que2.Enqueue((9123450, 0));
                while (que.Count > 0 && que2.Count > 0) {
                    var tmp = que.Dequeue();
                    var tmp2 = que2.Dequeue();
                    if (visited2.ContainsKey(tmp.Item1)) {
                        return tmp.Item2 + visited2[tmp.Item1];
                    }

                    var next = generate(tmp.Item1);
                    foreach (var item in next) {
                        if (visited.Add(item)) {
                            que.Enqueue((item, tmp.Item2 + 1));
                        }
                    }

                    var next2 = generate(tmp2.Item1);
                    foreach (var item in next2) {
                        if (visited2.TryAdd(item, tmp2.Item2 + 1)) {
                            que2.Enqueue((item, tmp2.Item2 + 1));
                        }
                    }
                }
                return -1;
            }

            static int[] generate(int s) {
                int idx = find0(s);
                List<int> res = new();
                if (idx < 3) {
                    swapAdd(3);
                    if (idx < 2) {
                        swapAdd(1);
                        if (idx == 1) {
                            swapAdd(-1);
                        }
                    } else {
                        swapAdd(-1);
                    }
                } else {
                    swapAdd(-3);
                    if (idx < 5) {
                        swapAdd(1);
                        if (idx == 4) {
                            swapAdd(-1);
                        }
                    } else {
                        swapAdd(-1);
                    }
                }
                return res.ToArray();

                void swapAdd(int offset) {
                    int idxTen = (int)Math.Pow(10, digit - idx - 1);
                    int osTen = (int)(idxTen / Math.Pow(10, offset));
                    int high = s / idxTen % 10;
                    int low = s / osTen % 10;
                    s -= high * idxTen;
                    s += low * idxTen;
                    s -= low * osTen;
                    s += high * osTen;
                    res.Add(s);
                    (high, low) = (low, high);
                    s -= high * idxTen;
                    s += low * idxTen;
                    s -= low * osTen;
                    s += high * osTen;
                }

                int find0(int n) {
                    int i = 0;

                    while (n % 10 != 0) {
                        i++;
                        n /= 10;
                    }
                    return digit - i - 1;
                }
            }
        }

        public static bool HaveConflict(string[] event1, string[] event2) {
            if (compare(event1[0], event2[0]) <= 0 && compare(event1[1], event2[0]) >= 0) {
                return true;
            }
            if (compare(event1[0], event2[0]) >= 0 && compare(event2[1], event1[0]) >= 0) {
                return true;
            }
            return false;

            static int compare(string t1, string t2) {
                int time1 = ((t1[0] - '0') * 10 + t1[1] - '0') * 60 + ((t1[3] - '0') * 10 + t1[^1] - '0');
                int time2 = ((t2[0] - '0') * 10 + t2[1] - '0') * 60 + ((t2[3] - '0') * 10 + t2[^1] - '0');
                int diff = time1 - time2;
                return diff;
            }
        }

        public static int MostBooked(int n, int[][] meetings) {
            // 空闲且编号是25的房间对应的优先级是"025"，不空闲25号房间也是"025"
            PriorityQueue<int, int> queRoom = new(); // 空闲房间
            for (int i = 0; i < n; i++) {
                queRoom.Enqueue(i, i);
            }
            int[] res = new int[n];
            Array.Sort(meetings, (a, b) => a[0].CompareTo(b[0]));
            PriorityQueue<int, int> queMeet = new();
            Dictionary<int, int> meetToRoom = new();

            for (int i = 0; i < meetings.Length; i++) {
                push(i);
            }

            int max = 0, idx = -1;
            for (int i = n - 1; i >= 0; i--) {
                if (res[i] >= max) {
                    idx = i;
                    max = res[i];
                }
            }
            return idx;

            void push(int idx) {
                // 当前时间：meetings[idx][0]
                //int endedMeet = queMeet.Peek();
                if (queMeet.TryPeek(out int endedMeet, out _)) {
                    while (meetings[endedMeet][1] <= meetings[idx][0]) {
                        queMeet.Dequeue();
                        queRoom.Enqueue(meetToRoom[endedMeet], meetToRoom[endedMeet]);
                        meetToRoom.Remove(endedMeet);

                        if (queMeet.Count > 0) {
                            endedMeet = queMeet.Peek();
                        } else {
                            break;
                        }
                    }
                }

                if (queRoom.Count > 0) {
                    var tmp = queRoom.Dequeue();
                    queMeet.Enqueue(idx, meetings[idx][1]);
                    meetToRoom[idx] = tmp;
                } else {
                    // 没空教室了
                    var idxFirst = queMeet.Dequeue(); // 最先结束的会议
                    var room = meetToRoom[idxFirst]; // 腾出来的房间
                    // 接着这个会议继续往下开
                    meetToRoom[idx] = room;
                    meetings[idx][1] += meetings[idxFirst][1] - meetings[idx][0];
                    meetings[idx][0] = meetings[idxFirst][1];
                    queMeet.Enqueue(idx, meetings[idx][1]);
                }
                res[meetToRoom[idx]]++;
            }
        }

        public static int MinDominoRotations(int[] tops, int[] bottoms) {
            (int, int)[] map = new (int, int)[7];
            fillMap(tops[0]);
            if (bottoms[0] != tops[0]) {
                fillMap(bottoms[0]);
            }

            int res = int.MaxValue;
            int may = tops[0];
            for (int i = 0; i < 2; i++) {
                if (map[may].Item1 + map[may].Item2 > 0) {
                    res = Math.Min(res, Math.Min(tops.Length - map[may].Item1, tops.Length - map[may].Item2));
                    break;
                }
                may = bottoms[0];
            }
            return res == int.MaxValue ? -1 : res;

            void fillMap(int i) {
                for (int j = 0; j < tops.Length; j++) {
                    if (tops[j] == i || bottoms[j] == i) {
                        if (tops[j] == i) {
                            map[i] = (map[i].Item1 + 1, map[i].Item2);
                        }
                        if (bottoms[j] == i) {
                            map[i] = (map[i].Item1, map[i].Item2 + 1);
                        }
                    } else {
                        map[i] = (0, 0);
                        break;
                    }
                }
            }
        }

        public static int[] AddNegabinary(int[] arr1, int[] arr2) {
            Array.Reverse(arr1);
            Array.Reverse(arr2);
            int[] res = new int[Math.Max(arr1.Length, arr2.Length) + 2];
            for (int i = 0; i < res.Length; i++) {
                res[i] = (i < arr1.Length ? arr1[i] : 0) + (i < arr2.Length ? arr2[i] : 0);
                // 2 1 0
                // 3 2 1 0
            }
            for (int i = 0; i < res.Length; i++) {
                if (res[i] >= 2) {
                    carry(i);
                }
            }
            int c;
            for (c = res.Length - 1; c >= 0 && res[c] == 0; c--)
                ;
            if (c == -1) {
                return new int[] { 0 };
            } else {
                for (int i = 0; i < (c + 1) / 2; i++) {
                    (res[i], res[c - i]) = (res[c - i], res[i]);
                }
            }
            return res[..(c + 1)];

            void carry(int idx) {
                int x = res[idx] - 2 * res[idx + 1];
                if (x >= 0 && x <= 2) {
                    res[idx] = x;
                    res[idx + 1] = 0;
                    if (x != 2) {
                        return;
                    }
                }

                int times = res[idx] / 2;
                res[idx + 1] += times;
                res[idx + 2] += times;
                res[idx] &= 1;

            }
        }

        // public static int[] AddNegabinary(int[] arr1, int[] arr2) {
        //     int n = arr1.Length, m = arr2.Length;
        //     int[] res = new int[n + m + 1];
        //     for (int i = res.Length - 1; i >= 0; i--) {

        //     }
        // }


        public static int NumTilePossibilities(string tiles) {
            var chosen = choose(tiles, 0);
            HashSet<string> res = new();

            foreach (var item in chosen) {
                StringBuilder sb = new(item);
                backtracking(sb, 0);
            }

            return res.Count - 1;

            void backtracking(StringBuilder sb, int startIdx) {
                if (startIdx == sb.Length) {
                    res.Add(sb.ToString());
                    return;
                }

                for (int i = startIdx; i < sb.Length; i++) {
                    if (!(sb[startIdx] == sb[i] && startIdx != i)) {
                        (sb[startIdx], sb[i]) = (sb[i], sb[startIdx]);
                        backtracking(sb, startIdx + 1);
                        (sb[startIdx], sb[i]) = (sb[i], sb[startIdx]);
                    }
                }
            }

            HashSet<string> choose(string s, int startIdx) {
                if (startIdx == s.Length - 1) {
                    HashSet<string> n = new() {
                        "",
                        s[startIdx].ToString()
                    };
                    return n;
                }
                HashSet<string> res = new();
                var case1 = choose(s, startIdx + 1);

                foreach (var item in case1) {
                    res.Add(item);
                    res.Add(s[startIdx].ToString() + item);
                }
                return res;
            }
        }

        public static int MaxSumBST(TreeNode root) {
            int res = 0;
            f(root);
            return res;

            // 自己是否是bst，最大值，最小值，和，isNull
            (bool, int, int, int, bool) f(TreeNode? node) {
                if (node is null) {
                    return (true, 0, 0, 0, true);
                }

                bool isBST = false;
                var l = f(node.left);
                var r = f(node.right);
                int min = 0, max = 0;

                if (l.Item5 && r.Item5) {
                    isBST = true;
                    min = max = node.val;
                } else if (l.Item5) {
                    isBST = r.Item1 && r.Item3 > node.val;
                    min = Math.Min(node.val, r.Item3);
                    max = node.val;
                } else if (r.Item5) {
                    isBST = l.Item1 && l.Item2 < node.val;
                    min = node.val;
                    max = Math.Max(node.val, l.Item2);
                } else {
                    if (l.Item1 && r.Item1 && l.Item2 < node.val && r.Item3 > node.val) {
                        isBST = true;
                        min = Math.Min(l.Item3, node.val);
                        max = Math.Max(node.val, r.Item2);
                    }
                }
                if (isBST) {
                    res = Math.Max(res, l.Item4 + r.Item4 + node.val);
                }
                return (isBST, max, min, l.Item4 + node.val + r.Item4, false);

            }
        }

        public static bool CheckValidString(string s) {
            return f(s, '(', ')') && f(string.Concat(s.Reverse()), ')', '(');

            static bool f(string s, char add, char remove) {
                int score1 = 0;
                int score2 = 0;
                for (int i = 0; i < s.Length; i++) {
                    if (s[i] == add) {
                        score1 += 1;
                        score2 += 1;
                    } else if (s[i] == remove) {
                        score1 += -1;
                        score2 += -1;
                        if (score2 < 0) {
                            return false;
                        }
                    } else {
                        score1--;
                        if (score1 < 0) {
                            score1++;
                        }
                        score2++;
                    }
                }
                return score1 <= 0 && score2 >= 0;
            }
        }

        public static int[] FindMode(TreeNode root) {
            int maxCount = 0;
            List<int> res = new();
            int count = 0;
            int last = 0;
            f(root);
            return res.ToArray();

            void f(TreeNode? node) {
                if (node is null) {
                    return;
                }

                f(node.left);
                if (last == node.val) {
                    count++;
                    if (count >= maxCount) {
                        if (count == maxCount) {
                            if (res.Count <= 0 || res[res.Count - 1] != node.val) {
                                res.Add(node.val);
                            }
                        } else {
                            res.Clear();
                            res.Add(node.val);
                        }
                        maxCount = count;
                    }
                } else {
                    count = 1;
                    if (count > maxCount) {
                        maxCount = count;
                        res.Clear();
                        res.Add(node.val);
                    } else if (count == maxCount) {
                        res.Add(node.val);
                    }
                    last = node.val;
                }
                f(node.right);
            }
        }

        public static int MirrorReflection(int p, int q) {
            int lcm = p * q / gcd(p, q);
            bool left = ((lcm / q) & 1) != 1;
            bool top = ((lcm / p) & 1) == 1;
            if (left && top) {
                return 2;
            } else if (!left && top) {
                return 1;
            } else if (!left && !top) {
                return 0;
            } else {
                return 3;
            }

            int gcd(int a, int b) {
                if (b <= a) {
                    return a % b == 0 ? b : gcd(b, a % b);
                } else {
                    return gcd(b, a);
                }
            }
        }

        public static int StoreWater(int[] bucket, int[] vat) {
            PriorityQueue<(int, int), int> que = new();
            int onlyDivideMax = 0; // 升级过容量或者没升级只倒水，需要多少次
            int updateCount = 0;
            int res = int.MaxValue;
            for (int i = 0; i < bucket.Length; i++) {
                if (bucket[i] == 0 && vat[i] != 0) { // 如果容量是0，但要求装一个正整数的水，那就必须升级
                    bucket[i]++;
                    updateCount++;
                }
            }

            for (int i = 0; i < bucket.Length; i++) {
                if (vat[i] != 0) {
                    que.Enqueue((vat[i], bucket[i]), -(int)Math.Ceiling(vat[i] * 1.0 / bucket[i]));
                    onlyDivideMax = Math.Max(onlyDivideMax, (int)Math.Ceiling(vat[i] * 1.0 / bucket[i]));
                }
            }
            res = Math.Min(res, onlyDivideMax + updateCount);

            while (que.Count > 0) {
                var slowest = que.Dequeue();
                int decreaseTo = -1 + (int)Math.Ceiling(slowest.Item1 * 1.0 / slowest.Item2); //应该降到这里，再计算一遍res
                if (decreaseTo <= 0) {
                    break;
                }
                int bucketTo = (int)Math.Ceiling(slowest.Item1 * 1.0 / decreaseTo);
                que.Enqueue((slowest.Item1, bucketTo), -(int)Math.Ceiling(slowest.Item1 * 1.0 / bucketTo));

                var newSlowest = que.Peek();
                onlyDivideMax = (int)Math.Ceiling(newSlowest.Item1 * 1.0 / newSlowest.Item2);
                updateCount += bucketTo - slowest.Item2;
                res = Math.Min(res, updateCount + onlyDivideMax);

            }
            return res;
        }

        public static TreeNode? SufficientSubset(TreeNode root, int limit) {
            int max = f(root, limit);
            if (max < limit) {
                return null;
            } else {
                return root;
            }

            // 返回最大路径和
            int f(TreeNode node, int limit) {
                if (node.left is null && node.right is null) {
                    return node.val;
                } else if (node.left is null) {
                    return node.val + f(node.right!, limit - node.val);
                } else if (node.right is null) {
                    return node.val + f(node.left!, limit - node.val);
                } else {
                    int l = node.val + f(node.left, limit - node.val);
                    int r = node.val + f(node.right, limit - node.val);
                    if (l < limit) { node.left = null; }
                    if (r < limit) { node.right = null; }
                    int curMax = Math.Max(l, r);
                    return curMax;
                }
            }
        }

        public static double FrogPosition2(int n, int[][] edges, int t, int target) {
            List<int>[] graph = new List<int>[n + 1];
            if (n == 1) {
                return target == 1 && t >= 1 ? 1 : 0;
            }
            for (int i = 0; i < edges.Length; i++) {
                if (graph[edges[i][0]] is null) {
                    graph[edges[i][0]] = new();
                }
                if (graph[edges[i][1]] is null) {
                    graph[edges[i][1]] = new();
                }
                graph[edges[i][0]].Add(edges[i][1]);
                graph[edges[i][1]].Add(edges[i][0]);
            }
            bool[] hash = new bool[n + 1];
            hash[1] = true;
            return dfs(1, t);

            double dfs(int s, int t) {
                int count = s == 1 ? graph[s].Count : graph[s].Count - 1;
                if (t == 0) {
                    return s == target ? 1 : 0;
                }
                if (count == 0) {
                    return s == target ? 1 : 0;
                }
                double possible = 0;
                for (int i = 0; i < graph[s].Count; i++) {
                    double tmp = 1;
                    if (!hash[graph[s][i]]) {
                        hash[graph[s][i]] = true;
                        tmp = dfs(graph[s][i], t - 1);
                        if (tmp != 0) {
                            possible = 1.0 / count;
                            possible *= tmp;
                            break;
                        }
                        hash[graph[s][i]] = false;
                    }
                }
                return possible;
            }
        }

        public static int[] InsertSort(int[] arr) {
            for (int i = 1; i < arr.Length; i++) {
                int tmp = arr[i];
                int j;
                for (j = i - 1; j >= 0; j--) {
                    if (arr[j] > tmp) {
                        arr[j + 1] = arr[j];
                    } else {
                        break;
                    }
                }
                arr[j + 1] = tmp;
            }
            return arr;
        }

        public static int ShortestPathBinaryMatrix(int[][] grid) {
            if (grid[0][0] == 1) {
                return -1;
            }

            int[][] dire = new int[8][] { new int[] { 1, 1 }, new int[] { 1, -1 }, new int[] { 1, 0 },
                new int[] { -1, 1 }, new int[] { -1, -1 }, new int[] { -1, 0 },
                new int[] { 0, 1 }, new int[] { 0, -1 }
                };
            Queue<(int, int, int)> que = new();// , , count
            bool[,] set = new bool[grid.Length, grid[0].Length];
            set[0, 0] = true;
            que.Enqueue((0, 0, 1));
            bool has = false;
            int res = int.MinValue;
            while (que.Count > 0) {
                var tmp = que.Dequeue();
                if (tmp.Item1 == grid.Length - 1 && tmp.Item2 == grid[0].Length - 1) {
                    res = tmp.Item3;
                    has = true;
                    break;
                }
                push(tmp);
            }
            return has ? res : -1;

            void push((int, int, int) tmp) {

                for (int i = 0; i < dire.Length; i++) {
                    var cur = (tmp.Item1 + dire[i][0], tmp.Item2 + dire[i][1], tmp.Item3 + 1);
                    if (cur.Item1 >= 0 && cur.Item1 < grid.Length && cur.Item2 >= 0 && cur.Item2 < grid[0].Length
                        && grid[cur.Item1][cur.Item2] == 0
                        && !set[cur.Item1, cur.Item2]) {

                        set[cur.Item1, cur.Item2] = true;
                        que.Enqueue(cur);
                    }
                }
            }
        }

        public static int MaxRotateFunction(int[] nums) {
            int sum = nums.Sum();
            int res = 0;
            int last = 0;
            for (int i = 0; i < nums.Length; i++) {
                last += i * nums[i];
            }
            res = last;
            for (int i = 1; i < nums.Length; i++) {
                last = last - sum + nums.Length * nums[i - 1];
                res = Math.Max(res, last);
            }
            return res;
        }

        public static bool IsIdealPermutation(int[] nums) {
            int min = -1;
            for (int i = 0; i < nums.Length;) {
                if (nums[i] < min) {
                    return false;
                }
                if (i + 1 < nums.Length) {
                    if (Math.Abs(nums[i] - nums[i + 1]) == 1) { //也可以增区间
                        min = Math.Min(nums[i], nums[i + 1]);
                        i += 2;
                    } else {
                        if (nums[i + 1] - nums[i] > 2) {
                            return false;
                        } else {
                            min = nums[i];
                            i++;
                        }
                    }
                } else {
                    break;
                }
            }
            return true;
        }

        public static int NumberOfWays2(int startPos, int endPos, int k) {
            if (((k - endPos + startPos) & 1) == 1 || ((k + endPos - startPos) & 1) == 1) {
                return 0;
            }
            checked {
                int a = (k - endPos + startPos) >> 1;
                int b = (k + endPos - startPos) >> 1;
                const int MOD = 1000000007;
                if (a < 0 || b < 0) {
                    return 0;
                }

                int min = Math.Min(a, b);
                int max = Math.Max(a, b);
                long res = 0;

                int[,] comb = new int[Math.Max(a + 2, b), a + 2];
                for (int i = 0; i < comb.GetLength(0); i++) {
                    comb[i, 1] = i;
                    comb[i, 0] = 1;
                } // 初始化
                for (int n = 2; n < comb.GetLength(1); n++) {
                    for (int m = n; m < comb.GetLength(0); m++) {
                        comb[m, n] = (comb[m - 1, n - 1] % MOD + comb[m - 1, n] % MOD) % MOD;
                    }
                }
                for (int i = 1; i <= min + 1; i++) {
                    res += (comb[min + 1, i] % MOD * (long)(comb[max - 1, i - 1] % MOD)) % MOD;
                }
                return (int)(res % MOD);
            }
        }

        public static NodeNext Connect(NodeNext root) {
            f(root);
            return root;

            void f(NodeNext? node) {
                if (node is null) {
                    return;
                }
                if (node.left is not null) {
                    g(node.left, node.right);
                }
                f(node.left);
                f(node.right);
            }

            void g(NodeNext? left, NodeNext? right) {
                if (left is null) {
                    return;
                }
                if (left.next is not null && right is null) { return; }
                left.next = right;
                if (right is not null) {
                    g(left.left, right.right);
                    g(left.left, right.left);
                    g(left.right, right.right);
                    g(left.right, right.left);
                }
            }
        }

        public static IList<TreeNode> DelNodes(TreeNode root, int[] to_delete) {
            HashSet<int> set = new(to_delete);
            return f(root);

            List<TreeNode> f(TreeNode? node) {
                if (node is null) {
                    return new();
                }
                List<TreeNode> res = new();
                var left = f(node.left);
                var right = f(node.right);

                if (set.Contains(node.val)) {
                    res.AddRange(left);
                    res.AddRange(right);
                } else {
                    // 当前节点不被删除

                    TreeNode? leftTree = null;
                    List<TreeNode> remainLeft = new(); // 除了连通的还有其他树节点
                    List<TreeNode> remainRight = new(); // 除了连通的还有其他树节点

                    TreeNode? rightTree = null;
                    if (node.left is not null) {
                        foreach (var item in left) {
                            if (item.val == node.left.val) {
                                leftTree = item;
                            } else {
                                remainLeft.Add(item);
                            }
                        }
                    }
                    if (node.right is not null) {
                        foreach (var item in right) {
                            if (item.val == node.right.val) {
                                rightTree = item;
                            } else {
                                remainRight.Add(item);
                            }
                        }
                    }

                    TreeNode cur = new(node.val);
                    cur.left = leftTree;
                    cur.right = rightTree;
                    res.Add(cur);

                    res.AddRange(remainLeft);
                    res.AddRange(remainRight);
                }

                return res;
            }
        }

        public static int MaximumTastiness(int[] price, int k) {
            Array.Sort(price);
            int l = 0;
            int r = price[^1] - price[0];
            int m;
            //for (int i = 0; i <= r; i++) {
            //    Console.WriteLine(has(i));
            //}
            while (l < r) {
                m = (l + r + 1) >> 1;
                if (!has(m)) {
                    r = m - 1;
                } else {
                    l = m;
                }
            }
            return l;

            bool has(int res) {
                int pre = 0;
                int cur = 1;
                int count = 1;
                while (count < k) {
                    while (cur < price.Length && price[cur] - price[pre] < res) {
                        cur++;
                    }
                    if (cur < price.Length) {
                        count++;
                        pre = cur;
                        cur++;
                    } else {
                        return false;
                    }
                }
                return true;
            }
        }

        public static int[] VowelStrings(string[] words, int[][] queries) {
            int[] arr = new int[words.Length];
            HashSet<char> alphabet = new() { 'a', 'e', 'i', 'o', 'u' };
            arr[0] = alphabet.Contains(words[0][0]) && alphabet.Contains(words[0][^1]) ? 1 : 0;
            for (int i = 1; i < words.Length; i++) {
                arr[i] = arr[i - 1];
                if (alphabet.Contains(words[i][0]) && alphabet.Contains(words[i][^1])) {
                    arr[i] += 1;
                }
            }

            int[] res = new int[queries.Length];
            for (int i = 0; i < queries.Length; i++) {
                res[i] = arr[queries[i][1]] - (queries[i][0] == 0 ? 0 : arr[queries[i][0] - 1]);
            }
            return res;
        }

        public static int MaxRepOpt1(string text) {
            return Math.Max(f(text), f(string.Concat(text.Reverse())));

            static int f(string s) {
                int[,,] dp = new int[s.Length, 2, 26];
                for (int i = 0; i < 26; i++) {
                    dp[0, 0, i] = s[0] - 'a' == i ? 1 : 0;
                    dp[0, 1, i] = s[0] - 'a' == i ? 0 : 1;
                } // 初始化完毕
                int res = 1;
                for (int i = 1; i < s.Length; i++) {
                    for (int c = 0; c < 26; c++) {
                        if (s[i] - 'a' == c) {
                            dp[i, 0, c] = dp[i - 1, 0, c] + 1;
                            dp[i, 1, c] = dp[i - 1, 1, c] + 1;
                        } else {
                            dp[i, 0, c] = 0;
                            dp[i, 1, c] = dp[i - 1, 0, c] + 1;
                        }
                        res = Math.Max(res, dp[i, 0, c]);
                    }
                }
                for (int i = 1; i < s.Length; i++) {
                    for (int c = 0; c < 26; c++) {
                        dp[i, 1, c] = Math.Max(dp[i, 1, c], dp[i - 1, 1, c]);
                    }
                }
                for (int i = 1; i < s.Length; i++) {
                    res = Math.Max(res, dp[i - 1, 1, s[i] - 'a']);
                }
                return res;
            }
        }

        public static int BagOfTokensScore(int[] tokens, int power) {
            Array.Sort(tokens);
            int cur1 = 0, cur2 = tokens.Length; // 定义：cur1即将要被使用，cur2已被使用
            int score = 0;
            int res = 0;
            while (cur1 < cur2) {
                while (cur1 < cur2 && power >= tokens[cur1]) {
                    score++;
                    power -= tokens[cur1];
                    cur1++;
                }
                res = Math.Max(res, score);
                if (cur1 >= tokens.Length) {
                    break;
                }
                power += tokens[--cur2];
                score--;
                if (score < 0) {
                    break;
                }
            }
            return res;
        }

        public static int DistinctAverages(int[] nums) {
            Array.Sort(nums);
            int l = 0, r = nums.Length - 1;
            HashSet<float> set = new();
            while (l < r) {
                set.Add((nums[l] + nums[r]) / 2f);
                l++;
                r--;
            }
            return set.Count;
        }

        public static int MinSubarray(int[] nums, int p) {
            int want = 0;
            for (int i = 0; i < nums.Length; i++) {
                want = (want + nums[i]) % p;
            }

            if (want == 0) {
                return 0;
            }

            Dictionary<int, int> modArr = new();
            int res = nums.Length;
            int prefix = nums[0] % p;
            for (int i = 0; i < nums.Length; i++) {
                int cur = int.MaxValue;
                if (prefix == want && i < nums.Length - 1) {
                    cur = i + 1;
                }
                //if (prefix >= want) {
                //    if (modArr.ContainsKey(prefix - want)) {
                //        cur = i - modArr[prefix - want];
                //    }
                //} else {
                //    if (modArr.ContainsKey(prefix - want + p)) {
                //        cur = i - modArr[prefix - want + p];
                //    }
                //}
                if (modArr.ContainsKey((prefix - want + p) % p)) {
                    cur = i - modArr[(prefix - want + p) % p];
                }
                res = Math.Min(res, cur);
                modArr[prefix] = i;
                if (i + 1 < nums.Length) {
                    prefix = (prefix + nums[i + 1] % p) % p;
                }
            }
            return res == nums.Length ? -1 : res;
        }

        public static int[] ApplyOperations(int[] nums) {
            int[] res = new int[nums.Length];
            for (int i = 0; i < nums.Length - 1; i++) {
                if (nums[i] == nums[i + 1]) {
                    nums[i] <<= 1;
                    nums[i + 1] = 0;
                }
            }
            for (int i = 0, j = 0; i < nums.Length; i++) {
                if (nums[i] != 0) {
                    res[j++] = nums[i];
                }
            }
            return res;
        }

        public static int EqualPairs(int[][] grid) {
            Dictionary<string, int> dict = new();
            for (int i = 0; i < grid.Length; i++) {
                StringBuilder sb = new();
                for (int j = 0; j < grid[i].Length; j++) {
                    sb.Append(grid[i][j].ToString());
                    sb.Append(',');
                }
                if (!dict.ContainsKey(sb.ToString())) {
                    dict.Add(sb.ToString(), 0);
                }
                dict[sb.ToString()]++;
            }
            int res = 0;
            for (int i = 0; i < grid[0].Length; i++) {
                StringBuilder sb = new();
                for (int j = 0; j < grid.Length; j++) {
                    sb.Append(grid[j][i].ToString());
                    sb.Append(',');
                }
                if (dict.ContainsKey(sb.ToString())) {
                    res += dict[sb.ToString()];
                }
            }
            return res;
        }

        public static int MiceAndCheese(int[] reward1, int[] reward2, int k) {
            int satisfy = 0;
            int sum = 0;

            for (int i = 0; i < reward1.Length; i++) {
                sum += max(i) ? reward1[i] : reward2[i];
                if (max(i)) {
                    satisfy++;
                }
            }

            // 直接return max的sum
            // 选择一个是max的元素，从max里减去两个差值最小的
            int minus = 0;
            List<int> diff = new();
            for (int i = 0; i < reward1.Length; i++) {
                if (satisfy >= k ? max(i) : !max(i)) {
                    diff.Add(Math.Abs(reward2[i] - reward1[i]));
                }
            }
            diff.Sort();
            for (int i = 0; i < Math.Abs(k - satisfy); i++) {
                minus += diff[i];
            }
            return sum - minus;

            bool max(int i) => reward1[i] >= reward2[i];
        }

        public static long MatrixSumQueries(int n, int[][] queries) {
            long sum = 0;
            HashSet<int> cols = new();
            HashSet<int> rows = new();
            for (int i = queries.Length - 1; i >= 0; i--) {
                if (queries[i][0] == 0 && !rows.Contains(queries[i][1])) {
                    // 有多少个列挡着
                    sum += queries[i][2] * (n - cols.Count);
                    rows.Add(queries[i][1]);
                } else if (queries[i][0] == 1 && !cols.Contains(queries[i][1])) {
                    sum += queries[i][2] * (n - rows.Count);
                    cols.Add(queries[i][1]);
                }
            }
            return sum;
        }

        public static IList<int> GrayCode(int n) {
            List<int> res = new(1 << n) { 0, 1 };
            for (int i = 1; i < n; i++) {
                int curLen = res.Count;
                int mask = 1 << i;
                for (int j = 0; j < curLen; j++) {
                    res.Add(mask + res[curLen - j - 1]);
                }
            }
            return res;
        }

        public static int[] NumSmallerByFrequency(string[] queries, string[] words) {
            int[] freq = new int[words.Length];
            for (int i = 0; i < words.Length; i++) {
                freq[i] = calc(words[i]);
            }
            Array.Sort(freq);

            int[] res = new int[queries.Length];
            for (int i = 0; i < queries.Length; i++) {
                res[i] = words.Length - (search(calc(queries[i]), freq) + 1);
            }
            return res;

            static int calc(string s) {
                int minFreq = 0;
                char min = s[0];
                foreach (char c in s) {
                    if (c == min) {
                        minFreq++;
                    } else if (c < min) {
                        min = c;
                        minFreq = 1;
                    }
                }
                return minFreq;
            }

            static int search(int val, int[] words) {
                int l = -1, r = words.Length - 1;
                int m;
                while (l < r) {
                    m = (l + r + 1) >> 1;
                    if (words[m] <= val) {
                        l = m;
                    } else {
                        r = m - 1;
                    }
                }
                return l;
            }
        }

        public static ListNode RemoveZeroSumSublists(ListNode head) {
            ListNode dummy = new(0) {
                next = head
            };
            Dictionary<int, ListNode> prefix = new() {
                { 0, dummy }
            }; // prefix -> node
            int last = 0;
            ListNode? cur = head;
            while (cur != null) {
                last += cur.val;
                prefix[last] = cur;
                cur = cur.next;
            }

            cur = dummy;
            last = 0;
            while (cur != null) { // 已知前缀和是x的最后一个节点，可以一下找出最长的和是0的子数组
                last += cur.val;
                cur.next = prefix[last].next;
                cur = cur.next;
            }
            return dummy.next;
        }

        public static bool IsMatch(string s, string p) {
            int patFrom = 0;
            int strFrom = 0; // 文本串匹配的起始位置
            if (p.Length == 0) {
                return s.Length == 0;
            }
            //第一阶段，检查头部，必须完全匹配
            for (int i = 0; i <= p.Length; i++) {
                if (i == p.Length || p[i] == '*') {
                    if (i == p.Length && i != s.Length) { // 长度不匹配
                        return false;
                    }
                    if (!match(p[patFrom..i], strFrom)) {
                        return false;
                    }
                    strFrom = i;
                    while (i < p.Length && p[i] == '*') {
                        i++;
                    }
                    patFrom = i;
                    break;
                }
            }
            // 检查尾部，必须完全匹配
            int cur = -1;
            for (int i = p.Length - 1; i >= 0; i--) {
                if (p[i] == '*') {
                    cur = i;
                    break;
                }
            }
            if (!match(p[(cur + 1)..], s.Length - (p.Length - 1 - cur))) {
                return false;
            }
            // 通过首尾位置检验
            // from: 范围开始的位置，不是*的位置
            // 开始检查除头部之外的部分
            while (patFrom < p.Length) {
                int to = p.Length;
                for (int i = patFrom; i < p.Length; i++) {
                    if (p[i] == '*') {
                        to = i;
                        break;
                    }
                }
                // 保证to是第一个星号或是模式串的末尾
                int next = matchRange(p[patFrom..to], strFrom, s.Length);
                if (next < 0) {
                    return false;
                } else {
                    strFrom = next;
                    patFrom = to + 1;
                    while (patFrom < p.Length && p[patFrom] == '*') {
                        patFrom++;
                    }
                    // 跳转到星号的后面
                }
            }


            return true;

            // 返回下一个应该开始匹配的位置
            int matchRange(string p, int i, int j) {  // str文本串上的左闭右开[i..len)区间
                if (i + p.Length == j) {
                    if (match(p, j - p.Length)) {
                        return j;
                    } else {
                        return -1;
                    }
                }
                for (int start = i; start + p.Length <= j; start++) {
                    if (match(p, start)) {
                        return start + p.Length;
                    }
                }
                return -1;
            }

            bool match(string p, int startIdx) {
                if (startIdx < 0) {
                    return false;
                }
                for (int i = 0; i < p.Length; i++) {
                    if (startIdx + i >= s.Length || p[i] != '?' && p[i] != s[startIdx + i]) {
                        return false;
                    }
                }
                return true;
            }
        }

        public static int NumTimesAllBlue(int[] flips) {
            int sum = 0;
            int rightSum = 0;
            int res = 0;
            for (int i = 0; i < flips.Length; i++) {
                sum += flips[i];
                rightSum += i + 1;
                if (sum == rightSum) {
                    res++;
                }
            }
            return res;
        }

        public static string LastSubstring(string s) {
            int i = 0; // 已知字典序最大的子串的起始位置
            int j = 1; // 当前要决策的位置
            int k = 0; // 扩展的字符数量

            while (j + k < s.Length) {
                if (s[i + k] == s[j + k]) {
                    k++;
                } else if (s[i + k] < s[j + k]) {
                    // 说明当前决策到的位置的子串更大
                    i = i + k + 1;
                    k = 0;
                    if (j <= i) {
                        j = i + 1;
                    }
                } else {
                    j = j + k + 1;
                    k = 0;
                }
            }
            return s[i..];
        }

        public static IList<bool> CanMakePaliQueries(string s, int[][] queries) {
            int[,] prefix = new int[s.Length, 26];
            prefix[0, s[0] - 'a']++;
            for (int i = 1; i < s.Length; i++) {
                for (int j = 0; j < 26; j++) {
                    prefix[i, j] = prefix[i - 1, j];
                }
                prefix[i, s[i] - 'a']++;
            }
            bool[] res = new bool[queries.Length];
            for (int i = 0; i < queries.Length; i++) {
                int[] cur = new int[26];
                for (int j = 0; j < 26; j++) {
                    cur[j] = prefix[queries[i][1], j] - (queries[i][0] == 0 ? 0 : prefix[queries[i][0] - 1, j]);
                }
                res[i] = canRecombPalin(cur, queries[i][2], queries[i][1] - queries[i][0] + 1);
            }
            return res;

            bool canRecombPalin(int[] alphabet, int k, int sum) {
                int n = alphabet.Count((x) => (x & 1) == 1);
                if ((sum & 1) == 0) {
                    return k >= (n >> 1);
                } else {
                    if (n < 26) {
                        if (n % 2 != 0) {
                            if (k >= n) {
                                return true;
                            }
                        }
                    }
                    if (n > 0) {
                        if (k >= (n - 1) / 2) {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public static IList<bool> CanMakePaliQueries2(string s, int[][] queries) {
            int[] prefix = new int[s.Length];
            const int MAX = 1 << 26;
            prefix[0] = 1 << (s[0] - 'a');
            for (int i = 1; i < s.Length; i++) {
                prefix[i] = prefix[i - 1];
                prefix[i] ^= 1 << (s[i] - 'a');
            }
            bool[] res = new bool[queries.Length];
            for (int i = 0; i < queries.Length; i++) {
                res[i] = canRecombPalin(prefix[queries[i][1]] ^ (queries[i][0] == 0 ? 0 : prefix[queries[i][0] - 1]),
                    queries[i][2]);
            }
            return res;

            bool canRecombPalin(int count, int k) {
                int n = 0;
                int tmp = count;
                while (tmp > 0) {
                    n++;
                    tmp &= tmp - 1;
                }
                //if ((sum & 1) == 0) {
                //    return k >= (n >> 1);
                //} else {
                //    if (count < MAX) {
                //        if ((n & 1) != 0) {
                //            if (k >= n) {
                //                return true;
                //            }
                //        }
                //    }
                //    if (count > 0) {
                //        if (k >= (n - 1) / 2) {
                //            return true;
                //        }
                //    }
                //}

                //return false;
                return k >= (n >> 1); // 频数是奇数的字符可以互相转化直到变为均为偶数
            }
        }

        public static int MinNumberOfSemesters(int n, int[][] relations, int k) {
            int u = (1 << (n + 1)) - 2;
            int[] map = new int[u + 1];
            Array.Fill(map, -1);
            return f(u); // 刚开始是满课供自由选择子集

            int f(int curCourage) {
                if (map[curCourage] > -1) {
                    return map[curCourage];
                }
                if (curCourage == 0) {
                    return 0;
                }
                int head = 0;
                for (int i = 0; i < relations.Length; i++) {
                    if ((curCourage & (1 << relations[i][1])) > 0 && (curCourage & (1 << relations[i][0])) > 0) {
                        head |= (1 << relations[i][1]);
                    }
                }
                head = curCourage & (u ^ head);
                int res = int.MaxValue;
                // 从head里面挑选子集
                if (count(head) == k) {
                    map[curCourage] = 1 + f(curCourage ^ head);
                    return map[curCourage];
                }
                for (int subset = head; subset > 0; subset = (subset - 1) & head) {
                    if (count(subset) <= k) {
                        res = Math.Min(1 + f(curCourage ^ subset), res);
                    }
                }
                map[curCourage] = res;
                return map[curCourage];
            }

            static int count(int n) {
                int res = 0;
                while (n > 0) {
                    n &= n - 1;
                    res++;
                }
                return res;
            }
        }

        public static int ClosedIsland(int[][] grid) {
            int[][] direct = new int[][] { new int[] { 1, 0 }, new int[] { 0, 1 }, new int[] { -1, 0 }, new int[] { 0, -1 } };
            int count = 2;
            for (int i = 1; i < grid.Length - 1; i++) {
                for (int j = 1; j < grid[i].Length - 1; j++) {
                    if (grid[i][j] == 0) {
                        if (f(i, j)) {
                            count++;
                        }
                    }
                }
            }
            return count - 2;

            bool f(int i, int j) { // 返回能不能扩展出一个封闭岛
                if (i < 0 || i >= grid.Length || j < 0 || j >= grid[0].Length) {
                    return false;
                }
                if (grid[i][j] != 0) {
                    return true; // 扩展出一个空的封闭岛
                }
                grid[i][j] = count;
                bool can = true;
                for (int m = 0; m < direct.Length; m++) {
                    can &= f(i + direct[m][0], j + direct[m][1]);
                    // if (!can) { break; } 提前短路会导致不把所有联通区域都算入，进而多出来区域
                }

                return can;
            }
        }

        public static int MaxSumDivThree(int[] nums) {
            // int[,] mod = new int[nums.Length, 3];
            int[] mod = new int[3];
            for (int i = 0; i < 3; i++) {
                if (nums[^1] % 3 == i) {
                    mod[i] = nums[^1];
                } else {
                    mod[i] = -1;
                }
            }

            for (int i = nums.Length - 2; i >= 0; i--) {
                int[] cur = new int[3];
                for (int k = 0; k < 3; k++) {
                    if (mod[(3 + k - nums[i] % 3) % 3] == -1) {
                        if (nums[i] % 3 == k) {
                            cur[k] = Math.Max(mod[k], nums[i]);
                        } else {
                            cur[k] = Math.Max(mod[k], -1);
                        }
                    } else {
                        cur[k] = Math.Max(mod[k], nums[i] + mod[(3 + k - nums[i] % 3) % 3]);
                    }
                }
                Array.Copy(cur, mod, 3);
            }
            return mod[0] == -1 ? 0 : mod[0];
        }

        public static int ConnectTwoGroups(IList<IList<int>> cost) {
            int initialState = (1 << cost[0].Count) - 1;
            Dictionary<(int, int), int> map = new();
            // f(0, initialState);
            return f(0, initialState);


            int f(int startIdx, int remain) {
                if (map.ContainsKey((startIdx, remain))) {
                    return map[(startIdx, remain)];
                }
                int res = int.MaxValue;
                int possible = 0;

                if (startIdx == cost.Count) {
                    int i = 0;
                    while (remain > 0) {
                        if ((remain & 1) > 0) {
                            int min = cost[0][i];
                            for (int j = 0; j < cost.Count; j++) {
                                min = Math.Min(min, cost[j][i]);
                            }
                            possible += min;
                        }
                        i++;
                        remain >>= 1;
                    }
                    return possible;
                }
                // 枚举idx连哪个边
                for (int i = 0; i < cost[0].Count; i++) {
                    // 连了第i号
                    possible += cost[startIdx][i];
                    possible += f(startIdx + 1, remain & (initialState ^ (1 << i)));
                    res = Math.Min(res, possible);
                    possible = 0;
                }
                map[(startIdx, remain)] = res;
                return res;
            }
        }

        public static int[] PondSizes(int[][] land) {
            int[,] dire = new int[8, 2] { { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, 1 }, { 0, -1 }, { -1, -1 }, { -1, 0 }, { -1, 1 } };
            List<int> res = new();
            for (int i = 0; i < land.Length; i++) {
                for (int j = 0; j < land[0].Length; j++) {
                    if (land[i][j] == 0) {
                        res.Add(dfs(i, j));
                    }
                }
            }
            res.Sort();
            return res.ToArray();

            int dfs(int x, int y) {
                int res = 0;
                if (x >= land.Length || x < 0 || y >= land[0].Length || y < 0) {
                    return 0;
                }
                if (land[x][y] != 0) {
                    return 0;
                }
                res++;
                land[x][y] = -1;
                for (int i = 0; i < dire.GetLength(0); i++) {
                    res += dfs(x + dire[i, 0], y + dire[i, 1]);
                }
                return res;
            }
        }

        public static bool ValidateStackSequences2(int[] pushed, int[] popped) {
            //Stack<int> st = new();
            // 在pushed上原地操作
            int stackIdx = -1;
            int cur2 = 0;
            for (int i = 0; i < pushed.Length; i++) {
                if (stackIdx + 1 > 0 && pushed[stackIdx] == popped[cur2]) { // 能pop出来这个元素就pop
                    stackIdx--;
                    cur2++;
                    i--; // 不push，所以i也不变
                } else { // 不然就不能pop，只能push
                    pushed[++stackIdx] = pushed[i];
                    //st.Push(pushed[i]);
                }
            }
            while (stackIdx + 1 > 0 && cur2 < popped.Length) {
                if (pushed[stackIdx--] != popped[cur2++]) {
                    return false;
                }
            }
            return true;
        }

        public static IList<IList<int>> ReconstructMatrix(int upper, int lower, int[] colsum) {
            if (upper + lower != colsum.Sum()) {
                return Array.Empty<IList<int>>();
            }
            List<IList<int>> res = new();

            res.Add(new int[colsum.Length].ToList());
            res.Add(new int[colsum.Length].ToList());
            int row1 = 0, row2 = 0;
            for (int i = 0; i < colsum.Length; i++) {
                if (colsum[i] == 2) {
                    res[0][i] = res[1][i] = 1;
                    row1 = ++row2;
                }
            }
            upper -= row1;
            lower -= row2;
            if (upper < 0 || lower < 0) {
                return Array.Empty<IList<int>>();
            }
            for (int i = 0; i < colsum.Length; i++) {
                if (colsum[i] == 1) {
                    if (upper > 0) {
                        res[0][i] = 1;
                        upper--;
                    }
                }
            }
            for (int i = colsum.Length - 1; i >= 0; i--) {
                if (colsum[i] == 1) {
                    if (lower > 0) {
                        res[1][i] = 1;
                        lower--;
                    }
                }
            }

            return res;
        }

        public static int LongestPalindrome(string word1, string word2) {
            string s = string.Concat(word1, word2);
            int m = word1.Length;
            int n = word2.Length;
            // 求最长回文序列
            int[,] dp = new int[m + n, m + n];
            for (int i = 0; i < s.Length; i++) {
                for (int j = i; j < s.Length; j++) {
                    dp[i, j] = 1;
                }
            }
            for (int i = 1; i < s.Length; i++) {
                for (int j = 0; j + i < s.Length; j++) {
                    if (s[j] == s[j + i]) {
                        if (i == 1) {
                            dp[j, j + i] = 2;
                        } else {
                            dp[j, j + i] = dp[j + 1, j + i - 1] + 2;
                        }
                    } else {
                        dp[j, j + i] = Math.Max(dp[j, j + i - 1], dp[j + 1, j + i]);
                    }
                }
            }
            int res = 0;
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    if (s[i] == s[m + j]) {
                        res = Math.Max(res, dp[i + 1, m + j - 1] + 2);
                    }
                }
            }
            return res;
        }

        public static bool IsCircularSentence(string sentence) {
            bool condition = sentence[0] == sentence[^1];
            if (!condition) {
                return false;
            }
            for (int i = 1; i < sentence.Length - 1; i++) {
                if (sentence[i] == ' ') {
                    if (sentence[i - 1] != sentence[i + 1]) {
                        return false;
                    }
                }
            }
            return true;
        }

        public static TreeNode? ConstructFromPrePost(int[] preorder, int[] postorder) {
            return f(0, preorder.Length - 1, 0, preorder.Length - 1);

            TreeNode? f(int l1, int r1, int l2, int r2) {
                int n = r1 - l1 + 1;
                if (n <= 0 || l1 >= preorder.Length || l2 >= postorder.Length) {
                    return null;
                }
                TreeNode root = new(preorder[l1]);
                HashSet<int> seen1 = new();
                HashSet<int> seen2 = new();
                for (int i = 0; i < n - 1; i++) {
                    seen1.Add(preorder[l1 + i + 1]);
                    seen2.Add(postorder[l2 + i]);
                    if (seen1.SetEquals(seen2)) {
                        // 是一个有效的左树
                        root.left = f(l1 + 1, l1 + i + 1, l2, l2 + i);
                        root.right = f(l1 + i + 2, r1, l2 + i + 1, r2 - 1);
                        break;
                    }
                }
                return root;
            }
        }

        public static ListNode? AddTwoNumbers(ListNode l1, ListNode l2) {
            return f(l1, l2, 0);

            ListNode? f(ListNode? cur1, ListNode? cur2, int carry) {
                if (cur1 is null && cur2 is null) {
                    if (carry == 0) {
                        return null;
                    } else {
                        return new(carry);
                    }
                }
                int a = cur1 is null ? 0 : cur1.val;
                int b = cur2 is null ? 0 : cur2.val;
                ListNode cur = new((a + b + carry) % 10);
                cur.next = f(cur1?.next, cur2?.next, (a + b + carry) / 10);
                return cur;
            }
        }

        public static ListNode AddTwoNumbers2(ListNode l1, ListNode l2) {
            Stack<int> st1 = new();
            Stack<int> st2 = new();
            bool carry = false;
            while (l1 is not null) {
                st1.Push(l1.val);
                l1 = l1.next!;
            }
            while (l2 is not null) {
                st2.Push(l2.val);
                l2 = l2.next!;
            }
            ListNode? pre = null;
            ListNode? cur = null;
            while (st1.Count > 0 || st2.Count > 0 || carry) {
                int a = st1.Count > 0 ? st1.Pop() : 0;
                int b = st2.Count > 0 ? st2.Pop() : 0;
                cur = new((a + b + (carry ? 1 : 0)) % 10);
                carry = a + b + (carry ? 1 : 0) >= 10;
                cur.next = pre;
                pre = cur;
            }
            return cur!;
        }

        public static int MinSteps(int n) {
            int factor = 2;
            int res = 0;

            while (n > 1) {
                if (n % factor == 0) {
                    res += factor;
                    n /= factor;
                } else {
                    factor++;
                }
            }
            return res;
        }

        public static int MatrixSum(int[][] nums) {
            for (int i = 0; i < nums.Length; i++) {
                Array.Sort(nums[i]);
            }
            int res = 0;
            for (int i = 0; i < nums[0].Length; i++) {
                int max = int.MinValue;
                for (int j = nums.Length - 1; j >= 0; j--) {
                    max = Math.Max(max, nums[j][i]);
                }
                res += max;
            }
            return res;
        }

        public static long ContinuousSubarrays(int[] nums) {
            int l = 0;
            int max = nums[l];
            int min = max;
            int r = -1; // [l..r]
            long res = 0;
            Deque<int> queMax = new();
            Deque<int> queMin = new();
            while (r + 1 <= nums.Length) { // 下一个位置能不能被包括，形成一个更大的不间断子数组
                if (r + 1 < nums.Length && check(nums[r + 1])) {
                    r++;
                    PushMax(r, queMax);
                    PushMin(r, queMin);
                    min = nums[queMin.PeekFront()];
                    max = nums[queMax.PeekFront()];
                } else {
                    res += r - l + 1;
                    if (l + 1 >= nums.Length) {
                        break;
                    }
                    Pop(l, queMin);
                    Pop(l, queMax);
                    l++;
                    if (r < l) { // 窗口没了
                        r++;
                        PushMax(r, queMax);
                        PushMin(r, queMin);
                    }
                    min = nums[queMin.PeekFront()];
                    max = nums[queMax.PeekFront()];
                    // 更新max和min
                }
            }
            return res;

            bool check(int val) {
                return max - val <= 2 && val - min <= 2;
            }

            void PushMax(int idx, Deque<int> que) { // 存放索引
                while (que.Count > 0 && nums[que.PeekBack()] < nums[idx]) {
                    que.PopBack();
                }
                que.PushBack(idx);
            }

            void PushMin(int idx, Deque<int> que) { // 存放索引
                while (que.Count > 0 && nums[que.PeekBack()] > nums[idx]) {
                    que.PopBack();
                }
                que.PushBack(idx);
            }

            int Pop(int val, Deque<int> que) {
                if (que.PeekFront() == val) {
                    return que.PopFront();
                }
                return -1;
            }
        }

        public static int KItemsWithMaximumSum(int numOnes, int numZeros, int numNegOnes, int k) {
            int res = 0;
            res += Math.Min(k, numOnes);
            k -= res;
            if (k == 0) {
                return res;
            }
            k -= Math.Min(k, numZeros);
            if (k == 0) {
                return res;
            }
            res -= Math.Min(k, numNegOnes);
            return res;
        }

        public static IList<long> MaximumEvenSplit(long finalSum) {
            List<long> res = new();
            if (finalSum % 2 == 1) {
                return res;
            }
            int cur = 2;
            while (cur <= finalSum) {
                res.Add(cur);
                finalSum -= cur;
                cur += 2;
            }
            if (finalSum > 0) {
                res[^1] += finalSum;
            }
            return res;
        }

        public static IList<IList<int>> FindPrimePairs(int n) {
            if (n == 1) {
                return new List<IList<int>>();
            }
            bool[] primes = new bool[n + 1];
            Array.Fill(primes, true);
            primes[0] = primes[1] = false;
            primes[2] = true;
            for (int i = 2; i <= Math.Sqrt(primes.Length); i++) {
                if (primes[i]) {
                    for (int j = i * 2; j < primes.Length; j += i) {
                        primes[j] = false;
                    }
                }
            }
            if (n % 2 == 1) {
                if (primes[n - 2]) {
                    return new List<IList<int>>() { new List<int> { 2, n - 2 } };
                } else {
                    return new List<IList<int>>();
                }
            }


            List<int> p = new();
            for (int i = 0; i < primes.Length; i++) {
                if (primes[i]) {
                    p.Add(i);
                }
            }
            int cur1 = 0, cur2 = p.Count - 1;
            List<IList<int>> res = new();
            while (cur1 <= cur2) {
                int comp = p[cur1] + p[cur2];
                if (comp == n) {
                    res.Add(new List<int> { p[cur1], p[cur2] });
                    cur1++;
                } else if (comp < n) {
                    cur1++;
                } else {
                    cur2--;
                }
            }
            return res;
        }

        public static int MinAddToMakeValid4(string s) {
            int count = 0;
            int res = 0;
            for (int i = 0; i < s.Length; i++) {
                if (s[i] == '(') {
                    count++;
                } else {
                    count--;
                    if (count < 0) {
                        res++;
                        count = 0;
                    }
                }
            }
            res += count;
            return res;
        }

        public static int MaximumGain(string s, int x, int y) {
            Stack<char> st = new();
            bool great = x > y; // true -> a
            int res = 0;
            for (int count = 0; count < 2; count++) {
                for (int i = 0; i < s.Length; i++) {
                    if (check(i)) {
                        st.Pop();
                        res += great ? x : y;
                    } else {
                        st.Push(s[i]);
                    }
                }
                s = new(string.Concat(st.ToArray().Reverse()));
                st.Clear();
                great = !great;
            }
            return res;

            bool check(int i) {
                return st.Count > 0 && st.Peek() == (great ? 'a' : 'b') && s[i] == (great ? 'b' : 'a');
            }
        }

        public static int TwoCitySchedCost(int[][] costs) {
            int res = 0;
            for (int i = 0; i < costs.Length; i++) {
                res += costs[i][1];
            } // 全部去B市
            int[] minus = new int[costs.Length];
            for (int i = 0; i < minus.Length; i++) {
                minus[i] = costs[i][1] - costs[i][0];
            }
            Array.Sort(minus, (a, b) => b.CompareTo(a));
            for (int i = 0; i < minus.Length >> 1; i++) {
                res -= minus[i];
            }
            return res;
        }

        public static string MaximumBinaryString(string binary) {
            if (binary.Length == 1) {
                return binary;
            }
            int ones = binary.Count((a) => a == '1');
            int j = 0;
            while (j < binary.Length && binary[j] == '1') {
                ones--;
                j++;
            }
            StringBuilder sb = new(new string('0', binary.Length));
            for (int i = 0; i < j; i++) {
                sb[i] = '1';
            }
            for (int i = sb.Length - 1; i >= sb.Length - ones; i--) {
                sb[i] = '1';
            }
            for (int i = j; i < sb.Length - ones - 1; i++) {
                sb[i] = '1';
            }
            return sb.ToString();
        }

        public static int FindDelayedArrivalTime(int arrivalTime, int delayedTime) {
            return (arrivalTime + delayedTime) % 24;
        }

        public static int SumOfMultiples(int n) {
            int sum = 0;
            for (int i = 0; i < n; i++) {
                if ((i + 1) % 3 == 0 || (i + 1) % 5 == 0 || (i + 1) % 7 == 0) {
                    sum += i + 1;
                }
            }
            return sum;
        }

        public static int[] GetSubarrayBeauty(int[] nums, int k, int x) {
            int[] hash = new int[101];
            int[] res = new int[nums.Length - k + 1];
            for (int i = 0; i < nums.Length - k + 1; i++) {
                if (i == 0) {
                    for (int j = 0; j < k; j++) {
                        hash[nums[i + j] + 50]++;
                    }
                } else {
                    hash[nums[i - 1] + 50]--;
                    hash[nums[i + k - 1] + 50]++;
                }
                res[i] = get();
            }
            return res;

            int get() {
                int count = 0;
                for (int i = 0; i < hash.Length; i++) {
                    if (hash[i] > 0) {
                        count += hash[i];
                        if (count >= x) {
                            return i - 50 < 0 ? i - 50 : 0;
                        }
                    }
                }
                return 0;
            }
        }

        public static int MinOperations(int[] nums) {
            bool flag = false;
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] != 1) {
                    flag = true;
                }
            }
            if (!flag) {
                return 0;
            }
            int n = nums.Length;
            int[,] array = new int[n, n];
            int firstOne = -1;
            int oneCount = 0;
            for (int i = 0; i < nums.Length; i++) {
                array[0, i] = nums[i];
                if (nums[i] == 1) {
                    oneCount++;
                }
            }
            if (oneCount > 0) {
                return nums.Length - oneCount;
            }

            for (int i = 1; i < array.GetLength(1) && firstOne == -1; i++) {
                for (int j = 0; j < n - i; j++) {
                    array[i, j] = gcd(array[i - 1, j], array[i - 1, j + 1]);
                    if (array[i, j] == 1) {
                        firstOne = i;
                        break;
                    }
                }
            }

            if (firstOne == -1) {
                return -1;
            } else {
                return firstOne + nums.Length - 1;
            }


            int gcd(int a, int b) {
                if (b == 0)
                    return a;
                return gcd(b, a % b);
            }
        }

        public static int[] TwoSum(int[] numbers, int target) {
            int cur1 = 0, cur2 = numbers.Length - 1;
            while (cur1 < cur2) {
                int comp = numbers[cur1] + numbers[cur2];
                if (comp < target) {
                    cur1++;
                } else if (comp > target) {
                    cur2--;
                } else {
                    break;
                }
            }
            return new int[] { cur1 + 1, cur2 + 1 };
        }

        public static int MaximumJumps(int[] nums, int target) {
            // target is abs
            int res = dfs(0, new());
            return res < 0 ? -1 : res;


            int dfs(int startIdx, Dictionary<int, int> map) {
                // return steps
                if (startIdx >= nums.Length - 1) {
                    return 0;
                }
                if (map.ContainsKey(startIdx)) {
                    return map[startIdx];
                }
                int res = int.MinValue;
                for (int i = startIdx + 1; i < nums.Length; i++) {
                    if (Math.Abs(nums[i] - nums[startIdx]) <= target) {
                        res = Math.Max(res, 1 + dfs(i, map));
                    }
                }
                map[startIdx] = res;
                return res;
            }
        }

        public static int MaxNonDecreasingLength(int[] nums1, int[] nums2) {
            int n = nums1.Length;
            int[,] dp = new int[n, 2];
            dp[0, 1] = dp[0, 0] = 1;
            for (int i = 0; i < n; i++) {
                dp[i, 0] = dp[i, 1] = 1;
            }
            int res = 1;
            for (int i = 1; i < n; i++) {
                // 填充dp[i, 0] dp[i, 1]
                if (nums1[i] >= nums1[i - 1]) {
                    dp[i, 0] = Math.Max(dp[i, 0], 1 + dp[i - 1, 0]);
                }
                if (nums2[i] >= nums1[i - 1]) {
                    dp[i, 1] = Math.Max(dp[i, 1], 1 + dp[i - 1, 0]);
                }
                if (nums1[i] >= nums2[i - 1]) {
                    dp[i, 0] = Math.Max(dp[i, 0], 1 + dp[i - 1, 1]);
                }
                if (nums2[i] >= nums2[i - 1]) {
                    dp[i, 1] = Math.Max(dp[i, 1], 1 + dp[i - 1, 1]);
                }
                res = Math.Max(res, dp[i, 0]);
                res = Math.Max(res, dp[i, 1]);

            }
            return res;
        }

        public static bool CheckArray(int[] nums, int k) {
            int n = nums.Length;
            int[] diff = new int[n + 1];
            int d = 0;
            for (int i = 0; i < n; i++) {
                d += diff[i]; // d是经过每个i后的差分数组的累加值
                int modify = 0;
                if (nums[i] + d < 0) {
                    return false;
                }
                if (nums[i] + d > 0) {
                    if (i + k > n) {
                        return false;
                    }
                    diff[i] -= nums[i] + d;
                    diff[i + k] += nums[i] + d;
                    modify -= nums[i] + d;
                }
                d += modify; // 当前diff的值发生了改变，需要更新一下d
            }

            return true;
        }

        public static int LengthOfLIS3(int[] nums) {
            // 最长递增子序列，贪心+二分，非动态规划
            int n = nums.Length;
            List<int> g = new();
            // g[i]: 长度是i + 1的递增序列的末尾元素的最小值
            // g.Add(nums[0]);

            for (int i = 0; i < n; i++) {
                // 当前的值就是nums[i]
                int l = 0, r = g.Count;
                while (l < r) {
                    int m = (l + r) >> 1;
                    if (g[m] < nums[i]) {
                        l = m + 1;
                    } else {
                        r = m;
                    }
                }
                if (l == g.Count) {
                    g.Add(nums[i]); // g函数的所有元素都不应该被修改，因为不是最小值。并且可以得到存在更长的子序列的末尾的值
                    r++;
                } else {
                    g[l] = nums[i];
                }
            }
            return g.Count;
        }

        public static int MinimizeArrayValue(int[] nums) {
            // 在 f f f t t t t t t中找第一个t
            int l = nums[0];
            int r = nums.Max() + 1;
            int[] helper = new int[nums.Length];
            nums.CopyTo(helper, 0);
            while (l < r) {
                int m = (l + r) >> 1;
                if (check(m)) {
                    r = m;
                } else {
                    l = m + 1;
                }
            }
            return l;

            bool check(int val) {
                nums.CopyTo(helper, 0);
                int l = 0; // 代表第一个小于val的位置
                if (val < nums[0]) {
                    return false;
                }
                for (int i = 1; i < helper.Length; i++) { // i之前的元素都应该<=val
                    int minus = Math.Min(helper[i], val - helper[l]);
                    while (helper[i] > 0 && l < i) {
                        helper[l] += minus;
                        helper[i] -= minus;
                        if (helper[l] == val) {
                            l++;
                        }
                        minus = Math.Min(helper[i], val - helper[l]);
                    }
                    if (helper[i] > val) {
                        return false;
                    }
                }
                return true;
            }
        }

        public static int MinimizeArrayValue2(int[] nums) {
            int start = nums[0];
            int n = nums.Length;
            long sum = 0;
            long curSum = 0;
            int res = 0;
            for (int i = 0; i < n; i++) {
                curSum += nums[i];
                if (nums[i] > get(i)) {
                    sum = curSum;
                    res = Math.Max(res, get(i + 1));
                }
            }
            return res;

            int get(int n) {
                return n == 0 ? 0 : (int)Math.Ceiling(sum * 1.0 / n);
            }
        }

        public static long RepairCars(int[] ranks, int cars) {
            long r;
            int max = ranks.Max();
            long l = 0;
            r = (long)max * cars * cars + 1;
            while (l < r) {
                long m = ((r - l) >> 1) + l;
                if (check(m)) {
                    r = m;
                } else {
                    l = m + 1;
                }
            }
            return l;

            bool check(long time) {
                long sum = 0;
                for (int i = 0; i < ranks.Length; i++) {
                    sum += (int)Math.Sqrt(time * 1.0 / ranks[i]);
                }
                return sum >= cars;
            }
        }

        public static long MaxAlternatingSum(int[] nums) {
            return dfs(0, 0);

            int dfs(int idx, int odd) {
                // idx 是第 odd 个
                if (idx == nums.Length) {
                    return 0;
                }
                int res = 0;
                if (odd == 0) {
                    res = Math.Max(res, nums[idx] + dfs(idx + 1, 1));
                    res = Math.Max(res, dfs(idx + 1, 0));
                } else {
                    res = Math.Max(res, -nums[idx] + dfs(idx + 1, 0));
                    res = Math.Max(res, dfs(idx + 1, 1));
                }
                return res;
            }
        }

        public static long MaxAlternatingSum2(int[] nums) {
            int n = nums.Length;
            long dp1 = 0;
            long dp0 = 0;
            for (int i = n - 1; i >= 0; i--) {
                // dp[i, 0] dp[i, 1]
                dp0 = Math.Max(nums[i] + dp1, dp0);
                dp1 = Math.Max(-nums[i] + dp0, dp1);
            }
            return dp0;
        }

        public static bool CarPooling(int[][] trips, int capacity) {
            int[] diff = new int[1002];
            for (int i = 0; i < trips.Length; i++) {
                diff[trips[i][1]] += trips[i][0];
                diff[trips[i][2] + 1] -= trips[i][0];
            }
            int d = 0;
            for (int i = 0; i < diff.Length; i++) {
                d += diff[i];
                if (d > capacity) {
                    return false;
                }
            }
            return true;
        }

        public static int NumberOfPairs(int[] nums) {
            if (nums.Length == 0) {
                return 0;
            }
            const int MOD = 1000000007;
            Dictionary<int, int> dict = new() {
                { gen(nums[0]), 1 }
            };
            long res = 0;
            for (int i = 1; i < nums.Length; i++) {
                int cur = gen(nums[i]);
                if (dict.ContainsKey(cur)) {
                    res = res % MOD + dict[cur] % MOD;
                }
                if (!dict.ContainsKey(cur)) {
                    dict[cur] = 0;
                }
                dict[cur]++;
            }
            return (int)res;

            int gen(int num) {
                string s = num.ToString();
                s = string.Concat(s.Reverse());
                return int.Parse(s) - num;
            }
        }

        public static int LakeCount(string[] field) {
            int m = field.Length;
            int n = field[0].Length;
            int[,] mat = new int[m, n];
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    mat[i, j] = field[i][j] == '.' ? 0 : 1;
                }
            }
            int count = 0;
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    if (infect(i, j)) {
                        count++;
                    }
                }
            }
            return count;

            bool infect(int i, int j) {
                if (i < 0 || i >= m || j < 0 || j >= n || mat[i, j] != 1) {
                    return false;
                }
                int[][] dire = new int[][] { new int[] { 1, 0 }, new int[] { 1, 1 }, new int[] { 1, -1 }, new int[] { -1, 0 },
                    new int[] { -1, 1 }, new int[] { -1, -1 }, new int[] { 0, 1 }, new int[] { 0, -1 } };
                mat[i, j] = 2;
                for (int m = 0; m < dire.Length; m++) {
                    infect(i + dire[m][0], j + dire[m][1]);
                }
                return true;
            }
        }

        public static int MinOperations2(int[] numbers) {
            int n = numbers.Length;
            if (n == 1) {
                return 0;
            }
            int allGCD = gcd(numbers[0], numbers[1]);
            for (int i = 2; i < n; i++) {
                allGCD = gcd(allGCD, numbers[i]);
            }
            int[,] mat = new int[n, 2];
            int[] max = new int[2];
            for (int i = 0; i < n; i++) {
                numbers[i] /= allGCD;

                while (numbers[i] % 3 == 0) {
                    numbers[i] /= 3;
                    mat[i, 1]++;
                }
                while ((numbers[i] & 1) == 0) {
                    numbers[i] >>= 1;
                    mat[i, 0]++;
                }
                if (numbers[i] != 1) {
                    return -1;
                }
                max[0] = Math.Max(max[0], mat[i, 0]);
                max[1] = Math.Max(max[1], mat[i, 1]);
            }
            int res = 0;
            for (int i = 0; i < n; i++) {
                res += max[0] - mat[i, 0];
                res += max[1] - mat[i, 1];
            }
            return res;

            int gcd(int a, int b) {
                if (a < b) {
                    return gcd(b, a);
                }
                if (a % b == 0) {
                    return b;
                }
                return gcd(b, a % b);
            }
        }

        public static int LeftRightDiff(int[] nums) {
            int max = nums[1];
            for (int i = 2; i < nums.Length; i++) {
                max = Math.Max(max, nums[i]);
            }
            int l = Math.Max(nums[0], max);
            int r = Math.Max(nums[^1], max);
            return Math.Max(Math.Abs(l - nums[^1]), Math.Abs(r - nums[0]));
        }

        public static int LeftRightDiff2(int[] nums) {
            int n = nums.Length;
            int[] left = new int[n];
            left[0] = nums[0];
            for (int i = 1; i < n; i++) {
                left[i] = Math.Max(left[i - 1], nums[i]);
            }
            int right = nums[^1];
            int res = 0;
            for (int i = n - 1; i >= 1; i--) {
                right = Math.Max(right, nums[i]);
                res = Math.Max(res, Math.Abs(right - left[i - 1]));
            }
            return res;
        }

        public static int AlternateDigitSum(int n) {
            int count = 0;
            int res = 0;
            while (n > 0) {
                res += (count % 2 == 0 ? n : -n) % 10;
                n /= 10;
                count++;
            }
            if (count % 2 == 0) {
                res = -res;
            }
            return res;
        }

        public static bool CanBeValid(string s, string locked) {
            int n = s.Length;
            return f(s, '(', locked) && f(string.Concat(s.Reverse()), ')', string.Concat(locked.Reverse()));

            bool f(string s, char add, string locked) {
                int max = 0;
                for (int i = 0; i < n; i++) {
                    if (locked[i] == '0') {
                        max++;
                    } else {
                        if (s[i] == add) {
                            max++;
                        } else {
                            max--;
                        }
                        if (max < 0) {
                            return false;
                        }
                    }
                }
                return max >= 0 && max % 2 == 0;
            }
        } // 贪心算法不需要证明，大概

        public static int MinFallingPathSum(int[][] matrix) {
            int n = matrix.Length;
            int[,] dp = new int[n, n];
            int min = int.MaxValue;
            for (int i = 0; i < n; i++) {
                dp[n - 1, i] = matrix[^1][i];
            }
            for (int i = n - 2; i >= 0; i--) {
                for (int j = 0; j < n; j++) {
                    if (j == 0) {
                        dp[i, j] = matrix[i][j] + dp[i + 1, j];
                        if (n > 1) {
                            dp[i, j] = Math.Min(dp[i, j], matrix[i][j] + dp[i + 1, j + 1]);
                        }
                    } else if (j == n - 1) {
                        dp[i, j] = matrix[i][j] + dp[i + 1, j];
                        if (n > 1) {
                            dp[i, j] = Math.Min(dp[i, j], matrix[i][j] + dp[i + 1, j - 1]);
                        }
                    } else {
                        dp[i, j] = matrix[i][j] + dp[i + 1, j];
                        dp[i, j] = Math.Min(dp[i, j], matrix[i][j] + dp[i + 1, j + 1]);
                        dp[i, j] = Math.Min(dp[i, j], matrix[i][j] + dp[i + 1, j - 1]);
                    }
                }
            }
            for (int i = 0; i < n; i++) {
                min = Math.Min(min, dp[0, i]);
            }
            return min;
        }

        public static int DistributeCoins(TreeNode root) {
            int res = 0;
            dfs(root);
            return res;

            (int, int) dfs(TreeNode? node) { // size, coin
                if (node is null) {
                    return (0, 0);
                }
                var l = dfs(node.left);
                var r = dfs(node.right);

                int size = l.Item1 + r.Item1 + 1;
                int coin = l.Item2 + r.Item2 + node.val;
                res += Math.Abs(size - coin); // 如果多了必须从这棵树的根部走出去，少了必须走进来，必须经过这个根部
                return (size, coin);
            }
        }

        public static int SumSubseqWidths(int[] nums) {
            checked {
                int n = nums.Length;
                const int MOD = 1000000007;
                Array.Sort(nums);
                long max = 0;
                long min = 0;

                for (int i = 0; i < n; i++) {
                    int great = n - i - 1;
                    min += pow(great) * nums[i] % MOD;
                    min %= MOD;
                }
                for (int i = n - 1; i >= 0; i--) {
                    int less = i;
                    max += pow(less) * nums[i] % MOD;
                    max %= MOD;
                }
                return (int)(max - min + MOD) % MOD;

                long pow(int n) {
                    if (n < 20) {
                        return 1 << n;
                    }
                    if ((n & 1) == 1) {
                        return (pow(n - 1) % MOD * 2) % MOD;
                    } else {
                        long tmp = pow(n >> 1);
                        return (tmp % MOD) * (tmp % MOD) % MOD;
                    }
                }

            }
        }

        public static bool WinnerOfGame(string colors) {
            PriorityQueue<int, int> a = new(new MaxHeapComparer<int>());
            PriorityQueue<int, int> b = new(new MaxHeapComparer<int>());
            bool isA = colors[0] == 'A';
            Dictionary<int, int> dict = new();
            for (int i = 0; i < colors.Length;) {
                int cnt = 1;
                while (i + 1 < colors.Length && colors[i + 1] == colors[i]) {
                    i++;
                    cnt++;
                }
                if (isA) {
                    a.Enqueue(i, cnt);
                } else {
                    b.Enqueue(i, cnt);
                }
                dict[i] = cnt;
                i++;
                isA = !isA;
            }
            isA = true;
            while (true) {
                if (isA) {
                    if (a.Count > 0 && dict[a.Peek()] >= 3) {
                        var tmp = a.Dequeue();
                        dict[tmp]--;
                        a.Enqueue(tmp, dict[tmp]);
                    } else {
                        return false;
                    }
                } else {
                    if (b.Count > 0 && dict[b.Peek()] >= 3) {
                        var tmp = b.Dequeue();
                        dict[tmp]--;
                        b.Enqueue(tmp, dict[tmp]);
                    } else {
                        return true;
                    }
                }
                isA = !isA;
            }
        }

        public static int MaxJump(int[] stones) {
            int l = 1;
            int r = stones[^1] - stones[0];
            while (l < r) {
                int m = (l + r) >> 1;
                if (check(m)) {
                    r = m;
                } else {
                    l = m + 1;
                }
            }
            return l;

            bool check(int max) {
                int last = 0; // last
                int cur = 0;
                HashSet<int> visited = new();
                while (cur < stones.Length) {
                    if (stones[cur] - stones[last] > max) {
                        if (last == cur - 1) {
                            return false;
                        }
                        last = cur - 1;
                        visited.Add(last);
                    } else {
                        cur++;
                    }
                }
                // 正着跑完了
                for (int i = 0; i < stones.Length;) {
                    cur = i + 1;
                    while (cur < stones.Length && visited.Contains(cur)) {
                        cur++;
                    }
                    if (cur >= stones.Length) {
                        break;
                    }
                    if (stones[cur] - stones[i] > max) {
                        return false;
                    }
                    i = cur;
                }
                return true;
            }
        }

        public static int SumOfSquares(int[] nums) {
            int res = 0;
            for (int i = 1; i <= nums.Length; i++) {
                if (nums.Length % i == 0) {
                    res += nums[i - 1] * nums[i - 1];
                }
            }
            return res;
        }

        public static int MaximumBeauty(int[] nums, int k) {
            int[] diff = new int[200003];
            int max = 0;
            for (int i = 0; i < nums.Length; i++) {
                diff[Math.Max(0, nums[i] - k)]++;
                diff[Math.Min(nums[i] + k + 1, diff.Length - 1)]--;
            }
            int d = 0;
            for (int i = 0; i < diff.Length; i++) {
                d += diff[i];
                max = Math.Max(max, d);
            }
            return max;
        }

        public static int MinimumIndex(IList<int> nums) {
            Dictionary<int, int> dict = new();
            for (int i = 0; i < nums.Count; i++) {
                if (!dict.ContainsKey(nums[i])) {
                    dict[nums[i]] = 0;
                }
                dict[nums[i]]++;
            }
            int max = 0;  // freq
            int val = -1; // value
            foreach (var item in dict) {
                if (item.Value > max) {
                    val = item.Key;
                    max = item.Value;
                }
            }
            if (max * 2 <= nums.Count) {
                return -1;
            }
            dict.Clear();
            int idx = 0;
            while (idx < nums.Count && nums[idx] != val) {
                idx++;
            }
            int left = 0;
            for (int i = idx; i < nums.Count; i++) {
                if (nums[i] == val) {
                    left++;
                }
                if (left * 2 > (i + 1) && (max - left) * 2 > (nums.Count - i - 1)) {
                    return i;
                }
            }
            return -1;
        }

        public static int LongestValidSubstring(string word, IList<string> forbidden) {
            int res = 0;
            int j = 0;
            const int LEN = 10;
            HashSet<string> set = new(forbidden);
            for (int i = 0; i < word.Length;) {
                while (j < word.Length && check(Math.Max(i, j + 1 - LEN), j)) {
                    j++;
                }
                res = Math.Max(res, j - i);
                i++;
                if (i > j) {
                    j = i;
                }
            }
            return res;

            bool check(int minLeft, int right) {
                for (int i = minLeft; i <= right; i++) {
                    if (set.Contains(word[i..(right + 1)])) {
                        return false;
                    }
                }
                return true;
            }
        }

        public static int[] SumOfDistancesInTree(int n, int[][] edges) {
            if (n == 1) {
                return new int[] { 0 };
            }
            if (n == 2) {
                return new int[] { 1, 1 };
            }
            List<int>[] graph = new List<int>[n];
            int[] res = new int[n];
            // 建图
            for (int i = 0; i < edges.Length; i++) {
                if (graph[edges[i][0]] is null) {
                    graph[edges[i][0]] = new();
                }
                graph[edges[i][0]].Add(edges[i][1]);
                if (graph[edges[i][1]] is null) {
                    graph[edges[i][1]] = new();
                }
                graph[edges[i][1]].Add(edges[i][0]);
            }
            int head = 0;
            while (graph[head].Count <= 1) {
                head++;
            }
            int[] size = new int[n]; // 包含该根节点的子树的节点数
            dfs(head, new bool[n]);
            // 填充好了size数组
            // 为head bfs填充答案数组
            // ans
            int ans = 0;
            Queue<(int, int)> que = new();
            bool[] visited = new bool[n];
            que.Enqueue((head, 0));
            while (que.Count > 0) {
                var tmp = que.Dequeue();
                ans += tmp.Item2;
                visited[tmp.Item1] = true;
                for (int i = 0; i < graph[tmp.Item1].Count; i++) {
                    if (!visited[graph[tmp.Item1][i]]) {
                        que.Enqueue((graph[tmp.Item1][i], tmp.Item2 + 1));
                    }
                }
            }
            res[head] = ans;
            // bfs求下一层的答案
            que.Enqueue((head, 0));
            while (que.Count > 0) {
                var tmp = que.Dequeue();

                for (int i = 0; i < graph[tmp.Item1].Count; i++) {
                    if (res[graph[tmp.Item1][i]] == 0) {
                        res[graph[tmp.Item1][i]] = res[tmp.Item1] + (n - 2 * size[graph[tmp.Item1][i]]);
                        que.Enqueue((graph[tmp.Item1][i], 0));
                    }
                }
            }
            return res;


            int dfs(int head, bool[] visited) {
                int sum = 1;
                visited[head] = true;
                for (int i = 0; i < graph[head].Count; i++) {
                    if (!visited[graph[head][i]]) {
                        sum += dfs(graph[head][i], visited);
                    }
                }
                visited[head] = false;
                size[head] = sum;
                return sum;
            }
        }

        public static string AddStrings(string num1, string num2) {
            num1 = string.Concat(num1.Reverse());
            num2 = string.Concat(num2.Reverse());
            StringBuilder sb = new();
            int len = Math.Max(num1.Length, num2.Length);
            int carry = 0;
            for (int i = 0; i < len; i++) {
                int a = 0, b = 0;
                if (i < num1.Length) {
                    a = num1[i] - '0';
                }
                if (i < num2.Length) {
                    b = num2[i] - '0';
                }
                sb.Append((a + b + carry) % 10);
                carry = (a + b + carry) / 10;
            }
            if (carry > 0) {
                sb.Append(carry);
            }
            return string.Concat(sb.ToString().Reverse());
        }

        public static string RemoveDuplicateLetters(string s) {
            Stack<char> st = new();
            int[] set = new int[26];
            bool[] used = new bool[26];
            for (int i = 0; i < s.Length; i++) {
                set[s[i] - 'a']++;
            }
            for (int i = 0; i < s.Length; i++) {
                set[s[i] - 'a']--;

                if (!used[s[i] - 'a']) {
                    while (st.Count > 0 && st.Peek() >= s[i] && set[st.Peek() - 'a'] > 0) {
                        used[st.Pop() - 'a'] = false;
                    }
                    st.Push(s[i]);
                    used[s[i] - 'a'] = true;
                }
            }
            return string.Concat(st.ToArray().Reverse());
        }

        public static int SmallestRepunitDivByK(int k) {
            if (k == 1) {
                return 1;
            }
            int modSum = 1;
            int term = 1;
            bool[] hash = new bool[k];
            hash[1] = true;
            // 10 % k
            // int mod = 1;
            int n = 1;
            while (modSum != 0) {
                term = (term * 10 % k) % k;
                modSum += term;
                modSum %= k;
                if (hash[modSum]) {
                    return -1;
                }
                hash[modSum] = true;

                n++;
            }
            return n;
        }

        public static int MaxSubarraySumCircular(int[] nums) {
            int n = nums.Length;
            int[] arr = new int[nums.Length << 1];
            Array.Copy(nums, 0, arr, 0, n);
            Array.Copy(nums, 0, arr, n, n);
            int[] prefix = new int[arr.Length + 1];
            for (int i = 1; i < prefix.Length; i++) {
                prefix[i] = prefix[i - 1] + arr[i - 1];
            }
            // 单调队列维护窗口
            Deque<int> deque = new();
            deque.PushBack(0); // prefix的下标
            int res = int.MinValue;
            for (int i = 1; i < prefix.Length; i++) {
                while (deque.Count > 0 && prefix[i] < prefix[deque.PeekBack()]) {
                    deque.PopBack();
                }
                deque.PushBack(i);
                while (deque.Count > 0 && i - deque.PeekFront() > n) {
                    deque.PopFront();
                }
                if (i != deque.PeekFront()) {
                    res = Math.Max(res, prefix[i] - prefix[deque.PeekFront()]);
                }
            }
            return res == int.MinValue ? nums.Max() : res;
        }

        public static int FindMaxValueOfEquation(int[][] points, int k) {
            PriorityQueue<int, int> que = new();
            int res = 0;
            que.Enqueue(0, points[0][0] - points[0][1]);
            for (int i = 1; i < points.Length; i++) {
                while (que.Count > 0 && points[i][0] - points[que.Peek()][0] > k) {
                    que.Dequeue();
                }
                if (que.Count > 0) {
                    int cur = points[i][0] + points[i][1] - (points[que.Peek()][0] - points[que.Peek()][1]);
                    res = Math.Max(res, cur);
                }
                que.Enqueue(i, points[i][0] - points[i][1]);
            }
            return res;
        }

        public static int SumSubarrayMins(int[] arr) {
            Stack<int> st = new(); // idx
            int n = arr.Length;
            const int MOD = 1000000007;
            int[] left = new int[n];
            int[] right = new int[n];
            Array.Fill(left, -1);
            Array.Fill(right, n);

            for (int i = 0; i < n; i++) {
                while (st.Count > 0 && arr[st.Peek()] > arr[i]) {
                    var a = st.Pop();
                    right[a] = i;
                }
                if (st.Count > 0) {
                    left[i] = st.Peek();
                }
                st.Push(i);
            }
            long res = 0;
            for (int i = 0; i < n; i++) {
                res = res % MOD + arr[i] * (right[i] - i) * (i - left[i]) % MOD;
            }
            return (int)res;
        }

        public static int NthMagicalNumber(int n, int a, int b) {
            checked {
                long l = Math.Max(a, b) - 1;
                const int MOD = 1000000007;
                long r = long.MaxValue;
                // < < < < >= >=
                while (l < r) {
                    long m = ((r - l) >> 1) + l;
                    long tmp = calc(m);
                    if (tmp < n) {
                        l = m + 1;
                    } else {
                        r = m;
                    }
                }
                return (int)(l % MOD);

                long calc(long m) {
                    long u = m / a;
                    long v = m / b;
                    long lcm = a * b / gcd(a, b);
                    return (u + v - m / lcm);
                }

                static long gcd(long a, long b) {
                    if (a < b) {
                        return gcd(b, a);
                    }
                    if (a % b == 0) {
                        return b;
                    }
                    return gcd(b, a % b);
                }

            }
        }

        public static int Trap2(int[] height) {
            int l = 0, r = height.Length - 1;
            int lMax = height[0];
            int rMax = height[^1];
            int res = 0;
            while (l + 1 < r) {
                if (lMax < rMax) {
                    res += Math.Max(lMax - height[l + 1], 0);
                    l++;
                    lMax = Math.Max(lMax, height[l]);
                } else {
                    res += Math.Max(rMax - height[r - 1], 0);
                    r--;
                    rMax = Math.Max(rMax, height[r]);
                }
            }
            return res;
        }

        public static int MaximumSum(int[] arr) {
            int n = arr.Length;
            if (n == 1) {
                return arr[0];
            }
            int[] dpL = new int[n];
            int[] dpR = new int[n];
            dpL[0] = arr[0];
            dpR[^1] = arr[^1];
            int res = Math.Max(dpL[0], dpR[^1]);
            for (int i = 1; i < n; i++) {
                dpL[i] = Math.Max(arr[i], arr[i] + dpL[i - 1]);
                res = Math.Max(dpL[i], res);
            }
            for (int i = n - 2; i >= 0; i--) {
                dpR[i] = Math.Max(arr[i], arr[i] + dpR[i + 1]);
                res = Math.Max(res, dpR[i]);
            }
            for (int i = 0; i < n; i++) {
                if (arr[i] < 0) {
                    res = Math.Max((i == 0 ? 0 : dpL[i - 1]) + (i + 1 == n ? 0 : dpR[i + 1]), res);
                }
            }
            return res;
        }

        public static int NumJewelsInStones(string jewels, string stones) {
            return stones.Count((c) => jewels.Contains(c));
        }

        public static int HalveArray(int[] nums) {
            PriorityQueue<int, double> que = new(new MaxHeapComparer<double>());
            int n = nums.Length;
            double[] arr = new double[n];

            long sum = 0;
            for (int i = 0; i < n; i++) {
                que.Enqueue(i, nums[i]);
                sum += nums[i];
                arr[i] = nums[i];
            }
            double curSum = sum;
            int res = 0;
            while (sum - curSum < sum / 2.0) {
                var val = que.Dequeue();
                curSum -= arr[val] / 2.0;
                arr[val] /= 2.0;
                que.Enqueue(val, arr[val]);
                res++;
            }
            return res;
        }

        public static int DeleteGreatestValue(int[][] grid) {
            for (int i = 0; i < grid.Length; i++) {
                Array.Sort(grid[i]);
            }
            int res = 0;
            for (int i = grid[0].Length - 1; i >= 0; i--) {
                int max = 0;
                for (int j = 0; j < grid.Length; j++) {
                    max = Math.Max(max, grid[j][i]);
                }
                res += max;
            }
            return res;
        }

        public static string ReorganizeString(string s) {
            int[] hash = new int[26];
            for (int i = 0; i < s.Length; i++) {
                hash[s[i] - 'a']++;
            }
            PriorityQueue<char, int> que = new(new MaxHeapComparer<int>());
            for (int i = 0; i < hash.Length; i++) {
                if (hash[i] > 0) {
                    que.Enqueue((char)('a' + i), hash[i]);
                }
            }
            StringBuilder res = new();
            while (que.Count >= 2) {
                var first = que.Dequeue();
                var second = que.Dequeue();
                res.Append(first);
                res.Append(second);
                hash[first - 'a']--;
                hash[second - 'a']--;
                if (hash[first - 'a'] > 0) {
                    que.Enqueue(first, hash[first - 'a']);
                }
                if (hash[second - 'a'] > 0) {
                    que.Enqueue(second, hash[second - 'a']);
                }
            }
            if (que.Count > 0) {
                res.Append(new string(que.Peek(), hash[que.Peek() - 'a']));
            }
            for (int i = 1; i < res.Length; i++) {
                if (res[i - 1] == res[i]) {
                    return string.Empty;
                }
            }
            return res.ToString();
        }

        public static IList<int> DistanceK(TreeNode root, TreeNode target, int k) {
            HashSet<int> path = new();
            int depth = getDepth(root, 0, new());
            List<int> res = new();
            calc(root, depth);

            return res;

            int getDepth(TreeNode node, int lv, HashSet<int> p) {
                if (node.val == target.val) {
                    path = new(p);
                    return lv;
                }
                int l = -1;
                if (node.left is not null) {
                    p.Add(node.left.val);
                    l = getDepth(node.left, lv + 1, p);
                    p.Remove(node.left.val);
                }
                if (node.right is not null) {
                    p.Add(node.right.val);
                    l = Math.Max(l, getDepth(node.right, lv + 1, p));
                    p.Remove(node.right.val);
                }
                return l;
            }

            void calc(TreeNode node, int dis) {
                if (dis == k) {
                    res.Add(node.val);
                }
                if (node.left is not null) {
                    if (path.Contains(node.left.val)) {
                        calc(node.left, dis - 1);
                    } else {
                        calc(node.left, dis + 1);
                    }
                }
                if (node.right is not null) {
                    if (path.Contains(node.right.val)) {
                        calc(node.right, dis - 1);
                    } else {
                        calc(node.right, dis + 1);
                    }
                }
            }
        }

        public static int NumFriendRequests(int[] ages) {
            Array.Sort(ages);
            double[] arr = new double[ages.Length];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = ages[i] / 2.0 + 7;
            }
            int res = 0;
            for (int i = 0; i < arr.Length; i++) {
                int l = 0;
                int r = i;
                double target = arr[i];
                while (l < r) {
                    int m = (l + r) / 2;
                    if (ages[m] <= target) {
                        l = m + 1;
                    } else {
                        r = m;
                    }
                }
                res += i - l;
            }
            for (int i = 0; i < ages.Length;) {
                int j = i;
                while (j + 1 < ages.Length && ages[j + 1] == ages[j]) {
                    j++;
                    if (ages[j] > 14) {
                        res += j - i;

                    }
                }
                i = j + 1;
            }
            return res;
        }

        public static IList<TreeNode> AllPossibleFBT(int n) {
            Dictionary<int, List<TreeNode>> map = new();
            return f(n, map);

            static List<TreeNode> f(int n, Dictionary<int, List<TreeNode>> map) {
                if (map.ContainsKey(n)) {
                    return map[n];
                }
                if (n == 1) {
                    return new() { new(0) };
                }
                if (n == 3) {
                    return new() { new(0, new(0), new(0)) };
                }
                List<TreeNode> res = new();
                for (int i = 1; i <= n - 2; i++) {
                    // i是左树size
                    // n - i - 1
                    var left = f(i, map);
                    var right = f(n - i - 1, map);
                    for (int p = 0; p < left.Count; p++) {
                        for (int q = 0; q < right.Count; q++) {
                            TreeNode cur = new(0, left[p], right[q]);
                            res.Add(cur);
                        }
                    }
                }
                map[n] = res;
                return res;
            }

        }

        public static int LongestOnes(int[] nums, int k) {
            int l = -1;
            int r = nums.Length;
            while (l < r) {
                int m = (l + r + 1) / 2;
                if (check(m)) {
                    l = m;
                } else {
                    r = m - 1;
                }
            }
            return l;

            bool check(int n) {
                int l = 0;
                int r = n - 1;
                int ones = 0;
                for (int i = 0; i < n; i++) {
                    ones += nums[i];
                }
                int diff = n - ones;
                while (r + 1 < nums.Length) {
                    ones -= nums[l];
                    l++;
                    r++;
                    ones += nums[r];
                    diff = Math.Min(diff, n - ones);
                }
                return diff <= k;
            }
        }

        public static int ConsecutiveNumbersSum(int n) {
            int res = 0;
            for (int i = 1; i <= n; i++) {
                int bas = (i + 1) * i / 2;
                if (bas <= n && (n - bas) % i == 0) {
                    res++;
                }
            }
            return res;
        }

        public static int[] LoudAndRich(int[][] richer, int[] quiet) {
            int n = quiet.Length;
            int[] res = new int[n];
            if (richer.Length == 0) {
                for (int i = 0; i < n; i++) {
                    res[i] = i;
                }
                return res;
            }

            List<int>[] graph = new List<int>[n + 1];
            for (int i = 0; i < richer.Length; i++) {
                if (graph[richer[i][1]] is null) {
                    graph[richer[i][1]] = new();
                }
                graph[richer[i][1]].Add(richer[i][0]);
            }
            int[] map = new int[n];
            Array.Fill(map, -1);
            for (int i = 0; i < n; i++) {
                res[i] = dfs(i, map);
            }
            return res;

            int dfs(int m, int[] map) {
                if (graph[m] is null) {
                    return m;
                }
                if (map[m] >= 0) {
                    return map[m];
                }
                int min = quiet[m];
                int res = m;
                for (int i = 0; i < graph[m].Count; i++) {
                    int tmp = dfs(graph[m][i], map);
                    if (quiet[tmp] < min) {
                        min = quiet[tmp];
                        res = tmp;
                    }
                }
                map[m] = res;
                return res;
            }
        }

        public static int MaxScoreSightseeingPair(int[] values) {
            int last = values[0];
            int res = 0;
            for (int i = 1; i < values.Length; i++) {
                int cur = values[i] - i;
                res = Math.Max(res, last + cur);
                last = Math.Max(last, values[i] + i);
            }
            return res;
        }

        public static int MincostTickets(int[] days, int[] costs) {
            int n = days.Length;
            int[] map = new int[n];
            Array.Fill(map, -1);
            return dfs(0);

            int dfs(int start) {
                if (start >= n) {
                    return 0;
                }
                // 从这个索引的一天开始，使用什么样的组合可以达到最优结果
                if (map[start] >= 0) {
                    return map[start];
                }
                int res = int.MaxValue;
                // 1 7 30
                int one = n, seven = n, thirty = n;
                for (int i = start; i < n; i++) {
                    int diff = days[i] - days[start];
                    if (diff >= 1) {
                        one = Math.Min(one, i);
                    }
                    if (diff >= 7) {
                        seven = Math.Min(seven, i);
                    }
                    if (diff >= 30) {
                        thirty = Math.Min(thirty, i);
                        break;
                    }
                }
                res = Math.Min(res, costs[0] + dfs(one));
                res = Math.Min(res, costs[1] + dfs(seven));
                res = Math.Min(res, costs[2] + dfs(thirty));
                return map[start] = res;
            }
        } // 0

        public static bool EscapeGhosts(int[][] ghosts, int[] target) {
            for (int i = 0; i < ghosts.Length; i++) {
                if (calc(ghosts[i]) <= calc(new int[] { 0, 0 })) {
                    return false;
                }
            }
            return true;

            int calc(int[] g) {
                return Math.Abs(target[0] - g[0]) + Math.Abs(target[1] - g[1]);
            }
        }

        public static void ReorderList(ListNode head) {
            int n = 0;
            ListNode? cur = head;
            ListNode first = head;
            ListNode? last = head;
            while (cur != null) {
                n++;
                last = cur;
                cur = cur.next;
            }
            cur = head;
            for (int i = 0; i < n / 2; i++) {
                cur = cur!.next;
            }

            ListNode? next = cur!.next;
            cur.next = null;
            while (next != null) {
                ListNode? tmp = next.next;
                next.next = cur;
                cur = next;
                next = tmp;
            }
            while (first.next != null && last != null && first != last) {
                ListNode? firstNxt = first.next;
                ListNode? lastNxt = last.next;
                first.next = last;
                if (firstNxt != last) {
                    last.next = firstNxt;
                } else {
                    break;
                }
                first = firstNxt;
                last = lastNxt;
            }
        }

        public static int SubarrayMaxSum(int[] arr) {
            int n = arr.Length;
            int min = arr[0];
            int res = int.MinValue;
            int curPrefix = arr[0];
            for (int i = 1; i < n; i++) {
                curPrefix += arr[i] == 0 ? 1 : -1;
                res = Math.Max(res, curPrefix - min);
                min = Math.Min(min, curPrefix);
            }
            return res + arr.Count((a) => a == 1);
        }

        public static int SumOfPower(int[] nums) {
            Array.Sort(nums);
            long prefixSum = 0;
            int n = nums.Length;
            long res = 0;
            const int MOD = (int)1e9 + 7;
            checked {
                for (int i = 0; i < n; i++) {
                    int mod = nums[i] % MOD;
                    long cur = (long)mod * mod % MOD * mod % MOD; // self
                    cur = (cur % MOD + prefixSum % MOD * mod % MOD * mod % MOD) % MOD;
                    res = (res + cur % MOD) % MOD;
                    prefixSum = (prefixSum % MOD + mod + prefixSum % MOD) % MOD;
                }
            }
            return (int)(res % MOD);
        }

        public static int Flipgame(int[] fronts, int[] backs) {
            HashSet<int> set = new();
            for (int i = 0; i < backs.Length; i++) {
                if (fronts[i] == backs[i]) {
                    set.Add(fronts[i]);
                }
            }
            int res = int.MaxValue;
            for (int i = 0; i < backs.Length; i++) {
                if (!set.Contains(backs[i])) {
                    res = Math.Min(res, backs[i]);
                }
                if (!set.Contains(fronts[i])) {
                    res = Math.Min(res, fronts[i]);
                }
            }
            return res == int.MaxValue ? 0 : res;
        }

        public static void SolveSudoku(char[][] board) {
            char[][] res = new char[board.Length][];

            HashSet<char>[,] boxes = new HashSet<char>[3, 3];
            HashSet<char>[] colSet = new HashSet<char>[board[0].Length];
            HashSet<char>[] rowSet = new HashSet<char>[board.Length];
            for (int i = 0; i < board.Length; i++) {
                for (int j = 0; j < board[0].Length; j++) {
                    if (boxes[i / 3, j / 3] is null) {
                        boxes[i / 3, j / 3] = new();
                    }
                    if (board[i][j] != '.') {
                        boxes[i / 3, j / 3].Add(board[i][j]);
                    }

                    if (colSet[j] is null) {
                        colSet[j] = new();
                    }
                    if (board[i][j] != '.') {
                        colSet[j].Add(board[i][j]);
                    }

                    if (rowSet[i] is null) {
                        rowSet[i] = new();
                    }
                    if (board[i][j] != '.') {
                        rowSet[i].Add(board[i][j]);
                    }
                }
            }

            bool found = false;
            OneRow(0, 0);
            for (int i = 0; i < board.Length; i++) {
                board[i] = new char[res[0].Length];
                for (int j = 0; j < res[0].Length; j++) {
                    board[i][j] = res[i][j];
                }
            }
            Display();


            void OneRow(int row, int col) {
                //Console.WriteLine(col);
                if (found) {
                    return;
                }
                if (col >= board[0].Length) {
                    OneRow(row + 1, 0);
                    return;
                }
                if (row >= board.Length) {
                    for (int i = 0; i < board.Length; i++) {
                        res[i] = new char[board[0].Length];
                        for (int j = 0; j < board[0].Length; j++) {
                            res[i][j] = board[i][j];
                        }
                    }
                    found = true;
                    return;
                }
                if (board[row][col] != '.') {
                    OneRow(row, col + 1);
                } else {
                    var box = boxes[row / 3, col / 3];
                    for (char i = '1'; i <= '9'; i++) {
                        if (!rowSet[row].Contains(i) && !box.Contains(i) && !colSet[col].Contains(i)) {
                            board[row][col] = i;

                            rowSet[row].Add(i);
                            box.Add(i);
                            colSet[col].Add(i);
                            Display();

                            OneRow(row, col + 1);

                            rowSet[row].Remove(i);
                            box.Remove(i);
                            colSet[col].Remove(i);


                            board[row][col] = '.';

                            //Thread.Sleep(1);
                            //Display();
                        }
                    }
                }
            }

            void Display() {
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < 9; i++) {
                    for (int j = 0; j < 9; j++) {
                        Console.Write(board[i][j]);
                        if ((j + 1) % 3 == 0) {
                            Console.Write(' ');
                        }
                    }
                    Console.WriteLine();
                    if ((i + 1) % 3 == 0) {
                        Console.WriteLine();
                    }
                }
            }
        }

        public static int MinSwapsCouples(int[] row) {
            int[] hash = new int[60];
            int n = row.Length;
            for (int i = 0; i < n; i++) {
                hash[row[i]] = i;
            }
            int res = 0;
            for (int i = 1; i < n; i += 2) {
                int idx = row[i] % 2 == 0 ? hash[row[i] + 1] : hash[row[i] - 1];
                //idx = hash[row[i] + 1];
                if (i - 1 != idx) {
                    (row[i - 1], row[idx]) = (row[idx], row[i - 1]);
                    hash[row[i - 1]] = i - 1;
                    hash[row[idx]] = idx;
                    res++;
                }

            }
            return res;
        }

        public static int UniquePathsIII(int[][] grid) {
            int n = grid.Length;
            int m = grid[0].Length;
            (int, int) start = (0, 0);
            (int, int) end = (0, 0);
            int total = m * n;
            int[][] direct = new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { -1, 0 } };
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    if (grid[i][j] == 1) {
                        start = (i, j);
                    }
                    if (grid[i][j] == 2) {
                        end = (i, j);
                    }
                    if (grid[i][j] == -1) {
                        total--;
                    }
                }
            }
            HashSet<(int, int)> visited = new();
            visited.Add(start);
            return f(start, visited);

            int f((int, int) start, HashSet<(int, int)> visited) {
                int res = 0;
                if (grid[start.Item1][start.Item2] == 2) {
                    return visited.Count == total ? 1 : 0;
                }
                for (int i = 0; i < direct.Length; i++) {
                    (int, int) cur = (direct[i][0] + start.Item1, direct[i][1] + start.Item2);
                    if (cur.Item1 >= 0 && cur.Item1 < grid.Length
                        && cur.Item2 >= 0 && cur.Item2 < grid[0].Length
                        && grid[cur.Item1][cur.Item2] != -1
                        && !visited.Contains(cur)) {

                        visited.Add(cur);
                        res += f(cur, visited);
                        visited.Remove(cur);
                    }
                }
                return res;
            }

        }

        public static int MaxSum(int[] nums1, int[] nums2) {
            int cur1 = 0, cur2 = 0;
            int n = nums1.Length;
            int m = nums2.Length;
            Dictionary<int, (int, int)> dict = new();
            while (cur1 < n && cur2 < m) {
                if (nums1[cur1] < nums2[cur2]) {
                    cur1++;
                } else if (nums1[cur1] > nums2[cur2]) {
                    cur2++;
                } else {
                    dict.Add(nums1[cur1], (cur1, cur2));
                    cur1++;
                }
            }
            Dictionary<(int, bool, bool), long> map = new();
            return (int)(Math.Max(dfs(0, true, false), dfs(0, false, false)) % 1000000007);

            long dfs(int startIdx, bool isOne, bool jumped) {
                if (isOne && startIdx >= n) {
                    return 0;
                }
                if (!isOne && startIdx >= m) {
                    return 0;
                }

                if (map.ContainsKey((startIdx, isOne, jumped))) {
                    return map[(startIdx, isOne, jumped)];
                }
                long res = 0;
                if (isOne) {
                    if (dict.ContainsKey(nums1[startIdx]) && !jumped) {
                        res = Math.Max(nums1[startIdx] + dfs(startIdx + 1, true, false),
                            dfs(dict[nums1[startIdx]].Item2, false, true));

                    } else {
                        res = nums1[startIdx] + dfs(startIdx + 1, true, false);
                    }
                } else {
                    if (dict.ContainsKey(nums2[startIdx]) && !jumped) {
                        res = Math.Max(nums2[startIdx] + dfs(startIdx + 1, false, false),
                            dfs(dict[nums2[startIdx]].Item1, true, true));
                    } else {
                        res = nums2[startIdx] + dfs(startIdx + 1, false, false);
                    }
                }
                return map[(startIdx, isOne, jumped)] = res;
            }
        }

        public static int NumBusesToDestination(int[][] routes, int source, int target) {
            if (source == target) {
                return 0;
            }
            int n = 0;
            HashSet<int> hs = new(); // 合并含有source的所有车
            // 把所有包含source站点的车合并为一个大公交车
            int start = -1;
            for (int i = 0; i < routes.Length; i++) {
                for (int j = 0; j < routes[i].Length; j++) {
                    n = Math.Max(n, routes[i][j]);
                    if (routes[i][j] == source) {
                        if (start == -1) {
                            start = i; // 大公交车记为start路车
                        }
                        hs.Add(i);
                    }
                }
            }
            HashSet<int>[] hash = new HashSet<int>[n + 1];// 记录经过i号站点的公交车有哪路
            for (int i = 0; i < routes.Length; i++) {
                for (int j = 0; j < routes[i].Length; j++) {
                    int tmp = hs.Contains(i) ? start : i;
                    if (hash[routes[i][j]] == null) {
                        hash[routes[i][j]] = new();
                    }
                    hash[routes[i][j]].Add(tmp);
                }
            }
            List<int>[] graph = new List<int>[routes.Length + 1];// 我们要构建一张图，图的节点是不同路的公交车
            for (int i = 0; i < hash.Length; i++) {
                if (hash[i] is null) {
                    continue;
                }
                var list = hash[i].ToList();
                for (int j = 0; j < list.Count - 1; j++) {
                    for (int k = j + 1; k < list.Count; k++) {
                        int from = hs.Contains(list[j]) ? start : list[j];
                        int to = hs.Contains(list[k]) ? start : list[k];
                        if (graph[from] is null) {
                            graph[from] = new();
                        }
                        if (graph[to] is null) {
                            graph[to] = new();
                        }
                        if (from != to) {
                            graph[to].Add(from);
                            graph[from].Add(to);
                        }
                    }
                }
            }
            if (target >= hash.Length) {
                return -1;
            }
            Queue<(int, int)> que = new();
            HashSet<int> visited = new() { start };
            int res = int.MaxValue;
            que.Enqueue((start, 1));
            if (start == -1) {
                return -1;
            }
            while (que.Count > 0) {
                var tmp = que.Dequeue();
                if (hash[target] != null && hash[target].Contains(tmp.Item1)) {
                    res = Math.Min(res, tmp.Item2);
                }
                if (graph[tmp.Item1] != null) {
                    for (int i = 0; i < graph[tmp.Item1].Count; i++) {
                        if (visited.Add(graph[tmp.Item1][i])) {
                            que.Enqueue((graph[tmp.Item1][i], tmp.Item2 + 1));
                        }
                    }
                }
            }
            return res == int.MaxValue ? -1 : res;
        }

        public static int AtMostNGivenDigitSet(string[] digits, int n) {
            string s = n.ToString();
            int[,,] map = new int[s.Length, 2, 2];
            return dfs(0, true, false);

            int dfs(int m, bool done, bool zero) {
                int res = 0;
                if (m == s.Length - 1) {
                    if (done) {
                        for (int i = 0; i < digits.Length; i++) {
                            if (int.Parse(digits[i]) <= s[m] - '0') {
                                res++;
                            }
                        }
                    } else {
                        res = digits.Length;
                    }
                    return res;
                }
                if (map[m, done ? 1 : 0, zero ? 1 : 0] > 0) {
                    return map[m, done ? 1 : 0, zero ? 1 : 0];
                }
                if (m == 0) {
                    for (int i = 0; i < digits.Length; i++) {
                        if (int.Parse(digits[i]) <= s[m] - '0') {
                            res += dfs(m + 1, digits[i] == s[0].ToString(), false);
                        }
                    }
                    res += dfs(m + 1, false, true);
                } else if (!done) {
                    if (zero) {
                        res += dfs(m + 1, false, true);
                    }
                    for (int i = 0; i < digits.Length; i++) {
                        res += dfs(m + 1, false, false);
                    }
                } else {
                    for (int i = 0; i < digits.Length; i++) {
                        if (int.Parse(digits[i]) <= s[m] - '0') {
                            res += dfs(m + 1, digits[i] == s[m].ToString(), false);
                        }
                    }
                }
                return map[m, done ? 1 : 0, zero ? 1 : 0] = res;
            }
        }

        public static int LongestIdealString(string s, int k) {
            int[] hash = new int[26];
            Array.Fill(hash, -1);
            int n = s.Length;
            int[] dp = new int[n];
            dp[^1] = 1;
            int res = 0;
            hash[s[^1] - 'a'] = n - 1;
            for (int i = n - 2; i >= 0; i--) {
                int ch = s[i];
                int cur = 1;
                for (int j = -k; j <= k; j++) {
                    if (j + ch - 'a' < hash.Length && j + ch - 'a' >= 0 && hash[j + ch - 'a'] != -1) {
                        cur = Math.Max(cur, dp[hash[j + ch - 'a']] + 1);
                    }
                }
                dp[i] = cur;
                hash[ch - 'a'] = i;
                res = Math.Max(res, dp[i]);
            }
            return res;

        }

        public static string LongestDiverseString(int a, int b, int c) {
            int[] freq = { a, b, c };
            // 0 1 2
            char[] map = { 'a', 'b', 'c' };
            PriorityQueue<int, int> que = new(new MaxHeapComparer<int>());
            if (a > 0) {
                que.Enqueue(0, a);
            }
            if (b > 0) {
                que.Enqueue(1, b);
            }
            if (c > 0) {
                que.Enqueue(2, c);
            }
            StringBuilder sb = new();
            while (que.Count > 0) {
                var tmp1 = que.Dequeue();
                int minus1 = Math.Min(2, freq[tmp1]);
                if (que.Count == 0) {
                    sb.Append(new string(map[tmp1], minus1));
                    break;
                }
                freq[tmp1] -= minus1;


                var tmp2 = que.Dequeue();
                int minus2 = Math.Min(1, freq[tmp2]);
                if (sb.Length > 0 && sb[^1] - 'a' == tmp1) {
                    sb.Append(new string(map[tmp2], minus2));
                    sb.Append(new string(map[tmp1], minus1));
                } else {
                    sb.Append(new string(map[tmp1], minus1));
                    sb.Append(new string(map[tmp2], minus2));
                }

                freq[tmp2] -= minus2;
                if (freq[tmp1] > 0) {
                    que.Enqueue(tmp1, freq[tmp1]);
                }
                if (freq[tmp2] > 0) {
                    que.Enqueue(tmp2, freq[tmp2]);
                }
            }
            return sb.ToString();
        }

        public static int MinimumSeconds(IList<int> nums) {
            Dictionary<int, int> dict = new();
            Dictionary<int, int> gap = new();
            int n = nums.Count;
            for (int i = 0; i < n; i++) {
                if (!dict.ContainsKey(nums[i])) {
                    gap[nums[i]] = -1;
                    dict[nums[i]] = i;
                } else {
                    gap[nums[i]] = Math.Max(gap[nums[i]], (i - dict[nums[i]] - 1 + 1) / 2);
                    dict[nums[i]] = i;
                }
            }
            HashSet<int> done = new();
            for (int i = 0; i < n; i++) {
                if (done.Add(nums[i])) {
                    gap[nums[i]] = Math.Max(gap[nums[i]], (i + n - dict[nums[i]] - 1 + 1) / 2);
                }
            }
            int res = int.MaxValue;
            foreach (var item in gap) {
                res = Math.Min(res, item.Value);
            }
            return res;
        }

        public static string SmallestStringWithSwaps(string s, IList<IList<int>> pairs) {
            int n = s.Length;
            List<int>[] graph = new List<int>[n];
            for (int i = 0; i < pairs.Count; i++) {
                if (graph[pairs[i][0]] is null) {
                    graph[pairs[i][0]] = new();
                }
                if (graph[pairs[i][1]] is null) {
                    graph[pairs[i][1]] = new();
                }
                graph[pairs[i][0]].Add(pairs[i][1]);
                graph[pairs[i][1]].Add(pairs[i][0]);
            }
            bool[] visited = new bool[n];
            char[] res = new char[n];
            for (int i = 0; i < n; i++) {
                if (!visited[i]) {
                    Queue<int> que = new();
                    que.Enqueue(i);
                    visited[i] = true;
                    List<int> connected = new();
                    // bfs
                    while (que.Count > 0) {
                        int tmp = que.Dequeue();
                        connected.Add(tmp);
                        if (graph[tmp] is not null) {

                            foreach (var item in graph[tmp]) {
                                if (!visited[item]) {
                                    visited[item] = true;
                                    que.Enqueue(item);
                                }
                            }
                        }
                    }
                    push(connected);
                }
            }
            return string.Concat(res);

            void push(List<int> connect) {
                List<char> str = new();
                for (int i = 0; i < connect.Count; i++) {
                    str.Add(s[connect[i]]);
                }
                str.Sort();
                connect.Sort();
                for (int i = 0; i < connect.Count; i++) {
                    res[connect[i]] = str[i];
                }
            }
        }

        public static bool IsGoodArray(int[] nums) {
            return f(0, nums.Length - 1) == 1;

            int f(int l, int r) {
                if (l == r) {
                    return nums[l];
                }
                if (r - l == 1) {
                    return gcd(nums[l], nums[r]);
                }
                return gcd(f(l, (l + r) >> 1), f(((l + r) >> 1) + 1, r));
            }

            int gcd(int a, int b) {
                if (a < b) {
                    return gcd(b, a);
                }
                if (a % b == 0) {
                    return b;
                } else {
                    return gcd(b, a % b);
                }
            }
        }

        public static bool CloseStrings(string word1, string word2) {
            if (word1.Length != word2.Length) {
                return false;
            }
            Dictionary<int, int> hash = new();
            Dictionary<int, int> hash2 = new();
            for (int i = 0; i < word1.Length; i++) {
                if (!hash.ContainsKey(word1[i] - 'a')) {
                    hash[word1[i] - 'a'] = 0;
                }
                hash[word1[i] - 'a']++;
            }
            for (int i = 0; i < word2.Length; i++) {
                if (!hash2.ContainsKey(word2[i] - 'a')) {
                    hash2[word2[i] - 'a'] = 0;
                }
                hash2[word2[i] - 'a']++;
            }
            if (!hash.Keys.ToHashSet().SetEquals(hash2.Keys)) {
                return false;
            }
            int[] a = hash.Values.ToArray();
            int[] b = hash2.Values.ToArray();
            Array.Sort(a);
            Array.Sort(b);
            return a.SequenceEqual(b);
        }

        public static int MaxProductPath(int[][] grid) {
            int n = grid.Length;
            int m = grid[0].Length;
            const int MOD = 1000000007;
            Dictionary<int, (long, long)> map = new();
            long res = f(0, 0).Item2;
            if (res < 0) {
                return -1;
            }
            return (int)(res % MOD);

            (long, long) f(int x, int y) {
                if (map.ContainsKey(x * m + y)) {
                    return map[x * m + y];
                }
                int cur = grid[x][y];
                if (x == n - 1 && y == m - 1) {
                    return (cur, cur);
                }
                // min max
                int[][] dire = new int[][] { new int[] { 1, 0 }, new int[] { 0, 1 } };
                long min = int.MaxValue;
                long max = int.MinValue;
                if (cur < 0) {
                    (min, max) = (max, min);
                    for (int i = 0; i < dire.Length; i++) {
                        if (dire[i][0] + x < n && dire[i][1] + y < m) {
                            min = Math.Max(min, f(dire[i][0] + x, dire[i][1] + y).Item2);
                            max = Math.Min(max, f(dire[i][0] + x, dire[i][1] + y).Item1);
                        }
                    }

                } else {
                    for (int i = 0; i < dire.Length; i++) {
                        if (dire[i][0] + x < n && dire[i][1] + y < m) {
                            min = Math.Min(min, f(dire[i][0] + x, dire[i][1] + y).Item1);
                            max = Math.Max(max, f(dire[i][0] + x, dire[i][1] + y).Item2);
                        }
                    }
                }

                return map[x * m + y] = (min * cur, max * cur);
            }
        }

        public static int OrangesRotting(int[][] grid) {
            Queue<(int, int, int)> que = new();
            int count = 0; // 有多少腐烂的橘子
            int n = grid.Length, m = grid[0].Length;
            for (int i = 0; i < grid.Length; i++) {
                for (int j = 0; j < grid[i].Length; j++) {
                    if (grid[i][j] == 2) {
                        que.Enqueue((i, j, 0));
                        grid[i][j] = -1; // 已经统计过的腐烂的橘子
                    }
                    if (grid[i][j] == 1) {
                        count++;
                    }
                }
            }
            int[][] dire = new int[][] { new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 } };
            int res = 0;
            while (que.Count > 0) {
                var tmp = que.Dequeue();
                for (int i = 0; i < dire.Length; i++) {
                    int curX = tmp.Item1 + dire[i][0];
                    int curY = tmp.Item2 + dire[i][1];
                    if (curX >= 0 && curX < n && curY >= 0 && curY < m && grid[curX][curY] == 1) {
                        que.Enqueue((curX, curY, tmp.Item3 + 1));
                        res = Math.Max(res, tmp.Item3 + 1);
                        count--;

                        grid[curX][curY] = -1;
                    }
                }
            }
            if (count != 0) {
                return -1;
            }
            return res;
        }

        public static int MaxAbsoluteSum(int[] nums) {
            int dpMin = nums[0];
            int dpMax = nums[0];
            int max = dpMax;
            int min = dpMin;
            for (int i = 1; i < nums.Length; i++) {
                dpMax = Math.Max(nums[i], nums[i] + dpMax);
                dpMin = Math.Min(nums[i], nums[i] + dpMin);
                max = Math.Max(max, dpMax);
                min = Math.Min(min, dpMin);
            }
            return Math.Max(Math.Abs(max), Math.Abs(min));
        }

        public static IList<int> SplitIntoFibonacci(string num) {
            List<int> res = new();
            List<int> path = new();
            if (num.Length < 3) {
                return res;
            }
            int end1 = 0, end2 = 1;
            int n = num.Length;
            bool found = false;
            // [0..end1] [end1+1..end2]
            while (end2 + 1 < Math.Min(n, 20)) {
                // 枚举end1
                while (end1 < end2) {
                    int n1, n2;
                    if (int.TryParse(num[0..(end1 + 1)], out n1) && int.TryParse(num[(end1 + 1)..(end2 + 1)], out n2)) {
                        if (n1 != 0 && num[0] == '0' || n1 == 0 && end1 + 1 > 1) {
                            return res;
                        }
                        if (n2 != 0 && num[end1 + 1] == '0' || n2 == 0 && end2 - end1 > 1) {

                        } else {
                            path.Add(n1);
                            path.Add(n2);
                            f(
                                int.Parse(num[0..(end1 + 1)]),
                                int.Parse(num[(end1 + 1)..(end2 + 1)]),
                                end2 + 1);
                            if (found) {
                                return res;
                            }
                        }
                    }
                    end1++;
                    path.Clear();
                }

                end1 = 0;
                end2++;
            }
            return res;

            void f(int a, int b, int idx) {
                if (idx >= n) {
                    res = path.ToList();
                    found = true;
                    return;
                }
                if (Math.Max(a.ToString().Length, b.ToString().Length) > n - idx) {
                    // 剩余数字位数太少
                    return;
                }
                int c = a + b;
                int cur = 0;
                for (int i = idx; i < n; i++) {
                    cur *= 10;
                    cur += num[i] - '0';
                    if (c != 0 && num[idx] == '0' || cur == 0 && i - idx >= 1) {
                        break;
                    }
                    if (cur > c) {
                        break;
                    }
                    if (cur == c) {
                        path.Add(c);
                        f(b, c, i + 1);
                        path.RemoveAt(path.Count - 1);
                    }
                }
            }
        }

        public static int SubtractProductAndSum(int n) {
            int product = 1;
            int sum = 0;
            while (n > 0) {
                sum += n % 10;
                product *= n % 10;
                n /= 10;
            }
            return product - sum;
        }

        public static long[] MaximumSegmentSum(int[] nums, int[] removeQueries) {
            int n = nums.Length;
            int[] arr = new int[n];
            long[] res = new long[n];
            int[] fa = new int[n + 1];
            long[] sum = new long[n + 1];
            for (int i = 0; i < fa.Length; i++) { fa[i] = i; }
            for (int i = n - 1; i >= 1; i--) {
                int x = removeQueries[i];
                arr[x] = nums[x];
                // 保证每一段内的所有节点的头部是段的下一个，这就是循环不变量

                // 不管右侧存不存在值，不存在与右侧连相当于找到了一个虚拟头作为代表元，存在相当于直接union
                int to = fa[x] = find(x + 1);
                sum[to] += sum[x] + arr[x];
                res[i - 1] = Math.Max(sum[to], res[i]);
            }
            return res;

            int find(int n) {
                if (fa[n] == n) {
                    return n;
                }
                return fa[n] = find(fa[n]); //  路径压缩
            }
        }

        public static int LargestIsland(int[][] grid) {
            int n = grid.Length;
            int[] fa = new int[n * n];
            int[] s = new int[fa.Length];
            int[][] dire = new int[][] { new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 } };
            for (int i = 0; i < fa.Length; i++) { fa[i] = i; }
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    s[i * n + j] = grid[i][j];
                }
            }
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    int cur = i * n + j;
                    if (grid[i][j] == 1) {

                        for (int k = 0; k < dire.Length; k++) {
                            if (i + dire[k][0] >= 0 && i + dire[k][0] < n &&
                                j + dire[k][1] >= 0 && j + dire[k][1] < n && grid[i + dire[k][0]][j + dire[k][1]] == 1) {

                                union(cur, (i + dire[k][0]) * n + j + dire[k][1]);
                            }
                        }
                    }
                }
            }
            int res = s.Max();
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    if (grid[i][j] == 0) {
                        HashSet<int> root = new();
                        for (int k = 0; k < dire.Length; k++) {
                            if (i + dire[k][0] >= 0 && i + dire[k][0] < n &&
                                j + dire[k][1] >= 0 && j + dire[k][1] < n) {

                                root.Add(find((i + dire[k][0]) * n + j + dire[k][1]));
                            }
                        }
                        int add = 0;
                        foreach (var item in root) {
                            add += s[item];
                        }
                        res = Math.Max(res, 1 + add);
                    }
                }
            }
            return res;

            int find(int n) {
                if (fa[n] == n) {
                    return n;
                }
                return fa[n] = find(fa[n]);
            }

            void union(int x, int y) {
                int a = find(x);
                int b = find(y);
                if (a != b) {
                    s[a] += s[b];
                    fa[b] = a;
                }
            }
        }

        public static int NumSimilarGroups(string[] strs) {
            int n = strs.Length;
            int[] fa = new int[n];
            int count = n;
            int m = strs[0].Length;
            for (int i = 0; i < n; i++) {
                fa[i] = i;
            }
            for (int i = 0; i < n; i++) {
                for (int j = i + 1; j < n; j++) {
                    if (check(i, j)) {
                        union(i, j);
                    }
                }
            }
            return count;

            bool check(int x, int y) {
                int cnt = 0;
                for (int i = 0; i < m; i++) {
                    if (strs[x][i] != strs[y][i]) {
                        cnt++;
                    }
                }
                return cnt <= 2;
            }

            int find(int n) {
                if (fa[n] == n)
                    return n;
                return fa[n] = find(fa[n]);
            }

            void union(int x, int y) {
                int a = find(x);
                int b = find(y);
                if (a != b) {
                    fa[a] = b;
                    count--;
                }
            }
        }

        public static double ChampagneTower(int poured, int query_row, int query_glass) {
            double[][] cups = new double[query_row + 1][];
            for (int i = 0; i <= query_row; i++) {
                cups[i] = new double[i + 1];
            }

            cups[0][0] = poured;
            for (int i = 1; i <= query_row; i++) {
                for (int j = 0; j < cups[i].Length; j++) {
                    double l = Math.Max(j < cups[i - 1].Length ? cups[i - 1][j] - 1 : 0, 0);
                    double r = Math.Max((j > 0 ? cups[i - 1][j - 1] : 0) - 1, 0);
                    cups[i][j] = l / 2 + r / 2;
                }
            }
            return cups[^1][query_glass];
        }

        public static int MinFallingPathSum2(int[][] grid) {
            int n = grid.Length; // n行n列
            //int[] dp = new int[n];
            int min1 = int.MaxValue;// 第一小的值
            int min2 = int.MaxValue;
            int idx1 = -1;// 第一小的idx
            for (int i = 0; i < n; i++) {
                //dp[i] = grid[^1][i];
                int x = grid[^1][i];
                upd(x, i);
            }
            for (int i = n - 2; i >= 0; i--) {
                for (int j = 0; j < n; j++) {
                    int x = grid[i][j];
                    if (idx1 != j) {
                        grid[i][j] = min1;
                    } else {
                        grid[i][j] = min2;
                    }

                    grid[i][j] += x;
                }
                idx1 = -1;
                min1 = min2 = int.MaxValue;
                for (int j = 0; j < n; j++) {
                    upd(grid[i][j], j);
                }
            }
            int res = int.MaxValue;
            for (int i = 0; i < n; i++) {
                res = Math.Min(res, grid[0][i]);
            }
            return res;

            void upd(int x, int i) {
                if (x < min1) {
                    min2 = min1;

                    min1 = x;
                    idx1 = i;
                } else if (x < min2) {
                    min2 = x;
                }
            }
        }

        public static IList<string> AmbiguousCoordinates(string s) {
            s = s[0..^1];
            int sep;
            List<string> res = new();
            for (sep = 1; sep < s.Length; sep++) {
                List<string> front = f(s[..sep]);
                List<string> back = f(s[sep..]);
                for (int i = 0; i < front.Count; i++) {
                    for (int j = 0; j < back.Count; j++) {
                        string cur = $"({front[i]}, {back[j]})";
                        res.Add(cur);
                    }
                }
            }
            return res;

            static List<string> f(string a) {
                List<string> res = new();
                for (int i = 1; i < a.Length; i++) {
                    string tmp = a[..i] + "." + a[i..];
                    if (check(tmp)) {
                        res.Add(tmp);
                    }
                }
                // 不带小数点
                if (check(a)) {
                    res.Add(a);
                }
                return res;
            }

            static bool check(string s) {
                int p = 0;
                if (s.Length == 1) {
                    return true;
                }
                if (s == "0") {
                    return true;
                }
                if (s[0] == '0' && s[1] != '.') {
                    return false;
                }
                while (p < s.Length && s[p] != '.') {
                    p++;
                }

                if (long.Parse(s[..p]) == 0 && p != 1) {
                    return false;
                }
                if (s[^1] == '0' && p != s.Length) { // 有小数点还有后导零
                    return false;
                }
                return true;
            }
        }

        public static int MinEatingSpeed(int[] piles, int h) {
            // 0 0 1 1 1
            int l = 0;
            int r = piles.Max();
            while (l < r) {
                int m = (l + r) >> 1;
                if (check(m)) {
                    r = m;
                } else {
                    l = m + 1;
                }
            }
            return l;

            bool check(int n) {
                int sum = 0;
                for (int i = 0; i < piles.Length; i++) {
                    sum += (int)Math.Ceiling((double)piles[i] / n);
                }
                return sum <= h;
            }
        }

        public static string PushDominoes(string dominoes) {
            int n = dominoes.Length;
            char[] res = new char[n];
            // idx, time, direct
            Queue<(int, int, int)> que = new();
            // direct: ’L' 'R' 0
            int[] time = new int[n]; // 最后一次受力的时间
            for (int i = 0; i < n; i++) {
                if (dominoes[i] == 'L') {
                    time[i] = 0;
                    que.Enqueue((i, 0, 'L'));
                } else if (dominoes[i] == 'R') {
                    time[i] = 0;
                    que.Enqueue((i, 0, 'R'));
                }
            }
            while (que.Count > 0) {
                var tmp = que.Dequeue();
                if (res[tmp.Item1] == 0) { // 已经定好的状态不应该再被修改，除非一个牌同一时间被撞击
                    if (tmp.Item3 == 0) {
                        res[tmp.Item1] = '.';
                    } else if (tmp.Item3 == 'R') {
                        res[tmp.Item1] = 'R';
                    } else {
                        res[tmp.Item1] = 'L';
                    }

                    if (tmp.Item1 != 0 && tmp.Item3 == 'L' && dominoes[tmp.Item1 - 1] == '.') { // 只能让立着的倒下
                        if (time[tmp.Item1 - 1] == tmp.Item2 + 1) {
                            res[tmp.Item1 - 1] = '.';
                        } else {
                            que.Enqueue((tmp.Item1 - 1, tmp.Item2 + 1, tmp.Item3));
                            time[tmp.Item1 - 1] = tmp.Item2 + 1;
                        }
                    }
                    if (tmp.Item1 != n - 1 && tmp.Item3 == 'R' && dominoes[tmp.Item1 + 1] == '.') {
                        if (time[tmp.Item1 + 1] == tmp.Item2 + 1) {
                            res[tmp.Item1 + 1] = '.';
                        } else {
                            que.Enqueue((tmp.Item1 + 1, tmp.Item2 + 1, tmp.Item3));
                            time[tmp.Item1 + 1] = tmp.Item2 + 1;
                        }
                    }
                }
            }
            for (int i = 0; i < n; i++) {
                if (res[i] == 0) {
                    res[i] = '.';
                }
            }
            return string.Concat(res);
        }

        public static int VideoStitching(int[][] clips, int time) {
            int n = clips.Length;
            int[,] map = new int[n, n + 1];
            Array.Sort(clips, (a, b) => a[0] != b[0] ? a[0].CompareTo(b[0]) : a[1].CompareTo(b[1]));
            return dfs(0, -1);

            int dfs(int idx, int s) { // 从clips[s][1]时间出发到time需要的最短线段个数
                int res;
                int st = s == -1 ? 0 : clips[s][1];
                if (idx >= n) {
                    return -1;
                }
                if (map[idx, s + 1] > 0 || map[idx, s + 1] == -1) {
                    return map[idx, s + 1];
                }
                if (clips[idx][0] > st) {
                    return -1;
                }
                if (clips[idx][1] >= time) {
                    return 1;
                }
                // choose
                int case1 = dfs(idx + 1, idx);
                int case2 = dfs(idx + 1, s);
                if (case1 < 0) {
                    res = case2;
                } else {
                    res = case1 + 1;
                    if (case2 > 0) {
                        res = Math.Min(res, case2);
                    }
                }

                return map[idx, s + 1] = res;
            }
        }

        public static int MinFlipsMonoIncr(string s) {
            int zero = s.Count(a => a == '0');
            int one = 0; // 1 of left area, [i..)
            int res = int.MaxValue;
            for (int i = 0; i < s.Length;) {
                res = Math.Min(res, one + zero);

                if (s[i] == '0') {
                    zero--;
                } else {
                    one++;
                }
                i++;
            }
            res = Math.Min(res, zero + one);
            return res;
        }

        public static int KConcatenationMaxSum(int[] arr, int k) {
            int n = arr.Length;
            const int MOD = 1000000007;
            long sum = arr.Sum();

            long dp = arr[0];
            long res = dp;
            for (int i = 1; i < n; i++) {
                dp = Math.Max(arr[i], arr[i] + dp);
                res = Math.Max(res, dp);
            }

            if (k >= 2) {
                long lSum = 0;
                long rSum = 0;
                long s = 0;

                for (int i = 0; i < n; i++) {
                    s += arr[i];
                    rSum = Math.Max(rSum, s);
                }
                s = 0;
                for (int i = n - 1; i >= 0; i--) {
                    s += arr[i];
                    lSum = Math.Max(lSum, s);
                }
                if (rSum + lSum + Math.Max(0, sum % MOD * (k - 2)) > res) {
                    return Math.Max(0, (int)((rSum % MOD + lSum % MOD + Math.Max(0, sum % MOD * (k - 2))) % MOD));
                } else {
                    return res < 0 ? 0 : (int)(res % MOD);
                }

            } else {
                return res < 0 ? 0 : (int)(res % MOD);
            }
        }

        public static int CountVowelPermutation(int n) {
            const int MOD = 1000000007;
            Dictionary<(int, int), long> map = new();
            return (int)((dfs(n - 1, 0) % MOD + dfs(n - 1, 1) % MOD + dfs(n - 1, 2) % MOD + dfs(n - 1, 3) % MOD + dfs(n - 1, 4) % MOD) % MOD);

            long dfs(int n, int c) {
                if (n == 0) {
                    return 1;
                }
                if (map.ContainsKey((n, c))) {
                    return map[(n, c)];
                }
                return map[(n, c)] = c switch {
                    0 => dfs(n - 1, 1) % MOD,
                    1 => (dfs(n - 1, 0) % MOD + dfs(n - 1, 2) % MOD) % MOD,
                    2 => (dfs(n - 1, 0) % MOD + dfs(n - 1, 1) % MOD + dfs(n - 1, 3) % MOD + dfs(n - 1, 4) % MOD) % MOD,
                    3 => (dfs(n - 1, 2) % MOD + dfs(n - 1, 4) % MOD) % MOD,
                    _ => dfs(n - 1, 0) % MOD,
                };
            }
        }

        public static IList<int> CircularPermutation(int n, int start) {
            int[] code = new int[1 << n];
            int s = -1;
            code[1] = 1;
            if (start <= 1) {
                s = start;
            }
            for (int i = 1; i < n; i++) {
                for (int j = 1 << i; j < 1 << (i + 1); j++) {
                    code[j] = code[(1 << i) - (j - (1 << i) + 1)] | (1 << i);
                    if (code[j] == start) {
                        s = j;
                    }
                }
            }
            int[] res = new int[1 << n];
            for (int i = 0; i < res.Length; i++) {
                res[i] = code[(s + i) % code.Length];
            }
            return res;
        }

        public static bool BtreeGameWinningMove(TreeNode root, int n, int x) {
            int l = 0;
            int r = 0;
            dfs(root);
            return l > n - l || r > n - r || n - (l + r + 1) > (l + r + 1);

            int dfs(TreeNode? node) { // 返回包括自己的整棵树的大小
                if (node is null) {
                    return 0;
                }
                int a = dfs(node.left), b = dfs(node.right);
                if (node.val == x) {
                    l = a;
                    r = b;
                }
                return a + 1 + b;
            }
        }

        public static int MinOperations(int[] nums1, int[] nums2) {
            int[] hash1 = new int[7];
            int[] hash2 = new int[7];
            int sum1 = 0, sum2 = 0;
            for (int i = 0; i < nums1.Length; i++) {
                hash1[nums1[i]]++;
                sum1 += nums1[i];
            }
            for (int i = 0; i < nums2.Length; i++) {
                hash2[nums2[i]]++;
                sum2 += nums2[i];
            }
            if (sum1 < sum2) {
                return MinOperations(nums2, nums1);
            }
            int cur1 = 6;
            int cur2 = 1;
            int diff = sum1 - sum2;
            if (diff == 0) {
                return 0;
            }
            if (nums1.Length * 6 < nums2.Length || nums2.Length * 6 < nums1.Length) {
                return -1;
            }
            int res = 0;
            while (true) {
                while (cur1 >= 1 && hash1[cur1] == 0) { cur1--; }
                while (cur2 < 7 && hash2[cur2] == 0) { cur2++; }
                diff = sum1 - sum2;
                int diff1 = cur1 - 1;
                int diff2 = 6 - cur2;
                res++; // 操作一次加一
                if (diff1 > diff2) {
                    if (diff1 >= diff) {
                        return res;
                    } else {
                        sum1 -= diff1;
                        hash1[cur1]--;
                        hash1[1]++;
                    }
                } else {
                    if (diff2 >= diff) {
                        return res;
                    } else {
                        sum2 += diff2;
                        hash2[cur2]--;
                        hash2[6]++;
                    }
                }
            }
        }

        public static int LongestSubsequence(string s, int k) {
            int n = s.Length;
            long[] min = new long[n + 1];
            Array.Fill(min, int.MaxValue);
            long[,] dp = new long[n, n + 1];
            dp[n - 1, 1] = s[^1] - '0';
            min[1] = dp[n - 1, 1];
            for (int i = n - 2; i >= 0; i--) {
                dp[i, 1] = s[i] - '0';
                min[1] = Math.Min(min[1], dp[i, 1]);
                for (int j = 2; j <= n - i; j++) {
                    dp[i, j] = (calc(s[i] - '0', j - 1)) + min[j - 1];
                }
                for (int j = 2; j <= n - i; j++) {
                    min[j] = Math.Min(min[j], dp[i, j]);
                }
            }

            int res = 0;
            for (int i = 1; i <= n; i++) {
                if (min[i] <= k) {
                    res = Math.Max(res, i);
                }
            }
            return res;

            int calc(int l, int r) {
                return !check(l, r) ? k + 1 : l << r;
            }

            bool check(int l, int r) {
                return l == 0 ? true : r < 31;
            }
        }

        public static int MinMalwareSpread(int[][] graph, int[] initial) {
            int n = graph.Length;
            int[] fa = new int[n];
            int[] size = new int[n];
            Array.Fill(size, 1);
            for (int i = 0; i < n; i++) { fa[i] = i; }
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    if (graph[i][j] == 1) {
                        union(i, j);
                    }
                }
            }
            Array.Sort(initial);
            int[] cnt = new int[n];
            for (int i = 0; i < initial.Length; i++) {
                cnt[find(initial[i])]++;
            }
            int max = 0;
            int res = -1;
            for (int i = 0; i < initial.Length; i++) {
                int x = find(initial[i]);
                if (cnt[x] == 1) {
                    if (size[x] > max) {
                        max = size[x];
                        res = initial[i];
                    }
                }
            }
            return max == 0 ? initial[0] : res;

            void union(int a, int b) {
                int x = find(a);
                int y = find(b);
                if (x == y) { return; }
                if (x < y) {
                    size[x] += size[y];
                    fa[y] = x;
                } else {
                    size[y] += size[x];
                    fa[x] = y;
                }
            }

            int find(int n) {
                if (fa[n] == n) {
                    return n;
                }
                return fa[n] = find(fa[n]);
            }
        }

        public static int MinMalwareSpreadII(int[][] graph, int[] initial) {
            int n = graph.Length;
            int[] fa = new int[n];
            for (int i = 0; i < n; i++) { fa[i] = i; }
            int[] size = new int[n];
            HashSet<int> set = new(initial);
            Array.Fill(size, 1);
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    if (graph[i][j] == 1 && !set.Contains(i) && !set.Contains(j)) {
                        union(i, j);
                    }
                }
            }

            Array.Sort(initial);
            HashSet<int>[] infect = new HashSet<int>[n]; // 每个联通块受到了哪些源节点的影响
            for (int i = 0; i < initial.Length; i++) {
                for (int j = 0; j < n; j++) {
                    if (graph[initial[i]][j] == 1 && initial[i] != j && !set.Contains(j)) {
                        if (infect[find(j)] is null) {
                            infect[find(j)] = new();
                        }
                        infect[find(j)].Add(initial[i]);
                    }
                }
            }
            int max = 0; // 查找影响节点最多的点，影响最多：指把不受其他节点感染的节点感染了
            int res = initial[0];
            for (int i = 0; i < initial.Length; i++) {
                int tmp = 0;
                HashSet<int> seen = new();
                for (int j = 0; j < n; j++) {
                    if (graph[initial[i]][j] == 1 && initial[i] != j && !set.Contains(j)) {
                        if (seen.Add(find(j))
                            && infect[find(j)].Contains(initial[i])
                            && infect[find(j)].Count == 1) {
                            tmp += size[find(j)];
                        }
                    }
                }
                if (tmp > max) {
                    max = tmp;
                    res = initial[i];
                }
            }
            return res;

            void union(int a, int b) {
                int x = find(a);
                int y = find(b);
                if (x == y) {
                    return;
                }
                size[y] += size[x];
                fa[x] = y;
            }

            int find(int n) {
                if (fa[n] == n) {
                    return n;
                }
                return fa[n] = find(fa[n]);
            }
        }

        public static TreeNode? MergeTrees(TreeNode root1, TreeNode root2) {
            return f(root1, root2);

            TreeNode? f(TreeNode? n1, TreeNode? n2) {
                TreeNode res;
                if (n1 is null && n2 is null) {
                    return null;
                }
                res = new((n1 is null ? 0 : n1.val) + (n2 is null ? 0 : n2.val));
                res.left = f(n1?.left, n2?.left);
                res.right = f(n1?.right, n2?.right);
                return res;
            }
        }

        public static IList<int> FlipMatchVoyage(TreeNode root, int[] voyage) {
            bool could = true;
            int cur = 0;
            List<int> res = new();
            f(root);
            if (!could) {
                res = new() { -1 };
            }
            return res;

            void f(TreeNode? node) {
                if (node == null) { return; }
                if (node.val != voyage[cur++]) {
                    could = false;
                }
                if (!could) {
                    return;
                }
                if (node.left is null && node.right is null) {
                    return;
                }
                if (node.left is not null && node.left.val != voyage[cur]) {
                    res.Add(node.val);
                    f(node.right);
                    f(node.left);
                } else {
                    f(node.left);
                    f(node.right);
                }

            }
        }

        public static int[] MaxDepthAfterSplit(string seq) {
            int n = seq.Length;
            int[] res = new int[n];
            res[0] = 1;
            int max = 0;
            for (int i = 1; i < n; i++) {
                res[i] = res[i - 1] + (seq[i] == '(' ? 1 : -1);
                max = Math.Max(max, res[i]);
            }
            for (int i = 0; i < n; i++) {
                if (seq[i] == '(') {
                    res[i] = res[i] > max / 2 ? 1 : 0;
                } else {
                    res[i] = res[i] >= max / 2 ? 1 : 0;
                }
            }
            return res;
        }

        public static string FindReplaceString(string s, int[] indices, string[] sources, string[] targets) {
            StringBuilder res = new();
            int[] items = new int[indices.Length];
            for (int i = 0; i < items.Length; i++) { items[i] = i; }
            Array.Sort(items, (a, b) => indices[a].CompareTo(indices[b]));
            int j = 0;
            for (int i = 0; i < s.Length; i++) {
                // indices[j] > i
                bool flag = false;
                while (j < indices.Length && indices[items[j]] <= i) {
                    j++;
                    flag = true;
                }
                if (flag)
                    j--;
                if (indices[items[j]] == i && s.IndexOf(sources[items[j]], indices[items[j]]) == indices[items[j]]) {
                    res.Append(targets[items[j]]);
                    i += sources[items[j]].Length - 1;
                } else {
                    res.Append(s[i]);
                }
            }
            return res.ToString();
        }

        public static int[] CircularGameLosers(int n, int k) {
            int[] ply = new int[n];
            int max = 1;
            int last = 0;
            int i = 1;
            ply[0] = 1;
            List<int> res = new();
            while (max != 2) {
                last = (last + k * i) % n;
                ply[last]++;
                i++;
                max = Math.Max(max, ply[last]);
            }
            for (int j = 0; j < n; j++) {
                if (ply[j] == 0) {
                    res.Add(j + 1);
                }
            }
            return res.ToArray();
        }

        public static int Ways(string[] pizza, int k) {
            const int MOD = 1000000007;
            int n = pizza.Length;           // row
            int m = pizza[0].Length;        // column
            int[][] pz = new int[n][];
            int[,,] map = new int[n, m, k];
            int[,] sum = new int[n, m]; // 前缀和
            for (int i = 0; i < n; i++) {
                pz[i] = new int[m];
                for (int j = 0; j < m; j++) {
                    pz[i][j] = pizza[i][j] == 'A' ? 1 : 0;
                }
            }
            sum[0, 0] = pz[0][0];
            for (int i = 1; i < n; i++) {
                sum[i, 0] = pz[i][0] + sum[i - 1, 0];
            }
            for (int i = 1; i < m; i++) {
                sum[0, i] = pz[0][i] + sum[0, i - 1];
            }
            for (int i = 1; i < n; i++) {
                for (int j = 1; j < m; j++) {
                    sum[i, j] = sum[i - 1, j] + sum[i, j - 1] - sum[i - 1, j - 1] + pz[i][j];
                }
            }
            return f(0, 0, k - 1) % MOD;

            int f(int x, int y, int k) { // x: row, y: column
                if (k == 0) {
                    return get(x, y, n - 1, m - 1) != 0 ? 1 : 0;
                }

                if (map[x, y, k] > 0) {
                    return map[x, y, k] - 1;
                }

                int res = 0;
                // 横着
                int remain = get(x, y, x, m - 1);
                for (int i = x + 1; i < n; i++) {
                    if (remain > 0) {
                        res = (res % MOD + f(i, y, k - 1) % MOD) % MOD;
                    } else {
                        remain += get(i, y, i, m - 1);
                    }
                }
                // 竖着
                remain = get(x, y, n - 1, y);
                for (int i = y + 1; i < m; i++) {
                    if (remain > 0) {
                        res = (res % MOD + f(x, i, k - 1) % MOD) % MOD;
                    } else {
                        remain += get(x, i, n - 1, i);
                    }

                }
                map[x, y, k] = res + 1;
                return res;
            }

            int get(int x, int y, int p, int q) {
                int a = sum[p, q];
                int d = x > 0 && y > 0 ? sum[x - 1, y - 1] : 0;
                int b = x > 0 ? sum[x - 1, q] : 0;
                int c = y > 0 ? sum[p, y - 1] : 0;
                return a - b - c + d;
            }
        }

        public static int[] CycleLengthQueries(int n, int[][] queries) {
            Dictionary<int, int> path = new();
            int[] res = new int[queries.Length];
            for (int i = 0; i < queries.Length; i++) {
                path.Clear();
                int cur = queries[i][0];
                int cnt = 0;
                while (cur > 0) {
                    path.Add(cur, cnt++);
                    cur >>= 1;
                }
                cur = queries[i][1];
                int anc = 1;
                cnt = 0;
                while (cur > 0) {
                    if (path.ContainsKey(cur)) {
                        anc = cur;
                        break;
                    }
                    cur >>= 1;
                    cnt++;
                }
                res[i] = cnt + path[anc] + 1;
            }
            return res;
        }

        public static int MaxSizeSlices(int[] slices) {
            return Math.Max(f(slices[1..]), f(slices[..^1]));

            int f(int[] arr) {
                int[,] dp = new int[arr.Length, slices.Length / 3 + 1];
                dp[0, 1] = arr[0];
                for (int i = 1; i < dp.GetLength(0); i++) { // 从[0..i]里选一个数
                    dp[i, 1] = Math.Max(arr[i], dp[i - 1, 1]);
                }
                for (int i = 2; i < dp.GetLength(0); i++) {
                    for (int j = 2; j < dp.GetLength(1); j++) {
                        dp[i, j] = Math.Max(
                            dp[i - 1, j],
                            dp[i - 2, j - 1] + arr[i]
                            );
                    }
                }
                int res = 0;
                for (int i = 0; i < arr.Length; i++) {
                    res = Math.Max(res, dp[i, slices.Length / 3]);
                }
                return res;
            }

        }

        public static int MinSwap(int[] nums1, int[] nums2) {
            int n = nums1.Length;
            int[,] dp = new int[n, 2];
            // 使得[0..i]严格递增，且第i个元素对换/未对换，需要最少多少次对换
            dp[0, 1] = 1;
            for (int i = 1; i < n; i++) {
                // 不换
                int x = int.MaxValue;
                if (nums1[i] > nums1[i - 1] && nums2[i] > nums2[i - 1]) {
                    x = Math.Min(x, dp[i - 1, 0]);
                }
                if (nums1[i] > nums2[i - 1] && nums2[i] > nums1[i - 1]) {
                    x = Math.Min(x, dp[i - 1, 1]);
                }
                dp[i, 0] = x;
                // 换
                x = int.MaxValue;
                if (nums1[i] > nums1[i - 1] && nums2[i] > nums2[i - 1]) {
                    x = Math.Min(x, dp[i - 1, 1] + 1);
                }
                if (nums1[i] > nums2[i - 1] && nums2[i] > nums1[i - 1]) {
                    x = Math.Min(x, dp[i - 1, 0] + 1);
                }
                dp[i, 1] = x;
            }
            return Math.Min(dp[n - 1, 0], dp[n - 1, 1]);
        }

        public static int Sum(int num1, int num2) {
            int carry = (num1 & num2) << 1;
            int xor = num1 ^ num2;
            while (carry != 0) {
                int tmp = xor;
                xor ^= carry;
                carry = (carry & tmp) << 1;
            }
            return xor;
        }

        public static bool CanChange(string start, string target) {
            List<int> st = new();
            List<int> tgt = new();
            int n = start.Length;
            for (int i = 0; i < n; i++) {
                if (start[i] != '_') {
                    st.Add(i);
                }
                if (target[i] != '_') {
                    tgt.Add(i);
                }
            }
            if (st.Count != tgt.Count) {
                return false;
            }
            for (int i = 0; i < st.Count; i++) {
                if (start[st[i]] != target[tgt[i]]) {
                    return false;
                }
                if (start[st[i]] == 'R') {
                    if (st[i] > tgt[i]) {
                        return false;
                    }
                } else {
                    if (st[i] < tgt[i]) {
                        return false;
                    }
                }
            }
            return true;
        }

        public static int MaxDistToClosest(int[] seats) {
            List<int> arr = new();
            for (int i = 0; i < seats.Length; i++) {
                if (seats[i] != 0) {
                    arr.Add(i);
                }
            }

            int res = 0;
            res = Math.Max(Math.Max(0, arr[0]), Math.Max(0, seats.Length - arr[^1] - 1));
            for (int i = 0; i < arr.Count - 1; i++) {
                res = Math.Max(res, (arr[i + 1] - arr[i]) / 2);
            }
            return res;
        }

        public static int CountServers(int[][] grid) {
            int n = grid.Length;
            int m = grid[0].Length;
            int[] row = new int[n];
            int[] cols = new int[m];
            int res = 0;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    if (grid[i][j] == 1) {
                        row[i]++;
                        cols[j]++;
                        res++;
                    }
                }
            }
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    if (grid[i][j] == 1 && row[i] == 1 && cols[j] == 1) {
                        res--;
                    }
                }
            }
            return res;
        }

        public static long MinimumCost(string s) {
            return Math.Min(f(s), f(s.Replace('0', '2').Replace('1', '0').Replace('2', '1')));

            long f(string _s) {
                int n = _s.Length;
                int[] help1 = new int[n];
                int[] help2 = new int[n];
                help1[0] = _s[0] - '0'; // 先计算变成全零的答案
                for (int i = 1; i < n; i++) {
                    help1[i] = _s[i - 1] != _s[i] ? help1[i - 1] + 1 : help1[i - 1];
                }
                help2[^1] = _s[^1] - '0';
                for (int i = n - 2; i >= 0; i--) {
                    help2[i] = _s[i + 1] != _s[i] ? help2[i + 1] + 1 : help2[i + 1];
                }
                long l = 0, r = help1.Sum((a) => (long)a);
                long prefixL = 0;
                long prefixR = r;
                long res = r;
                for (long i = 1; i < n; i++) { // 在i的前面进行分割
                    prefixL += help2[i - 1];
                    l = _s[(int)(i - 1)] == '0' ? prefixL - i * help2[i - 1] : prefixL - i * (help2[i - 1] - 1);

                    prefixR -= help1[i - 1];
                    r = _s[(int)i] == '0' ? prefixR - (n - i) * help1[i] : prefixR - (n - i) * (help1[i] - 1);

                    res = Math.Min(res, l + r);
                }
                res = Math.Min(res, prefixL);
                return res;

            }
        }

        public static int GoodNodes(TreeNode root) {
            int res = 0;
            f(root, int.MinValue);
            return res;

            void f(TreeNode? node, int max) {
                if (node is null) {
                    return;
                }
                if (node.val >= max) {
                    res++;
                }
                f(node.left, Math.Max(node.val, max));
                f(node.right, Math.Max(node.val, max));
            }
        }

        public static IList<string> SummaryRanges(int[] nums) {
            int start = 0, end = 0;
            List<string> res = new();
            while (end < nums.Length) {
                while (end + 1 < nums.Length && nums[end + 1] == nums[end] + 1) {
                    end++;
                }
                res.Add(nums[start] == nums[end] ? $"{nums[start]}" : $"{nums[start]}->{nums[end]}");
                start = ++end;
            }
            return res;
        }

        public static int MinExtraChar(string s, string[] dictionary) {
            HashSet<string> dict = new(dictionary);
            int maxLen = 0;
            for (int i = 0; i < dictionary.Length; i++) {
                maxLen = Math.Max(maxLen, dictionary[i].Length);
            }
            int n = s.Length;
            int[] map = new int[n];
            Array.Fill(map, -1);
            return f(0);


            int f(int idx) {
                if (idx >= n) {
                    return 0;
                }
                if (map[idx] >= 0) {
                    return map[idx];
                }

                int res = int.MaxValue;
                for (int i = idx; i < n; i++) { // 枚举开始位置
                    for (int j = i; j < Math.Min(n, i + maxLen); j++) { // 枚举结束位置
                        if (dict.Contains(s[i..(j + 1)])) {
                            res = Math.Min(res, i - idx + f(j + 1));
                        }
                    }
                }

                return map[idx] = res == int.MaxValue ? n - idx : res;
            }
        }

        public static int UniqueLetterString(string s) {
            List<int>[] pos = new List<int>[26];
            for (int i = 0; i < 26; i++) {
                pos[i] = new() { -1 };
            }
            for (int i = 0; i < s.Length; i++) {
                pos[s[i] - 'A'].Add(i);
            }
            for (int i = 0; i < 26; i++) {
                pos[i].Add(s.Length);
            }
            int res = 0;
            for (int i = 0; i < 26; i++) {
                for (int j = 1; j < pos[i].Count - 1; j++) {
                    res += (pos[i][j] - pos[i][j - 1])
                        * (pos[i][j + 1] - pos[i][j]);
                }
            }
            return res;
        }

        public static int MaxAbsValExpr(int[] arr1, int[] arr2) {
            int val1 = arr1[0] + arr2[0] - 0;
            int val2;
            int res = int.MinValue;
            int n = arr1.Length;
            for (int i = 1; i < n; i++) {
                val2 = -(arr1[i] + arr2[i] - i);
                res = Math.Max(res, val2 + val1);
                val1 = Math.Max(val1, arr1[i] + arr2[i] - i);
            }
            val1 = arr1[0] - arr2[0] - 0;
            for (int i = 1; i < n; i++) { // 第二个绝对值内是负的
                val2 = -arr1[i] + arr2[i] + i;
                res = Math.Max(res, val1 + val2);
                val1 = Math.Max(val1, arr1[i] - arr2[i] - i);
            }
            val1 = -arr1[0] - arr2[0] - 0;
            for (int i = 1; i < n; i++) {
                val2 = arr1[i] + arr2[i] + i;
                res = Math.Max(res, val1 + val2);
                val1 = Math.Max(val1, -arr1[i] - arr2[i] - i);
            }
            val1 = -arr1[0] + arr2[0] - 0;
            for (int i = 1; i < n; i++) {
                val2 = arr1[i] - arr2[i] + i;
                res = Math.Max(res, val1 + val2);
                val1 = Math.Max(val1, -arr1[i] + arr2[i] - i);
            }
            return res;
        }

        public static int RemoveStones(int[][] stones) {
            int[] fa = new int[stones.Length];
            int[] size = new int[fa.Length];

            for (int i = 0; i < fa.Length; i++) {
                fa[i] = i;
                size[i] = 1;
            }
            int n = 0, m = 0;
            for (int i = 0; i < stones.Length; i++) {
                n = Math.Max(n, stones[i][0]);
                m = Math.Max(m, stones[i][1]);
            }
            List<int>[] row = new List<int>[n + 1];
            List<int>[] col = new List<int>[m + 1];

            for (int i = 0; i < stones.Length; i++) {
                if (row[stones[i][0]] is null) {
                    row[stones[i][0]] = new();
                }
                if (col[stones[i][1]] is null) {
                    col[stones[i][1]] = new();
                }
                row[stones[i][0]].Add(i);
                col[stones[i][1]].Add(i);
            }
            for (int i = 0; i < row.Length; i++) {
                if (row[i] is null) {
                    continue;
                }
                for (int j = 0; j < row[i].Count - 1; j++) {
                    union(row[i][j], row[i][j + 1]);
                }
            }
            for (int i = 0; i < col.Length; i++) {
                if (col[i] is null) {
                    continue;
                }
                for (int j = 0; j < col[i].Count - 1; j++) {
                    union(col[i][j], col[i][j + 1]);
                }
            }
            int res = 0;
            for (int i = 0; i < size.Length; i++) {
                if (fa[i] == i) {
                    res += size[i] - 1;
                }
            }
            return res;

            int find(int x) {
                if (fa[x] == x) {
                    return x;
                }
                return fa[x] = find(fa[x]);
            }

            void union(int a, int b) {
                int x = find(a);
                int y = find(b);
                if (x == y) {
                    return;
                }
                fa[x] = y;
                size[y] += size[x];
            }
        }

        public static int MinimumJumps(int[] forbidden, int a, int b, int x) {
            Queue<(int, bool, int)> que = new();
            // 位置，是否跳回，第几步
            que.Enqueue((0, false, 0));
            HashSet<int> forb = new(forbidden);
            HashSet<int> seen = new() { que.Peek().Item1 };
            while (que.Count > 0) {
                var tmp = que.Dequeue();
                if (a > b && tmp.Item1 - b > x
                    || a < b && tmp.Item1 > 6000
                    || forb.Contains(tmp.Item1)
                    ) {
                    continue;
                }
                if (tmp.Item1 == x) {
                    return tmp.Item3;
                }
                if (!tmp.Item2 && tmp.Item1 - b >= 0 && seen.Add(tmp.Item1 - b)) {
                    que.Enqueue((tmp.Item1 - b, true, tmp.Item3 + 1));
                }
                if (seen.Add(tmp.Item1 + a)) {
                    que.Enqueue((tmp.Item1 + a, false, tmp.Item3 + 1));
                }
            }
            return -1;
        }

        public static int MinTrioDegree(int n, int[][] edges) {
            int[] degree = new int[n + 1];
            for (int i = 0; i < edges.Length; i++) {
                degree[edges[i][0]]++;
                degree[edges[i][1]]++;
            }
            HashSet<int>[] nxt = new HashSet<int>[n + 1];
            for (int i = 0; i < nxt.Length; i++) {
                nxt[i] = new();
            }
            for (int i = 0; i < edges.Length; i++) {
                nxt[edges[i][0]].Add(edges[i][1]);
                nxt[edges[i][1]].Add(edges[i][0]);
            }
            int res = int.MaxValue;
            for (int i = 0; i < nxt.Length; i++) {
                // 验证是否组成了三元组
                foreach (var j in nxt[i]) {
                    foreach (var k in nxt[j]) {
                        if (nxt[k].Contains(i)) {
                            res = Math.Min(res,
                                degree[i] + degree[j] + degree[k] - 6
                                );
                        }
                    }
                }
            }
            return res == int.MaxValue ? -1 : res;
        }

        public static long WaysToBuyPensPencils(int total, int cost1, int cost2) {
            long res = 0;
            for (int i = total; i >= 0; i -= cost1) { // i是剩余买2的钱
                res += i / cost2 + 1;
            }
            return res;
        }

        public static long[] CountBlackBlocks(int m, int n, int[][] coordinates) {
            checked {
                Dictionary<(int, int), long> dict = new();
                int[][] dire =
                    new int[][] { new int[] { 0, 0 }, new int[] { -1, 0 }, new int[] { 0, -1 }, new int[] { -1, -1 } };
                foreach (var item in coordinates) {
                    for (int i = 0; i < dire.Length; i++) {
                        int[] cur = new int[] { item[0] + dire[i][0], item[1] + dire[i][1] };
                        if (check(cur)) {
                            dict.TryAdd((cur[0], cur[1]), 0);
                            dict[(cur[0], cur[1])]++;
                        }
                    }
                }
                var vals = dict.Values.ToArray();
                Array.Sort(vals);
                long[] res = new long[5];
                for (int i = 0; i < vals.Length; i++) {
                    res[vals[i]]++;
                }
                res[0] = (long)(m - 1) * (n - 1) - res.Sum();
                return res;
            }
            bool check(int[] pos) => pos[0] >= 0 && pos[1] >= 0 && pos[0] < m - 1 && pos[1] < n - 1;
        }

        public static bool CanTraverseAllPairs(int[] nums) {
            int max = nums.Max();
            List<int>[] factors = new List<int>[max + 1];
            factors[1] = new();
            for (int i = 2; i <= max; i++) {
                for (int j = i; j <= max; j += i) {
                    if (factors[j] is null) {
                        factors[j] = new();
                    }
                    factors[j].Add(i);
                }
            }

            List<int>[] bucket = new List<int>[max + 1];
            for (int i = 0; i < nums.Length; i++) {
                for (int j = 0; j < factors[nums[i]].Count; j++) {
                    if (bucket[factors[nums[i]][j]] is null) {
                        bucket[factors[nums[i]][j]] = new();
                    }
                    bucket[factors[nums[i]][j]].Add(i);
                }
            }
            int[] fa = new int[nums.Length];
            for (int i = 0; i < fa.Length; i++) { fa[i] = i; }
            for (int i = 0; i < bucket.Length; i++) {
                if (bucket[i] is null) {
                    continue;
                }
                for (int j = 0; j < bucket[i].Count - 1; j++) {
                    union(bucket[i][j], bucket[i][j + 1]);
                }
            }
            int cnt = 0;
            for (int i = 0; i < fa.Length; i++) {
                if (fa[i] == i) {
                    cnt++;
                }
            }
            return cnt == 1;

            void union(int a, int b) {
                if (a < b) {
                    union(b, a);
                    return;
                }
                int x = find(a);
                int y = find(b);

                if (x != y) {
                    fa[x] = y;
                }
            }

            int find(int x) {
                if (fa[x] == x) {
                    return x;
                }
                return fa[x] = find(fa[x]);
            }
        }

        public static int CaptureForts(int[] forts) {
            int last = -1;
            int res = 0;
            for (int k = 0; k < 2; k++) {
                for (int i = 0; i < forts.Length; i++) {
                    if (forts[i] == 1) {
                        last = i;
                    } else if (forts[i] == -1) {
                        if (last != -1) {
                            res = Math.Max(res, i - last - 1);
                        }
                        last = -1;
                    }
                }
                Array.Reverse(forts);
            }
            return res;

        }

        public static int MinRefuelStops(int target, int startFuel, int[][] stations) {
            int reach = -1;
            // 向前推
            PriorityQueue<int, int> que = new(new MaxHeapComparer<int>()); // 反悔堆
            int res = 0;
            int last = 0;
            que.Enqueue(startFuel, startFuel);

            while (que.Count > 0) {
                var tmp = que.Dequeue();
                if (tmp + last >= target) {
                    return res;
                }
                res++;
                while (reach + 1 < stations.Length && stations[reach + 1][0] <= tmp + last) {
                    reach++;
                    que.Enqueue(stations[reach][1], stations[reach][1]);
                }
                last += tmp;
            }
            return -1;
        }

        public static int ScheduleCourse(int[][] courses) { // WA
            PriorityQueue<int, int> que = new(); // 反悔堆
            // 先寻找一个不太正确的贪心，再进行反悔。
            Array.Sort(courses, (a, b) => a[1].CompareTo(b[1]));
            int t = 0;
            for (int i = 0; i < courses.Length; i++) {
                int cur = courses[i][0];
                if (t + cur > courses[i][1]) {
                    if (que.Count > 0 && courses[que.Peek()][0] > cur) {
                        var tmp = que.Dequeue();
                        t -= courses[tmp][0];
                        t += cur;
                        que.Enqueue(i, -courses[i][0]);
                    }
                } else {
                    t += cur;
                    que.Enqueue(i, -courses[i][0]);
                }
            }
            return que.Count;
        }

        public static int MagicTower(int[] nums) {
            int n = nums.Length;
            PriorityQueue<int, int> que = new(); // 反悔堆
            int res = 0;
            int sum = 0;
            int minus = 0;
            for (int i = 0; i < n; i++) {
                que.Enqueue(i, nums[i]);
                sum += nums[i];

                while (que.Count > 0 && sum < 0) {
                    var tmp = que.Dequeue();
                    if (nums[tmp] > 0) {
                        que.Enqueue(tmp, nums[tmp]);
                        break;
                    }
                    sum -= nums[tmp];
                    minus += nums[tmp];
                    res++;
                }
                if (sum < 0) { return -1; }
            }
            if (minus + sum < 0) { return -1; }
            return res;
        }

        public static int SmallestRangeII(int[] nums, int k) {
            Array.Sort(nums);
            int max = nums[^1] + k;
            int min = nums[0] + k;
            int res = max - min;
            for (int i = nums.Length - 1; i >= 1; i--) {
                //nums[i] -= k;
                min = Math.Min(min, nums[i] - k);
                max = Math.Max(nums[i - 1] + k, nums[^1] - k);
                res = Math.Min(res, max - min);
            }
            return res;
        }

        public static int PreimageSizeFZF(int k) {
            long l = 0, r = long.MaxValue - 1;
            long x1 = -1, x2 = -1;
            checked {
                while (l <= r) {
                    long m = ((r - l) >> 1) + l;
                    long tmp = f(m);
                    if (tmp > k) {
                        r = m - 1;
                    } else if (tmp < k) {
                        l = m + 1;
                    } else {
                        x1 = m;
                        r = m - 1;
                    }
                }
                if (x1 == -1) {
                    return 0;
                }
                l = 0;
                r = long.MaxValue;
                while (l <= r) {
                    long m = ((r - l) >> 1) + l;
                    long tmp = f(m);
                    if (tmp > k) {
                        r = m - 1;
                    } else if (tmp < k) {
                        l = m + 1;
                    } else {
                        x2 = m;
                        l = m + 1;
                    }
                }
                return (int)(x2 - x1 + 1);

                static long f(long x) {
                    long factor = 5;
                    long res = 0;
                    while (factor <= x) {
                        res += x / factor;
                        if (factor > x / 5) {
                            break;
                        }
                        factor *= 5;
                    }
                    return res;
                }
            }
        }

        public static string ShortestSuperstring(string[] words) {
            throw new NotImplementedException();

            //int f(int bit) {

            //}

            int getShort(int a, int b) {
                int res = Math.Min(words[a].Length, words[b].Length);
                for (int i = res; i > 0; i--) { // len
                    bool flag = true;
                    for (int j = 0; j < res; j++) {
                        if (words[a][words[a].Length - i + j] != words[b][j]) {
                            flag = false;
                            break;
                        }
                    }
                    if (flag) {
                        return i;
                    }
                }
                return 0;
            }
        }

        public static int MaximumSafenessFactor(IList<IList<int>> grid) {
            // TTTFFF 二分答案
            int n = grid.Count;
            List<(int x, int y)> ones = new();
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    if (grid[i][j] == 1) {
                        ones.Add((i, j));
                    }
                }
            }
            int[][] dire = [[1, 0], [-1, 0], [0, 1], [0, -1]];
            int l = -1, r = int.MaxValue / 10;
            while (l < r) {
                int m = l + ((r - l + 1) >> 1);
                if (check(m)) {
                    l = m;
                } else {
                    r = m - 1;
                }
            }
            return l;

            bool check(int dis) {
                if (dis == 0) {
                    return true;
                }
                if (grid[0][0] == 1 || grid[n - 1][n - 1] == 1) {
                    return false;
                }
                if (calc((0, 0)) < dis) {
                    return false;
                }
                bool[,] visited = new bool[n, n];
                Queue<(int x, int y)> que = new();
                que.Enqueue((0, 0));
                visited[0, 0] = true;
                while (que.Count > 0) {
                    var tmp = que.Dequeue();
                    if (tmp == (n - 1, n - 1)) {
                        return true;
                    }
                    for (int i = 0; i < dire.Length; i++) {
                        int x = dire[i][0] + tmp.x;
                        int y = dire[i][1] + tmp.y;
                        if (x >= 0 && x < n && y >= 0 && y < n && !visited[x, y]) {
                            visited[x, y] = true;
                            if (calc((x, y)) >= dis) { // dis是最小距离
                                que.Enqueue((x, y));
                            }
                        }
                    }
                }
                return false;
            }

            int calc((int x, int y) p) {
                int res = int.MaxValue;
                for (int i = 0; i < ones.Count; i++) {
                    res = Math.Min(res,
                        Math.Abs(ones[i].x - p.x) + Math.Abs(ones[i].y - p.y));
                }
                return res;
            }
        }

        public static int SmallestSubarrays(int[] arr) {
            // 修改了数组的定义，在x前的每一个元素j都是arr[j..x)的按位或值
            int n = arr.Length;
            HashSet<int> set = new() { arr[0] };
            for (int i = 1; i < n; i++) {
                int x = arr[i];
                set.Add(x);
                for (int j = i - 1; j >= 0; j--) {
                    if ((x | arr[j]) == arr[j]) {
                        break;
                    }
                    arr[j] |= x;
                    set.Add(arr[j]);
                }
            }
            return set.Count;
        }

        public static int DistMoney(int money, int children) {
            if (money < children) {
                return -1;
            }
            if (money <= 8) {
                return 0;
            }
            money -= children;//为每个儿童先分发一块钱
            if (money / 7.0 > children) {
                return children - 1;
            } else if (money / 7 * 7 == money && money / 7 == children) {
                return children;
            } else {
                if (money % 7 == 3) {
                    if (children - money / 7 < 2) {
                        return money / 7 - 1;
                    }
                }
                return money / 7;
            }
        }

        public static int BestRotation(int[] nums) {
            int n = nums.Length;
            int[] diff = new int[n + 1];
            for (int i = 0; i < n; i++) {
                // [nums[i], n-1]
                int from = (nums[i] - i + n) % n;
                int to = (n - 1 - i) % n;
                if (from < to) {
                    diff[from]++;
                    diff[to + 1]--;
                } else {
                    diff[from]++;
                    diff[^1]--;

                    diff[0]++;
                    diff[to + 1]--;
                }
            }
            int max = 0;
            int res = -1;
            int d = 0;
            for (int i = 0; i < diff.Length; i++) {
                d += diff[i];
                if (d > max) {
                    max = d;
                    res = (n - i) % n;
                } else if (d == max) {
                    res = Math.Min(res, (n - i) % n);
                }
            }
            return res;

        }

        public static int MinimizeMax(int[] nums, int p) {
            Array.Sort(nums);
            int min = int.MaxValue, max = int.MinValue;
            for (int i = 0; i < nums.Length; i++) {
                min = Math.Min(min, nums[i]);
                max = Math.Max(max, nums[i]);
            }
            // 0 0 1 1 1 1
            int l = 0, r = max - min;
            while (l < r) {
                int m = (l + r) >> 1;
                if (!check(m)) {
                    l = m + 1;
                } else {
                    r = m;
                }
            }
            return l;

            bool check(int gap) {
                int start = 0;
                int i = start + 1;
                int res = 0;
                while (i < nums.Length) {
                    while (i < nums.Length && Math.Abs(nums[i] - nums[i - 1]) <= gap) { i++; }
                    res += (i - start) / 2;
                    start = i++;
                }
                return res >= p;
            }
        }

        public static long MaximumTripletValue(int[] nums) {
            int a = nums[0];
            int[] max = new int[nums.Length];
            max[^1] = nums[^1];
            for (int i = nums.Length - 2; i >= 0; i--) {
                max[i] = Math.Max(nums[i], max[i + 1]);
            }
            long res = 0;
            for (int i = 1; i < nums.Length - 1; i++) {
                int sub = a - nums[i];
                a = Math.Max(a, nums[i]);
                res = Math.Max(res, (long)max[i + 1] * sub);
            }
            return res;
        }

        public static int LongestCycle(int[] edges) {
            HashSet<int> seen = new();
            int res = -1;
            for (int i = 0; i < edges.Length; i++) {
                find(i, 1, new());
            }
            return res;

            void find(int node, int n, Dictionary<int, int> vis) {
                if (seen.Contains(node)) {
                    bring(vis);
                    return;
                }
                if (vis.ContainsKey(node)) {
                    res = Math.Max(res, n - vis[node]);
                    bring(vis);
                    return;
                }
                vis.Add(node, n);
                if (edges[node] != -1) {
                    find(edges[node], n + 1, vis);
                } else {
                    bring(vis);
                }
            }

            void bring(Dictionary<int, int> vis) {
                foreach (var v in vis) {
                    seen.Add(v.Key);
                }
                //vis.Clear();
            }
        }

        public static int RegionsBySlashes(string[] grid) {
            int n = grid.Length;
            int[] fa = new int[n * n << 2];
            for (int i = 0; i < fa.Length; i++) { fa[i] = i; }
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    int bas = (i * n + j) * 4;
                    if (grid[i][j] == ' ') {
                        union(bas, bas + 1);
                        union(bas + 2, bas + 1);
                        union(bas + 3, bas + 1);
                    } else if (grid[i][j] == '/') {
                        union(bas, bas + 3);
                        union(bas + 1, bas + 2);
                    } else {
                        union(bas + 3, bas + 2);
                        union(bas + 1, bas);
                    }
                    if (i != 0) {
                        union(bas, bas - n * 4 + 2);
                    }
                    if (i != n - 1) {
                        union(bas + 2, bas + n * 4);
                    }
                    if (j != 0) {
                        union(bas + 3, bas - 4 + 1);
                    }
                    if (j != n - 1) {
                        union(bas + 1, bas + 4 + 3);
                    }
                }
            }
            int res = 0;
            for (int i = 0; i < fa.Length; i++) {
                if (fa[i] == i) {
                    res++;
                }
            }
            return res;

            int find(int x) {
                if (fa[x] == x) {
                    return x;
                }
                return fa[x] = find(fa[x]);
            }

            void union(int x, int y) {
                int a = find(x);
                int b = find(y);
                if (a == b) {
                    return;
                }
                fa[a] = b;
            }
        }

        public static int SumDistance(int[] nums, string s, int d) {
            checked {
                const int MOD = 1000000007;
                int n = nums.Length;
                long[] final = new long[n];
                for (int i = 0; i < n; i++) {
                    final[i] = (long)nums[i] + (s[i] == 'L' ? -d : d);
                }
                Array.Sort(final);
                long res = 0;
                for (int i = 0; i < n; i++) {
                    res = (res % MOD + MOD - (n - i - 1) * final[i]) % MOD;
                    res = (res % MOD + i * final[i] % MOD) % MOD;
                }
                return (int)res;
            }
        }


        private class MaxHeapComparer<T> : IComparer<T> where T : IComparable<T> {
            public int Compare(T x, T y) {
                return y.CompareTo(x);
            }
        }
    }
}
