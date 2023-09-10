using System.Diagnostics;
using System.Text;

namespace Imitate {
    internal class Program {
        static void Main(string[] args) {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            Dictionary<char, List<char>> pairs = new();
            string[] lines;
            try {
                lines = File.ReadAllLines(@"data.txt");
            } catch (FileNotFoundException) {
                Console.WriteLine("找不到文件（同目录下data.txt），请重启程序");
                Console.ReadKey();
                return;
            }
            int count;
            do {
                Console.WriteLine("请输入生成字数：");
            } while (!int.TryParse(Console.ReadLine(), out count));

            if (lines.Length == 0) {
                Console.WriteLine("数据文本空，程序退出.");
                Console.ReadKey();
                return;
            }
            foreach (var item in lines) {
                foreach (var character in item)
                    if (!pairs.ContainsKey(character))
                        pairs.Add(character, new List<char>() { });
            }
            foreach (var item in lines) {
                for (int i = 0; i < item.Length - 1; i++)
                    pairs[item[i]].Add(item[i + 1]);
            }
            stopwatch.Stop();
            Console.WriteLine("读取用时 {0}.", stopwatch.Elapsed);

            stopwatch.Start();
            StringBuilder sb = new();
            for (int i = 0; i < 1; i++) {
                Random random = new();
                int max = lines.Length;
                char first = lines[random.Next(0, max)][0];
                char second;
                sb.Append(first);
                for (int j = 0; j < count; j++) {
                    max = pairs[first].Count;
                    if (max != 0) {
                        second = pairs[first][random.Next(0, max)];
                        first = second;
                        sb.Append(first);
                    } else {
                        sb.Append('，');
                        first = lines[random.Next(0, max)][0];
                        continue;
                    }
                }
                sb.AppendLine();
            }
            stopwatch.Stop();
            Console.WriteLine("生成用时 {0}.", stopwatch.Elapsed);
            File.WriteAllText(@"result.txt", sb.ToString());
            Console.ReadKey();
        }


    }
}