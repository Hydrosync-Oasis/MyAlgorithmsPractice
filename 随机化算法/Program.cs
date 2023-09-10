using 排序;

namespace 随机化算法 {
    internal class Program {
        static (int, int) pos;
        static int[] arr;

        static void Main(string[] args) {
            Console.BufferWidth = 300;
            arr = ArraySort.GenerateMonotoArr(200);
            Console.Write(ArraySort.VisualizationArr(arr.ToList()));
            pos = Console.GetCursorPosition();
            int ans = SA(arr);
            Console.WriteLine();
            Console.WriteLine($"{arr[ans]}");
        }

        static int SA(int[] arr) {
            Random ran = new();
            const double d = 0.999;
            const double T0 = 100d;
            double T = T0;
            const double Tk = 0.1d;
            int x = ran.Next(0, arr.Length);
            int l = 0, r = arr.Length - 1;

            while (T > Tk) {
                int x0 = x;
                double increasement = T / T0 * ((arr.Length << 1) * ran.Next(0, 100) / 100.0 - (arr.Length));
                x += (int)increasement;
                if (x < l) {
                    x = l;
                } else if (x > r) {
                    x = r;
                }
                double delta = Math.Abs(arr[x] - arr[x0]);
                if (arr[x] < arr[x0]) {
                    double p = Math.Exp(-Math.Abs(delta) / T);
                    if (ran.Next(0, 1000) / 1000.0 < p) {
                        //接受
                    } else {//拒绝
                        x = x0;
                    }
                } else {
                    //接受
                }
                DrawArrow(x);
                Console.SetCursorPosition(0, pos.Item2 + 1);
                Console.WriteLine($"温度: {T}");
                //Thread.Sleep(1);

                T *= d;
            }

            return x;
        }

        static void DrawArrow(int x) {
            Console.SetCursorPosition(pos.Item1, pos.Item2);
            Console.Write(new string(' ', x));
            Console.Write('I');
            Console.Write(new string(' ', arr.Length - x - 1));
        }
    }
}