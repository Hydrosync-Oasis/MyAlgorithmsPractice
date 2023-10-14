using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Algo4 {
    internal class Klotski {
        public delegate int EvaluationFunc(State state);
        EvaluationFunc func;
        public State rightState;
        int N { get => state.N; }

        /// <summary>
        /// 表示当前华容道的棋盘状态
        /// </summary>
        public class State {
            /// <summary>
            /// 表明该状态的阶数
            /// </summary>
            public int N { get; private set; }
            public int Step { get; set; }

            private int[] state;

            public State(int[][] state) {
                N = state.Length;
                this.state = new int[N * N];
                for (int i = 0; i < N; i++) {
                    for (int j = 0; j < N; j++) {
                        this[i, j] = state[i][j];
                    }
                }
            }

            public State(int[] state) {
                N = (int)Math.Sqrt(state.Length);
                this.state = state;
            }

            public int GetCode() {
                StringBuilder sb = new();
                for (int i = 0; i < state.Length; i++) {
                    sb.Append(state[i]);
                    sb.Append(',');
                }
                return sb.ToString().GetHashCode();
            }

            public List<State> GenNext() {
                int x = -1, y = -1;
                for (int i = 0; i < N; i++) {
                    for (int j = 0; j < N; j++) {
                        if (this[i, j] == 0) {
                            x = i;
                            y = j;
                            break;
                        }
                    }
                }
                List<State> ans = new();
                if (x != 0) {
                    (this[x, y], this[x - 1, y]) = (this[x - 1, y], this[x, y]);
                    ans.Add(this.Copy());
                    ans[^1].Step++;
                    (this[x, y], this[x - 1, y]) = (this[x - 1, y], this[x, y]);
                }
                if (x != N - 1) {
                    (this[x, y], this[x + 1, y]) = (this[x + 1, y], this[x, y]);
                    ans.Add(this.Copy());
                    ans[^1].Step++;

                    (this[x, y], this[x + 1, y]) = (this[x + 1, y], this[x, y]);
                }
                if (y != 0) {
                    (this[x, y], this[x, y - 1]) = (this[x, y - 1], this[x, y]);
                    ans.Add(this.Copy());
                    ans[^1].Step++;

                    (this[x, y], this[x, y - 1]) = (this[x, y - 1], this[x, y]);
                }
                if (y != N - 1) {
                    (this[x, y], this[x, y + 1]) = (this[x, y + 1], this[x, y]);
                    ans.Add(this.Copy());
                    ans[^1].Step++;

                    (this[x, y], this[x, y + 1]) = (this[x, y + 1], this[x, y]);
                }
                return ans;
            }

            public static implicit operator State(int[][] input) => new(input);

            public int this[int x, int y] {
                get => state[N * x + y];
                set => state[N * x + y] = value;
            }

            public override int GetHashCode() => GetCode().GetHashCode();

            public State Copy() {
                int[] copy = new int[N * N];
                state.CopyTo(copy, 0);
                State c = new State(copy);
                c.Step = Step;
                return c;
            }

            public override bool Equals(object? obj) {
                if (obj is State s) {
                    return state.SequenceEqual(s.state);
                } else {
                    return false;
                }
            }

            public override string ToString() {
                StringBuilder sb = new();
                for (int i = 0; i < N; i++) {
                    for (int j = 0; j < N; j++) {
                        sb.Append(this[i, j]);
                        sb.Append('\t');
                    }
                    sb.AppendLine();
                }
                return sb.ToString();
            }
        }

        State state;

        public Klotski(State state, EvaluationFunc func) {
            this.state = state;
            this.func = func;
            int[] sta = new int[N * N];
            for (int i = 0; i < N * N - 1; i++) {
                sta[i] = i + 1;
            }
            rightState = new(sta);
        }

        public List<State> Search() {
            Dictionary<State, State> map = new();
            PriorityQueue<State, int> pq = new();
            HashSet<State> closed = new() { };

            Dictionary<State, int> dist = new() { [state] = func(state) };
            pq.Enqueue(state, dist[state]);
            while (pq.Count > 0) {
                var tmp = pq.Dequeue();
                if (closed.Contains(tmp)) { // 延迟删除
                    continue;
                }
                if (tmp.Equals(rightState)) { // 达到了终点状态
                    break;
                }

                closed.Add(tmp);

                List<State> nxt = tmp.GenNext();
                foreach (var item in nxt) {
                    if (closed.Contains(item)) {
                        continue;
                    }
                    int f = func(item);
                    if (!dist.TryGetValue(item, out int value) || value > f + tmp.Step + 1) {
                        map[item] = tmp;
                        dist[item] = f + tmp.Step + 1;
                        pq.Enqueue(item, f + tmp.Step + 1);
                    }

                }
            }

            List<State> result = new();
            State cur = rightState;
            while (true) {
                result.Add(cur);
                if (!map.TryGetValue(cur, out State s)) {
                    break;
                } else {
                    cur = s;
                }
            }
            result.Reverse();
            return result;
        }
    }
}
