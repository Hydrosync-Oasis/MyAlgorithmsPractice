using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
