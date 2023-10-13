using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Algo4 {
    internal class _3x3Search {
        public delegate int EvaluationFunc(State state);
        EvaluationFunc func;
        public static State rightState = new((int[][])[[1, 2, 3], [4, 5, 6], [7, 8, 0]]);


        public class State {
            private int[] state = new int[9];
            public State(int[][] state) {
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        this[i, j] = state[i][j];
                    }
                }
            }

            public State(int[] state) {
                this.state = state;
            }

            public long GetCode() {
                long number = 0;
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        number *= 10;
                        number += this[i, j];
                    }
                }
                return number;
            }

            public List<State> GenNext() {
                int x = -1, y = -1;
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
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
                    (this[x, y], this[x - 1, y]) = (this[x - 1, y], this[x, y]);
                }
                if (x != 2) {
                    (this[x, y], this[x + 1, y]) = (this[x + 1, y], this[x, y]);
                    ans.Add(this.Copy());
                    (this[x, y], this[x + 1, y]) = (this[x + 1, y], this[x, y]);
                }
                if (y != 0) {
                    (this[x, y], this[x, y - 1]) = (this[x, y - 1], this[x, y]);
                    ans.Add(this.Copy());
                    (this[x, y], this[x, y - 1]) = (this[x, y - 1], this[x, y]);
                }
                if (y != 2) {
                    (this[x, y], this[x, y + 1]) = (this[x, y + 1], this[x, y]);
                    ans.Add(this.Copy());
                    (this[x, y], this[x, y + 1]) = (this[x, y + 1], this[x, y]);
                }
                return ans;
            }

            public static implicit operator State(int[][] input) => new(input);

            public int this[int x, int y] {
                get => state[3 * x + y];
                set => state[3 * x + y] = value;
            }

            public override int GetHashCode() => GetCode().GetHashCode();

            public State Copy() {
                int[] copy = new int[9];
                state.CopyTo(copy, 0);
                return new(copy);
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
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        sb.Append(this[i, j]);
                        sb.Append(' ');
                    }
                    sb.AppendLine();
                }
                return sb.ToString();
            }
        }

        State state;

        public _3x3Search(int[][] state, EvaluationFunc func) {
            this.state = state;
            this.func = func;
        }

        public List<State> Search() {
            Dictionary<State, State> map = new();
            PriorityQueue<State, int> pq = new();
            HashSet<State> visited = new HashSet<State>() { state };
            pq.Enqueue(state, func(state));
            while (pq.Count > 0) {
                var tmp = pq.Dequeue();
                if (tmp.Equals(rightState)) {
                    break;
                }

                List<State> nxt = tmp.GenNext();
                foreach (var item in nxt) {
                    if (visited.Add(item)) {
                        map[item] = tmp;
                        pq.Enqueue(item, func(item));
                    }
                }
            }

            List<State> result = new();
            State cur = rightState;
            while (true) {
                result.Add(cur);
                if (!map.ContainsKey(cur)) {
                    break;
                }
                cur = map[cur];
            }
            result.Reverse();
            return result;
        }
    }
}
