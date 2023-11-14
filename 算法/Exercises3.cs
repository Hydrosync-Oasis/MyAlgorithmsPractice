using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

    }
}

