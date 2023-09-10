// See https://aka.ms/new-console-template for more information
using 树;
using 排序;
using System.Diagnostics;

AVLTree<long> tree = new();

int[] arr = ArraySort.GenerateOrderlyInts(1, 50);
ArraySort.ShuffleArr(ref arr);
Stopwatch sw = Stopwatch.StartNew();
foreach (var item in arr) {
    tree.Add(item);
}
sw.Stop();
Console.WriteLine($"添加耗时：{sw.Elapsed}");
ArraySort.ShuffleArr(ref arr);
sw.Restart();
foreach (var item in tree) {
    
}
sw.Stop();
Console.WriteLine($"遍历耗时：{sw.Elapsed}");

Console.WriteLine("Done.");