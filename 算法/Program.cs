using System.Runtime.Serialization.Json;
using System.Text;

//using 链表;

namespace LeetCode
{

    internal class Program
    {
        static void Main(string[] args)
        {
            TreeNode root = new(1);
            root.right = new(1);
            root.right.right = new(1);
            root.right.left = new(1);
            root.right.right = new(1);
            root.right.left.left = new(1);
            root.right.left.right = new(1);
            root.right.right.left = new(1);
            Console.WriteLine(StrStr("mississippi", "isi")); 
            //[1,7,4,9,2,5]
            //TreeNode res = InsertIntoMaxTree(root, 3);
            //node.next.next = new(3);
            //node.next.next.next = new(4);
            //node.next.next.next.next = new(5);
            //Console.WriteLine(LengthOfLIS2(new int[] { 1, 1, 2, 4, 6, 3, 6, 7 }));
            //int a = LengthOfLIS(new int[] { 1, 3, 6, 7, 9, 4, 10, 5 });
            //Console.WriteLine(a);

        }

        public static long GetObjectByte<T>(T t) where T : class
        {
            DataContractJsonSerializer formatter = new(typeof(T));
            using MemoryStream stream = new();
            formatter.WriteObject(stream, t);
            return stream.Length;
        }

        public static int[] TwoSumVer1(int[] nums, int target)
        {//O (N^2) --->  O (N LogN)
            int[] sorted = new int[nums.Length];
            nums.CopyTo(sorted, 0);
            Array.Sort(sorted);
            //排序
            int length = nums.Length;
            int num1 = 0;
            int num2 = 0;
            for (int i = 0; i < length; i++)
            {
                if (Array.BinarySearch<int>(sorted, target - sorted[i]) >= 0)
                {
                    num2 = target - sorted[i];
                    num1 = sorted[i];
                    break;
                }
            }

            for (int i = 0; i < length; i++)
            {
                if (num1 == nums[i])
                {
                    num1 = i;
                    break;
                }
            }

            for (int i = length - 1; i >= 0; i--)
            {
                if (num2 == nums[i])
                {
                    num2 = i;
                    break;
                }
            }
            return new int[] { num1, num2 };
        }

        public static int[] TwoSumVer2(int[] nums, int target)
        {
            int length = nums.Length;
            Dictionary<int, List<int>> dic = new();
            for (int i = 0; i < length; i++)
            {
                if (!dic.ContainsKey(nums[i]))
                {
                    dic.Add(nums[i], new List<int>(new int[] { i }));
                }
                else
                {
                    dic[nums[i]].Add(i);
                }
            }

            for (int i = 0; i < length; i++)
            {
                int j = target - nums[i];
                if (!dic.ContainsKey(j))
                {
                    continue;
                }

                if (dic[j].Count != 0)
                {
                    if (dic[j].Contains(i))
                    {
                        if (dic[j].Count == 1)
                        {
                            continue;
                        }
                        else
                        {
                            dic[j].Remove(i);
                        }
                    }
                    return new int[2] { i, dic[j][0] };
                }

            }

            return Array.Empty<int>();
        }

        public static int[] TwoSumVer3(int[] nums, int target)
        {
            Dictionary<int, int> sb = new();
            for (int i = 0; i < nums.Length; i++)
            {
                if (sb.ContainsKey(target - nums[i]))
                {
                    return new int[2] { i, sb[target - nums[i]] };
                }
                sb[nums[i]] = i;
            }
            return Array.Empty<int>();
        }

        public static int BinarySearch(int[] nums, int element)
        {
            int length = nums.Length;
            int min = 0, max = length - 1;
            while (min <= max)
            {
                int mid = min + (max - min) / 2;
                if (nums[mid] > element)
                {
                    max = mid - 1;
                }
                else if (nums[mid] < element)
                {
                    min = mid + 1;
                }
                else
                {
                    //j找到
                    return mid;
                }
            }

            return -1;
        }

        public static int BinarySearchLower(int[] nums, int element)
        {
            int length = nums.Length;
            int min = 0, max = length - 1;
            while (min < max)
            {
                int mid = min + (max - min + 1) / 2;
                if (nums[mid] > element)
                {
                    max = mid - 1;
                }
                else if (nums[mid] < element)
                {
                    min = mid;
                }
                else
                {
                    //j找到
                    return mid;
                }
            }

            return min;
        }

        //输入：nums1 = [4,9,5], nums2 = [9,4,9,8,4]
        //4 5 9, 4 4 8 9 9
        //输出：[9,4]
        public static int[] GetUnite(int[] nums1, int[] nums2)
        {
            Dictionary<int, int> dic = new();
            Dictionary<int, int> Repeating = new();
            for (int i = 0; i < nums1.Length; i++)
            {
                dic[nums1[i]] = 1;
            }
            for (int i = 0; i < nums2.Length; i++)
            {
                if (dic.ContainsKey(nums2[i]))
                {
                    Repeating[nums2[i]] = 1;
                }
            }
            return Repeating.Keys.ToArray();
        }

        public static bool IsAnagramVer1(string s, string t)
        {
            Dictionary<char, int> dic = new();
            foreach (var item in s)
            {
                if (dic.ContainsKey(item))
                {
                    dic[item]++;
                }
                else
                {
                    dic[item] = 1;
                }
            }

            foreach (var item in t)
            {
                if (dic.ContainsKey(item))
                {
                    dic[item]--;
                }
                else
                {
                    return false;
                }
            }

            foreach (var item in dic.Keys)
            {
                if (dic[item] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsAnagramVer2(string s, string t)
        {
            //输入: s = "anagram", t = "nagaram"
            //输出: true
            char[] chars1 = s.ToCharArray();
            char[] chars2 = t.ToCharArray();
            Array.Sort(chars1);
            Array.Sort(chars2);
            return chars1.SequenceEqual(chars2);
        }

        //210   012
        public static int Reverse(int x)
        {
            int minus = 1;
            if (x == 0 || x == int.MinValue) return 0;
            if (x < 0)
            {
                minus = -1;
                x = -x;
            }//处理正负

            int result = 0;
            int k = 0;//Length
            List<int> reversedInt = new(10);
            while (true)
            {
                int tmp = GetDigit(x, k);
                if (tmp == -1)
                {
                    break;
                }
                reversedInt.Add(tmp);
                k++;
            }
            if (k == 10 && reversedInt[0] >= 3)
            {
                return 0;
            }
            for (int i = 0; i < k - 1; i++)
            {
                result += reversedInt[k - 1 - i] * powsTen[i];
            }

            if (reversedInt[0] == 2)
            {
                if (result > 147483647)
                {
                    return 0;
                }
            }
            result += reversedInt[0] * powsTen[k - 1];
            return minus * result;
        }

        public static bool IsPalindrome(int x)
        {
            if (x < 0)
            {
                return false;
            }
            //123321
            List<int> ints = new();
            int i = 0;
            while (true)
            {
                int digit = GetDigit(x, i);
                if (digit == -1)
                {
                    break;
                }
                ints.Add(digit);
                i++;
            }

            for (int j = 0; j < i / 2; j++)
            {
                if (ints[j] != ints[i - j - 1])
                {
                    return false;
                }
            }
            return true;
        }

        static int[] powsTen = { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000 };

        private static int GetDigit(int num, int index)
        {
            int a = index >= 10 ? 0 : num / powsTen[index];
            return a == 0 ? -1 : a % 10;
        }

        public static bool ValidSquare(int[] p1, int[] p2, int[] p3, int[] p4)
        {
            #region 废掉
            ////x:[0], y:[1]
            ////先证明中心对称
            ////先找对点
            //int[] x = { p1[0], p2[0], p3[0], p4[0] };
            //int[] y = { p1[1], p2[1], p3[1], p4[1] };
            //int fuck = 0;
            //if ((x[0] - x[1]) * (x[0] - x[2]) + (y[0] - y[1]) * (y[0] - y[2]) == 0)
            //{
            //    fuck++;
            //}
            //if ((x[0] - x[2]) * (x[0] - x[3]) + (y[0] - y[2]) * (y[0] - y[3]) == 0)
            //{
            //    fuck++;
            //}
            //if ((x[0] - x[3]) * (x[0] - x[1]) + (y[0] - y[3]) * (y[0] - y[1]) == 0)
            //{
            //    fuck++;
            //}
            //if (fuck <= 1)
            //{
            //    return false;
            //}
            ////Dictionary<int, int> dic = new();
            ////dic[p1[0]] = p1[1];
            ////dic[p2[0]] = p2[1];
            ////dic[p3[0]] = p3[1];
            ////dic[p4[0]] = p4[1];

            ////HashSet<int> ys = new(y);
            //Array.Sort(x);
            //Array.Sort(y);//排序点
            //if (x[0] == x[3])
            //{
            //    return false;
            //}

            //if (x[0] + x[3] == x[1] + x[2] && y[0] + y[3] == y[1] + y[2])
            //{//中心对称
            //    if (y[0] - y[2] == x[0] - x[2])
            //    {
            //        return true;
            //    }
            //}
            //return false;
            //0 0
            //1 1
            //0 0
            //0 0 
            #endregion
            HashSet<double> set = new(6)
            {
                GetDistance(p1, p2),
                GetDistance(p1, p3),
                GetDistance(p1, p4),
                GetDistance(p2, p3),
                GetDistance(p2, p4),
                GetDistance(p3, p4)
            };
            if (set.Contains(0))
            {
                return false;
            }
            if (set.Count == 2)
            {
                return true;
            }
            return false;
        }

        private static double GetDistance(int[] p1, int[] p2)
        {
            return Math.Sqrt((p1[0] - p2[0]) * (p1[0] - p2[0]) + (p1[1] - p2[1]) * (p1[1] - p2[1]));
        }

        public static int[] ArrayRankTransform(int[] arr)
        {
            int[] source = new int[arr.Length];
            Array.Copy(arr, source, arr.Length);
            Dictionary<int, int> dic = new();
            Array.Sort(arr);
            int j = 1;
            for (int i = 0; i < arr.Length; i++)
            {
                if (!dic.ContainsKey(arr[i]))
                {
                    dic[arr[i]] = j;
                }
                else
                {
                    continue;
                }
                j++;
            }

            int[] res = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                res[i] = dic[source[i]];
            }
            return res;
        }

        public static string FractionAddition(string expression)
        {
            char[] possibleOperators = new char[] { '-', '+' };
            string[] nums = expression.Split(possibleOperators, StringSplitOptions.RemoveEmptyEntries);
            List<char> operators = new(10);
            List<string> integr = new(20);//综合起来了
            int j = 0;
            for (int i = 0; i < expression.Length;)
            {
                if (expression[i] == '-' || expression[i] == '+')
                {
                    operators.Add(expression[i]);
                    i++;
                }
                else
                {
                    if (j < nums.Length)
                    {
                        i += nums[j++].Length;
                    }
                }
            }
            if (operators.Count != nums.Length)
            {
                operators.Insert(0, '+');
            }
            integr.Add("0");

            for (int i = 0; i < operators.Count; i++)
            {
                integr.Add(operators[i].ToString());
                integr.Add(nums[i].ToString());
            }//分割完毕

            Stack<string> stackOperators = new(10);
            Stack<string> stackNums = new(10);
            foreach (var item in integr)
            {
                if (item == "+" || item == "-")
                {
                    stackOperators.Push(item);
                }
                else//num
                {
                    stackNums.Push(item);
                    if (stackNums.Count == 2)
                    {
                        stackNums.Push(FracArithmetic(stackNums.Pop(), stackNums.Pop(), stackOperators.Peek() == "+"));
                        stackOperators.Pop();
                    }
                }
            }
            string[] resultFraction = stackNums.Pop().Split('/');
            int[] resultFracInts = { int.Parse(resultFraction[0]), int.Parse(resultFraction[1]) };
            Simplify(resultFracInts);
            return String.Format("{0}/{1}", resultFracInts[0], resultFracInts[1]);
        }

        private static void Simplify(int[] fraction)
        {
            if (fraction[0] == 0)
            {
                fraction[1] = 1;
                return;
            }
            else
            {
                int st = fraction[0], nd = fraction[1];
                while (true)
                {
                    //欧几里得算法:fraction[0] & fraction[1]
                    int mod = st % nd;
                    if (mod == 0)
                    {
                        break;
                    }
                    st = nd;
                    nd = mod;
                }
                nd = nd < 0 ? -nd : nd;
                //nd是最大公约数
                fraction[0] /= nd;
                fraction[1] /= nd;
            }
        }

        private static string FracArithmetic(string n1, string n2, bool isAdd)//来写写
        {
            if (n1.Length == 1)
            {
                n1 = "0/1";
            }
            else if (n2.Length == 1)
            {
                n2 = "0/1";
            }

            int n1Numerator, n1Denominator;//分子  分母
            int n2Numerator, n2Denominator;
            string[] frac1 = n1.Split('/');
            string[] frac2 = n2.Split('/');
            n1Numerator = int.Parse(frac1[0]);
            n1Denominator = int.Parse(frac1[1]);
            n2Numerator = int.Parse(frac2[0]);
            n2Denominator = int.Parse(frac2[1]);
            int LM = n1Denominator * n2Denominator;

            //对于第一个分数，乘的是n2D
            n1Denominator = LM;
            n1Numerator *= n2Denominator;
            //对于第二个分数，乘的是n1D
            n2Numerator *= (LM / n2Denominator);
            n1Numerator = isAdd ? n1Numerator : -n1Numerator;//判断到底是加还是减
            int[] result = { n1Numerator + n2Numerator, n1Denominator };
            return String.Format("{0}/{1}", result[0], result[1]);
        }

        public static Int64 Fractorial(int num)
        {
            return TailRecursionFractorial(num, num - 1);
        }

        private static Int64 TailRecursionFractorial(Int64 n1, Int64 n2)
        {
            return n2 == 1 ? n1 : TailRecursionFractorial(n1 * n2, n2 - 1);
        }

        public static int Peach(int remain, int times)
        {
            return RecursionPeach(remain, times);
        }

        private static int RecursionPeach(int peaches, int times)
        {
            if (times == 0)
            {
                return peaches;
            }
            else
            {
                return RecursionPeach(2 * (peaches + 1), times - 1);
            }
        }

        public static string GenerateTheString(int n)
        {
            StringBuilder result = new();
            for (int i = 0; i < n - 1; i++)
            {
                result.Append('w');
            }
            if (n % 2 == 0)
            {
                result.Append('a');
            }
            else
            {
                result.Append('w');
            }
            return result.ToString();
        }


        /* Definition for a binary tree node.*/
        public class TreeNode
        {
            public int val;
            public TreeNode? left;
            public TreeNode? right;
            public TreeNode(int val = 0, TreeNode? left = null, TreeNode? right = null)
            {
                this.val = val;
                this.left = left;
                this.right = right;
            }
        }

        public static int MaxLevelSum(TreeNode root)
        {
            Stack<TreeNode> stck = new();
            stck.Push(root);
            int maxSum = int.MinValue;
            int currentlevel = 1;
            int maxLevel = 0;
            while (true)
            {
                List<TreeNode> nodeList = new();
                while (stck.Count != 0)
                {
                    nodeList.Add(stck.Pop());
                }//下一级的所有节点都在nodeList中

                int thisSum = 0;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    thisSum += nodeList[i].val;
                }

                if (thisSum > maxSum)
                {
                    maxSum = thisSum;
                    maxLevel = currentlevel;
                }
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (nodeList[i].left is not null)
                    {
                        stck.Push(nodeList[i].left);
                    }
                    if (nodeList[i].right is not null)
                    {
                        stck.Push(nodeList[i].right);
                    }
                }
                if (stck.Count == 0)
                {
                    break;
                }

                currentlevel++;
            }
            return maxLevel;
        }

        public static int MinDepth(TreeNode root)
        {
            return CursionGetMinDepth(root, 0);
        }

        private static int CursionGetMinDepth(TreeNode node, int level)
        {
            //终止条件
            if (node is null)
            {
                return level;
            }
            else if (IsLeafNode(node))
            {
                return level + 1;
            }
            else
            {
                int a = int.MaxValue, b = int.MaxValue;
                if (node.left is not null)
                {
                    a = CursionGetMinDepth(node.left, level + 1);
                }
                if (node.right is not null)
                {
                    b = CursionGetMinDepth(node.right, level + 1);
                }
                //int b = CursionGetMinDepth(node.right, level);
                return a >= b ? b : a;
            }
        }

        private static bool IsLeafNode(TreeNode node) => node.left is null && node.right is null;

        public static int LengthOfLongestSubstringVer1(string s)
        {
            int length = s.Length;
            for (int i = length; i >= 2; i--)//i个
            {

                for (int j = 0; j <= length - i; j++)
                {
                    Dictionary<char, int> dic = new();
                    //chars = new(s[j..(i + j)]);
                    //if (chars.Count == i)
                    //{
                    //    return i;
                    //}
                    int k;
                    for (k = j; k < i + j; k++)
                    {
                        if (dic.ContainsKey(s[k]))
                        {
                            break;
                        }
                        dic[s[k]] = 1;
                    }
                    if (k == i + j)
                    {
                        return i;
                    }
                }
            }
            return 1;
        }

        public static int LengthOfLongestSubstringVer2(string s)
        {
            int length = s.Length;
            int max = 0;
            for (int i = 0; i < length; i++)//固定i不动了，不完全是滑动窗口
            {
                int j = i;//j为可扩展到的最大索引
                HashSet<char> set = new(256);

                while (true)
                {
                    if (j == length)
                    {
                        break;
                    }
                    if (!set.Add(s[j]))
                    {
                        break;
                    }

                    if (j - i + 1 > max)
                    {
                        max = j - i + 1;
                    }

                    j++;
                }
            }
            return max;
        }

        public static int LengthOfLongestSubstringVer3(string s)
        {
            //滑动窗口：双指针
            int a = 0, b = 0;//from to
            int max = 0;
            while (true)
            {
                if (a == s.Length)
                {
                    break;
                }

                if (b + 1 > s.Length)
                {
                    break;
                }
                if (IsAllImparity(s[a..(b + 1)]))
                {
                    max = max < b - a + 1 ? b - a + 1 : max;

                    b++;
                }
                else
                {
                    a++;
                }
            }

            return max;
        }

        private static bool IsAllImparity(string str)
        {
            HashSet<char> values = new();
            foreach (var item in str)
            {
                if (!values.Add(item))
                {
                    return false;
                }
            }
            return true;
        }

        public static int NumColor(TreeNode root)
        {
            ints.Clear();
            CursionNumColor(root);
            return ints.Count;
        }

        public static HashSet<int> ints = new();

        private static void CursionNumColor(TreeNode node)
        {

            ints.Add(node.val);
            if (node.left is not null)
            {
                CursionNumColor(node.left);
            }
            if (node.right is not null)
            {
                CursionNumColor(node.right);
            }
        }

        /*  1+2)*3-4)*5-6)))
         *  转换为中序表达式：
         *  ((1+2)*((3-4)*(5-6)))
        */
        public static IList<int> MinSubsequence(int[] nums)
        {
            int sum = 0;
            foreach (var item in nums)
            {
                sum += item;
            }
            int[] sorted = new int[nums.Length];
            Array.Copy(nums, sorted, nums.Length);
            Array.Sort(sorted);
            sorted = sorted.Reverse().ToArray();
            int newSum = 0;//代表目前子序列之和
            List<int> res = new();
            for (int i = 0; i < nums.Length; i++)
            {
                newSum += sorted[i];
                res.Add(sorted[i]);
                if (newSum > sum - newSum)
                {
                    break;
                }
            }
            return res;
        }

        public static IList<string> LetterCombinations(string digits)
        {
            if (string.IsNullOrEmpty(digits))
            {
                return Array.Empty<string>();
            }
            dicNumToAlphabet[2] = "abc";
            dicNumToAlphabet[3] = "def";
            dicNumToAlphabet[4] = "ghi";
            dicNumToAlphabet[5] = "jkl";
            dicNumToAlphabet[6] = "mno";
            dicNumToAlphabet[7] = "pqrs";
            dicNumToAlphabet[8] = "tuv";
            dicNumToAlphabet[9] = "wxyz";
            if (digits.Length == 1)
            {
                string res = dicNumToAlphabet[int.Parse(digits)];
                string[] arr = new string[res.Length];
                for (int i = 0; i < res.Length; i++)
                {
                    arr[i] = res[i].ToString();
                }

                return arr;
            }
            /*
             * 2-abc
             * 3-def
             * 4-ghi
             * 5-jkl
             * 6-mno
             * 7-pqrs
             * 8-tuv
             * 9-wxyz
             */
            int[] ints = new int[digits.Length];
            for (int i = 0; i < digits.Length; i++)
            {
                ints[i] = int.Parse(digits[i].ToString());
            }
            int first = int.Parse(digits[0].ToString());
            return RecursionCombine(first, ints[1..(digits.Length)]);

        }

        static Dictionary<int, string> dicNumToAlphabet = new();

        private static string[] RecursionCombine(int firstDigi, params int[] digits)
        {
            //int[] dg = new int[digits.Length + 1];
            //dg[0] = firstDigi;
            //for (int i = 1; i < digits.Length; i++)
            //{
            //    dg[i] = int.Parse(digits[i].ToString());
            //}

            if (digits.Length >= 2)
            {
                string[] res = RecursionCombine(digits[0], digits[1..(digits.Length)]);
                List<string> strings = new();
                for (int i = 0; i < res.Length; i++)
                {
                    strings.AddRange(Combine2(dicNumToAlphabet[firstDigi], res[i]));
                }
                return strings.ToArray();
            }
            else
            {
                string txt1 = dicNumToAlphabet[firstDigi];
                string txt2 = dicNumToAlphabet[digits[0]];
                return Combine(txt1, txt2);
            }
        }

        private static string[] Combine(string txt1, string txt2)
        {
            List<string> res = new();
            for (int i = 0; i < txt1.Length; i++)
            {
                for (int j = 0; j < txt2.Length; j++)
                {
                    res.Add(txt1[i].ToString() + txt2[j]);
                }
            }

            return res.ToArray();
        }

        private static string[] Combine2(string txt1, string nondetachableTxt2)
        {
            string[] res = new string[txt1.Length];
            for (int i = 0; i < txt1.Length; i++)
            {
                res[i] = (txt1[i].ToString() + nondetachableTxt2);
            }

            return res;
        }

        public static TreeNode AddOneRow(TreeNode root, int val, int depth)
        {
            if (depth == 1)
            {
                TreeNode nRoot = new(val, root);
                return nRoot;
            }// > 1
             //广度 搜到depth-1层
            Queue<TreeNode> que = new();
            List<TreeNode> thsLevelsNode = new();
            que.Enqueue(root);
            for (int level = 1; level < depth - 1; level++)
            {
                thsLevelsNode.Clear();
                while (que.Count != 0)
                {
                    thsLevelsNode.Add(que.Dequeue());
                }

                foreach (var treeNode in thsLevelsNode)
                {
                    if (treeNode.left is not null)
                    {
                        que.Enqueue(treeNode.left);
                    }
                    if (treeNode.right is not null)
                    {
                        que.Enqueue(treeNode.right);
                    }
                }
            }//depth - 1 层结果在que里存储

            while (que.Count != 0)
            {
                TreeNode thsNode = que.Dequeue();
                TreeNode? thsLeft = thsNode.left, thsRight = thsNode.right;
                //添加左右字树val
                thsNode.left = new(val, thsLeft);
                thsNode.right = new(val, null, thsRight);
            }

            return root;
        }

        public static int BinSearch(int[] arr, int value)
        {
            int left = 0, right = arr.Length;
            int mid;
            while (left <= right)
            {
                mid = (left + right) / 2;
                if (arr[mid] > value)
                {
                    right = mid - 1;
                }
                else if (arr[mid] < value)
                {
                    left = mid;
                }
                else
                {
                    return mid;
                }
            }

            return -1;
        }

        public static IList<string> StringMatching(string[] words)//每个单词都不一样
        {
            BubbleSort(words);

            HashSet<string> result = new();
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = i + 1; j < words.Length; j++)
                {
                    if (words[j].IndexOf(words[i]) != -1)
                    {
                        result.Add(words[i]);
                    }
                }
            }

            return result.ToArray();
        }

        private static void BubbleSort(string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                for (int j = 0; j < values.Length - i - 1; j++)
                {
                    if (values[j].Length > values[j + 1].Length)
                    {
                        (values[j], values[j + 1]) = (values[j + 1], values[j]);
                    }
                }
            }
        }

        public static int LengthOfLIS(int[] nums)
        {
            int max = 0;
            int tmp;
            for (int i = 0; i < nums.Length; i++)
            {
                tmp = RecursionLengthOfLIS(nums[i..]);
                if (tmp > max)
                {
                    max = tmp;
                }
            }

            return max;
        }

        private static int RecursionLengthOfLIS(int[] nums)
        {
            if (nums.Length == 1)
            {
                return 1;
            }

            int max = 1;
            for (int i = 1; i < nums.Length; i++)
            {
                if (nums[i] > nums[0])
                {
                    max = Math.Max(max, 1 + RecursionLengthOfLIS(nums[i..]));
                }
            }

            return max;
        }

        public static bool IsBalanced(TreeNode root)
        {
            RecursionIsBalanced(root);
            return isBaln;
        }

        static bool isBaln = true;

        public static int RecursionIsBalanced(TreeNode root)
        {
            if (root is null)
            {
                return 0;
            }
            //if (Math.Abs(GetDepthOfBinTree(root.left) - GetDepthOfBinTree(root.right)) >= 2)
            //{
            //    return false;
            //}
            if (root.left is null && root.right is null)
            {
                return 1;
            }
            int leftLv = 1 + RecursionIsBalanced(root.left);
            int rightLv = 1 + RecursionIsBalanced(root.right);

            if (Math.Abs(leftLv - rightLv) > 1)
            {
                isBaln = false;
            }

            return Math.Max(leftLv, rightLv);

        }

        private static int GetDepthOfBinTree(TreeNode tree)
        {
            if (tree is null)
            {
                return 0;
            }
            if (tree.left is null && tree.right is null)
            {
                return 1;
            }
            return Math.Max(1 + GetDepthOfBinTree(tree.left), 1 + GetDepthOfBinTree(tree.right));
        }

        public static bool IsValidBST(TreeNode root)
        {
            InOrderBinT(root);
            return IsValid;
        }

        static long previosVal = long.MinValue;

        static bool IsValid = true;

        private static void InOrderBinT(TreeNode root)
        {
            if (root is null)
            {
                return;
            }

            InOrderBinT(root.left);
            if (root.val > previosVal)
            {
                previosVal = root.val;
                InOrderBinT(root.right);
            }
            else
            {
                IsValid = false;
                return;
            }
        }

        public static int LengthOfLIS2(int[] nums)
        {
            int max = 1;
            for (int i = 0; i < nums.Length; i++)//遍历一遍从哪里开始子序列最长
            {
                max = Math.Max(RecursionLengthOfLIS2(nums[i..]), max);
            }
            return max;
        }

        public static int RecursionLengthOfLIS2(int[] nums)
        {
            if (nums.Length == 1)
            {
                return 1;
            }

            int max = 1;
            for (int i = 1; i < nums.Length; i++)//遍历当前数组有没有比第一位更大的
            {
                if (nums[i] > nums[0])
                {
                    max = Math.Max(max, 1 + RecursionLengthOfLIS2(nums[i..]));
                }
            }

            return max;
        }

        static IList<int> _pathCombine = new List<int>();

        static IList<IList<int>> _resultCombine = new List<IList<int>>();


        public static int MinStartValue(int[] nums)
        {
            //-3 2 -3 4 2
            int accumulate = 0;
            int min = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                accumulate += nums[i];
                min = Math.Min(min, accumulate);
            }

            return 1 - min;
        }

        public static int FindMaxConsecutiveOnes(int[] nums)
        {
            int max = 1;
            int thsCount = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] == 1)
                {
                    thsCount++;
                }
                else
                {
                    max = Math.Max(max, thsCount);
                    thsCount = 0;
                }
            }

            max = Math.Max(max, thsCount);
            return max;
        }

        public static int FindPoisonedDuration(int[] timeSeries, int duration)
        {
            int result = 0;
            int length = timeSeries.Length;
            if (length == 1)
            {
                return duration;
            }

            if (timeSeries[1] - timeSeries[0] > duration)
            {
                result += duration * 2;
            }
            else
            {
                result += timeSeries[1] + duration - timeSeries[0];
            }

            for (int i = 2; i < length; i++)
            {
                if (timeSeries[i] - timeSeries[i - 1] > duration)
                {
                    result += duration;
                }
                else
                {
                    result += timeSeries[i] - timeSeries[i - 1];
                }

            }

            return result;
        }

        /// <summary>
        /// 回溯求组合数
        /// </summary>
        /// <param name="n">集合总位数</param>
        /// <param name="k">组合位数</param>
        public static void CombineBacktracking(int n, int k, int startIndex)
        {

            if (_pathCombine.Count == k)
            {
                int[] tmp = new int[k];
                _pathCombine.CopyTo(tmp, 0);
                _resultCombine.Add(tmp);
                //_path.Clear();
                return;
            }
            //start
            for (int i = startIndex; n - i + _pathCombine.Count >= k; i++)
            {
                _pathCombine.Add(i + 1);//从1开始比从0开始要多一
                CombineBacktracking(n, k, i + 1);
                _pathCombine.RemoveAt(_pathCombine.Count - 1);
            }
        }

        static int _pathSum;

        static IList<int> _pathCombinationSum = new List<int>();

        static IList<IList<int>> _resultSum = new List<IList<int>>();

        static int[] _sourceCandidates;

        public static IList<IList<int>> CombinationSum(int[] candidates, int target)
        {
            _resultSum.Clear();
            Array.Sort(candidates);
            _sourceCandidates = candidates;
            BacktrackingCombinationSum(0, target);
            return _resultSum;
        }

        public static void BacktrackingCombinationSum(int startIndex, int target)
        {
            //if (_pathSum > target)
            //{
            //    return;
            //}

            if (_pathSum == target)
            {
                int[] tmp = _pathCombinationSum.ToArray();
                _resultSum.Add(tmp);
                return;
            }

            for (int i = startIndex; i < _sourceCandidates.Length; i++)
            {
                _pathSum += _sourceCandidates[i];
                _pathCombinationSum.Add(_sourceCandidates[i]);
                if (_pathSum > target)
                {
                    _pathSum -= _sourceCandidates[i];
                    _pathCombinationSum.RemoveAt(_pathCombinationSum.Count - 1);
                    break;
                }
                BacktrackingCombinationSum(i, target);
                _pathSum -= _sourceCandidates[i];
                _pathCombinationSum.RemoveAt(_pathCombinationSum.Count - 1);

            }
        }

        public static string SolveEquation(string equation)
        {
            #region backup
            //string[] equa = equation.Split('=');

            //bool isPlus;
            //int[] factorX = { 0, 0 };
            //int[] constant = { 0, 0 };
            //for (int j = 0; j < 2; j++)
            //{
            //    if (equa[j][0] != 'x' && equa[j][0] != '-')
            //    {
            //        equa[j] = equa[j].Insert(0, "+");
            //    }
            //}

            //for (int j = 0; j < 2; j++)
            //{
            //    isPlus = true;
            //    for (int i = 0; i < equa[j].Length; i++)//处理左边
            //    {
            //        switch (equa[j][i])
            //        {

            //            case '+':
            //                isPlus = true;
            //                break;
            //            case '-':
            //                isPlus = false;
            //                break;
            //            case 'x':
            //                if (((i >= 1 && (equa[j][i - 1] == '+' || equa[j][i - 1] == '-'))) || i == 0)
            //                {
            //                    factorX[j] = isPlus ? factorX[j] + 1 : factorX[j] - 1;
            //                }
            //                break;
            //            default://是数字
            //                int num = int.Parse(equa[j][i].ToString());
            //                if (i + 1 < equa[j].Length && equa[j][i + 1] == 'x')
            //                {
            //                    factorX[j] = isPlus ? factorX[j] + num : factorX[j] - num;
            //                }
            //                else
            //                {
            //                    constant[j] = isPlus ? constant[j] + num : constant[j] - num;
            //                }
            //                break;
            //        }
            //    }
            //}
            //if (factorX[0] == factorX[1] && constant[0] == constant[1])
            //{
            //    return "Infinite solutions";
            //}
            //else if (factorX[0] == factorX[1] && constant[0] != constant[1])
            //{
            //    return "No solution";
            //}
            //else
            //{
            //    return String.Format("x={0}", (-constant[0] + constant[1]) / (factorX[0] - factorX[1]));
            //} 
            #endregion
            string[] equa = equation.Split('=');
            List<List<string>> items = new(2);
            char[] opers = { '+', '-' };
            items.Add(new(equa[0].Split(opers, StringSplitOptions.RemoveEmptyEntries)));
            items.Add(new(equa[1].Split(opers, StringSplitOptions.RemoveEmptyEntries)));
            int[] factorX = { 0, 0 };
            int[] constant = { 0, 0 };

            for (int j = 0; j < 2; j++)
            {
                if (equa[j][0] != '-')
                {
                    equa[j] = equa[j].Insert(0, "+");
                }
            }


            Queue<char> stck = new();

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < equa[j].Length; i++)
                {
                    if (equa[j][i] == '+' || equa[j][i] == '-')
                    {
                        stck.Enqueue(equa[j][i]);//dispose the sign
                    }
                }
            }

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < items[j].Count; i++)
                {
                    bool isPlus = stck.Dequeue() == '+';
                    if (int.TryParse(items[j][i].ToString(), out int num1))
                    {
                        constant[j] = isPlus ? constant[j] + num1 : constant[j] - num1;
                    }
                    else//X
                    {
                        int facto;
                        if (items[j][i].Length == 1)
                        {
                            facto = 1;
                        }
                        else
                        {
                            facto = int.Parse(items[j][i][..^1]);
                        }
                        factorX[j] = isPlus ? factorX[j] + facto : factorX[j] - facto;
                    }
                }
            }

            if (factorX[0] == factorX[1] && constant[0] == constant[1])
            {
                return "Infinite solutions";
            }
            else if (factorX[0] == factorX[1] && constant[0] != constant[1])
            {
                return "No solution";
            }
            else
            {
                return String.Format("x={0}", (-constant[0] + constant[1]) / (factorX[0] - factorX[1]));
            }
        }

        public static IList<IList<string>> Partition(string s)
        {
            _resultPartition.Clear();
            _pathPartition.Clear();
            _pathPartition.Add(s);
            BacktrackingPartition(s, 1);
            return _resultPartition;
        }

        static List<string> _pathPartition = new();

        static IList<IList<string>> _resultPartition = new List<IList<string>>();

        private static void BacktrackingPartition(string s, int startIndex)//开始应为1
        {
            //长4的字符串，能分4下
            if (startIndex >= s.Length)
            {
                bool leaf = true;
                for (int i = 0; i < _pathPartition.Count; i++)
                {
                    if (!IsPalindromeString(_pathPartition[i]))
                    {
                        leaf = false;
                        break;
                    }
                }
                if (leaf)
                {
                    string[] tmp = _pathPartition.ToArray();
                    if (tmp[^1] == "")
                    {
                        _resultPartition.Add(tmp[..(_pathPartition.Count - 1)]);
                    }
                    else
                    {
                        _resultPartition.Add(tmp);
                    }
                }

                return;

            }


            //如何按索引分割字符串[1-length]
            for (int i = startIndex; i <= s.Length; i++)
            {
                _pathPartition = SplitLastStr(i, _pathPartition.ToArray());//尝试分割
                                                                           //BacktrackingPartition(_pathPartition[_pathPartition.Count - 1], 1);
                BacktrackingPartition(s, i + 1);
                _pathPartition = MergeLastTwoStr(_pathPartition.ToArray());//试图还原
            }

        }

        private static List<string> SplitLastStr(int interspaceIndex, params string[] str)
        {
            int a = 0;
            for (int i = 0; i < str.Length - 1; i++)
            {
                a += str[i].Length;
            }
            interspaceIndex -= a;
            if (str[^1].Length == 0)
            {
                return str.ToList();
            }
            List<string> strs = new(str);
            string tmp = strs[^1];
            strs.RemoveAt(strs.Count - 1);
            strs.Add(tmp[..interspaceIndex]);
            strs.Add(tmp[interspaceIndex..]);
            return strs;
        }

        private static List<string> MergeLastTwoStr(params string[] str)
        {
            List<string> strs = new(str);
            strs[^2] += strs[^1];
            strs.RemoveAt(strs.Count - 1);
            return strs;
        }

        private static bool IsPalindromeString(string str)
        {
            for (int i = 0; i < str.Length / 2; i++)
            {
                if (str[i] != str[str.Length - i - 1])
                {
                    return false;
                }
            }

            return true;
        }

        public static IList<IList<int>> CombinationSum3(int k, int n)
        {
            _resultCombinationSum3.Clear();
            BacktrackingCombinationSum3(k, 1, n);

            return _resultCombinationSum3;
        }

        static IList<int> _pathCombinationSum3 = new List<int>();

        static IList<IList<int>> _resultCombinationSum3 = new List<IList<int>>();

        static int sumCombinationSum3 = 0;

        private static void BacktrackingCombinationSum3(int remainLevel, int startNum, int target)
        {
            if (remainLevel == 0)
            {
                if (target == sumCombinationSum3)
                {
                    int[] tmp = new int[_pathCombinationSum3.Count];
                    _pathCombinationSum3.CopyTo(tmp, 0);
                    _resultCombinationSum3.Add(tmp);
                }

                return;
            }

            //[1--9]
            for (int i = startNum; i <= 9; i++)
            {
                if (9 - startNum + 1 < remainLevel)
                {
                    break;
                }

                _pathCombinationSum3.Add(i);
                sumCombinationSum3 += i;

                BacktrackingCombinationSum3(remainLevel - 1, i + 1, target);
                _pathCombinationSum3.RemoveAt(_pathCombinationSum3.Count - 1);
                sumCombinationSum3 -= i;
            }
        }

        public static IList<IList<string>> GetAllSubstring(string source)
        {
            _resultGetAllSubstring.Clear();
            BacktrackingGetAllSubstring(source, 0);
            return _resultGetAllSubstring;
        }

        static IList<string> _pathGetAllSubstring = new List<string>();

        static IList<IList<string>> _resultGetAllSubstring = new List<IList<string>>();

        private static void BacktrackingGetAllSubstring(string s, int startIndex)
        {
            if (startIndex + 1 > s.Length)
            {
                string[] tmp = _pathGetAllSubstring.ToArray();
                _resultGetAllSubstring.Add(tmp);
                return;
            }

            for (int i = startIndex; i < s.Length; i++)
            {
                _pathGetAllSubstring.Add(s[startIndex..(i + 1)]);//添加当前一部分 左闭右开
                BacktrackingGetAllSubstring(s, i + 1);
                _pathGetAllSubstring.RemoveAt(_pathGetAllSubstring.Count - 1);
            }
        }

        public static int Knapsack(int[] values, int[] bulks, int maxBulk)
        {
            int[,] dp = new int[maxBulk, values.Length];
            /*
             3000 4             dp[bulk, item] item is value
             2000 3             dp[j, i]
             1500 1
             */
            List<int> path = new();

            for (int k = 0; k < maxBulk; k++)
            {
                if (bulks[0] <= k + 1)
                {
                    dp[k, 0] = values[0];
                }
            }

            for (int i = 1; i < values.Length; i++)
            {
                for (int j = 0; j < maxBulk; j++)//j+1才是体积
                {
                    int another;
                    if (j + 1 - bulks[i] == 0)
                    {
                        another = 0;
                    }
                    else if (j + 1 - bulks[i] > 0)
                    {
                        another = dp[j - bulks[i], i - 1];
                    }
                    else
                    {
                        another = -values[i];//不能选了
                    }

                    if (dp[j, i - 1] > values[i] + another)
                    {
                        dp[j, i] = dp[j, i - 1];
                    }
                    else
                    {
                        dp[j, i] = values[i] + another;
                        path.Add(i);
                    }

                }
            }

            Console.WriteLine(PrintTwoDimensionArr(dp));
            for (int i = 0; i < path.Count; i++)
            {
                Console.WriteLine(path[i]);
            }
            return dp[maxBulk - 1, values.Length - 1];
        }

        public static string PrintTwoDimensionArr(int[,] arr)
        {
            StringBuilder sb = new();
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                for (int j = 0; j < arr.GetLength(0); j++)
                {
                    sb.Append(arr[j, i] + ", ");
                }

                sb.Append('\n');
            }

            return sb.ToString();

        }

        public static int MaxScore(string s)
        {
            int max;
            int score = 0;
            for (int i = s.Length - 1; i >= 1; i--)
            {
                if (s[i] == '1')
                {
                    score++;
                }
            }
            if (s[0] == '0')
            {
                score++;
            }//先处理一次
            max = score;

            for (int i = 1; i < s.Length - 1; i++)
            {
                if (s[i] == '0')
                {
                    //----->>>从左向右
                    score++;
                }
                else
                {
                    score--;
                }

                max = Math.Max(max, score);
            }

            return max;
        }

        public class ListNode
        {
            public int val;
            public ListNode? next;
            public ListNode(int x)
            {
                val = x;
                next = null;
            }
        }

        public static ListNode? DetectCycle(ListNode head)
        {
            ListNode? fast;
            ListNode? slow;
            fast = slow = head;
            while (true)
            {
                fast = fast?.next?.next;
                slow = slow?.next;
                if (fast is null || slow is null)
                {
                    return null;
                }

                if (fast == slow)
                {
                    ListNode met = fast;
                    ListNode root = head;
                    while (true)
                    {
                        if (met == root)
                        {
                            return met;
                        }

                        met = met.next;
                        root = root.next;
                    }
                }
            }

        }

        public static int Rob(int[] nums)
        {//2 7 9 3 1
            return DPRob(nums);
        }

        public static int RecursionRob(int[] nums, int startIndex)//纯暴力搜索
        {
            switch (nums.Length - startIndex)
            {
                case 0:
                    return 0;
                case 1:
                    return nums[^1];
                case 2:
                    return Math.Max(nums[^1], nums[^2]);
            }

            int max = 0;
            for (int i = startIndex; i < startIndex + 2; i++)//确定起始位置和后续处理区间
            {
                max = Math.Max(nums[i] + RecursionRob(nums, i + 2), max);
            }

            return max;
        }

        public static int DPRob(int[] nums)
        {
            switch (nums.Length)
            {
                case 0:
                    return 0;
                case 1:
                    return nums[0];
                default:
                    break;
            }

            int[] dp = new int[2];
            //初始化
            dp[0] = nums[0];
            dp[1] = Math.Max(nums[0], nums[1]);
            for (int i = 2; i < nums.Length; i++)
            {
                int tmp = Math.Max(dp[1], dp[0] + nums[i]);
                dp[0] = dp[1];
                dp[1] = tmp;
            }

            return dp[1];

        }

        public static int FindContentChildren(int[] g, int[] s)
        {
            Array.Sort(g);// 7 8 9 10
            Array.Sort(s);// 5 6 7 8
            int count = 0;
            int cur1 = 0, cur2 = 0;
            if (g.Length == 0 || s.Length == 0)
            {
                return 0;
            }
            do
            {
                if (s[cur2] >= g[cur1])
                {
                    cur1++;
                    count++;
                }
                cur2++;

            } while (cur2 < s.Length && cur1 < g.Length);

            return count;
        }

        public static int WiggleMaxLength(int[] nums)
        {
            //1 7 4 9 2 5 贪心
            int count = 0;
            int sign = 0;
            for (int i = 0; i < nums.Length - 1; i++)
            {
                int tmp = Math.Sign(nums[i + 1] - nums[i]);
                if (sign != tmp && tmp != 0)
                {
                    count++;
                    sign = tmp;
                }
            }

            return count + 1;
        }

        public static int MaxSubArray(int[] nums)
        {
            int sum = 0;
            int max = int.MinValue;
            for (int i = 0; i < nums.Length; i++)
            {
                sum += nums[i];
                max = Math.Max(sum, max);
                if (sum < 0)
                {
                    sum = 0;//重新开始
                }

            }

            return max;
        }

        public static int DPMaxSubArray(int[] nums)
        {
            int[] dp = new int[nums.Length];
            dp[0] = nums[0];
            int result = nums[0];
            for (int i = 1; i < nums.Length; i++)
            {
                dp[i] = Math.Max(nums[i], nums[i] + dp[i - 1]);
                result = Math.Max(dp[i], result);
            }

            return result;
        }

        public static int DeepestLeavesSum(TreeNode root)
        {
            Queue<TreeNode> que = new();
            List<TreeNode> nodes = new();
            que.Enqueue(root);
            while (true)
            {
                nodes.Clear();
                while (que.Count != 0)
                {
                    nodes.Add(que.Dequeue());
                }
                bool leaf = true;
                int sum = 0;
                foreach (var item in nodes)
                {
                    sum += item.val;
                    if (item.left is not null)
                    {
                        que.Enqueue(item.left);
                        leaf = false;
                    }
                    else
                    {
                        leaf = leaf && true;
                    }
                    if (item.right is not null)
                    {
                        que.Enqueue(item.right);
                        leaf = false;
                    }
                    else
                    {
                        leaf = leaf && true;
                    }
                }

                if (leaf)
                {
                    return sum;
                }

            }
        }

        public static IList<string> RestoreIpAddresses(string s)
        {
            _resultIP.Clear();
            BacktrackingRestoreIpAddresses(s, 0, 4);
            return _resultIP;
        }

        static IList<string> _pathIP = new List<string>(4);

        static IList<string> _resultIP = new List<string>();

        public static void BacktrackingRestoreIpAddresses(string s, int startIndex, int level)
        {
            if (s.Length - startIndex > level * 3)//剩的太多了，分不完了
            {
                return;
            }

            if (level == 1)//还差最后一个部分，判断是否能作为ip地址
            {
                if (int.TryParse(s[startIndex..], out int res))//是否到尾变成了空字符
                {//NO 
                    if (res <= 255 && (s[startIndex..][0] != '0' || s[startIndex..].Length <= 1))
                    {//可以一战
                        _pathIP.Add(res.ToString());
                        string sb = string.Join('.', _pathIP);
                        _resultIP.Add(sb.ToString());
                        _pathIP.RemoveAt(_pathIP.Count - 1);
                    }
                }

                return;
            }

            for (int i = startIndex; i < startIndex + 3 && i < s.Length; i++)//判断3次就够
            {
                //划分区域
                _pathIP.Add(s[startIndex..(i + 1)]);

                if (int.Parse(_pathIP[_pathIP.Count - 1]) <= 255 && (_pathIP[_pathIP.Count - 1][0] != '0' || _pathIP[_pathIP.Count - 1].Length <= 1))
                {
                    BacktrackingRestoreIpAddresses(s, i + 1, level - 1);
                }

                _pathIP.RemoveAt(_pathIP.Count - 1);
            }
        }

        public static string Convert(string s, int numRows)
        {
            if (numRows == 1)
            {
                return s;
            }
            StringBuilder[] txt = new StringBuilder[numRows];
            int k = 0;//count
            bool plus = true;
            for (int i = 0; i < s.Length; i++)
            {
                if (txt[k] is null)
                {
                    txt[k] = new();
                }
                txt[k].Append(s[i]);

                if (k == numRows - 1 || (k == 0
                                         && i != 0))
                {//k已达到最大值，应折回
                    plus = !plus;
                }

                k = plus ? k + 1 : k - 1;
            }

            StringBuilder result = new();
            for (int i = 0; i < numRows; i++)
            {
                result.Append(txt[i]);
            }

            return result.ToString();

        }

        public int BusyStudent(int[] startTime, int[] endTime, int queryTime)
        {
            int count = 0;
            for (int i = 0; i < startTime.Length; i++)
            {
                if ((queryTime >= startTime[i] && queryTime <= endTime[i]))
                {
                    count++;
                }
            }

            return count;
        }

        public static IList<IList<int>> Subsets(int[] nums)
        {
            _resultSubsets.Clear();
            BacktrackingSubsets(nums, 0);
            return _resultSubsets;
        }

        static IList<int> _pathSubsets = new List<int>();

        static IList<IList<int>> _resultSubsets = new List<IList<int>>();

        public static void BacktrackingSubsets(int[] nums, int startIndex)
        {
            int[] tmp = _pathSubsets.ToArray();
            _resultSubsets.Add(tmp);
            tmp = default;
            for (int i = startIndex; i < nums.Length; i++)
            {
                _pathSubsets.Add(nums[i]);
                BacktrackingSubsets(nums, i + 1);
                _pathSubsets.RemoveAt(_pathSubsets.Count - 1);
            }
        }

        public static IList<IList<int>> CombinationSum2(int[] candidates, int target)
        {
            int[] sorted = candidates.ToArray();
            Array.Sort(sorted);
            _resultCombinationSum2.Clear();
            BacktrackingCombinationSum2(sorted, target, 0, new bool[candidates.Length]);
            return _resultCombinationSum2;
        }

        static IList<int> _pathCombinationSum2 = new List<int>();

        static int _sumCombinationSum2 = 0;

        static IList<IList<int>> _resultCombinationSum2 = new List<IList<int>>();

        public static void BacktrackingCombinationSum2(int[] candidates, int target, int startIndex, bool[] status)
        {
            //status最开始为全FALSE
            //candi应该已排序好
            if (_sumCombinationSum2 > target)
            {
                return;
            }
            else if (_sumCombinationSum2 == target)
            {
                int[] tmp = _pathCombinationSum2.ToArray();
                _resultCombinationSum2.Add(tmp);
            }

            for (int i = startIndex; i < candidates.Length; i++)
            {
                _pathCombinationSum2.Add(candidates[i]);
                _sumCombinationSum2 += candidates[i];
                status[i] = true;

                if (i <= 0 || candidates[i - 1] != candidates[i] || status[i - 1] != false)//只允许同枝，不允许同层
                {
                    BacktrackingCombinationSum2(candidates, target, i + 1, status);
                }

                status[i] = false;
                _pathCombinationSum2.RemoveAt(_pathCombinationSum2.Count - 1);
                _sumCombinationSum2 -= candidates[i];
            }
        }

        public static TreeNode ConstructMaximumBinaryTree(int[] nums)
        {
            return RecursionConstructMaximumBinaryTree(nums);
        }

        private static TreeNode RecursionConstructMaximumBinaryTree(int[] nums)
        {
            if (nums.Length == 1)
            {
                return new(nums[0]);
            }
            int max = 0;
            int indx = 0;//max indx
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] > max)
                {
                    max = nums[i];
                    indx = i;
                }
            }

            TreeNode tree = new(max);
            if (indx == 0)
            {
                tree.right = RecursionConstructMaximumBinaryTree(nums[1..]);
            }
            else if (indx == nums.Length - 1)
            {
                tree.left = RecursionConstructMaximumBinaryTree(nums[..^1]);
            }
            else
            {
                tree.right = RecursionConstructMaximumBinaryTree(nums[(indx + 1)..]);
                tree.left = RecursionConstructMaximumBinaryTree(nums[..indx]);
            }

            return tree;
        }

        public static string IntToRoman(int num)
        {
            string source = num.ToString();
            int thsNum;
            string res = null;
            for (int i = source.Length - 1; i >= 0; i--)
            {
                thsNum = powsTen[i] * GetDigit(num, i);
                string add = "";
                switch (thsNum)
                {
                    case 1:
                        add = "I";
                        break;
                    case 2:
                        add = "II";
                        break;
                    case 3:
                        add = "III";
                        break;
                    case 4:
                        add = "IV";
                        break;
                    case 5:
                        add = "V";
                        break;
                    case 6:
                        add = "VI";
                        break;
                    case 7:
                        add = "VII";
                        break;
                    case 8:
                        add = "VIII";
                        break;
                    case 9:
                        add = "IX";
                        break;
                    case 10:
                        add = "X";
                        break;
                    case 40:
                        add = "XL";
                        break;
                    case 50:
                        add = "L";
                        break;
                    case 90:
                        add = "XC";
                        break;
                    case 100:
                        add = "C";
                        break;
                    case 400:
                        add = "CD";
                        break;
                    case 500:
                        add = "D";
                        break;
                    case 900:
                        add = "CM";
                        break;
                    case 1000:
                        add = "M";
                        break;
                    default:
                        if (thsNum > 10 && thsNum < 40)
                        {
                            add = new('X', GetDigit(num, i));
                        }
                        else if (thsNum > 50 && thsNum < 90)
                        {
                            add = "L";
                            add += new string('X', GetDigit(num, i) - 5);
                        }
                        else if (thsNum > 100 && thsNum < 400)
                        {
                            add = new('C', GetDigit(num, i));
                        }
                        else if (thsNum > 500 && thsNum < 900)
                        {
                            add = "D";
                            add += new string('C', GetDigit(num, i) - 5);
                        }
                        else if (thsNum > 1000)
                        {
                            add = new('M', GetDigit(num, i));
                        }
                        break;
                }
                res += add;
            }

            return res;
        }

        public static string CountAndSay(int n)
        {
            string[] nums = new string[n + 1];
            nums[1] = "1";
            string pre = nums[1].ToString();
            for (int i = 2; i <= n; i++)
            {//生成每一个数
                string res = null;
                int count = 1;
                int thsNum = int.Parse(pre[0].ToString()); ;
                int left = 0, right = 0;
                while (true)//滑窗
                {
                    if (right == pre.Length - 1)
                    {
                        res += count;
                        res += thsNum;
                        break;
                    }
                    if (pre[right] == pre[right + 1])
                    {
                        thsNum = int.Parse(pre[right].ToString());
                        right++;
                        count++;
                    }
                    else
                    {
                        res += count;
                        res += thsNum;

                        count = 1;
                        left = right + 1;
                        right = left;
                        thsNum = int.Parse(pre[right].ToString());
                    }
                }

                nums[i] = res;
                pre = res;
            }

            return nums[n];
        }

        public static ListNode? RemoveNthFromEnd(ListNode head, int n)
        {
            if (head.next is null)
            {
                return null;
            }

            List<ListNode> node = new(32);
            ListNode? cur = head;
            while (true)
            {
                if (cur is null)
                {
                    break;
                }

                node.Add(cur);
                cur = cur.next;
            }

            if (n == node.Count)
            {
                return node[1];
            }

            node.Add(null);
            node[^(n + 2)].next = node[^(n)];
            return node[0];
        }

        public static int RemoveElement(int[] nums, int val)
        {
            int cur1 = 0, cur2 = 0;//双指针
            int count = nums.Length;
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] != val)
                {
                    nums[cur1] = nums[cur2];
                    cur1++;
                }
                else//equal
                {
                    count--;
                }

                cur2++;
            }

            return count;
        }

        public static void MoveZeroes(int[] nums)
        {
            if (nums.Length == 1)
            {
                return;
            }

            int cur1 = 0;
            for (int i = 0; i < nums.Length - 1; i++)
            {
                if (nums[cur1] == 0)
                {
                    if (nums[i + 1] != 0)
                    {
                        (nums[cur1], nums[i + 1]) = (nums[i + 1], nums[cur1]);
                        cur1++;
                    }
                }
                else
                {
                    cur1++;
                }
                //无论如何cur2都要加1
            }
        }

        public static void ReverseString(char[] s)
        {
            for (int i = 0; i < s.Length / 2; i++)
            {
                (s[i], s[s.Length - i - 1]) = (s[s.Length - i - 1], s[i]);
            }
        }

        public static string ReplaceSpace(string s)
        {
            int i = 0;
            while (i < s.Length)
            {
                if (s[i] == ' ')
                {
                    string right, left;
                    left = s[0..i];
                    right = s[(i + 1)..];
                    s = left + "%20" + right;

                    i += 3;
                }
                else
                {
                    i++;
                }
            }

            return s;
        }

        public static string ReplaceSpace2(string s)
        {
            int spaceCount = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ')
                {
                    spaceCount++;
                }
            }

            char[] resChars = new char[spaceCount * 2 + s.Length];
            //双指针
            int cur = resChars.Length - 1;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (s[i] == ' ')
                {
                    resChars[cur--] = '0';
                    resChars[cur--] = '2';
                    resChars[cur--] = '%';
                }
                else
                {
                    resChars[cur--] = s[i];
                }
            }

            return new(resChars);

        }

        public static string ReverseWords(string s)
        {
            s = s.Trim();
            List<char> chars = s.ToCharArray().ToList();
            int cur1 = -1;
            int f;
            for (f = 0; f < chars.Count; f++)
            {
                if (chars[f] != ' ')
                {
                    cur1++;
                }
                else
                {
                    if (chars[f - 1] != ' ')
                    {
                        cur1++;
                    }
                }

                chars[cur1] = chars[f];
            }

            chars.RemoveRange(cur1, f - cur1 - 1);

            for (int i = 0; i < chars.Count / 2; i++)
            {
                (chars[i], chars[chars.Count - i - 1]) = (chars[chars.Count - i - 1], chars[i]);
            }//反转所有字符

            chars.AddRange(new char[] { ' ' });
            int space;
            cur1 = 0;
            for (int i = 0; i < chars.Count; i++)
            {
                if (chars[i] == ' ')
                {
                    space = i;
                    for (int j = 0; j < (i - 1 - cur1 + 1) / 2; j++)
                    {
                        (chars[j + cur1], chars[i - 1 - j]) = (chars[i - 1 - j], chars[j + cur1]);
                    }

                    cur1 = space + 1;
                }
            }

            chars.RemoveAt(chars.Count - 1);
            return new(chars.ToArray());
        }

        public static ListNode? GetIntersectionNode(ListNode headA, ListNode headB)
        {
            if (headA is null || headB is null)
            {
                return null;
            }

            ListNode? nodeA = headA;
            ListNode? nodeB = headB;
            int countA = 0, countB = 0;

            while (nodeA is not null)
            {
                countA++;
                nodeA = nodeA.next;
            }
            while (nodeB is not null)
            {
                countB++;
                nodeB = nodeB.next;
            }

            ListNode longer = countA > countB ? headA : headB;
            ListNode shorter = countA <= countB ? headA : headB;
            for (int i = 0; i < Math.Abs(countA - countB); i++)
            {
                longer = longer.next;
            }

            while (true)
            {
                if (longer == shorter)
                {
                    return longer;
                }
                else if (longer is null)
                {
                    return null;
                }

                longer = longer?.next;
                shorter = shorter?.next;

            }
        }

        public static int IsPrefixOfWord(string sentence, string searchWord)
        {
            string[] words = sentence.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].IndexOf(searchWord) == 0)
                {
                    return i + 1;
                }
            }

            return -1;
        }

        public static int IsPrefixOfWord2(string sentence, string searchWord)
        {
            //双指针
            int cur1 = 0;//searchWord
            int cur2 = 0;
            int wordCount = 0;
            for (int i = 0; i < sentence.Length; i++)
            {
                if (i == 0 || sentence[i - 1] == ' ')
                {
                    cur2 = i;
                    wordCount++;
                }
                if (sentence[cur2] == searchWord[cur1])
                {
                    if (cur1 == searchWord.Length - 1)
                    {
                        return wordCount;
                    }

                    cur2++;
                    cur1++;
                }
                else
                {
                    cur1 = 0;
                }
            }

            return -1;
        }

        public static IList<IList<string>> PrintTree(TreeNode root)
        {
            int depth = GetDepthOfBinTree(root);
            _depth = depth;
            int column = (int)Math.Pow(2, depth) - 1;
            string[,] data = new string[depth, column];
            SetTreeForm(data, root, 0, (column - 1) / 2);
            IList<string>[] res = new List<string>[depth];
            for (int row = 0; row < depth; row++)
            {
                if (res[row] is null)
                {
                    res[row] = new string[column].ToList();
                }
                for (int col = 0; col < column; col++)
                {
                    res[row][col] = data[row, col] ?? "";
                }
            }

            return res;
        }

        static int _depth;//二叉树的整棵深度

        private static void SetTreeForm(string[,] data, TreeNode node, int row, int column)
        {
            data[row, column] = node.val.ToString();

            if (node.left is not null)
            {
                SetTreeForm(data, node.left, row + 1, column - (int)Math.Pow(2, _depth - 1 - row - 1));
            }
            if (node.right is not null)
            {
                SetTreeForm(data, node.right, row + 1, column + (int)Math.Pow(2, _depth - 1 - row - 1));
            }//叶子结点自动返回
        }

        public static int CompareVersion(string version1, string version2)
        {
            string[] nums1 = version1.Split('.');
            string[] nums2 = version2.Split('.');
            int count = Math.Max(nums1.Length, nums2.Length);
            for (int i = 0; i < count; i++)
            {
                int a, b;
                a = i >= nums1.Length ? 0 : int.Parse(nums1[i]);
                b = i < nums2.Length ? int.Parse(nums2[i]) : 0;

                if (a > b)
                {
                    return 1;
                }
                else if (a < b)
                {
                    return -1;
                }
            }

            return 0;

        }

        public static TreeNode BstToGst(TreeNode root)
        {
            _BSTSum = 0;
            RecursionBstTraversal(root);
            return root;
        }

        static int _BSTSum;

        public static void RecursionBstTraversal(TreeNode root)
        {
            if (root.right is not null)
            {
                RecursionBstTraversal(root.right);
            }

            _BSTSum += root.val;
            root.val = _BSTSum;

            if (root.left is not null)
            {
                RecursionBstTraversal(root.left);
            }
        }

        public static IList<int> InorderTraversal(TreeNode root)
        {
            Stack<TreeNode> stck = new();
            TreeNode cur = root;
            List<int> res = new();

            while (cur is not null || stck.Count != 0)
            {
                if (cur is not null)
                {
                    stck.Push(cur);
                    cur = cur.left;
                }
                else
                {
                    cur = stck.Pop();
                    res.Add(cur.val);
                    cur = cur.right;
                }

            }

            return res;
        }

        public static int Fibonacci(int n)
        {
            return RecursionFibonacci(1, 1, n);
        }

        private static int RecursionFibonacci(int a, int b, int n)
        {
            if (n == 1) return a;
            else if (n == 2) return b;
            else return RecursionFibonacci(b, a + b, n - 1); //n >= 3
        }

        public static int[][] GenerateMatrix(int n)
        {
            int[][] res = new int[n][];
            for (int i = 0; i < n; i++)
            {
                res[i] = new int[n];
            }//初始化

            int direction = 0;
            int count = 1;
            int x = -1, y = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    //if ((x + 1 >= n || y + 1 >= n || x - 1 < 0 || y - 1 < 0) || (res[x + 1][y] != 0 || res[x - 1][y] != 0 || res[x][y + 1] != 0 || res[x][y - 1] != 0))
                    //{
                    //    direction++;
                    //}
                    switch (direction % 4)
                    {
                        case 0://→
                            x++;
                            if (x >= n || res[y][x] != 0)
                            {
                                x--;
                                direction++;
                                j--;
                                continue;
                            }
                            break;
                        case 1://↓
                            y++;
                            if (y >= n || res[y][x] != 0)
                            {
                                y--;
                                direction++;
                                j--;
                                continue;
                            }
                            break;
                        case 2://←
                            x--;
                            if (x < 0 || res[y][x] != 0)
                            {
                                x++;
                                direction++;
                                j--;
                                continue;
                            }
                            break;
                        case 3://↑
                            y--;
                            if (y < 0 || res[y][x] != 0)
                            {
                                y++;
                                direction++;
                                j--;
                                continue;
                            }
                            break;
                    }

                    res[y][x] = count++;
                }
            }

            return res;
        }

        public static bool CanBeEqual(int[] target, int[] arr)
        {
            Array.Sort(target);
            Array.Sort(arr);
            return target.SequenceEqual(arr);
        }

        public static double MyPow(double x, int n)
        {
            if (n == 0 || x == 1)
            {
                return 1;
            }
            else if (n == 1)
            {
                return x;
            }

            if (n < 0)
            {
                if (n == int.MinValue)
                {
                    double fuck = MyPow(x, -n / 2);
                    return fuck * fuck;
                }
                else
                {
                    return 1 / MyPow(x, -n);
                }

            }
            double res = MyPow(x, n / 2);
            if (n % 2 == 0)
            {
                return res * res;
            }
            else
            {
                return res * res * x;
            }
        }

        public static double MyPow2(double x, int n)
        {
            if (n == 0)
            {
                return 1;
            }
            double contribution = 1;
            int count = Math.Abs(n);
            for (int i = count; i != 1; i /= 2)
            {
                if (i % 2 == 1)
                {
                    contribution *= x;//补齐
                }

                x *= x;//==>>迭代算平方 快速幂
            }

            return n < 0 ? 1 / x * contribution : x * contribution;
        }//不会证明

        public static IList<int> FindClosestElements(int[] arr, int k, int x)
        {
            int[] diff = arr.ToArray();
            for (int i = 0; i < diff.Length; i++)
            {
                diff[i] = Math.Abs(diff[i] - x);
            }

            Array.Sort<int, int>(diff, arr);
            int[] res = arr.ToArray()[..k];//排序不稳定有概率出bug
            Array.Sort(res);
            return res;
        }

        public static ListNode SwapPairs(ListNode head)
        {
            if (head is null || head.next is null)
            {
                return head;
            }

            ListNode last = new(0);
            last.next = head;
            ListNode cur1 = head, cur2 = head.next;
            ListNode res = cur2;
            while (true)
            {
                cur1.next = null;
                last.next = cur2;
                ListNode? tmp = cur2?.next;
                cur2.next = cur1;
                cur1.next = tmp;

                last = cur1;
                cur1 = cur1.next;
                if (cur1 is null || cur1.next is null)
                {//偶数空          奇数空一
                    return res;
                }
                cur2 = cur1.next;
            }
        }

        public static ListNode SwapPairs2(ListNode head)
        {
            if (head is null || head.next is null)
            {
                return head;
            }

            ListNode dummy = new(0);
            dummy.next = head;
            ListNode cur1 = head;
            ListNode cur2 = cur1.next;
            ListNode last = dummy;
            while (true)
            {
                last.next = cur2;
                ListNode tmp = cur2.next;
                cur1.next = null;
                cur2.next = cur1;
                cur1.next = tmp;
                last = cur1;
                cur1 = cur1.next;
                if (cur1 is null)
                {
                    return dummy.next;
                }

                cur2 = cur1.next;
                if (cur2 is null)
                {
                    return dummy.next;
                }

            }
        }

        public int MaxProduct(int[] nums)
        {
            Array.Sort(nums);
            return (nums[^1] - 1) * (nums[^2] - 1);
        }

        public int MaxProduct2(int[] nums)
        {
            int max1 = int.MinValue, max2 = int.MinValue;
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] > max1)
                {
                    max2 = max1;
                    max1 = nums[i];
                }
                else if (nums[i] > max2)
                {
                    max2 = nums[i];
                }
            }

            return (max2 - 1) * (max1 - 1);
        }

        public static int WidthOfBinaryTree(TreeNode root)//超时
        {
            //层序遍历
            List<TreeNode?> level = new()
            {
                root
            };
            int max = 1;
            while (true)
            {
                List<TreeNode?> tmp = new();
                bool isNotNull = true;
                for (int i = 0; i < level.Count; i++)
                {
                    if (level[i]?.left is not null)
                    {
                        isNotNull = false;
                    }
                    if (level[i] is null && i == level.Count - 1)
                    {
                        break;
                    }
                    if (isNotNull && level[i]?.right is null)
                    {
                        continue;
                    }
                    else if (isNotNull)
                    {
                        tmp.Add(level[i]?.right);
                        isNotNull = false;
                        continue;
                    }

                    tmp.Add(level[i]?.left);
                    tmp.Add(level[i]?.right);
                }

                level.Clear();
                level = tmp.ToList();

                int end = -1;
                if (0 == tmp.Count)
                {
                    return max;
                }
                for (int i = 0; i < tmp.Count; i++)
                {
                    if (tmp[i] is not null)
                    {
                        end = i;
                    }

                    max = Math.Max(end + 1, max);
                }
            }
        }

        public static int MinSubArrayLen(int target, int[] nums)
        {
            int cur1 = 0, cur2 = 0, sum = nums[0], minLength = nums.Length;
            bool exist = false;
            while (true)
            {
                if (sum < target)
                {
                    cur2++;
                    if (cur2 >= nums.Length)
                    {
                        return !exist ? 0 : minLength;
                    }

                    sum += nums[cur2];
                }
                else//满足
                {
                    exist = true;
                    minLength = Math.Min(minLength, cur2 - cur1 + 1);
                    sum -= nums[cur1];
                    cur1++;
                }
            }
        }

        public static string MinWindow(string s, string t)
        {
            Dictionary<char, int> dic = new();
            foreach (var item in t)
            {
                if (!dic.ContainsKey(item))
                {
                    dic[item] = 0;
                }
                dic[item]++;
            }
            Dictionary<char, int> set = new();
            int length = s.Length + 1;

            int minI = -1, minJ = -1;
            for (int i = 0; i < s.Length; i++)
            {//i is startIndex
                if (dic.ContainsKey(s[i]))
                {
                    int j = i - 1;//j是右端索引，先加再判断是否包含此字符
                    while (true)
                    {
                        if (IsContainsString(set, dic))
                        {
                            if (j - i + 1 < length)
                            {
                                length = j - i + 1;
                                minI = i;
                                minJ = j;
                            }

                            if (i == s.Length)
                            {
                                break;
                            }
                            if (set.ContainsKey(s[i]))
                            {
                                if (set[s[i]] > 1)
                                {
                                    set[s[i]]--;
                                }
                                else
                                {
                                    set.Remove(s[i]);
                                }
                            }

                            i++;//删

                        }
                        else
                        {
                            j++;
                            if (j == s.Length)//end
                            {
                                break;
                            }

                            if (dic.ContainsKey(s[j]))//判断是否是t的字符
                            {
                                if (!set.ContainsKey(s[j]))
                                {
                                    set.Add(s[j], 0);
                                }
                                set[s[j]]++;
                            }
                        }
                    }

                    break;
                }
            }
            if (minI == -1)//相当于未更新
            {
                return String.Empty;
            }
            return s[minI..(minJ + 1)];
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="s">被查找的字符串</param>
        /// <param name="t">预期包含的字符串</param>
        /// <returns></returns>
        private static bool IsContainsString(Dictionary<char, int> s, Dictionary<char, int> t)
        {

            foreach (var item in t.Keys)
            {
                if (s.ContainsKey(item))
                {
                    if (t[item] > s[item])
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckInclusion(string s1, string s2)
        {
            Dictionary<char, int> dicS1 = new(26);//记录s1
            for (int i = 0; i < s1.Length; i++)
            {
                if (!dicS1.ContainsKey(s1[i]))
                {
                    dicS1.Add(s1[i], 0);
                }
                dicS1[s1[i]]++;
            }

            Dictionary<char, int> dicWin = new(26);//窗口中包含的字符及其数量
            int cur1 = 0, cur2 = 0;
            dicWin.Add(s2[cur1], 1);//在s2中滑动窗口
            while (true)
            {
                if (IsContainsString(dicWin, dicS1))//包含
                {
                    if (cur2 - cur1 + 1 == s1.Length)
                    {
                        return true;
                    }
                    if (dicWin.ContainsKey(s2[cur1]))
                    {
                        dicWin[s2[cur1]]--;
                    }

                    cur1++;
                }
                else//not enough
                {
                    cur2++;
                    if (cur2 == s2.Length)
                    {
                        break;
                    }
                    if (dicS1.ContainsKey(s2[cur2]))
                    {
                        if (!dicWin.ContainsKey(s2[cur2]))
                        {
                            dicWin[s2[cur2]] = 0;
                        }
                        dicWin[s2[cur2]]++;
                    }
                }
            }

            return false;
        }

        public static int[] Shuffle(int[] nums, int n)
        {
            int[] res = new int[2 * n];
            int index = 0;
            for (int i = 0; i < n; i++)
            {
                res[index++] = nums[i];
                res[index++] = nums[i + n];
            }

            return res;
        }

        public static void Merge(int[] nums1, int m, int[] nums2, int n)
        {
            int cur1 = m - 1;
            int cur2 = n - 1;
            int targetIndex = nums1.Length - 1;//从倒数第一个开始
            while (cur2 >= 0)//双指针倒叙遍历第二个数组
            {
                if (cur1 < 0)
                {
                    nums1[0] = int.MinValue;//出现了第一个数组中第一个数都比第二个数组的前n（n>=2）数还大的情况，需要第一个数组的指针不动，第二个数组的所有元素依次全部加入到头部
                    cur1 = 0;
                }
                if (nums1[cur1] > nums2[cur2])
                {
                    nums1[targetIndex--] = nums1[cur1--];
                }
                else if (nums1[cur1] < nums2[cur2])
                {
                    nums1[targetIndex--] = nums2[cur2--];
                }
                else
                {
                    nums1[targetIndex--] = nums2[cur2--];
                    nums1[targetIndex--] = nums1[cur1--];
                }
            }
        }

        public static TreeNode? InsertIntoMaxTree(TreeNode root, int val)
        {
            //遍历
            TreeNode dummyNode = new(-1, null, root);
            TreeNode cur1 = dummyNode;//last
            TreeNode? cur2 = root;//this
            while (true)
            {
                int val2;
                val2 = cur2 is null ? int.MinValue : cur2.val;

                if (val > val2)
                {
                    cur1.right = new(val, cur2);
                    return dummyNode.right;
                }
                else
                {
                    cur1 = cur2;
                    cur2 = cur2.right;
                }
            }
        }

        public static IList<string> FindRepeatedDnaSequences(string s)
        {
            //长度为10
            int length = 10;
            HashSet<string> set = new();
            HashSet<string> res = new();
            for (int i = 0; i < s.Length - length + 1; i++)
            {
                string tmp = s[i..(i + length)];
                if (!set.Add(tmp))
                {
                    res.Add(tmp);
                }

            }

            return res.ToList();
        }

        public static int[] SortedSquares(int[] nums)
        {

            int cur1 = 0, cur2 = 0;
            List<int> res = new(nums.Length);
            if (nums[^1] < 0)//先找位置
            {
                cur1 = cur2 = nums.Length - 1;
            }
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] == 0)
                {
                    cur1 = cur2 = i;
                }
                if (i < nums.Length - 1 && nums[i] < 0 && nums[i + 1] > 0)
                {
                    cur1 = i;
                    cur2 = i + 1;
                }
            }
            if (cur1 == cur2)
            {
                cur2++;
            }

            while (true)
            {
                int left = int.MaxValue, right = int.MaxValue;
                if (cur1 >= 0)
                {
                    left = nums[cur1] * nums[cur1];
                }
                if (cur2 < nums.Length)
                {
                    right = nums[cur2] * nums[cur2];
                }
                if (left == right && right == int.MaxValue)
                {
                    return res.ToArray();
                }

                if (left < right)
                {
                    res.Add(left);
                    cur1--;
                }
                else
                {
                    res.Add(right);
                    cur2++;
                }
            }
        }

        public static bool ValidateStackSequences(int[] pushed, int[] popped)
        {
            Stack<int> stck = new();
            int j = 0;
            for (int i = 0; i < pushed.Length; i++)
            {
                stck.Push(pushed[i]);
                while (stck.Peek() == popped[j])
                {
                    stck.Pop();
                    j++;

                    if (stck.Count == 0)
                    {
                        break;
                    }
                }
            }

            return stck.Count == 0;
        }

        public static ListNode? ReverseList(ListNode head)
        {
            if (head is null || head.next is null)
            {
                return head;
            }

            ListNode cur1 = head;
            ListNode? cur2 = head.next;
            cur1.next = null;

            while (cur2 is not null)
            {
                ListNode? tmp = cur2.next;
                cur2.next = cur1;
                cur1 = cur2;
                cur2 = tmp;
            }

            return cur1;
        }

        public static bool IsHappy(int n)
        {
            HashSet<int> path = new();
            while (true)
            {
                int[] tmp = GetDigits(n);
                n = 0;
                for (int i = 0; i < tmp.Length; i++)
                {
                    n += tmp[i] * tmp[i];
                }

                if (!path.Add(n))
                {
                    return false;
                }

                if (n == 1)
                {
                    return true;
                }
            }
        }

        private static int[] GetDigits(int n)
        {
            List<int> digits = new(10);
            while (n != 0)
            {
                digits.Add(n % 10);
                n /= 10;
            }

            return digits.ToArray();
        }

        public static int FourSumCount(int[] nums1, int[] nums2, int[] nums3, int[] nums4)
        {
            //nums1 nums2
            int n = nums1.Length;
            Dictionary<int, int> dic12 = new();//sum-->>ways
            Dictionary<int, int> dic34 = new();//sum-->>ways
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int sum = nums1[i] + nums2[j];
                    if (!dic12.ContainsKey(sum))
                    {
                        dic12[sum] = 0;
                    }
                    dic12[sum]++;

                    int sum2 = nums3[i] + nums4[j];
                    if (!dic34.ContainsKey(sum2))
                    {
                        dic34[sum2] = 0;
                    }
                    dic34[sum2]++;
                }
            }

            int resultCount = 0;
            foreach (var pairs in dic12)
            {
                if (dic34.ContainsKey(-pairs.Key))
                {
                    resultCount += dic34[-pairs.Key] * dic12[pairs.Key];
                }
            }

            return resultCount;
        }

        public static bool CanConstruct(string ransomNote, string magazine)
        {
            Dictionary<char, int> dic = new(26);
            foreach (var item in magazine)
            {
                if (!dic.ContainsKey(item))
                {
                    dic[item] = 0;
                }
                dic[item]++;
            }

            foreach (var item in ransomNote)
            {
                if (!dic.ContainsKey(item) || (dic.ContainsKey(item) && dic[item] == 0))
                {
                    return false;
                }
                else
                {
                    dic[item]--;
                }
            }

            return true;
        }

        public static IList<IList<int>> ThreeSum(int[] nums)
        {
            Array.Sort(nums);
            if (nums[0] * nums[^1] > 0)
            {
                return Array.Empty<IList<int>>();
            }

            int n = nums.Length;
            Dictionary<int, int> dic1 = new();//cur0 value -->> last  index
            for (int i = 0; i < n; i++)
            {
                dic1[nums[i]] = i;
            }

            List<IList<int>> res = new();
            for (int i = 0; i < n; i++)
            {
                if (i > 0 && nums[i] == nums[i - 1])
                {
                    continue;
                }
                for (int j = i + 1; j < n; j++)
                {
                    if ((j > i + 1 && nums[j] == nums[j - 1])
                        || !dic1.ContainsKey(-(nums[i] + nums[j])))
                    {
                        continue;
                    }
                    else
                    {
                        int targetIndex = dic1[-(nums[i] + nums[j])];
                        if (targetIndex <= i || targetIndex <= j)//强行对三数排序
                        {
                            continue;
                        }

                        res.Add(new List<int>(new int[] { nums[i], nums[j], -(nums[i] + nums[j]) }));
                    }
                }
            }

            return res;
        }

        public static IList<IList<int>> ThreeSum2(int[] nums)
        {
            Array.Sort(nums);
            List<IList<int>> res = new();
            HashSet<string> set = new();
            if (nums[0] * nums[^1] > 0)
            {
                return Array.Empty<IList<int>>();
            }

            for (int cur0 = 0; cur0 < nums.Length - 2; cur0++)
            {
                //cur0去重
                if (cur0 >= 1 && nums[cur0 - 1] == nums[cur0])// cur0 cur0 cur1 cur1 cur1 cur2 cur2 cur2
                {
                    continue;
                }

                int cur1 = cur0 + 1, cur2 = nums.Length - 1;
                while (cur1 < cur2)
                {//双指针处理2-sum
                    int twoSum = nums[cur1] + nums[cur2];
                    if (twoSum > -nums[cur0])
                    {
                        cur2--;
                    }
                    else if (twoSum < -nums[cur0])
                    {
                        cur1++;
                    }
                    else//equal
                    {
                        res.Add(new int[] { nums[cur0], nums[cur1], nums[cur2] });
                        //选择了某一组数 在选择这一组数后更改指针位置时要跨过重复元素
                        while (cur1 + 1 < nums.Length && nums[cur1 + 1] == nums[cur1])
                        {
                            cur1++;
                        }
                        while (cur2 - 1 >= 0 && nums[cur2 - 1] == nums[cur2])
                        {
                            cur2--;
                        }
                        cur1++;
                        cur2--;
                    }
                }
            }

            return res;
        }

        public static IList<IList<int>> FourSum(int[] nums, int target)
        {
            Array.Sort(nums);
            int n = nums.Length;

            if (nums[0] > 0 && nums[0] > target)
            {
                return Array.Empty<IList<int>>();
            }
            List<IList<int>> res = new();//-2 -1 0 0 1 2
            for (int i = 0; i < n; i++)
            {
                if (i > 0 && nums[i] == nums[i - 1])
                {
                    continue;
                }
                for (int j = i + 1; j < n; j++)
                {
                    if (j > i + 1 && nums[j] == nums[j - 1])
                    {
                        continue;
                    }

                    int cur1 = j + 1, cur2 = n - 1;
                    while (cur1 < cur2)
                    {
                        int sum = nums[i] + nums[j] + nums[cur1] + nums[cur2];
                        if (sum > target)
                        {
                            cur2--;
                        }
                        else if (sum < target)
                        {
                            cur1++;
                        }
                        else//equal     -1 -1 0 1 2 2
                        {
                            res.Add(new int[] { nums[i], nums[j], nums[cur1], nums[cur2] });
                            cur1++;
                        }

                        while (cur1 < cur2 && cur1 - 1 > j && nums[cur1] == nums[cur1 - 1])
                        {
                            cur1++;
                        }
                        while (cur1 < cur2 && cur2 + 1 < nums.Length && nums[cur2] == nums[cur2 + 1])
                        {
                            cur2--;
                        }
                    }
                }
            }

            return res;
        }

        public static int NumSpecial(int[][] mat)
        {
            int count = 0;
            for (int i = 0; i < mat.Length; i++)
            {
                int index = -1;
                bool satisfy = false;
                for (int j = 0; j < mat[0].Length; j++)
                {
                    if (mat[i][j] == 1)
                    {
                        if (index == -1 && !satisfy)
                        {
                            satisfy = true;
                            index = j;
                        }
                        else if (satisfy)
                        {
                            satisfy = false;
                            break;
                        }
                    }
                }
                if (satisfy)
                {
                    //检查index列
                    for (int u = 0; u < mat.Length; u++)
                    {
                        if (mat[u][index] == 1 && u != i)
                        {
                            satisfy = false;
                            break;
                        }
                    }
                }

                if (satisfy)
                {
                    count++;
                }
            }

            return count;
        }

        public static string ReverseStr(string s, int k)
        {
            bool needReverse = true;
            char[] chars = s.ToCharArray();
            int startIndex = 0;
            while (true)
            {
                if (needReverse)
                {
                    if (s.Length - startIndex < k)
                    {
                        k = s.Length - startIndex;
                    }

                    for (int i = 0; i < k / 2; i++)//i is count
                    {
                        (chars[i + startIndex], chars[startIndex + k - i - 1]) = (chars[startIndex + k - i - 1], chars[i + startIndex]);
                    }
                }

                startIndex += k;
                needReverse = !needReverse;

                if (startIndex >= s.Length - 1)
                {
                    return new(chars);
                }
            }
        }

        public static string ReverseLeftWords(string s, int n)
        {
            char[] source = s.Reverse().ToArray();
            Array.Reverse(source, 0, s.Length - n);
            Array.Reverse(source, s.Length - n, n);
            return new(source);
        }

        public static int StrStr(string haystack, string needle)
        {
            int cur1 = 0, cur2 = 0;
            int index = 0;
            while (cur2 < needle.Length)//&& cur1 < haystack.Length)
            {
                if (cur1 >= haystack.Length)
                {
                    return -1;
                }
                if (haystack[cur1] != needle[cur2])
                {
                    cur1 = index + 1;
                    cur2 = 0;
                    index = cur1;
                }
                else
                {
                    cur1++;
                    cur2++;
                }
            }

            return index;
        }

        public static int[] GetPrefixArr(string s)
        {
            int[] next = new int[s.Length];
            next[0] = 0;
            int cur1 = 0;//前缀头
            int cur2 = 1;//后缀头
            for (int i = 1; i < s.Length; i++)
            {

                if (s[cur1] == s[cur2])
                {

                }
            }
        }
    }

    public class MyCircularQueue
    {
        int[] data;
        int N;
        int first = 0, last = 0;

        public MyCircularQueue(int k)
        {
            data = new int[k];
            //N = k;
        }

        public bool EnQueue(int value)
        {
            if (N == data.Length)
            {
                return false;
            }
            data[last] = value;
            last = (last + 1) % this.data.Length;
            N++;
            return true;
        }

        public bool DeQueue()
        {
            if (N == 0)
            {
                return false;
            }
            data[first] = default;
            first = (first + 1) % data.Length;
            N--;

            return true;
        }

        public int Front()
        {
            if (N == 0)
            {
                return -1;
            }
            else
            {
                return data[first];
            }
        }

        public int Rear()
        {
            if (this.N == 0)
            {
                return -1;
            }
            else
            {
                return data[(last - 1 + data.Length) % data.Length];
            }
        }

        public bool IsEmpty() => first == last;

        public bool IsFull() => (last + 1) % data.Length == first;
    }

    public class MyCircularDeque
    {
        int[] data;

        int start = 0;//先设置再移动
        int end = 0;//先移动再设置
        int N = 0;

        public MyCircularDeque(int k) => data = new int[k];

        public bool InsertFront(int value)
        {
            if (N == data.Length)
            {
                return false;
            }
            data[start] = value;
            start = (start + data.Length - 1) % data.Length;//此时start处的数组成员空
            N++;
            return true;
        }

        public bool InsertLast(int value)
        {
            if (N == data.Length)
            {
                return false;
            }
            end = (end + data.Length + 1) % data.Length;//此时start处的数组成员空
            data[end] = value;
            N++;
            return true;
        }

        public bool DeleteFront()
        {
            if (N == 0)
            {
                return false;
            }
            start = (start + data.Length + 1) % data.Length;
            data[start] = default;
            N--;
            return true;
        }

        public bool DeleteLast()
        {
            if (N == 0)
            {
                return false;
            }
            data[end] = default;
            end = (end + data.Length - 1) % data.Length;
            N--;
            return true;
        }

        public int GetFront()
        {
            if (N == 0)
            {
                return -1;
            }
            else
            {
                return data[(start + data.Length + 1) % data.Length];
            }
        }

        public int GetRear()
        {
            if (N == 0)
            {
                return -1;
            }
            else
            {
                return data[end];
            }
        }

        public bool IsEmpty() => N == 0;

        public bool IsFull() => N == data.Length;
    }

    public class MyLinkedListNoDummy
    {
        private class Node
        {
            public int val;
            public Node? next;

            public Node(int val, Node? next = null)
            {
                this.val = val;
                this.next = next;
            }
        }

        private Node? head;

        int N = 0;

        public MyLinkedListNoDummy()
        {

        }

        public int Get(int index)
        {
            Node? cur = head;
            for (int i = 0; i < index; i++)
            {
                cur = cur?.next;
            }
            return cur is null ? -1 : cur.val;
        }

        public void AddAtHead(int val)
        {
            if (N == 0)
            {
                head = new(val);
            }
            else
            {
                head = new(val, head);
            }
            N++;
        }

        public void AddAtTail(int val)
        {
            Node cur = head;

            if (N == 0)
            {
                head = new(val);
                N++;
                return;
            }
            while (true)
            {
                if (cur.next is null)
                {
                    cur.next = new(val);
                    N++;
                    return;
                }

                cur = cur.next;
            }
        }

        public void AddAtIndex(int index, int val)
        {
            if (index > N)
            {
                return;
            }
            if (index <= 0)
            {
                AddAtHead(val);
            }
            else if (index == N)
            {
                AddAtTail(val);
            }
            else
            {
                Node? cur = head;
                for (int i = 0; i < index - 1; i++)
                {
                    cur = cur?.next;
                }

                if (cur is null)
                {
                    return;
                }
                else
                {
                    Node? tmp = cur.next;
                    cur.next = new(val, tmp);
                }

                N++;
            }
        }

        public void DeleteAtIndex(int index)
        {
            if (index >= N || index < 0)
            {
                return;
            }

            if (index == 0)
            {
                head = head?.next;
                N--;
                return;
            }

            Node? cur = head;
            for (int i = 0; i < index - 1; i++)
            {
                cur = cur?.next;
            }
            if (cur is null)
            {
                return;
            }

            cur.next = cur.next?.next;
            N--;
        }
    }

    public class MyLinkedList
    {
        private class Node
        {
            public int val;
            public Node? next;

            public Node(int val, Node? next = null)
            {
                this.val = val;
                this.next = next;
            }
        }

        private Node dummyhead;

        int N = 0;

        public MyLinkedList()
        {
            dummyhead = new(-1, null);//dummy
        }

        public int Get(int index)
        {
            Node? cur = dummyhead;
            for (int i = 0; i < index + 1; i++)
            {
                cur = cur?.next;
            }
            return cur is null ? -1 : cur.val;
        }

        public void AddAtHead(int val)
        {
            dummyhead.next = new(val, dummyhead.next);

            N++;
        }

        public void AddAtTail(int val)
        {
            Node cur = dummyhead;

            while (true)
            {
                if (cur.next is null)
                {
                    cur.next = new(val);
                    N++;
                    return;
                }

                cur = cur.next;
            }
        }

        public void AddAtIndex(int index, int val)
        {
            Node? cur = dummyhead;
            for (int i = 0; i < index; i++)
            {
                cur = cur?.next;
            }

            if (cur is null)
            {
                return;
            }
            else
            {
                Node? tmp = cur?.next;
                cur.next = new(val, tmp);
                N++;
            }

        }

        public void DeleteAtIndex(int index)
        {
            if (index >= N || index < 0)
            {
                return;
            }

            Node? cur = dummyhead;
            for (int i = 0; i < index; i++)
            {
                cur = cur?.next;
            }

            cur.next = cur.next?.next;
            N--;
        }
    }

}