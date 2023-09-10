using System.Text;

namespace 排序 {
    public class ArraySort<T> where T : IComparable {
        T[] arr;
        private bool isSorted;
        private readonly VisualMode visualization;
        Random ran = new();
        private bool threeWay;

        /// <summary>
        /// 指定Sort函数应采用的排序方法
        /// </summary>
        public enum SortMethod {
            DequeueSort,
            BubbleSort,
            Selection,
            Insert,
            BinaryInsert,
            ShellSort,
            HeapSort,
            MergeSort,
            MergeBU,
            ThreeWayQuick,
            QuickSort,
            RadixSort,
        }

        public enum VisualMode {
            Num,
            Bar,
            None
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values">应被排序的数组</param>
        /// <param name="visual">是否在排序时采用可视化输出。<em>仅对整形数组有效</em></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ArraySort(T[] values, VisualMode mode) {
            if (values is null) {
                throw new ArgumentNullException(nameof(values));
            } else if (values.Length == 0) {
                throw new ArgumentOutOfRangeException(nameof(values), "长度为 0.");
            } else {
                arr = values;
            }
            isSorted = IsSorted();
            this.visualization = mode;
        }

        public static implicit operator ArraySort<T>(T[] values) => new(values, VisualMode.None);

        public static explicit operator T[](ArraySort<T> arraySortObject) => arraySortObject.arr;

        public bool IsSorted(bool needException = false) {
            //if (isSorted) {
            //    return true;
            //}
            for (int i = 0; i < arr.Length - 1; i++) {
                if (!IsLess(arr[i], arr[i + 1])) {
                    if (needException) {
                        throw new Exception("排序方法错误在第" + i);
                    }
                    return false;
                }
            }
            return true;
        }

        private static bool IsLess(T a, T b) => a.CompareTo(b) <= 0;

        int left, top;

        public void Sort(SortMethod method) {
            (left, top) = Console.GetCursorPosition();
            switch (method) {
                case ArraySort<T>.SortMethod.Selection:
                    SelectionSort();
                    break;
                case ArraySort<T>.SortMethod.Insert:
                    InsertSort(0, arr.Length - 1);
                    break;
                case ArraySort<T>.SortMethod.BinaryInsert:
                    BinaryInsertSort();
                    break;
                case ArraySort<T>.SortMethod.ShellSort:
                    ShellSort();
                    break;
                case ArraySort<T>.SortMethod.DequeueSort:
                    DequeueSort();
                    break;
                case ArraySort<T>.SortMethod.MergeSort:
                    MergeSort();
                    break;
                case ArraySort<T>.SortMethod.MergeBU:
                    MergeBUSort();
                    break;
                case ArraySort<T>.SortMethod.QuickSort:
                    threeWay = false;
                    QuickSort();
                    break;
                case ArraySort<T>.SortMethod.BubbleSort:
                    BubbleSort();
                    break;
                case ArraySort<T>.SortMethod.ThreeWayQuick:
                    threeWay = true;
                    QuickSort();
                    break;
                case ArraySort<T>.SortMethod.HeapSort:
                    HeapSort();
                    break;
                case ArraySort<T>.SortMethod.RadixSort:
                    RadixSort(0, arr.Length - 1);
                    break;
                default:
                    throw new ArgumentException("不支持的排序方法", nameof(method));
            }
            isSorted = true;
            Visualization(arr);
        }

        private void BubbleSort() {
            for (int i = 0; i < arr.Length; i++) {
                for (int j = 0; j < arr.Length - i - 1; j++) {
                    if (!IsLess(arr[j], arr[j + 1])) {
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                        { Visualization(arr, 1); }
                    }
                }
            }
        }

        private void SelectionSort() {
            for (int i = 1; i < arr.Length / 2 + 1; i++) {
                //前i个和后i个是最小和最大区域
                int min = i - 1;
                int max = arr.Length - i;
                for (int j = i - 1; j <= arr.Length - i; j++) {
                    if (IsLess(arr[max], arr[j])) { max = j; }
                    if (IsLess(arr[j], arr[min])) { min = j; }
                }
                if (min == arr.Length - i && max == i - 1) {
                    (arr[min], arr[max]) = (arr[max], arr[min]);
                } else if (max == i - 1 && min != arr.Length - i) {
                    (arr[^i], arr[max]) = (arr[max], arr[^i]);
                    (arr[i - 1], arr[min]) = (arr[min], arr[i - 1]);
                } /*else if (max != i - 1 && min == arr.Length - i) {
                    (arr[i - 1], arr[min]) = (arr[min], arr[i - 1]);
                    (arr[arr.Length - i], arr[max]) = (arr[max], arr[arr.Length - i]);
                }*/ else {
                    (arr[i - 1], arr[min]) = (arr[min], arr[i - 1]);
                    (arr[^i], arr[max]) = (arr[max], arr[^i]);
                }

                Visualization(arr);
            }
        }

        private void InsertSort(int left, int right) {
            for (int i = left + 1; i <= right; i++) { //前i个元素有序
                int j = i;
                T wannaMoveElement = arr[i];
                while (j >= left + 1 && !IsLess(arr[j - 1], wannaMoveElement)) {
                    arr[j] = arr[j - 1];
                    j--;
                    Visualization(arr);
                }
                //循环退出说明arr[j - 1] <= wannaMoveElement
                //最后一次的循环是arr[j] > wannaMoveElement
                arr[j] = wannaMoveElement;
            }
        }

        private void BinaryInsertSort() {
            //在前面有序部分中二分查找较大的数
            for (int i = 1; i < arr.Length; i++) {
                if (IsLess(arr[i], arr[i - 1])) {
                    int index = BinarySearch(arr[i], 0, i - 1);
                    T wannaMove = arr[i];
                    for (int j = i - 1; j >= index; j--) {
                        arr[j + 1] = arr[j];
                        Visualization(arr);
                    }
                    arr[index] = wannaMove;
                }
            }
        }

        private void ShellSort() {
            int n;
            int gap = arr.Length - 1;

            while (gap > 0) {
                n = arr.Length / gap;
                for (int j = 0; j < gap; j++) {
                    for (int i = 1; i < n; i++) {
                        //产生了序列0 * 4 + j, 1 * 4 + j, 2 * 4 + j, ...
                        int k = i;
                        while ((k - 1) * gap + j >= 0
                            && IsLess(arr[k * gap + j], arr[(k - 1) * gap + j])) {
                            (arr[k * gap + j], arr[(k - 1) * gap + j])
                                = (arr[(k - 1) * gap + j], arr[k * gap + j]);
                            k--;
                            Visualization(arr);
                        }
                    }
                }
                if (gap == 1) {
                    break;
                }
                gap = gap / 3 + 1;
            }

        }

        private void DequeueSort() {
            //2
            for (int count = 1; count <= arr.Length - 1; count++) {
                for (int i = 0; i < arr.Length - count; i++) {
                    if (!IsLess(arr[0], arr[1])) {
                        (arr[0], arr[1]) = (arr[1], arr[0]);
                    }
                    MoveToLast(0, arr);
                    Visualization(arr);

                }
                for (int i = 0; i < count; i++) {
                    MoveToLast(0, arr);
                }
                Visualization(arr);

            }

            static void MoveToLast(int index, T[] array) {
                //0 or 1
                T tmp = array[index];
                for (int i = index; i < array.Length - 1; i++) {
                    (array[i + 1], array[i]) = (array[i], array[i + 1]);
                }
                array[^1] = tmp;
            }
        }

        private void HeapSort() {
            for (int i = arr.Length - 1; i >= 0; i--) {
                int idx = i;                //倒序==自底向上方法，渐进复杂度为线性
                int left = (i << 1) + 1;    //正序==自顶向下方法，随着进行，深度和宽度都增大，复杂度为线性对数
                while (left < arr.Length) {
                    int right = left + 1;
                    int largest = right < arr.Length && IsLess(arr[left], arr[right]) ? right : left;
                    if (IsLess(arr[largest], arr[idx])) {
                        break;
                    }
                    (arr[idx], arr[largest]) = (arr[largest], arr[idx]);
                    idx = largest;
                    left = (idx << 1) + 1;
                    Visualization(arr);

                }
            }
            int size = arr.Length;
            for (int i = 0; i < arr.Length; i++) {
                size--;
                (arr[0], arr[^(i + 1)]) = (arr[^(i + 1)], arr[0]);
                //开始取左右孩子索引
                int idx = 0;
                while ((idx << 1) + 1 < size) {
                    int left = (idx << 1) + 1;
                    int right = left + 1;
                    int largestIdx = (right < size && IsLess(arr[left], arr[right])) ? right : left;
                    if (IsLess(arr[largestIdx], arr[idx])) {
                        break;
                    } else {
                        (arr[idx], arr[largestIdx]) = (arr[largestIdx], arr[idx]);
                    }
                    idx = largestIdx;
                    Visualization(arr);
                }
            }
        }

        private void MergeSort() {
            RecursionMergeSort(0, arr.Length - 1);
        }

        private void MergeBUSort() {
            RecursionMergeBUSort(16);
        }

        private void RecursionMergeBUSort(int length) {
            int count = arr.Length << 1;
            while (length < count) {
                int i = 0;
                int halfLength = length >> 1;
                while (i < arr.Length) {
                    MergeConquer(i,
                        i + halfLength >= arr.Length ? arr.Length - 1 : i + halfLength,
                        i + length - 1 >= arr.Length ? arr.Length - 1 : i + length - 1);
                    i += length;
                }
                length <<= 1;
            }
            //RecursionMergeBUSort(length << 1, visualization);
        }

        private void RecursionMergeSort(int left, int right) {
            if (left != right) {
                //(var first, var second) = MergeDivide(values, values.Length / 2);
                RecursionMergeSort(left, (left + right) >> 1);
                RecursionMergeSort(((left + right) >> 1) + 1, right);
                MergeConquer(left, ((left + right) >> 1) + 1, right);
                Visualization(arr, 20);

            }
        }

        private void MergeConquer(int index1, int index2, int end) {
            if (end - index1 <= 15) {
                InsertSort(index1, end);
            } else {
                if (IsLess(arr[index2 - 1], arr[index2])) {
                    return;
                }
                int cur1 = index1, cur2 = index2;
                List<T> res = new(end - index1 + 1);
                while (cur1 < index2 || cur2 <= end) {
                    if (cur1 >= index2) {
                        res.Add(arr[cur2++]);
                    } else if (cur2 > end) {
                        res.Add(arr[cur1++]);
                    } else if (IsLess(arr[cur1], arr[cur2])) {
                        res.Add(arr[cur1++]);
                    } else {
                        res.Add(arr[cur2++]);
                    }
                }

                for (int i = 0; i < res.Count; i++) {
                    arr[index1 + i] = res[i];
                    Visualization(arr, 5);

                }
            }
        }

        private void QuickSort() {
            HelperQuickSort(0, arr.Length - 1);
        }

        private void HelperQuickSort(int left, int right) {
            if (right - left <= 15) {
                InsertSort(left, right);
            } else {
                if (threeWay) {
                    (int divPos1, int divPos2) = NetherLand(left, right);
                    HelperQuickSort(left, divPos1);
                    HelperQuickSort(divPos2, right);
                } else {
                    int divPos = RecursionQuickSort(left, right);
                    HelperQuickSort(left, divPos - 1);
                    HelperQuickSort(divPos + 1, right);
                }
            }
        }

        private int RecursionQuickSort(int left, int right) {
            int baseIdx = ran.Next(left, right);
            (arr[left], arr[baseIdx]) = (arr[baseIdx], arr[left]);
            T val = arr[left];//基准值
            int cur1 = left + 1;
            int cur2 = right;
            while (true) {
                while (cur1 <= cur2 && IsLess(arr[cur1], val)) { cur1++; }
                while (cur1 <= cur2 && IsLess(val, arr[cur2])) { cur2--; }
                if (cur1 > cur2) { break; }//[left + 1..cur1)都小于基准值arr[left]         (cur2..right]都大于基准值
                (arr[cur1], arr[cur2]) = (arr[cur2], arr[cur1]);
                Visualization(arr);
            }
            int cur = cur1 - 1;//cur1 - 1是小于基准值的最后一位

            #region 另一种方法
            //for (int i = left + 1; i < cur; i++) {
            //    if (!IsLess(arr[i], val)) {
            //        //如果大于基准值移动到后面
            //        (arr[i], arr[cur]) = (arr[cur--], arr[i--]); //判断交换后的是不是还比val大
            //        if (visualization) { Visualization(arr); }
            //    }
            //}
            //if (!IsLess(arr[cur], val)) { cur--; }//最后一次交换后仍然还是比val大
            #endregion
            (arr[cur], arr[left]) = (arr[left], arr[cur]);
            Visualization(arr);
            return cur;
        }

        private (int, int) NetherLand(int left, int right) {
            int idx = ran.Next(left, right + 1);
            T val = arr[idx];
            (arr[idx], arr[left]) = (arr[left], arr[idx]);
            int sep1 = left;//[..sep1]都小于val
            int sep2 = right + 1;//[sep2..]都大于val
            int i = left + 1;//[sep1 + 1 .. i - 1]等于val
            while (i != sep2) {
                if (!IsLess(val, arr[i])) {
                    sep1++;
                    (arr[i], arr[sep1]) = (arr[sep1], arr[i]);
                    i++;
                } else if (val.CompareTo(arr[i]) == 0) {
                    i++;
                } else {
                    sep2--;
                    (arr[i], arr[sep2]) = (arr[sep2], arr[i]);
                }
                Visualization(arr);
            }
            (arr[left], arr[sep1]) = (arr[sep1], arr[left]);//把基准值移动到等于区
            Visualization(arr);
            return (sep1 - 1, sep2);
        }

        private void RadixSort(int left, int right) {
            T max = arr.Max()!;
            if (typeof(T).Name != "Int16" && typeof(T).Name != "Int32" && typeof(T).Name != "Int64") {
                throw new InvalidOperationException("无法对非整数进行基数排序");
            }
            var maxInt = (long)Convert.ChangeType(max, typeof(long))!;
            int digit = ArraySort<T>.GetDigits(maxInt);
            long[] backup = (long[])Convert.ChangeType(arr, typeof(long[]))!;
            long[] result = new long[backup.Length];
            Visualization(result);
            for (int i = 0; i < digit; i++) {
                int[] bucket = new int[10];
                for (int j = 0; j < backup.Length; j++) {
                    bucket[ArraySort<T>.GetADigit(backup[j], i)]++;//开始装桶
                }
                for (int j = 1; j < bucket.Length; j++) {
                    bucket[j] += bucket[j - 1];//求前缀和
                }
                for (int j = backup.Length - 1; j >= 0; j--) {
                    int count = --bucket[ArraySort<T>.GetADigit(backup[j], i)];
                    //[0..count)内填充
                    result[count] = backup[j];
                    Visualization(result);
                }
                result.CopyTo(backup, 0);
            }
            arr = (T[])Convert.ChangeType(result, typeof(T[]));
        }

        private static int GetDigits(long num) {
            int digit = 0;
            while (num != 0) {
                digit++;
                num /= 10;
            }
            return digit;
        }

        private static int GetADigit(long num, int digit) {
            for (int i = 0; i < digit; i++) {
                num /= 10;
            }
            return (int)num % 10;
        }

        private void Visualization(T[] values, int sleepTime = 1) {
            Visualization((long[])Convert.ChangeType(values, typeof(long[])), sleepTime);
        }

        private void Visualization(long[] values, int sleepTime = 1) {
            if (visualization == VisualMode.None) {
                return;
            }
            Console.SetCursorPosition(left, top);
            if (visualization == VisualMode.Num) {
                Console.WriteLine(this);
            } else {
                var s = ArraySort.VisualizationArr((long[])Convert.ChangeType(values, typeof(long[])));
                Console.WriteLine(s);
            }
            Thread.Sleep(sleepTime);
        }

        /// <summary>
        /// 二分法求下界
        /// </summary>
        /// <param name="targetValue">目标值</param>
        /// <returns>索引位置</returns>
        private int BinarySearch(T targetValue, int left, int right) {
            while (left < right) {
                int mid = ((right - left) >> 1) + left;
                if (targetValue.CompareTo(arr[mid]) > 0) {
                    left = mid + 1;
                } else {
                    right = mid;
                }
            }
            return left;
        }

        public string ArrToStr() {
            StringBuilder sb = new();
            for (int i = 0; i < arr.Length - 1; i++) {
                sb.Append(arr[i]);
                sb.Append(", ");
            }
            sb.Append(arr[^1]);
            return sb.ToString();
        }

        public override string ToString() {
            StringBuilder sb = new();
            sb.Append($"此数组基本信息：\n\t长度：{arr.Length}\n\t{(isSorted ? "已排序" : "未排序")}\n");
            sb.Append("数组成员：" + ArrToStr());
            return sb.ToString();
        }
    }

    /// <summary>
    /// 排序前的工具类
    /// </summary>
    public static class ArraySort {
        public static int[] GenerateOrderlyInts(int min = 1, int max = 100, bool reversed = false) {
            int[] res = new int[max - min + 1];
            for (int i = 0; i < res.Length; i++) {
                if (!reversed) {
                    res[i] = min + i;
                } else {
                    res[i] = max - i;
                }
            }
            return res;
        }

        public static int[] GenerateRandomInts(int count, int min = 0, int max = int.MaxValue) {
            int[] arr = new int[count];
            Random ran = new();
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = ran.Next(min, max);
            }
            return arr;
        }

        public static void ShuffleArr<T>(ref T[] arr) {
            Random ran = new();
            for (int i = 0; i < arr.Length - 1; i++) {
                int targetIndex = ran.Next(i + 1, arr.Length);
                (arr[i], arr[targetIndex]) = (arr[targetIndex], arr[i]);
            }
        }

        public static int[] GenerateSameNums(int count, int num) {
            int[] arr = new int[count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = num;
            }
            return arr;
        }

        public static string VisualizationArr(long[] arr) {
            int[,] res = new int[arr.Length, 60];
            long max = arr.Max();
            for (int i = 0; i < arr.Length; i++) {
                for (int j = 0; j < Math.Floor(arr[i] * (60.0 / max)); j++) {
                    res[i, j] = 1;
                }
            }
            StringBuilder sb = new();
            for (int i = 0; i < res.GetLength(1); i++) {
                for (int j = 0; j < res.GetLength(0); j++) {
                    if (res[j, i] == 0) {
                        sb.Append(' ');
                    } else {
                        sb.Append('M');
                    }
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }

        public static string VisualizationArr(List<int> arr) {
            int[,] res = new int[arr.Count, 60];
            int max = arr.Max();
            for (int i = 0; i < arr.Count; i++) {
                for (int j = 0; j < Math.Floor(arr[i] * (60.0 / max)); j++) {
                    res[i, j] = 1;
                }
            }

            StringBuilder sb = new();

            for (int i = 0; i < res.GetLength(1); i++) {
                for (int j = 0; j < res.GetLength(0); j++) {
                    if (res[j, i] == 0) {
                        sb.Append(' ');
                    } else {
                        sb.Append('M');
                    }
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }

        public static int[] GenerateMonotoArr(int n) {
            int point = n / 5;
            List<int> pointIdx = new();
            Random ran = new();
            for (int i = 0; i < point; i++) {
                pointIdx.Add(ran.Next(0, n));
            }
            int[] arr = new int[n];
            pointIdx.Sort();
            bool increasing = true;
            int lastIdx = 0;
            for (int i = 0; i < pointIdx.Count; i++) {
                for (int j = lastIdx + 1; j <= pointIdx[i]; j++) {
                    if (increasing) {
                        arr[j] = arr[j - 1] + ran.Next(0, 10);
                    } else {
                        var newVal = arr[j - 1] - ran.Next(0, 9);
                        if (newVal < 0) {
                            newVal = 0;
                        }
                        arr[j] = newVal;

                    }
                }
                increasing = !increasing;
                lastIdx = pointIdx[i];
            }
            for (int i = pointIdx[^1] + 1; i < arr.Length; i++) {

                var newVal = arr[i - 1] - ran.Next(0, 9);
                if (newVal < 0) {
                    newVal = 0;
                }
                arr[i] = newVal;

            }
            return arr;
        }
    }
}
