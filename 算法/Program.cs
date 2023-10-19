using System.Diagnostics;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Algorithm {
    internal class Program {
        static void Main(string[] args) {
            //Stack<int> st = new(new int[] { 1, 2, 3, 4, 5 });
            //Exercises.ReverseStack(st);
            Stopwatch sw = Stopwatch.StartNew();

            //Test();
            var res3 = Exercises.TupleSameProduct([1, 2, 4, 5, 10]);
            sw.Stop();
            Console.WriteLine(res3);
            //Check();
            //Gen();
            Console.WriteLine(sw.Elapsed);
        }
        static void Input() {
            string[] nm = Console.ReadLine().Split(' ');
            int n = int.Parse(nm[0]);
            int m = int.Parse(nm[1]);

            int[][] land = new int[n][];
            for (int i = 0; i < n; i++) {
                land[i] = new int[m];
                string[] row = Console.ReadLine().Split(' ');
                for (int j = 0; j < m; j++) {
                    land[i][j] = int.Parse(row[j]);
                }
            }
            Dictionary<(int, int), long> map = new();
            long result = Calc(0, 0, n, m, land, map);

            Console.WriteLine(result);
        }
        static int[][] GenArr() {
            string filePath = "C:\\Users\\Silver Wind\\Desktop\\1.txt"; // 将 "path_to_your_file.txt" 替换为您的文件路径

            try {
                string content = File.ReadAllText(filePath);
                int[][] array = Parse2DArray(content);
                return array;
                // 输出解析后的二维数组
                Console.WriteLine("解析后的二维数组：");
                foreach (int[] row in array) {
                    Console.Write("[ ");
                    foreach (int element in row) {
                        Console.Write(element + " ");
                    }
                    Console.WriteLine("]");
                }
            } catch (Exception ex) {
                Console.WriteLine("发生错误：" + ex.Message);
            }
            return null;
        }

        static int[][] Parse2DArray(string input) {
            var pattern = @"\[([\d, \[\]-]*?)\]";
            var matches = Regex.Matches(input, pattern);

            var result = new List<int[]>();
            foreach (Match match in matches) {
                string matchContent = match.Groups[1].Value;
                int[] row = Parse1DArray(matchContent);
                result.Add(row);
            }

            return result.ToArray();
        }

        static int[] Parse1DArray(string input) {
            var pattern = @"([-?\d]*)";
            var matches = Regex.Matches(input, pattern);

            var result = new List<int>();
            foreach (Match match in matches) {
                if (int.TryParse(match.Groups[1].Value, out int element)) {
                    result.Add(element);
                }
            }

            return result.ToArray();
        }

        static void Test() {
            StringBuilder sb = new();
            for (int i = 0; i < 999; i++) {
                sb.Append(i);
                sb.Append('\n');
            }
            File.WriteAllText(@"C:\Users\Silver Wind\Desktop\ipt.txt", sb.ToString());
        }

        public static int MinOperations(int[] nums1, int[] nums2) {
            int n = nums1.Length, m = nums2.Length;
            if (6 * n < m || 6 * m < n) {
                return -1;
            }
            int[] cnt1 = new int[7];
            int[] cnt2 = new int[7];
            int diff = 0;
            foreach (int i in nums1) {
                ++cnt1[i];
                diff += i;
            }
            foreach (int i in nums2) {
                ++cnt2[i];
                diff -= i;
            }
            if (diff == 0) {
                return 0;
            }
            if (diff > 0) {
                return Help(cnt2, cnt1, diff);
            }
            return Help(cnt1, cnt2, -diff);
        }

        public static int Help(int[] h1, int[] h2, int diff) {
            int[] h = new int[7];
            for (int i = 1; i < 7; ++i) {
                h[6 - i] += h1[i];
                h[i - 1] += h2[i];
            }
            int res = 0;
            for (int i = 5; i > 0 && diff > 0; --i) {
                int t = Math.Min((diff + i - 1) / i, h[i]);
                res += t;
                diff -= t * i;
            }
            return res;
        }

        static ListNode? GenListNode(string s) {
            string[] sarr = s.Split(',');
            int[] arr = new int[sarr.Length];
            for (int i = 0; i < sarr.Length; i++) {
                arr[i] = int.Parse(sarr[i]);
            }
            ListNode res = new(-1);
            ListNode cur = res;
            for (int i = 0; i < arr.Length; i++) {
                cur.next = new(arr[i]);
                cur = cur.next;
            }
            return res.next;
        }

        static long Calc(int i, int state, int n, int m, int[][] land, Dictionary<(int, int), long> map) {
            if (i == n) {
                return 1;
            }
            if (map.ContainsKey((i, state))) {
                return map[(i, state)];
            }
            List<int> states = new List<int>();
            GenState(state, i, states, n, m, land);

            long res = 0;
            foreach (int nextState in states) {
                res += Calc(i + 1, nextState, n, m, land, map);
                res %= 100000000;  // 取模操作，防止结果溢出
            }
            map[(i, state)] = res;

            return res;
        }

        static void GenState(int lastState, int row, List<int> states, int n, int m, int[][] land) {
            // n行m列
            for (int i = 0; i < (1 << m); i++) {
                bool valid = true;

                for (int j = 0; j < m; j++) {
                    if ((lastState & (1 << j)) != 0 && (i & (1 << j)) > 0 || ((i & (1 << j)) > 0 && land[row][j] != 1)
                        || // 以下代码的作用：对于菜地的某一个格子，检测它的左右是不是被种地了
                        (i & (1 << j)) > 0 && (j > 0 && (i & (1 << (j - 1))) > 0 || j + 1 < m && (i & (1 << (j + 1))) > 0)
                        ) {
                        valid = false;
                        break;
                    }
                }

                if (valid) {
                    states.Add(i);
                }
            }
        }

        public static void Check() {
            //Random ran = new();`
            //List<int> arr = new();
            //for (int j = 0; j < 200; j++) {
            //    arr.Clear();
            //    int lower = ran.Next(1, 40);
            //    if (Exercises.MaxA(lower) != Exercises.MaxA2(lower)) {
            //        throw new Exception();
            //    }
            //}
            List<int> arg = new();
            List<int> arg2 = new();
            Random ran = new();
            for (int j = 0; j < 5000000; j++) {
                arg.Add(1);
                arg2.Add(1);
                for (int i = 0; i <= 10; i++) {
                    arg.Add(ran.Next(0, 2));
                    arg2.Add(ran.Next(0, 2));
                }
                arg.Add(ran.Next(2, 4));
                arg.Add(ran.Next(2, 4));
                var res = Exercises.AddNegabinary(arg.ToArray(), arg2.ToArray());
                foreach (var item in res) {
                    if (item < 0 || item >= 2) {
                        throw new Exception();
                    }
                }
                arg.Clear();
                arg2.Clear();
            }
        }

        public static void Gen() {
            string path = @"C:\Users\Silver Wind\Desktop\caseInversionPair";
            Random ran = new();
            for (int i = 1; i <= 5; i++) {
                int len = ran.Next(1, 1000);
                StringBuilder sb = new();
                sb.AppendLine(len.ToString());
                for (int j = 0; j < len - 1; j++) {
                    sb.AppendLine(ran.Next(0, 88888).ToString());
                }// 100 0000 0000
                sb.Append(ran.Next(0, 88888));
                File.WriteAllText($"{path}\\{i}.in", sb.ToString());
            }
        }

        public static int[][] GenerateArr(string expr) {
            //[0,9],[4,1],[5,7],[6,2],[7,4],[10,9]
            List<int[]> list = new();
            string[] arr = expr.Split(',');
            for (int i = 0; i < arr.Length; i += 2) {
                list.Add(new int[] { int.Parse(arr[i][1..]), int.Parse(arr[i + 1][..^1]) });
            }
            return list.ToArray();
        }

        public static long GetObjectByte<T>(T t) where T : class {
            DataContractJsonSerializer formatter = new(typeof(T));
            using MemoryStream stream = new();
            formatter.WriteObject(stream, t);
            return stream.Length;
        }

        static int[] map = { 0, 2, 5, 5, 4, 5, 6, 3, 7, 6 };

        static int Dfs(int startIdx, int n, int sticks) {
            if (startIdx == n) {
                return sticks >= map[n] ? 1 : 0;
            }

            int res = 0;
            //有两种选择，一种是拼成当前数字
            if (sticks - map[startIdx] >= 0) {
                int next = Dfs(startIdx + 1, n, sticks - map[startIdx]);
                res += next + 1;
            }
            //不拼当前数字
            res += Dfs(startIdx + 1, n, sticks);

            return res;
        }
        public static void TestAyeL(long[] arr) {
            long x = arr[0];
            int i = 1;
            int j = i;
            while (i < arr.Length) {
                if (arr[i] <= x) {
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                    j++;
                }
                i++;
            }
            //(arr[0], arr[j - 1]) = (arr[j - 1], arr[0]);
        }

        public static void CheckSort(long[] arr) {
            long x = arr[0];
            bool nextStep = false;
            for (int i = 1; i < arr.Length; i++) {
                if (!nextStep) {
                    if (arr[i] > x) {
                        nextStep = true;
                    }
                } else {
                    if (arr[i] < x) {
                        throw new Exception();
                    }
                }
            }
        }

        public static void Hanoi(int n, string from, string to, string other) {
            if (n >= 2) {
                Hanoi(n - 1, from, other, to);
                Console.WriteLine($"把第{n}个环从{from}柱移动到{to}柱");
                Hanoi(n - 1, other, to, from);
            } else {
                Console.WriteLine($"把第{n}个环从{from}柱移动到{to}柱");
            }
        }

        public static int Factorial(int status, int num) => num <= 1 ? status : Factorial(status * num, num - 1);

        public static int Search(int[] arr) {
            // 0 0 0 1 1 1 1 1 1
            int l = 0, r = arr.Length;  // 设计循环不变量，默认假想r处存在“1”，但是真正搜索的地方是[l..r-1]，r处相当于“反悔值”
                                        // 反悔值是[l..r-1]内没有1的时候可以选中的值，此时r即为结果作为返回值
            while (l < r) { // 因为正搜索的区间是[l..r-1]，所以必须要求l < r
                int m = (l + r) >> 1;
                // 二分法的本质是控制l，r两个变量排除不可能是答案的区间最终找到答案。
                // 下面的代码是在区间[l..r-1]这个实际区间中进行排除
                if (arr[m] == 0) {
                    l = m + 1; // 要查找的是第一个出现1的地方，l肯定不可能是1，要+1找下一个位置
                } else {
                    r = m; // m处是1实锤了，虽然不知道它是不是第一个1，但是不管怎样它都“可以”被我们当做是r，即可以反悔的值
                }
            }
            // 何时可以取到那个“假想的值”？当数组不断排除掉错误的区间时发现把所有可能的值全部排除了才会取到这个假想、不存在的值
            // 就是发现实际区间[l..r-1]内只有0，没有1的时候才会迫不得已取到r这个位置
            // 所以当数组全是0时，由于代码假想了下标是len的位置有1，所以程序也不会抛出异常，而是返回这个假想的不存在值
            return l;
        }
    }

}
