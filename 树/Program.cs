// See https://aka.ms/new-console-template for more information

using 树;

char[] cs = ['a', 'b', 'c', 'd', 'e', 'f'];
ulong[] wt = [1, 2, 3, 4, 5, 6];
HuffmanTree<char> t = new(cs, wt);

for (int i = 0; i < cs.Length; i++) {
    Console.WriteLine(t.GetCode(cs[i]));
}

var t2 = HuffmanTree<char>.CreateFromText("Available on the following websites with GPU acceleration".ToLower(), out string txt);
Console.WriteLine(txt);
Console.WriteLine("------");
Console.WriteLine(t2.Decode("1110"));
