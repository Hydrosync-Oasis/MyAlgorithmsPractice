using System.ComponentModel;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Algorithm {
    internal static partial class Exercises {
        public static int NetworkDelayTime(int[][] times, int n, int k) {
            bool[] locked = new bool[n + 1];
            PriorityQueue<int, int> pq = new();
            int[] dist = new int[n + 1];

            for (int i = 1; i <= n; i++) {
                dist[i] = int.MaxValue;
            }

            List<(int, int)>[] graph = new List<(int, int)>[n + 1];
            for (int i = 0; i < times.Length; i++) {
                if (graph[times[i][0]] is null) {
                    graph[times[i][0]] = new();
                }
                graph[times[i][0]].Add((times[i][1], times[i][2]));
            }
            dist[k] = 0;
            pq.Enqueue(k, 0);
            while (pq.Count > 0) {
                var tmp = pq.Dequeue();
                if (locked[tmp]) { // 延迟删除
                    continue;
                }
                locked[tmp] = true;
                if (graph[tmp] is null) {
                    continue;
                }
                foreach (var item in graph[tmp]) {
                    if (!locked[item.Item1]) {
                        dist[item.Item1] = Math.Min(dist[item.Item1], dist[tmp] + item.Item2);
                        pq.Enqueue(item.Item1, dist[tmp] + item.Item2);
                    }
                }
            }
            int res = 0;
            for (int i = 0; i <= n; i++) {
                if (dist[i] == int.MaxValue) {
                    return -1;
                }
                res = Math.Max(res, dist[i]);
            }

            return res;
        }

        public static long MaxKelements(int[] nums, int k) {
            PriorityQueue<int, int> pq = new();
            int n = nums.Length;
            for (int i = 0; i < n; i++) {
                pq.Enqueue(i, -nums[i]);
            }
            int cnt = 0;
            long res = 0;
            while (cnt < k) {
                int tmp = pq.Dequeue();
                res += nums[tmp];
                nums[tmp] = (nums[tmp] + 2) / 3;
                pq.Enqueue(tmp, -nums[tmp]);
                cnt++;
            }
            return res;
        }

        public static int NetworkDelayTime2(int[][] times, int n, int k) {
            int[] w = new int[times.Length + 1];    // 每个边的权重
            int[] nxt = new int[times.Length + 1];  // 边的下一个边
            int[] to = new int[times.Length + 1];   // 边指向的点
            int[] a = new int[n + 1];               // 点指向的第一个边
            for (int i = 0; i < times.Length; i++) {
                w[i + 1] = times[i][2];
                nxt[i + 1] = a[times[i][0]];
                a[times[i][0]] = i + 1;
                to[i + 1] = times[i][1];
            }

            int[] dis = new int[n + 1];
            bool[] visited = new bool[n + 1];
            for (int i = 0; i < dis.Length; i++) {
                dis[i] = int.MaxValue;
            }
            dis[k] = 0;
            visited[k] = true;
            PriorityQueue<int, int> pq = new();
            pq.Enqueue(k, 0);
            while (pq.Count > 0) {
                var tmp = pq.Dequeue();
                visited[tmp] = true;
                int x = dis[tmp];
                for (int i = a[tmp]; i != 0; i = nxt[i]) {
                    if (x + w[i] < dis[to[i]]) {
                        dis[to[i]] = x + w[i];
                        pq.Enqueue(to[i], dis[to[i]]);
                    }
                }
            }
            int res = 0;
            for (int i = 1; i < dis.Length; i++) {
                if (dis[i] == int.MaxValue) {
                    return -1;
                } else {
                    res = Math.Max(res, dis[i]);
                }
            }
            return res;
        }

        public static int TupleSameProduct(int[] nums) {
            Dictionary<int, int> dict = new();
            for (int i = 0; i < nums.Length; i++) {
                for (int j = i + 1; j < nums.Length; j++) {
                    if (!dict.ContainsKey(nums[i] * nums[j])) {
                        dict[nums[i] * nums[j]] = 0;
                    }
                    dict[nums[i] * nums[j]]++;
                }
            }
            int res = 0;
            foreach (var item in dict.Values) {
                res += (item - 1) * item;
            }
            return res * 4;
        }

        public static int MaxSatisfaction(int[] satisfaction) {
            Array.Sort(satisfaction);
            int n = satisfaction.Length;
            int res = 0;
            for (int i = 1; i <= n; i++) {
                int sum = 0;
                for (int j = n - i, cnt = 1; j < n; j++, cnt++) {
                    sum += satisfaction[j] * cnt;
                }
                res = Math.Max(res, sum);
            }
            return res;
        }

        public static int SwimInWater(int[][] grid) {
            PriorityQueue<(int, int), int> pq = new();
            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            int n = grid.Length;
            int[,] dis = new int[n, n];
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    dis[i, j] = int.MaxValue;
                }
            }

            pq.Enqueue((0, 0), grid[0][0]);
            dis[0, 0] = 0;
            int[][] dire =
                new int[][] { new int[] { 1, 0 }, new int[] { 0, 1 }, new int[] { -1, 0 }, new int[] { 0, -1 } };
            while (pq.Count > 0) {
                var p = pq.Dequeue();
                if (!visited.Add(p)) {
                    continue;
                }
                for (int i = 0; i < dire.Length; i++) {
                    (int, int) cur = (dire[i][0] + p.Item1, dire[i][1] + p.Item2);
                    if (cur.Item1 >= 0 && cur.Item1 < n && cur.Item2 >= 0 && cur.Item2 < n) {
                        if (!visited.Contains(cur)) {
                            dis[cur.Item1, cur.Item2] =
                                Math.Min(dis[cur.Item1, cur.Item2],
                                Math.Max(grid[cur.Item1][cur.Item2], dis[p.Item1, p.Item2]));

                            pq.Enqueue(cur, dis[cur.Item1, cur.Item2]);
                        }
                    }
                }
            }

            return dis[n - 1, n - 1];
        }

        public static int MaxEqualFreq(int[] nums) {
            Dictionary<int, int> dict = new();
            Dictionary<int, int> d2 = new();
            int res = 1;
            for (int i = 0; i < nums.Length; i++) {
                if (!dict.ContainsKey(nums[i])) {
                    dict.Add(nums[i], 0);
                }
                add(dict[nums[i]]);
                dict[nums[i]]++;
                if (check()) {
                    res = Math.Max(res, i + 1);
                }
            }
            return res;


            void add(int key) {
                if (!d2.ContainsKey(key)) {
                    if (!d2.ContainsKey(key + 1)) {
                        d2[key + 1] = 0;
                    }
                    d2[key + 1]++;
                } else {
                    d2[key]--;
                    if (d2[key] == 0) {
                        d2.Remove(key);
                    }

                    if (!d2.ContainsKey(key + 1)) {
                        d2[key + 1] = 0;
                    }
                    d2[key + 1]++;
                }
            }

            bool check() {
                if (d2.Count > 2 || d2.Count == 0) {
                    return false;
                }
                int max = -1, min = nums.Length;
                if (d2.Count == 2) {
                    foreach (var item in d2) {
                        max = Math.Max(max, item.Key);
                        min = Math.Min(min, item.Key);
                    }
                    if (min == 1 && d2[min] == 1) {
                        return true;
                    }
                    if (max - min == 1 && d2[max] == 1) {
                        return true;
                    }
                } else {
                    foreach (var item in d2) {
                        if (item.Key == 1 || item.Value == 1) {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public static int HIndex(int[] citations) {
            Array.Sort(citations);

            int l = 0, r = citations.Length;
            // T T T F F
            while (l < r) {
                int m = (l + r + 1) >> 1;
                if (found(m) >= m) {
                    l = m;
                } else {
                    r = m - 1;
                }
            }
            return l;

            int found(int val) {
                int l = 0, r = citations.Length;
                // 大于等于num的第一个数
                while (l < r) {
                    int m = (l + r) >> 1;
                    if (citations[m] >= val) {
                        r = m;
                    } else {
                        l = m + 1;
                    }
                }
                return citations.Length - l;
            }
        }

        public static int MinimumDistance(string word) {
            int n = word.Length;
            int[,,] dp = new int[26, 26, n];
            // L, R, N
            for (int i = 0; i < 26; i++) {
                for (int j = 0; j < 26; j++) {
                    for (int k = 0; k < n; k++) {
                        dp[i, j, k] = int.MaxValue;
                    }
                }
            }

            for (int i = 0; i < 26; i++) {
                dp[word[0] - 'A', i, 0] = 0;
            }
            for (int i = 0; i < 26; i++) {
                dp[i, word[0] - 'A', 0] = 0;
            }

            for (int i = 1; i < n; i++) {
                int pre = word[i - 1] - 'A';
                int cur = word[i] - 'A';
                for (int j = 0; j < 26; j++) {
                    if (dp[j, pre, i - 1] != int.MaxValue) {
                        dp[cur, pre, i] = Math.Min(dp[cur, pre, i],
                            dp[j, pre, i - 1] + dist(j, cur));
                    }
                    if (dp[pre, j, i - 1] != int.MaxValue) {
                        dp[pre, cur, i] = Math.Min(dp[pre, cur, i],
                            dp[pre, j, i - 1] + dist(j, cur));
                    }
                }
                for (int j = 0; j < 26; j++) {
                    if (dp[pre, j, i - 1] != int.MaxValue) {
                        dp[cur, j, i] = Math.Min(dp[cur, j, i],
                            dp[pre, j, i - 1] + dist(pre, cur));
                    }
                    if (dp[j, pre, i - 1] != int.MaxValue) {
                        dp[j, cur, i] = Math.Min(dp[j, cur, i],
                            dp[j, pre, i - 1] + dist(pre, cur));
                    }
                }
            }
            int res = int.MaxValue;
            for (int i = 0; i < 26; i++) {
                for (int j = 0; j < 26; j++) {
                    res = Math.Min(res, dp[i, j, n - 1]);
                }
            }
            return res;


            int dist(int a, int b) {
                int ai = a / 6;
                int aj = a % 6;
                int bi = b / 6;
                int bj = b % 6;
                return Math.Abs(ai - bi) + Math.Abs(aj - bj);
            }
        }

        public static int FindMaximumXOR(int[] nums) {
            int max = 0;
            for (int i = 0; i < nums.Length; i++) {
                max = Math.Max(max, bitCnt(nums[i]));
            }

            int curAns = 0;
            int mask = 0;
            for (int i = max - 1; i >= 0; i--) {
                HashSet<int> seen = new();
                mask |= 1 << i; // 逐步递推，
                int tryyy = curAns | (1 << i); // 能不能实现这一位是一
                bool flag = false;
                for (int j = 0; j < nums.Length; j++) {
                    if (seen.Contains(tryyy ^ (nums[j] & mask))) {
                        flag = true;
                        break;
                    }
                    seen.Add(nums[j] & mask);
                }
                if (flag) {
                    curAns = tryyy;
                }
            }
            return curAns;

            static int bitCnt(int num) {
                int cnt = 0;
                while (num > 0) {
                    cnt++;
                    num >>= 1;
                }
                return cnt;
            }
        }

        public static int MaxProduct(string[] words) {
            int n = words.Length;
            HashSet<int>[] albt = new HashSet<int>[26];// 存放下标
            for (int i = 0; i < albt.Length; i++) {
                albt[i] = new();
            }
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < words[i].Length; j++) {
                    albt[words[i][j] - 'a'].Add(i);
                }
            }

            bool[,] can = new bool[n, n];
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    can[i, j] = true;
                }
            }
            for (int i = 0; i < albt.Length; i++) {
                foreach (var j in albt[i]) {
                    foreach (var k in albt[i]) {
                        can[j, k] = false;
                    }
                }
            }
            int res = 0;
            for (int i = 0; i < n; i++) {
                for (int j = i + 1; j < n; j++) {
                    if (can[i, j]) {
                        res = Math.Max(res, words[i].Length * words[j].Length);
                    }
                }
            }
            return res;
        }

        public static int MaxDotProduct(int[] nums1, int[] nums2) {
            int n = nums1.Length;
            int m = nums2.Length;
            int[,] dp = new int[n, m]; // 包含i和j的乘积的最大点积
            int[,] dp2 = new int[n, m];// 记录max
            dp[0, 0] = dp2[0, 0] = nums1[0] * nums2[0];
            for (int i = 1; i < n; i++) {
                dp[i, 0] = nums1[i] * nums2[0];
                dp2[i, 0] = Math.Max(dp2[i - 1, 0], dp[i, 0]);
            }
            for (int i = 1; i < m; i++) {
                dp[0, i] = nums1[0] * nums2[i];
                dp2[0, i] = Math.Max(dp2[0, i - 1], dp[0, i]);
            }
            for (int i = 1; i < n; i++) {
                for (int j = 1; j < m; j++) {
                    dp[i, j] = nums1[i] * nums2[j] + Math.Max(0, dp2[i - 1, j - 1]);
                    dp2[i, j] = Math.Max(dp2[i - 1, j], dp2[i, j - 1]);
                    dp2[i, j] = Math.Max(dp2[i, j], dp[i, j]);
                }
            }

            Print2DArray(dp);

            Print2DArray(dp2);
            return dp2[n - 1, m - 1];
        }

        static void Print2DArray(int[,] array) {
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);

            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    Console.Write(array[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("---");
        }

        public static string DecodeAtIndex(string s, int k) {
            long curLen = 0;
            long lastLen = 0;
            // 后面没数字的用1等效
            if (char.IsLetter(s[^1])) {
                s += '1';
            }
            for (int i = 0; i < s.Length; i++) {
                if (char.IsDigit(s[i])) {
                    lastLen = (lastLen + curLen) * (s[i] - '0');
                    curLen = 0;
                } else {
                    curLen++;
                }
            }
            if (curLen != 0) {
                throw new Exception();
            }
            s = '1' + s;
            long part = 0;
            long find = k - 1;
            for (int i = s.Length - 1; i >= 0; i--) {
                if (char.IsDigit(s[i])) {
                    long tmp = lastLen;
                    lastLen -= curLen;
                    long multiplied = lastLen;
                    lastLen /= (s[i] - '0');
                    if (find >= tmp - curLen) {
                        return s[(int)(find - multiplied + i + 1)].ToString();
                        // 前面加了个虚拟数，所以下标要加一
                    }
                    part = lastLen;
                    find %= part;
                    curLen = 0;

                } else {
                    curLen++;
                }
            }

            return "";
        }

        public static int MaximumMinutes(int[][] grid) {
            Queue<(int, int)> que = new();
            int n = grid.Length;
            int m = grid[0].Length;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    if (grid[i][j] == 1) {
                        que.Enqueue((i, j));
                    }
                    if (grid[i][j] == 2) {
                        grid[i][j] = -1;
                    }
                }
            }
            int[][] dire = new int[4][] { new int[] { 1, 0 }, new int[] { -1, 0 },
                new int[] { 0, 1 }, new int[] { 0, -1 } };

            while (que.Count > 0) {
                var tmp = que.Dequeue();
                int x = tmp.Item1, y = tmp.Item2;
                for (int i = 0; i < dire.Length; i++) {
                    int curX = dire[i][0] + x;
                    int curY = dire[i][1] + y;
                    if (curX >= 0 && curX < n && curY >= 0 && curY < m) {
                        if (grid[curX][curY] != 0) {
                            continue;
                        }
                        grid[curX][curY] = grid[x][y] + 1;
                        que.Enqueue((curX, curY));
                    }
                }
            }

            grid[0][0] = -1;

            int l = -1, r = 1000000000;
            while (l < r) {
                int mid = (l + r + 1) >> 1;
                if (check(mid)) {
                    l = mid;
                } else {
                    r = mid - 1;
                }
            }
            return l;

            bool check(int stay) {
                Queue<(int, int)> que = new();
                int[,] copy = new int[n, m];
                for (int i = 0; i < n; i++) {
                    for (int j = 0; j < m; j++) {
                        copy[i, j] = grid[i][j];
                        if (copy[i, j] == 0) {
                            copy[i, j] = int.MaxValue;
                        }
                    }
                }
                copy[0, 0] = stay + 1;
                // copy[n - 1, m - 1] = int.MaxValue;
                que.Enqueue((0, 0));
                while (que.Count > 0) {
                    var tmp = que.Dequeue();
                    int x = tmp.Item1, y = tmp.Item2;
                    if (tmp == ((n - 1, m - 1))) {
                        return true;
                    }
                    // use dire
                    for (int i = 0; i < dire.Length; i++) {
                        int curX = dire[i][0] + x;
                        int curY = dire[i][1] + y;
                        if (curX >= 0 && curX < n && curY >= 0 && curY < m) {
                            if (copy[curX, curY] == -1) {
                                continue;
                            }
                            if (copy[curX, curY] > copy[x, y] + 1 || // 题目允许火和人同时到达安全屋
                                copy[curX, curY] == copy[x, y] + 1 && (curX, curY) == (n - 1, m - 1)) {

                                copy[curX, curY] = copy[x, y] + 1;
                                que.Enqueue((curX, curY));
                            }
                        }
                    }
                }
                return false;
            }
        }

        public static int[] SuccessfulPairs(int[] spells, int[] potions, long success) {
            Array.Sort(potions);
            int[] res = new int[spells.Length];
            for (int i = 0; i < res.Length; i++) {
                res[i] = find(spells[i]);
            }
            return res;

            int find(int times) {
                int l = 0, r = potions.Length;
                while (l < r) {
                    int m = (l + r) >> 1;
                    if ((long)potions[m] * times >= success) {
                        r = m;
                    } else {
                        l = m + 1;
                    }
                }
                return potions.Length - l;
            }
        }

        public static int[] SuccessfulPairs2(int[] spells, int[] potions, long success) {
            int max = -1;
            for (int i = 0; i < spells.Length; i++) {
                max = Math.Max(max, spells[i]);
            }
            int[] bucket = new int[max + 2];
            for (int i = 0; i < potions.Length; i++) {
                potions[i] = (int)Math.Ceiling(success * 1.0 / potions[i]);
                if (potions[i] < 0) {
                    // 溢出
                    potions[i] = int.MaxValue;
                }
                bucket[Math.Min(max + 1, potions[i])]++;
            }
            for (int i = 1; i < bucket.Length; i++) {
                bucket[i] += bucket[i - 1];
            }
            int[] res = new int[spells.Length];
            for (int i = 0; i < res.Length; i++) {
                res[i] = bucket[spells[i]];
            }
            return res;
        }

        public static int MinOperations2(int[] nums1, int[] nums2) {
            const int ERROR = 1010;
            int n = nums1.Length;
            int case1 = calc();
            (nums1[^1], nums2[^1]) = (nums2[^1], nums1[^1]);
            int case2 = 1 + calc();
            int res = Math.Min(case1, case2);
            if (res == ERROR) {
                return -1;
            }
            return res;

            int calc() {
                int[] c1 = new int[n];
                int[] c2 = new int[n];
                Array.Copy(nums1, c1, n);
                Array.Copy(nums2, c2, n);
                int res = 0;
                int m1 = c1[^1];
                int m2 = c2[^1];
                for (int i = 0; i < n - 1; i++) {
                    if (c1[i] > m1) {
                        (c1[i], c2[i]) = (c2[i], c1[i]);
                        res++;
                    }
                }
                for (int i = 0; i < n - 1; i++) {
                    if (c2[i] > m2) {
                        (c1[i], c2[i]) = (c2[i], c1[i]);
                        res++;
                    }

                }
                for (int i = 0; i < n - 1; i++) {
                    if (c1[i] > c1[^1]) {
                        return ERROR;
                    }
                    if (c2[i] > c2[^1]) {
                        return ERROR;
                    }
                }
                return res;
            }
        }

        public static int FindTheCity(int n, int[][] edges, int distanceThreshold) {
            int[,] dis = new int[n, n];
            const int CANT = int.MaxValue;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    dis[i, j] = CANT;
                }
            }
            for (int i = 0; i < edges.Length; i++) {
                dis[edges[i][0], edges[i][1]] = edges[i][2];
                dis[edges[i][1], edges[i][0]] = edges[i][2];
                dis[edges[i][0], edges[i][0]] = 0;
                dis[edges[i][1], edges[i][1]] = 0;
            }
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    for (int k = 0; k < n; k++) {
                        if (dis[j, i] == CANT || dis[i, k] == CANT) {
                            continue;
                        }
                        if (dis[j, i] + dis[i, k] < dis[j, k]) {
                            dis[j, k] = dis[j, i] + dis[i, k];
                        }
                    }
                }
            }
            int min = int.MaxValue;
            int res = 0;
            for (int i = 0; i < n; i++) {
                int cnt = 0;
                for (int j = 0; j < n; j++) {
                    if (dis[i, j] <= distanceThreshold) {
                        cnt++;
                    }
                }
                cnt--;
                if (cnt <= min) {
                    min = cnt;
                    res = i;
                }
            }
            return res;
        }

        public static bool IsPossible(int[] target) {
            int sum = target.Sum();
            PriorityQueue<int, int> pq = new(new MaxHeapComparer<int>());
            for (int i = 0; i < target.Length; i++) {
                pq.Enqueue(i, target[i]);
            }
            while (target[pq.Peek()] != 1) {
                var tmp = pq.Dequeue();
                int x = target[tmp];
                if (x > sum - x) {
                    int a = target[tmp];
                    target[tmp] %= sum - x;
                    if (target[tmp] == 0) {
                        target[tmp] = sum - x;
                    }
                    sum -= a - target[tmp];
                    pq.Enqueue(tmp, target[tmp]);
                } else {
                    return false;
                }
            }
            return true;
        }

        public static TreeNode BstToGst2(TreeNode root) {
            f(root, 0);
            return root;

            int f(TreeNode cur, int rightSum) {
                if (cur is null) {
                    return 0;
                }
                int sum = rightSum;
                int ret = 0;
                sum += f(cur.right, rightSum);
                ret += sum - rightSum;
                ret += cur.val;
                cur.val += sum;
                ret += f(cur.left, cur.val);
                return ret;
            }
        }

        public static long MinimumFuelCost(int[][] roads, int seats) {
            int n = roads.Length + 1;
            HashSet<int>[] graph = new HashSet<int>[n];
            for (int i = 0; i < graph.Length; i++) {
                graph[i] = new();
            }

            for (int i = 0; i < roads.Length; i++) {
                graph[roads[i][0]].Add(roads[i][1]);
                graph[roads[i][1]].Add(roads[i][0]);
            }

            long[] cnt = new long[n]; // 以i为根的子树的节点数
            f(0, 0);
            long res = 0;
            for (int i = 1; i < n; i++) {
                res += calc(i);
            }
            return res;

            long calc(int node) { // 计算车的数量
                return (cnt[node] + seats - 1) / seats;
            }

            long f(int node, int pa) {
                long oth = 0;
                if (cnt[node] != 0)
                    return cnt[node];
                foreach (var item in graph[node]) {
                    if (item != pa) {
                        oth += f(item, node);
                    }
                }
                oth++;
                return cnt[node] = oth;
            }
        }

        public static int NextBeautifulNumber(int n) {
            List<int> res = new();
            StringBuilder sb = new();
            int[] bucket = new int[10];

            for (int i = 1; i <= 10000000; i++) {
                Array.Fill(bucket, 0);
                int tmp = i;
                while (tmp != 0) {
                    bucket[tmp % 10]++;
                    tmp /= 10;
                }
                bool flag = true;
                for (int j = 0; j < 10; j++) {
                    if (bucket[j] != 0 && bucket[j] != j) {
                        flag = false;
                    }
                    if (bucket[j] != 0) {
                        tmp += j;
                    }
                }
                if (flag) {
                    res.Add(i);
                }
            }
            for (int i = 0; i < res.Count; i++) {
                sb.Append(res[i]);
                sb.Append(',');
            }
            File.WriteAllText(@"C:\Users\Silver Wind\Desktop\1.txt", sb.ToString());
            int l = 0, r = res.Count;
            // F F T T T T
            while (l < r) {
                int m = (l + r) >> 1;
                if (res[m] > n) {
                    r = m;
                } else {
                    l = m + 1;
                }
            }

            return res[l];
        }

        public static int MaxNumEdgesToRemove(int n, int[][] edges) {
            int cnt1 = n, cnt2 = n;
            int[] pa1 = new int[n + 1];
            int[] pa2 = new int[n + 1];
            for (int i = 0; i < n + 1; i++) {
                pa1[i] = i;
                pa2[i] = i;
            }

            int res = 0;
            for (int i = 0; i < edges.Length; i++) {
                var x = edges[i];
                if (x[0] == 3) {
                    if (!union(pa1, x[1], x[2], ref cnt1) & !union(pa2, x[1], x[2], ref cnt2)) {
                        res++;
                    }
                }
            }
            for (int i = 0; i < edges.Length; i++) {
                var x = edges[i];
                if (x[0] == 1) {
                    if (!union(pa1, x[1], x[2], ref cnt1)) {
                        res++;
                    }
                } else if (x[0] == 2) {
                    if (!union(pa2, x[1], x[2], ref cnt2)) {
                        res++;
                    }
                }
            }

            return cnt1 > 1 || cnt2 > 1 ? -1 : res;

            int find(int[] pa, int x) {
                if (pa[x] == x) {
                    return x;
                }
                return pa[x] = find(pa, pa[x]);
            }

            bool union(int[] pa, int x, int y, ref int cnt) {
                int a = find(pa, x);
                int b = find(pa, y);
                if (a != b) {
                    pa[a] = b;
                    cnt--;
                }
                return a != b; // 返回是否真正合并了
            }
        }

        public static int[] SecondGreaterElement(int[] nums) {
            int n = nums.Length;
            int[] res = new int[n];
            Stack<int> st1 = new();
            Stack<int> st2 = new();
            Array.Fill(res, -1);
            for (int i = 0; i < n; i++) {
                List<int> tmp = new();
                while (st2.Count > 0 && nums[st2.Peek()] < nums[i]) {
                    res[st2.Pop()] = nums[i];
                }
                while (st1.Count > 0 && nums[st1.Peek()] < nums[i]) {
                    tmp.Add(st1.Pop());
                }
                st1.Push(i);
                tmp.Reverse();
                foreach (var item in tmp) {
                    st2.Push(item);
                }
            }

            return res;
        }

        public static TreeNode ReverseOddLevels(TreeNode root) {
            Queue<(TreeNode, int)> que = new();
            que.Enqueue((root, 0));
            while (que.Count > 0) {
                var tmp = que.Peek();
                List<TreeNode> pa = new();
                List<int> ls = new();
                if (tmp.Item1.left is null) {
                    que.Dequeue();
                }

                if (tmp.Item2 % 2 == 0) {
                    while (que.Count > 0 && que.Peek().Item1.left is not null) {
                        pa.Add(que.Dequeue().Item1);
                        ls.Add(pa[^1].left.val);
                        ls.Add(pa[^1].right.val);
                    }
                    ls.Reverse();
                    for (int i = 0; i < pa.Count; i++) {
                        pa[i].left.val = ls[2 * i];
                        pa[i].right.val = ls[2 * i + 1];
                    }
                    foreach (var item in pa) {
                        que.Enqueue((item.left, tmp.Item2 + 1));
                        que.Enqueue((item.right, tmp.Item2 + 1));
                    }
                } else {
                    tmp = que.Dequeue();
                    if (tmp.Item1.left is not null) {
                        que.Enqueue((tmp.Item1.left, tmp.Item2 + 1));
                        que.Enqueue((tmp.Item1.right, tmp.Item2 + 1));
                    }
                }
            }
            return root;
        }

        public static string MakeSmallestPalindrome(string s) {
            char[] res = s.ToCharArray();
            int n = s.Length;
            for (int i = 0; i < n / 2; i++) {
                res[i] = res[^(i + 1)] = (char)Math.Min(s[i], s[^(i + 1)]);
            }
            return string.Concat(res);
        }

        public static int MinimumEffortPath(int[][] heights) {
            PriorityQueue<(int, int), int> pq = new();
            pq.Enqueue((0, 0), 0);
            int n = heights.Length;
            int m = heights[0].Length;
            int[,] dis = new int[n, m];
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    dis[i, j] = int.MaxValue;
                }
            }

            dis[0, 0] = 0;
            bool[,] vis = new bool[n, m];
            int[][] dire = new int[4][] { new int[] { 1, 0 }, new int[] { -1, 0 },
                           new int[] { 0, 1 }, new int[] { 0, -1 } };
            while (pq.Count > 0) {
                var tmp = pq.Dequeue();
                int x = tmp.Item1;
                int y = tmp.Item2;
                if (vis[x, y]) {
                    continue;
                }

                vis[x, y] = true;
                // 1111
                for (int i = 0; i < dire.Length; i++) {
                    int curX = dire[i][0] + x;
                    int curY = dire[i][1] + y;
                    if (curX >= 0 && curY >= 0 && curX < n && curY < m) {
                        pq.Enqueue((curX, curY),
                            Math.Max(dis[x, y], Math.Abs(heights[curX][curY] - heights[x][y])));
                        dis[curX, curY] = Math.Min(dis[curX, curY],
                            Math.Max(dis[x, y], Math.Abs(heights[curX][curY] - heights[x][y])));
                    }
                }
            }
            return dis[n - 1, m - 1];
        }

        public static int MinDeletionSize(string[] strs) {
            List<(int l, int r)>[] bucket = new List<(int l, int r)>[strs[0].Length];
            int lastLV = -1;
            int res = 0;
            f(0);
            return res;

            void f(int lv) {
                if (lv == strs[0].Length) {
                    return;
                }

                List<(int l, int r)> list = new();
                bool flag = true;
                List<(int l, int r)> cur;
                cur = new() { (0, strs.Length - 1) };
                if (lastLV != -1) {
                    cur = bucket[lastLV];
                }
                for (int i = 0; cur != null && i < cur.Count && flag; i++) {
                    int l = cur[i].l;
                    int r = cur[i].r;
                    for (int j = l; j < r; j++) {
                        if (strs[j][lv] > strs[j + 1][lv]) {
                            flag = false;
                            res++;
                            break;
                        }
                    }
                    if (flag) {
                        // 分组循环
                        int cur1 = l, cur2 = l;
                        while (cur2 <= r) {
                            while (cur2 <= r && strs[cur2][lv] == strs[cur1][lv]) {
                                cur2++;
                            }
                            if (cur1 != cur2 - 1)
                                list.Add((cur1, cur2 - 1));
                            cur1 = cur2;
                        }
                        bucket[lv] = list;
                    }

                }

                if (flag)
                    lastLV = lv;
                f(lv + 1);
            }
        }

        public static long MaximumSumOfHeights(IList<int> maxHeights) {
            Stack<int> st = new();
            int n = maxHeights.Count;
            long[] right = new long[n];
            Array.Fill(right, n);
            for (int i = 0; i < n; i++) {
                while (st.Count > 0 && maxHeights[st.Peek()] > maxHeights[i]) {
                    right[st.Pop()] = i;
                }
                st.Push(i);
            }

            st.Clear();
            long[] left = new long[n];
            Array.Fill(left, -1);
            for (int i = n - 1; i >= 0; i--) {
                while (st.Count > 0 && maxHeights[st.Peek()] > maxHeights[i]) {
                    left[st.Pop()] = i;
                }
                st.Push(i);
            }

            long[] dp1 = new long[n];
            long[] dp2 = new long[n];
            dp1[0] = maxHeights[0];
            for (int i = 1; i < n; i++) {
                if (maxHeights[i] > maxHeights[i - 1]) {
                    dp1[i] = dp1[i - 1] + maxHeights[i];
                } else {
                    dp1[i] += (i - left[i]) * maxHeights[i];
                    dp1[i] += left[i] == -1 ? 0 : dp1[left[i]];
                }
            }
            dp2[^1] = maxHeights[^1];
            for (int i = n - 2; i >= 0; i--) {
                if (maxHeights[i] > maxHeights[i + 1]) {
                    dp2[i] = maxHeights[i] + dp2[i + 1];
                } else {
                    dp2[i] += maxHeights[i] * (right[i] - i);
                    dp2[i] += right[i] == n ? 0 : dp2[right[i]];
                }
            }
            long res = 0;
            for (int i = 0; i < n; i++) {
                res = Math.Max(res, dp1[i] + dp2[i] - maxHeights[i]);
            }
            return res;

        }

        public static bool IsEscapePossible(int[][] blocked, int[] source, int[] target) {
            int len = blocked.Length;
            int[] x = new int[len + 2];
            int[] y = new int[len + 2];
            for (int i = 0; i < len; i++) {
                x[i] = blocked[i][0];
                y[i] = blocked[i][1];
            }
            // 存放出发点目标点
            x[^1] = source[0];
            x[^2] = target[0];
            y[^1] = source[1];
            y[^2] = target[1];

            Array.Sort(x);
            Array.Sort(y);
            int m, n;// grid size
            int cnt = 0; // 计数器
            Dictionary<int, int> dict = new() {
                [x[0]] = cnt++,
            };
            int last = x[0];
            for (int i = 1; i < len + 2; i++) {
                if (x[i] - last == 1) {
                    dict[x[i]] = cnt++;
                    last = x[i];
                } else if (x[i] == last) {
                    // nothing
                } else {
                    cnt++;
                    dict[x[i]] = cnt++;
                    last = x[i];
                }
            }
            int sx = dict[source[0]];
            int tx = dict[target[0]];
            int max = 0; // 检查是否含有1e6-1
            int xMin = int.MaxValue;
            for (int i = 0; i < blocked.Length; i++) {
                max = Math.Max(max, blocked[i][0]);
                xMin = Math.Min(xMin, blocked[i][0]);
                blocked[i][0] = dict[blocked[i][0]];
            }
            m = max == 999999 ? cnt : cnt + 1;

            cnt = 0;
            dict[y[0]] = cnt++;
            last = y[0];
            for (int i = 1; i < len + 2; i++) {
                if (y[i] - last == 1) {
                    dict[y[i]] = cnt++;
                    last = y[i];
                } else if (y[i] == last) {
                    // nothing        
                } else {
                    cnt++;
                    dict[y[i]] = cnt++;
                    last = y[i];
                }
            }

            int sy = dict[source[1]];
            int ty = dict[target[1]];
            max = 0;
            int yMin = int.MaxValue;
            for (int i = 0; i < len; i++) {
                max = Math.Max(max, blocked[i][1]);
                yMin = Math.Min(xMin, blocked[i][1]);
                blocked[i][1] = dict[blocked[i][1]];
            }
            n = max == 999999 ? cnt : cnt + 1;

            HashSet<(int, int)> set = new();
            for (int i = 0; i < blocked.Length; i++) {
                set.Add((blocked[i][0], blocked[i][1]));
            }
            bool[,] vis = new bool[m + 2, n + 2];
            Queue<(int x, int y)> que = new();
            int[][] dire = new int[][] { new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 } };
            que.Enqueue((sx, sy));
            while (que.Count > 0) {
                var tmp = que.Dequeue();
                if (tmp == (tx, ty)) {
                    return true;
                }
                for (int i = 0; i < dire.Length; i++) {
                    int curX = dire[i][0] + tmp.x;
                    int curY = dire[i][1] + tmp.y;
                    if (curX >= (xMin > 0 ? -1 : 0) && curX < m && curY >= (yMin > 0 ? -1 : 0)
                        && curY < n && !vis[curX + 1, curY + 1] && !set.Contains((curX, curY))
                        ) {
                        que.Enqueue((curX, curY));
                        vis[curX + 1, curY + 1] = true;
                    }
                }
            }

            return false;
        }

        public static int MinStoneSum(int[] piles, int k) {
            PriorityQueue<int, int> pq = new();
            for (int i = 0; i < piles.Length; i++) {
                pq.Enqueue(i, -piles[i]);
            }

            int cnt = 0;
            int sum = piles.Sum();
            while (cnt < k) {
                int tmp = pq.Dequeue();
                if ((piles[tmp] >> 1) == 0) {
                    break;
                }
                sum -= piles[tmp] >> 1;
                piles[tmp] -= piles[tmp] >> 1;
                pq.Enqueue(tmp, -piles[tmp]);
                cnt++;
            }
            return sum;
        }

        public static long MinimumPerimeter(long neededApples) {
            int l = 0, r = 70000;
            // f f t t 
            while (l < r) {
                int m = (r + l) >> 1;
                if (f(m) >= neededApples) {
                    r = m;
                } else {
                    l = m + 1;
                }
            }
            return l << 3;

            // formular
            long f(int n) {
                return 4 * (long)n * (n + 1) * (n + 1) - 2 * n * (n + 1);
            }
        }

        public static int[] FullBloomFlowers(int[][] flowers, int[] people) {
            List<(int, int)> vals = new();
            for (int i = 0; i < flowers.Length; i++) {
                vals.Add((flowers[i][0], 1));
                vals.Add((flowers[i][1] + 1, -1));
            }
            vals.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            PriorityQueue<int, int> pq = new();
            for (int i = 0; i < people.Length; i++) {
                pq.Enqueue(i, people[i]);
            }
            int d = 0;
            int cur = 0;
            int[] res = new int[people.Length];
            while (pq.Count > 0) {
                var tmp = pq.Dequeue();
                while (cur < vals.Count && people[tmp] >= vals[cur].Item1) {
                    d += vals[cur].Item2;
                    cur++;
                }
                res[tmp] = d;
            }
            return res;
        }

        public static bool CircularArrayLoop(int[] nums) {
            HashSet<int> vis = new();
            HashSet<int> seen = new();
            // 先找正数
            int n = nums.Length;
            for (int i = 0; i < n; i++) {
                seen.Clear();
                if (nums[i] < 0) {
                    continue;
                }
                if (!vis.Add(i)) {
                    continue;
                }
                int nxt = i;
                while (true) {
                    nxt = (nxt + nums[nxt]) % n;
                    if (nums[nxt] > 0) {
                        if (!seen.Add(nxt)) {
                            if (seen.Count > 1 && nums[nxt] != n) {
                                return true;
                            } else {
                                break;
                            }
                        }
                    } else {
                        // > 0
                        break;
                    }
                }
            }

            vis.Clear();
            for (int i = 0; i < n; i++) {
                seen.Clear();
                if (nums[i] > 0) {
                    continue;
                }
                if (!vis.Add(i)) {
                    continue;
                }
                int nxt = i;
                while (true) {
                    nxt = (-nums[nxt] * n + nxt + nums[nxt]) % n;
                    if (nums[nxt] < 0) {
                        if (!seen.Add(nxt)) {
                            if (seen.Count > 1 && -nums[nxt] != n) {
                                return true;
                            } else {
                                break;
                            }
                        }
                    } else {
                        // > 0
                        break;
                    }
                }
            }
            return false;
        }

        public static int MinimumDeviation(int[] nums) {
            SortedDictionary<int, int> tree = new();
            for (int i = 0; i < nums.Length; i++) {
                int x = nums[i];
                if (x % 2 == 1) { x <<= 1; }

                tree.TryAdd(x, 0);
                tree[x]++;
            }
            int res = tree.Keys.Last() - tree.Keys.First();
            while (true) {
                int max = tree.Keys.Last();
                if (max % 2 == 1) {
                    break;
                }
                tree[max]--;
                if (tree[max] == 0)
                    tree.Remove(max);
                max >>= 1;
                tree.TryAdd(max, 0);
                tree[max]++;

                res = Math.Min(tree.Keys.Last() - tree.Keys.First(), res);
            }
            return res;
        }

        public static long MakeSimilar(int[] nums, int[] target) {
            Array.Sort(nums);
            Array.Sort(target);
            List<int> oddNums = new();
            List<int> evenNums = new();
            List<int> oddTar = new();
            List<int> evenTar = new();
            int n = nums.Length;

            for (int i = 0; i < n; i++) {
                if (nums[i] % 2 == 1) {
                    oddNums.Add(nums[i]);
                } else {
                    evenNums.Add(nums[i]);
                }

                if (target[i] % 2 == 1) {
                    oddTar.Add(target[i]);
                } else {
                    evenTar.Add(target[i]);
                }
            }

            long res = 0;
            for (int i = 0; i < oddNums.Count; i++) {
                res += Math.Abs(oddNums[i] - oddTar[i]) / 2;
            }
            for (int i = 0; i < evenNums.Count; i++) {
                res += Math.Abs(evenNums[i] - evenTar[i]) / 2;
            }
            return res / 2;

        }

        public static ListNode? DeleteDuplicates2(ListNode head) {
            if (head is null || head.next is null) {
                return head;
            }

            ListNode cur1 = head;
            ListNode? cur2 = head;
            while (true) {
                while (cur2 is not null && cur2.val == cur1.val) {
                    cur2 = cur2.next;
                }
                cur1.next = cur2;
                if (cur2 is null) {
                    break;
                }
                cur1 = cur1.next!;
                cur2 = cur2.next;
            }
            return head;
        }

        public static int MaximumRows(int[][] matrix, int numSelect) {
            int[] nums = new int[matrix.Length];
            for (int i = 0; i < matrix.Length; i++) {
                for (int j = 0; j < matrix[i].Length; j++) {
                    nums[i] |= matrix[i][j] << (matrix[i].Length - j - 1);
                }
            }
            int res = 0;
            int cnt = 1 << matrix[0].Length;
            for (int i = 0; i < cnt; i++) {
                int tmp = i;
                int bitc = 0;
                while (tmp > 0) {
                    if (tmp % 2 == 1) {
                        bitc++;
                    }
                    tmp >>= 1;
                }
                if (bitc != numSelect) {
                    continue;
                }
                int ans = 0;
                for (int j = 0; j < nums.Length; j++) {
                    if ((i | nums[j]) == i) {
                        ans++;
                    }
                }
                res = Math.Max(ans, res);
            }

            return res;
        }

        public static int Count(string num1, string num2, int min_sum, int max_sum) {
            num1 = num1.PadLeft(num2.Length, '0');
            long res = 0;
            const int MOD = 1000000007;
            int[,,,] map = new int[num1.Length + 1, num1.Length * 9, 2, 2];
            for (int i = 0; i < map.GetLength(0); i++) {
                for (int j = 0; j < map.GetLength(1); j++) {
                    for (int k = 0; k < 2; k++) {
                        for (int m = 0; m < 2; m++) {
                            map[i, j, k, m] = -1;
                        }
                    }
                }
            }

            // 计算首位等于num1
            res = f(1, num1[0] - '0', false, num1[0] < num2[0]) % MOD;
            for (int i = num1[0] + 1; i < num2[0]; i++) {
                res = (res + f(1, i - '0', true, true)) % MOD;
            }
            if (num1[0] != num2[0]) {
                res = (res + f(1, num2[0] - '0', num1[0] < num2[0], false)) % MOD;
            }
            return (int)res;


            // 第d位开始计算，前面的数位和是sum，前d-1位是否严格大于num1，前d-1位是否严格小于num2
            long f(int d, int sum, bool greater, bool less) {
                long ans = 0;
                if (d > num1.Length) {
                    return ans;
                } else if (d == num1.Length) {
                    if (sum >= min_sum && sum <= max_sum) {
                        return 1;
                    } else {
                        return 0;
                    }
                }
                if (map[d, sum, greater ? 1 : 0, less ? 1 : 0] != -1) {
                    return map[d, sum, greater ? 1 : 0, less ? 1 : 0];
                }

                if (!greater && !less) {
                    // 前d-1位正好完全等于num1和num2的前d-1位
                    ans += f(d + 1, sum + (num1[d] - '0'), false, num1[d] < num2[d]);
                    for (int i = num1[d] + 1; i < num2[d]; i++) {
                        ans = (ans + f(d + 1, sum + i - '0', true, true)) % MOD;
                    }
                    if (num1[d] != num2[d]) {
                        ans = (ans + f(d + 1, sum + (num2[d] - '0'), num1[d] < num2[d], false)) % MOD;
                    }
                } else if (!greater && less) {
                    // 前d-1位小于num2，但和num1贴边
                    ans += f(d + 1, sum + (num1[d] - '0'), false, true);
                    for (int i = num1[d] + 1; i <= '9'; i++) {
                        ans = (ans + f(d + 1, sum + i - '0', true, true)) % MOD;
                    }
                } else if (greater && !less) {
                    // 前d-1位严格大于num1，但和less贴边了
                    for (int i = '0'; i < num2[d]; i++) {
                        ans = (ans + f(d + 1, sum + i - '0', true, true)) % MOD;
                    }
                    ans = (ans + f(d + 1, sum + num2[d] - '0', true, false)) % MOD;
                } else {
                    // 没有任何限制
                    for (int i = '0'; i <= '9'; i++) {
                        ans = (ans + f(d + 1, sum + i - '0', true, true)) % MOD;
                    }
                }

                return map[d, sum, greater ? 1 : 0, less ? 1 : 0] = (int)ans;
            }
        }

        public static int MinTaps(int n, int[] ranges) {
            int[] left = new int[ranges.Length];
            int[] right = new int[ranges.Length];
            int choose = 0; // 选择后能扩展到的最大的右边界
            int res = 0;
            for (int v = 0; v < ranges.Length; v++) {
                left[v] = -ranges[v] + v;
                right[v] = ranges[v] + v;
            }

            Array.Sort(left, right);
            int maxV = 0; // 目前的最大值
            int maxI = -1;// 目前的最大索引
            int i = 0, j = 0;
            while (j < left.Length && choose < n) {

                while (j < left.Length && left[j] <= choose) {
                    if (right[j] > maxV) {
                        maxV = right[j];
                        maxI = j;
                    }
                    j++;
                }
                res++;
                if (choose == maxV) {
                    break;
                }
                choose = maxV;
                i = j;
            }
            if (choose < n) {
                return -1;
            }
            return res;
        }

        public static bool CanConvert(string str1, string str2) {
            if (str1.SequenceEqual(str2)) {
                return true;
            }
            int[] map = new int[26];
            Array.Fill(map, -1);
            for (int i = 0; i < str1.Length; i++) {
                if (map[str1[i] - 'a'] != -1 && map[str1[i] - 'a'] != str2[i]) {
                    return false;
                }
                map[str1[i] - 'a'] = str2[i];
            }
            int kind = 0;
            Array.Fill(map, -1);
            for (int i = 0; i < str2.Length; i++) {
                map[str2[i] - 'a'] = 0;
            }

            for (int i = 0; i < map.Length; i++) {
                if (map[i] != -1) {
                    kind++;
                }
            }
            return kind < 26;
        }

        public static long MaximumSumOfHeights2(IList<int> maxHeights) {
            // 枚举峰顶
            int n = maxHeights.Count;
            long res = 0;
            for (int i = 0; i < n; i++) {
                long tmp = 0;
                tmp += maxHeights[i];
                int last = maxHeights[i];
                for (int j = i + 1; j < n; j++) {
                    tmp += last = Math.Min(last, maxHeights[j]);
                }
                last = maxHeights[i];
                for (int j = i - 1; j >= 0; j--) {
                    tmp += last = Math.Min(last, maxHeights[j]);
                }
                res = Math.Max(res, tmp);
            }
            return res;
        }

        public static double MaxAverageRatio(int[][] classes, int extraStudents) {
            PriorityQueue<int, double> pq = new();
            for (int i = 0; i < classes.Length; i++) {
                pq.Enqueue(i, -diff(classes[i]));
            }
            while (extraStudents > 0) {
                int tmp = pq.Dequeue();
                classes[tmp][0]++;
                classes[tmp][1]++;
                pq.Enqueue(tmp, -diff(classes[tmp]));
                extraStudents--;
            }
            double res = 0;
            for (int i = 0; i < classes.Length; i++) {
                res += (double)classes[i][0] / classes[i][1];
            }
            return res / classes.Length;

            static double diff(int[] classes) {
                return (double)(classes[1] - classes[0]) / (classes[1] * (classes[1] + 1));
            }
        }

        public static double MincostToHireWorkers(int[] quality, int[] wage, int k) {
            int n = quality.Length;
            (int qua, int wage)[] staff = new (int, int)[n];
            for (int i = 0; i < n; i++) {
                staff[i] = (quality[i], wage[i]);
            }
            Array.Sort(staff, (a, b) => (1.0 * a.Item1 / a.Item2).CompareTo(1.0 * b.Item1 / b.Item2));
            PriorityQueue<int, int> pq = new();
            for (int i = 1; i < n; i++) {
                pq.Enqueue(i, staff[i].qua);
            }
            int remainSum = 0; // 其他工人的工资quality的和
            HashSet<int> set = new(); // 记录其他工人前k-1名的序号
            for (int i = 0; i < k - 1; i++) {
                remainSum += staff[pq.Peek()].qua;
                set.Add(pq.Dequeue());
            }

            double res = int.MaxValue;
            for (int i = 0; i <= n - k; i++) {
                if (set.Contains(i)) {
                    // 更新剩余工人的分数
                    remainSum -= staff[i].qua;
                    while (pq.Count > 0 && pq.Peek() <= i) {
                        pq.Dequeue();
                    }
                    if (pq.Count == 0) {
                        break;
                    }
                    int tmp = pq.Dequeue();
                    remainSum += staff[tmp].qua;
                    set.Add(tmp);
                    set.Remove(i);
                }
                res = Math.Min(res, staff[i].wage + remainSum * 1.0 * staff[i].wage / staff[i].qua);
            }
            return res;
        }

        public static bool CanMeasureWater(int jug1Capacity, int jug2Capacity, int targetCapacity) {
            if (jug1Capacity == jug2Capacity) {
                return targetCapacity == jug1Capacity;
            }
            if (jug1Capacity == targetCapacity || jug2Capacity == targetCapacity) {
                return true;
            }
            int max = Math.Max(jug2Capacity, jug1Capacity);
            int min = Math.Min(jug2Capacity, jug1Capacity);
            bool[] vis = new bool[max + min + 1];
            Queue<int> que = new();
            que.Enqueue(max - min);
            vis[max] = vis[min] = vis[max - min] = true;
            while (que.Count > 0) {
                var tmp = que.Dequeue();
                if (tmp == targetCapacity) {
                    return true;
                }

                if (max - tmp > 0 && min - (max - tmp) > 0) {
                    if (!vis[min - (max - tmp)]) {
                        vis[min - (max - tmp)] = true;
                        que.Enqueue(min - (max - tmp));
                    }
                }
                if (min - tmp > 0 && max - (min - tmp) > 0) {
                    if (!vis[max - (min - tmp)]) {
                        vis[max - (min - tmp)] = true;
                        que.Enqueue(max - (min - tmp));
                    }
                }

                if (tmp < max && tmp - min > 0 && !vis[tmp - min]) {
                    vis[tmp - min] = true;
                    que.Enqueue(tmp - min);
                }

                if (tmp + min < vis.Length && !vis[tmp + min]) {
                    vis[tmp + min] = true;
                    que.Enqueue(tmp + min);
                }
                if (tmp + max < vis.Length && !vis[tmp + max]) {
                    vis[tmp + max] = true;
                    que.Enqueue(tmp + max);
                }
            }

            return false;
        }

        public static void WiggleSort(int[] nums) {
            int n = nums.Length;
            n = (n + 1) / 2 * 2;
            for (int i = 0; i < n - 1; i += 2) {
                if (get(i) > get(i + 1)) {
                    if (i - 1 >= 0 && get(i + 1) > get(i - 1)) {
                        swap(i, i + 1);
                        swap(i, i - 1);
                    } else {
                        swap(i, i + 1);
                    }
                } else {
                    if (i > 1 && get(i) > get(i - 1)) {
                        swap(i, i - 1);
                    }
                }
            }
            int get(int i) {
                return i >= nums.Length ? int.MaxValue : nums[i];
            }


            void swap(int i, int j) {
                (nums[i], nums[j]) = (nums[j], nums[i]);
            }
        }

        public static int MajorityElement(int[] nums) {
            int cnt = 0;
            int pos = -1;
            for (int i = 0; i < nums.Length; i++) {
                if (pos >= 0 && nums[pos] == nums[i]) {
                    cnt++;
                } else {
                    if (cnt > 0) {
                        cnt--;
                    } else {
                        pos = i;
                        cnt = 1;
                    }
                }
            }
            return nums[pos];
        }

        public static long IncremovableSubarrayCount(int[] nums) {
            int n = nums.Length;
            int a;
            // [0..a)
            for (a = 1; a < n && nums[a] > nums[a - 1]; a++) { }
            int b;
            // (b..n-1]
            for (b = n - 2; b >= 0 && nums[b + 1] > nums[b]; b--) { }
            long res = 0;
            if (a == n && b == -1) {
                return (1 + (long)n) * n / 2;
            }
            res += a + n - b - 1 + 1;
            int cur1 = 0, cur2 = b + 1;
            while (cur1 < a && cur2 < n) {
                if (nums[cur2] > nums[cur1]) {
                    res += n - cur2;
                    cur1++;
                } else {
                    cur2++;
                }
            }

            return res;
        }

        public static int CountPairs(IList<IList<int>> coordinates, int k) {
            // 对于每一个点，遍历x的异或值的可能情况，确定是否存在y这样的异或值
            Dictionary<int, Dictionary<int, int>> dict = new(); // key是x坐标，value是y坐标和频数
            for (int i = 0; i < coordinates.Count; i++) {
                if (!dict.ContainsKey(coordinates[i][0])) {
                    dict.Add(coordinates[i][0], new());
                    dict[coordinates[i][0]].Add(coordinates[i][1], 0);
                }
                dict[coordinates[i][0]][coordinates[i][1]]++;
            }

            int res = 0;
            for (int i = 0; i < coordinates.Count; i++) {
                for (int j = 0; j <= k; j++) {
                    if (!dict.ContainsKey(j ^ coordinates[i][0])) {
                        continue;
                    }

                }
            }
            throw new NotImplementedException();
            return 0;
        }

        public static int StoneGameVI(int[] aliceValues, int[] bobValues) {
            int n = aliceValues.Length;
            int[] index = new int[n];
            for (int i = 0; i < n; i++) {
                index[i] = i;
            }
            Array.Sort(index, (a, b) => (aliceValues[b] + bobValues[b]).CompareTo(aliceValues[a] + bobValues[a]));
            int diff = 0;
            for (int i = 0; i < n; i++) {
                if (i % 2 == 0) {
                    diff += aliceValues[index[i]];
                } else {
                    diff -= bobValues[index[i]];
                }
            }
            return Math.Sign(diff);
        }

        public static int MinGroupsForValidAssignment(int[] nums) {
            Dictionary<int, int> dict = new();
            int n = nums.Length;
            for (int i = 0; i < n; i++) {
                if (!dict.ContainsKey(nums[i])) {
                    dict[nums[i]] = 0;
                }
                dict[nums[i]]++;
            }

            int min = n;
            foreach (var item in dict) {
                min = Math.Min(min, item.Value);
            }

            int res = int.MaxValue;
            for (int i = 1; i <= min; i++) {
                int tmp = 0;
                // i, i+1
                foreach (var item in dict) {
                    int val = calc(item.Value, i);
                    if (val == -1) {
                        tmp = -1;
                        break;
                    }
                    tmp += val;
                }
                if (tmp > 0) {
                    res = Math.Min(res, tmp);
                }
            }
            return res;

            int calc(int sum, int m) {
                for (int i = sum / (m + 1); i >= 0; i--) {
                    if ((sum - i) % m == 0) {
                        return (sum - i) / m;
                    }
                }
                return -1;
            }
        }

        public static int StoneGameVII(int[] stones) {
            int n = stones.Length;
            int[] pre = new int[n];
            pre[0] = stones[0];
            for (int i = 1; i < n; i++) {
                pre[i] = pre[i - 1] + stones[i];
            }
            Dictionary<(int, int), int> map = new();

            int res = dfs(0, n - 1);

            return res;

            int sum(int l, int r) {
                return pre[r] - (l == 0 ? 0 : pre[l - 1]);
            }

            int dfs(int l, int r) {
                // 最大化当前玩家减去对方玩家分数的差值，返回差值
                if (r - l == 1) {
                    return Math.Max(stones[l], stones[r]);
                }
                if (map.ContainsKey((l, r))) {
                    return map[(l, r)];
                }
                int res1 = -dfs(l + 1, r) + sum(l + 1, r);
                int res2 = -dfs(l, r - 1) + sum(l, r - 1);
                return map[(l, r)] = Math.Max(res1, res2);

            }
        }

        public static int MinimumTimeToInitialState(string word, int k) {
            int[] next = BuildNextArray(word);
            HashSet<int> len = new();
            int n = word.Length;
            if (n == k) {
                return 1;
            }
            int j = next[^1] - 1;
            len.Add(j);
            while (j >= 0 && next[j] >= 1) {
                j = next[j] - 1;
                len.Add(j);
            }
            int res = 0;
            for (int i = n - k; i >= 0; i -= k) {
                res++;
                if (len.Contains(i - 1)) {
                    return res;
                }
            }
            return (word.Length + k - 1) / k;

            throw new NotImplementedException();

            int[] BuildNextArray(string s) {
                int[] next = new int[s.Length];
                int j = 0;

                for (int i = 1; i < s.Length; i++) {
                    while (j > 0 && s[i] != s[j]) {
                        j = next[j - 1];
                    }

                    if (s[i] == s[j]) {
                        j++;
                    }

                    next[i] = j;
                }

                return next;
            }
        }

        public static int MaxResult(int[] nums, int k) {
            PriorityQueue<int, int> pq = new();
            int n = nums.Length;
            int[] dp = new int[n];
            dp[^1] = nums[^1];
            pq.Enqueue(n - 1, -dp[^1]);
            for (int i = n - 2; i >= 0; i--) {
                while (pq.Count > 0 && pq.Peek() - i > k) {
                    pq.Dequeue();
                }
                var tmp = pq.Peek();
                dp[i] = nums[i] + dp[tmp];
                pq.Enqueue(i, -dp[i]);
            }
            return dp[0];
        }

        public static int MaxDistance(IList<IList<int>> arrays) {
            int n = arrays.Count;
            for (int i = 0; i < n; i++) {
                arrays[i] = new int[] { arrays[i][0], arrays[i][^1] };
            }

            int min = arrays[0][0];
            int max = arrays[0][1];
            int res = int.MinValue;
            for (int i = 1; i < n; i++) {
                res = Math.Max(res, Math.Abs(arrays[i][1] - min));
                min = Math.Min(min, arrays[i][0]);

                res = Math.Max(res, Math.Abs(arrays[i][0] - max));
                max = Math.Max(max, arrays[i][1]);
            }

            return res;
        }

        public static int ClosestToTarget(int[] arr, int target) {
            int res = int.MaxValue;
            int n = arr.Length;
            for (int i = 0; i < n; i++) {
                // 模板：修改arr的每一位，使arr[j]为j到i闭区间的所有值的按位与（交集）
                int x = arr[i];
                res = Math.Min(res, Math.Abs(x - target));
                int j = i - 1;
                while (j >= 0 && (arr[j] & x) != arr[j]) {
                    arr[j] &= x;
                    res = Math.Min(res, Math.Abs(arr[j] - target));
                }
            }

            return res;
        }

        public static bool PredictTheWinner(int[] nums) {
            int n = nums.Length;
            int[,] map = new int[n, n];
            int sum = nums.Sum();
            return dfs(0, n - 1, sum) * 2 >= sum;

            int dfs(int l, int r, int sum) {
                if (l == r) {
                    return nums[l];
                }
                if (map[l, r] > 0) {
                    return map[l, r];
                }
                int res1 = nums[l] + sum - dfs(l + 1, r, sum - nums[l]);
                int res2 = nums[r] + sum - dfs(l, r - 1, sum - nums[r]);
                return map[l, r] = Math.Max(res1, res2);
            }
        }

        public static string StoneGameIII(int[] stoneValue) {
            int n = stoneValue.Length;
            int[] p = new int[n + 1];
            int[] dp = new int[n];
            Array.Fill(dp, int.MinValue);
            for (int i = 0; i < n; i++) {
                dp[i] = int.MinValue;
            }
            for (int i = 1; i < p.Length; i++) {
                p[i] += p[i - 1];
            }
            int a = dfs(0);
            if (a * 2 == p[^1]) {
                return "Tie";
            }
            if (a * 2 > p[^1]) {
                return "Alice";
            }
            return "Bob";

            int sum(int l, int r) => p[r + 1] - p[l];

            int dfs(int l) {
                // [l..]的区间，先手能拿多少石子
                if (l == n - 1) {
                    return stoneValue[l];
                }
                if (dp[l] != int.MinValue) {
                    return dp[l];
                }

                int res = int.MinValue;
                for (int i = 1; i <= 3; i++) {
                    int cur = 0;
                    for (int j = 0; j < i && j + l < n; j++) {
                        cur += stoneValue[j + l];
                    }
                    if (i + l < n) {
                        cur += sum(i + l, n - 1) - dfs(i + l);
                    }
                    res = Math.Max(res, cur);
                }
                return dp[l] = res;
            }
        }

        public static bool SumGame(string num) {
            int cnt = num.Count((a) => a == '?');
            int sumL = 0, sumR = 0;
            int n = num.Length;
            int cntL = 0, cntR = 0;
            for (int i = 0; i < n / 2; i++) {
                if (num[i] == '?') {
                    cntL++;
                } else {
                    sumL += num[i] - '0';
                }
            }
            cntR = cnt - cntL;
            for (int i = n / 2; i < n; i++) {
                if (num[i] != '?') {
                    sumR += num[i] - '0';
                }
            }
            bool first = true;
            while (cnt-- > 0) {
                if (first) {
                    if (sumL > sumR || sumL == sumR && cntL > cntR) {
                        if (cntL > 0) {
                            cntL--;
                            sumL += 9;
                        } else {
                            if (sumR + 9 > sumL) {
                                sumR += 9;
                            }
                            cntR--;
                        }
                    } else {
                        if (cntR > 0) {
                            cntR--;
                            sumR += 9;
                        } else {
                            if (sumL + 9 > sumR) {
                                sumL += 9;
                            }
                            cntL--;
                        }
                    }
                } else {
                    if (sumL > sumR) {
                        if (cntR > 0) {
                            int add = Math.Min(9, sumL - sumR);
                            sumR += add;
                            cntR--;
                        } else {
                            return true;
                        }
                    } else {
                        if (cntL > 0) {
                            int add = Math.Min(9, sumR - sumL);
                            sumL += add;
                            cntL--;
                        } else {
                            return true;
                        }
                    }
                }
                first = !first;
            }
            return sumL != sumR;
        }

        public static TreeNode LowestCommonAncestor(TreeNode root, TreeNode p, TreeNode q) {
            TreeNode? res = null;
            contains(root);
            return res!;

            int contains(TreeNode? cur) {
                if (cur is null) {
                    return 0;
                }
                int a = contains(cur.left);
                int b = contains(cur.right);
                if ((cur == q || cur == p) && (a == 1 || b == 1)) {
                    res = cur;
                    return -1;
                }
                if (cur == p || cur == q) {
                    return 1;
                }
                if (a == -1 || b == -1) {
                    return -1;
                }
                if (a == 1 && b == 1) {
                    res = cur;
                    return 2;
                }
                return a + b;
            }
        }

        public static int[][] BuildMatrix(int k, int[][] rowConditions, int[][] colConditions) {
            int[] cols = new int[k + 1];
            int[] rows = new int[k + 1];
            List<int>[] graphCol = new List<int>[k + 1];
            List<int>[] graphRow = new List<int>[k + 1];
            for (int i = 0; i < graphCol.Length; i++) {
                graphCol[i] = new();
                graphRow[i] = new();
            }
            int[] inR = new int[k + 1];
            int[] inC = new int[k + 1];
            for (int i = 0; i < rowConditions.Length; i++) {
                graphRow[rowConditions[i][0]].Add(rowConditions[i][1]);
                inR[rowConditions[i][1]]++;
            }
            for (int i = 0; i < colConditions.Length; i++) {
                graphCol[colConditions[i][0]].Add(colConditions[i][1]);
                inC[colConditions[i][1]]++;
            }
            // 拓扑排序，邻接表记录的是下一个点
            Queue<int> que = new();
            for (int i = 1; i < inR.Length; i++) {
                if (inR[i] == 0) {
                    que.Enqueue(i);
                }
            }
            if (que.Count == 0) {
                return Array.Empty<int[]>();
            }
            int cnt = 0;
            while (que.Count > 0) {
                List<int> tmp = new();
                while (que.Count > 0) {
                    tmp.Add(que.Dequeue());
                }
                foreach (var item in tmp) {
                    rows[item] = cnt;
                    for (int i = 0; i < graphRow[item].Count; i++) {
                        inR[graphRow[item][i]]--;
                        if (inR[graphRow[item][i]] == 0) {
                            que.Enqueue(graphRow[item][i]);
                        }
                    }
                }
                cnt++;
            }
            que.Clear();
            cnt = 0;
            for (int i = 1; i < inC.Length; i++) {
                if (inC[i] == 0) {
                    que.Enqueue(i);
                }
            }
            if (que.Count == 0) {
                return Array.Empty<int[]>();
            }
            while (que.Count > 0) {
                List<int> tmp = new();
                while (que.Count > 0) {
                    tmp.Add(que.Dequeue());
                }
                foreach (var item in tmp) {
                    cols[item] = cnt;
                    for (int i = 0; i < graphCol[item].Count; i++) {
                        inC[graphCol[item][i]]--;
                        if (inC[graphCol[item][i]] == 0) {
                            que.Enqueue(graphCol[item][i]);
                        }
                    }
                    cnt++;
                }
            }
            if (inC.Any((a) => a != 0) || inR.Any((a) => a != 0)) {
                return Array.Empty<int[]>();
            }
            int[][] res = new int[k][];
            for (int i = 0; i < k; i++) {
                res[i] = new int[k];
            }
            for (int i = 1; i <= k; i++) {
                res[rows[i]][cols[i]] = i;
            }
            return res;
        }

        public static int LenLongestFibSubseq(int[] arr) {
            int n = arr.Length;
            Dictionary<int, int> dict = new();
            for (int i = 0; i < n; i++) {
                dict[arr[i]] = i;
            }
            int[,] dp = new int[n, n];
            // dp[first, second]
            int res = 0;
            for (int i = n - 2; i >= 0; i--) {
                for (int j = i + 1; j < n; j++) {
                    if (dict.TryGetValue(arr[i] + arr[j], out int idx)) {
                        dp[i, j] = dp[j, idx] + 1;
                    }
                    dp[i, j] = Math.Max(2, dp[i, j]);
                    res = Math.Max(res, dp[i, j]);
                }
            }
            return res > 2 ? res : 0;
        }

        public static int MinOperations(string s1, string s2, int x) {
            int n = s1.Length;
            List<int> idx = new();
            for (int i = 0; i < n; i++) {
                if (s1[i] != s2[i]) {
                    idx.Add(i);
                }
            }
            if (idx.Count % 2 != 0) {
                return -1;
            }
            int cur1 = 0, cur2 = 1;
            int m = idx.Count;
            List<List<int>> groups = new();

            while (cur1 < m) {
                groups.Add(new());
                groups[^1].Add(idx[cur1]);
                //bool flag = false;
                while (cur2 < m && idx[cur2] - idx[cur2 - 1] <= x) {
                    groups[^1].Add(idx[cur2]);
                    cur2++;
                    //flag = true;
                }
                cur1 = cur2;
                cur2++;
            }
            int res = 0;
            int odd = 0;
            for (int i = 0; i < groups.Count; i++) {
                if (groups[i].Count % 2 == 1) {
                    odd++;
                }
                res += f(i);
            }
            res += odd / 2 * x;
            return res;

            int f(int idx) {
                // 计算groups[idx]的值
                int res = 0;
                if (groups[idx].Count % 2 == 0) {
                    for (int i = 0; i < groups[idx].Count - 1; i += 2) {
                        res += groups[idx][i + 1] - groups[idx][i];
                    }
                    return res;
                }

                for (int i = 0; i < groups[idx].Count - 1; i++) {
                    groups[idx][i] = groups[idx][i + 1] - groups[idx][i];
                }
                var x = groups[idx];
                groups[idx].RemoveAt(x.Count - 1);
                if (x.Count == 0) {
                    return 0;
                }
                int[,] dp = new int[x.Count / 2, 2];
                dp[0, 0] = x[0];
                dp[0, 1] = x[1];
                for (int i = 1; i < dp.GetLength(0); i++) {
                    dp[i, 0] = dp[i - 1, 0] + x[i * 2];
                    dp[i, 1] = Math.Min(dp[i - 1, 0] + x[i * 2 + 1], dp[i - 1, 1] + x[i * 2 + 1]);
                }
                return int.Min(dp[dp.GetLength(0) - 1, 1], dp[dp.GetLength(0) - 1, 0]);
            }
        }

        public static int CountTriplets(int[] nums) {
            Dictionary<int, int> dict = new();
            int n = nums.Length;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    int x = nums[i] & nums[j];
                    if (!dict.TryGetValue(x, out int value)) {
                        value = 0;
                        dict.Add(x, value);
                    }
                    dict[x] = ++value;
                }
            }
            int res = 0;
            for (int i = 0; i < n; i++) {
                foreach (var j in dict.Keys) {
                    if ((nums[i] & j) == 0) {
                        res += dict[j];
                    }
                }
            }
            return res;
        }

        public static int MinIncrements(int n, int[] cost) {
            int[] cost2 = new int[n + 1];
            Array.Copy(cost, 0, cost2, 1, n);
            int max = dfs(1);
            int res = dfs2(1, max);
            return res;

            int dfs(int idx) {
                // 返回从idx开始的最大路径和
                if (idx > n) {
                    return 0;
                }
                int res = 0;
                int l = dfs(idx << 1);
                int r = dfs((idx << 1) | 1);
                res = Math.Max(l, r);
                return res + cost2[idx];
            }

            int dfs2(int idx, int add) {
                // 从idx出发，给所有路径增加add的最少操作
                if (idx > n) {
                    return 0;
                }

                int res = 0;
                int max = dfs(idx);
                // 获取左右路径的最大值
                // 当前节点+=add-max

                int l = dfs2(idx << 1, add - (cost2[idx] + (add - max)));
                int r = dfs2(idx << 1 | 1, add - (cost2[idx] + (add - max)));
                return l + r + add - max;
            }
        }

        public static long MinIncrementOperations(int[] nums, int k) {
            int n = nums.Length;
            long[] dp = new long[n];
            Array.Fill(dp, -1);
            f(0);
            f(1);
            f(2);
            return Math.Min(dp[0], Math.Min(dp[1], dp[2]));

            long f(int startIdx) {
                if (startIdx >= n) {
                    return 0;
                }
                if (dp[startIdx] != -1) {
                    return dp[startIdx];
                }

                long res = long.MaxValue;
                // 改
                int right = startIdx;
                int greaterIdx = startIdx;
                //int cnt = 0;
                while (right < n) {
                    if (nums[right] >= k) {
                        greaterIdx = right;
                    }
                    if (right >= greaterIdx + 3) {
                        break;
                    }
                    right++;
                }

                for (int i = greaterIdx + 1; i <= greaterIdx + 3; i++) {
                    long has = Math.Max(0, -nums[startIdx] + k);
                    has += f(i);
                    res = Math.Min(res, has);
                }
                return dp[startIdx] = res;

            }
        }

        public static int MinNonZeroProduct(int p) {
            if (p == 1) {
                return 1;
            }
            const int MOD = 10_0000_0007;
            checked {

                long cnt1 = ((1L << p) - 1) >> 1;
                BigInteger remain = ((((BigInteger)1 << p) + 1) * (1L << (p - 1)) - (1L << p) - cnt1) / (cnt1 + 1);
                return (int)((pow(remain, cnt1, MOD) * (remain + 1) % MOD) % MOD);
            }

            static long pow(BigInteger a, long b, long p) {
                long result = 1;
                a %= p;
                while (b > 0) {
                    if (b % 2 == 1)
                        result = (long)((result * a) % p);
                    b >>= 1;
                    a = (a * a) % p;
                }
                return result;
            }
        }

        public static bool IsValidSerialization(string preorder) {
            List<string> stack = new();
            string[] e = preorder.Split(',');
            for (int i = 0; i < e.Length; i++) {
                stack.Add(e[i]);
                process();
            }
            return stack.Count == 1 && stack[0] == "#";

            void process() {
                if (stack[^1] == "#") {
                    // 只检查井号，其他不用检查
                    if (stack.Count > 2 && stack[^2] == "#" && stack[^3] != "#") {
                        stack.RemoveRange(stack.Count - 3, 3);
                        stack.Add("#");
                        process();
                    }
                }
            }
        }

        public static int CountKSubsequencesWithMaxBeauty(string s, int k) {
            const int MOD = 1000000007;

            int[] freq = new int[26];
            for (int i = 0; i < s.Length; i++) {
                freq[s[i] - 'a']++;
            }
            Array.Sort(freq, (a, b) => b.CompareTo(a));
            if (freq.Count((i) => i != 0) < k) { return 0; }
            int res = 1;
            // 先找到第k-1个位置处有多少个连续的词频数

            int lastIndex = -1;
            int lastCnt = 0;
            for (int i = 0; i < freq.Length; i++) {
                if (freq[i] == freq[k - 1]) {
                    lastIndex = i;
                    break;
                }
            }
            lastCnt = lastIndex;

            int cnt;
            if (k == freq.Length || freq[k - 1] != freq[k]) {
                cnt = 0;
                lastCnt = k;
            } else {
                cnt = freq.Count((i) => i == freq[k - 1]);
                res = (int)((long)(res * Math.Pow(freq[k - 1], k - lastCnt)) % MOD * Combination(cnt, k - lastCnt) % MOD);
            }
            for (int i = 0; i < lastCnt; i++) {
                res = (int)((long)res * freq[i] % MOD);
            }

            return res;



            static int Combination(int n, int k) {
                if (k > n)
                    return 0;
                if (k == 0 || k == n)
                    return 1;

                int[,] dp = new int[n + 1, k + 1];

                // Initialize the table
                for (int i = 0; i <= n; i++) {
                    for (int j = 0; j <= Math.Min(i, k); j++) {
                        // Base cases
                        if (j == 0 || j == i)
                            dp[i, j] = 1;
                        else
                            dp[i, j] = (dp[i - 1, j - 1] + dp[i - 1, j]) % MOD;
                    }
                }

                return dp[n, k];
            }
        }

        public static int MaxFrequency2(int[] nums, int k) {
            Array.Sort(nums);
            int l = 0, r = 0;
            long curK = 0;
            int res = 1;
            while (true) {
                r++;
                if (r >= nums.Length) {
                    break;
                }

                curK += ((long)nums[r] - nums[r - 1]) * (r - l);
                while (curK > k) {
                    l++;
                    if (l > r) {
                        break;
                    }
                    curK -= nums[r] - nums[l];
                }
                res = Math.Max(res, r - l + 1);
            }

            return res;
        }

        public static int MaximumLength(int[] nums, int k) {
            int n = nums.Length;
            int[,] dp = new int[n, k + 1];
            for (int i = 0; i < n; i++) {
                dp[i, 0] = 1;
            }

            for (int i = n - 2; i >= 0; i--) {
                for (int j = 0; j <= k; j++) {
                    for (int i1 = i + 1; i1 < n; i1++) {
                        if (nums[i1] != nums[i] && j >= 1) {
                            dp[i, j] = Math.Max(dp[i, j], dp[i1, j - 1] + 1);
                        } else if (nums[i1] == nums[i]) {
                            dp[i, j] = Math.Max(dp[i, j], dp[i1, j] + 1);
                        }
                    }

                }
                for (int l = 1; l <= k; l++) {
                    dp[i, l] = Math.Max(dp[i, l - 1], dp[i, l]);
                }
            }

            return dp[0, k];
        }

        public static bool[] IsArraySpecial(int[] nums, int[][] queries) {
            int n = nums.Length;
            int[] pre = new int[n];
            int l, next = 0;
            bool flag = false;
            while ((l = next) < n) {
                int r = l + 1;
                while (r < n && nums[r] % 2 != nums[r - 1] % 2) {
                    r++;
                }

                next = r--;

                Array.Fill(pre, flag ? 1 : 3, l, r - l + 1);
                flag = !flag;

            }

            for (int i = 1; i < n; i++) {
                pre[i] += pre[i - 1];
            }

            bool[] res = new bool[queries.Length];
            for (int i = 0; i < res.Length; i++) {
                var x = queries[i];
                int sum =
                    pre[x[1]] - (x[0] != 0 ? pre[x[0] - 1] : 0);
                res[i] = sum == x[1] - x[0] + 1 || sum == (x[1] - x[0] + 1) * 3;
            }

            return res;
        }

        public static long MinEnd(int n, int x) {
            int digit = 1;
            long n2 = n - 1;
            while (digit <= x) {
                if ((x & digit) != 0) {
                    var remain = n2 & (digit - 1);
                    n2 = ((n2 - remain) << 1) + remain;
                }
                digit <<= 1;
            }

            return n2 | x;
        }

        public static int Search2(int[] nums, int target) {
            int n = nums.Length;
            int l = -1, r = n - 1;
            while (l < r) {
                int m = (l + r + 1) >> 1;
                if (nums[m] < nums[0]) {
                    r = m - 1;
                } else {
                    l = m;
                }
            }

            int res = -1;
            Find(0, l);
            Find(l + 1, n - 1);


            return res;

            void Find(int l, int r) {
                // l = 0;
                // r = right;
                while (l <= r) {
                    int m = (l + r) >> 1;
                    if (nums[m] < target) {
                        l = m + 1;
                    } else if (nums[m] > target) {
                        r = m - 1;
                    } else {
                        res = m;
                        return;
                    }
                }
            }

        }

        public static int MaxNumOfMarkedIndices(int[] nums) {
            int n = nums.Length;
            int res = 0;
            Array.Sort(nums);

            int last = n - 1;
            for (int i = n - 2; i >= 0; i--) {
                if (nums[last] >= (nums[i] << 1)) {
                    last--;
                    res += 2;
                }
            }

            return Math.Min(n / 2 * 2, res);
        }

        public static int MaximumRobots(int[] chargeTimes, int[] runningCosts, long budget) {
            int n = chargeTimes.Length;
            // T T F F F
            int l = -1, r = n;
            while (l < r) {
                int m = (l + r + 1) >> 1;
                if (check(m)) {
                    l = m;
                } else {
                    r = m - 1;
                }
            }

            return l;


            bool check(int k) {
                if (k == 0) {
                    return true;
                }

                LinkedList<int> que = new();
                // 递减队列
                long sum = 0;
                for (int i = 0; i < k; i++) {
                    push(que, i);
                    sum += runningCosts[i];
                }
                for (int i = 0; i < n - k + 1; i++) {
                    if (chargeTimes[que.First.Value] + k * sum <= budget) {
                        return true;
                    }

                    if (i + k >= n) {
                        return false;
                    }
                    push(que, i + k);
                    sum -= runningCosts[i];
                    sum += runningCosts[i + k];
                    PopAndMax(que, i);
                }

                return false;
            }

            void push(LinkedList<int> que, int i) {
                while (que.Count > 0 && chargeTimes[que.Last.Value] <= chargeTimes[i]) {
                    que.RemoveLast();
                }

                que.AddLast(i);
            }

            int PopAndMax(LinkedList<int> que, int i) {
                if (que.Count == 0) {
                    throw new Exception();
                }

                if (que.First.Value == i) {
                    var tmp = que.First.Value;
                    que.RemoveFirst();
                    return tmp;
                } else {
                    return que.First.Value;
                }
            }
        }

        public static string RemoveStars(string s) {
            List<char> stack = new();
            int top = 0;// 顶层的下一个元素的下标
            int n = s.Length;
            for (int i = 0; i < n; i++) {
                var x = s[i];
                if (x == '*') {
                    top--;
                } else {
                    if (stack.Count <= top) {
                        stack.Add(x);
                    } else {
                        stack[top] = x;
                    }
                    top++;
                }
            }

            return string.Concat(stack[..top]);
        }

        public static int MinimizeSum(int[] nums) {
            Array.Sort(nums);
            return Math.Min(Math.Min(
                    Math.Abs(nums[^1] - nums[2]), Math.Abs(nums[0] - nums[^3]))
                , Math.Abs(nums[^2] - nums[1]));
        }

        public static int MaxOperations(string s) {
            // 策略：每次都移动最左侧的1
            int cnt = 0;
            int res = 0;
            for (int i = 0; i < s.Length; i++) {
                if (s[i] == '1') {
                    cnt++;
                } else if (i > 0 && s[i - 1] == '1') {
                    res += cnt;
                }
            }

            return res;
        }

        public static int NumTilings(int n) {
            int[,] dp = new int[n, 3];
            // 0: 平整
            // 1: 第一行凸出一块
            // 2: 第二行凸出一块
            dp[n - 1, 0] = 1;
            dp[n - 1, 1] = dp[n - 1, 2] = 0;
            const int MOD = 1000000007;
            for (int i = n - 2; i >= 0; i--) {
                // 填充两个横着的二连块
                dp[i, 0] = (dp[i, 0] % MOD + (i + 2 < n ? dp[i + 2, 0] : 1) % MOD) % MOD;
                // 填充一个横着的二连块
                dp[i, 1] = (dp[i, 1] % MOD + (i + 1 < n ? dp[i + 1, 2] : 1) % MOD) % MOD;
                dp[i, 2] = (dp[i, 2] % MOD + (i + 1 < n ? dp[i + 1, 1] : 1) % MOD) % MOD;
                // 竖着填充一个二连块
                dp[i, 0] = (dp[i, 0] % MOD + dp[i + 1, 0] % MOD) % MOD;
                // 填充一个指向左下角的三连块
                dp[i, 0] = (dp[i, 0] % MOD + (dp[i + 1, 2] % MOD)) % MOD;
                // 填充一个指向左上角的三连块
                dp[i, 0] = (dp[i, 0] % MOD + dp[i + 1, 1] % MOD) % MOD;
                // 填充一个指向右上角、右下角的三连块
                dp[i, 1] = (dp[i, 1] % MOD + (i + 2 < n ? dp[i + 2, 0] : 1)) % MOD;
                dp[i, 2] = (dp[i, 2] % MOD + (i + 2 < n ? dp[i + 2, 0] : 1) % MOD) % MOD;
            }
            return dp[0, 0] % MOD;
        }

        public static double MinAreaFreeRect(int[][] points) {
            double res = double.MaxValue;
            HashSet<(int, int)> set = [];
            int n = points.Length;
            for (int i = 0; i < n; i++) {
                set.Add((points[i][0], points[i][1]));
            }

            for (int i = 0; i < n; i++) {
                for (int j = i + 1; j < n; j++) {
                    for (int k = j + 1; k < n; k++) {
                        var s = calcS(points[i], points[j], points[k]);
                        if (s == -1) {
                            continue;
                        }
                        res = Math.Min(res, s);
                    }
                }
            }

            return res == double.MaxValue ? 0 : res;


            double calcS(int[] a, int[] b, int[] c) {
                Vector2 ba = new([a[0] - b[0], a[1] - b[1]]);
                Vector2 cb = new([b[0] - c[0], b[1] - c[1]]);
                Vector2 ac = new([c[0] - a[0], c[1] - a[1]]);
                if (Vector2.Dot(ba, cb) == 0) {
                    var bc = Vector2.Multiply(-1, cb);
                    int[] d = [a[0] + (int)bc[0], a[1] + (int)bc[1]];
                    if (set.Contains((d[0], d[1]))) {
                        return getLen(ba) * getLen(cb);
                    }
                } else if (Vector2.Dot(ba, ac) == 0) {
                    int[] d = [b[0] + (int)ac[0], b[1] + (int)ac[1]];
                    if (set.Contains((d[0], d[1]))) {
                        return getLen(ba) * getLen(ac);
                    }

                } else if (Vector2.Dot(cb, ac) == 0) {
                    int[] d = [a[0] + (int)cb[0], a[1] + (int)cb[1]];
                    if (set.Contains((d[0], d[1]))) {
                        return getLen(cb) * getLen(ac);
                    }
                }
                return -1;

            }

            double getLen(Vector2 v) {
                return Math.Sqrt(v[0] * (double)v[0] + v[1] * v[1]);
            }
        }

        public static int[] FindRedundantConnection(int[][] edges) {
            int n = edges.Length;
            int[] inDegree = new int[n + 1];
            List<int[]> candidates = [];

            // 计算入度和出度
            foreach (var edge in edges) {
                inDegree[edge[1]]++;
            }

            for (int i = 0; i < n; i++) {
                if (inDegree[edges[i][1]] == 2) {
                    candidates.Add(edges[i]);
                }
            }
            // 情况1：存在入度为2的节点
            if (candidates.Count > 0) {
                int[] edge1 = candidates[0];
                int[] edge2 = candidates[1];
                bool one = tryEdge(edge1);
                bool two = tryEdge(edge2);
                if (!one) {
                    return edge2;
                }

                if (!two) {
                    return edge1;
                }

                return edge2;
            }

            bool tryEdge(int[] e) {
                int[] pa = new int[n + 1];
                for (int i = 1; i <= n; i++) {
                    pa[i] = i; // 重置并查集
                }

                foreach (int[] edge in edges) {
                    if (edge[0] == e[0] && edge[1] == e[1]) {

                    } else {
                        Union(edge[0], edge[1]);
                    }
                }

                if (Find(e[0]) == Find(e[1])) {
                    return true;
                }

                return false;

                int Find(int x) {
                    if (pa[x] != x) {
                        pa[x] = Find(pa[x]); // 路径压缩
                    }
                    return pa[x];
                }

                void Union(int x, int y) {
                    pa[Find(x)] = Find(y);
                }
            }


            // 情况2：不存在入度为2的节点，寻找环
            var graph = new Dictionary<int, List<int>>();
            foreach (var edge in edges) {
                if (!graph.ContainsKey(edge[0])) {
                    graph[edge[0]] = new List<int>();
                }
                graph[edge[0]].Add(edge[1]);
            }

            var visited = new bool[n + 1];
            Array.Fill(visited, false);
            var cycleEdges = new HashSet<(int, int)>();
            foreach (var edge in edges.Reverse()) {
                if (DetectCycle(edge[0], graph, [], visited, cycleEdges)) {
                    break;
                }
            }
            // 寻找最后一个出现的边
            for (int i = n - 1; i >= 0; i--) {
                if (cycleEdges.Contains((edges[i][0], edges[i][1]))) {
                    return edges[i];
                }
            }



            return [];

            bool DetectCycle(int node, Dictionary<int, List<int>> graph, HashSet<int> path, bool[] visited, HashSet<(int, int)> cycleEdges) {
                if (path.Contains(node))
                    return true;
                if (visited[node])
                    return false;

                visited[node] = true;
                path.Add(node);

                if (graph.ContainsKey(node)) {
                    foreach (var neighbor in graph[node]) {
                        cycleEdges.Add((node, neighbor));
                        if (DetectCycle(neighbor, graph, path, visited, cycleEdges)) {
                            return true;
                        }
                        cycleEdges.Remove((node, neighbor));
                    }
                }

                path.Remove(node);
                return false;
            }
        }

        public static bool HasValidPath(char[][] grid) {
            int w = grid.Length;
            int h = grid[0].Length;

            if ((w + h - 1) % 2 != 0) {
                return false;
            }
            Dictionary<(int, int, int), bool> map = [];

            return f(0, 0, 0);


            bool f(int x, int y, int val) {
                if (val > 0) {
                    return false;
                }
                if (map.TryGetValue((x, y, val), out bool res)) {
                    return res;
                }
                int cur;
                if (grid[x][y] == '(') {
                    cur = 1;
                } else {
                    cur = -1;
                }

                if (x == w - 1 && y == h - 1) {
                    return val == (grid[x][y] == ')' ? -1 : 1);
                }

                if (check(x + 1, y)) {
                    if (f(x + 1, y, val - cur)) {
                        return map[(x,y,val)] = true;
                    }
                }

                if (check(x, y + 1)) {
                    if (f(x, y + 1, val - cur)) {
                        return map[(x,y,val)] = true;
                    }
                }

                return map[(x, y, val)] = false;
            }

            bool check(int x, int y) {
                return x >= 0 && x < w && y >= 0 && y < h;
            }
        }

        public static int MaxValueOfCoins(IList<IList<int>> piles, int k) {
            int n = piles.Count;
            Dictionary<(int, int), int> map = [];
            return dfs(k, 0);

            // 从第j个开始，选择k个
            int dfs(int k, int j) {
                if (j == n || k == 0) {
                    return 0;
                }

                if (map.TryGetValue((k, j), out int res)) {
                    return res;
                }

                int sum = 0;
                int max = 0;
                max = Math.Max(max, dfs(k, j + 1));
                for (int i = 0; i < Math.Min(k, piles[j].Count); i++) {
                    sum += piles[j][i];
                    max = Math.Max(max, dfs(k - i - 1, j + 1) + sum);
                }

                return map[(k, j)] = max;
            }
        }
        public static int FindLatestStep(int[] arr, int m) {
            int n = arr.Length;
            (int l, int r)[] lr = new (int, int)[n + 1];
            Array.Fill(lr, (-1, -1));
            int res = -1;
            int j = 1;
            foreach (int i in arr) {
                lr[i] = (i, i);
                if (i + 1 <= n && lr[i + 1].r != -1) {
                    lr[i].r = lr[i + 1].r;
                }

                if (i > 1 && lr[i - 1].l != -1) {
                    lr[i].l = lr[i - 1].l;
                }

                if (i + 1 <= n && lr[i + 1].r != -1) {
                    // 合并
                    if (lr[i + 1].r - lr[i + 1].l + 1 == m) {
                        res = j - 1;
                    }
                    lr[lr[i + 1].r].l = lr[i].l;
                }

                if (i > 1 && lr[i - 1].l != -1) {
                    // 合并
                    if (lr[i - 1].r - lr[i - 1].l + 1 == m) {
                        res = j - 1;
                    }
                    lr[lr[i - 1].l].r = lr[i].r;
                }

                if (lr[i].r - lr[i].l + 1 == m) {
                    res = j;
                }

                j++;
            }

            return res;
        }

        public static int MaxDistance(string s, int k) {
            // 贪心法
            int x = 0, y = 0;
            int n = s.Length;
            int cur;
            int res = 0;
            for (int i = 0; i < s.Length; i++) {
                char c = s[i];
                switch (c) {
                    case 'N':
                        y++;
                        break;
                    case 'S':
                        y--;
                        break;
                    case 'E':
                        x++;
                        break;
                    case 'W':
                        x--;
                        break;
                }

                // 什么也不改时，当前的曼哈顿距离：
                cur = Math.Abs(x) + Math.Abs(y);
                // 在这个当前距离的基础上，可以进行k次更改，每更改一次就加2，即+2k；
                // 但不能超过i+1步。
                cur = Math.Min(i + 1, cur + 2 * k);
                res = Math.Max(res, cur);
            }

            return res;
        }

        public static bool CanArrange(int[] arr, int k) {
            for (int i = 0; i < arr.Length; i++) {
                arr[i] %= k;
                arr[i] = (arr[i] + k) % k;
            }
            Dictionary<int, int> freq = [];
            for (int i = 0; i < arr.Length; i++) {
                if (!freq.ContainsKey(arr[i])) {
                    freq[arr[i]] = 0;
                }

                freq[arr[i]]++;
            }

            foreach (var i in freq.Keys) {
                if (i != 0 && (!freq.ContainsKey(k - i) || freq[i] != freq[k - i])) {
                    return false;
                }
            }

            return !freq.ContainsKey(0) || freq[0] % 2 == 0;
        }

        public static int MinZeroArray(int[] nums, int[][] queries) {
            int n = queries.Length;

            // 二分查找的边界
            int left = 0, right = n;

            // 二分查找
            while (left < right) {
                int mid = (left + right) / 2;
                // 检查前mid个查询是否能将nums变成零数组
                if (CanZeroArray(nums, queries, mid)) {
                    right = mid;  // 如果能变成零数组，尝试更小的k
                } else {
                    left = mid + 1;  // 否则尝试更大的k
                }
            }

            // 判断最终结果
            return CanZeroArray(nums, queries, left) ? left : -1;


            static bool CanZeroArray(int[] nums, int[][] queries, int k) {
                int m = nums.Length;
                int[] d = new int[m + 1];  // 差分数组

                // 处理前k个查询
                for (int i = 0; i < k; i++) {
                    int l = queries[i][0];
                    int r = queries[i][1];
                    int val = queries[i][2];

                    d[l] += val;
                    if (r + 1 < m)
                        d[r + 1] -= val;
                }

                // 使用前缀和更新数组
                int currentDiff = 0;
                for (int i = 0; i < m; i++) {
                    currentDiff += d[i];
                    if (nums[i] - currentDiff > 0) {
                        return false;  // 如果当前元素不能通过操作使其为零，则返回false
                    }
                }

                return true;
            }
        }

        public static int CountOfPairs(int[] nums) {
            int n = nums.Length;
            long sum = 0;
            const int mod = 10_0000_0007;
            Dictionary<(int, int), long> map = [];
            for (int i = 0; i <= nums[0]; i++) {
                var tmp = Dfs(0, i);
                if (tmp != -1) {
                    sum = (sum % mod + tmp % mod) % mod;
                }
            }

            return (int)sum;

            long Dfs(int start, int curNum) {
                int curSum = nums[start];
                if (curSum - curNum < 0) {
                    return -1;
                }

                if (start == n - 1) {
                    return 1;
                }

                if (map.TryGetValue((start, curNum), out var tmp)) {
                    return tmp;
                }

                long res = 0;
                // 枚举下一个
                if (curNum > nums[start + 1]) {
                    return -1;
                }
                for (int nextNum = curNum; nextNum <= nums[start + 1]; nextNum++) {
                    // 检查是否递减
                    var nextNum2 = nums[start + 1] - nextNum;
                    if (nextNum2 > nums[start] - curNum) {
                        continue;
                    }
                    var nextRes = Dfs(start + 1, nextNum);
                    if (nextRes != -1) {
                        res = (res % mod + nextRes % mod) % mod;
                    }
                }

                return map[(start, curNum)] = res;
            }
        }

        public static int MaxDistinctElements(int[] nums, int k) {
            if (nums.Length <= 2 * k + 1)
                return nums.Length;
            Array.Sort(nums);
            int a = -k; // 从-k开始上涨
            int b = k; // 下降
            int l = 0, r = nums.Length - 1;
            HashSet<int> set = [];
            while (l <= r) {
                if (l == 0) {
                    set.Add(nums[l] + a);
                } else {
                    a = Math.Min(Math.Max(-k, nums[l - 1] + a - nums[l] + 1), k); // 尽量贴边
                    set.Add(nums[l] + a);
                }

                if (l == r) {
                    break;
                }

                if (r == nums.Length - 1) {
                    set.Add(nums[r] + b);
                } else {
                    b = Math.Max(Math.Min(k, nums[r + 1] + b - nums[r] - 1), -k); // 尽量贴边
                    set.Add(nums[r] + b);
                }
                set.Add(nums[r] + b);


                r--;
                l++;
            }

            return set.Count;
        }

        public static int MinDifference(int[] nums) {
            int n = nums.Length;
            if (n <= 4) {
                return 0;
            }
            Array.Sort(nums);
            int res = int.MaxValue;
            for (int i = 0; i <= 3; i++) {
                res = Math.Min(res, nums.Skip(i).Take(n - 3).Max() - nums.Skip(i).Take(n - 3).Min());
            }

            return res;
        }

        public static  long MaxWeight(int[] pizzas) {
            Array.Sort(pizzas);
            int n = pizzas.Length;
            long res = 0;
            for (int i = 0; i < (n / 4 + 1) / 2; i++) {
                res += pizzas[^(i + 1)];
            }

            for (int i = 1; i <= n / 4 - (n / 4 + 1)/ 2; i++) {
                res += pizzas[^((n / 4 + 1)/ 2 + 2 * i)];
            }

            return res;
        }

        public static int MinimumEffort(int[][] tasks) {
            Array.Sort(tasks, (a, b) => {
                int x = b[1] - b[0] - (a[1] - a[0]);
                if (x != 0) {
                    return x;
                } else {
                    return b[1] - a[1];
                }
            });

            int initial = 0;
            int cur = 0;
            for (int i = 0; i < tasks.Length; i++) {
                initial += Math.Max(0, tasks[i][1] - cur);
                cur = tasks[i][1] - cur <= 0 ? cur - tasks[i][0] : tasks[i][1] - tasks[i][0];
            }
            return initial;
        }

        public static IList<string> GetWordsInLongestSubsequence(string[] words, int[] groups) {
            int n = words.Length;
            int[] dp = new int[n];
            Array.Fill(dp, 1);
            int[] map = new int[n];
            Array.Fill(map, -1);
            for (int i = 1; i < n; i++) {
                for (int j = 0; j < i; j++) {
                    if (groups[i] != groups[j] && IsValid(words[i], words[j])) {
                        if (dp[j] + 1 >= dp[i]) {
                            dp[i] = dp[j] + 1;
                            map[i] = j;
                        }
                    }
                }
            }

            int max = 1;
            int start = 0;
            for (int i = 0; i < n; i++) {
                if (dp[i] > max) {
                    max = dp[i];
                    start = i;
                }
            }

            List<string> ls = [words[start]];
            while (map[start] != -1) {
                start = map[start];
                ls.Add(words[start]);
            }

            ls.Reverse();
            return ls;

            bool IsValid(string s1, string s2) {
                if (s1.Length != s2.Length) {
                    return false;
                }

                int cnt = 0;
                for (int i = 0; i < s1.Length; i++) {
                    if (s1[i] != s2[i]) {
                        cnt++;
                    }
                }

                return cnt == 1;
            }
        }

        public static string ClearStars(string s) {
            int n = s.Length;
            int[] pos = new int[n];
            PriorityQueue<int, CharacterWithIndex> pq = new PriorityQueue<int, CharacterWithIndex>();
            for (int i = 0; i < n; i++) {
                if (s[i] != '*') {
                    pq.Enqueue(i, new(i, s[i]));
                    pos[i] = 1;
                } else {
                    var index = pq.Dequeue();
                    pos[index] = 0;
                }
            }

            StringBuilder sb = new();
            for (int i = 0; i < n; i++) {
                if (pos[i] == 1) {
                    sb.Append(s[i]);
                }
            }

            return sb.ToString();
        }

        public struct CharacterWithIndex(int index, char character) : IComparable<CharacterWithIndex> {
            public int Index = index;
            public char Character { get; set; } = character;

            public int CompareTo(CharacterWithIndex other) {
                if (this.Character < other.Character) {
                    return -1;
                }
                if (this.Character > other.Character) {
                    return 1;
                }
                if (this.Index == other.Index) {
                    return 0;
                }
                return this.Index > other.Index ? -1 : 1;
            }
        }

        public static int UniqueXorTriplets(int[] nums) {
            HashSet<int> set = [];
            int n = nums.Length;
            for (int i = 0; i < n; i++) {
                for (int j = i; j < n; j++) {
                    set.Add(nums[i] ^ nums[j]);
                }
            }

            HashSet<int> resSet = [];

            for (int i = 0; i < n; i++) {
                foreach (var num in set) {
                    resSet.Add(nums[i] ^ num);
                }
            }

            return resSet.Count();
        }

    }
}
