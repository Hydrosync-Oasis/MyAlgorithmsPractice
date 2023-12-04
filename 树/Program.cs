// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using 树;

SegmentTreeDynamic tree = new(1, 20);
tree.Update(1, 5, 2);
tree.Update(2, 6, 3);
tree.Add(1, 3, 3);
// tree.Update(1, 2, 10);
// tree.Update(2, 2, 10);
// tree.Update(4, 5, 1);


int res1 = tree.Query(1, 3);
int res2 = tree.Query(4, 6);
int res3 = tree.Query(3, 4);
Console.WriteLine(res1);
Console.WriteLine(res2);
Console.WriteLine(res3);



