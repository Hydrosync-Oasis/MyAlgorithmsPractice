namespace 图 {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
            char[][] nodes = new char[8][];
            nodes[0] = new char[] { 'A', 'B' };
            nodes[1] = new char[] { 'A', 'C' };
            nodes[2] = new char[] { 'B', 'D' };
            nodes[3] = new char[] { 'D', 'C' };
            nodes[4] = new char[] { 'B', 'E' };
            nodes[5] = new char[] { 'C', 'E' };
            nodes[6] = new char[] { 'D', 'F' };
            nodes[7] = new char[] { 'E', 'F' };
            int[] w = { 5, 0, 15, 30, 20, 35, 20, 10 };
            Graph<char> graph = new(nodes, w);
            var res = graph.DijkstraMinEdge('A');
            foreach (var item in res) {
                Console.WriteLine($"{item.Key}\t{item.Value}");
            }
            var res2 = graph.TopologicalSort();
            foreach (var item in res2) {
                Console.WriteLine(item);
            }

            int[][] p = new int[5][];
            p[0] = new int[] { 1, 1 };
            p[1] = new int[] { 1, 2 };
            p[2] = new int[] { 2, 3 };
            p[3] = new int[] { 2, 4 };
            p[4] = new int[] { 4, 2 };

            var res3 = Graph<int>.GenerateAdjacentMat(p);

            for (int i = 0; i < res3.GetLength(0); i++) {
                for (int j = 0; j < res3.GetLength(1); j++) {
                    Console.Write(res3[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}