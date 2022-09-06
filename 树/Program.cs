// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using 树;

BST<int> bST = new(5, 2, 6, 1, 3, 7, 4);
//Random ran = new();
//for (int i = 0; i < 10; i++)
//{
//    bST.Add(ran.Next(0, 10));
//}
//Stopwatch stopwatch = new();
//stopwatch.Start();
//bST.LoopFind(5);
//bST.GetLevel();
//stopwatch.Stop();
//Console.WriteLine(stopwatch.Elapsed);
int[] ints = bST.IteratePreOrder3();
for (int i = 0; i < ints.Length; i++)
{
    Console.WriteLine(ints[i]);
}
//bST.Remove(8);
