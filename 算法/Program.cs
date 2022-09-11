using System.Runtime.Serialization.Json;
using System.Text;

namespace LeetCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TreeNode root = new(1);
            root.right = new(2);

            TreeNode res = TrimBST(root, 2, 4);
            //[1,7,4,9,2,5]
            //TreeNode res = InsertIntoMaxTree(root, 3);
            //node.next.next = new(3);
            //node.next.next.next = new(4);
            //node.next.next.next.next = new(5);
            //Console.WriteLine(LengthOfLIS2(new int[] { 1, 1, 2, 4, 6, 3, 6, 7 }));
            //int a = LengthOfLIS(new int[] { 1, 3, 6, 7, 9, 4, 10, 5 });
            //Console.WriteLine(a);

        }

        public static long GetObjectByte<T>(T t) where T : class
        {
            DataContractJsonSerializer formatter = new(typeof(T));
            using MemoryStream stream = new();
            formatter.WriteObject(stream, t);
            return stream.Length;
        }

        
    }
}