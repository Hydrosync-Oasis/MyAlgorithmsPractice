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
            var res3 = Exercises.SumDistance([10421, 535686, -577310, -553499, -446384, -214024, 433375, 482991, -134992, 488334, -538986, -495046, 235593, 469077, -175854, -258011, 105550, -33819, -262675, -527552, -572600, 295456, -570995, -272723, -226772, -805, 429160, -351177, -463359, -35635, -42218, 532797, 328108, 397721, -519551, -382316, 374672, -149682, -551360, -429581, 577121, -262244, -240933, -472284, -311647, 575937, -515348, 571070, 431488, 508682, -581561, 528184, 294618, 458063, -480161, 520654, 506387, 45202, -532471, 504453, 62212, -86197, -351839, -355186, -197206, 236586, -241362, -113048, -294682, -142178, 539933, -167605, -183969, 4042, -253632, -387325, -266191, -369563, -82026, -111758, -285366, 392160, -440295, -336066, 165370, -257198, -21327, -349883, 161336, 420300, 151952, -538676, -526180, 492660, -187458, -75064, -499415, 12995, -199088, -203047, 319206, 507945, 87339, -69057, 546163, 97641, -49327, -251455, 172750, -408543, -511831, -135448, 162504, 320036, 400270, 394198, -78558, 536092, -539557, 545814, -310633, 216965, 109096, -211067, 439355, -332107, -104653, 546759, 245372, -483196, 550737, -540599, -258516, -364869, -345323, 437309, 92676, -59957, -227323, -441134, -148657, -300253, -116205, 55479, -459449, -269787, -304272, -256586, -570802, 497174, 513753, -230465, 50327, -172955, -550822, -475817, 587138, -153441, -387182, -553339, 560009, 65247, 444673, 135420, -509690, 492398, 198089, -129229, -546313, 493402, -429792, -290510, -253252, -293591, 235755, -372590, 126760, 491670, 363788, 281735, 527688, -262211, -387086, 218518, 372942, 430089, -85166, -136439, -442631, 444942, 222378, -382502, 483845, 211997, -160464, 539794, 299625, 21495, 303000, 50598, 363647, -169734, 507160, -239002, 38905, -380255, -421730, 398512, 497697, -195994, -359741, 200484, 120892, 244454, -584256, -273069, -163032, -478946, -441271, -382870, -154119, -275001, 22798, 245623, -399550, 287023, 76378, -169737, 546609, 220057, -307094, -364841, 499324, -159330, -568679, 465161, -221706, 579306, 79993, 138311, -419992, 486358, -81176, -16117, 258250, -419364, -231481, 227683, -207085, 351215, 580870, -293665, -555527, 141857, 189480, -68788, -546102, -14871, -554917, 447765, -478627, 78241, -174224, 326800, 573073, -26316, -61998, -418186, 59570, 322281, -18540, 543431, -490520, -211079, -593702, -168196, 280596, -365957, 380525, 86350, -398705, -80021, -68042, -455482, -326453, 161828, 441446, -513844, 305710, 347467, -484546, -451888, 433152, 505827, 442371, -111490, 474818, 129197, 232666, 493296, -566406, -273795, -334127, 594412, -405121, 589993, -559967, 196306, 17649, 114052, -192167, 118413, -520174, 9095, 72997, -72327, 192948, -337742, -171602, -97266, -464709, -160740, 140213, -299483, 522012, 307883, 399166, 229470, -406582, -168788, 591476, -232937, -306744, 416391, -194439, 241535, 360093, 154576, 269829, 156728, -241586, -163873, -84086, 168965, 136489, 257752, 351340, 51964, -317544, 29576, -273908, -452286, 428847, -333711, -558388, -215598, 104658, 324973, -121800, -518718, -238016, 475075, -140504, 357590, 119459, 84831, 133621, -197236, -217846, -181933, -483140, 58375, 383558, 232105, -175795, -580121, -501145, 129886, 271616, -103045, -339857, 47161, -557374, -401535, -535796, 248529, -188638, 91766, 456064, -87809, -403877, 156909, -276961, 460881, 89217, 154442, -76425, 32038, -381836, -335586, 326399, -362485, -357178, -537569, -275730, -570632, -589394, -97176, 418366, 503319, 167716, -214835, 519746, 595712, -414845, 384612, -114654, -95411, 150576, 144676, -377374, 472251, 63285, 154117, -84932, 334399, 413746, -47127, -22361, -553407, -526034, 9624, 36756, 388852, -452388, 106364, 439729, 579180, 492276, 34799, 334241, -114782, 381049, -450353, 335365, -131514, 556661, -362147, -386226, 418799, -144668, -275345, 115318, 134563, -42432, 18623, -211442, -518693, -126957, -352327, -573198, 593733, -377303, 403255, -491208, 453333, 514995, 250277, -576142, 464818, -343631, -362597, 260797, -318935, 61494, 226302, -554440, 136885, -402182, -462896, 139167, -265132, -67914, -336666, -534159, -490950, -337600, -95504, 207026, -91921, 74887, 5457, -381568, -99837, -497088, -293348, 4609, 281644, -308310, 55378, 317985, 174232, -49098, 574370, 357005, -227748, 201844, 81284, 471257, 188629, -348594, -294110, 42526, 75635, -29806, -108748, -477927, -190381, 206736, -89130, 144579, 7742, 445984, -51715, 582270, 65443, -474534, -450300, -584597, 11965, -443980, 122479, 197067, 171684, -94662, 409242, -204539, 117235, -35341, -238544, -495931, 479765, 573233, 482142, 237933, -396344, 539422, 439392, -65002, 553778, -483116, 381807, 142362, 437692, -214768, -118291, -64387, -388124, 238859, 34644, -312891, 584207, 57772, -61654, 152487, -426099, -471821, -40319, -79806, 463536, 512696, 177623, -69736, -84054, -473591, 317920, -426926, -495351, 465778, -241357, -289633, 441161, 362187, 102256, 102931, -397589, -114568, 266556, 558474, 164613, -219023, 45935, 5949, -57197, -535702, 99969, -34121, -568937, -31773, -556003, -417630, 247361, -437020, -476065, 590294, 349583, -360608, 448647, 455933, 282533, 426527, -515754, 223269, 513916, 361165, -525785, -192643, 426738, -69872, -360710, 376086, -111926, -288619, -49688, -145979, -414745, -345609, 311806, 203814, -271386, -421091, 147989, -440359, -460888, 445283, -368640, -785, 368093, 68593, -457706, 489169, 172661, 402639, -39431, 517806, 587950, -21729, -128341, -190247, -65219, 304435, -8825, 412081, 367536, -311987, 514562, -205558, -88492, -405252, 453706, -381828, -372600, 205494, -78442, 309605, 437175, -288136, 382223, 331811, 137563, 89594, -285988, 484839, 281643, 139912, -229210, 427092, 462609, 29199, 415077, -354605, 334262, 323173, 101503, -26818, 589797, -503679, 579203, -216247, -7106, -491896, 81178, -265686, -542459, 448992, -356918, -329080, 443805, 366790, -8796, -67815, 200563, 508616, -355835, -103225, -245608, -444158, -477996, -112667, -556085, -345180, 377394, 103958, 31900, -167947, -482655, -115708, 64493, 588276, -341187, 7056, -487301, -64097, 336443, 293385, -546621, 393138, -15841, -158431, -481584, 403378, 488511, -279278, 189639, 80424, 335452, -265522, -306252, 373120, -534559, -386224, -69912, 724, -123838, -282688, 520230, -529204, 488479, -398197, 192476, -9829, 329387, 67180, 188864, 277775, -158610, -570005, -132065, 435511, 446656, -245811, -530643, 365696, 205509, -197716, 293128, -326248, -178340, -121648, -57220, 155213, 522129], "RRLRLRRLRRLRLLRLRLLRRLLLLRLRLRLLLRLLLLRRLLLLRRRLRLRRLRLRRRLRLRRLLLLLLLLRLRRRLRRRLRRLLRRRLRLRLLRRLRLRLLLLLLRRLLRRLRLLRLRRLLLLLRRLRLRRLRRRLLLRRLRRLRLRLRRRRRLRLRLRLRLRRRLRLRLRLRLRLLLRLLLLRRLRRRRLRRRLRRLRLLRRRLLLLRRLLRLRLLRLLRRRRRRRLLRRLRLLLRLRRLRLLRLRLRLLLLRRLLLRRRRRRRLRRRRRLRLLLRRRRLRLLLRLRRRLRLRLLRRRRRRLLRRRRRRRRRLRRRLLRLRRLRRRLLLLRLRRLRRLLRRRRLLRRLRLLRLRRLLRRLRRRLRRLLLLLRRRLRLRLLRLLLLRRLRRRLLLLRLRRLLRLRLLLRRRRLRRRLRRLLRRLLLRLRRLRLLLLLRLLRRRLLRRRRRRRRLLRLLRLLLRRRRRLLRLLRRRRLRRRRLRRLLLRLRRRLLLLLLRRLRLLLLRRLLRRRLLLLLLLLRLLRRRLRLLRLLLRRRLLLLLLLRLRLRLRRLLLRLLLRLLLRLRLLRLLRLLRRRRRRRLRLLLLRRRLLRRLRRRRLLLRRRLLRLRRLRLRLLRRLLLRLRLLRLLLLRLLRRRRRLLRLLLRLRLLLLLRLLLLRLLRLRRRRLLLRLRRRRRRRRRLRLLRLRRLLRRLRRRRLLLLLRRLRRLRRRLRRLRRRRLLLRLRLLLLLRLLRRLRLLLLRLLRLLRRRRRLRRLRLLLRLRRRLRL", 248);
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
            List<int> l = [];
            Random ran = new();
            for (int i = 0; i < 100000; i++) {
                l.Add(ran.Next(1, 100000));
            }
            var res = Exercises.LongestCycle(l.ToArray());
            Console.WriteLine(res);
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
