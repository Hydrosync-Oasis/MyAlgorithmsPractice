using System.Diagnostics;

namespace 队列
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Queue1<int> queue1 = new();
            Queue2<int> queue2 = new();
            Queue3<int> queue3 = new();
            //Console.WriteLine(TestTime(queue1, 100000));
            Console.WriteLine(TestTime(queue2, 20000000));
            Console.WriteLine(TestTime(queue3, 20000000));
        }

        public static TimeSpan TestTime(IQueue<int> queue, int num)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            for (int i = 0; i < num; i++)
            {
                queue.Enqueue(i);
            }

            for (int i = 0; i < num; i++)
            {
                queue.Dequeue();
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}