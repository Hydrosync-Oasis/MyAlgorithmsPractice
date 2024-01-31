using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

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

    }
}
