using System.Diagnostics;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace 排序 {
    internal class Program {
        static void Main(string[] args) {
            Stopwatch stopwatch = new();
            var arr = ArraySort.GenerateOrderlyInts(1, 8000000);
            //new int[] { 4, 1, 1, 2, 2 };
            //var arr = new long[] { 1, 4, 5, 1, 4, 9, 8, 1, 0 };
            ArraySort.ShuffleArr(ref arr);
            var backup = arr.ToArray();
            ArraySort<int> example = new(arr, ArraySort<int>.VisualMode.None);
            //Console.WriteLine(example.BinarySearch(82));
            stopwatch.Start();
            example.Sort(ArraySort<int>.SortMethod.QuickSort);
            //Console.WriteLine(example.IsSorted());
            stopwatch.Stop();
            TimeSpan time1 = stopwatch.Elapsed;
            Console.WriteLine($"排序的时间：{time1}");
            //Console.WriteLine(example.ArrToStr());
            Console.WriteLine(example.IsSorted());
            //example = backup;
            //stopwatch.Restart();
            //example.Sort(method: ArraySort<int>.SortMethod.ThreeWayQuick);
            //stopwatch.Stop();
            //TimeSpan time2 = stopwatch.Elapsed;
            //Console.WriteLine($"排序的时间：{time2}");
            //Console.WriteLine(example.IsSorted());
            //Console.WriteLine(time2.CompareTo(time1) switch {
            //    < 0 => $"后者比前者快了{time1 - time2}",
            //    0 => $"二者一样快{time1}",
            //    > 0 => $"前者比后者快了{time2 - time1}",
            //});
        }

        public static void Test(long[] nums) {
            int l = 0; //[0..l)
            int r = nums.Length;//[..len)
            (var left, var top) = Console.GetCursorPosition();
            while (l < nums.Length && l < r) {
                while (l < nums.Length && nums[l] == l + 1) {
                    l++;
                }
                while (l < r && nums[l] != l + 1) {
                    if (nums[l] > nums.Length || nums[l] < 1 || nums[l] == nums[nums[l] - 1]) {
                        Swap(nums, l, --r);
                        Visual(nums, left, top);
                    } else {
                        Swap(nums, l, nums[l] - 1);
                        Visual(nums, left, top);
                    }
                }

            }

            static void Visual(long[] nums, int left, int top) {
                Console.SetCursorPosition(left, top);
                var s = ArraySort.VisualizationArr(nums);
                Console.WriteLine(s);
            }

            static void Swap(long[] arr, long i, long j) {
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }

        }
    }
}
