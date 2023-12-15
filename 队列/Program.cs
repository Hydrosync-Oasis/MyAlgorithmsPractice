using System.Diagnostics;

namespace 队列 {
    internal class Program {
        static void Main(string[] args) {
            IQueue<int> que = new Queue2<int>();
            Queue<int> right = new();
            Random ran = new();
            for (int i = 0; i < 50000000; i++) {
                if (que.Count != right.Count) {
                    throw new Exception();
                }
                int state = ran.Next(0, 2);
                if (state == 0) {
                    var a = ran.Next();
                    que.Enqueue(a);
                    right.Enqueue(a);
                    
                } else {
                    if (que.Count != 0) {
                        var a = que.Dequeue();
                        var b = right.Dequeue();
                        if (a != b) {
                            throw new Exception();
                        }
                    }
                }
            }
        }

        public static TimeSpan TestTime(IQueue<int> queue, int num) {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            for (int i = 0; i < num; i++) {
                queue.Enqueue(i);
            }

            for (int i = 0; i < num; i++) {
                queue.Dequeue();
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
