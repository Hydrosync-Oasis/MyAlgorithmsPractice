using 链表;

LinkedList1<int> list = new(0);
list.AddLast(9);
list.AddLast(8);
list.AddLast(8);
list.AddLast(7);
list.AddLast(10);

list.RemoveAt(4);
Console.WriteLine(list);
Console.WriteLine(list.IndexOf(10));
list.Insert(99, 1);
Console.WriteLine(list);
list.RemoveAt(5);
Console.WriteLine(list);
list.Set(0, -1);
Console.WriteLine(list);
list.Set(1, -2);
Console.WriteLine(list);
//-1 -2 9 8 8
list.Reverse();
Console.WriteLine(list);
Console.WriteLine(list.GetLast());
list.AddLast(114514);
Console.WriteLine(list);