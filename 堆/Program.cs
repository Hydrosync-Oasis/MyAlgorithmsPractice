namespace 堆 {
    internal class Program {
        static void Main(string[] args) {
            Heap<int, int> heap = new((a, b) => a.CompareTo(b));
            heap.Push(0, 4);
            heap.Push(1, 6);
            heap.Push(2, 7);
            heap.Push(3, 1);
            heap.Push(4, 3);
            heap.Push(5, 5);
            heap.Push(6, 2);
            Console.WriteLine(heap.Pop());
            Console.WriteLine(heap.Pop());
            Console.WriteLine(heap.Pop());
            Console.WriteLine(heap.Pop());
            heap[3] = 99;
            Console.WriteLine(heap.Peek());
            heap.Remove(3);
            Console.WriteLine(heap.Push(5, 90));
            Console.WriteLine(heap.Peek());
        }
    }
}