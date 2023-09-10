using System.ComponentModel;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using 树;
using 队列;

namespace Algorithm {
    internal static partial class Exercises {
        public static int[] TwoSumVer1(int[] nums, int target) {//O (N^2) --->  O (N LogN)
            int[] sorted = new int[nums.Length];
            nums.CopyTo(sorted, 0);
            Array.Sort(sorted);
            //排序
            int length = nums.Length;
            int num1 = -1;
            int num2 = -1;
            for (int i = 0; i < length; i++) {
                if (Array.BinarySearch(sorted, target - sorted[i]) >= 0) {
                    num2 = target - sorted[i];
                    num1 = sorted[i];
                    break;
                }
            }

            for (int i = 0; i < length; i++) {
                if (num1 == -1 && num1 == nums[i]) {
                    num1 = i;
                    break;
                }
                if (num2 == -1 && num2 == nums[^(i + 1)]) {
                    num2 = nums.Length - i - 1;
                    break;
                }
            }
            return new int[] { num1, num2 };
        }

        public static int[] TwoSumVer2(int[] nums, int target) {
            int length = nums.Length;
            Dictionary<int, List<int>> dic = new();
            for (int i = 0; i < length; i++) {
                if (!dic.ContainsKey(nums[i])) {
                    dic.Add(nums[i], new List<int>(new int[] { i }));
                } else {
                    dic[nums[i]].Add(i);
                }
            }

            for (int i = 0; i < length; i++) {
                int j = target - nums[i];
                if (!dic.ContainsKey(j)) {
                    continue;
                }

                if (dic[j].Count != 0) {
                    if (dic[j].Contains(i)) {
                        if (dic[j].Count == 1) {
                            continue;
                        } else {
                            dic[j].Remove(i);
                        }
                    }
                    return new int[2] { i, dic[j][0] };
                }

            }

            return Array.Empty<int>();
        }

        public static int[] TwoSumVer3(int[] nums, int target) {
            Dictionary<int, int> sb = new();
            for (int i = 0; i < nums.Length; i++) {
                if (sb.ContainsKey(target - nums[i])) {
                    return new int[2] { i, sb[target - nums[i]] };
                }
                sb[nums[i]] = i;
            }
            return Array.Empty<int>();
        }

        public static int BinarySearch(int[] nums, int element) {
            int length = nums.Length;
            int min = 0, max = length - 1;
            while (min <= max) {
                int mid = min + (max - min) / 2;
                if (nums[mid] > element) {
                    max = mid - 1;
                } else if (nums[mid] < element) {
                    min = mid + 1;
                } else {
                    //j找到
                    return mid;
                }
            }

            return -1;
        }

        public static int BinarySearchLower(int[] nums, int element) {
            int length = nums.Length;
            int min = 0, max = length - 1;
            while (min < max) {
                int mid = min + (max - min + 1) / 2;
                if (nums[mid] > element) {
                    max = mid - 1;
                } else if (nums[mid] < element) {
                    min = mid;
                } else {
                    //j找到
                    return mid;
                }
            }

            return min;
        }

        //输入：nums1 = [4,9,5], nums2 = [9,4,9,8,4]
        //4 5 9, 4 4 8 9 9
        //输出：[9,4]
        public static int[] GetUnite(int[] nums1, int[] nums2) {
            Dictionary<int, int> dic = new();
            Dictionary<int, int> Repeating = new();
            for (int i = 0; i < nums1.Length; i++) {
                dic[nums1[i]] = 1;
            }
            for (int i = 0; i < nums2.Length; i++) {
                if (dic.ContainsKey(nums2[i])) {
                    Repeating[nums2[i]] = 1;
                }
            }
            return Repeating.Keys.ToArray();
        }

        public static bool IsAnagramVer1(string s, string t) {
            Dictionary<char, int> dic = new();
            foreach (var item in s) {
                if (dic.ContainsKey(item)) {
                    dic[item]++;
                } else {
                    dic[item] = 1;
                }
            }

            foreach (var item in t) {
                if (dic.ContainsKey(item)) {
                    dic[item]--;
                } else {
                    return false;
                }
            }

            foreach (var item in dic.Keys) {
                if (dic[item] != 0) {
                    return false;
                }
            }

            return true;
        }

        public static bool IsAnagramVer2(string s, string t) {
            //输入: s = "anagram", t = "nagaram"
            //输出: true
            char[] chars1 = s.ToCharArray();
            char[] chars2 = t.ToCharArray();
            Array.Sort(chars1);
            Array.Sort(chars2);
            return chars1.SequenceEqual(chars2);
        }

        public static int Reverse(int x) {
            int minus = 1;
            if (x == 0 || x == int.MinValue) return 0;
            if (x < 0) {
                minus = -1;
                x = -x;
            }//处理正负

            int result = 0;
            int k = 0;//Length
            List<int> reversedInt = new(10);
            while (true) {
                int tmp = GetDigit(x, k);
                if (tmp == -1) {
                    break;
                }
                reversedInt.Add(tmp);
                k++;
            }
            if (k == 10 && reversedInt[0] >= 3) {
                return 0;
            }
            for (int i = 0; i < k - 1; i++) {
                result += reversedInt[k - 1 - i] * powsTen[i];
            }

            if (reversedInt[0] == 2) {
                if (result > 147483647) {
                    return 0;
                }
            }
            result += reversedInt[0] * powsTen[k - 1];
            return minus * result;
        }

        public static bool IsPalindrome(int x) {
            if (x < 0) {
                return false;
            }
            //123321
            List<int> ints = new();
            int i = 0;
            while (true) {
                int digit = GetDigit(x, i);
                if (digit == -1) {
                    break;
                }
                ints.Add(digit);
                i++;
            }

            for (int j = 0; j < i / 2; j++) {
                if (ints[j] != ints[i - j - 1]) {
                    return false;
                }
            }
            return true;
        }

        public static bool IsPalindrome2(int x) {
            if (x < 0) {
                return false;
            }
            int help = 1;
            while (x / help >= 10) {
                help *= 10;
            }
            while (help > 1) {
                if (x / help != x % 10) {
                    return false;
                }
                x %= help;
                x /= 10;
                help /= 100;
            }
            return true;
        }

        static int[] powsTen = { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000 };

        private static int GetDigit(int num, int index) {
            int a = index >= 10 ? 0 : num / powsTen[index];
            return a == 0 ? -1 : a % 10;
        }

        public static bool ValidSquare(int[] p1, int[] p2, int[] p3, int[] p4) {
            HashSet<double> set = new(6)
            {
                GetDistance(p1, p2),
                GetDistance(p1, p3),
                GetDistance(p1, p4),
                GetDistance(p2, p3),
                GetDistance(p2, p4),
                GetDistance(p3, p4)
            };
            if (set.Contains(0)) {
                return false;
            }
            if (set.Count == 2) {
                return true;
            }
            return false;
        }

        private static double GetDistance(int[] p1, int[] p2) {
            return Math.Sqrt((p1[0] - p2[0]) * (p1[0] - p2[0]) + (p1[1] - p2[1]) * (p1[1] - p2[1]));
        }

        public static int[] ArrayRankTransform(int[] arr) {
            int[] source = new int[arr.Length];
            Array.Copy(arr, source, arr.Length);
            Dictionary<int, int> dic = new();
            Array.Sort(arr);
            int j = 1;
            for (int i = 0; i < arr.Length; i++) {
                if (!dic.ContainsKey(arr[i])) {
                    dic[arr[i]] = j;
                } else {
                    continue;
                }
                j++;
            }

            int[] res = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++) {
                res[i] = dic[source[i]];
            }
            return res;
        }

        public static string FractionAddition(string expression) {
            char[] possibleOperators = new char[] { '-', '+' };
            string[] nums = expression.Split(possibleOperators, StringSplitOptions.RemoveEmptyEntries);
            List<char> operators = new(10);
            List<string> integr = new(20);//综合起来了
            int j = 0;
            for (int i = 0; i < expression.Length;) {
                if (expression[i] == '-' || expression[i] == '+') {
                    operators.Add(expression[i]);
                    i++;
                } else {
                    if (j < nums.Length) {
                        i += nums[j++].Length;
                    }
                }
            }
            if (operators.Count != nums.Length) {
                operators.Insert(0, '+');
            }
            integr.Add("0");

            for (int i = 0; i < operators.Count; i++) {
                integr.Add(operators[i].ToString());
                integr.Add(nums[i].ToString());
            }//分割完毕

            Stack<string> stackOperators = new(10);
            Stack<string> stackNums = new(10);
            foreach (var item in integr) {
                if (item == "+" || item == "-") {
                    stackOperators.Push(item);
                } else//num
                  {
                    stackNums.Push(item);
                    if (stackNums.Count == 2) {
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

        private static void Simplify(int[] fraction) {
            if (fraction[0] == 0) {
                fraction[1] = 1;
                return;
            } else {
                int st = fraction[0], nd = fraction[1];
                while (true) {
                    //欧几里得算法:fraction[0] & fraction[1]
                    int mod = st % nd;
                    if (mod == 0) {
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
            if (n1.Length == 1) {
                n1 = "0/1";
            } else if (n2.Length == 1) {
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
            return string.Format("{0}/{1}", result[0], result[1]);
        }

        public static long Fractorial(int num) {
            return TailRecursionFractorial(num, num - 1);
        }

        private static long TailRecursionFractorial(long n1, long n2) {
            return n2 == 1 ? n1 : TailRecursionFractorial(n1 * n2, n2 - 1);
        }

        public static int Peach(int remain, int times) {
            return RecursionPeach(remain, times);
        }

        private static int RecursionPeach(int peaches, int times) {
            if (times == 0) {
                return peaches;
            } else {
                return RecursionPeach(2 * (peaches + 1), times - 1);
            }
        }

        public static string GenerateTheString(int n) {
            StringBuilder result = new();
            for (int i = 0; i < n - 1; i++) {
                result.Append('w');
            }
            if (n % 2 == 0) {
                result.Append('a');
            } else {
                result.Append('w');
            }
            return result.ToString();
        }

        public static int MaxLevelSum(TreeNode root) {
            Stack<TreeNode> stck = new();
            stck.Push(root);
            int maxSum = int.MinValue;
            int currentlevel = 1;
            int maxLevel = 0;
            while (true) {
                List<TreeNode> nodeList = new();
                while (stck.Count != 0) {
                    nodeList.Add(stck.Pop());
                }//下一级的所有节点都在nodeList中

                int thisSum = 0;
                for (int i = 0; i < nodeList.Count; i++) {
                    thisSum += nodeList[i].val;
                }

                if (thisSum > maxSum) {
                    maxSum = thisSum;
                    maxLevel = currentlevel;
                }
                for (int i = 0; i < nodeList.Count; i++) {
                    if (nodeList[i].left is not null) {
                        stck.Push(nodeList[i].left!);
                    }
                    if (nodeList[i].right is not null) {
                        stck.Push(nodeList[i].right!);
                    }
                }
                if (stck.Count == 0) {
                    break;
                }

                currentlevel++;
            }
            return maxLevel;
        }

        public static int MinDepth(TreeNode root) {
            return CursionGetMinDepth(root, 0);
        }

        private static int CursionGetMinDepth(TreeNode node, int level) {
            //终止条件
            if (node is null) {
                return level;
            } else if (IsLeafNode(node)) {
                return level + 1;
            } else {
                int a = int.MaxValue, b = int.MaxValue;
                if (node.left is not null) {
                    a = CursionGetMinDepth(node.left, level + 1);
                }
                if (node.right is not null) {
                    b = CursionGetMinDepth(node.right, level + 1);
                }
                //int b = CursionGetMinDepth(node.right, level);
                return a >= b ? b : a;
            }
        }

        private static bool IsLeafNode(TreeNode node) => node.left is null && node.right is null;

        public static int LengthOfLongestSubstringVer1(string s) {
            int length = s.Length;
            for (int i = length; i >= 2; i--)//i个
            {

                for (int j = 0; j <= length - i; j++) {
                    Dictionary<char, int> dic = new();
                    //chars = new(s[j..(i + j)]);
                    //if (chars.Count == i)
                    //{
                    //    return i;
                    //}
                    int k;
                    for (k = j; k < i + j; k++) {
                        if (dic.ContainsKey(s[k])) {
                            break;
                        }
                        dic[s[k]] = 1;
                    }
                    if (k == i + j) {
                        return i;
                    }
                }
            }
            return 1;
        }

        public static int LengthOfLongestSubstringVer2(string s) {
            int length = s.Length;
            int max = 0;
            for (int i = 0; i < length; i++)//固定i不动了，不完全是滑动窗口
            {
                int j = i;//j为可扩展到的最大索引
                HashSet<char> set = new(256);

                while (true) {
                    if (j == length) {
                        break;
                    }
                    if (!set.Add(s[j])) {
                        break;
                    }

                    if (j - i + 1 > max) {
                        max = j - i + 1;
                    }

                    j++;
                }
            }
            return max;
        }

        public static int LengthOfLongestSubstringVer3(string s) {
            //滑动窗口：双指针
            int a = 0, b = 0;//from to
            int max = 0;
            while (true) {
                if (a == s.Length) {
                    break;
                }

                if (b + 1 > s.Length) {
                    break;
                }
                if (IsAllImparity(s[a..(b + 1)])) {
                    max = max < b - a + 1 ? b - a + 1 : max;

                    b++;
                } else {
                    a++;
                }
            }

            return max;
        }

        private static bool IsAllImparity(string str) {
            HashSet<char> values = new();
            foreach (var item in str) {
                if (!values.Add(item)) {
                    return false;
                }
            }
            return true;
        }

        public static int NumColor(TreeNode root) {
            ints.Clear();
            CursionNumColor(root);
            return ints.Count;
        }

        public static HashSet<int> ints = new();

        private static void CursionNumColor(TreeNode node) {

            ints.Add(node.val);
            if (node.left is not null) {
                CursionNumColor(node.left);
            }
            if (node.right is not null) {
                CursionNumColor(node.right);
            }
        }

        /*  1+2)*3-4)*5-6)))
         *  转换为中序表达式：
         *  ((1+2)*((3-4)*(5-6)))
        */
        public static IList<int> MinSubsequence(int[] nums) {
            int sum = 0;
            foreach (var item in nums) {
                sum += item;
            }
            int[] sorted = new int[nums.Length];
            Array.Copy(nums, sorted, nums.Length);
            Array.Sort(sorted);
            sorted = sorted.Reverse().ToArray();
            int newSum = 0;//代表目前子序列之和
            List<int> res = new();
            for (int i = 0; i < nums.Length; i++) {
                newSum += sorted[i];
                res.Add(sorted[i]);
                if (newSum > sum - newSum) {
                    break;
                }
            }
            return res;
        }

        public static IList<string> LetterCombinations(string digits) {
            if (string.IsNullOrEmpty(digits)) {
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
            if (digits.Length == 1) {
                string res = dicNumToAlphabet[int.Parse(digits)];
                string[] arr = new string[res.Length];
                for (int i = 0; i < res.Length; i++) {
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
            for (int i = 0; i < digits.Length; i++) {
                ints[i] = int.Parse(digits[i].ToString());
            }
            int first = int.Parse(digits[0].ToString());
            return RecursionCombine(first, ints[1..(digits.Length)]);

        }

        static Dictionary<int, string> dicNumToAlphabet = new();

        private static string[] RecursionCombine(int firstDigi, params int[] digits) {
            //int[] dg = new int[digits.Length + 1];
            //dg[0] = firstDigi;
            //for (int i = 1; i < digits.Length; i++)
            //{
            //    dg[i] = int.Parse(digits[i].ToString());
            //}

            if (digits.Length >= 2) {
                string[] res = RecursionCombine(digits[0], digits[1..(digits.Length)]);
                List<string> strings = new();
                for (int i = 0; i < res.Length; i++) {
                    strings.AddRange(Combine2(dicNumToAlphabet[firstDigi], res[i]));
                }
                return strings.ToArray();
            } else {
                string txt1 = dicNumToAlphabet[firstDigi];
                string txt2 = dicNumToAlphabet[digits[0]];
                return Combine(txt1, txt2);
            }
        }

        private static string[] Combine(string txt1, string txt2) {
            List<string> res = new();
            for (int i = 0; i < txt1.Length; i++) {
                for (int j = 0; j < txt2.Length; j++) {
                    res.Add(txt1[i].ToString() + txt2[j]);
                }
            }

            return res.ToArray();
        }

        private static string[] Combine2(string txt1, string nondetachableTxt2) {
            string[] res = new string[txt1.Length];
            for (int i = 0; i < txt1.Length; i++) {
                res[i] = (txt1[i].ToString() + nondetachableTxt2);
            }

            return res;
        }

        public static TreeNode AddOneRow(TreeNode root, int val, int depth) {
            if (depth == 1) {
                TreeNode nRoot = new(val, root);
                return nRoot;
            }// > 1
             //广度 搜到depth-1层
            Queue<TreeNode> que = new();
            List<TreeNode> thsLevelsNode = new();
            que.Enqueue(root);
            for (int level = 1; level < depth - 1; level++) {
                thsLevelsNode.Clear();
                while (que.Count != 0) {
                    thsLevelsNode.Add(que.Dequeue());
                }

                foreach (var treeNode in thsLevelsNode) {
                    if (treeNode.left is not null) {
                        que.Enqueue(treeNode.left);
                    }
                    if (treeNode.right is not null) {
                        que.Enqueue(treeNode.right);
                    }
                }
            }//depth - 1 层结果在que里存储

            while (que.Count != 0) {
                TreeNode thsNode = que.Dequeue();
                TreeNode? thsLeft = thsNode.left, thsRight = thsNode.right;
                //添加左右字树val
                thsNode.left = new(val, thsLeft);
                thsNode.right = new(val, null, thsRight);
            }

            return root;
        }

        public static int BinSearch(int[] arr, int value) {
            int left = 0, right = arr.Length;
            int mid;
            while (left <= right) {
                mid = (left + right) / 2;
                if (arr[mid] > value) {
                    right = mid - 1;
                } else if (arr[mid] < value) {
                    left = mid;
                } else {
                    return mid;
                }
            }

            return -1;
        }

        public static IList<string> StringMatching(string[] words)//每个单词都不一样
        {
            BubbleSort(words);

            HashSet<string> result = new();
            for (int i = 0; i < words.Length; i++) {
                for (int j = i + 1; j < words.Length; j++) {
                    if (words[j].IndexOf(words[i]) != -1) {
                        result.Add(words[i]);
                    }
                }
            }

            return result.ToArray();
        }

        private static void BubbleSort(string[] values) {
            for (int i = 0; i < values.Length; i++) {
                for (int j = 0; j < values.Length - i - 1; j++) {
                    if (values[j].Length > values[j + 1].Length) {
                        (values[j], values[j + 1]) = (values[j + 1], values[j]);
                    }
                }
            }
        }

        public static int LengthOfLIS(int[] nums) {
            int max = 0;
            int tmp;
            for (int i = 0; i < nums.Length; i++) {
                tmp = RecursionLengthOfLIS(nums[i..]);
                if (tmp > max) {
                    max = tmp;
                }
            }

            return max;
        }

        private static int RecursionLengthOfLIS(int[] nums) {
            if (nums.Length == 1) {
                return 1;
            }

            int max = 1;
            for (int i = 1; i < nums.Length; i++) {
                if (nums[i] > nums[0]) {
                    max = Math.Max(max, 1 + RecursionLengthOfLIS(nums[i..]));
                }
            }

            return max;
        }

        public static bool IsBalanced(TreeNode root) {
            RecursionIsBalanced(root);
            return isBaln;
        }

        static bool isBaln = true;

        private static int RecursionIsBalanced(TreeNode? root) {
            if (root is null) {
                return 0;
            }
            if (root.left is null && root.right is null) {
                return 1;
            }
            int leftLv = 1 + RecursionIsBalanced(root.left);
            int rightLv = 1 + RecursionIsBalanced(root.right);
            if (Math.Abs(leftLv - rightLv) > 1) {
                isBaln = false;
            }
            return Math.Max(leftLv, rightLv);
        }

        private static int GetDepthOfBinTree(TreeNode? tree) {
            if (tree is null) {
                return 0;
            }
            if (tree.left is null && tree.right is null) {
                return 1;
            }
            return Math.Max(1 + GetDepthOfBinTree(tree.left), 1 + GetDepthOfBinTree(tree.right));
        }

        public static bool IsValidBST(TreeNode root) {
            InOrderBinT(root);
            return IsValid;
        }

        static long previosVal = long.MinValue;
        static bool IsValid = true;

        private static void InOrderBinT(TreeNode? root) {
            if (root is null) {
                return;
            }

            InOrderBinT(root.left);
            if (root.val > previosVal) {
                previosVal = root.val;
                InOrderBinT(root.right);
            } else {
                IsValid = false;
                return;
            }
        }

        public static int LengthOfLIS2(int[] nums) {
            int max = 1;
            for (int i = 0; i < nums.Length; i++)//遍历一遍从哪里开始子序列最长
            {
                max = Math.Max(RecursionLengthOfLIS2(nums[i..]), max);
            }
            return max;
        }

        private static int RecursionLengthOfLIS2(int[] nums) {
            if (nums.Length == 1) {
                return 1;
            }

            int max = 1;
            for (int i = 1; i < nums.Length; i++)//遍历当前数组有没有比第一位更大的
            {
                if (nums[i] > nums[0]) {
                    max = Math.Max(max, 1 + RecursionLengthOfLIS2(nums[i..]));
                }
            }

            return max;
        }

        static IList<int> _pathCombine = new List<int>();
        static IList<IList<int>> _resultCombine = new List<IList<int>>();

        public static int MinStartValue(int[] nums) {
            //-3 2 -3 4 2
            int accumulate = 0;
            int min = 0;
            for (int i = 0; i < nums.Length; i++) {
                accumulate += nums[i];
                min = Math.Min(min, accumulate);
            }

            return 1 - min;
        }

        public static int FindMaxConsecutiveOnes(int[] nums) {
            int max = 1;
            int thsCount = 0;
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] == 1) {
                    thsCount++;
                } else {
                    max = Math.Max(max, thsCount);
                    thsCount = 0;
                }
            }

            max = Math.Max(max, thsCount);
            return max;
        }

        public static int FindPoisonedDuration(int[] timeSeries, int duration) {
            int result = 0;
            int length = timeSeries.Length;
            if (length == 1) {
                return duration;
            }

            if (timeSeries[1] - timeSeries[0] > duration) {
                result += duration * 2;
            } else {
                result += timeSeries[1] + duration - timeSeries[0];
            }

            for (int i = 2; i < length; i++) {
                if (timeSeries[i] - timeSeries[i - 1] > duration) {
                    result += duration;
                } else {
                    result += timeSeries[i] - timeSeries[i - 1];
                }

            }

            return result;
        }

        public static IList<IList<int>> Combine(int n, int k) {
            CombineBacktracking(n, k, 0);
            return _resultCombine;
        }

        /// <summary>
        /// 回溯求组合数
        /// </summary>
        /// <param name="n">集合总位数</param>
        /// <param name="k">组合位数</param>
        public static void CombineBacktracking(int n, int k, int startIndex) {
            if (_pathCombine.Count == k) {
                int[] tmp = new int[k];
                _pathCombine.CopyTo(tmp, 0);
                _resultCombine.Add(tmp);
                //_path.Clear();
                return;
            }
            //start
            for (int i = startIndex; n - i + _pathCombine.Count >= k; i++) {
                _pathCombine.Add(i + 1);//从1开始比从0开始要多一
                CombineBacktracking(n, k, i + 1);
                _pathCombine.RemoveAt(_pathCombine.Count - 1);

            }
        }

        static int _pathSum;
        static IList<int> _pathCombinationSum = new List<int>();
        static IList<IList<int>> _resultSum = new List<IList<int>>();
        static int[]? _sourceCandidates;

        public static IList<IList<int>> CombinationSum(int[] candidates, int target) {
            _resultSum.Clear();
            Array.Sort(candidates);
            _sourceCandidates = candidates;
            BacktrackingCombinationSum(0, target);
            return _resultSum;
        }

        public static void BacktrackingCombinationSum(int startIndex, int target) {
            //if (_pathSum > target)
            //{
            //    return;
            //}

            if (_pathSum == target) {
                int[] tmp = _pathCombinationSum.ToArray();
                _resultSum.Add(tmp);
                return;
            }

            for (int i = startIndex; i < _sourceCandidates!.Length; i++) {
                _pathSum += _sourceCandidates[i];
                _pathCombinationSum.Add(_sourceCandidates[i]);
                if (_pathSum > target) {
                    _pathSum -= _sourceCandidates[i];
                    _pathCombinationSum.RemoveAt(_pathCombinationSum.Count - 1);
                    break;
                }
                BacktrackingCombinationSum(i, target);
                _pathSum -= _sourceCandidates[i];
                _pathCombinationSum.RemoveAt(_pathCombinationSum.Count - 1);

            }
        }

        public static string SolveEquation(string equation) {
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

            for (int j = 0; j < 2; j++) {
                if (equa[j][0] != '-') {
                    equa[j] = equa[j].Insert(0, "+");
                }
            }


            Queue<char> stck = new();

            for (int j = 0; j < 2; j++) {
                for (int i = 0; i < equa[j].Length; i++) {
                    if (equa[j][i] == '+' || equa[j][i] == '-') {
                        stck.Enqueue(equa[j][i]);//dispose the sign
                    }
                }
            }

            for (int j = 0; j < 2; j++) {
                for (int i = 0; i < items[j].Count; i++) {
                    bool isPlus = stck.Dequeue() == '+';
                    if (int.TryParse(items[j][i].ToString(), out int num1)) {
                        constant[j] = isPlus ? constant[j] + num1 : constant[j] - num1;
                    } else//X
                      {
                        int facto;
                        if (items[j][i].Length == 1) {
                            facto = 1;
                        } else {
                            facto = int.Parse(items[j][i][..^1]);
                        }
                        factorX[j] = isPlus ? factorX[j] + facto : factorX[j] - facto;
                    }
                }
            }

            if (factorX[0] == factorX[1] && constant[0] == constant[1]) {
                return "Infinite solutions";
            } else if (factorX[0] == factorX[1] && constant[0] != constant[1]) {
                return "No solution";
            } else {
                return String.Format("x={0}", (-constant[0] + constant[1]) / (factorX[0] - factorX[1]));
            }
        }

        public static IList<IList<string>> Partition(string s) {
            _resultPartition.Clear();
            _pathPartition.Clear();
            _pathPartition.Add(s);
            BacktrackingPartition(s, 1);
            string[][] res = new string[_resultPartition.Count][];
            for (int i = 0; i < res.Length; i++) {
                res[i] = _resultPartition[i].ToArray();
            }
            return _resultPartition;
        }

        static List<string> _pathPartition = new();
        static IList<IList<string>> _resultPartition = new List<IList<string>>();

        private static void BacktrackingPartition(string s, int startIndex)//开始应为1
        {
            //长4的字符串，能分4下
            if (startIndex >= s.Length) {
                bool leaf = true;
                for (int i = 0; i < _pathPartition.Count; i++) {
                    if (!IsPalindromeString(_pathPartition[i])) {
                        leaf = false;
                        break;
                    }
                }
                if (leaf) {
                    string[] tmp = _pathPartition.ToArray();
                    if (tmp[^1] == "") {
                        _resultPartition.Add(tmp[..(_pathPartition.Count - 1)]);
                    } else {
                        _resultPartition.Add(tmp);
                    }
                }

                return;

            }


            //如何按索引分割字符串[1-length]
            for (int i = startIndex; i <= s.Length; i++) {
                _pathPartition = SplitLastStr(i, _pathPartition.ToArray());//尝试分割
                                                                           //BacktrackingPartition(_pathPartition[_pathPartition.Count - 1], 1);
                BacktrackingPartition(s, i + 1);
                _pathPartition = MergeLastTwoStr(_pathPartition.ToArray());//试图还原
            }

        }

        private static List<string> SplitLastStr(int interspaceIndex, params string[] str) {
            int a = 0;
            for (int i = 0; i < str.Length - 1; i++) {
                a += str[i].Length;
            }
            interspaceIndex -= a;
            if (str[^1].Length == 0) {
                return str.ToList();
            }
            List<string> strs = new(str);
            string tmp = strs[^1];
            strs.RemoveAt(strs.Count - 1);
            strs.Add(tmp[..interspaceIndex]);
            strs.Add(tmp[interspaceIndex..]);
            return strs;
        }

        private static List<string> MergeLastTwoStr(params string[] str) {
            List<string> strs = new(str);
            strs[^2] += strs[^1];
            strs.RemoveAt(strs.Count - 1);
            return strs;
        }

        public static bool IsPalindromeString(string str) {
            for (int i = 0; i < str.Length / 2; i++) {
                if (str[i] != str[str.Length - i - 1]) {
                    return false;
                }
            }

            return true;
        }

        public static IList<IList<int>> CombinationSum3(int k, int n) {
            _resultCombinationSum3.Clear();
            BacktrackingCombinationSum3(k, 1, n);

            return _resultCombinationSum3;
        }

        static IList<int> _pathCombinationSum3 = new List<int>();
        static IList<IList<int>> _resultCombinationSum3 = new List<IList<int>>();
        static int sumCombinationSum3 = 0;

        private static void BacktrackingCombinationSum3(int remainLevel, int startNum, int target) {
            if (remainLevel == 0) {
                if (target == sumCombinationSum3) {
                    int[] tmp = new int[_pathCombinationSum3.Count];
                    _pathCombinationSum3.CopyTo(tmp, 0);
                    _resultCombinationSum3.Add(tmp);
                }

                return;
            }

            //[1--9]
            for (int i = startNum; i <= 9; i++) {
                if (9 - startNum + 1 < remainLevel) {
                    break;
                }

                _pathCombinationSum3.Add(i);
                sumCombinationSum3 += i;

                BacktrackingCombinationSum3(remainLevel - 1, i + 1, target);
                _pathCombinationSum3.RemoveAt(_pathCombinationSum3.Count - 1);
                sumCombinationSum3 -= i;
            }
        }

        public static IList<IList<string>> GetAllSubstring(string source) {
            _resultGetAllSubstring.Clear();
            BacktrackingGetAllSubstring(source, 0);
            return _resultGetAllSubstring;
        }

        static IList<string> _pathGetAllSubstring = new List<string>();
        static IList<IList<string>> _resultGetAllSubstring = new List<IList<string>>();

        private static void BacktrackingGetAllSubstring(string s, int startIndex) {
            if (startIndex + 1 > s.Length) {
                string[] tmp = _pathGetAllSubstring.ToArray();
                _resultGetAllSubstring.Add(tmp);
                return;
            }

            for (int i = startIndex; i < s.Length; i++) {
                _pathGetAllSubstring.Add(s[startIndex..(i + 1)]);//添加当前一部分 左闭右开
                BacktrackingGetAllSubstring(s, i + 1);
                _pathGetAllSubstring.RemoveAt(_pathGetAllSubstring.Count - 1);
            }
        }

        public static int Knapsack(int[] values, int[] bulks, int maxBulk) {
            int[,] dp = new int[maxBulk, values.Length];
            /*
             3000 4             dp[bulk, item] item is value
             2000 3             dp[j, i]
             1500 1
             */
            List<int> path = new();

            for (int k = 0; k < maxBulk; k++) {
                if (bulks[0] <= k + 1) {
                    dp[k, 0] = values[0];
                }
            }

            for (int i = 1; i < values.Length; i++) {
                for (int j = 0; j < maxBulk; j++)//j+1才是体积
                {
                    int another;
                    if (j + 1 - bulks[i] == 0) {
                        another = 0;
                    } else if (j + 1 - bulks[i] > 0) {
                        another = dp[j - bulks[i], i - 1];
                    } else {
                        another = -values[i];//不能选了
                    }

                    if (dp[j, i - 1] > values[i] + another) {
                        dp[j, i] = dp[j, i - 1];
                    } else {
                        dp[j, i] = values[i] + another;
                        path.Add(i);
                    }

                }
            }

            Console.WriteLine(PrintTwoDimensionArr(dp));
            for (int i = 0; i < path.Count; i++) {
                Console.WriteLine(path[i]);
            }
            return dp[maxBulk - 1, values.Length - 1];
        }

        public static string PrintTwoDimensionArr(int[,] arr) {
            StringBuilder sb = new();
            for (int i = 0; i < arr.GetLength(1); i++) {
                for (int j = 0; j < arr.GetLength(0); j++) {
                    sb.Append(arr[j, i] + ", ");
                }

                sb.Append('\n');
            }

            return sb.ToString();

        }

        public static int MaxScore(string s) {
            int max;
            int score = 0;
            for (int i = s.Length - 1; i >= 1; i--) {
                if (s[i] == '1') {
                    score++;
                }
            }
            if (s[0] == '0') {
                score++;
            }//先处理一次
            max = score;

            for (int i = 1; i < s.Length - 1; i++) {
                if (s[i] == '0') {
                    //----->>>从左向右
                    score++;
                } else {
                    score--;
                }

                max = Math.Max(max, score);
            }

            return max;
        }

        public static ListNode? DetectCycle(ListNode head) {
            ListNode? fast;
            ListNode? slow;
            fast = slow = head;
            while (true) {
                fast = fast?.next?.next;
                slow = slow?.next;
                if (fast is null || slow is null) {
                    return null;
                }

                if (fast == slow) {
                    ListNode met = fast;
                    ListNode root = head;
                    while (true) {
                        if (met == root) {
                            return met;
                        }

                        met = met.next;
                        root = root.next;
                    }
                }
            }

        }

        public static int Rob(int[] nums) {//2 7 9 3 1
            return DPRob(nums);
        }

        private static int RecursionRob(int[] nums, int startIndex)//纯暴力搜索
        {
            switch (nums.Length - startIndex) {
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

        public static int DPRob(int[] nums) {
            switch (nums.Length) {
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
            for (int i = 2; i < nums.Length; i++) {
                int tmp = Math.Max(dp[1], dp[0] + nums[i]);
                dp[0] = dp[1];
                dp[1] = tmp;
            }

            return dp[1];

        }

        public static int FindContentChildren(int[] g, int[] s) {
            Array.Sort(g);// 7 8 9 10
            Array.Sort(s);// 5 6 7 8
            int count = 0;
            int cur1 = 0, cur2 = 0;
            if (g.Length == 0 || s.Length == 0) {
                return 0;
            }
            do {
                if (s[cur2] >= g[cur1]) {
                    cur1++;
                    count++;
                }
                cur2++;

            } while (cur2 < s.Length && cur1 < g.Length);

            return count;
        }

        public static int WiggleMaxLength(int[] nums) {
            //1 7 4 9 2 5 贪心
            int count = 0;
            int sign = 0;
            for (int i = 0; i < nums.Length - 1; i++) {
                int tmp = Math.Sign(nums[i + 1] - nums[i]);
                if (sign != tmp && tmp != 0) {
                    count++;
                    sign = tmp;
                }
            }

            return count + 1;
        }

        public static int MaxSubArray(int[] nums) {
            int sum = 0;
            int max = int.MinValue;
            for (int i = 0; i < nums.Length; i++) {
                sum += nums[i];
                max = Math.Max(sum, max);
                if (sum < 0) {
                    sum = 0;//重新开始
                }

            }

            return max;
        }

        public static int DPMaxSubArray(int[] nums) {
            int[] dp = new int[nums.Length];
            dp[^1] = nums[^1];
            int res = dp[^1];
            for (int i = nums.Length - 2; i >= 0; i--) {
                dp[i] = Math.Max(nums[i], nums[i] + dp[i + 1]);
                res = Math.Max(dp[i], res);//不确定从哪开始的连续子数组和最大，所以要在遍历的时候寻找最大值
            }
            return res;
        }

        public static int DeepestLeavesSum(TreeNode root) {
            Queue<TreeNode> que = new();
            List<TreeNode> nodes = new();
            que.Enqueue(root);
            while (true) {
                nodes.Clear();
                while (que.Count != 0) {
                    nodes.Add(que.Dequeue());
                }
                bool leaf = true;
                int sum = 0;
                foreach (var item in nodes) {
                    sum += item.val;
                    if (item.left is not null) {
                        que.Enqueue(item.left);
                        leaf = false;
                    } else {
                        leaf = leaf && true;
                    }
                    if (item.right is not null) {
                        que.Enqueue(item.right);
                        leaf = false;
                    } else {
                        leaf = leaf && true;
                    }
                }

                if (leaf) {
                    return sum;
                }

            }
        }

        public static IList<string> RestoreIpAddresses(string s) {
            _resultIP.Clear();
            BacktrackingRestoreIpAddresses(s, 0, 4);
            return _resultIP;
        }

        static IList<string> _pathIP = new List<string>(4);
        static IList<string> _resultIP = new List<string>();

        public static void BacktrackingRestoreIpAddresses(string s, int startIndex, int level) {
            if (s.Length - startIndex > level * 3)//剩的太多了，分不完了
            {
                return;
            }

            if (level == 1)//还差最后一个部分，判断是否能作为ip地址
            {
                if (int.TryParse(s[startIndex..], out int res))//是否到尾变成了空字符
                {//NO 
                    if (res <= 255 && (s[startIndex..][0] != '0' || s[startIndex..].Length <= 1)) {//可以一战
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

                if (int.Parse(_pathIP[_pathIP.Count - 1]) <= 255 && (_pathIP[_pathIP.Count - 1][0] != '0' || _pathIP[_pathIP.Count - 1].Length <= 1)) {
                    BacktrackingRestoreIpAddresses(s, i + 1, level - 1);
                }

                _pathIP.RemoveAt(_pathIP.Count - 1);
            }
        }

        public static string Convert(string s, int numRows) {
            if (numRows == 1) {
                return s;
            }
            StringBuilder[] txt = new StringBuilder[numRows];
            int k = 0;//count
            bool plus = true;
            for (int i = 0; i < s.Length; i++) {
                if (txt[k] is null) {
                    txt[k] = new();
                }
                txt[k].Append(s[i]);

                if (k == numRows - 1 || (k == 0
                                         && i != 0)) {//k已达到最大值，应折回
                    plus = !plus;
                }

                k = plus ? k + 1 : k - 1;
            }

            StringBuilder result = new();
            for (int i = 0; i < numRows; i++) {
                result.Append(txt[i]);
            }

            return result.ToString();

        }

        public static int BusyStudent(int[] startTime, int[] endTime, int queryTime) {
            int count = 0;
            for (int i = 0; i < startTime.Length; i++) {
                if ((queryTime >= startTime[i] && queryTime <= endTime[i])) {
                    count++;
                }
            }

            return count;
        }

        public static IList<IList<int>> Subsets(int[] nums) {
            _resultSubsets.Clear();
            BacktrackingSubsets(nums, 0);
            return _resultSubsets;
        }

        static IList<int> _pathSubsets = new List<int>();
        static IList<IList<int>> _resultSubsets = new List<IList<int>>();

        public static void BacktrackingSubsets(int[] nums, int startIndex) {
            int[] tmp = _pathSubsets.ToArray();
            _resultSubsets.Add(tmp);
            for (int i = startIndex; i < nums.Length; i++) {
                _pathSubsets.Add(nums[i]);
                BacktrackingSubsets(nums, i + 1);
                _pathSubsets.RemoveAt(_pathSubsets.Count - 1);
            }
        }

        public static IList<IList<int>> CombinationSum2(int[] candidates, int target) {
            int[] sorted = candidates.ToArray();
            Array.Sort(sorted);
            _resultCombinationSum2.Clear();
            BacktrackingCombinationSum2(sorted, target, 0, new bool[candidates.Length]);
            return _resultCombinationSum2;
        }

        static IList<int> _pathCombinationSum2 = new List<int>();
        static int _sumCombinationSum2 = 0;
        static IList<IList<int>> _resultCombinationSum2 = new List<IList<int>>();

        public static void BacktrackingCombinationSum2(int[] candidates, int target, int startIndex, bool[] status) {
            //status最开始为全FALSE
            //candi应该已排序好
            if (_sumCombinationSum2 > target) {
                return;
            } else if (_sumCombinationSum2 == target) {
                int[] tmp = _pathCombinationSum2.ToArray();
                _resultCombinationSum2.Add(tmp);
            }

            for (int i = startIndex; i < candidates.Length; i++) {
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

        public static TreeNode ConstructMaximumBinaryTree(int[] nums) {
            return RecursionConstructMaximumBinaryTree(nums);
        }

        private static TreeNode RecursionConstructMaximumBinaryTree(int[] nums) {
            if (nums.Length == 1) {
                return new(nums[0]);
            }
            int max = 0;
            int indx = 0;//max indx
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] > max) {
                    max = nums[i];
                    indx = i;
                }
            }

            TreeNode tree = new(max);
            if (indx == 0) {
                tree.right = RecursionConstructMaximumBinaryTree(nums[1..]);
            } else if (indx == nums.Length - 1) {
                tree.left = RecursionConstructMaximumBinaryTree(nums[..^1]);
            } else {
                tree.right = RecursionConstructMaximumBinaryTree(nums[(indx + 1)..]);
                tree.left = RecursionConstructMaximumBinaryTree(nums[..indx]);
            }

            return tree;
        }

        public static string IntToRoman(int num) {
            string source = num.ToString();
            int thsNum;
            string res = "";
            for (int i = source.Length - 1; i >= 0; i--) {
                thsNum = powsTen[i] * GetDigit(num, i);
                string add = "";
                switch (thsNum) {
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
                        if (thsNum > 10 && thsNum < 40) {
                            add = new('X', GetDigit(num, i));
                        } else if (thsNum > 50 && thsNum < 90) {
                            add = "L";
                            add += new string('X', GetDigit(num, i) - 5);
                        } else if (thsNum > 100 && thsNum < 400) {
                            add = new('C', GetDigit(num, i));
                        } else if (thsNum > 500 && thsNum < 900) {
                            add = "D";
                            add += new string('C', GetDigit(num, i) - 5);
                        } else if (thsNum > 1000) {
                            add = new('M', GetDigit(num, i));
                        }
                        break;
                }
                res += add;
            }

            return res;
        }

        public static string CountAndSay(int n) {
            string[] nums = new string[n + 1];
            nums[1] = "1";
            string pre = nums[1].ToString();
            for (int i = 2; i <= n; i++) {//生成每一个数
                string res = null;
                int count = 1;
                int thsNum = int.Parse(pre[0].ToString()); ;
                int left, right = 0;
                while (true)//滑窗
                {
                    if (right == pre.Length - 1) {
                        res += count;
                        res += thsNum;
                        break;
                    }
                    if (pre[right] == pre[right + 1]) {
                        thsNum = int.Parse(pre[right].ToString());
                        right++;
                        count++;
                    } else {
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

        public static ListNode? RemoveNthFromEnd(ListNode head, int n) {
            if (head.next is null) {
                return null;
            }

            List<ListNode?> node = new(32);
            ListNode? cur = head;
            while (cur is not null) {
                node.Add(cur);
                cur = cur.next;
            }

            if (n == node.Count) {
                return node[1];
            }

            node.Add(null);
            node[^(n + 2)].next = node[^(n)];
            return node[0];
        }

        public static int RemoveElement(int[] nums, int val) {
            int cur1 = 0, cur2 = 0;//双指针
            int count = nums.Length;
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] != val) {
                    nums[cur1] = nums[cur2];
                    cur1++;
                } else//equal
                  {
                    count--;
                }

                cur2++;
            }

            return count;
        }

        public static void MoveZeroes(int[] nums) {
            if (nums.Length == 1) {
                return;
            }

            int cur1 = 0;
            for (int i = 0; i < nums.Length - 1; i++) {
                if (nums[cur1] == 0) {
                    if (nums[i + 1] != 0) {
                        (nums[cur1], nums[i + 1]) = (nums[i + 1], nums[cur1]);
                        cur1++;
                    }
                } else {
                    cur1++;
                }
                //无论如何cur2都要加1
            }
        }

        public static void ReverseString(char[] s) {
            for (int i = 0; i < s.Length / 2; i++) {
                (s[i], s[s.Length - i - 1]) = (s[s.Length - i - 1], s[i]);
            }
        }

        public static string ReplaceSpace(string s) {
            int i = 0;
            while (i < s.Length) {
                if (s[i] == ' ') {
                    string right, left;
                    left = s[0..i];
                    right = s[(i + 1)..];
                    s = left + "%20" + right;

                    i += 3;
                } else {
                    i++;
                }
            }

            return s;
        }

        public static string ReplaceSpace2(string s) {
            int spaceCount = 0;
            for (int i = 0; i < s.Length; i++) {
                if (s[i] == ' ') {
                    spaceCount++;
                }
            }

            char[] resChars = new char[spaceCount * 2 + s.Length];
            //双指针
            int cur = resChars.Length - 1;
            for (int i = s.Length - 1; i >= 0; i--) {
                if (s[i] == ' ') {
                    resChars[cur--] = '0';
                    resChars[cur--] = '2';
                    resChars[cur--] = '%';
                } else {
                    resChars[cur--] = s[i];
                }
            }

            return new(resChars);

        }

        public static string ReverseWords(string s) {
            s = s.Trim();
            List<char> chars = s.ToCharArray().ToList();
            int cur1 = -1;
            int f;
            for (f = 0; f < chars.Count; f++) {
                if (chars[f] != ' ') {
                    cur1++;
                } else {
                    if (chars[f - 1] != ' ') {
                        cur1++;
                    }
                }

                chars[cur1] = chars[f];
            }

            chars.RemoveRange(cur1, f - cur1 - 1);

            for (int i = 0; i < chars.Count / 2; i++) {
                (chars[i], chars[chars.Count - i - 1]) = (chars[chars.Count - i - 1], chars[i]);
            }//反转所有字符

            chars.AddRange(new char[] { ' ' });
            int space;
            cur1 = 0;
            for (int i = 0; i < chars.Count; i++) {
                if (chars[i] == ' ') {
                    space = i;
                    for (int j = 0; j < (i - 1 - cur1 + 1) / 2; j++) {
                        (chars[j + cur1], chars[i - 1 - j]) = (chars[i - 1 - j], chars[j + cur1]);
                    }

                    cur1 = space + 1;
                }
            }

            chars.RemoveAt(chars.Count - 1);
            return new(chars.ToArray());
        }

        public static ListNode? GetIntersectionNode(ListNode headA, ListNode headB) {
            if (headA is null || headB is null) {
                return null;
            }

            ListNode? nodeA = headA;
            ListNode? nodeB = headB;
            int countA = 0, countB = 0;

            while (nodeA is not null) {
                countA++;
                nodeA = nodeA.next;
            }
            while (nodeB is not null) {
                countB++;
                nodeB = nodeB.next;
            }

            ListNode longer = countA > countB ? headA : headB;
            ListNode shorter = countA <= countB ? headA : headB;
            for (int i = 0; i < Math.Abs(countA - countB); i++) {
                longer = longer.next;
            }

            while (true) {
                if (longer == shorter) {
                    return longer;
                } else if (longer is null) {
                    return null;
                }

                longer = longer?.next;
                shorter = shorter?.next;

            }
        }

        public static int IsPrefixOfWord(string sentence, string searchWord) {
            string[] words = sentence.Split(' ');
            for (int i = 0; i < words.Length; i++) {
                if (words[i].IndexOf(searchWord) == 0) {
                    return i + 1;
                }
            }

            return -1;
        }

        public static int IsPrefixOfWord2(string sentence, string searchWord) {
            //双指针
            int cur1 = 0;//searchWord
            int cur2 = 0;
            int wordCount = 0;
            for (int i = 0; i < sentence.Length; i++) {
                if (i == 0 || sentence[i - 1] == ' ') {
                    cur2 = i;
                    wordCount++;
                }
                if (sentence[cur2] == searchWord[cur1]) {
                    if (cur1 == searchWord.Length - 1) {
                        return wordCount;
                    }

                    cur2++;
                    cur1++;
                } else {
                    cur1 = 0;
                }
            }

            return -1;
        }

        public static IList<IList<string>> PrintTree(TreeNode root) {
            int depth = GetDepthOfBinTree(root);
            _depth = depth;
            int column = (int)Math.Pow(2, depth) - 1;
            string[,] data = new string[depth, column];
            SetTreeForm(data, root, 0, (column - 1) / 2);
            IList<string>[] res = new List<string>[depth];
            for (int row = 0; row < depth; row++) {
                if (res[row] is null) {
                    res[row] = new string[column].ToList();
                }
                for (int col = 0; col < column; col++) {
                    res[row][col] = data[row, col] ?? "";
                }
            }

            return res;
        }

        static int _depth;//二叉树的整棵深度

        private static void SetTreeForm(string[,] data, TreeNode node, int row, int column) {
            data[row, column] = node.val.ToString();

            if (node.left is not null) {
                SetTreeForm(data, node.left, row + 1, column - (int)Math.Pow(2, _depth - 1 - row - 1));
            }
            if (node.right is not null) {
                SetTreeForm(data, node.right, row + 1, column + (int)Math.Pow(2, _depth - 1 - row - 1));
            }//叶子结点自动返回
        }

        public static int CompareVersion(string version1, string version2) {
            string[] nums1 = version1.Split('.');
            string[] nums2 = version2.Split('.');
            int count = Math.Max(nums1.Length, nums2.Length);
            for (int i = 0; i < count; i++) {
                int a, b;
                a = i >= nums1.Length ? 0 : int.Parse(nums1[i]);
                b = i < nums2.Length ? int.Parse(nums2[i]) : 0;

                if (a > b) {
                    return 1;
                } else if (a < b) {
                    return -1;
                }
            }

            return 0;

        }

        public static TreeNode BstToGst(TreeNode root) {
            _BSTSum = 0;
            RecursionBstTraversal(root);
            return root;
        }

        static int _BSTSum;

        private static void RecursionBstTraversal(TreeNode root) {
            if (root.right is not null) {
                RecursionBstTraversal(root.right);
            }

            _BSTSum += root.val;
            root.val = _BSTSum;

            if (root.left is not null) {
                RecursionBstTraversal(root.left);
            }
        }

        public static IList<int> InorderTraversal(TreeNode root) {
            Stack<TreeNode> stck = new();
            TreeNode cur = root;
            List<int> res = new();

            while (cur is not null || stck.Count != 0) {
                if (cur is not null) {
                    stck.Push(cur);
                    cur = cur.left;
                } else {
                    cur = stck.Pop();
                    res.Add(cur.val);
                    cur = cur.right;
                }

            }

            return res;
        }

        public static long Fibonacci(int n) => RecursionFibonacci(1, 1, n);

        private static long RecursionFibonacci(long a, long b, int n) {
            if (n == 1) return a;
            if (n == 2) return b;
            else return RecursionFibonacci(b, a + b, n - 1); //n >= 3
        }

        public static int[][] GenerateMatrix(int n) {
            int[][] res = new int[n][];
            for (int i = 0; i < n; i++) {
                res[i] = new int[n];
            }//初始化

            int direction = 0;
            int count = 1;
            int x = -1, y = 0;
            for (int j = 0; j < n * n; j++) {
                switch (direction % 4) {
                    case 0://→
                        x++;
                        if (x >= n || res[y][x] != 0) {
                            x--;
                            direction++;
                            j--;
                            continue;
                        }
                        break;
                    case 1://↓
                        y++;
                        if (y >= n || res[y][x] != 0) {
                            y--;
                            direction++;
                            j--;
                            continue;
                        }
                        break;
                    case 2://←
                        x--;
                        if (x < 0 || res[y][x] != 0) {
                            x++;
                            direction++;
                            j--;
                            continue;
                        }
                        break;
                    case 3://↑
                        y--;
                        if (y < 0 || res[y][x] != 0) {
                            y++;
                            direction++;
                            j--;
                            continue;
                        }
                        break;
                }

                res[y][x] = count++;
            }


            return res;
        }

        public static bool CanBeEqual(int[] target, int[] arr) {
            Array.Sort(target);
            Array.Sort(arr);
            return target.SequenceEqual(arr);
        }

        public static double MyPow(double x, int n) {
            if (n == 0 || x == 1) {
                return 1;
            } else if (n == 1) {
                return x;
            }

            if (n < 0) {
                if (n == int.MinValue) {
                    double fuck = MyPow(x, -n / 2);
                    return fuck * fuck;
                } else {
                    return 1 / MyPow(x, -n);
                }

            }
            double res = MyPow(x, n / 2);
            if (n % 2 == 0) {
                return res * res;
            } else {
                return res * res * x;
            }
        }

        public static double MyPow2(double x, long n) {
            if (n == 0 || x == 1) {
                return 1;
            }
            if (n < 0) {
                return 1 / MyPow2(x, -n);
            }
            if (x == -1) {
                return (n & 1) == 0 ? 1 : -1;
            }
            double multiplied = x;
            double res = 1;
            while (n != 0) {
                long flag = n & 1;
                if (flag == 1) {
                    res *= multiplied;
                }
                multiplied *= multiplied;
                n >>= 1;
            }
            return res;
        }//二进制快速幂

        public static IList<int> FindClosestElements(int[] arr, int k, int x) {
            int[] diff = arr.ToArray();
            for (int i = 0; i < diff.Length; i++) {
                diff[i] = Math.Abs(diff[i] - x);
            }

            Array.Sort<int, int>(diff, arr);
            int[] res = arr.ToArray()[..k];//排序不稳定有概率出bug
            Array.Sort(res);
            return res;
        }

        public static ListNode SwapPairs(ListNode head) {
            if (head is null || head.next is null) {
                return head;
            }

            ListNode last = new(0) { next = head };
            ListNode cur1 = head, cur2 = head.next;
            ListNode res = cur2;
            while (true) {
                cur1.next = null;
                last.next = cur2;
                ListNode? tmp = cur2?.next;
                cur2.next = cur1;
                cur1.next = tmp;

                last = cur1;
                cur1 = cur1.next;
                if (cur1 is null || cur1.next is null) {//偶数空          奇数空一
                    return res;
                }
                cur2 = cur1.next;
            }
        }

        public static ListNode SwapPairs2(ListNode head) {
            if (head is null || head.next is null) {
                return head;
            }

            ListNode dummy = new(0) { next = head };
            ListNode cur1 = head;
            ListNode cur2 = cur1.next;
            ListNode last = dummy;
            while (true) {
                last.next = cur2;
                ListNode tmp = cur2.next;
                cur1.next = null;
                cur2.next = cur1;
                cur1.next = tmp;
                last = cur1;
                cur1 = cur1.next;
                if (cur1 is null) {
                    return dummy.next;
                }

                cur2 = cur1.next;
                if (cur2 is null) {
                    return dummy.next;
                }

            }
        }

        public static int MaxProduct(int[] nums) {
            Array.Sort(nums);
            return (nums[^1] - 1) * (nums[^2] - 1);
        }

        public static int MaxProduct2(int[] nums) {
            int max1 = int.MinValue, max2 = int.MinValue;
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] > max1) {
                    max2 = max1;
                    max1 = nums[i];
                } else if (nums[i] > max2) {
                    max2 = nums[i];
                }
            }

            return (max2 - 1) * (max1 - 1);
        }

        public static int WidthOfBinaryTree(TreeNode root) {
            Dictionary<TreeNode, long> map = new();
            map[root] = 1;
            List<long> min = new();
            List<long> max = new();

            f(root, 0);
            long res = 0;
            for (int i = 0; i < min.Count; i++) {
                res = Math.Max(res, max[i] - min[i] + 1);
            }
            return (int)res;


            void f(TreeNode? node, int level) {
                if (node is null) {
                    return;
                }
                if (node.left is not null) {
                    map[node.left] = map[node] << 1;
                }
                if (node.right is not null) {
                    map[node.right] = map[node] << 1 | 1;
                }
                if (min.Count == level) {
                    min.Add(long.MaxValue);
                    max.Add(long.MinValue);
                }
                min[level] = Math.Min(min[level], map[node]);
                max[level] = Math.Max(max[level], map[node]);
                f(node.left, level + 1);
                f(node.right, level + 1);

            }
        }

        public static int MinSubArrayLen(int target, int[] nums) {
            int cur1 = 0, cur2 = 0, sum = nums[0], minLength = nums.Length;
            bool exist = false;
            while (true) {
                if (sum < target) {
                    cur2++;
                    if (cur2 >= nums.Length) {
                        return !exist ? 0 : minLength;
                    }

                    sum += nums[cur2];
                } else//满足
                  {
                    exist = true;
                    minLength = Math.Min(minLength, cur2 - cur1 + 1);
                    sum -= nums[cur1];
                    cur1++;
                }
            }
        }

        public static string MinWindow(string s, string t) {
            Dictionary<char, int> dic = new();
            foreach (var item in t) {
                if (!dic.ContainsKey(item)) {
                    dic[item] = 0;
                }
                dic[item]++;
            }
            Dictionary<char, int> set = new();
            int length = s.Length + 1;

            int minI = -1, minJ = -1;
            for (int i = 0; i < s.Length; i++) {//i is startIndex
                if (dic.ContainsKey(s[i])) {
                    int j = i - 1;//j是右端索引，先加再判断是否包含此字符
                    while (true) {
                        if (IsContainsString(set, dic)) {
                            if (j - i + 1 < length) {
                                length = j - i + 1;
                                minI = i;
                                minJ = j;
                            }

                            if (i == s.Length) {
                                break;
                            }
                            if (set.ContainsKey(s[i])) {
                                if (set[s[i]] > 1) {
                                    set[s[i]]--;
                                } else {
                                    set.Remove(s[i]);
                                }
                            }

                            i++;//删

                        } else {
                            j++;
                            if (j == s.Length) {//end
                                break;
                            }

                            if (dic.ContainsKey(s[j])) {//判断是否是t的字符
                                if (!set.ContainsKey(s[j])) {
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
        private static bool IsContainsString(Dictionary<char, int> s, Dictionary<char, int> t) {

            foreach (var item in t.Keys) {
                if (s.ContainsKey(item)) {
                    if (t[item] > s[item]) {
                        return false;
                    }
                } else {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckInclusion(string s1, string s2) {
            Dictionary<char, int> dicS1 = new(26);//记录s1
            for (int i = 0; i < s1.Length; i++) {
                if (!dicS1.ContainsKey(s1[i])) {
                    dicS1.Add(s1[i], 0);
                }
                dicS1[s1[i]]++;
            }

            Dictionary<char, int> dicWin = new(26);//窗口中包含的字符及其数量
            int cur1 = 0, cur2 = 0;
            dicWin.Add(s2[cur1], 1);//在s2中滑动窗口
            while (true) {
                if (IsContainsString(dicWin, dicS1))//包含
                {
                    if (cur2 - cur1 + 1 == s1.Length) {
                        return true;
                    }
                    if (dicWin.ContainsKey(s2[cur1])) {
                        dicWin[s2[cur1]]--;
                    }

                    cur1++;
                } else//not enough
                  {
                    cur2++;
                    if (cur2 == s2.Length) {
                        break;
                    }
                    if (dicS1.ContainsKey(s2[cur2])) {
                        if (!dicWin.ContainsKey(s2[cur2])) {
                            dicWin[s2[cur2]] = 0;
                        }
                        dicWin[s2[cur2]]++;
                    }
                }
            }

            return false;
        }

        public static int[] Shuffle(int[] nums, int n) {
            int[] res = new int[2 * n];
            int index = 0;
            for (int i = 0; i < n; i++) {
                res[index++] = nums[i];
                res[index++] = nums[i + n];
            }

            return res;
        }

        public static void Merge(int[] nums1, int m, int[] nums2, int n) {
            int cur1 = m - 1;
            int cur2 = n - 1;
            int targetIndex = nums1.Length - 1;//从倒数第一个开始
            while (cur2 >= 0)//双指针倒叙遍历第二个数组
            {
                if (cur1 < 0) {
                    nums1[0] = int.MinValue;//出现了第一个数组中第一个数都比第二个数组的前n（n>=2）数还大的情况，需要第一个数组的指针不动，第二个数组的所有元素依次全部加入到头部
                    cur1 = 0;
                }
                if (nums1[cur1] > nums2[cur2]) {
                    nums1[targetIndex--] = nums1[cur1--];
                } else if (nums1[cur1] < nums2[cur2]) {
                    nums1[targetIndex--] = nums2[cur2--];
                } else {
                    nums1[targetIndex--] = nums2[cur2--];
                    nums1[targetIndex--] = nums1[cur1--];
                }
            }
        }

        public static TreeNode? InsertIntoMaxTree(TreeNode root, int val) {
            //遍历
            TreeNode dummyNode = new(-1, null, root);
            TreeNode cur1 = dummyNode;//last
            TreeNode? cur2 = root;//this
            while (true) {
                int val2;
                val2 = cur2 is null ? int.MinValue : cur2.val;

                if (val > val2) {
                    cur1.right = new(val, cur2);
                    return dummyNode.right;
                } else {
                    cur1 = cur2;
                    cur2 = cur2.right;
                }
            }
        }

        public static IList<string> FindRepeatedDnaSequences(string s) {
            //长度为10
            int length = 10;
            HashSet<string> set = new();
            HashSet<string> res = new();
            for (int i = 0; i < s.Length - length + 1; i++) {
                string tmp = s[i..(i + length)];
                if (!set.Add(tmp)) {
                    res.Add(tmp);
                }

            }

            return res.ToList();
        }

        public static int[] SortedSquares(int[] nums) {

            int cur1 = 0, cur2 = 0;
            List<int> res = new(nums.Length);
            if (nums[^1] < 0)//先找位置
            {
                cur1 = cur2 = nums.Length - 1;
            }
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] == 0) {
                    cur1 = cur2 = i;
                }
                if (i < nums.Length - 1 && nums[i] < 0 && nums[i + 1] > 0) {
                    cur1 = i;
                    cur2 = i + 1;
                }
            }
            if (cur1 == cur2) {
                cur2++;
            }

            while (true) {
                int left = int.MaxValue, right = int.MaxValue;
                if (cur1 >= 0) {
                    left = nums[cur1] * nums[cur1];
                }
                if (cur2 < nums.Length) {
                    right = nums[cur2] * nums[cur2];
                }
                if (left == right && right == int.MaxValue) {
                    return res.ToArray();
                }

                if (left < right) {
                    res.Add(left);
                    cur1--;
                } else {
                    res.Add(right);
                    cur2++;
                }
            }
        }

        public static bool ValidateStackSequences(int[] pushed, int[] popped) {
            Stack<int> stck = new();
            int j = 0;
            for (int i = 0; i < pushed.Length; i++) {
                stck.Push(pushed[i]);
                while (stck.Peek() == popped[j]) {
                    stck.Pop();
                    j++;

                    if (stck.Count == 0) {
                        break;
                    }
                }
            }

            return stck.Count == 0;
        }

        public static ListNode? ReverseList(ListNode? head) {
            if (head is null || head.next is null) {
                return head;
            }

            ListNode cur1 = head;
            ListNode? cur2 = cur1.next;
            cur1.next = null;

            while (cur2 is not null) {
                ListNode? next = cur2.next;
                cur2.next = cur1;
                cur1 = cur2;
                cur2 = next;
            }

            return cur1;
        }

        public static ListNode? ReverseBetween(ListNode head, int left, int right) {
            int count = 1;//一种不使用dummy的方法
            ListNode? cur = head;
            while (count < left - 1) {
                cur = cur!.next;
                count++;
            }
            //此时的cur还是反转起始端的上一个节点
            ListNode cur2;//反转部分的起始端
            if (left > 1) {
                cur2 = cur!.next!;
            } else {
                cur2 = head;//cur是无用的，因为头节点没有父节点
            }
            ListNode start;//开始反转的地方
            if (left != 1) {
                start = cur!.next!;
                cur.next = null;//断开反转的部分与前面的连接
            } else {
                start = cur2!;//从开头反转，无需断开
            }//cur指向start的上一个节点
            ListNode? pre = null;
            count = 0;
            while (count < right - left + 1) {
                ListNode next = start!.next!;
                start.next = pre;//start指针移动并开始反转链表
                pre = start;
                start = next!;
                count++;
            }
            //此时，pre是反转部分的末节点
            if (left != 1) {
                cur!.next = pre;
            }
            cur2.next = start;
            if (left > 1) {
                return head;
            } else {
                return pre;//如果反转部分起始端在第一位，那么原先是末节点的pre在反转后才是真正的“head”头部分
            }
        }

        public static bool IsHappy(int n) {
            HashSet<int> path = new();
            while (true) {
                int[] tmp = GetDigits(n);
                n = 0;
                for (int i = 0; i < tmp.Length; i++) {
                    n += tmp[i] * tmp[i];
                }

                if (!path.Add(n)) {
                    return false;
                }

                if (n == 1) {
                    return true;
                }
            }
        }

        private static int[] GetDigits(int n) {
            List<int> digits = new(10);
            while (n != 0) {
                digits.Add(n % 10);
                n /= 10;
            }

            return digits.ToArray();
        }

        public static int FourSumCount(int[] nums1, int[] nums2, int[] nums3, int[] nums4) {
            //nums1 nums2
            int n = nums1.Length;
            Dictionary<int, int> dic12 = new();//sum-->>ways
            Dictionary<int, int> dic34 = new();//sum-->>ways
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    int sum = nums1[i] + nums2[j];
                    if (!dic12.ContainsKey(sum)) {
                        dic12[sum] = 0;
                    }
                    dic12[sum]++;

                    int sum2 = nums3[i] + nums4[j];
                    if (!dic34.ContainsKey(sum2)) {
                        dic34[sum2] = 0;
                    }
                    dic34[sum2]++;
                }
            }

            int resultCount = 0;
            foreach (var pairs in dic12) {
                if (dic34.ContainsKey(-pairs.Key)) {
                    resultCount += dic34[-pairs.Key] * dic12[pairs.Key];
                }
            }

            return resultCount;
        }

        public static bool CanConstruct(string ransomNote, string magazine) {
            Dictionary<char, int> dic = new(26);
            foreach (var item in magazine) {
                if (!dic.ContainsKey(item)) {
                    dic[item] = 0;
                }
                dic[item]++;
            }

            foreach (var item in ransomNote) {
                if (!dic.ContainsKey(item) || (dic.ContainsKey(item) && dic[item] == 0)) {
                    return false;
                } else {
                    dic[item]--;
                }
            }

            return true;
        }

        public static IList<IList<int>> ThreeSum(int[] nums) {
            Array.Sort(nums);
            if (nums[0] * nums[^1] > 0) {
                return Array.Empty<IList<int>>();
            }

            int n = nums.Length;
            Dictionary<int, int> dic1 = new();//cur0 value -->> last  index
            for (int i = 0; i < n; i++) {
                dic1[nums[i]] = i;
            }

            List<IList<int>> res = new();
            for (int i = 0; i < n; i++) {
                if (i > 0 && nums[i] == nums[i - 1]) {
                    continue;
                }
                for (int j = i + 1; j < n; j++) {
                    if ((j > i + 1 && nums[j] == nums[j - 1])
                        || !dic1.ContainsKey(-(nums[i] + nums[j]))) {
                        continue;
                    } else {
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

        public static IList<IList<int>> ThreeSum2(int[] nums) {
            Array.Sort(nums);
            List<IList<int>> res = new();
            if (nums[0] * nums[^1] > 0) {
                return Array.Empty<IList<int>>();
            }

            for (int cur0 = 0; cur0 < nums.Length - 2; cur0++) {
                //cur0去重
                if (cur0 >= 1 && nums[cur0 - 1] == nums[cur0])// cur0 cur0 cur1 cur1 cur1 cur2 cur2 cur2
                {
                    continue;
                }

                int cur1 = cur0 + 1, cur2 = nums.Length - 1;
                while (cur1 < cur2) {//双指针处理2-sum
                    int twoSum = nums[cur1] + nums[cur2];
                    if (twoSum > -nums[cur0]) {
                        cur2--;
                    } else if (twoSum < -nums[cur0]) {
                        cur1++;
                    } else {//equal
                        res.Add(new int[] { nums[cur0], nums[cur1], nums[cur2] });
                        //选择了某一组数 在选择这一组数后更改指针位置时要跨过重复元素
                        while (cur1 + 1 < nums.Length && nums[cur1 + 1] == nums[cur1]) {
                            cur1++;
                        }
                        while (cur2 - 1 >= 0 && nums[cur2 - 1] == nums[cur2]) {
                            cur2--;
                        }
                        cur1++;
                        cur2--;
                    }
                }
            }

            return res;
        }

        public static IList<IList<int>> FourSum(int[] nums, int target) {
            Array.Sort(nums);
            int n = nums.Length;

            if (nums[0] > 0 && nums[0] > target) {
                return Array.Empty<IList<int>>();
            }
            List<IList<int>> res = new();//-2 -1 0 0 1 2
            for (int i = 0; i < n; i++) {
                if (i > 0 && nums[i] == nums[i - 1]) {
                    continue;
                }
                for (int j = i + 1; j < n; j++) {
                    if (j > i + 1 && nums[j] == nums[j - 1]) {
                        continue;
                    }

                    int cur1 = j + 1, cur2 = n - 1;
                    while (cur1 < cur2) {
                        int sum = nums[i] + nums[j] + nums[cur1] + nums[cur2];
                        if (sum > target) {
                            cur2--;
                        } else if (sum < target) {
                            cur1++;
                        } else//equal     -1 -1 0 1 2 2
                          {
                            res.Add(new int[] { nums[i], nums[j], nums[cur1], nums[cur2] });
                            cur1++;
                        }

                        while (cur1 < cur2 && cur1 - 1 > j && nums[cur1] == nums[cur1 - 1]) {
                            cur1++;
                        }
                        while (cur1 < cur2 && cur2 + 1 < nums.Length && nums[cur2] == nums[cur2 + 1]) {
                            cur2--;
                        }
                    }
                }
            }

            return res;
        }

        public static int NumSpecial(int[][] mat) {
            int count = 0;
            for (int i = 0; i < mat.Length; i++) {
                int index = -1;
                bool satisfy = false;
                for (int j = 0; j < mat[0].Length; j++) {
                    if (mat[i][j] == 1) {
                        if (index == -1 && !satisfy) {
                            satisfy = true;
                            index = j;
                        } else if (satisfy) {
                            satisfy = false;
                            break;
                        }
                    }
                }
                if (satisfy) {
                    //检查index列
                    for (int u = 0; u < mat.Length; u++) {
                        if (mat[u][index] == 1 && u != i) {
                            satisfy = false;
                            break;
                        }
                    }
                }

                if (satisfy) {
                    count++;
                }
            }

            return count;
        }

        public static string ReverseStr(string s, int k) {
            bool needReverse = true;
            char[] chars = s.ToCharArray();
            int startIndex = 0;
            while (true) {
                if (needReverse) {
                    if (s.Length - startIndex < k) {
                        k = s.Length - startIndex;
                    }

                    for (int i = 0; i < k / 2; i++) { //i is count
                        (chars[i + startIndex], chars[startIndex + k - i - 1]) = (chars[startIndex + k - i - 1], chars[i + startIndex]);
                    }
                }

                startIndex += k;
                needReverse = !needReverse;

                if (startIndex >= s.Length - 1) {
                    return new(chars);
                }
            }
        }

        public static string ReverseLeftWords(string s, int n) {
            char[] source = s.Reverse().ToArray();
            Array.Reverse(source, 0, s.Length - n);
            Array.Reverse(source, s.Length - n, n);
            return new(source);
        }

        public static int StrStr(string haystack, string needle) {
            int cur1 = 0, cur2 = 0;
            int index = 0;
            while (cur2 < needle.Length)//&& cur1 < haystack.Length)
            {
                if (cur1 >= haystack.Length) {
                    return -1;
                }
                if (haystack[cur1] != needle[cur2]) {
                    cur1 = index + 1;
                    cur2 = 0;
                    index = cur1;
                } else {
                    cur1++;
                    cur2++;
                }
            }

            return index;
        }

        public static int StrStr2(string haystack, string needle) {
            int[] next = GetPrefixArr(needle);//mississippi
            int j = 0;
            for (int i = 0; ; i++) {
                while (j >= 1 && haystack[i] != needle[j]) {//直接跳转到符合条件的位置，防止if的i自加副作用
                    j = next[j - 1];
                }
                if (haystack[i] == needle[j]) {
                    j++;
                }

                if (j == needle.Length) {//变量j为先加再检查，所以要检查位置要向后偏移一位
                    return i - needle.Length + 1;
                }
                if (i == haystack.Length - 1) {//j != needle.Length
                    return -1;
                }
            }
        }

        public static int[] GetPrefixArr(string s) {
            int[] dp = new int[s.Length];
            dp[0] = 0;
            for (int i = 1; i < s.Length; i++) {
                int check = dp[i - 1];
                if (s[i] == s[check]) {
                    dp[i] = dp[i - 1] + 1;
                } else {
                    while (check > 0 && s[i] != s[check]) {
                        check = dp[check - 1];
                    }
                    if (check == 0) {
                        dp[i] = s[check] == s[i] ? 1 : 0;
                    } else {
                        dp[i] = dp[check] + 1;
                    }
                }
            }
            return dp;
        }

        public static int MaxProductDP(int[] nums) {
            int[] dp1 = new int[nums.Length]; //以i结尾的乘积最大子数组
            int[] dp2 = new int[nums.Length]; //以i结尾的乘积最小子数组
            dp1[0] = dp2[0] = nums[0];
            int res = nums[0];
            for (int i = 1; i < nums.Length; i++) {
                dp1[i] = Math.Max(nums[i], nums[i] * dp1[i - 1]);
                dp1[i] = Math.Max(dp1[i], nums[i] * dp2[i - 1]);
                res = Math.Max(res, dp1[i]);
                dp2[i] = Math.Min(nums[i], nums[i] * dp1[i - 1]);
                dp2[i] = Math.Min(dp2[i], nums[i] * dp2[i - 1]);
            }
            return res;
        }

        public static bool RepeatedSubstringPattern(string s) {
            int[] next = GetPrefixArr(s);
            int num = s.Length - next[^1];
            return next[^1] != 0 && s.Length % num == 0;
        }

        public static int[] ConstructArray(int n, int k) {
            //k只能≤n-1
            List<int> res = new(n);
            int[] swing = GenerateSwingArr(k + 1);
            res.AddRange(swing);
            for (int i = swing[1] + 1; i <= n; i++) {
                res.Add(i);
            }

            return res.ToArray();
        }

        /// <summary>
        /// 生成1 ~ k 的摆动数列
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private static int[] GenerateSwingArr(int k) {
            if (k == 1) {
                return new int[] { 1 };
            }
            int[] res = new int[k];

            int i = 0;//index
            int j = 1;//1 ~ k
            bool isForward = true;
            while (true) {
                if (i == k + 1)//正好到头并超出
                {
                    isForward = false;
                    i = k - 2;
                } else if (i == k)//过了终点一位
                  {
                    i = k - 1;
                    isForward = false;
                }

                res[i] = j++;
                i = isForward ? i + 2 : i - 2;

                if (i == -1) {
                    return res;
                }
            }
        }

        public static string SmallestNumber(string pattern) {
            string[] arr = new string[pattern.Length + 1];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = (i + 1).ToString();
            }

            int cur1 = -2, cur2 = -2;
            for (int i = 0; i < pattern.Length; i++) {
                if (pattern[i] == 'D' && cur1 == -2) {
                    cur1 = i;
                    cur2 = i;
                } else if (pattern[i] == 'D')//Last
                  {
                    cur2++;
                }

                if ((pattern[i] == 'I' && cur2 + 1 == i) || (i == pattern.Length - 1 && pattern[i] == 'D'))//刚结束D序列的两种情况
                {
                    Array.Reverse(arr, cur1, cur2 - cur1 + 2);
                    cur1 = -2;
                }
            }

            return string.Concat(arr);
        }

        public static int MinOperations(string[] logs) {
            int n = 0;
            for (int i = 0; i < logs.Length; i++) {
                switch (logs[i]) {
                    case "./":
                        break;
                    case "../":
                        n = n == 0 ? 0 : n - 1;
                        break;
                    default:
                        n++;
                        break;
                }
            }

            return n;
        }

        public static string[] SplitString(string s, params char[] operators) {
            s += operators[0];//统一字符串形式
            HashSet<char> set = new(operators);
            int cur1 = 0, cur2 = 0;
            bool isFirst = true;
            List<string> res = new(s.Length / 2);

            for (int i = 0; i < s.Length; i++) {
                //  102 5602    148 45   2 
                if (set.Contains(s[i])) {
                    if (!isFirst) {
                        res.Add(s[cur1..(cur2 + 1)]);
                        cur1 = cur2 = i;
                        isFirst = true;
                    } else {
                        cur1++;
                        cur2++;
                    }
                } else {
                    if (isFirst) {
                        cur1 = cur2 = i;
                        isFirst = false;
                    } else {
                        cur2++;
                    }
                }
            }

            return res.ToArray();
        }

        public static TreeNode TrimBST(TreeNode root, int low, int high) {
            Queue<TreeNode> que = new();
            root = new(int.MinValue, null, root);
            que.Enqueue(root);
            while (true) {
                if (que.Count == 0) {
                    return root.right;
                }

                int count = que.Count;
                bool needAgain = false;
                for (int i = 0; i < count; i++) {
                    TreeNode node = que.Peek();
                    if (node.left != null) {
                        if (node.left.val < low) {
                            node.left = node.left.right;
                            needAgain = true;
                        } else if (node.left.val > high) {
                            node.left = node.left.left;
                            needAgain = true;
                        }
                    }
                    if (node.right != null) {
                        if (node.right.val < low) {
                            node.right = node.right.right;
                            needAgain = true;
                        } else if (node.right.val > high) {
                            node.right = node.right.left;
                            needAgain = true;
                        }
                    }

                    if (!needAgain) {
                        if (node.left is not null) {
                            que.Enqueue(node.left);
                        }
                        if (node.right is not null) {
                            que.Enqueue(node.right);
                        }

                        que.Dequeue();
                    }
                }
            }
        }

        public static int[][] SpiralMatrixIII(int rows, int cols, int rStart, int cStart) {
            int n = 0;
            int[][] res = new int[rows * cols][];

            //itself
            res[n++] = new int[] { rStart, cStart };

            int x = 0, y = 1;
            int dire = 1;
            int size = 3;
            while (n < rows * cols) {
                if (0 <= x + rStart && x + rStart < rows && 0 <= y + cStart && y + cStart < cols) {
                    res[n++] = new int[] { x + rStart, y + cStart };
                }
                //开始螺旋
                switch (dire % 4) {
                    case 1:
                        x++;
                        if (x > size / 2) {
                            x--;
                            dire++;
                            goto case 2;
                        }
                        break;
                    case 2:
                        y--;
                        if (y < -size / 2) {
                            y++;
                            dire++;
                            goto case 3;
                        }
                        break;
                    case 3:
                        x--;
                        if (x < -size / 2) {
                            x++;
                            dire++;
                            goto case 0;
                        }
                        break;
                    case 0://4
                        y++;
                        if (y > size / 2) {
                            dire++;
                            size += 2;//本周期已尽
                            //x不变
                        }
                        break;
                }

            }

            return res;
        }

        public static int SpecialArray(int[] nums) {
            //如果 x 小于数组的最小值
            if (nums.Length < nums.Min()) {
                return nums.Length;
            }

            Dictionary<int, int> dic = new();
            for (int i = 0; i < nums.Length; i++) {
                if (!dic.ContainsKey(nums[i])) {
                    dic[nums[i]] = 0;
                }
                dic[nums[i]]++;
            }

            int count = nums.Length;
            for (int i = 0; i <= 1000; i++) {
                //先判断数组最大值与最小值间的连续整数中是否存在
                if (dic.ContainsKey(i - 1)) {
                    count -= dic[i - 1];
                }
                if (count == i) {
                    return count;
                }
            }

            return -1;
        }

        /// <summary>
        /// 给定一个非负整数，你至多可以交换一次数字中的任意两位。返回你能得到的最大值。
        /// </summary>
        /// <param name="num">整数</param>
        /// <returns>得到的最大值</returns>
        public static int MaximumSwap(int num) {
            List<int> digits = new(8);
            while (num > 0) {
                digits.Add(num % 10);
                num /= 10;
            }
            int[] maxValue = new int[digits.Count];
            maxValue[0] = digits[0];
            int[] maxIndex = new int[digits.Count];
            for (int i = 1; i < maxValue.Length; i++) {
                if (digits[i] > maxValue[i - 1]) {
                    maxIndex[i] = i;
                    maxValue[i] = digits[i];
                } else {
                    maxIndex[i] = maxIndex[i - 1];
                    maxValue[i] = maxValue[i - 1];
                }
            }
            for (int i = digits.Count - 1; i >= 0; i--) {
                if (digits[i] != maxValue[i]) {
                    (digits[i], digits[maxIndex[i]]) = (digits[maxIndex[i]], digits[i]);
                    break;//only one chance
                }
            }
            int res = 0;
            for (int i = 0; i < digits.Count; i++) {
                res += powsTen[i] * digits[i];
            }
            return res;
        }

        public static IList<IList<int>> FindSubsequences(int[] nums) {
            _resultFindSubsequences.Clear();
            BacktrackingFindSubsequences(nums, int.MinValue, 0, new());
            return _resultFindSubsequences;
        }

        static IList<IList<int>> _resultFindSubsequences = new List<IList<int>>();
        static IList<int> _pathFindSubsequences = new List<int>();

        //static HashSet<string> noDistinct = new();

        private static void BacktrackingFindSubsequences(int[] nums, int lastNum, int startIndex, HashSet<int> set) {
            if (_pathFindSubsequences.Count >= 2) {
                _resultFindSubsequences.Add(_pathFindSubsequences.ToList());
            }
            for (int i = startIndex; i < nums.Length; i++) {
                if (nums[i] >= lastNum) {
                    if (set.Add(nums[i])) {
                        _pathFindSubsequences.Add(nums[i]);
                        BacktrackingFindSubsequences(nums, nums[i], i + 1, new());
                        _pathFindSubsequences.RemoveAt(_pathFindSubsequences.Count - 1);
                    }
                }
            }
        }

        public static double TrimMean(int[] arr) {
            Array.Sort(arr);
            return arr[(arr.Length / 20)..^(arr.Length / 20)].Average();
        }

        public static bool IsPalindrome(ListNode head) {
            if (head.next is null) {
                return true;
            }
            int n = 1;
            ListNode? cur1 = head;
            ListNode? cur2;
            while (cur1.next != null) {
                n++;
                cur1 = cur1.next;
            }//计数

            cur2 = head.next;
            cur1 = head;
            for (int i = 0; i < n / 2 + n % 2 - 1; i++) {
                cur1 = cur1!.next;
                cur2 = cur2!.next;
            }//找到中点处

            cur1!.next = ReverseList(cur2);//反转后半
            cur2 = cur1.next;
            cur1 = head;
            for (int i = 0; i < n / 2; i++) {
                if (cur1!.val == cur2!.val) {
                    cur1 = cur1.next;
                    cur2 = cur2.next;
                } else {
                    return false;
                }
            }

            return true;
        }

        public static int FlipLights(int n, int presses) => presses switch {
            0 => 1,
            1 => n >= 4 ? 4 : n + 1,
            >= 2 => n switch {
                1 => 2,
                2 => 4,
                _ => presses == 2 ? 7 : 8,
            },
        };

        public static int[] FrequencySort(int[] nums) {
            //Array.Sort(nums);
            int lastNum = nums[0];
            Dictionary<int, int> dic = new();
            for (int i = 0; i < nums.Length; i++) {
                if (!dic.ContainsKey(nums[i])) {
                    dic[nums[i]] = 0;
                }
                dic[nums[i]]++;
            }
            List<int> res = new(nums.Length);
            int[] tmpVal = dic.Keys.ToArray();
            int[] tmpTimes = dic.Values.ToArray();
            Array.Sort(nums, (a, b) => {
                if (dic[a] != dic[b]) {
                    return dic[a] - dic[b];
                } else {
                    return b - a;
                }

            });

            return nums.ToArray();
        }

        public static int MaxLengthBetweenEqualCharacters(string s) {
            Dictionary<char, int> dic = new();
            int max = -1;
            for (int i = 0; i < s.Length; i++) {
                if (dic.ContainsKey(s[i])) {
                    max = Math.Max(i - dic[s[i]] - 1, max);
                } else {
                    dic[s[i]] = i;
                }
            }

            return max;
        }

        public static string ReorderSpaces(string text) {
            int space = 0;
            foreach (var item in text) {
                if (item == ' ') {
                    space++;
                }
            }
            string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            StringBuilder res = new();
            if (words.Length == 1) {
                res.Append(words[0]);
                res.Append(new String(' ', text.Length - words[0].Length));
            } else {
                for (int i = 0; i < words.Length - 1; i++) {
                    string item = words[i];
                    res.Append(item);
                    res.Append(new string(' ', space / (words.Length - 1)));
                }
                res.Append(words[^1]);
                res.Append(new string(' ', space - space / (words.Length - 1) * (words.Length - 1)));
            }
            return res.ToString();
        }

        public static IList<IList<int>> Permute(int[] nums) {
            _resultPermute.Clear();
            bool[] used = new bool[nums.Length];
            BacktrackingPermute(nums, used);
            return _resultPermute;
        }

        static List<IList<int>> _resultPermute = new();
        static List<int> _pathPermute = new();

        private static void BacktrackingPermute(int[] nums, bool[] path) {
            if (_pathPermute.Count == nums.Length) {
                _resultPermute.Add(_pathPermute.ToList());
                return;
            }
            for (int i = 0; i < nums.Length; i++) {
                if (path[i]) {
                    continue;
                }
                _pathPermute.Add(nums[i]);
                path[i] = true;
                BacktrackingPermute(nums, path);
                _pathPermute.RemoveAt(_pathPermute.Count - 1);
                path[i] = false;
            }
        }

        public static bool IsBracketValid(string s) {
            Dictionary<char, char> dic = new() {
                ['('] = ')',
                ['{'] = '}',
                ['['] = ']',
            };

            Stack<char> st = new(s.Length / 2);
            foreach (var item in s) {
                if (dic.ContainsKey(item)) {
                    st.Push(dic[item]);
                } else {
                    if (st.TryPop(out char res)) {
                        if (res != item) {
                            return false;
                        }
                    } else {
                        return false;
                    }
                }
            }
            return st.Count == 0;
        }

        public static string RemoveDuplicates(string s) {
            Stack<char> st = new();
            foreach (char item in s) {
                if (st.TryPeek(out char val)) {
                    if (item == val) {
                        st.Pop();
                        continue;
                    }
                }
                st.Push(item);
            }

            return new(st.Reverse().ToArray());
        }

        public static int EvalRPN(string[] tokens) {
            Stack<int> st = new();
            HashSet<string> op = new() { "+", "-", "*", "/" };
            foreach (var item in tokens) {
                int a = -1, b = -1;
                if (op.Contains(item)) {
                    a = st.Pop();
                    b = st.Pop();
                    st.Push(item switch {
                        "+" => a + b,
                        "-" => b - a,
                        "*" => a * b,
                        "/" => b / a,
                        _ => 0
                    });
                } else {
                    st.Push(int.Parse(item));
                }
            }

            return st.Pop();
        }

        public static int[] MaxSlidingWindow(int[] nums, int k) {
            int[] res = new int[nums.Length - k + 1];
            Deque<int> que = new();
            for (int i = 0; i < k; i++) {
                push(i);
            }
            for (int i = 0; i < res.Length; i++) { //第一次已初始化
                res[i] = nums[que.GetFront()];
                if (i + k < nums.Length) {
                    pop(i);
                    push(i + k);
                }
            }


            return res;

            void push(int idx) {
                var val = nums[idx];
                while (que.Count > 0 && nums[que.GetBack()] <= val) {
                    que.PopBack();
                }
                que.PushBack(idx);
            }

            void pop(int idx) {
                if (idx == que.GetFront()) {
                    que.PopFront();
                }
            }
        }

        public static bool CanFormArray(int[] arr, int[][] pieces) {
            Dictionary<int, int[]> dic = new();
            for (int i = 0; i < pieces.Length; i++) {
                dic[pieces[i][0]] = pieces[i];
            }
            for (int i = 0; i < arr.Length;) {
                if (!dic.ContainsKey(arr[i])) {
                    return false;
                }
                if (dic[arr[i]].Length == 1) {
                    i++;
                    continue;
                } else { //有多个值
                    int headVal = arr[i];
                    for (int j = 0; j < dic[headVal].Length && i < arr.Length; j++, i++) {
                        if (dic[headVal][j] != arr[i]) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public static bool CanMakeArithmeticProgression(int[] arr) {
            int max = int.MinValue, min = int.MaxValue;
            for (int i = 0; i < arr.Length; i++) {
                max = Math.Max(arr[i], max);
                min = Math.Min(arr[i], min);
            }
            double d = (max - min) / (arr.Length - 1.0);
            if (d == 0) {
                return true;
            }
            HashSet<int> set = new();
            for (int i = 0; i < arr.Length; i++) {
                if ((arr[i] - min) % d != 0) {
                    return false;
                } else {
                    if (!set.Add(arr[i])) {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool SearchMatrix(int[][] matrix, int target) {
            for (int i = 0; i < matrix.Length; i++) {
                if (matrix[i][^1] >= target) {
                    for (int j = 0; j < matrix[0].Length; j++) {
                        if (matrix[i][j] == target) {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static int RemoveDuplicates(ref int[] nums) {
            int cur1 = 0, cur2 = 0;
            int count = nums.Length;
            while (cur2 < nums.Length) {
                if (cur2 + 1 < nums.Length && nums[cur2] == nums[cur2 + 1]) {
                    count--;
                } else {
                    nums[cur1] = nums[cur2];
                    cur1++;
                }

                cur2++;
            }

            return count;
        }

        public static int[] Decrypt(int[] code, int k) {
            int[] res = new int[code.Length];
            for (int i = 0; i < code.Length; i++) {
                int val = 0;
                for (int j = 0; j != k; j += Math.Sign(k)) { //k > 0
                    val += code[(j + Math.Sign(k) + i + code.Length) % code.Length];
                    res[i] = val;
                }
            }

            return res;
        }

        public static int[] Decrypt2(int[] code, int k) {
            //k为负数取左边
            //k为正数取右边
            int[] res = new int[code.Length];
            if (k == 0) {
                return res;
            }
            int sign = Math.Sign(k);
            int cur1 = (sign + code.Length) % code.Length,
                cur2 = (k + code.Length) % code.Length;
            int sum = 0;
            for (int i = 0; i < Math.Abs(k); i++) {
                int index = (cur1 + i * sign + code.Length) % code.Length;
                sum += code[index];
            }
            res[0] = sum;
            for (int i = 1; i < res.Length; i++) {
                if (sign == 1) {
                    cur2 = (cur2 + 1) % res.Length;
                    sum += code[cur2];
                    sum -= code[cur1];
                    cur1 = (cur1 + 1) % res.Length;
                } else {
                    cur1 = (cur1 + 1) % res.Length;
                    sum += code[cur1];
                    sum -= code[cur2];
                    cur2 = (cur2 + 1) % res.Length;
                }
                res[i] = sum;
            }

            return res;
        }

        public static ListNode? DeleteDuplicates(ListNode head) {
            if (head is null || head.next is null) {
                return head;
            }
            ListNode dummy = new(int.MinValue) { next = head };
            ListNode cur1 = dummy;
            ListNode? cur2 = dummy.next;
            while (cur2 is not null) { //1 2 3 3 4 4 5
                bool flag = false;
                while (cur2.next is not null && cur2.val == cur2.next.val) {
                    cur2 = cur2.next;
                    flag = true;
                }
                if (flag) {
                    cur1.next = cur2.next;
                    cur2 = cur2.next;
                } else {
                    cur1 = cur1.next;
                    cur2 = cur1?.next;
                }
            }
            return dummy.next;
        }

        public static int RotatedDigits(int n) {
            int[] nums = new int[] {
                0,1,2,5,6,8,9
            };
            BacktrackingRotatedDigits(nums, 0, n, false);
            return countRotatedDigits.Count;
        }

        private static int _sumRotatedDigits = 0;
        private static HashSet<int> countRotatedDigits = new();

        private static void BacktrackingRotatedDigits(int[] arr, int level, int target, bool contains2569) {
            //level向上 每位都在(2, 5, 6, 9, 0, 1, 8)内，至少一位在(2, 5, 6, 9)内
            if (_sumRotatedDigits > target || level >= 6) {
                return;
            } else {
                if (contains2569) {
                    countRotatedDigits.Add(_sumRotatedDigits);
                }
            }
            for (int i = 0; i < arr.Length; i++) {
                bool tmp = contains2569 || (arr[i] == 2 || arr[i] == 5 || arr[i] == 6 || arr[i] == 9);
                _sumRotatedDigits += arr[i] * powsTen[level];
                BacktrackingRotatedDigits(arr, level + 1, target, tmp);
                _sumRotatedDigits -= arr[i] * powsTen[level];
            }
        }

        public static ListNode? Partition(ListNode head, int x) {
            if (head is null) {
                return null;
            }
            ListNode dummy = new(int.MinValue) {
                next = head
            };
            ListNode last = dummy;
            int count = 0;
            if (head.next is null) {
                last = head;
            } else {
                while (last.next is not null) {
                    if (last.next.val >= x) {
                        count++;
                    }
                    last = last.next;
                }
            }

            ListNode? cur1 = dummy, cur2 = cur1.next;
            while (cur2 is not null && count > 0) {
                bool flag = false;
                if (cur2.val >= x) {
                    if (cur2 != last) {
                        cur1.next = cur2.next;
                        cur2.next = null;
                        last.next = cur2;
                        last = last.next;
                        flag = true;
                    }
                    count--;
                }
                if (!flag) {
                    cur1 = cur1.next;
                }
                cur2 = cur1?.next;
            }

            return dummy.next;
        }

        public static ListNode MiddleNode(ListNode head) {
            ListNode? slow = head;
            ListNode? fast = head.next;
            while (fast is not null) {
                slow = slow?.next;
                fast = fast?.next?.next;
            }

            return slow;
        }

        public static ListNode? OddEvenList(ListNode head) {
            int index = 1;
            ListNode odd = new(int.MinValue);
            ListNode even = new(int.MinValue);
            ListNode backupEven = even;
            ListNode backupOdd = odd;
            ListNode? cur = head;
            while (cur is not null) {
                if ((index++ & 1) == 1) {
                    odd.next = cur;
                    odd = odd.next;
                } else {
                    even.next = cur;
                    even = even.next;
                }
                cur = cur.next;
            }
            even.next = null;
            odd.next = backupEven.next;
            return backupOdd.next;
        }

        public static ListNode? MergeTwoLists(ListNode list1, ListNode list2) {
            ListNode? cur1 = list1;
            ListNode? cur2 = list2;
            ListNode resCur = new(int.MinValue);
            ListNode backup = resCur;
            while (cur1 is not null || cur2 is not null) {
                if (cur1 is null && cur2 is not null) {
                    resCur.next = cur2;
                    cur2 = cur2.next;
                } else if (cur1 is not null && cur2 is null) {
                    resCur.next = cur1;
                    cur1 = cur1.next;
                } else {
                    if (cur1.val < cur2.val) {
                        resCur.next = cur1;
                    } else {
                        resCur.next = cur2;
                    }
                }

                resCur = resCur.next;
            }

            return backup.next;
        }

        public static TreeNode? InvertTree(TreeNode? root) {
            if (root is null) {
                return null;
            }
            RecursionInvertTree(root);
            return root;
        }

        private static void RecursionInvertTree(TreeNode root) {
            TreeNode? left = root?.left,
                right = root?.right;
            root.left = right;
            root.right = left;
            if (root.left is not null) {
                RecursionInvertTree(root.left);
            }
            if (root.right is not null) {
                RecursionInvertTree(root.right);
            }
        }

        public static bool CheckPermutation(string s1, string s2) {
            if (s1.Length != s2.Length) {
                return false;
            }
            Dictionary<char, int> dic = new(s1.Length);
            for (int i = 0; i < s1.Length; i++) {
                if (!dic.ContainsKey(s1[i])) {
                    dic[s1[i]] = 0;
                }
                dic[s1[i]]++;

                if (!dic.ContainsKey(s2[i])) {
                    dic[s2[i]] = 0;
                }
                dic[s2[i]]--;
            }

            foreach (var item in dic.Values) {
                if (item != 0) {
                    return false;
                }
            }

            return true;
        }

        public static bool IsSymmetric(TreeNode root) {
            List<TreeNode> thisLv = new() {
                root
            },
                nextLv = new();
            while (thisLv.Count != 0) {
                for (int i = 0; i < thisLv.Count / 2; i++) {
                    if (thisLv[i].val != thisLv[thisLv.Count - i - 1].val) {
                        return false;
                    }
                }
                for (int i = 0; i < thisLv.Count; i++) {
                    if (thisLv[i].val != int.MinValue) {
                        nextLv.Add(thisLv[i].left ?? new(int.MinValue));
                        nextLv.Add(thisLv[i].right ?? new(int.MinValue));
                    }
                }
                thisLv = nextLv.ToList();
                nextLv.Clear();
            }

            return true;
        }

        public static bool IsSymmetric2(TreeNode root) {
            return RecursionIsSymmetric(root.left, root.right);
        }

        private static bool RecursionIsSymmetric(TreeNode? left, TreeNode? right) {
            if (left is null && right is null) {
                return true;
            } else if (left is not null && right is null) {
                return false;
            } else if (left is null && right is not null) {
                return false;
            } else if (left.val != right.val) {
                return false;
            }

            bool outside = RecursionIsSymmetric(left.left, right.right);
            bool inside = RecursionIsSymmetric(left.right, right.left);

            return outside && inside;
        }

        public static int MaxDepth(TreeNode root) {
            if (root is null) {
                return 0;
            }
            return RecursionMaxDepth(root, 1);
        }

        private static int RecursionMaxDepth(TreeNode root, int depth) {
            if (root.left is null && root.right is null) {
                return depth;
            }
            if (root.left is not null && root.right is not null) {
                int left = RecursionMaxDepth(root.left, depth + 1);
                int right = RecursionMaxDepth(root.right, depth + 1);
                return Math.Max(left, right);
            }
            if (root.left is not null) {
                return RecursionMaxDepth(root.left, depth + 1);
            } else {
                return RecursionMaxDepth(root.right!, depth + 1);
            }
        }

        public static int CountNodes(TreeNode root) {
            int count = 0;
            RecursionCountNodes(root, ref count);
            return count;
        }

        private static void RecursionCountNodes(TreeNode? node, ref int count) {
            if (node is not null) {
                count++;
                RecursionCountNodes(node.left, ref count);
                RecursionCountNodes(node.right, ref count);
            }
        }

        public static int CountNodes2(TreeNode root) {
            if (root is null) {
                return 0;
            }
            TreeNode cur = root;
            int h1 = 1, h2 = 1;
            while (cur.left is not null) {
                cur = cur.left;
                h1++;
            }
            cur = root;
            while (cur.right is not null) {
                cur = cur.right;
                h2++;
            }
            if (h1 == h2) {
                return (int)Math.Pow(2, h1) - 1;
            } else {
                _countNodes = 0;
                BacktrackingCountNodes2(root, 1, h2);
                return (int)Math.Pow(2, h2) - 1 + _countNodes;
            }
        }

        static int _countNodes;

        private static void BacktrackingCountNodes2(TreeNode node, int depth, int maxDepth) {
            if (depth == maxDepth) {
                if (node.left is not null) {
                    _countNodes++;
                }
                if (node.right is not null) {
                    _countNodes++;
                }
                return;
            } else {
                for (int i = 0; i < 2; i++) {
                    switch (i) {
                        case 0:
                            BacktrackingCountNodes2(node.left, depth + 1, maxDepth);
                            break;
                        case 1:
                            BacktrackingCountNodes2(node.right, depth + 1, maxDepth);
                            break;
                    }
                }
            }
        }

        public static void Rotate(int[] nums, int k) {
            k %= nums.Length;
            for (int i = 0; i < nums.Length / 2; i++) {
                (nums[i], nums[^(1 + i)]) = (nums[^(1 + i)], nums[i]);
            }
            for (int i = 0; i < k / 2; i++) {
                (nums[i], nums[k - 1 - i]) = (nums[k - 1 - i], nums[i]);
            }
            for (int i = 0; i < (nums.Length - k) / 2; i++) {
                (nums[i + k], nums[^(1 + i)]) = (nums[^(1 + i)], nums[i + k]);
            }
        }

        public static string[] Permutation(string s) {
            var arr = s.ToCharArray();
            _resultPermutation.Clear();
            BacktrackingPermutation(arr, 0);
            return _resultPermutation.ToArray();
        }

        static List<string> _resultPermutation = new();

        private static void BacktrackingPermutation(char[] s, int startIdx) {
            if (startIdx == s.Length) {
                _resultPermutation.Add(new string(s));
            }
            HashSet<char> history = new();
            for (int i = startIdx; i < s.Length; i++) {
                if (history.Add(s[i])) {
                    (s[startIdx], s[i]) = (s[i], s[startIdx]);
                    BacktrackingPermutation(s, startIdx + 1);
                    (s[startIdx], s[i]) = (s[i], s[startIdx]);
                }
            }
        }

        public static void SetZeroes(int[][] matrix) {
            bool column = false, row = false;
            for (int i = 0; i < matrix.Length; i++) {
                if (matrix[i][0] == 0) {
                    column = true;
                    break;
                }
            }
            for (int i = 0; i < matrix[0].Length; i++) {
                if (matrix[0][i] == 0) {
                    row = true;
                    break;
                }
            }
            for (int i = 1; i < matrix.Length; i++) {
                for (int j = 1; j < matrix[0].Length; j++) {
                    if (matrix[i][j] == 0) {
                        matrix[i][0] = 0;
                        matrix[0][j] = 0;
                    }
                }
            }
            for (int i = 1; i < matrix.Length; i++) {
                for (int j = 1; j < matrix[0].Length; j++) {
                    if (matrix[0][j] == 0 || matrix[i][0] == 0) {
                        matrix[i][j] = 0;
                    }
                }
            }
            if (column) {
                for (int i = 0; i < matrix.Length; i++) {
                    matrix[i][0] = 0;
                }
            }
            if (row) {
                for (int i = 0; i < matrix[0].Length; i++) {
                    matrix[0][i] = 0;
                }
            }
        }

        public static ListNode? InsertionSortList(ListNode? head) {
            if (head is null || head.next is null) {
                return head;
            }
            ListNode sortedStart = head;
            ListNode sortedEnd = head;
            ListNode cur;
            while (sortedEnd.next != null) {
                cur = sortedEnd.next;
                if (cur.val < sortedStart.val) {
                    ListNode? tmp = cur.next;
                    cur.next = sortedStart;
                    sortedStart = cur;
                    sortedEnd.next = tmp;
                } else {
                    ListNode compare = sortedStart;
                    while (compare.next is not null && cur.val > compare.next.val) {
                        compare = compare.next;
                    }
                    if (compare.next == cur) {
                        sortedEnd = cur;
                    } else {
                        ListNode? tmp = compare.next;
                        ListNode? tmpCur = cur.next;
                        cur.next = null;
                        compare.next = cur;
                        cur.next = tmp;
                        sortedEnd.next = tmpCur;
                    }
                }
            }
            return sortedStart;
        }


        public static string ReformatNumber(string number) {
            number = number.Replace("-", "");
            number = number.Replace(" ", "");
            StringBuilder sb = new();
            int i = 0;
            int remain = number.Length % 3;
            if (remain == 1) {
                remain = 4;
            }
            for (; i < number.Length && number.Length - i > remain; i++) {
                sb.Append(number[i]);
                if ((i + 1) % 3 == 0 && i != 1 && i != number.Length - 1) {
                    sb.Append('-');
                }
            }
            switch (number.Length - i) {
                case 4:
                    //sb.Append('-');
                    sb.Append(number[^4..^2] + "-" + number[^2..]);
                    break;
                case 3 or 2:
                    //sb.Append('-');
                    sb.Append(number[^(number.Length - i)..]);
                    break;
                case 1:
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(number[^1]);
                    break;
                default:
                    break;
            }

            return sb.ToString();
        }

        public static int PartitionDisjoint(int[] nums) {
            int[] min = new int[nums.Length];
            int[] max = new int[nums.Length];
            max[0] = nums[0];
            min[^1] = nums[^1];
            for (int i = 1; i < nums.Length; i++) {
                max[i] = Math.Max(max[i - 1], nums[i]);
                min[^(i + 1)] = Math.Min(nums[^(i + 1)], min[^(i)]);
            }
            for (int i = 0; i < min.Length - 1; i++) {
                if (min[i + 1] - max[i] >= 0) {
                    return i + 1;
                }
            }

            return -1;
        }

        public static int PartitionDisjoint2(int[] nums) {
            int thsMax = nums[0];                   //left区间移动 thsMax存在的意义是
            int leftMax = int.MaxValue;             //设置窗口内最大值保证不漏下本应包含在窗口内的最大值
            int pos = -1;                           //thsMax是连续区间的最大值，而窗口也是连续的，只检测
            for (int i = 0; i < nums.Length; i++) { //当前遍历到的元素是否应成为最大值会漏掉应包含在窗口内
                thsMax = Math.Max(thsMax, nums[i]); //的最大值
                if (nums[i] < leftMax) {
                    pos = i;
                    leftMax = thsMax;


                }
            }
            return pos + 1;
        }

        public static bool CanTransform(string start, string end) {
            int n = start.Length;
            HashSet<int> history = new();
            Dictionary<char, int> dic = new();
            dic['R'] = 0;
            dic['L'] = 0;
            dic['X'] = 0;
            for (int i = 0; i < n; i++) {
                dic[start[i]]++;
                dic[end[i]]--;
            }
            foreach (var item in dic.Values) {
                if (item != 0) {
                    return false;
                }
            }
            for (int i = 0; i < n; i++) {                                    //RXXLRXRXL
                if (end[i] == 'R' || end[i] == 'L') {                        //XRLXXRRLX
                    bool flag = false;
                    int j = i;

                    while (true) {
                        if (end[i] == 'R') {
                            if (j == -1) break;
                        } else {
                            if (j == n) break;
                        }

                        if (start[j] == (end[i] == 'R' ? 'L' : 'R')) break;
                        if (start[j] == end[i]) {
                            flag = flag || history.Add(j);
                            if (flag) break;
                        }

                        j -= (System.Convert.ToInt32(end[i] == 'R') * 2 - 1);
                    }
                    if (!flag) {
                        return false;
                    }
                }

            }

            return true;
        }

        public static ListNode SwapNodes(ListNode head, int k) {
            ListNode cur1 = head;
            ListNode cur2 = cur1;
            for (int i = 0; i < k - 1; i++) {
                cur2 = cur2.next;
            }
            ListNode firstNode = cur2;
            while (cur2.next is not null) {
                cur1 = cur1.next;
                cur2 = cur2.next;
            }
            (cur1.val, firstNode.val) = (firstNode.val, cur1.val);

            return head;
        }

        public static bool CheckOnesSegment(string s) {
            int one = 0;
            int zero = 0;
            for (int i = 0; i < s.Length && s[i] == '1'; i++) {
                one++;
            }
            for (int i = s.Length - 1; i >= 0 && s[i] == '0'; i++) {
                zero++;
            }
            return one + zero == s.Length;
        }

        public static void Merge2(int[] array, int m, int n) {
            int i = 0;    //array[..i] must be sorted
            int j = m;    //array[j..] is sorted too.
            while (n != 0 && i != j) {
                int j2;
                if (array[j] < array[i]) {
                    j2 = j;
                    while (j < array.Length && array[j] < array[i]) {
                        j++;
                    }
                    SwapArray(array, i, m, j - 1);
                    i += j - j2;
                    m += j - j2;
                    n -= j - j2;
                }
                i++;
            }

            static void SwapArray(int[] array, int start, int mid, int end) {
                Array.Reverse(array, start, mid - start);
                Array.Reverse(array, mid, end - mid + 1);
                Array.Reverse(array, start, end - start + 1);
            }
        }

        public static int MinAddToMakeValid(string s) {
            string last = s;
            do {
                s = last;
                last = s.Replace("()", "");
            } while (last != s);
            return last.Length;
        }

        public static int MinAddToMakeValid2(string s) {
            //除非是明确说出来
            Stack<char> st = new();
            int invalidCount = 0;
            for (int i = 0; i < s.Length; i++) {
                if (s[i] == '(') {
                    st.Push(s[i]);
                } else { //)
                    if (!st.TryPop(out _)) {
                        invalidCount++;
                    }
                }
            }

            return invalidCount + st.Count;
        }

        public static int MinAddToMakeValid3(string s) {
            int leftBracket = 0;
            int rightBracket = 0;
            foreach (var item in s) {
                if (item == '(') {
                    leftBracket++;
                } else {
                    if (leftBracket == 0) {
                        rightBracket++;
                    } else {
                        leftBracket--;
                    }
                }
            }

            return leftBracket + rightBracket;
        }

        public static int MySqrt(int x) {
            int left = 0, right = 46340;
            while (left <= right) {
                int mid = (left + right) >> 1;
                if (mid * mid > x) {
                    right = mid - 1;
                } else if (mid * mid == x) {
                    return mid;
                } else {
                    //<
                    if ((mid + 1) * (mid + 1) > x || mid == right) {
                        return mid;
                    }
                    left = mid + 1;
                }
            }
            return -1;
        }

        public static ListNode ShuffleLinkedList(ListNode linkedlist) {
            ListNode cur1 = linkedlist;
            int n = 1;
            while (cur1.next is not null) {
                cur1 = cur1.next;
                n++;
            }
            cur1 = linkedlist;
            for (int i = 0; i < n / 2; i++) {
                cur1 = cur1.next;
            }
            RecursionShuffleLinkedList(linkedlist, n / 2, cur1, n - n / 2);
            return linkedlist;
        }

        public static void RecursionShuffleLinkedList(ListNode first, int length1, ListNode second, int length2) {
            if (length1 != 0 && length2 != 0) {
                ListNode cur1 = first;
                for (int i = 0; i < length1; i++) {
                    cur1 = cur1.next;
                }
                RecursionShuffleLinkedList(first, length1 / 2, cur1, length1 - length1 / 2);
                ListNode cur2 = first;
                for (int i = 0; i < length2; i++) {
                    cur2 = cur2.next;
                }
                RecursionShuffleLinkedList(second, length2 / 2, cur2, length2 - length2 / 2);
                Random ran = new();
                cur1 = first;
                cur2 = second;
                for (int i = 0; i < length1; i++) {
                    if (ran.Next(0, 2) == 0) {
                        (cur1.val, cur2.val) = (cur2.val, cur1.val);
                    }
                    cur1 = cur1.next;
                    cur2 = cur2.next;
                }
            }
        }

        public static IList<string> SubdomainVisits(string[] cpdomains) {
            string[] count = new string[cpdomains.Length];
            string[][] domains = new string[cpdomains.Length][];
            for (int i = 0; i < cpdomains.Length; i++) {
                var tmp = cpdomains[i].Split(' ');
                count[i] = tmp[0];
                domains[i] = tmp[1].Split('.');
            }
            Dictionary<string, int> dic = new();
            for (int i = 0; i < domains.Length; i++) {
                string tmp = domains[i][^1];
                if (!dic.ContainsKey(tmp)) {
                    dic.Add(tmp, 0);
                }
                for (int j = domains[i].Length - 2; j >= 0; j--) {
                    tmp = domains[i][j] + "." + tmp;
                    if (!dic.ContainsKey(tmp)) {
                        dic.Add(tmp, 0);
                    }
                }
            }

            for (int i = 0; i < domains.Length; i++) {
                string tmp = domains[i][^1];
                for (int j = 0; j < domains[i].Length; j++) {
                    dic[tmp] += int.Parse(count[i]);
                    if (j != domains[i].Length - 1) {
                        tmp = domains[i][^(j + 2)] + "." + tmp;
                    }
                }
            }
            IList<string> res = new List<string>();
            foreach (var item in dic) {
                res.Add($"{item.Value} {item.Key}");
            }
            return res;
        }

        public static ListNode? SortList(ListNode head) {
            if (head is null || head.next is null) {
                return head;
            }
            int length = 2;
            int n = 1;
            ListNode cur = head;
            ListNode dummy = new(int.MinValue);
            dummy.next = head;
            while (cur.next is not null) {
                cur = cur.next;
                n++;
            }
            while (length >> 1 <= n) {
                int length1, length2;
                int visited = 0;
                ListNode? beforeFirst = dummy;
                while (visited < n) {
                    if (length >> 1 > n - visited) {
                        length1 = (n - visited) >> 1;
                        length2 = n - visited - ((n - visited) >> 1);
                    } else {
                        length1 = length >> 1;
                        length2 = length - (length >> 1);
                    }

                    cur = beforeFirst;
                    for (int i = 0; i < length1; i++) {
                        cur = cur.next!;//get beforeSecond
                    }
                    MergeListNode(beforeFirst, cur, length2);
                    visited += length;
                    for (int i = 0; i < length && beforeFirst is not null; i++) {
                        beforeFirst = beforeFirst.next;//寻找下一组子数组的beforeFirst
                    }
                }
                length <<= 1;
            }
            return dummy.next;
        }

        private static void MergeListNode(ListNode beforeFirst, ListNode beforeSecond, int length2) {
            //implement this function first
            ListNode second = beforeSecond.next!;
            ListNode first = beforeFirst.next;
            while (second is not null && length2 != 0 && second != first) {
                if (first.val <= second.val && first.next.val <= second.val) {
                    beforeFirst = beforeFirst.next;//指针后移以找到可以插空的地方
                    first = first.next;
                } else if (first.val <= second.val) {
                    //可以插空
                    ListNode? next = second.next;
                    beforeSecond.next = beforeSecond.next.next;
                    second.next = null;
                    ListNode tmp = first.next;
                    first.next = second;
                    second.next = tmp;
                    beforeFirst = beforeFirst.next;
                    first = first.next;
                    second = next;
                    length2--;
                } else if (first.val > second.val) {
                    ListNode next = second.next;
                    beforeSecond.next = beforeSecond.next.next;
                    ListNode tmp = first;
                    second.next = null;
                    beforeFirst.next = second;
                    second.next = tmp;
                    first = second;//first移动到下一位
                    beforeFirst = beforeFirst.next;
                    second = next;
                    length2--;
                }
            }
        }

        public static ListNode ArrToList(int[] array) {
            ListNode res = new(array[0]);
            ListNode cur = res;
            for (int i = 1; i < array.Length; i++) {
                cur.next = new(array[i]);
                cur = cur.next;
            }
            return res;
        }

        public static int[] ThreeEqualParts(int[] arr) {
            int ones = arr.Sum();
            int[] fail = new int[] { -1, -1 };
            if (ones % 3 != 0) {
                return fail;
            }
            if (ones == 0) {
                return new int[] { 0, 2 };
            }
            int part = ones / 3;
            int i = arr.Length - 1; // 右侧部分刚好能扩展到的位置
            int sum = arr[i];// [i..]
            while (i >= 1 && sum < part) {
                i--;
                sum += arr[i];
            }
            int j = i - 1;
            sum = arr[j];
            while (j >= 1 && sum < part) {
                j--;
                sum += arr[j];
            }
            int k = j - 1;
            sum = arr[k];
            while (k >= 1 && sum < part) {
                k--;
                sum += arr[k];
            }
            int cur1 = k, cur2 = j, cur3 = i;
            for (; cur3 < arr.Length; cur1++, cur2++, cur3++) {
                if (cur1 >= j || cur2 >= i || arr[cur1] != arr[cur2] || arr[cur2] != arr[cur3]) {
                    return fail;
                }
            }
            return new int[] { cur1 - 1, cur2 };
        }

        public static int MaxAscendingSum(int[] nums) {
            int sum = nums[0];
            int max = nums[0];
            for (int i = 1; i < nums.Length; i++) {
                if (nums[i] > nums[i - 1]) {
                    sum += nums[i];
                    max = Math.Max(sum, max);
                } else {
                    sum = nums[i];
                }
            }
            return max;
        }

        public static int[] AdvantageCount(int[] nums1, int[] nums2) {
            Array.Sort(nums1);
            int[] ints = new int[nums1.Length];
            for (int i = 0; i < ints.Length; i++) {
                ints[i] = i + 1;
            }
            int cur1 = 0, cur2 = 0;
            List<int> res = new();
            bool[] visited = new bool[nums1.Length];
            Array.Sort(nums2, ints);
            while (cur1 < nums1.Length) {
                if (nums1[cur1] > nums2[cur2]) {
                    res.Add(nums1[cur1]);
                    visited[cur1] = true;
                    cur2++;
                }
                cur1++;
            }
            for (int i = visited.Length - 1; i >= 0; i--) {
                if (!visited[i]) {
                    res.Add(nums1[i]);
                }
            }
            int[] result = res.ToArray();
            Array.Sort(ints, result);
            return result;
        }

        public static int ScoreOfParentheses(string s) {
            int level = 0;
            int[] score = new int[(s.Length >> 1) + 1];
            for (int i = 0; i < s.Length; i++) {
                if (s[i] == '(') {
                    level++;
                } else {
                    //出层
                    if (score[level] == 0) {
                        score[level] = 1;//遇到一对括号
                    }
                    score[level - 1] += score[level] * 2;//返回上级
                    score[level] = 0;//防止本层分数累加
                    level--;
                }
            }

            return score[0] >> 1;
        }

        public static IList<int> NarcissisticNumber(int n) {
            _resultNarcissisticNumber.Clear();
            BacktrackingNarcissisticNumber(0, n, n);
            return _resultNarcissisticNumber;
        }

        private static IList<int> _pathNarcissisticNumber = new List<int>();
        private static int _sumNarcissisticNumber = 0;
        private static IList<int> _resultNarcissisticNumber = new List<int>();

        /// <summary>
        /// 回溯
        /// </summary>
        /// <param name="num">要枚举的数</param>
        /// <param name="level">已经递归完的层数</param>
        private static void BacktrackingNarcissisticNumber(int level, int maxLevel, int pow) {
            if (level == maxLevel) {
                int num = 0;
                for (int i = 0; i < _pathNarcissisticNumber.Count; i++) {
                    num += powsTen[i] * _pathNarcissisticNumber[i];
                }
                if (num == _sumNarcissisticNumber) {
                    _resultNarcissisticNumber.Add(num);
                }
                return;
            }
            for (int i = 0; i < 10; i++) {
                //0--9
                if (i == 0 && level == maxLevel - 1) {
                    continue;
                }
                _pathNarcissisticNumber.Add(i);
                _sumNarcissisticNumber += (int)Math.Pow(i, pow);
                BacktrackingNarcissisticNumber(level + 1, maxLevel, pow);
                _pathNarcissisticNumber.RemoveAt(_pathNarcissisticNumber.Count - 1);
                _sumNarcissisticNumber -= (int)Math.Pow(i, pow);
            }
        }

        public static bool AreAlmostEqual(string s1, string s2) {
            int index1 = -1;
            int index2 = -1;
            for (int i = 0; i < s1.Length; i++) {
                if (s1[i] != s2[i]) {
                    if (index1 == -1) {
                        index1 = i;
                    } else if (index2 == -1) {
                        index2 = i;
                    } else {
                        return false;
                    }
                }
            }
            if (index1 == index2 && index2 == -1) {
                return true;
            } else if (index2 == -1) {
                return false;
            }
            return s1[index1] == s2[index2] && s2[index1] == s1[index2];
        }

        public static int NumComponents(ListNode head, int[] nums) {
            HashSet<int> set = new(nums);
            ListNode? cur1 = head;
            int res = 0;
            bool hasComponentNode = false;
            while (cur1 is not null) {
                if (set.Contains(cur1.val)) {
                    if (!hasComponentNode) {
                        res++;
                        hasComponentNode = true;
                    }
                } else {
                    hasComponentNode = false;
                }
                cur1 = cur1.next;
            }

            return res;
        }

        public static int MaxChunksToSorted(int[] arr) {
            int res = 0;
            int max = int.MinValue;
            for (int i = 0; i < arr.Length; i++) {
                max = Math.Max(max, arr[i]);
                if (max == i) {
                    res++;
                }
            }

            return res;
        }

        public static int MaxProfit(int[] prices) {
            //递增子区间
            int cur1 = 0, cur2 = 0;
            int sum = 0;
            for (int i = 0; i < prices.Length; i++) {
                if (i == prices.Length - 1 && cur2 == i) {
                    sum += prices[cur2] - prices[cur1];
                } else if (i + 1 < prices.Length && prices[i + 1] < prices[i]) {
                    sum += prices[cur2] - prices[cur1];
                    cur2 = cur1 = i + 1;
                } else if (i + 1 < prices.Length && prices[i + 1] >= prices[i]) {
                    cur2 = i + 1;
                }
            }//1, 9, 6, 9, 1, 7, 1, 1, 5, 9, 9, 9

            return sum;
        }

        public static bool CanJump(int[] nums) {
            //2 0 1 1 4
            if (nums.Length == 1) {
                return true;
            }
            int maxCover = nums[0];//maxCover记录的不是长度是最大达到的索引

            for (int i = 1; i <= maxCover; i++) {
                int cover = nums[i];
                maxCover = Math.Max(i + cover, maxCover);
                if (maxCover >= nums.Length - 1) {
                    return true;
                }
            }

            return false;
        }

        public static int Jump(int[] nums) {
            if (nums.Length == 1) {
                return 0;
            }
            int count = 0;
            int maxRight = 0;
            int maxLeft = 0;
            for (int i = 0; i < nums.Length;) {
                if (nums[i] + i == nums.Length - 1) {
                    count++;
                    break;//优先考虑：能一次性跳完就直接跳过去并结束
                }
                for (int j = i; j <= nums[i] + i; j++) {     //当你在一个位置时，应优先考虑是否能够
                    if (j >= nums.Length) {                  //一次性直接跳完（count仅需加1）
                        return count + 1;                    //如果不能，再去考虑如何间接地跳才能使得
                    }                                        //跳的步骤最少，即count自增的量最少
                    if (maxRight <= nums[j] + j) {
                        maxRight = nums[j] + j;
                        maxLeft = j;
                    }
                }
                count++;//跳了一次，+1

                if (maxLeft >= nums.Length - 1) {
                    break;//跳出去了，该结束了
                }
                i = maxLeft;
            }
            return count;
        }

        public static int LargestSumAfterKNegations(int[] nums, int k) {
            Array.Sort(nums);
            int i;
            for (i = 0; i < nums.Length && k > 0; i++) {
                if (nums[i] < 0) {
                    nums[i] *= -1;
                    k--;
                } else {
                    break;
                }
            }
            if (k > 0) {
                if (i >= nums.Length) {
                    i = nums.Length - 1;
                } else if (i > 0 && nums[i] > nums[i - 1]) {
                    i--;//找最小的正数
                }
                if ((k & 1) == 1) {
                    nums[i] *= -1;
                }
            }

            return nums.Sum();
        }

        public static int CanCompleteCircuit(int[] gas, int[] cost) {
            int sum;
            for (int i = 0; i < gas.Length;) {
                int get = gas[i] - cost[i];
                if (get < 0) {
                    i++;
                    continue;
                } else {
                    sum = 0;
                    int nextIndex = i;
                    int count = 0;
                    while (sum >= 0 && count < gas.Length) {
                        sum += gas[nextIndex] - cost[nextIndex];
                        nextIndex = (nextIndex + 1) % gas.Length;
                        if (sum >= 0) {
                            count++;
                        } else {
                            if (nextIndex <= i) {//注意：sum < 0
                                return -1;      //说明转了一个轮回，转回的必要条件是起始索引不在0，
                            }                   //既然不在0那么从0开始的区间一定是因为不满足题意
                            break;              //而跳到了不满足题意的点后面，也就是说如果从前面
                        }                       //往后加（[start..end], start>=0, end为不满足
                    }                           //题意的点）的话是不满足题意的，就算循环回去到了前
                    if (count == gas.Length) {  //面，又怎么可能满足题意呢？
                        //成功了，sum连续地未出现负数的情况，满足题意
                        return i;
                    } else {
                        i = nextIndex;//跳到不满足题意的点的后面
                    }
                }
            }

            return -1;
        }

        public static IList<string> BuildArray(int[] target, int n) {
            List<string> res = new();
            int count = 1;
            for (int i = 0; i < target.Length; i++) {
                if (target[i] == count) {
                    res.Add("Push");
                } else {
                    res.Add("Push");
                    res.Add("Pop");
                    i--;
                }
                count++;
            }
            return res;
        }

        public static int Candy(int[] ratings) {
            int[] candies = new int[ratings.Length];
            for (int i = 0; i < candies.Length; i++) {
                candies[i] = 1;
            }
            for (int i = 1; i < candies.Length; i++) {
                if (ratings[i] > ratings[i - 1]) {
                    candies[i] = candies[i - 1] + 1;
                }
            }
            for (int i = candies.Length - 1; i >= 1; i--) {
                if (ratings[i - 1] > ratings[i]) {
                    candies[i - 1] = Math.Max(candies[i] + 1, candies[i - 1]);
                }
            }

            return candies.Sum();
        }

        public static bool LemonadeChange(int[] bills) {
            if (bills[0] > 5) {
                return false;//一定没零钱
            } else {
                Dictionary<int, int> map = new() {
                    [5] = 0,
                    [10] = 0,
                    [20] = 0,
                };
                for (int i = 0; i < bills.Length; i++) {
                    switch (bills[i]) {
                        case 5:
                            map[bills[i]]++;//直接入账了
                            break;
                        case 10:
                            if (map[5] > 0) {
                                map[5]--;
                                map[10]++;
                            } else {
                                return false;
                            }
                            break;
                        case 20:
                            //找回15块，15 = 10 + 5 = 5 + 5 + 5
                            if (map[5] < 1) {
                                return false;
                            } else if (map[5] == 1 || map[5] == 2) {
                                //>=2
                                if (map[10] < 1) {
                                    return false;
                                } else {
                                    map[10]--;
                                    map[5]--;
                                }
                            } else if (map[5] >= 3) {
                                if (map[10] > 0) {
                                    map[10]--;
                                    map[5]--;
                                } else {
                                    map[5] -= 3;
                                }
                            }
                            break;
                    }
                }

                return true;
            }
        }

        public static int TotalFruit(int[] fruits) {
            Dictionary<int, int> window = new();
            int cur1 = 0, cur2 = -1;
            int max = 0;
            for (int i = 0; i < fruits.Length; i++) {
                if ((!window.ContainsKey(fruits[i]) && window.Count <= 1) || window.ContainsKey(fruits[i])) {
                    if (!window.ContainsKey(fruits[i])) {
                        window.Add(fruits[i], 0);
                    }
                    window[fruits[i]]++;
                    cur2++;
                    max = Math.Max(max, cur2 - cur1 + 1);
                } else if (!window.ContainsKey(fruits[i]) && window.Count == 2) {
                    int kind = fruits[cur1];
                    while (window.ContainsKey(kind) && window.Count != 1) {
                        window[fruits[cur1]]--;
                        if (window[fruits[cur1]] == 0) {
                            window.Remove(fruits[cur1]);
                        }
                        cur1++;
                    }
                    window.Add(fruits[i], 1);
                    cur2++;
                }
            }

            return max;
        }

        public static int[][] ReconstructQueue(int[][] people) {
            Array.Sort(people, delegate (int[] a, int[] b) {
                if (a[0] == b[0]) {
                    return a[1] - b[1];
                }
                return b[0] - a[0];
            });//按照身高排有确定解，按照k排序解不确定，只有范围
            List<int[]> res = new();
            for (int i = 0; i < people.Length; i++) {
                res.Insert(people[i][1], people[i]);
            }
            return res.ToArray();
        }

        public static int FindMinArrowShots(int[][] points) {
            Array.Sort(points, (a, b) => { return a[0].CompareTo(b[0]); });
            int lappedBalloon = 0;
            for (int i = 1; i < points.Length;) {
                if (points[i][0].CompareTo(points[i - 1][1]) < 0) {
                    lappedBalloon++;
                    points[i][1] = Math.Min(points[i - 1][1], points[i][1]);
                }
            }
            return points.Length - lappedBalloon;
        }

        public static int CountStudents(int[] students, int[] sandwiches) {
            int[] sandwich = new int[2];
            for (int i = 0; i < students.Length; i++) {
                sandwich[students[i]]++;
            }
            for (int i = 0; i < sandwiches.Length;) {
                if (sandwiches[i] == 0 && sandwich[0] > 0) {
                    sandwich[0]--;
                    i++;
                } else if (sandwiches[i] == 1 && sandwich[1] > 0) {
                    sandwich[1]--;
                    i++;
                } else {
                    break;//队列中不存在想吃当前三明治的人，结束
                }
            }
            return sandwich.Sum();
        }

        public static int GloryOfTheHorseOfLight(int[] time, int tp) {
            if (tp == 0) {
                return time.Sum();
            }
            List<int> time2 = new();
            for (int i = 0; i < tp - 1; i++) {
                time2.Add(0);
            }
            time2.AddRange(time);
            for (int i = 0; i < tp - 1; i++) {
                time2.Add(0);
            }
            int cur1 = 0;
            int cur2 = cur1 + tp - 1;
            int windows = 0;
            int maxTime;
            for (int i = 0; i < tp; i++) {
                windows += time2[i];
            }
            maxTime = windows;
            int timeSum = 0;
            while (true) {
                timeSum += time2[cur2];
                cur2++;
                if (cur2 == time2.Count) {
                    break;
                }
                windows += time2[cur2];
                windows -= time2[cur1];
                cur1++;
                maxTime = Math.Max(maxTime, windows);
            }
            return timeSum - maxTime;
        }

        public static int KthGrammar(int n, int k) {
            if (n == 1) { return 0; }
            int count = 1 << (n - 2);//此位置的根节点为01
            return k > count ? RecursionKth(10, n - 1, k - count) : RecursionKth(01, n - 1, k);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputNum">根节点值，只能是10或01</param>
        /// <param name="level"></param>
        /// <param name="k">从左往右数的第几位？</param>
        /// <returns></returns>
        public static int RecursionKth(int inputNum, int level, int k) {
            if (level == 1) {
                return k switch {
                    2 => inputNum % 10,
                    1 => inputNum / 10,
                };
            }
            int lastLevelCount = 1 << (level - 1);//最后一行的一半数
            if ((k > lastLevelCount && inputNum == 01) || (k <= lastLevelCount && inputNum == 10)) {
                return RecursionKth(10, level - 1, k - lastLevelCount > 0 ? k - lastLevelCount : k);
            } else {
                return RecursionKth(01, level - 1, k - lastLevelCount > 0 ? k - lastLevelCount : k);
            }
        }

        public static int[] NextGreaterElement(int[] nums1, int[] nums2) {
            int[] dp = new int[nums2.Length];//下标
            dp[^1] = -1;
            for (int i = nums2.Length - 2; i >= 0; i--) {
                if (nums2[i] < nums2[i + 1]) {
                    dp[i] = i + 1;
                } else {
                    int idx = dp[i + 1];
                    while (idx != -1 && nums2[idx] < nums2[i]) { idx = dp[idx]; }
                    dp[i] = idx;
                }
            }

            Dictionary<int, int> dic = new();
            for (int i = 0; i < nums2.Length; i++) {
                dic[nums2[i]] = dp[i];
            }
            int[] res = new int[nums1.Length];
            for (int i = 0; i < nums1.Length; i++) {
                res[i] = dic[nums1[i]] == -1 ? -1 : nums2[dic[nums1[i]]];
            }
            return res;
        }

        public static int[] NextGreaterElements(int[] nums) {
            int[] dp = new int[nums.Length];//下标
            int maxIdx = -1;
            int max = int.MinValue;
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] > max) {
                    max = nums[i];
                    maxIdx = i;
                }
            }
            dp[maxIdx] = -1;
            for (int i = 1; i < nums.Length; i++) {
                int trueIdx = (maxIdx - i + nums.Length) % nums.Length;
                if (nums[trueIdx] < nums[(trueIdx + 1) % nums.Length]) {
                    dp[trueIdx] = (trueIdx + 1) % nums.Length;
                } else {
                    int idx = dp[(trueIdx + 1) % nums.Length];
                    while (idx != -1 && nums[idx] <= nums[trueIdx]) { //循环查找下一个比trueIdx位置大的数
                        idx = dp[idx];
                    }
                    dp[trueIdx] = idx;
                }
            }

            for (int i = 0; i < dp.Length; i++) {
                dp[i] = dp[i] == -1 ? -1 : nums[dp[i]];
            }
            return dp;

        }

        public static string MergeAlternately(string word1, string word2) {
            int minLen = Math.Min(word1.Length, word2.Length);
            StringBuilder sb = new();
            for (int i = 0; i < 2 * minLen; i++) {
                sb.Append(i & 1 switch {
                    0 => word1[i >> 1],
                    _ => word2[i >> 1],
                });
            }
            sb.Append(minLen == word1.Length ? word2[minLen..] : word1[minLen..]);
            return sb.ToString();
        }

        public static int ArraySign(int[] nums) {
            bool hasNegative = false;
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] == 0) {
                    return 0;
                } else if (nums[i] < 0) {
                    hasNegative = !hasNegative;
                }
            }
            return hasNegative switch {
                true => -1,
                _ => 1,
            };
        }

        public static int CountMatches(IList<IList<string>> items, string ruleKey, string ruleValue) {
            int idx = ruleKey switch {
                "type" => 0,
                "color" => 1,
                _ => 2,
            };
            int count = 0;
            foreach (var rule in items) {
                if (rule[idx] == ruleValue) {
                    count++;
                }
            }
            return count;
        }

        public static int ReversePairs(int[] nums) {
            return DivideArray(nums, 0, nums.Length);
        }

        private static int DivideArray(int[] nums, int left, int right) {
            //[left..right)
            if (right - left <= 1) {
                return 0;
            } else {
                return
                DivideArray(nums, left, ((left - right) >> 1) + right) +
                DivideArray(nums, ((left - right) >> 1) + right, right) +
                Merge(nums, left, ((left - right) >> 1) + right, right);
            }
        }

        private static int Merge(int[] nums, int left, int mid, int right) {
            //[left..mid)
            //[mid..right)
            int cur1 = left, cur2 = mid;
            int i = 0;
            int[] helper = new int[right - left];
            int res = 0;
            while (true) {
                if (cur1 < mid && cur2 < right) {
                    if (nums[cur1] <= nums[cur2]) {
                        helper[i++] = nums[cur1++];
                    } else {
                        if (nums[cur1] > nums[cur2]) {
                            res += mid - cur1;
                        }
                        helper[i++] = nums[cur2++];
                    }
                } else if (cur1 < mid && cur2 >= right) {
                    helper[i++] = nums[cur1++];
                } else if (cur1 >= mid && cur2 < right) {
                    helper[i++] = nums[cur2++];
                } else {
                    for (int j = left; j < right; j++) {
                        nums[j] = helper[j - left];
                    }
                    return res;
                }
            }
        }

        public static int GetMod(string dividend, int divisor) {
            //如果dividend < divisor, 那么dividend一定不会溢出
            int thisMod = 0;
            string? remainedNum = null;
            if (int.TryParse(dividend, out int num)) {
                if (num < divisor) {
                    return num;
                }
            }
            for (int i = 0; i < dividend.Length; i++) {
                remainedNum += dividend[i];
                int added;
                if (int.Parse(remainedNum) / divisor != 0) {
                    added = int.Parse(remainedNum) - int.Parse(remainedNum) / divisor * divisor;
                    thisMod = 0;
                } else {
                    added = 0;//不够除了，remainedNum比divisor还小
                }
                if (added == 0) {// 不够
                    if (thisMod != 0) {//说明当前的余数还有东西
                        thisMod *= 10;
                        thisMod += int.Parse(dividend[i].ToString());//更新当前余数
                    }
                } else {
                    thisMod = thisMod * 10 + added;
                    remainedNum = thisMod.ToString();
                }
            }
            return thisMod;
        }

        public static IList<string> LetterCasePermutation(string s) {
            List<bool> alphabet = new();
            _resultLetterPermutation.Clear();
            for (int i = 0; i < s.Length; i++) {
                alphabet.Add(!int.TryParse(s[i].ToString(), out _));
            }
            bool[] path = alphabet.ToArray();
            _pathLetterPermutation = s;
            for (int i = 0; i < path.Length; i++) {
                if (path[i]) {
                    BacktrackingLetterPermutation(path, i);
                    break;
                }
            }
            if (_resultLetterPermutation.Count == 0) {
                _resultLetterPermutation.Add(_pathLetterPermutation);
            }
            return _resultLetterPermutation;
        }

        private static void BacktrackingLetterPermutation(bool[] path, int startIndex) {
            int idx = -1;
            for (int i = startIndex + 1; i < path.Length; i++) {
                if (path[i]) {
                    idx = i;//判断下一步从哪开始
                    break;
                }
            }
            if (startIndex == -1) {//递归树此层无字母可以改变大小写了
                _resultLetterPermutation.Add(_pathLetterPermutation);
            } else {
                for (int i = 0; i < 2; i++) {
                    if (i == 0) {
                        _pathLetterPermutation = _pathLetterPermutation[..startIndex]
                            + _pathLetterPermutation[startIndex].ToString().ToLower()
                            + _pathLetterPermutation[(startIndex + 1)..];
                    } else {
                        _pathLetterPermutation = _pathLetterPermutation[..startIndex]
                            + _pathLetterPermutation[startIndex].ToString().ToUpper()
                            + _pathLetterPermutation[(startIndex + 1)..];
                    }
                    BacktrackingLetterPermutation(path, idx);
                }
            }
        }

        static List<string> _resultLetterPermutation = new();
        static string? _pathLetterPermutation = null;

        public static int MagicalString(int n) {
            //描述串初始值为1，2
            string describe = "122";
            StringBuilder source = new("122");
            int curD = 2;
            int curS = 2;
            int count = 1;
            if (n <= 3) {
                return 1;
            }
            while (curS < n - 1) {
                if (describe[curD] == '2') {
                    if (source[^1] == '1') {
                        source.Append("22");
                    } else {
                        source.Append("11");
                        count += 2;
                    }
                    curS += 2;
                } else {
                    if (source[^1] == '1') {
                        source.Append('2');
                    } else {
                        source.Append('1');
                        count++;
                    }
                    curS++;
                }
                curD++;
                if (source.Length > describe.Length) {
                    describe = source.ToString();
                }
            }
            if (n < source.Length) {
                if (source[^1] == '1') {
                    return count - 1;
                } else {
                    return count;
                }
            }
            return count;
        }

        public static string ToHexadecimal(int num) {
            string res = "";
            while (num != 0) {
                num--;
                int digit = num % 26;
                int c = 64;
                res += (char)(c + digit + 1);
                num /= 26;
            }
            return res.ToString();
        }

        public static int[] TopKFrequent(int[] nums, int k) {
            Dictionary<int, int> dic = new();
            foreach (var item in nums) {
                if (!dic.ContainsKey(item)) {
                    dic[item] = 0;
                }
                dic[item]++;
            }
            Tuple<int, int>[] heap = new Tuple<int, int>[k];
            int i;
            var keys = dic.Keys.ToArray();
            for (i = 0; i < k; i++) {
                heap[i] = (keys[i], dic[keys[i]]).ToTuple();
                Insert(heap, i);
            }
            for (; i < keys.Length; i++) {
                if (dic[keys[i]] > heap[0].Item2) {
                    heap[0] = new Tuple<int, int>(keys[i], dic[keys[i]]);
                    Heapify(heap, k);//将最小的顶替掉，进入堆内的都是较大的数
                }
            }

            int[] res = new int[k];
            for (int j = 0; j < res.Length; j++) {
                res[j] = heap[j].Item1;
            }
            return res;

            static void Insert(Tuple<int, int>[] heap, int index) {
                int parent = (index - 1) / 2;
                while (heap[parent].Item2 > heap[index].Item2) {
                    (heap[parent], heap[index]) = (heap[index], heap[parent]);
                    index = parent;
                    parent = (index - 1) / 2;
                }
            }

            static void Heapify(Tuple<int, int>[] heap, int size) {
                int idx = 0;
                int left = (idx << 1) + 1;
                while (left < size) {
                    int right = left + 1;
                    int lowestIdx = (right < size && heap[right].Item2 < heap[left].Item2) ? right : left;
                    if (heap[lowestIdx].Item2 >= heap[idx].Item2) {
                        break;
                    }
                    (heap[lowestIdx], heap[idx]) = (heap[idx], heap[lowestIdx]);

                    idx = lowestIdx;
                    left = (idx << 1) + 1;
                }
            }
        }

        public static int MinMoves(int target, int maxDoubles) {
            if (maxDoubles == 0 || target == 1) {
                return target - 1;
            }
            int res = 0;
            if ((target & 1) == 1) {
                target--;
                res++;
            }
            while (target != 1 && maxDoubles != 0) {
                target >>= 1;
                res++;
                maxDoubles--;
                if (target != 1 && (target & 1) == 1) {
                    target--;
                    res++;
                }
            }
            if (target != 1) {
                res += target - 1;
            }
            return res;
        }

        public static bool SearchMatrix2(int[][] matrix, int target) {
            for (int i = 0; i < matrix.Length; i++) {
                if (matrix[i][^1] == target) {
                    return true;
                } else if (matrix[i][^1] > target) {
                    //在第i行中查找
                    int l = 0, r = matrix[i].Length - 1;
                    while (l <= r) {
                        int m = ((l - r) >> 1) + r;
                        if (matrix[i][m] == target) {
                            return true;
                        } else if (matrix[i][m] > target) {
                            r = m - 1;
                        } else {
                            l = m + 1;
                        }
                    }
                }
            }
            return false;
        }

        public static void SortColors(int[] nums) {
            // 0 1 2
            int zero = -1;//[0..zero]
            int two = nums.Length;//[i..nums.Length)
            int i = 0;//[zero..i)
            while (i != two) {
                if (nums[i] == 0) {
                    zero++;
                    (nums[i], nums[zero]) = (nums[zero], nums[i]);
                    i++;
                } else if (nums[i] == 2) {
                    two--;
                    (nums[i], nums[two]) = (nums[two], nums[i]);
                } else {
                    i++;
                }
            }
        }

        public static string PaperFold(int k) {
            Queue<string> que = new();
            RecursionPaperFold(k, 0, que, true);
            return string.Join(' ', que.ToArray());
        }

        private static void RecursionPaperFold(int k, int level, Queue<string> que, bool isDown) {
            if (k <= level) {
                return;
            }
            RecursionPaperFold(k, level + 1, que, true);
            que.Enqueue(isDown ? "down" : "up");
            RecursionPaperFold(k, level + 1, que, false);

        }

        public static bool IsValidBST2(TreeNode root) {
            return (RecursionIsBST(root) ?? new Tuple<int, int, bool>(-1, -1, true)).Item3;
        }

        private static Tuple<int, int, bool>? RecursionIsBST(TreeNode? root) {
            if (root is null) {
                return null;
            }
            var leftInfo = RecursionIsBST(root.left);
            var rightInfo = RecursionIsBST(root.right);
            int min = root.val, max = root.val;
            bool isBST = false;
            if (leftInfo is not null) {
                min = Math.Min(leftInfo.Item1, min);
                max = Math.Max(leftInfo.Item2, max);
            }
            if (rightInfo is not null) {
                min = Math.Min(rightInfo.Item1, min);
                max = Math.Max(rightInfo.Item2, max);
            }

            if ((leftInfo is null || leftInfo.Item3) &&
                (rightInfo is null || rightInfo.Item3) &&
                (leftInfo is null || leftInfo.Item2 < root.val) &&
                (rightInfo is null || root.val < rightInfo.Item1)) {
                isBST = true;
            }

            return new(min, max, isBST);
        }

        public static int DiameterOfBinaryTree(TreeNode root) {
            return RecursionDiameter(root).Item2;//高度，直径
        }

        private static Tuple<int, int> RecursionDiameter(TreeNode? node) {
            if (node is null) {
                return new(0, 0);
            }
            var left = RecursionDiameter(node.left);
            var right = RecursionDiameter(node.right);
            int dia = Math.Max(left.Item2, right.Item2);
            dia = Math.Max(dia, left.Item1 + right.Item1 + 1);

            return new(Math.Max(left.Item1, right.Item1) + 1, dia);
        }

        public static int MyAtoi(string s) {
            if (string.IsNullOrEmpty(s)) {
                return 0;
            }
            int curSpace = 0;
            while (curSpace < s.Length && s[curSpace] == ' ') {
                curSpace++;
            }
            bool negative = false;
            if (curSpace < s.Length && (s[curSpace] == '-' || s[curSpace] == '+')) {
                if (s[curSpace] == '-') {
                    negative = true;
                }
                curSpace++;
            }
            while (curSpace < s.Length && s[curSpace] == '0') {
                curSpace++;
            }
            if (curSpace >= s.Length) { return 0; }
            int res = 0;
            int curOtherChar = curSpace;
            while (curOtherChar < s.Length && int.TryParse(s[curOtherChar].ToString(), out _)) {
                curOtherChar++;
            }//找到了第一个不是数字的字符
            curOtherChar--;
            if (curOtherChar >= s.Length) { curOtherChar = s.Length - 1; }
            if (curOtherChar - curSpace + 1 > 10) {
                return negative ? int.MinValue : int.MaxValue;
            } else if (curOtherChar - curSpace + 1 == 10) {
                if (int.Parse(s[curSpace].ToString()) > 2) {
                    return negative ? int.MinValue : int.MaxValue;
                } else if (int.Parse(s[curSpace].ToString()) == 2) {
                    if (!negative && int.Parse(s[(curSpace + 1)..(curOtherChar + 1)]) > 147483647) {
                        return int.MaxValue;
                    } else if (negative && int.Parse(s[(curSpace + 1)..(curOtherChar + 1)]) > 147483648) {
                        return int.MinValue;
                    }
                }
            }
            for (int i = curSpace; i <= curOtherChar; i++) {
                if (int.TryParse(s[i].ToString(), out int tmp)) {
                    res += tmp * (int)Math.Pow(10, curOtherChar - i);
                } else {
                    break;
                }
            }

            return negative ? -res : res;
        }

        public static string GenerateLeftBracket(string s) {
            //例如
            //输入：1 + 1) * 3 + 4) / 5)
            //输出：(1 + 1) * ((3 + 4) / 5)
            if (string.IsNullOrWhiteSpace(s)) {
                return s;
            }
            HashSet<char> opSet = new() { '+', '*', '-', '/' };
            HashSet<char> symbols = new() { '+', '*', '-', '/', ')', ' ' };
            string[] parts = s.Split(symbols.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Stack<char> opStack = new();
            Stack<string> num = new();
            int cur = 0;
            int i = 0;
            while (i < s.Length) {
                if (opSet.Contains(s[i])) {
                    opStack.Push(s[i]);
                    i++;
                } else if (char.IsDigit(s[i])) {
                    num.Push(parts[cur]);
                    i += parts[cur++].Length;
                } else if (s[i] == ')') {
                    string num1 = num.Pop();
                    string num2 = num.Pop();
                    num.Push("(" + num2 + opStack.Pop() + num1 + ")");
                    i++;
                } else {
                    i++;
                }
            }
            while (opStack.Count > 0) {
                string num1 = num.Pop();
                string num2 = num.Pop();
                num.Push("(" + num2 + opStack.Pop() + num1 + ")");
            }
            return num.Pop();
        }

        public static string InfixToSuffix(string s) {
            if (string.IsNullOrEmpty(s)) {
                return "0";
            }
            s = String.Join("", s.Split(' '));
            string[] nums = s.Split(new char[] { '+', '-', '*', '×', '/', '÷', '(', ')', '^', '%' }, StringSplitOptions.RemoveEmptyEntries);
            if (nums.Length == 0) {
                throw new ArgumentException("表达式内无数字");
            }
            Dictionary<char, int> opPrio = new() {
                ['+'] = 1,
                ['-'] = 1,
                ['*'] = 2,
                ['×'] = 2,
                ['/'] = 2,
                ['÷'] = 2,
                ['^'] = 3,
                ['%'] = 3,
            };
            Stack<string> numStack = new();
            Stack<Tuple<char, int>> opStack = new();
            int bracketPrio = 0;
            int i = 0;
            int numCur = 0;
            while (i < s.Length) {
                if ((s[i] == '+' || s[i] == '-') &&
                        (i == 0 || s[i - 1] == '(' || opPrio.ContainsKey(s[i - 1]))) {
                    numStack.Push(s[i] + nums[numCur]);
                    i += nums[numCur++].Length;
                } else if (char.IsDigit(s[i])) {
                    numStack.Push(nums[numCur]);
                    i += nums[numCur++].Length - 1;
                } else if (s[i] == '(') {
                    bracketPrio += 3;
                } else if (s[i] == ')') {
                    Push(numStack, opStack);
                    bracketPrio -= 3;
                } else if (opPrio.ContainsKey(s[i])) {//如果是低优先级
                    int prio = bracketPrio + opPrio[s[i]];
                    while (opStack.Count != 0 && (prio <= opStack.Peek().Item2)) {
                        Push(numStack, opStack);
                    }
                    opStack.Push(new(s[i], prio));
                }
                i++;
            }
            while (opStack.Count != 0) {
                Push(numStack, opStack);
            }
            return numStack.Pop();

            static void Push(Stack<string> numStack, Stack<Tuple<char, int>> opStack) {
                string num1 = "";
                try {
                    num1 = numStack.Pop();
                } catch (InvalidOperationException) {
                    throw new ArgumentException("表达式内有空括号");
                }
                try {
                    string num2 = numStack.Pop();
                    numStack.Push(num1 + "_" + num2 + "_" + opStack.Pop().Item1);
                } catch (InvalidOperationException) {
                    throw new ArgumentException("输入的表达式符号附近的运算数不完整");
                }

            }
        }

        public static string SuffixToInfix(string s) {
            Stack<string> numSt = new();
            string[] components = s.Split('_');
            foreach (var item in components) {
                if (int.TryParse(item, out _)) {
                    numSt.Push(item);
                } else {
                    //opSt.Push(item);
                    var first = numSt.Pop();
                    var second = numSt.Pop();
                    numSt.Push('(' + first + item + second + ')');
                }
            }
            return numSt.Pop();
        }

        public static double Eval(string expression) {
            string[] components = expression.Split('_', StringSplitOptions.RemoveEmptyEntries);
            Stack<double> numStack = new();
            for (int i = 0; i < components.Length; i++) {
                if (double.TryParse(components[i], out double num)) {
                    numStack.Push(num);
                } else {
                    //opStack.Push(char.Parse(components[i]));
                    double num1 = numStack.Pop();
                    double num2 = numStack.Pop();
                    numStack.Push(char.Parse(components[i]) switch {
                        '+' => num1 + num2,
                        '-' => num1 - num2,
                        '%' => num1 % num2,
                        '/' or '÷' => num1 / num2,
                        '*' or '×' => num1 * num2,
                        '^' => Math.Pow(num1, num2),
                        _ => throw new ArgumentException("输入的表达式包含其他字符")
                    });
                }
            }
            return numStack.Pop();
        }

        public static int[] ZigZagMatrix(int[][] mat) {
            Tuple<int, int> p1 = new(0, 0);
            Tuple<int, int> p2 = new(0, 0);
            List<int> res = new();
            bool isDown = false;
            while (p1.Item1 != mat.Length && p2.Item2 != mat[0].Length) {
                res.AddRange(GenerateLineFromTo(mat, p1, p2, isDown));
                if (p1.Item2 < mat[0].Length - 1) {
                    p1 = new(p1.Item1, p1.Item2 + 1);
                } else {
                    p1 = new(p1.Item1 + 1, p1.Item2);
                }
                if (p2.Item1 < mat.Length - 1) {
                    p2 = new(p2.Item1 + 1, p2.Item2);
                } else {
                    p2 = new(p2.Item1, p2.Item2 + 1);
                }
                isDown = !isDown;
            }
            return res.ToArray();
        }

        private static IList<int> GenerateLineFromTo(int[][] mat, Tuple<int, int> p1, Tuple<int, int> p2, bool toDown) {
            Tuple<int, int> start = new(p1.Item1, p1.Item2);
            if (!toDown) {
                start = new(p2.Item1, p2.Item2);
            }
            int x = start.Item1, y = start.Item2;
            List<int> res = new();
            while (true) {
                if (x >= mat.Length || y >= mat[0].Length || x < 0 || y < 0) { break; }
                res.Add(mat[x][y]);
                if (toDown) {
                    x++;
                    y--;
                } else {
                    x--;
                    y++;
                }
            }
            return res;
        }

        public static void Rotate(int[][] matrix) {
            int a = matrix.Length;//边长
            for (int n = 0; n < a / 2; n++) {//层数从零开始
                for (int j = n; j < a - n - 1; j++) {
                    Tuple<int, int> p1 = new(n, j);
                    Tuple<int, int> p2 = new(p1.Item2, a - n - 1);
                    Tuple<int, int> p3 = new(a - n - 1, a - p1.Item2 - 1);
                    Tuple<int, int> p4 = new(a - p1.Item2 - 1, n);
                    int tmp = matrix[p1.Item1][p1.Item2];
                    matrix[p1.Item1][p1.Item2] = matrix[p4.Item1][p4.Item2];
                    matrix[p4.Item1][p4.Item2] = matrix[p3.Item1][p3.Item2];
                    matrix[p3.Item1][p3.Item2] = matrix[p2.Item1][p2.Item2];
                    matrix[p2.Item1][p2.Item2] = tmp;
                }
            }
        }

        public static int MaximumUnits(int[][] boxTypes, int truckSize) {
            Array.Sort(boxTypes, (a, b) =>
                b[1].CompareTo(a[1])
            );
            int remain = truckSize;
            int res = 0;
            for (int i = 0; i < boxTypes.Length; i++) {
                if (remain > boxTypes[i][0]) {
                    remain -= boxTypes[i][0];
                    res += boxTypes[i][0] * boxTypes[i][1];
                } else {
                    res += remain * boxTypes[i][1];
                    break;
                }
            }
            return res;
        }

        static List<int> lightsPos = new();
        static int resultLightsPos;

        public static int MinLights(string street) {
            street = street.ToUpper();
            resultLightsPos = int.MaxValue;
            int last = -1;
            for (int i = street.Length - 1; i >= 0; i--) {
                if (street[i] == '.') {
                    last = i;
                    break;
                }
            }
            BacktrackingMinLights(street, 0, last);

            return resultLightsPos;
        }

        public static void BacktrackingMinLights(string street, int startIndex, int lastIndex) {
            //x..xxx.x.xx.....xxx...xxxxx.x
            bool flag = false;
            for (int i = startIndex; i < street.Length; i++) {
                if (street[i] == '.') {
                    lightsPos.Add(i);
                    BacktrackingMinLights(street, i + 1, lastIndex);//收集一下如果末尾时安装上最后一个路灯会如何
                    lightsPos.RemoveAt(lightsPos.Count - 1);
                    if (i == lastIndex) {
                        GatherMinLightsAnswer(street);//收集一下末尾时不要最后一个路灯的情况
                    }
                    flag = true;
                }
            }
            if (!flag) {//开始收集
                GatherMinLightsAnswer(street);
            }
        }

        private static void GatherMinLightsAnswer(string street) {
            var str = street.ToCharArray();
            int cur = 0;
            for (int i = 0; i < str.Length; i++) {
                if (str[i] == '.') {
                    if (cur < lightsPos.Count && Math.Abs(lightsPos[cur] - i) <= 1) {
                        if (i == str.Length - 1 || str[i + 1] == 'X' || i - lightsPos[cur] == 1) {
                            cur++;
                        }
                    } else { return; }
                }
            }
            resultLightsPos = Math.Min(resultLightsPos, lightsPos.Count);
        }

        public static int MinLights2(string street) {
            street = street.ToUpper();
            int res = 0;
            for (int i = 0; i < street.Length;) {
                if (street[i] == 'X') {
                    i++;
                } else {// .
                    res++;
                    if (i == street.Length - 1) {
                        break;
                    }
                    if (street[i + 1] == 'X') {
                        i += 2;//.X
                    } else {//..X ...
                        i += 3;
                    }
                }
            }
            return res;
        }

        public static int NumMatchingSubseq(string s, string[] words) {
            int count = 0;
            int cur1 = 0, cur2;
            for (int i = 0; i < words.Length; i++) {
                for (cur2 = 0; cur2 < words[i].Length && cur1 < s.Length; cur1++) {
                    //s         abcdef  cur1
                    //words[i]  cdf     cur2
                    if (words[i][cur2] == s[cur1]) {
                        cur2++;
                    }
                }
                if (cur2 == words[i].Length) {
                    count++;
                }
                cur1 = cur2 = 0;
            }
            return count;
        }

        public static int NumMatchingSubseq2(string s, string[] words) {
            Queue<int>[] qus = new Queue<int>[26];
            for (int i = 0; i < qus.Length; i++) {
                qus[i] = new();
            }
            for (int i = 0; i < s.Length; i++) {
                qus[s[i] - 'a'].Enqueue(i);
            }
            Queue<int>[] back = new Queue<int>[26];
            qus.CopyTo(back, 0);
            int res = 0;
            for (int i = 0; i < words.Length; i++) {
                int lastIdx = -1;
                bool flag = false;
                for (int j = 0; j < words[i].Length; j++) {
                    char c = words[i][j];
                    if (qus[c - 'a'].Count > 0) {
                        int thsIdx = qus[c - 'a'].Dequeue();
                        if (thsIdx > lastIdx) {
                            lastIdx = thsIdx;
                        } else { j--; }
                    } else {
                        flag = true;
                        break;
                    }
                }//还原现场
                back.CopyTo(qus, 0);
                if (!flag) { res++; }
            }
            return res;
        }

        public static bool PossibleBipartition(int n, int[][] dislikes) {
            if (dislikes.Length == 0) {
                return true;
            }
            List<int>[] people = new List<int>[n + 1];
            for (int i = 0; i < people.Length; i++) {
                people[i] = new();
            }
            foreach (var item in dislikes) {
                people[item[0]].Add(item[1]);
                people[item[1]].Add(item[0]);
            }
            UnionFindSet<int> set = new();
            for (int i = 1; i <= n; i++) {
                set.Add(i);
            }
            foreach (List<int> group in people) {
                for (int i = 0; i < group.Count - 1; i++) {
                    set.Union(group[i], group[i + 1]);
                }
            }
            foreach (var item in dislikes) {
                if (set.IsSameSet(item[0], item[1])) {
                    return false;
                }
            }
            return true;
        }

        public static int FindKthLargest(int[] nums, int k) {
            //自底向上
            int[] heap = new int[k];
            for (int i = 0; i < k; i++) {
                heap[i] = nums[i];
            }
            for (int i = k - 1; i >= 0; i--) {
                //下沉
                Sink(heap, i, k);
            }
            for (int i = k; i < nums.Length; i++) {
                if (nums[i] > heap[0]) {
                    heap[0] = nums[i];
                }
                Sink(heap, 0, k);
            }
            return heap[0];

            static void Sink(int[] heap, int index, int size) {
                int idx = index;
                int left = (idx << 1) + 1;
                while (left < size) {
                    int right = left + 1;
                    int lowestIdx = right < size && heap[right] < heap[left] ? right : left;
                    if (heap[idx] > heap[lowestIdx]) {
                        (heap[idx], heap[lowestIdx]) = (heap[lowestIdx], heap[idx]);
                    } else {
                        break;
                    }
                    idx = lowestIdx;
                    left = (idx << 1) + 1;
                }
            }
        }

        public static int LongestConsecutive(int[] nums) {
            if (nums.Length == 0) {
                return 0;
            }
            UnionFindSet<int> set = new();
            for (int i = 0; i < nums.Length; i++) {
                set.Add(nums[i]);
            }
            for (int i = 0; i < nums.Length; i++) {
                set.Union(nums[i], nums[i] + 1);
                set.Union(nums[i] - 1, nums[i]);
            }
            return set.GetEachSetCount().Max();
        }

        public static string Hanoi(int n) {
            _hanoiStep = new();
            RecursionHanoi(n, "left", "right", "mid");
            return _hanoiStep.ToString();
        }

        static StringBuilder _hanoiStep;

        private static void RecursionHanoi(int n, string from, string to, string other) {
            //return $"把{n}从{from}移动到{to}";
            if (n == 1) {
                _hanoiStep.AppendLine($"把第{n}个环从{from}柱移动到{to}柱");
                return;
            } else {
                RecursionHanoi(n - 1, from, other, to);
                _hanoiStep.AppendLine($"把第{n}个环从{from}柱移动到{to}柱");
                RecursionHanoi(n - 1, other, to, from);
            }
        }

        public static bool IsPowerOfTwo(int n) => (n > 0) && ((n - 1) & n) == 0;

        public static void ReverseStack(ref Stack<int> stack) {
            myReverseSt = stack;
            status = 1;
            RecursionReverseSt();
            stack = myReverseSt;
        }

        static Stack<int> myReverseSt;
        static int reverseLevel;
        static int status;
        static int top;

        private static long RecursionReverseSt(long arg = long.MinValue) {
            if (status == 1) {
                if (myReverseSt.Count > reverseLevel) {
                    top = myReverseSt.Pop();
                    status = 2;
                    RecursionReverseSt(myReverseSt.Peek());//top应该在接下来的递归的末尾处被放入st中
                    //逆序一次过程进行完毕
                    if (reverseLevel < myReverseSt.Count) {
                        RecursionReverseSt();
                    }
                }
                return long.MinValue;
            } else {
                if (myReverseSt.Count > reverseLevel) {
                    myReverseSt.Push((int)RecursionReverseSt(myReverseSt.Pop()));
                } else {//抵达栈底
                    myReverseSt.Push(top);
                    status = 1;//恢复状态
                    reverseLevel++;//多放好了一个位置，此位置不应该再碰
                }
                return arg;
            }
        }

        public static long[,] MatrixMultiply(long[,] matrix1, long[,] matrix2) {
            if (matrix1.GetLength(1) != matrix2.GetLength(0)) {
                throw new ArgumentException("矩阵维数错误");
            }
            long[,] res = new long[matrix1.GetLength(0), matrix2.GetLength(1)];
            for (int r = 0; r < res.GetLength(0); r++) {
                for (int c = 0; c < res.GetLength(1); c++) {
                    for (int k = 0; k < matrix1.GetLength(1); k++) {
                        res[r, c] += matrix1[r, k] * matrix2[k, c];
                    }
                }
            }
            return res;
        }

        public static long FastFibonacci(int n) {
            long[,] transform = new long[,] { { 0, 1 }, { 1, 1 } };
            var res = RecursionMatrixPow(transform, n);
            return res[0, 1];
        }

        private static long[,] RecursionMatrixPow(long[,] matrix, int n) {
            if (n == 1) {
                return matrix;
            } else {
                var tmp = RecursionMatrixPow(matrix, n >> 1);
                var res = MatrixMultiply(tmp, tmp);
                if ((n & 1) == 0) {
                    return res;
                } else {
                    return MatrixMultiply(matrix, res);
                }
            }
        }

        public static string HanoiNoRecursion(int n) {
            Stack<ValueTuple<int, string, string, string, int>> st = new();
            st.Push(new(n, "left", "right", "mid", 0));
            StringBuilder res = new();//状态值说明：
            //0：需要进行第一个分支
            //1：需要进行第二个分支
            //2：此节点下的两个分支全部进行完毕
            while (st.Count > 0) {
                var cur = st.Pop();
                if (cur.Item1 < 2) {//最底层————叶子节点
                    res.AppendLine($"把第{cur.Item1}个环从{cur.Item2}柱移动到{cur.Item3}柱");
                } else {//cur代表的是当前遍历到的父节点
                    if (cur.Item5 == 0) {
                        cur.Item5 = 1;//维护父节点cur的状态值
                        st.Push(cur);
                        var cur1 = (cur.Item1 - 1, cur.Item2, cur.Item4, cur.Item3, 0);
                        //新节点优先遍历左子树，即0
                        st.Push(cur1);//本次压栈一定使当前层的左子树全部遍历完后才会接触到栈中的cur
                    } else if (cur.Item5 == 1) {
                        res.AppendLine($"把第{cur.Item1}个环从{cur.Item2}柱移动到{cur.Item3}柱");
                        cur.Item5 = 2;//同上，也维护父节点cur的状态值
                        st.Push(cur);
                        var cur2 = (cur.Item1 - 1, cur.Item4, cur.Item3, cur.Item2, 0);
                        st.Push(cur2);//本次压栈一定使当前层的右子树全部遍历完后才会接触到栈中的cur
                    }
                }
            }
            return res.ToString();
        }

        public static IList<string> ConvertString(string num) {
            _resultConvert.Clear();
            BacktrackingConvertStr(num, 0);
            List<string> res = new();
            foreach (var item in _resultConvert) {
                res.Add(item.ToString());
            }
            return res;
        }

        static List<int> _pathConvert = new();
        static IList<StringBuilder> _resultConvert = new List<StringBuilder>();

        private static void BacktrackingConvertStr(string num, int startIdx) {
            if (startIdx == num.Length) {
                StringBuilder sb = new();
                for (int i = 0; i < _pathConvert.Count; i++) {
                    sb.Append((char)('a' + _pathConvert[i] - 1));
                }
                _resultConvert.Add(sb);
                return;
            }

            for (int i = startIdx; i < Math.Min(startIdx + 3, num.Length); i++) {
                var tmp = num[startIdx..(i + 1)];
                if (int.TryParse(tmp, out int res)) {
                    if (res >= 1 && res <= 26) {
                        _pathConvert.Add(res);
                        BacktrackingConvertStr(num, i + 1);
                        _pathConvert.RemoveAt(_pathConvert.Count - 1);
                    } else if (res > 26) {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 取得num串从startIdx开始转换为字母的转换方式数量
        /// </summary>
        /// <param name="num">原数字string串</param>
        /// <param name="startIdx">从startIdx出发计算num有多少种转换方式</param>
        /// <returns>转换方式数量</returns>
        public static int RecursionConvertStr(string num, int startIdx) {
            if (startIdx >= num.Length) {
                return 1;//空串也仅有一种转换方式，或者理解为能够递归到此处说明路径一定都满足了tmp的区间限制
            } else if (startIdx == num.Length - 1) {
                return num[startIdx] != '0' ? 1 : 0;
            }
            //剩余字符一定>=2个
            //一种情况，当前字符就可以成为一种选择
            int case1 = 0, case2 = 0;
            if (num[startIdx] != '0') {
                case1 = RecursionConvertStr(num, startIdx + 1);
                int tmp = int.Parse(num[startIdx..(startIdx + 2)]);
                if (tmp <= 26 && tmp > 0) { case2 = RecursionConvertStr(num, startIdx + 2); }//==> 1 * Recursion....
            } else {
                int tmp = int.Parse(num[startIdx..(startIdx + 2)]);
                if (tmp <= 26 && tmp > 0) { //一定<=26
                    case1 = RecursionConvertStr(num, startIdx + 2);
                }//==> 1 * Recursion....
                if (startIdx + 3 <= num.Length) {
                    tmp = int.Parse(num[startIdx..(startIdx + 3)]);
                    if (tmp <= 26 && tmp > 0) { //一定<=26
                        case2 = RecursionConvertStr(num, startIdx + 3);
                    }
                }

            }
            return case1 + case2;
        }

        public static int ConvertStrDP(string num) {
            int[] dp = new int[num.Length + 1];
            dp[^1] = 1;
            dp[^2] = num[^1] != '0' ? 1 : 0;
            for (int i = dp.Length - 3; i >= 0; i--) {
                //对于现在的每一位，都应该
                int case1 = 0, case2 = 0;
                if (num[i] != '0') {
                    case1 = dp[i + 1];
                    int tmp = int.Parse(num[i..(i + 2)]);
                    if (tmp <= 26 && tmp > 0) { case2 = dp[i + 2]; }//==> 1 * Recursion....
                } else {
                    int tmp = int.Parse(num[i..(i + 2)]);
                    if (tmp <= 26 && tmp > 0) { //一定<=26
                        case1 = dp[i + 2];
                    }//==> 1 * Recursion....
                    if (i + 3 <= num.Length) {
                        tmp = int.Parse(num[i..(i + 3)]);
                        if (tmp <= 26 && tmp > 0) { //一定<=26
                            case2 = dp[i + 3];
                        }
                    }
                }
                dp[i] = case1 + case2;
            }
            return dp[0];
        }

        public static int Knapsack01MaxValue(int[] weight, int[] values, int maxWeight) {
            return Recursion01KnapsackTD(weight, values, maxWeight, 0, 0);
        }

        public static int Recursion01KnapsackTD(int[] weight, int[] values, int remain, int startIdx, int sum) {
            int tryValue = sum;
            for (int i = startIdx; i < weight.Length; i++) {
                if (weight[i] <= remain) {
                    tryValue = Math.Max(Recursion01KnapsackTD(weight, values, remain - weight[i], i + 1, sum + values[i]), tryValue);
                }
            }
            return tryValue;
        }

        public static int Recursion01KnapsackBU(int[] weight, int[] values, int remain, int idx) {
            //返回从idx开始，当前状态的背包最多能装多大
            if (idx == weight.Length - 1) {
                return weight[idx] <= remain ? values[idx] : 0;
            }
            //没遍历到底
            //选当前物品：
            int case1 = remain >= weight[idx] ? values[idx] + Recursion01KnapsackBU(weight, values, remain - weight[idx], idx + 1) : -1;
            //不选当前物品：
            int case2 = Recursion01KnapsackBU(weight, values, remain, idx + 1);
            return Math.Max(case1, case2);
        }

        static int queenColumn;//列的竖直投影
        static int queenleftDiag;//递减对角线延伸到第一行，负数需要移位
        static int queenrightDiag;//递增，同上
        static int queenMask;

        public static int NQueen(int n) {
            queenColumn = (1 << n) - 1;
            queenleftDiag = (1 << (2 * n - 1)) - 1;
            queenrightDiag = (1 << (2 * n - 1)) - 1;
            queenMask = (1 << n) - 1;
            return RecursionNQueen(n, 0);
        }

        private static int RecursionNQueen(int n, int row) {
            if (row == n) {
                return 1;
            }
            int res = 0;
            int filter = queenMask & (queenColumn & ((queenrightDiag << (row)) >> (n - 1)) & (queenleftDiag >> row));
            //filter = queenMask & ~filter;
            int rightest, bak;
            while (filter != 0) {
                //在当前行中遍历
                rightest = filter & (~filter + 1);
                filter -= rightest;
                bak = n - 1 - row;
                queenleftDiag ^= (rightest << row);
                queenrightDiag ^= rightest << bak;
                queenColumn ^= rightest;
                res += RecursionNQueen(n, row + 1);
                queenleftDiag |= (rightest << row);
                queenrightDiag |= rightest << bak;
                queenColumn |= rightest;
            }
            return res;
        }

        public static int LengthOfLISDP(int[] nums) {
            dpLIS = new();
            return RecursionLengthOfLIS2(nums, 0);
        }

        static Dictionary<int, int> dpLIS = new();

        /// <summary>
        /// 取大于lastNum的最长递增子序列长度
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="startIdx"></param>
        /// <returns></returns>
        private static int RecursionLengthOfLIS2(int[] nums, int startIdx) {
            int lastNum = startIdx == 0 ? int.MinValue : nums[startIdx - 1];
            if (dpLIS.ContainsKey(startIdx)) {
                return dpLIS[startIdx];
            }

            if (startIdx == nums.Length - 1) {
                return nums[startIdx] > lastNum ? 1 : 0;
            }
            int max = 0;
            for (int i = startIdx; i < nums.Length; i++) {
                if (nums[i] > lastNum) {
                    max = Math.Max(1 + RecursionLengthOfLIS2(nums, i + 1), max);
                }
            }
            dpLIS.Add(startIdx, max);
            return max;
        }

        public static int LengthOfLISDP2(int[] nums) {
            int[] dp = new int[nums.Length + 1];
            for (int i = nums.Length - 1; i >= 0; i--) {
                int last = i == 0 ? int.MinValue : nums[i - 1];
                if (i == nums.Length - 1) {
                    dp[i] = nums[i] > last ? 1 : 0;
                    continue;
                }
                int max = 0;
                for (int j = i; j < nums.Length; j++) {
                    if (nums[j] > last) {
                        max = Math.Max(1 + dp[j + 1], max);
                    }
                }
                dp[i] = max;
            }
            return dp[0];
        }

        public static int[] PivotArray(int[] nums, int pivot) {
            //[0..i) 小于p
            //[i..j) 等于p
            //[r..)  大于p

            int i = 0, j = 0;
            int r = nums.Length;
            for (int k = 0; j != r; k++) {
                if (nums[k] < pivot) {
                    (nums[k], nums[i]) = (nums[i], nums[k]);
                    i++;
                    j++;
                } else if (nums[k] > pivot) {
                    r--;
                    (nums[k], nums[r]) = (nums[r], nums[k]);
                    k--;
                } else {
                    j++;
                }
            }
            return nums;
        }

        /// <summary>
        /// 给定一个整型数组cards，代表数值不同的纸牌排成一条线。玩家A和玩家B依次拿走每张纸牌，规定玩家A先拿，玩家B后拿，但是每个玩家每次只能拿走最左或最右的纸牌，玩家A和玩家B都绝顶聪明。请返回最后获胜者的分数。
        /// </summary>
        /// <param name="cards">纸牌数组cards</param>
        /// <returns>最后获胜者的分数</returns>
        public static int CardsGame(int[] cards) {
            return Math.Max(
            CardsGameFirst(cards, 0, cards.Length - 1),
            CardsGameSecond(cards, 0, cards.Length - 1)
            );
        }

        private static int CardsGameFirst(int[] cards, int left, int right) {
            if (left == right) {
                return cards[left];
            }
            //只能取left或right
            return Math.Max(
                cards[left] + CardsGameSecond(cards, left + 1, right),
                cards[right] + CardsGameSecond(cards, left, right - 1)
                );
        }

        private static int CardsGameSecond(int[] cards, int left, int right) {
            if (left == right) {
                return 0;
            }
            return Math.Min(
                CardsGameFirst(cards, left + 1, right),
                CardsGameFirst(cards, left, right - 1)
                );
        }

        public static int CardsGameDP(int[] cards) {
            int n = cards.Length;
            int[,] f = new int[n, n];
            int[,] s = new int[n, n];
            for (int i = 0; i < n; i++) {
                f[i, i] = cards[i];
            }

            for (int i = 1; i < n; i++) {
                for (int j = 0; j + i < n; j++) {
                    f[j, j + i] = Math.Max(
                        cards[j] + s[j + 1, j + i],
                        cards[j + i] + s[j, j + i - 1]);
                    s[j, j + i] = Math.Min(
                        f[j, j + i - 1],
                        f[j + 1, j + i]);
                }
            }

            return Math.Max(f[0, n - 1], s[0, n - 1]);
        }

        public static int GetChangeWay(int[] values, int capacity) {
            return RecursionGetChange(values, 0, capacity);
        }

        /// <summary>
        /// 从index开始选择零钱有几种找零方案
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="values"></param>
        /// <param name="index"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        private static int RecursionGetChange(int[] values, int index, int capacity) {
            if (index >= values.Length) {
                return capacity == 0 ? 1 : 0;
            }
            int times = 0;
            int ways = 0;
            int newCapcty = capacity - times * values[index];
            while (newCapcty >= 0) {
                int tmp = RecursionGetChange(values, index + 1, newCapcty);
                ways += 1 * tmp;
                newCapcty = capacity - ++times * values[index];
            }
            return ways;
        }

        public static int GetChangeWayDP(int[] coins, int amount) {
            int[,] dp = new int[coins.Length + 1, amount + 1];
            dp[coins.Length, 0] = 1;
            //idx最后一列已经填充完毕
            for (int idx = dp.GetLength(0) - 2; idx >= 0; idx--) {
                for (int cap = 0; cap <= amount; cap++) {
                    int ways = 0;
                    //while (newCapcty >= 0) {//优化：类似于前缀和
                    //    int tmp = dp[idx + 1, newCapcty];
                    //    ways += 1 * tmp;
                    //    newCapcty = cap - ++times * values[idx];
                    //}
                    ways = dp[idx + 1, cap];//times为0
                    if (cap - coins[idx] >= 0) {
                        ways += dp[idx, cap - coins[idx]];
                    }
                    dp[idx, cap] = ways;
                }
            }
            return dp[0, amount];
        }

        public static bool CanPartition(int[] nums) {
            int sum = nums.Sum();
            return RecursionCanParti(nums, sum, 0, sum);
        }

        private static bool RecursionCanParti(int[] nums, int sum, int startIdx, int rest) {
            if (startIdx == nums.Length) {
                return (sum >> 1) == rest;
            }
            //选择当前位置的元素
            bool res1 = RecursionCanParti(nums, sum, startIdx + 1, rest - nums[startIdx]);
            //不选择当前位置的元素
            bool res2 = RecursionCanParti(nums, sum, startIdx + 1, rest);
            return res1 || res2;
        }

        public static bool CanPartitionDP(int[] nums) {
            int sum = nums.Sum();
            if ((sum & 1) == 1) {
                return false;
            }
            bool[] dp = new bool[sum + 1];
            for (int i = 0; i < dp.GetLength(0); i++) {
                dp[i] = (sum >> 1) == i;
            }
            for (int i = nums.Length - 1; i >= 0; i--) {
                for (int j = dp.GetLength(0) - 1; j >= 0; j--) {
                    bool way1 = j - nums[i] >= 0 && dp[j - nums[i]];
                    dp[j] |= way1;
                }
            }

            return dp[sum];
        }

        public static int MinStickers(string[] stickers, string target) {
            int[,]? sticMap = new int[stickers.Length, 26];
            Dictionary<string, int> map = new();
            map.Add("", 0);
            #region 预处理
            for (int i = 0; i < stickers.Length; i++) {
                foreach (var item in stickers[i]) {
                    sticMap[i, item - 'a']++;
                }
            }
            var arr = target.ToCharArray();
            Array.Sort(arr);
            #endregion
            return RecursionMinStic(new(arr), sticMap, map);
        }

        private static int RecursionMinStic(string target, int[,] sticMap, Dictionary<string, int> map) {
            if (map.ContainsKey(target)) {
                return map[target];
            }
            int[] target2 = new int[26];
            for (int i = 0; i < target.Length; i++) {
                target2[target[i] - 'a']++;
            }
            //去除target[0]
            int res = int.MaxValue;
            StringBuilder sb = new();
            for (int i = 0; i < sticMap.GetLength(0); i++) {
                sb.Clear();
                if (sticMap[i, target[0] - 'a'] > 0) {
                    for (int j = 0; j < 26; j++) {
                        sb.Append(new string((char)('a' + j), Math.Max(target2[j] - sticMap[i, j], 0)));
                    }
                    var tmp = RecursionMinStic(sb.ToString(), sticMap, map);
                    if (tmp != -1) {
                        res = Math.Min(res, 1 + tmp);
                    }
                }
            }
            map.Add(target, res == int.MaxValue ? -1 : res);
            return map[target];
        }

        public static int MinElements(int[] nums, int limit, int goal) {
            return (int)(Math.Ceiling(Math.Abs(goal - nums.Sum(x => (long)x)) * 1.0 / limit));
        }

        public static int IntegerBreak(int n) {
            if (n == 0) {
                return 1;
            }
            int res = 0;
            for (int i = 2; i <= n; i++) {
                res = Math.Max(res, i * IntegerBreak(n - i));
            }
            return res;
        }

        public static int IntegerBreakDP(int n) {
            if (n == 2) {
                return 1;
            } else if (n == 3) {
                return 2;
            }
            int[] dp = new int[n + 1];
            dp[0] = 1;
            for (int i = 3; i <= n; i++) {
                int res = 0;
                for (int j = 2; j <= i; j++) {
                    res = Math.Max(res, j * dp[i - j]);
                }
                dp[i] = res;
            }
            return dp[n];
        }

        /// <summary>
        /// 失败的力扣古董键盘题目，写不出来DP解法
        /// </summary>
        /// <param name="k"></param>
        /// <param name="n"></param>
        /// <param name="doneStr"></param>
        /// <returns></returns>
        private static int RecursionKeyboard(int k, int n, string doneStr) {
            if (doneStr.Length == n) {
                //mapKeyboard.Add(doneStr, 1);
                return 1;
            }
            int[] used = new int[26];
            string newStr = doneStr;
            int res = 0;
            foreach (var item in doneStr) {
                used[item - 'a']--;//需要遍历，不必计算次数
            }
            for (int i = 0; i < used.Length; i++) {
                used[i] += k;
            }
            for (int i = 0; i < used.Length; i++) {
                if (used[i] > 0) {
                    res += RecursionKeyboard(k, n, newStr + (char)('a' + i));
                }
            }
            return res;
        }

        public static int CoinChange(int[] coins, int amount) {
            if (amount == 0) {
                return 0;
            }
            int count = int.MaxValue;
            for (int i = 0; i < coins.Length; i++) {
                if (amount - coins[i] >= 0) {
                    var tmp = CoinChange(coins, amount - coins[i]);
                    if (tmp >= 0) {//大于0才说明能继续换零钱
                        count = Math.Min(count, 1 + tmp);
                    }
                }
            }
            return count == int.MaxValue ? -1 : count;
        }

        public static int CoinChangeDP(int[] coins, int amount) {
            int[] dp = new int[amount + 1];
            //0不用初始化
            for (int i = 1; i < dp.Length; i++) {
                int count = int.MaxValue;
                for (int j = 0; j < coins.Length; j++) {
                    if (i - coins[j] >= 0 && dp[i - coins[j]] >= 0) {
                        count = Math.Min(count, 1 + dp[i - coins[j]]);
                    }
                }
                dp[i] = count == int.MaxValue ? -1 : count;
            }
            return dp[amount];
        }

        public static int LongestPalindrome(string s) {
            Dictionary<char, int> dic = new();
            foreach (var item in s) {
                if (!dic.ContainsKey(item)) {
                    dic.Add(item, 0);
                }
                dic[item]++;
            }
            int oddsCount = -1;
            int oddsSum = 0;
            int evenSum = 0;
            foreach (var item in dic) {
                if ((item.Value & 1) == 1) {
                    oddsCount++;
                    oddsSum += item.Value;
                } else {
                    evenSum += item.Value;
                }
            }
            if (oddsCount >= 0) {
                return oddsSum - oddsCount + evenSum;
            } else {
                return evenSum;
            }
        }

        public static string LongestSubPalindrome(string s) {
            int[] dp = new int[s.Length * 2 + 1];//记录了从该位置出发的单臂长度，最小为0
            StringBuilder sb = new();
            for (int i = 0; i < s.Length; i++) {
                sb.Append('#');
                sb.Append(s[i]);
            }
            sb.Append('#');
            for (int i = 1; i < sb.Length; i++) {
                int oppsite = i - 2;//i - 2 * 1
                for (int j = 1; oppsite >= 0; j++, oppsite = i - (j << 1)) {
                    // if (sb[i] == sb[oppsite] && dp[i - j] + i - j + 1 == i) {
                    if (sb[i] == sb[oppsite] && dp[i - j] + 1 == j) {
                        dp[i - j]++;
                    }
                }
            }
            int max = 0;
            int maxIdx = -1;
            for (int i = 0; i < dp.Length; i++) {
                if (dp[i] > max) {
                    max = dp[i];
                    maxIdx = i;
                }
            }
            return s[((maxIdx - max) >> 1)..((maxIdx + 1 + max) >> 1)];
        }

        public static int CountSubstrings(string s) {
            int[] dp = new int[s.Length * 2 + 1];//记录了从该位置出发的单臂长度，最小为0
            StringBuilder sb = new();
            for (int i = 0; i < s.Length; i++) {
                sb.Append('#');
                sb.Append(s[i]);
            }
            sb.Append('#');
            int res = 0;
            for (int i = 1; i < sb.Length; i++) {
                int oppsite = i - 2;//i - 2 * 1
                for (int j = 1; oppsite >= 0; j++, oppsite = i - (j << 1)) {
                    // if (sb[i] == sb[oppsite] && dp[i - j] + i - j + 1 == i) {
                    if (sb[i] == sb[oppsite] && dp[i - j] + 1 == j) {
                        dp[i - j]++;
                        if ((i & 1) == 0) {
                            res++;
                        }
                    }
                }
            }
            return res;

        }

        static Dictionary<string, bool> palindromeMap = new();

        public static int CountSubstringsDP(string s) {
            RecursionCountSubstrings(s, 0, s.Length - 1);
            return palindromeMap.Count((a) => a.Value);
        }

        public static bool RecursionCountSubstrings(string s, int left, int right) {
            if (right < left) {
                return false;
            }
            if (palindromeMap.ContainsKey($"{left},{right}")) {
                return palindromeMap[$"{left},{right}"];
            }
            if (right == left) {
                palindromeMap[$"{left},{right}"] = true;
            } else {
                if (left + 1 == right && s[left] == s[right]) {
                    palindromeMap[$"{left},{right}"] = true;
                } else {
                    if (s[left] != s[right]) {
                        palindromeMap[$"{left},{right}"] = false;
                    } else {
                        if (RecursionCountSubstrings(s, left + 1, right - 1)) {
                            palindromeMap[$"{left},{right}"] = true;
                        } else {
                            palindromeMap[$"{left},{right}"] = false;
                        }
                    }
                }
            }
            RecursionCountSubstrings(s, left + 1, right);
            RecursionCountSubstrings(s, left, right - 1);
            return palindromeMap[$"{left},{right}"];
        }

        public static int NumIslands(char[][] grid) {
            int res = 0;
            for (int i = 0; i < grid.Length; i++) {
                for (int j = 0; j < grid[i].Length; j++) {
                    if (grid[i][j] == '1') {
                        Infection(grid, i, j);
                        res++;
                    }
                }
            }
            return res;

            static void Infection(char[][] grid, int x, int y) {
                if (x >= grid.Length || y >= grid[0].Length || x < 0 || y < 0 || grid[x][y] != '1') {
                    return;
                }
                grid[x][y] = '2';
                Infection(grid, x - 1, y);
                Infection(grid, x + 1, y);
                Infection(grid, x, y + 1);
                Infection(grid, x, y - 1);
            }
        }

        public static string Manacher(string s) {
            StringBuilder sb = new("#");
            for (int i = 0; i < s.Length; i++) {
                sb.Append(s[i]);
                sb.Append('#');
            }
            int[] len = new int[sb.Length];//记录半径长
            int mid = 0;
            int r = -1;
            int idx = 0;
            int max = 0;
            for (int i = 0; i < len.Length; i++) {
                len[i] = i >= r ? 1 : Math.Min(r - i + 1, len[2 * mid - i]);//不管怎样都可以扩出一定的距离，在此基础上看看能不能继续暴力扩
                while (i + len[i] < sb.Length && i - len[i] >= 0 && sb[i + len[i]] == sb[i - len[i]]) {
                    len[i]++;
                }
                if (i + len[i] - 1 > r) {
                    r = i + len[i] - 1;
                    mid = i;
                }
                if (len[i] > max) {
                    max = len[i];
                    idx = i;
                }
            }
            return s[((idx - len[idx] + 1) / 2)..((idx + len[idx]) / 2)];
        }

        public static int[] DailyTemperatures(int[] temperatures) {
            Stack<int> st = new();//存放index就可以找到对应的温度
            int[] res = new int[temperatures.Length];
            for (int i = 0; i < temperatures.Length; i++) {
                while (st.Count != 0 && temperatures[st.Peek()] < temperatures[i]) {
                    var tmp = st.Pop();
                    res[tmp] = i - tmp;
                }
                st.Push(i);
            }
            return res;
        }

        public static int Trap(int[] height) {
            Stack<int> st = new();//存放height数组中的索引
            //Dictionary<int, int> finishedHeight = new();
            int doneHeight;//按行装水，已经装了多少行
            int sum = 0;
            for (int i = 0; i < height.Length; i++) {
                doneHeight = 0;//初始化认为底层没装任何水
                while (st.Count > 0 && height[i] >= height[st.Peek()]) {
                    var popped = st.Pop();
                    //装水开始辣
                    int width = i - popped - 1;//只计算中间
                    int h = Math.Min(height[popped], height[i]) - doneHeight;

                    doneHeight = Math.Min(height[popped], height[i]);
                    sum += width * h;
                }
                if (st.Count > 0) {
                    sum += (i - st.Peek() - 1) * (Math.Min(height[i], height[st.Peek()]) - doneHeight);
                }
                st.Push(i);
            }
            return sum;
        }

        public static int LargestRectangleArea(int[] heights) {
            Stack<LinkedList<int>> st = new();
            Dictionary<int, int> dicRight = new();//存放第一个比[key]小的索引
            Dictionary<int, int> dicLeft = new();
            int res = 0;
            for (int i = 0; i < heights.Length; i++) {
                if (st.Count > 0 && heights[st.Peek().First!.Value] == heights[i]) { //相等，可以合并这两个链表
                    var last = st.Pop();
                    last.AddLast(i);
                    if (st.Count > 0) {
                        dicLeft.Add(i, st.Peek().Last!.Value);//计算这一层前面的元素是什么，即左侧较小值
                    }
                    st.Push(last);
                } else {
                    while (st.Count > 0 && heights[st.Peek().First!.Value] > heights[i]) {
                        var popped = st.Pop();
                        foreach (var item in popped) {
                            dicRight.Add(item, i);
                        }
                    }

                    //如果本来就因为相等而合并链表了，下面的就不执行
                    if (st.Count > 0 && heights[st.Peek().Last!.Value] == heights[i]) {//在上面的while循环中弹出了一些元素后，还有可能能继续合并元素
                        var last = st.Pop();
                        last.AddLast(i);
                        if (st.Count > 0) {
                            dicLeft[i] = st.Peek().Last!.Value;
                        }
                        st.Push(last);
                    } else {
                        if (st.Count > 0) {
                            dicLeft.Add(i, st.Peek().Last!.Value);
                        }
                        st.Push(new(new List<int> { i }));
                    }
                }
            }
            while (st.Count > 0) {
                var tmp = st.Pop();
                if (st.Count > 0) {
                    foreach (var item in tmp) {
                        dicLeft[item] = st.Peek().Last!.Value;
                    }
                }
            }
            for (int i = 0; i < heights.Length; i++) {
                int left = -1, right = heights.Length;
                if (dicLeft.ContainsKey(i)) {
                    left = dicLeft[i];
                }
                if (dicRight.ContainsKey(i)) {
                    right = dicRight[i];
                }
                res = Math.Max((right - 1 - left) * heights[i], res);
            }

            return res;
        }

        public static IList<int> PreorderTraversal(TreeNode root) {
            List<int> res = new();
            TreeNode? cur = root;
            TreeNode? rightest;
            while (cur is not null) {
                rightest = cur.left;
                if (rightest is null) {
                    res.Add(cur.val);
                    cur = cur.right;
                    continue;
                }

                while (rightest.right is not null && rightest.right != cur) {
                    rightest = rightest.right;
                }
                if (rightest.right is null) {
                    rightest.right = cur;
                    res.Add(cur.val);
                    cur = cur.left;
                } else if (rightest.right == cur) {
                    rightest.right = null;
                    cur = cur.right;
                }
            }
            return res;
        }

        public static IList<int> PreorderTraversal2(TreeNode root) {
            Stack<(TreeNode, int)> st = new();
            // 0：子树都还没遍历，1：遍历完左子树回来了，2：所有子树全遍历完回来这个节点了
            List<int> res = new();
            if (root is not null) {
                st.Push((root, 0));
            }
            while (st.Count > 0) {
                var tmp = st.Pop();
                if (tmp.Item2 == 0) {
                    st.Push((tmp.Item1, 1));
                    res.Add(tmp.Item1.val);
                    if (tmp.Item1.left is not null) {
                        st.Push((tmp.Item1.left, 0));
                    }

                } else if (tmp.Item2 == 1) {
                    st.Push((tmp.Item1, 2));
                    if (tmp.Item1.right is not null) {
                        st.Push((tmp.Item1.right, 0));
                    }

                } else {
                    // 什么都不用干了

                }
            }
            return res;
        }


        public static int MonotoneIncreasingDigits(int n) {
            string s = n.ToString();
            if (s.Length == 1) {
                return n;
            } else {
                bool flag = false;
                int idx = -1;
                for (int i = 0; i < s.Length - 1; i++) {
                    if (int.Parse(s[i].ToString()) > int.Parse(s[i + 1].ToString())) {
                        flag = true;
                        idx = i;
                        break;
                    }
                }
                if (!flag) {
                    return n;
                }
                while (idx > 0 && s[idx] == s[idx - 1]) { //特殊情况，如果是相同的数字相邻，会导致减一后不单调
                    idx--;
                }
                char[] arr = s.ToCharArray();
                arr[idx] = char.Parse((int.Parse(s[idx].ToString()) - 1).ToString());
                for (int i = idx + 1; i < s.Length; i++) {
                    arr[i] = '9';
                }
                return int.Parse(string.Join("", arr));
            }
        }

        public static int MaxProfit(int[] prices, int fee) {
            //1, 2, 3, 4, 5, 6, 7, 8, 4, 5, 6, 5, 6, 7, 8, 9 //2
            int minPrice = int.MaxValue;
            int res = 0;
            for (int i = 0; i < prices.Length; i++) {
                if (prices[i] + fee < minPrice) {
                    minPrice = prices[i] + fee;//买入价格越低越好
                }
                if (prices[i] > minPrice) {
                    res += prices[i] - minPrice;//买了下来
                    minPrice = prices[i];//保证第二个if不会重复算利润，也适用于在第一个if时检查区间是否能合并/重开一个区间
                }
            }
            return res;
        }

        public static bool IsInterleave(string s1, string s2, string s3) {
            if (s1.Length + s2.Length != s3.Length) {
                return false;
            } else if (s1.Length == 0) {
                return s2 == s3;
            } else if (s2.Length == 0) {
                return s1 == s3;
            }

            bool[,] dp = new bool[s1.Length + 1, s2.Length + 1];
            dp[0, 0] = true;

            dp[0, 1] = s2[0] == s3[0];
            for (int i = 2; i < dp.GetLength(1); i++) { //s1空字符 + s2的前几个字符能否组成s3
                dp[0, i] = dp[0, i - 1] && s3[i - 1] == s2[i - 1];
            }
            dp[1, 0] = s1[0] == s3[0];
            for (int i = 2; i < dp.GetLength(0); i++) {
                dp[i, 0] = dp[i - 1, 0] && s3[i - 1] == s1[i - 1];
            }

            for (int i = 1; i < dp.GetLength(0); i++) {
                for (int j = 1; j < dp.GetLength(1); j++) {
                    //s1拿出i个字符和s2拿出j个字符进行判断
                    if (s3[i + j - 1] == s1[i - 1]) {
                        dp[i, j] = dp[i - 1, j];
                    }
                    if (s3[i + j - 1] == s2[j - 1]) {
                        dp[i, j] |= dp[i, j - 1];
                    }
                }
            }

            return dp[s1.Length, s2.Length];
        }

        public static int NthUglyNumber(int n) {
            int[] history = new int[n + 1];
            history[1] = 1;
            int cur2 = 1, cur3 = 1, cur5 = 1;
            for (int i = 2; i < history.Length; i++) {
                int choosen = Math.Min(history[cur2] * 2, Math.Min(history[cur3] * 3, history[cur5] * 5));

                //判断选择的是哪个指针并维护该指针
                if (history[cur2] * 2 == choosen) {
                    cur2++;//选择了这个指针，就要向后更新一步
                }
                if (history[cur3] * 3 == choosen) {
                    cur3++;
                }
                if (history[cur5] * 5 == choosen) {
                    cur5++;
                }
                history[i] = choosen;
            }
            return history[n];
        }

        public static int FindUnsortedSubarray(int[] nums) {
            int leftMax = nums[0];
            int l = -1, r = -1;
            for (int i = 1; i < nums.Length; i++) {
                if (nums[i] >= leftMax) {
                    leftMax = nums[i];
                } else {
                    if (l == -1) {
                        l = i;
                    }
                    r = i;
                }
            }
            int l2 = -1, r2 = -1;
            int rightMin = nums[^1];
            for (int i = nums.Length - 2; i >= 0; i--) {
                if (nums[i] <= rightMin) {
                    rightMin = nums[i];
                } else {
                    if (r2 == -1) {
                        r2 = i;
                    }
                    l2 = i;
                }
            }
            return Math.Max(r, r2) != -1 ? Math.Max(r, r2) - Math.Min(l, l2) + 1 : 0;
        }

        public static int MaxPoints(int[][] points) {
            int max = 1;
            for (int i = 0; i < points.Length; i++) {
                for (int j = i + 1; j < points.Length; j++) {
                    bool isUndefined = false;
                    decimal slope = 0m;
                    if (points[i][0] - points[j][0] == 0) {
                        isUndefined = true;
                    } else {
                        slope = (1m * points[i][1] - points[j][1]) / (1m * points[i][0] - points[j][0]);
                    }
                    int ans = 2;
                    for (int k = j + 1; k < points.Length; k++) {
                        if (points[i][0] - points[k][0] == 0 && isUndefined) {
                            ans++;
                        } else if (!isUndefined && points[i][0] - points[k][0] != 0 && (1m * points[i][1] - points[k][1]) / (1m * points[i][0] - points[k][0]) == slope) {
                            ans++;
                        }
                    }
                    max = Math.Max(ans, max);
                }
            }
            return max;
        }

        public static bool ValidPalindrome(string s) {
            return RecursionValidPalindrome(s, false, 0, s.Length - 1);
        }

        private static bool RecursionValidPalindrome(string s, bool once, int start, int end) {
            if (s.Length <= 1) {
                return true;
            }
            int cur1 = start, cur2 = end;
            bool flag = once;
            while (cur1 < cur2) {
                if (s[cur1] == s[cur2]) {
                    cur1++;
                    cur2--;
                } else {
                    if (flag) {
                        return false;
                    }
                    //判断动哪个指针
                    if (s[cur1 + 1] == s[cur2] && s[cur1] != s[cur2 - 1]) {
                        cur1++;
                    } else if (s[cur1 + 1] != s[cur2] && s[cur1] == s[cur2 - 1]) {
                        cur2--;
                    } else if (s[cur1 + 1] == s[cur2] && s[cur1] == s[cur2 - 1]) {
                        return RecursionValidPalindrome(s, true, cur1 + 1, cur2) || RecursionValidPalindrome(s, true, cur1, cur2 - 1);
                    }

                    flag = true;
                }
            }
            return true;
        }

        public static int FindTargetSumWays(int[] nums, int target) {
            return RecursionTargetSum(nums, target, 0, new());
        }

        private static int RecursionTargetSum(int[] nums, int target, int startIdx, Dictionary<ValueTuple<int, int>, int> cache) {
            if (startIdx == nums.Length - 1) {
                if (target == 0 && target == nums[startIdx]) {
                    return 2;
                } else if (target == nums[startIdx] || -target == nums[startIdx]) {
                    return 1;
                } else {
                    return 0;
                }
            }
            if (cache.ContainsKey((target, startIdx))) {
                return cache[(target, startIdx)];
            }

            int ways = 0;
            ways += RecursionTargetSum(nums, target + nums[startIdx], startIdx + 1, cache);
            ways += RecursionTargetSum(nums, target - nums[startIdx], startIdx + 1, cache);
            cache[(target, startIdx)] = ways;
            return ways;
        }

        public static int FindMaxForm(string[] strs, int m, int n) {
            ValueTuple<int, int>[] vals = new (int, int)[strs.Length];
            for (int i = 0; i < strs.Length; i++) {
                int zero = 0, one = 0;
                foreach (var item in strs[i]) {
                    if (item == '1') {
                        one++;
                    } else {
                        zero++;
                    }
                }
                vals[i] = (zero, one);
            }
            int[,,] dp = new int[strs.Length, m + 1, n + 1];
            for (int i = vals.Length - 1; i >= 0; i--) {
                for (int j = 0; j <= m; j++) {
                    for (int k = 0; k <= n; k++) {
                        if (i == vals.Length - 1) {
                            dp[i, j, k] = j >= vals[i].Item1 && k >= vals[i].Item2 ? 1 : 0;
                        } else {
                            int case2 = -1;
                            if (j - vals[i].Item1 >= 0 && k - vals[i].Item2 >= 0) {
                                case2 = dp[i + 1, j - vals[i].Item1, k - vals[i].Item2] + 1;
                            }
                            int case1 = dp[i + 1, j, k];
                            dp[i, j, k] = Math.Max(case1, case2);
                        }
                    }
                }
            }
            return dp[0, m, n];
        }

        private static int RecursionFindMaxForm((int, int)[] info, int m, int n, int startIdx) {
            if (startIdx == info.Length - 1 || m < 0 || n < 0) {
                return m >= info[startIdx].Item1 && n >= info[startIdx].Item2 ? 1 : 0;
            }
            int case1 = RecursionFindMaxForm(info, m, n, startIdx + 1);
            int case2 = RecursionFindMaxForm(info, m - info[startIdx].Item1, n - info[startIdx].Item2, startIdx + 1) + 1;
            return Math.Max(case1, case2);
        }

        public static int ClimbStairs(int n) {
            int[] dp = new int[n + 1];
            dp[0] = 1;
            dp[1] = 1;
            for (int i = 3; i <= n; i++) {
                dp[i] = dp[i - 1] + dp[i - 2];
            }
            return dp[n];
        }

        public static bool WordBreak(string s, IList<string> wordDict) {
            bool[] dp = new bool[s.Length + 1];
            dp[^1] = true;
            for (int i = s.Length - 1; i >= 0; i--) {
                for (int j = 0; j < wordDict.Count; j++) {
                    if (i + wordDict[j].Length <= s.Length && wordDict[j] == s[i..(i + wordDict[j].Length)]) {
                        if (dp[i + wordDict[j].Length]) {
                            dp[i] = true;
                            break;
                        }
                    }

                    dp[i] = false;
                }
            }

            return dp[0];
        }

        private static bool RecursionWordBreak(string s, IList<string> dict, int startIdx) {
            if (startIdx == s.Length) {
                return true;
            }
            foreach (var item in dict) {
                if (startIdx + item.Length <= s.Length && item == s[startIdx..(startIdx + item.Length)]) {
                    if (RecursionWordBreak(s, dict, startIdx + item.Length)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static int RobII(int[] nums) {
            //RobI
            if (nums.Length == 1) {
                return nums[0];
            }
            int[] dp = new int[nums.Length];
            dp[0] = nums[0];
            for (int i = 1; i < nums.Length; i++) {
                int case1 = dp[i - 1];
                int case2 = i - 2 >= 0 ? dp[i - 2] + nums[i] : 0;
                dp[i] = Math.Max(case1, case2);
            }
            int[] dp2 = new int[nums.Length];
            dp2[1] = nums[1];
            for (int i = 2; i < nums.Length; i++) {
                int case1 = dp2[i - 1];
                int case2 = i - 2 >= 0 ? dp2[i - 2] + nums[i] : 0;
                dp2[i] = Math.Max(case1, case2);
            }
            //得到了dp[^2]
            return Math.Max(dp[^2], dp2[^1]);
        }

        public static int RecursionRob2(int[] nums, int startIdx) { //[0..startIdx]内最多有多少钱能偷到
            if (startIdx == 0) {
                return nums[0];
            } else if (startIdx < 0) {
                return 0;
            }
            int ans = 0;
            ans = Math.Max(RecursionRob2(nums, startIdx - 1), RecursionRob2(nums, startIdx - 2) + nums[startIdx]);
            return ans;
        }

        public static int RobIII(TreeNode root) {
            var res = RecursionRobIII(root);
            return Math.Max(res.Item1, res.Item2);
        }

        private static (int, int) RecursionRobIII(TreeNode? root) {
            if (root is null) {
                return (0, 0);
            }
            var left = RecursionRobIII(root.left);
            var right = RecursionRobIII(root.right);
            int case1 = root.val + left.Item2 + right.Item2; //偷根节点
            int case2 = Math.Max(left.Item1, left.Item2) + Math.Max(right.Item1, right.Item2);//不偷根节点
            return (case1, case2);//返回 偷和不偷的最大值
        }

        public static int MaxProfitI(int[] prices) {
            int min = prices[0];
            int max = 0;
            int res = 0;
            for (int i = 1; i < prices.Length; i++) {
                if (prices[i] < min) {
                    min = prices[i];
                    max = min;
                } else {
                    max = Math.Max(max, prices[i]);
                    res = Math.Max(res, max - min);
                }
            }
            return res > 0 ? res : 0;
        }

        /// <summary>
        /// 输入为一个int类型的数组，在数组中选择三个切割点，将该数组分为四个子数组（不包含切割点），若这四个子数组的和相等，则返回true, 否则返回false.
        /// </summary>
        /// <param name="nums">输入数组</param>
        /// <returns>能否分成四个和相等的子数组</returns>
        public static bool FourIntervalsEqualSum(int[] nums) {
            int[] prefixSum = new int[nums.Length];
            prefixSum[0] = nums[0];
            Dictionary<int, int> dic = new();
            for (int i = 1; i < nums.Length; i++) {
                prefixSum[i] = nums[i] + prefixSum[i - 1];
                dic[prefixSum[i]] = i;
            }
            //题目：分成四段，假设均正整数
            for (int i = 1; i < nums.Length - 4; i++) {
                int trySum = prefixSum[i - 1];
                int seprator = nums[i];
                int count = 1;
                int sum = trySum;
                while (true) {
                    sum += seprator + trySum;
                    if (dic.ContainsKey(sum) && dic[sum] + 1 < nums.Length) {//能找到这样的分割和 && 切的一刀在数组内部
                        seprator = nums[dic[sum] + 1];
                    } else {
                        break;
                    }
                    count++;
                }
                if (count == 3) {
                    return true;
                }
            }
            return false;
        }

        public static int MinDays(int n) {
            return RecursionMinDays(n, new());
        }

        private static int RecursionMinDays(int n, Dictionary<int, int> cache) {
            if (n <= 1) {
                return n;
            }
            if (cache.ContainsKey(n)) {
                return cache[n];
            }
            int case1 = n % 3 + 1 + RecursionMinDays(n / 3, cache);
            int case2 = n % 2 + 1 + RecursionMinDays(n >> 1, cache);
            cache.Add(n, Math.Min(case1, case2));
            return cache[n];
        }

        public static ListNode? GetKthFromEnd(ListNode? head, int k) {
            if (head is null || head.next is null) {
                return head;
            }
            ListNode cur1 = head;
            ListNode cur2 = head;
            for (int i = 0; i < k - 1; i++) {
                cur2 = cur2.next;
            }
            while (cur2.next is not null) {
                cur1 = cur1.next;
                cur2 = cur2.next;
            }
            return cur1;
        }

        public static ListNode? MergeKLists(ListNode[] lists) {
            if (lists.Length == 0) {
                return null;
            }
            PriorityQueue<ListNode, int> heap = new();
            for (int i = 0; i < lists.Length; i++) {
                if (lists[i] is not null) {
                    heap.Enqueue(lists[i], lists[i].val);
                }
            }
            ListNode res = new(int.MinValue);
            ListNode cur = res;
            while (heap.Count != 0) {
                var tmp = heap.Dequeue();
                cur.next = new(tmp.val);
                if (tmp.next is not null) {
                    heap.Enqueue(tmp.next, tmp.next.val);
                }
                cur = cur.next;
            }
            return res.next;
        }

        public static bool IsRectangleCover(int[][] rectangles) {
            int left = rectangles[0][0];
            int bottom = rectangles[0][1];
            int right = rectangles[0][2];
            int top = rectangles[0][3];
            int sum = 0;
            HashSet<(int, int)> set = new();
            for (int i = 0; i < rectangles.Length; i++) {
                int x1 = rectangles[i][0];
                int y1 = rectangles[i][1];
                int x2 = rectangles[i][2];
                int y2 = rectangles[i][3];
                left = Math.Min(left, x1);
                bottom = Math.Min(bottom, y1);
                right = Math.Max(right, x2);
                top = Math.Max(top, y2);
                sum += (y2 - y1) * (x2 - x1);
                if (!set.Add((x1, y1))) {
                    set.Remove((x1, y1));
                }
                if (!set.Add((x2, y2))) {
                    set.Remove((x2, y2));
                }
                if (!set.Add((x1, y2))) {
                    set.Remove((x1, y2));
                }
                if (!set.Add((x2, y1))) {
                    set.Remove((x2, y1));
                }
            }
            if ((right - left) * (top - bottom) != sum) {
                return false;
            } else if (set.Count != 4
                || !set.Contains((left, bottom)) || !set.Contains((left, top))
                || !set.Contains((right, bottom)) || !set.Contains((right, top))) {
                return false;
            }
            return true;
        }

        public static string CompressString(string S) {
            int i = 0;
            StringBuilder sb = new();
            while (i < S.Length) {
                int j;
                for (j = i; j < S.Length - 1 && S[j] == S[j + 1]; j++) ;
                sb.Append(S[i]);
                sb.Append((j - i + 1).ToString());
                i = j + 1;
            }
            return sb.Length >= S.Length ? S : sb.ToString();
        }

        public static string DecodeString(string s) {
            return RecursionDecode(s, 0).Item1;
        }

        private static (string, int) RecursionDecode(string s, int startIdx) {
            StringBuilder sb = new();
            int count = 0;
            for (int i = startIdx; i < s.Length;) {
                if (s[i] == '[') {
                    var res = RecursionDecode(s, i + 1);
                    sb.Append(Multiply(res.Item1, count));
                    i = res.Item2 + 1;
                    count = 0;
                } else if (s[i] == ']') {
                    return (sb.ToString(), i);
                } else if (!int.TryParse(s[i].ToString(), out int c)) {
                    sb.Append(s[i]);
                    i++;
                } else {
                    count = count * 10 + c;
                    i++;
                }
            }
            return (sb.ToString(), s.Length);

            static string Multiply(string s, int count) {
                StringBuilder sb = new();
                for (int i = 0; i < count; i++) {
                    sb.Append(s);
                }
                return sb.ToString();
            }
        }

        public static int NthSuperUglyNumber(int n, int[] primes) {
            if (n == 1) {
                return 1;
            }
            int[] idxes = new int[primes.Length];
            Array.Fill(idxes, 1);
            int[] history = new int[n + 1];
            history[1] = 1;
            PriorityQueue<int, int> heap = new();//primes数组下标-->>乘积
            for (int i = 0; i < idxes.Length; i++) {
                heap.Enqueue(i, primes[i] * history[idxes[i]]);
            }
            int j = 2;
            while (j < history.Length) {
                var tmp = heap.Dequeue();
                history[j] = primes[tmp] * history[idxes[tmp]];
                idxes[tmp]++;
                if (primes[tmp] * history[idxes[tmp]] > 0) { //答案保证是带符号Int32，所以溢出可以直接舍弃
                    heap.Enqueue(tmp, primes[tmp] * history[idxes[tmp]]);
                }
                if (history[j - 1] == history[j]) {
                    continue;
                }
                j++;
            }
            return history[^1];
        }

        public static IList<IList<int>> KSmallestPairs(int[] nums1, int[] nums2, int k) {
            long count = Math.Min(k, (long)nums1.Length * nums2.Length);
            if (count == 1) {
                return new List<IList<int>>() { new List<int>() { nums1[0] + nums2[0] } };
            }
            PriorityQueue<(int, int), int> heap = new();
            HashSet<(int, int)> set = new() { (0, 0) };
            List<IList<int>> res = new();
            heap.Enqueue((0, 0), nums1[0] + nums2[0]);
            while (res.Count < count) {
                var tmp = heap.Dequeue();
                res.Add(new List<int>() { nums1[tmp.Item1], nums2[tmp.Item2] });
                (int, int) p1 = (tmp.Item1 + 1, tmp.Item2);
                if (p1.Item1 < nums1.Length && set.Add(p1)) {
                    heap.Enqueue(p1, nums1[p1.Item1] + nums2[p1.Item2]);
                }
                (int, int) p2 = (tmp.Item1, tmp.Item2 + 1);
                if (p2.Item2 < nums2.Length && set.Add(p2)) {
                    heap.Enqueue(p2, nums1[p2.Item1] + nums2[p2.Item2]);
                }
            }
            return res;
        }

        public static int FirstMissingPositive(int[] nums) {
            int l = 0; //左区域[0..l)，下标i对应的元素为i + 1
            int r = nums.Length;//垃圾区域[..len)
            while (l < nums.Length && l < r) {
                while (l < nums.Length && nums[l] == l + 1) {
                    l++;
                }
                while (l < r && nums[l] != l + 1) {
                    if (nums[l] > nums.Length || nums[l] < 1 || nums[l] == nums[nums[l] - 1]) { //超过下标，小于1，交换后和原数相等这三种情况
                        Swap(nums, l, --r);//是垃圾数据，不需要
                    } else {
                        Swap(nums, l, nums[l] - 1);
                    }
                }

            }
            //返回不存在的数，即左区域最右侧的下一个元素l，此处缺少正整数l + 1
            return l + 1;

            static void Swap(int[] arr, int i, int j) => (arr[i], arr[j]) = (arr[j], arr[i]);
        }

        public static bool DigitCount(string num) {
            Dictionary<int, int> dic = new();
            for (int i = 0; i < num.Length; i++) {
                var tmp = int.Parse(num[i].ToString());
                if (!dic.ContainsKey(tmp)) {
                    dic[tmp] = 0;
                }
                dic[tmp]++;
            }
            for (int i = 0; i < num.Length; i++) {
                if (dic.ContainsKey(i) && dic[i] == int.Parse(num[i].ToString())) {

                } else if (!dic.ContainsKey(i) && num[i] == '0') {

                } else {
                    return false;
                }
            }
            return true;
        }

        public static int MinOperations(int[] nums, int x) {
            int[] prefixSum = new int[nums.Length];
            prefixSum[0] = nums[0];
            for (int i = 1; i < nums.Length; i++) {
                prefixSum[i] = nums[i] + prefixSum[i - 1];
            }
            Dictionary<int, int> dic = new();
            for (int i = nums.Length - 1; i >= 1; i--) {
                dic.Add(prefixSum[^1] - prefixSum[i - 1], i);
            }
            dic.Add(prefixSum[^1], 0);
            int res = nums.Length + 1;
            for (int i = 0; i < prefixSum.Length; i++) {
                if (dic.ContainsKey(x - prefixSum[i])) {
                    res = Math.Min(res, i + 1 + nums.Length - dic[x - prefixSum[i]]);
                }
                if (prefixSum[i] == x) {
                    res = Math.Min(res, i + 1);
                }
                if (dic.ContainsKey(x)) {
                    res = Math.Min(res, nums.Length - dic[x]);
                }
            }
            return res == nums.Length + 1 ? -1 : res;
        }

        public static int MinOperations2(int[] nums, int x) {
            int l = -1;//定义：[0,l]
            int r = 0;//定义：[r,nums.Length]
            int leftSum = 0;
            int rightSum = nums.Sum();//O(N)
            int res = nums.Length + 1;
            while (l <= r && l < nums.Length - 1 && r <= nums.Length) {
                if (leftSum + rightSum == x) {
                    res = Math.Min(res, l + 1 + nums.Length - r);
                    l++;
                    leftSum += nums[l];
                } else if (leftSum + rightSum > x) {
                    if (r >= nums.Length) {
                        break;
                    }
                    rightSum -= nums[r];
                    r++;
                } else if (leftSum + rightSum < x) {
                    l++;
                    leftSum += nums[l];
                }
            }
            return res == nums.Length + 1 ? -1 : res;
        }

        public static int LongestUnivaluePath(TreeNode root) {
            if (root is null) {
                return 0;
            }
            var res = RecursionUnivalue(root);
            return Math.Max(res.Item1, res.Item2) - 1;
        }

        /// <summary>
        /// 返回以node为根节点的信息
        /// </summary>
        /// <param name="node"></param>
        /// <returns>（包含头节点的高度，不包含头的高度，过头的路径高度）</returns>
        private static (int, int, int) RecursionUnivalue(TreeNode? node) {
            if (node is null) {
                return (0, 0, 0);
            }
            var left = RecursionUnivalue(node.left);
            var right = RecursionUnivalue(node.right);
            int case1 = Math.Max(Math.Max(left.Item1, left.Item2), Math.Max(right.Item1, right.Item2));//不过头
            int case2 = 1;//过头
            int height = 0;
            if (node.left is not null && node.val == node.left.val) {
                case2 += left.Item3;
                height = left.Item3;
            }
            if (node.right is not null && node.val == node.right.val) {
                case2 += right.Item3;
                height = Math.Max(right.Item3, height);
            }
            height++;
            return (case2, case1, height);
        }

        public static string Evaluate(string s, IList<IList<string>> knowledge) {
            Dictionary<string, string> dic = new();
            foreach (var item in knowledge) {
                dic.Add(item[0], item[1]);
            }
            StringBuilder sb = new();
            for (int i = 0; i < s.Length; i++) {
                if (s[i] == '(') {
                    int l = i + 1;
                    while (i + 1 < s.Length && s[i] != ')') {
                        i++;
                    }
                    if (dic.TryGetValue(s[l..i], out string word)) {
                        sb.Append(word);
                    } else {
                        sb.Append('?');
                    }
                } else {
                    sb.Append(s[i]);
                }
            }
            return sb.ToString();
        }

        public static int PathSum(TreeNode? root, int targetSum) {
            if (root is null) {
                return 0;
            }
            var res = RecursionPathSum(root, targetSum);
            return res.Item1 + res.Item2;
        }

        private static (int, int) RecursionPathSum(TreeNode? root, int targetSum) {
            if (root is null) {
                return (0, 0);
            }
            int equal = root.val == targetSum ? 1 : 0;
            var case1 = RecursionPathSum(root.left, targetSum);
            var case2 = RecursionPathSum(root.right, targetSum);

            var case3 = RecursionPathSum(root.left, targetSum - root.val);
            var case4 = RecursionPathSum(root.right, targetSum - root.val);
            return (case3.Item1 + case4.Item1 + equal, case1.Item2 + case1.Item1 + case2.Item2 + case2.Item1);
        }

        public static int RearrangeCharacters(string s, string target) {
            int[] ss = new int[26];
            foreach (var item in s) {
                ss[item - 'a']++;
            }
            int[] t = new int[26];
            foreach (var item in target) {
                t[item - 'a']++;
            }
            int count = int.MaxValue;
            foreach (var item in target) {
                count = Math.Min(count, ss[item - 'a'] / t[item - 'a']);
            }
            return count;
        }

        public static int MaxFrequency(int[] nums, int k) {
            Array.Sort(nums, (a, b) => b.CompareTo(a));
            int[] prefixArr = new int[nums.Length];
            prefixArr[0] = nums[0];
            for (int i = 1; i < nums.Length; i++) {
                prefixArr[i] = nums[i] + prefixArr[i - 1];
            }
            int cur1 = nums.Length - 1;
            int cur2 = cur1 - 1;
            int padding;//= nums[cur2] * 1 - suffixArr[^1];
            int res = 1;
            while (cur2 <= cur1 && (cur2 != 0 || cur1 != 0) && cur2 >= 0) {
                padding = nums[cur2] * (cur1 - cur2) - (prefixArr[cur1] - prefixArr[cur2]);
                if (padding <= k) {
                    res = Math.Max(res, cur1 - cur2 + 1);
                    cur2--;
                    while (cur2 > 0 && nums[cur2] == nums[cur2 - 1]) {
                        cur2--;
                    }
                } else {
                    cur1--;
                }
            }
            return res;
        }

        public static int SubarraySum(int[] nums, int k) {
            Dictionary<int, int> dic = new() { [0] = 1 };
            int res = 0;
            int last = 0;
            for (int i = 0; i < nums.Length; i++) {
                if (dic.ContainsKey(nums[i] + last - k)) {
                    res += dic[nums[i] + last - k];
                }

                if (!dic.ContainsKey(nums[i] + last)) {
                    dic.Add(nums[i] + last, 0);
                }
                dic[nums[i] + last]++;
                last = nums[i] + last;
            }
            return res;
        }

        public static IList<string> FindWords(char[][] board, string[] words) {
            List<string> res = new();
            int[] history = new int[26];
            for (int i = 0; i < board.Length; i++) {
                for (int j = 0; j < board[i].Length; j++) {
                    history[board[i][j] - 'a']++;
                }
            }
            foreach (var item in words) {
                int[] shi = new int[26];
                foreach (var s in item) {
                    shi[s - 'a']++;
                }
                for (int k = 0; k < 26; k++) {
                    if (shi[k] > history[k]) {
                        goto DoNotFind;
                    }
                }
                for (int i = 0; i < board.Length; i++) {
                    for (int j = 0; j < board[i].Length; j++) {
                        if (BacktrackingFindWords(board, item, 0, i, j)) {
                            res.Add(item);
                            goto DoNotFind;
                        }
                    }
                }
            DoNotFind:;

            }
            return res;
        }

        private static bool BacktrackingFindWords(char[][] board, string toFind, int strStartIdx, int x, int y) {
            if (board[x][y] != toFind[strStartIdx]) {
                return false;
            }
            if (strStartIdx == toFind.Length - 1) {
                return true;
            }
            bool case1 = false;
            char tmp;
            if (x + 1 < board.Length) {
                tmp = board[x][y];
                board[x][y] = ',';
                case1 |= BacktrackingFindWords(board, toFind, strStartIdx + 1, x + 1, y);
                board[x][y] = tmp;
            }
            if (!case1 && y + 1 < board[0].Length) {
                tmp = board[x][y];
                board[x][y] = ',';
                case1 |= BacktrackingFindWords(board, toFind, strStartIdx + 1, x, y + 1);
                board[x][y] = tmp;
            }
            if (!case1 && x - 1 >= 0) {
                tmp = board[x][y];
                board[x][y] = ',';
                case1 |= BacktrackingFindWords(board, toFind, strStartIdx + 1, x - 1, y);
                board[x][y] = tmp;
            }
            if (!case1 && y - 1 >= 0) {
                tmp = board[x][y];
                board[x][y] = ',';
                case1 |= BacktrackingFindWords(board, toFind, strStartIdx + 1, x, y - 1);
                board[x][y] = tmp;
            }
            return case1;
        }

        public static IList<string> FindWords2(char[][] board, string[] words) {
            PrefixTree tr = new();
            foreach (var item in words) {
                tr.Add(item);
            }
            List<string> res = new();
            foreach (var item in tr.Children.Values) {
                for (int i = 0; i < board.Length; i++) {
                    for (int j = 0; j < board[0].Length; j++) {
                        BacktrackingTreeFind(board, i, j, item, new(), res, tr);
                    }
                }
            }
            return res;
        }

        private static void BacktrackingTreeFind(char[][] board, int x, int y, PrefixTree.Node node, StringBuilder history, List<string> result, PrefixTree tree) {
            if (node.val != board[x][y] || tree.Count == 0) {
                return;
            }
            if (node.end != 0) {
                history.Append(node.val);
                tree.Remove(history.ToString());
                result.Add(history.ToString());
                history.Remove(history.Length - 1, 1);
                if (node.children.Count == 0) {
                    return;
                }
            }
            int[][] dire = new int[4][];
            dire[0] = new int[] { -1, 0 };
            dire[1] = new int[] { 0, -1 };
            dire[2] = new int[] { 1, 0 };
            dire[3] = new int[] { 0, 1 };
            for (int i = 0; i < dire.Length; i++) {
                int newX = dire[i][0] + x;
                int newY = dire[i][1] + y;
                if (newX >= 0 && newX < board.Length &&
                    newY >= 0 && newY < board[0].Length) {
                    if (node.children.ContainsKey(board[newX][newY])) {
                        history.Append(node.val);
                        var tmp = board[x][y];
                        board[x][y] = ' ';
                        BacktrackingTreeFind(board, newX, newY, node.children[board[newX][newY]], history, result, tree);
                        board[x][y] = tmp;
                        history.Remove(history.Length - 1, 1);
                    }
                }
            }
        }

        public static string LongestCommonPrefix(string[] strs) {
            PrefixTree tr = new();
            foreach (var item in strs) {
                tr.Add(item);
                if (string.IsNullOrEmpty(item)) {
                    return string.Empty;
                }
            }
            var node = tr.root;
            int count = 0;
            while (true) {
                if (node.children.Count == 1 && node.end == 0) {
                    count++;
                    PrefixTree.Node next = null;
                    foreach (var item in node.children) {
                        next = item.Value;
                    }
                    node = next;
                } else {
                    break;
                }
            }
            return strs[0][..count];
        }

        public static int FindMin(int[] nums) {
            if (nums.Length == 1) {
                return nums[0];
            }
            int l = 0, r = nums.Length - 1;
            while (true) {
                if (nums[l] < nums[r]) {
                    return nums[l];
                }
                if (r - l == 1) {
                    return Math.Min(nums[l], nums[r]);
                }
                //l >= r
                int m = (l + r) >> 1;
                if (nums[l] > nums[m]) {
                    r = m;
                    continue;
                }
                if (nums[r] < nums[m]) {
                    l = m;
                }
                //l >= r && l >= m && r <= m
                //l == m == r
                while (l < m) {
                    if (nums[l] == nums[m]) {
                        l++;
                    } else if (nums[l] < nums[m]) {
                        return nums[l];
                    } else if (nums[l] > nums[m]) {
                        r = m;
                        break;
                    }
                }
            }
        }

        public static int FindMaxLength(int[] nums) {
            for (int i = 0; i < nums.Length; i++) {
                nums[i] = nums[i] == 0 ? -1 : 1;
            }
            Dictionary<int, int> dict = new() { [0] = -1, };
            int lastSum = 0;
            int res = 0;
            for (int i = 0; i < nums.Length; i++) {
                lastSum += nums[i];
                if (dict.ContainsKey(lastSum)) {
                    res = Math.Max(res, i - dict[lastSum]);
                }
                dict.TryAdd(lastSum, i);
            }
            return res;
        }

        public static int Search(int[] arr, int target) {
            if (arr.Length <= 2) {
                return Array.IndexOf(arr, target);
            }
            int n = arr.Length;
            int l = 0, r = n - 1;
            while (l + 1 < r) {
                int m = (l + r) >> 1;
                if (arr[l] > arr[m] && arr[l] >= arr[r]) {
                    r = m;
                } else if (arr[m] >= arr[l] && arr[m] > arr[r]) {
                    l = m;
                } else if (arr[l] >= arr[m] && arr[m] > arr[l] || arr[l] > arr[m] && arr[m] >= arr[l]) {
                    return seek(target, 0, n);
                } else {
                    r--;
                }
            }

            // 两次二分
            int cs1 = seek(target, 0, r);
            if (cs1 != -1) {
                return cs1;
            }
            return seek(target, r, n);


            int seek(int val, int l, int r) { // 左闭右开区间
                int m;

                while (l < r) { // < < < >= >= >=
                    m = (l + r) >> 1;
                    if (arr[m] >= val) {
                        r = m;
                    } else {
                        l = m + 1;
                    }
                }
                return l < n && arr[l] == val ? l : -1;
            }
        }

        public static int NumDistinct(string s, string t) {
            if (s.Length == 0 && t.Length == 0) {
                return 1;
            }
            if (s.Length == 0) {
                return 0;
            } else if (t.Length == 0) {
                return 1;
            }
            int[,] dp = new int[s.Length + 1, t.Length + 1];
            dp[1, 1] = s[0] == t[0] ? 1 : 0;
            for (int i = 2; i <= s.Length; i++) {
                dp[i, 1] = dp[i - 1, 1] + (s[i - 1] == t[0] ? 1 : 0);
            }
            for (int i = 2; i <= s.Length; i++) {
                for (int j = 2; j <= t.Length; j++) {
                    if (s[i - 1] == t[j - 1]) {
                        dp[i, j] = dp[i - 1, j] + dp[i - 1, j - 1];
                    } else {
                        dp[i, j] = dp[i - 1, j];
                    }
                }
            }
            return dp[s.Length, t.Length];
        }

        public static Node? CopyRandomList(Node head) {
            if (head is null) {
                return null;
            }
            Node dummy = new(int.MinValue) { next = head };
            Node cur2 = dummy;
            Node? cur1 = head;
            Dictionary<Node, Node> map = new();
            while (cur1 is not null) {
                cur2.next = new(cur1.val);
                cur2 = cur2.next;
                map.Add(cur1, cur2);
                cur1 = cur1.next;
            }
            cur2 = dummy.next;
            cur1 = head;
            while (cur2 is not null) {
                cur2.random = cur1.random is null ? null : map[cur1.random];
                cur2 = cur2.next;
                cur1 = cur1.next;
            }
            return dummy.next;
        }

        public static bool ParseBoolExpr(string expression) {
            Stack<int> timesSt = new();
            Stack<bool> valSt = new();
            Stack<char> opSt = new();
            HashSet<char> opSet = new() { '|', '&', '!' };
            HashSet<char> valSet = new() { 'f', 't' };
            foreach (char c in expression) {
                if (c == '(') {
                    timesSt.Push(1);
                } else if (c == ',') {
                    timesSt.Push(timesSt.Pop() + 1);
                } else if (valSet.Contains(c)) {
                    valSt.Push(c == 't');
                } else if (opSet.Contains(c)) {
                    opSt.Push(c);
                } else if (c == ')') {
                    int times = timesSt.Pop();
                    char op = opSt.Pop();
                    bool res = valSt.Pop();
                    if (op == '!') {
                        res = !res;
                    } else {
                        for (int i = 0; i < times - 1; i++) {
                            if (op == '|') {
                                res = res | valSt.Pop();
                            } else if (op == '&') {
                                res = res & valSt.Pop();
                            }
                        }
                    }
                    valSt.Push(res);
                }
            }
            return valSt.Pop();
        }

        public static int ReachNumber(int target) {
            int n = 0;
            while (calc(n) < target) {
                n++;
            }
            if (calc(n) == target) return n;
            while (((calc(n) - target) & 1) != 0) {
                n++;
            }
            return n;

            int calc(int n) {
                return n * (1 + n) / 2;
            }

        }

        public static IList<string> AddOperators(string num, int target) {
            List<string> res = new();
            BacktrackingAddOpe(num, target, 0, new(), res, num[0] - '0');
            return res;
        }

        private static void BacktrackingAddOpe(string num, int target, int index, StringBuilder path, List<string> result, int firstDigit) {
            path.Append(num[index]);
            if (path.Length >= 2 && !int.TryParse(path[^2].ToString(), out _)) {
                firstDigit = int.Parse(num[index].ToString());//更改前导数
            }
            if (index == num.Length - 1) {
                if (CalcExpression(path.ToString()) == target) {
                    result.Add(path.ToString());
                }
                return;
            }

            List<string> ops = new() { "", "+", "-", "*" };
            string backup = path.ToString();
            for (int i = 0; i < 4; i++) {
                if (i == 0 && firstDigit == 0 && num[index] == '0') {
                    continue;
                }
                path.Append(ops[i]);
                BacktrackingAddOpe(num, target, index + 1, path, result, firstDigit);
                path.Clear();
                path.Append(backup);
            }
        }

        private static long CalcExpression(string num) {
            if (num[0] != '-') {
                num = "+" + num;
            }
            var low = num.Split(new char[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries);
            long ans = 0;
            int idx = 0;
            for (int i = 0; i < num.Length;) {
                bool isPlus = false;
                if (num[i] == '+') {
                    isPlus = true;
                }
                i++;
                long ans2 = 1;
                var high = low[idx].Split('*', StringSplitOptions.RemoveEmptyEntries);
                if (high.Length != 0) {
                    foreach (var item in high) {
                        ans2 *= long.Parse(item);
                    }
                } else {
                    ans2 = long.Parse(low[idx]);
                }
                i += low[idx].Length;
                idx++;
                ans += (isPlus ? 1 : -1) * ans2;
            }
            return ans;

        }

        public static string GreatestLetter(string s) {
            int[] lower = new int[26];
            int[] upper = new int[26];

            char res = (char)0;
            foreach (char c in s) {
                if (char.IsUpper(c)) {
                    upper[c - 'A']++;
                }
                if (char.IsLower(c)) {
                    lower[c - 'a']++;
                }
                if (char.IsUpper(c) && lower[c - 'A'] > 0) {
                    res = (char)Math.Max((int)res, (int)c);
                } else if (char.IsLower(c) && upper[c - 'a'] > 0) {
                    lower[c - 'a']++;
                    res = (char)Math.Max((int)res, (int)(char.ToUpper(c)));
                }
            }
            return res == '\u0000' ? "" : res.ToString();
        }

        public static IList<IList<int>> ZigzagLevelOrder(TreeNode root) {
            if (root is null) {
                return new List<IList<int>>();
            }
            IList<IList<int>> res = new List<IList<int>>();
            Stack<TreeNode> st1 = new();
            Stack<TreeNode> st2 = new();
            st1.Push(root);
            while (st1.Count > 0 || st2.Count > 0) {
                List<int> curLevel = new();
                if (st1.Count > 0) {
                    while (st1.Count > 0) {
                        var tmp = st1.Pop();
                        curLevel.Add(tmp.val);
                        if (tmp.left is not null) {
                            st2.Push(tmp.left);
                        }
                        if (tmp.right is not null) {
                            st2.Push(tmp.right);
                        }
                    }
                } else {
                    while (st2.Count > 0) {
                        var tmp = st2.Pop();
                        curLevel.Add(tmp.val);
                        if (tmp.right is not null) {
                            st1.Push(tmp.right);
                        }
                        if (tmp.left is not null) {
                            st1.Push(tmp.left);
                        }
                    }
                }
                res.Add(curLevel);
            }
            return res;
        }

        public static int MaxArea(int[] height) {
            int cur1 = 0;
            int cur2 = height.Length - 1;
            int res = 0; //Math.Min(height[cur2], height[cur1]) * (cur2 - cur1);
            while (cur2 > cur1) {
                res = Math.Max(Math.Min(height[cur2], height[cur1]) * (cur2 - cur1), res);
                if (height[cur1] <= height[cur2]) {
                    cur1++;
                } else {
                    cur2--;
                }
            }
            return res;
        }

        public static bool IsPathCrossing(string path) {
            HashSet<(int, int)> history = new();
            var p = (0, 0);
            history.Add(p);
            foreach (var item in path) {
                switch (item) {
                    case 'N':
                        p.Item2++;
                        break;
                    case 'S':
                        p.Item2--;
                        break;
                    case 'W':
                        p.Item1--;
                        break;
                    case 'E':
                        p.Item1++;
                        break;
                }
                if (!history.Add(p)) {
                    return true;
                }
            }
            return false;
        }

        public static int MaxPathSum(TreeNode root) {
            var res = RecursionMaxPathSum(root);
            if (res.Item4) {
                return res.Item5;
            } else {
                return Math.Max(res.Item1, res.Item2);
            }
        }

        private static (int, int, int, bool, int) RecursionMaxPathSum(TreeNode? node) {
            //选头，不选，向下延伸的链，全是负数
            if (node is null) {
                return (0, 0, 0, true, int.MinValue);
            }
            var left = RecursionMaxPathSum(node.left);
            var right = RecursionMaxPathSum(node.right);
            //不选头
            int case1 = Math.Max(Math.Max(left.Item1, left.Item2), Math.Max(right.Item1, right.Item2));
            //向下链
            int down = node.val;
            down += Math.Max(Math.Max(left.Item3, 0), right.Item3);
            //选头
            int case2 = down;
            case2 = Math.Max(case2, Math.Max(0, left.Item3) + node.val + Math.Max(0, right.Item3));
            return (case2, case1, down, left.Item4 && right.Item4 && node.val < 0, Math.Max(node.val, Math.Max(right.Item5, left.Item5)));
        }

        public static IList<string>? WordBreakII(string s, IList<string> wordDict) {
            PrefixTree tr = new();
            foreach (var item in wordDict) {
                tr.Add(item);
            }
            PrefixTree.Node root = tr.root;
            List<string>? res;
            mapWordBreak = new();
            res = RecursionWordBreakII(s, 0, root);

            return res;
        }

        static Dictionary<int, List<string>> mapWordBreak;

        private static List<string>? RecursionWordBreakII(string s, int startIdx, PrefixTree.Node root) {
            if (mapWordBreak.ContainsKey(startIdx)) {
                return mapWordBreak[startIdx];
            }
            PrefixTree.Node cur = root;
            List<string> res = new();
            if (startIdx >= s.Length) {
                return null;
            }
            for (int i = startIdx; i < s.Length; i++) {
                if (cur.children.ContainsKey(s[i])) {
                    cur = cur.children[s[i]];
                    if (cur.end > 0) {
                        //s[startIdx..i]满足条件
                        var tmp = RecursionWordBreakII(s, i + 1, root);
                        if (tmp is not null) {
                            foreach (var item in tmp) {
                                res.Add(s[startIdx..(i + 1)] + " " + item);
                            }
                        } else {
                            res.Add(s[startIdx..(i + 1)]);
                        }
                    }
                } else {
                    break;
                }
            }
            mapWordBreak.Add(startIdx, res);
            return res;
        }

        public static int[] CountDistanceToCapital(int[] path) {
            //先转换成：path[i]代表城市i到首都的距离是多少
            int cur, next, prev;
            for (int i = 0; i < path.Length; i++) {
                if (path[i] == i) {
                    path[i] = 0;
                } else if (path[i] > 0) {
                    prev = cur = i;
                    next = path[cur];
                    while (next >= 0 && path[next] != next) {
                        path[cur] = prev;
                        prev = cur;
                        cur = next;
                        next = path[next];
                    }
                    if (next >= 0) { //找到了首都
                        path[cur] = -1;
                    } else { //找到了其他通往首都的路径
                        int tmp = path[prev];
                        path[prev] = path[cur] - 1;//先更改离首都有一格远的节点 / 离已知路径节点有一格远的路径距离
                        next = cur;
                        cur = prev;
                        prev = tmp;
                    }

                    if (prev != cur) { //如果有上一级
                        next = cur;
                        cur = prev;
                        while (path[cur] >= 0) { //如果当前记录不是距离记录而是“上一节点”的记录，就继续回退
                            prev = path[cur];//记录了上一个节点
                            path[cur] = path[next] - 1;
                            next = cur;
                            cur = prev;//当回退到不能再回退的时候prev和cur相等，当前记录的意义就是距离
                        }
                    }
                }
            }

            path = new[] { -2, -4, 0, -2, -3, -4, -4, -2, -3, -1 };
            //path: 每个城市距离首都的距离是多少  =>  距离为i的城市有几座
            for (int i = 0; i < path.Length; i++) {
                if (path[i] > 0) {                  //逻辑：被覆盖的数据不能丢失，如果该数据没有被统计，应该紧接着统计该处的数据
                    continue;                       //如果被覆盖的数据已经统计过了，那么被覆盖掉对结果是不可能有影响的
                }                                   //如何标记即将被覆盖的数据是否统计过：在统计完一个数据贡献的词频后，
                int cur1 = i;                       //将该数据替换为0。数据被替换后，其意义变成了i的词频，而词频设置为0
                int cur2 = -path[cur1];             //是符合词频的，下标i从来没有出现过，那它的词频就是0，和我们人为设置成0
                while (true) {                      //是不矛盾的。
                    if (path[cur2] >= 0) {          //如果要被覆盖的数据还没有利用过，直接覆盖是不可以的，因为这样会导致
                        path[cur2]++;               //的丢失，就不能原地实现了。所以要在丢失一个数据后抓紧处理这个丢失的数据
                        break;                      //如果要等所有数据都处理了一遍那么原数组就全乱套了，原有的数据都会丢失。
                    }
                    int tmp = -path[cur2];
                    path[cur2] = 1;
                    cur1 = cur2;
                    cur2 = tmp;
                }
                path[i] = path[i] > 0 ? path[i] : 0;
            }
            path[0] = 1;
            return path;
        }

        public static bool EvaluateTree(TreeNode root) {
            if (root.val == 1 || root.val == 0) {
                return root.val == 1;
            }
            if (root.val == 2) {
                return EvaluateTree(root.left!) || EvaluateTree(root.right!);
            } else {
                return EvaluateTree(root.left!) && EvaluateTree(root.right!);
            }
        }

        public static int ShortestSubarray(int[] nums, int k) {
            int[] prefix = new int[nums.Length + 1];
            prefix[0] = 0;
            for (int i = 1; i <= nums.Length; i++) {
                prefix[i] = prefix[i - 1] + nums[i - 1];
            }
            // 单调队列维护子数组窗口
            Deque<int> que = new();
            que.PushBack(0);
            int res = int.MaxValue;
            for (int i = 0; i < prefix.Length; i++) {
                while (que.Count > 0 && prefix[que.PeekBack()] > prefix[i]) {
                    que.PopBack();
                }
                que.PushBack(i);
                while (prefix[i] - prefix[que.PeekFront()] >= k) {
                    res = Math.Min(res, i - que.PeekFront());
                    que.PopFront();
                }
            }
            return res == int.MaxValue ? -1 : res;
        }

        public static bool IsBoomerang(int[][] points) {
            if (points[0][0] == points[1][0] && points[0][1] == points[1][1] ||
                points[2][0] == points[1][0] && points[2][1] == points[1][1] ||
                points[1][0] == points[2][0] && points[1][1] == points[2][1]) {
                return false;
            }
            double k = (points[0][1] - points[1][1] * 1d) / (points[0][0] - points[1][0]);
            if ((points[2][1] - points[1][1] * 1d) / (points[2][0] - points[1][0]) == k ||
                (points[0][0] == points[1][0] && points[2][0] == points[1][0])) {
                return false;
            }
            return true;
        }

        public static int RollTheDice(int n, int[] rollMax, int lastNum, int times) {
            if (n == 1) {
                if (lastNum == -1) {
                    return 6;
                }
                return rollMax[lastNum - 1] - times > 0 ? 6 : 5;
            }

            int count = 0;
            for (int i = 1; i <= 6; i++) {
                if (i == lastNum) {
                    if (rollMax[i - 1] - times > 0) {
                        count += RollTheDice(n - 1, rollMax, i, times + 1);
                    }
                } else {
                    count += RollTheDice(n - 1, rollMax, i, 1);
                }
            }
            return count;
        }

        public static int DieSimulator(int n, int[] rollMax) {
            const int mod = 1000000007;
            int rollMaxMax = -1;
            for (int i = 0; i < rollMax.Length; i++) {
                rollMax[i] = Math.Min(rollMax[i], n);
                rollMaxMax = Math.Max(rollMaxMax, rollMax[i]);
            }
            int[,,] dp = new int[n + 1, 6, rollMaxMax + 1];//n    lastNum     times

            for (int i = 1; i <= 6; i++) {
                for (int j = 1; j <= rollMaxMax; j++) {
                    dp[1, i - 1, j] = rollMax[i - 1] - j > 0 ? 6 : 5;
                }
            }

            for (int i = 2; i <= n; i++) { //n
                for (int k = 1; k <= 6; k++) { //lastNum
                    for (int m = rollMaxMax; m >= 0; m--) { //times
                        for (int j = 1; j <= 6; j++) {
                            if (j == k) {
                                if (rollMax[j - 1] - m > 0) {
                                    dp[i, k - 1, m] = (dp[i, k - 1, m] + dp[i - 1, j - 1, m + 1]) % mod;// RollTheDice(n - 1, rollMax, j, i + 1);
                                }
                            } else {
                                dp[i, k - 1, m] = (dp[i, k - 1, m] + dp[i - 1, j - 1, 1]) % mod; //RollTheDice(n - 1, rollMax, m, 1);
                            }
                        }
                    }
                }
            }

            return dp[n, 1, 0];
        }

        public static int FindLengthOfLCIS(int[] nums) {
            int res = 0;
            int count = 0;
            for (int i = 1; i < nums.Length; i++) {
                if (nums[i] > nums[i - 1]) {
                    count++;
                } else {
                    res = Math.Max(res, count);
                    count = 0;
                }
            }
            return Math.Max(count, res);
        }

        public static int FindLength(int[] nums1, int[] nums2) {
            int[,] dp = new int[nums1.Length + 1, nums2.Length + 1];
            int res = 0;
            for (int i = 1; i <= nums2.Length; i++) {
                dp[1, i] = nums1[0] == nums2[i - 1] ? 1 : 0;
                res = Math.Max(res, dp[1, i]);
            }
            for (int i = 2; i <= nums1.Length; i++) {
                dp[i, 1] = nums1[i - 1] == nums2[0] ? 1 : 0;
                res = Math.Max(res, dp[i, 1]);
            }

            for (int i = 2; i <= nums1.Length; i++) {
                for (int j = 2; j <= nums2.Length; j++) {
                    dp[i, j] = nums1[i - 1] == nums2[j - 1] ? dp[i - 1, j - 1] + 1 : 0;
                    res = Math.Max(res, dp[i, j]);
                }
            }
            return res;
        }

        public static int FillCups(int[] amount) {
            int count = 0;
            Array.Sort(amount);
            if (amount[^1] != amount[^2]) {
                count += amount[1] - amount[0];
                amount[^1] -= count;
                amount[^2] -= count;
                if (2 * amount[0] < amount[^1]) { //如果不能靠前面的减完
                    count += 2 * amount[0];
                    amount[^1] -= 2 * amount[0];
                    count += amount[^1];
                } else { //如果能靠前面的减完    如果前面的数值大于等于后面
                    amount[0] -= amount[^1] / 2;
                    amount[1] -= (amount[^1] - amount[^1] / 2);
                    count += amount[^1];
                    amount[^1] = 0;
                    if ((amount[^1] & 1) == 0) {
                        count += amount[0];
                    } else {
                        count += amount[0] + 1;
                    }
                }
            } else {
                count += amount[1] - amount[0];
                count += (int)Math.Ceiling(1.5 * amount[0]);
            }
            return count;
        }

        public static int MinDistance(string word1, string word2) {
            int[,] dp = new int[word1.Length + 1, word2.Length + 1];
            for (int i = 1; i <= word1.Length; i++) {
                dp[i, 0] = i;
            }
            for (int i = 1; i <= word2.Length; i++) {
                dp[0, i] = i;
            }
            for (int i = 1; i <= word1.Length; i++) {
                for (int j = 1; j <= word2.Length; j++) {
                    if (word1[i - 1] == word2[j - 1]) {
                        dp[i, j] = dp[i - 1, j - 1];
                    } else {
                        //delete
                        int del = dp[i - 1, j] + 1;
                        //replace
                        int rpl = dp[i - 1, j - 1] + 1;
                        //insert
                        int ist = Math.Min(dp[i - 1, j], dp[i, j - 1]) + 1;
                        dp[i, j] = Math.Min(ist, Math.Min(del, rpl));
                    }
                }
            }
            return dp[word1.Length, word2.Length];
        }

        public static string AlphabetBoardPath(string target) {
            Dictionary<char, (int, int)> map = new();
            for (int i = 'a'; i <= 'z'; i++) { //生成board表
                map.Add((char)i, ((i - 'a') / 5, (i - 'a') % 5));
            }
            char from = 'a';
            StringBuilder sb = new();
            for (int i = 0; i < target.Length; i++) {
                sb.Append(GeneratePath(map[from], map[target[i]]));
                from = target[i];
            }
            return sb.ToString();

            static StringBuilder GeneratePath((int, int) from, (int, int) to) {
                StringBuilder sb = new();
                if (from == to) {
                    sb.Append('!');
                    return sb;
                }
                if (to.Item2 < from.Item2) {
                    sb.Append(new string('L', from.Item2 - to.Item2));
                }
                if (from.Item1 <= to.Item1) {
                    sb.Append(new string('D', to.Item1 - from.Item1));
                }
                if (to.Item1 < from.Item1) {
                    sb.Append(new string('U', from.Item1 - to.Item1));
                }
                if (from.Item2 <= to.Item2) {
                    sb.Append(new string('R', to.Item2 - from.Item2));
                }

                sb.Append('!');
                return sb;
            }
        }

        public static int BalancedString(string s) {
            int l = 0, r = -1;
            Dictionary<char, int> map = new() { //最多4个字符
                ['Q'] = 0,
                ['W'] = 0,
                ['E'] = 0,
                ['R'] = 0,
            };
            for (int i = 0; i < s.Length; i++) {
                map[s[i]]++;
            }
            Dictionary<char, int> mapWindow = new() {
                ['Q'] = 0,
                ['W'] = 0,
                ['E'] = 0,
                ['R'] = 0,
            };
            int average = s.Length / 4;
            int res = s.Length;
            while (l < s.Length && r < s.Length) {
                if (map['Q'] - mapWindow['Q'] > average ||
                    map['W'] - mapWindow['W'] > average ||
                    map['E'] - mapWindow['E'] > average ||
                    map['R'] - mapWindow['R'] > average) { //不能

                    r++;
                    if (r == s.Length) {
                        break;
                    } else {
                        mapWindow[s[r]]++;
                    }

                } else {
                    res = Math.Min(res, r - l + 1);
                    mapWindow[s[l]]--;
                    l++;
                }
            }
            return res;
        }

        public static TreeNode SortedArrayToBST(int[] nums) {
            return RecursionGenerateBST(nums, 0, nums.Length - 1);
        }

        private static TreeNode RecursionGenerateBST(int[] nums, int l, int r) {
            if (l == r) {
                return new(nums[l]);
            } else if (l + 1 == r) {
                return new(nums[l]) { right = new(nums[r]) };
            }
            int m = (r + l) / 2;
            TreeNode tree = new(nums[m]);
            tree.left = RecursionGenerateBST(nums, l, m - 1);
            tree.right = RecursionGenerateBST(nums, m + 1, r);
            return tree;
        }

        public static int LongestWPI(int[] hours) {
            int[] prefix = new int[hours.Length + 1];
            for (int i = 1; i <= hours.Length; i++) {
                prefix[i] = prefix[i - 1] + (hours[i - 1] > 8 ? 1 : -1);
            }
            Stack<int> st = new(); // 递减栈，大于的数据一定没有用
            st.Push(0);
            for (int i = 1; i < prefix.Length; i++) {
                if (st.Count > 0 && prefix[st.Peek()] > prefix[i]) {
                    st.Push(i);
                }
            }
            int res = 0;
            for (int i = prefix.Length - 1; i >= 0; i--) {
                if (st.Count > 0 && prefix[i] <= prefix[st.Peek()]) {
                    continue;
                }
                while (st.Count > 0 && prefix[i] > prefix[st.Peek()]) {
                    var tmp = st.Pop();
                    res = Math.Max(res, i - tmp);
                }
            }
            return res;
        }

        public static int MaxSumSubmatrix(int[][] matrix, int k) {
            int[,] prefix = new int[matrix.Length, matrix[0].Length];
            for (int i = 0; i < matrix.Length; i++) {
                prefix[i, 0] = matrix[i][0];
                for (int j = 1; j < matrix[0].Length; j++) {
                    prefix[i, j] = prefix[i, j - 1] + matrix[i][j];
                }
            }
            int res = int.MinValue;
            for (int i = 0; i < matrix[0].Length; i++) { //枚举两个列包括的所有组合
                for (int j = i; j < matrix[0].Length; j++) {
                    SortedSet<int> prefix2 = new() { 0 };
                    int curSum = 0;
                    for (int r = 0; r < matrix.Length; r++) {
                        curSum += prefix[r, j] - (i == 0 ? 0 : prefix[r, i - 1]);//将一块作为Sum
                        var tmp = prefix2.GetViewBetween(curSum - k, int.MaxValue).FirstOrDefault(int.MaxValue);
                        if (tmp != int.MaxValue) {
                            res = Math.Max(res, curSum - tmp);
                        }
                        prefix2.Add(curSum);
                    }
                }
            }
            return res;
        }

        public static void NextPermutation(int[] nums) {
            int cur1 = nums.Length - 1;
            while (cur1 > 0 && nums[cur1] <= nums[cur1 - 1]) { //寻找最后一个递增段
                cur1--;
            }
            if (cur1 == 0) {
                Array.Reverse(nums);
                return;
            }
            //cur1--;
            //确保了[cur..]的区间均为递减段
            Array.Reverse(nums, cur1, nums.Length - cur1);
            cur1--;
            for (int i = cur1; i < nums.Length; i++) {
                if (nums[i] > nums[cur1]) {
                    (nums[i], nums[cur1]) = (nums[cur1], nums[i]);
                    break;
                }
            }
        }

        public static int FindSubstringInWraproundString(string s) {
            int[] dp = new int[26];
            int[] dp2 = new int[s.Length];
            dp[s[^1] - 'a'] = 1;
            dp2[^1] = 1;
            for (int i = s.Length - 2; i >= 0; i--) {
                if (s[i + 1] - s[i] == 1 || s[i + 1] - s[i] == -25) {
                    dp2[i] += 1 + dp2[i + 1];
                } else {
                    dp2[i] += 1;
                }
                int tmp = 1;
                if (s[i + 1] - s[i] == 1 || s[i + 1] - s[i] == -25) {
                    tmp += dp2[i + 1];
                }
                dp[s[i] - 'a'] = Math.Max(dp[s[i] - 'a'], tmp);
            }
            return dp.Sum();
        }

        public static int DistinctSubseqII(string s) {
            int[] dp = new int[s.Length];
            int[] alphabet = new int[26];
            const int MOD = 1000000007;
            dp[0] = 1;
            alphabet[s[0] - 'a'] = 1;
            int sum = 1;
            for (int i = 1; i < s.Length; i++) {
                dp[i] = (dp[i] + sum + 1) % MOD; //1是不要前面的子序列，只要自己这一个字母所形成的单字符子序列
                dp[i] = (dp[i] - alphabet[s[i] - 'a'] + MOD) % MOD;
                //for (int j = 0; j < i; j++) { 可以省去这部分枚举过程，直接使用数组当哈希表
                //    if (s[i] != s[j]) { //为了避免重复，只对不可能出现重复的项进行累加 = 对所有项进行累加 - 和当前字符相同的子序列数
                //        dp[i] = (dp[i] + dp[j]) % MOD;
                //    }
                //}
                alphabet[s[i] - 'a'] = (alphabet[s[i] - 'a'] + dp[i]) % MOD;
                sum = (sum + dp[i]) % MOD;
            }
            int res = 0;
            for (int i = 0; i < dp.Length; i++) {
                res = (res + dp[i]) % MOD;
            }
            return res;
        }

        public static TreeNode RecoverFromPreorder(string traversal) {
            int i = 0;
            while (i < traversal.Length && char.IsDigit(traversal[i])) {
                i++;
            }
            TreeNode root = new(int.Parse(traversal[..i]));
            RecursionRecoverPreorder(traversal, i, root, 1);
            return root;
        }

        private static int RecursionRecoverPreorder(string traversal, int startIdx, TreeNode root, int level) { //接受从横线开始 根左右 返回：整理完左子树后到达的string下标
            int i = startIdx;
            int curLv = 0;
            while (i < traversal.Length && traversal[i] == '-') {
                i++;
                curLv++;
            }
            if (curLv != level) {
                return startIdx; //不是本层能处理的，递归结束出栈让上一层去处理
            }
            int left = 0;
            while (i < traversal.Length && char.IsDigit(traversal[i])) {
                left *= 10;
                left += traversal[i] - '0';
                i++;
            }
            root.left = new(left);//一定有左孩子，因为开头已经判断了有几条横线，即在第几层
            i = RecursionRecoverPreorder(traversal, i, root.left, level + 1);
            curLv = 0;
            while (i < traversal.Length && traversal[i] == '-') { //查看是否有右孩子
                i++;
                curLv++;
            }
            if (curLv < level) {
                return i - curLv;
            } else if (curLv == level) {
                //有右孩子
                int right = 0;
                while (i < traversal.Length && char.IsDigit(traversal[i])) {
                    right *= 10;
                    right += traversal[i] - '0';
                    i++;
                }
                root.right = new(right);
                i = RecursionRecoverPreorder(traversal, i, root.right, level + 1);
            }

            return i;
        }

        public static IList<string> GenerateParenthesis(int n) {
            return BacktrackingParenthesis((0, (n << 1) - 1));
        }

        private static List<string> BacktrackingParenthesis((int, int) interval) {
            if (interval.Item2 - interval.Item1 == 1) {
                return new() { "()" };
            } else if (interval.Item2 - interval.Item1 < 1) {
                return new() { "" };
            }
            List<string> res = new();
            for (int i = interval.Item2; i >= interval.Item1; i -= 2) {
                var part1 = BacktrackingParenthesis((interval.Item1 + 1, i - 1));
                var part2 = BacktrackingParenthesis((i + 1, interval.Item2));
                //卡氏积
                for (int m = 0; m < part1.Count; m++) {
                    for (int n = 0; n < part2.Count; n++) {
                        res.Add("(" + part1[m] + ")" + part2[n]);
                    }
                }
            }
            return res;
        }

        public static void RecoverTree(TreeNode root) {
            List<int> data = new();
            PreorderRecoverTree(root, data);
            int first = -1, second = -1;
            for (int i = 1; i < data.Count; i++) {
                if (data[i] < data[i - 1]) {
                    if (first == -1) {
                        first = i - 1;
                        second = i;
                    } else {
                        second = i;
                    }
                }
            }
            (data[first], data[second]) = (data[second], data[first]);
            PreorderRecover(root, new(data));
        }

        private static void PreorderRecoverTree(TreeNode? root, List<int> res) {
            if (root is null) { return; }
            PreorderRecoverTree(root.left, res);
            res.Add(root.val);
            PreorderRecoverTree(root.right, res);
        }

        private static void PreorderRecover(TreeNode? root, Queue<int> data) {
            if (root is null) {
                return;
            }
            PreorderRecover(root.left, data);
            root.val = data.Dequeue();
            PreorderRecover(root.right, data);
        }

        public static TreeNode? DeleteNode(TreeNode root, int key) {
            if (root is null) {
                return null;
            }
            TreeNode dummy = new(int.MaxValue) {
                left = root,
            };
            TreeNode cur = dummy;
            TreeNode check = dummy;
            while (check is not null && check.val != key) {
                if (check.val < key) {
                    check = check.right;
                } else {
                    check = check.left;
                }
            }
            if (check is null) {
                return root;
            }
            //if (cur.left is null && cur.right is null) {
            //    return null;
            //}

            while (true) {
                if (cur!.val < key) {
                    if (cur.right!.val == key) {
                        //找出cur为父了
                        break;
                    } else {
                        cur = cur.right;
                    }
                } else {
                    if (cur.left!.val == key) {
                        //找出cur为父了
                        break;
                    } else {
                        cur = cur.left;
                    }
                }
            }
            TreeNode par = cur;
            cur = key > par.val ? par.right! : par.left!;
            if (cur.left is null && cur.right is null) {
                bool isLeft = key < par.val;
                if (isLeft) {
                    par.left = null;
                } else {
                    par.right = null;
                }
            } else if (cur.right is not null) {
                TreeNode mostLeft = cur.right;
                TreeNode mostLeftParent;

                while (mostLeft.left is not null) {
                    mostLeft = mostLeft.left;
                }
                bool isLeft;
                if (cur.right == mostLeft) {
                    mostLeftParent = cur;
                    isLeft = false;
                } else {
                    mostLeftParent = cur.right;
                    while (mostLeftParent.left != mostLeft) {
                        mostLeftParent = mostLeftParent.left!;
                    }
                    isLeft = true;
                }

                (cur.val, mostLeft.val) = (mostLeft.val, cur.val);
                TreeNode? tmp = mostLeft.right;
                if (isLeft) {
                    mostLeftParent.left = tmp;
                } else {
                    mostLeftParent.right = tmp;
                }
            } else if (cur.left is not null) {
                TreeNode mostRight = cur.left;
                TreeNode mostRightParent;

                while (mostRight.right is not null) {
                    mostRight = mostRight.right;
                }
                bool isRight;
                if (cur.left == mostRight) {
                    mostRightParent = cur;
                    isRight = false;
                } else {
                    mostRightParent = cur.left;
                    while (mostRightParent.right != mostRight) {
                        mostRightParent = mostRightParent.right!;
                    }
                    isRight = true;
                }

                (cur.val, mostRight.val) = (mostRight.val, cur.val);
                TreeNode? tmp = mostRight.left;
                if (isRight) {
                    mostRightParent.right = tmp;
                } else {
                    mostRightParent.left = tmp;
                }
            }
            return dummy.left;
        }

        public static IList<int> PreorderTraversalNonRecursion(TreeNode root) {
            if (root is null) {
                return Array.Empty<int>();
            }
            List<int> res = new();
            Stack<(TreeNode, int)> st = new();//左树是否被遍历完，0=左树未完，1=左树遍历完，2=右树遍历完
            st.Push((root, 0));
            while (st.Count > 0) {
                var cur = st.Pop();
                if (cur.Item2 == 0) {
                    st.Push(new(cur.Item1, 1));
                    if (cur.Item1.left is not null) {
                        st.Push((cur.Item1.left, 0));
                    }
                    res.Add(cur.Item1.val);
                } else if (cur.Item2 == 1) {
                    st.Push(new(cur.Item1, 2));
                    if (cur.Item1.right is not null) {
                        st.Push((cur.Item1.right, 0));
                    }
                } else {

                }
            }
            return res;
        }

        public static bool CanCross(int[] stones) {
            if (stones[1] != 1) {
                return false;
            }
            return RecursionCanCross(stones, 1, 1, new());
        }

        private static bool RecursionCanCross(int[] stones, int startIdx, int lastStep, Dictionary<(int, int), bool> map) {
            if (startIdx >= stones.Length - 1) {
                return true;
            }

            if (map.ContainsKey((startIdx, lastStep))) {
                return map[(startIdx, lastStep)];
            }
            bool res = false;
            for (int curCase = lastStep - 1; curCase < lastStep + 2; curCase++) {
                //跳case1步能跳到哪
                int target = stones[startIdx] + curCase;
                int nextIdx = -1;
                for (int i = startIdx + 1; i < stones.Length; i++) {
                    if (stones[i] == target) {
                        nextIdx = i;
                    } else if (stones[i] > target) {
                        break;
                    }
                }

                if (nextIdx != -1) {
                    if (res) {
                        break;
                    }
                    res |= RecursionCanCross(stones, nextIdx, curCase, map);
                }
            }
            map[(startIdx, lastStep)] = res;
            return res;

        }

        public static int MinMeetingRooms(int[][] intervals) {
            Array.Sort(intervals, (a, b) => a[1].CompareTo(b[1]));//NlogN
            PriorityQueue<int[], int> que = new();
            for (int i = 0; i < intervals.Length; i++) {
                que.Enqueue(intervals[i], intervals[i][0]);
            }
            int res = 1;
            int j = 0;
            while (que.Count > 0 && j < intervals.Length) { //N^2 logN
                int right = intervals[j][1];
                int count = 0;//重叠的数量
                List<int[]> popped = new();
                while (que.Count > 0 && que.Peek()[0] < right) {
                    count++;
                    if (!que.Peek().SequenceEqual(intervals[j])) {
                        popped.Add(que.Dequeue());
                    } else {
                        que.Dequeue();
                    }
                }
                res = Math.Max(res, count);
                j++;
                for (int i = 0; i < popped.Count; i++) {
                    que.Enqueue(popped[i], popped[i][0]);
                }
            }
            return res;
        }

        public static int MinMeetingRooms2(int[][] intervals) {
            PriorityQueue<int[], int> rooms = new();
            //能来就来，不能来就再开一场房间
            Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0]));  //只有按照开始时间排序，按照结 
            rooms.Enqueue(intervals[0], intervals[0][1]);           //束时间最早的从队列中弹出才会保证两个会议间隔最短
            for (int i = 1; i < intervals.Length; i++) {
                var tmp = rooms.Peek();
                if (intervals[i][0] < tmp[1]) {
                    rooms.Enqueue(intervals[i], intervals[i][1]);
                } else {
                    rooms.Dequeue();
                    tmp[1] = intervals[i][1];
                    rooms.Enqueue(tmp, tmp[1]);
                }
            }
            return rooms.Count;
        }

        public static int MinMeetingRooms3(int[][] intervals) {
            Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0]));
            int max = 1;

            var heap = new PriorityQueue<int, int>();
            for (int i = 0; i < intervals.Length; i++) {
                int[] interval = intervals[i];
                while (heap.Count > 0 && heap.Peek() <= interval[0]) { // 弹出不重叠的
                    heap.Dequeue();
                }
                heap.Enqueue(interval[1], interval[1]);
                max = Math.Max(max, heap.Count);
            }

            return max;
        }

        public static int CountSubarrays(int[] nums, int k) {
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] == k) {
                    nums[i] = 0;
                } else if (nums[i] < k) {
                    nums[i] = -1;
                } else {
                    nums[i] = 1;
                }
            }

            Dictionary<(bool, int), int> prefix = new() { [(false, 0)] = 1, }; //是否是奇数坐标，sum
            int sum = 0;
            int res = 0;
            for (int i = 0; i < nums.Length; i++) {
                sum += nums[i];
                if ((i & 1) != 0) { //寻找奇数长度和为0，偶数长度和为1的子数组
                    if (prefix.ContainsKey((true, sum))) {
                        res += prefix[(true, sum)];
                    }
                    if (prefix.ContainsKey((false, sum - 1))) {
                        res += prefix[(false, sum - 1)];
                    }
                } else {
                    if (prefix.ContainsKey((false, sum))) {
                        res += prefix[(false, sum)];
                    }
                    if (prefix.ContainsKey((true, sum - 1))) {
                        res += prefix[(true, sum - 1)];
                    }
                }
                if (!prefix.TryAdd(((i & 1) == 0, sum), 1)) {
                    prefix[((i & 1) == 0, sum)]++;
                }
            }
            return res;
        }

        public static int FindMinMoves(int[] machines) {
            long sum = machines.Sum((a) => (long)a);
            if (sum % machines.Length != 0) {
                return -1;
            }
            int average = (int)sum / machines.Length;
            long left = 0;
            long right = sum - machines[0];
            long res = Math.Abs(right - average * (machines.Length - 1));
            for (int i = 1; i < machines.Length; i++) {
                left += machines[i - 1];
                right -= machines[i];
                long l = left - i * average;
                long r = right - (machines.Length - i - 1) * average;
                if (l > 0 && r > 0) {
                    res = Math.Max(res, Math.Max(l, r));
                } else if (l * r < 0) {
                    res = Math.Max(res, Math.Max(Math.Abs(l), Math.Abs(r)));
                } else {
                    res = Math.Max(res, -l - r);
                }
            }
            return (int)res;
        }

        public static IList<string> RemoveInvalidParentheses(string s) {
            List<string> ans = new();
            RecursionRemoveParenth(new(s), 0, 0, ans, new char[] { '(', ')' });
            return ans;
        }

        private static void RecursionRemoveParenth(string s, int check, int del, List<string> ans, char[] ops) {
            int count = 0;//检查括号正确性
            for (int i = check; i < s.Length; i++) {
                if (s[i] == ops[0]) {
                    count++;
                } else if (s[i] == ops[1]) {
                    count--;
                    if (count < 0) { //出现多的右括号，可以从del开始检查有哪些括号能被删除
                        for (int j = del; j < s.Length; j++) { //枚举删除位置
                            if (s[j] == ops[1] && (j == 0 || s[j - 1] != ops[1])) {
                                RecursionRemoveParenth(s[0..j] + s[(j + 1)..], i, j, ans, ops);
                                //删除了一个右括号，那么[0..j]左闭右闭区间内一定括号正确匹配，可以把i作为下次检查
                                //的起始位置继续进行括号检查。
                                //优化：j的前方删除了字符 下一次一定不需要再删除了，再删前面就匹配不了了
                            }
                        }

                        return;//找到一个多余的右括号就在此停止，枚举删除的位置，递归运行起来相当于对每个能删除的位置进行回溯组合
                    }
                }
            }
            //逛了一圈均正确
            if (ops.SequenceEqual(new char[] { ')', '(' })) {
                ans.Add(string.Concat(s.Reverse()));
            }
            //前面全整完了
            if (ops.SequenceEqual(new char[] { '(', ')' })) {
                RecursionRemoveParenth(string.Concat(s.Reverse()), 0, 0, ans, new char[] { ')', '(' });
            }

        }

        public static string RemoveOuterParentheses(string s) {
            int count = 0;
            StringBuilder sb = new();
            foreach (var item in s) {
                if (item == '(') {
                    count++;
                } else {
                    count--;
                }

                if (item == '(' && count > 1 || item == ')' && count > 0) {
                    sb.Append(item);
                }
            }
            return sb.ToString();
        }

        public static int LongestValidParentheses(string s) {
            Stack<(int, int)> st = new();//idx, score
            st.Push((-1, 0));
            int res = 0;
            for (int i = 0; i < s.Length; i++) {
                char item = s[i];
                int count = st.Count == 0 ? 0 : st.Peek().Item2;
                if (item == '(') {
                    count++;
                } else {
                    count--;
                }
                if (st.Count > 0) {
                    var top = st.Peek();
                    while (st.Count > 0 && st.Peek().Item2 > count) {
                        var popped = st.Pop();
                        if (popped.Item2 == top.Item2) {
                            if (((top.Item1 - popped.Item1) & 1) == 0) {
                                res = Math.Max(res, top.Item1 - popped.Item1);
                            }
                        }
                    }
                }

                st.Push((i, count));
            }
            if (st.Count > 0) {
                var top = st.Pop();
                while (st.Count > 0) {
                    var popped = st.Pop();
                    if (popped.Item2 == top.Item2) {
                        res = Math.Max(res, top.Item1 - popped.Item1);
                    } else {
                        if (st.Count > 0) {
                            top = popped;
                        }
                    }
                }
            }
            return res;
        }

        public static bool CheckPalindromeFormation(string a, string b) {
            return CheckPalindrome(a, b, 0);
        }

        public static bool CheckPalindrome(string a, string b, int count) {
            int i;
            for (i = 0; i < a.Length;) {
                if (a[i] == b[^(i + 1)]) {
                    i++;
                } else {
                    break;
                }
            }
            //i位置就不匹配了
            if (i >= a.Length / 2) {
                return true;
            }
            if (IsPalindromeString(a[i..^(i)]) || IsPalindromeString(b[i..^(i)])) {
                return true;
            }
            if (count > 0) {
                return false;
            }
            return CheckPalindrome(b, a, count + 1);//检查第二次
        }

        public static int[] SortArrayByParityII(int[] nums) {
            Partition(nums);
            for (int i = 0; i < nums.Length >> 1; i += 2) { //数组前一半是奇数后一半是偶数
                (nums[i], nums[^(1 + i)]) = (nums[^(1 + i)], nums[i]);
            }
            return nums;

            //快慢指针
            static void Partition(int[] nums) {
                int odd = 0; //[0..odd)是奇数
                int even = 0;//[odd..length)是偶数
                for (int i = 0; i < nums.Length; i++) {
                    if ((nums[i] & 1) == 0) {
                        even++;
                    } else {
                        (nums[odd], nums[i]) = (nums[i], nums[odd]);
                        odd++;
                        even++;
                    }
                }
            }
        }

        public static int CountPrimes(int n) {
            n--;
            if (n < 2) {
                return 0;
            } else if (n == 2 || n == 3) {
                return n - 1;
            }

            bool[] arr = new bool[n + 1];//FALSE为质数，TRUE为合数
            arr[0] = arr[1] = true;
            int c = (int)Math.Sqrt(n);
            for (int i = 2; i <= c; i++) {
                while (arr[i]) {
                    i++;
                }
                for (int j = i; i * j <= n; j++) { //从i * i开始算
                    arr[i * j] = true;
                }
            }
            return arr.Count((a) => !a);
        }

        public static int FindCelebrity(int n) {
            int l = 0, r = n - 1;
            while (l < r) {
                if (Knows(l, r) && !Knows(r, l)) {
                    //l肯定不是名人
                    l++;
                } else if (!Knows(l, r) && Knows(r, l)) {
                    r--;
                } else {
                    l++;
                    r--;
                }
            }
            if (r != l) {
                return -1;
            } else {
                for (int i = 0; i < n; i++) {
                    if (r != i && (!Knows(i, r) || Knows(r, i))) {
                        return -1;
                    }
                }
                return r;
            }

            static bool Knows(int a, int b) {
                if (a == 0 && b == 1) {
                    return true;
                }
                if (a == 2 && b == 0) {
                    return true;
                }
                if (a == 2 && b == 1) {
                    return true;
                }
                return false;
            }
        }

        public static int TreeCountProduct(TreeNode tree, long left, long right) {
            int res = 0;
            DFS(tree, tree.val, left, right, ref res);
            return res;

            static void DFS(TreeNode node, long product, long left, long right, ref int ans) {
                if (node.left is null && node.right is null) {
                    //收集结果
                    if (product <= right && left <= product) {
                        ans++;
                    }
                    return;
                }
                if (node.left is not null) {
                    DFS(node.left, product * node.left.val, left, right, ref ans);
                }
                if (node.right is not null) {
                    DFS(node.right, product * node.right.val, left, right, ref ans);
                }
            }
        }
    }
}
