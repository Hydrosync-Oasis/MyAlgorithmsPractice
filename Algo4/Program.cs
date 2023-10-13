// See https://aka.ms/new-console-template for more information
using Algo4;
using System.Diagnostics;
using static Algo4._3x3Search;

State rightState = _3x3Search.rightState;
(int x, int y)[] rightPos = new (int, int)[9];
for (int i = 0; i < 3; i++) {
    for (int j = 0; j < 3; j++) {
        rightPos[rightState[i, j]] = (i, j);
    }
}
_3x3Search t = new([[5, 1, 3], [2, 6, 8], [4, 7, 0]], F);
Stopwatch sw = Stopwatch.StartNew();
var path = t.Search();
sw.Stop();
Console.WriteLine($"Done! 用时{sw.Elapsed}，以下是还原步骤：");
foreach (var item in path) {
    Console.WriteLine(item);
    Console.WriteLine("---");
}


int F(State state) {
    // 0--8
    // 0: 空缺位置

    int dis = 0;
    for (int i = 0; i < 3; i++) {
        for (int j = 0; j < 3; j++) {
            dis += Math.Abs(i - rightPos[state[i, j]].x) + Math.Abs(j - rightPos[state[i, j]].y);
        }
    }
    return dis;
}
