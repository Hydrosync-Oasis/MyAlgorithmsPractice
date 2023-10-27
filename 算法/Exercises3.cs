using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

    }
}
