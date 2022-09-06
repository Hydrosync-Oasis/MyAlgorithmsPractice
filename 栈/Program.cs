namespace 栈
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stack2<int> stack2 = new Stack2<int>();
            stack2.Push(4);
            stack2.Push(5);
            stack2.Push(6);
            Console.WriteLine(stack2.Pop());

        }
    }
}