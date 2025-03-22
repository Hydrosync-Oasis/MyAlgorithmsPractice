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

        private readonly EvaluationFunc _func;
        public State TargetState;
        private int N { get => _state.N; }

        /// <summary>
        /// 表示当前华容道的棋盘状态
        /// </summary>
        public class State {
            /// <summary>
            /// 表明该状态的阶数
            /// </summary>
            public int N { get; private set; }
            public int Step { get; set; }

            private readonly int[] _state;

            public State(int[][] state) {
                N = state.Length;
                this._state = new int[N * N];
                for (int i = 0; i < N; i++) {
                    Array.Copy(state[i], 0, this._state, N * i, N);
                    // for (int j = 0; j < N; j++) {
                    //     this[i, j] = state[i][j];
                    // }
                }
            }

            public State(int[] state, int n) {
                N = n;
                this._state = state;
            }

            public int GetCode() {
                StringBuilder sb = new();
                for (int i = 0; i < _state.Length; i++) {
                    sb.Append(_state[i]);
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
                get => _state[N * x + y];
                set => _state[N * x + y] = value;
            }

            public override int GetHashCode() => GetCode().GetHashCode();

            public State Copy() {
                int[] copy = new int[N * N];
                _state.CopyTo(copy, 0);
                State c = new(copy, N) {
                    Step = Step
                };
                return c;
            }

            public override bool Equals(object? obj) {
                return obj is State s && _state.SequenceEqual(s._state);
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

        private readonly State _state;

        public Klotski(State state, EvaluationFunc func) {
            this._state = state;
            this._func = func;
            int[] sta = new int[N * N];
            for (int i = 0; i < N * N - 1; i++) {
                sta[i] = i + 1;
            }
            TargetState = new(sta, N);
        }

        public List<State> Search() {
            Dictionary<State, State> map = [];
            PriorityQueue<State, int> pq = new();
            HashSet<State> closed = [];

            Dictionary<State, int> dist = new() { [_state] = _func(_state) };
            pq.Enqueue(_state, dist[_state]);
            bool found = false;
            while (pq.Count > 0) {
                var tmp = pq.Dequeue();
                if (closed.Contains(tmp)) { // 延迟删除
                    continue;
                }
                if (tmp.Equals(TargetState)) { // 达到了终点状态
                    found = true;
                    break;
                }

                closed.Add(tmp);

                List<State> nxt = tmp.GenNext();
                foreach (var item in nxt) {
                    if (closed.Contains(item)) {
                        continue;
                    }
                    int f = _func(item);
                    if (!dist.TryGetValue(item, out int value) || value > f + tmp.Step + 1) {
                        map[item] = tmp;
                        dist[item] = f + tmp.Step + 1;
                        pq.Enqueue(item, f + tmp.Step + 1);
                    }

                }
            }

            List<State> result = new();
            if (!found) {
                return result;
            }
            State cur = TargetState;
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
