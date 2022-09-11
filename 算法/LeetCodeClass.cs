using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode算法
{

    internal class MyCircularQueue
    {
        int[] data;
        int N;
        int first = 0, last = 0;

        public MyCircularQueue(int k)
        {
            data = new int[k];
            //N = k;
        }

        public bool EnQueue(int value)
        {
            if (N == data.Length)
            {
                return false;
            }
            data[last] = value;
            last = (last + 1) % this.data.Length;
            N++;
            return true;
        }

        public bool DeQueue()
        {
            if (N == 0)
            {
                return false;
            }
            data[first] = default;
            first = (first + 1) % data.Length;
            N--;

            return true;
        }

        public int Front()
        {
            if (N == 0)
            {
                return -1;
            }
            else
            {
                return data[first];
            }
        }

        public int Rear()
        {
            if (this.N == 0)
            {
                return -1;
            }
            else
            {
                return data[(last - 1 + data.Length) % data.Length];
            }
        }

        public bool IsEmpty() => first == last;

        public bool IsFull() => (last + 1) % data.Length == first;
    }

    internal class MyCircularDeque
    {
        int[] data;

        int start = 0;//先设置再移动
        int end = 0;//先移动再设置
        int N = 0;

        public MyCircularDeque(int k) => data = new int[k];

        public bool InsertFront(int value)
        {
            if (N == data.Length)
            {
                return false;
            }
            data[start] = value;
            start = (start + data.Length - 1) % data.Length;//此时start处的数组成员空
            N++;
            return true;
        }

        public bool InsertLast(int value)
        {
            if (N == data.Length)
            {
                return false;
            }
            end = (end + data.Length + 1) % data.Length;//此时start处的数组成员空
            data[end] = value;
            N++;
            return true;
        }

        public bool DeleteFront()
        {
            if (N == 0)
            {
                return false;
            }
            start = (start + data.Length + 1) % data.Length;
            data[start] = default;
            N--;
            return true;
        }

        public bool DeleteLast()
        {
            if (N == 0)
            {
                return false;
            }
            data[end] = default;
            end = (end + data.Length - 1) % data.Length;
            N--;
            return true;
        }

        public int GetFront()
        {
            if (N == 0)
            {
                return -1;
            }
            else
            {
                return data[(start + data.Length + 1) % data.Length];
            }
        }

        public int GetRear()
        {
            if (N == 0)
            {
                return -1;
            }
            else
            {
                return data[end];
            }
        }

        public bool IsEmpty() => N == 0;

        public bool IsFull() => N == data.Length;
    }

    internal class MyLinkedListNoDummy
    {
        private class Node
        {
            public int val;
            public Node? next;

            public Node(int val, Node? next = null)
            {
                this.val = val;
                this.next = next;
            }
        }

        private Node? head;

        int N = 0;

        public MyLinkedListNoDummy()
        {

        }

        public int Get(int index)
        {
            Node? cur = head;
            for (int i = 0; i < index; i++)
            {
                cur = cur?.next;
            }
            return cur is null ? -1 : cur.val;
        }

        public void AddAtHead(int val)
        {
            if (N == 0)
            {
                head = new(val);
            }
            else
            {
                head = new(val, head);
            }
            N++;
        }

        public void AddAtTail(int val)
        {
            Node cur = head;

            if (N == 0)
            {
                head = new(val);
                N++;
                return;
            }
            while (true)
            {
                if (cur.next is null)
                {
                    cur.next = new(val);
                    N++;
                    return;
                }

                cur = cur.next;
            }
        }

        public void AddAtIndex(int index, int val)
        {
            if (index > N)
            {
                return;
            }
            if (index <= 0)
            {
                AddAtHead(val);
            }
            else if (index == N)
            {
                AddAtTail(val);
            }
            else
            {
                Node? cur = head;
                for (int i = 0; i < index - 1; i++)
                {
                    cur = cur?.next;
                }

                if (cur is null)
                {
                    return;
                }
                else
                {
                    Node? tmp = cur.next;
                    cur.next = new(val, tmp);
                }

                N++;
            }
        }

        public void DeleteAtIndex(int index)
        {
            if (index >= N || index < 0)
            {
                return;
            }

            if (index == 0)
            {
                head = head?.next;
                N--;
                return;
            }

            Node? cur = head;
            for (int i = 0; i < index - 1; i++)
            {
                cur = cur?.next;
            }
            if (cur is null)
            {
                return;
            }

            cur.next = cur.next?.next;
            N--;
        }
    }

    internal class MyLinkedList
    {
        private class Node
        {
            public int val;
            public Node? next;

            public Node(int val, Node? next = null)
            {
                this.val = val;
                this.next = next;
            }
        }

        private Node dummyhead;

        int N = 0;

        public MyLinkedList()
        {
            dummyhead = new(-1, null);//dummy
        }

        public int Get(int index)
        {
            Node? cur = dummyhead;
            for (int i = 0; i < index + 1; i++)
            {
                cur = cur?.next;
            }
            return cur is null ? -1 : cur.val;
        }

        public void AddAtHead(int val)
        {
            dummyhead.next = new(val, dummyhead.next);

            N++;
        }

        public void AddAtTail(int val)
        {
            Node cur = dummyhead;

            while (true)
            {
                if (cur.next is null)
                {
                    cur.next = new(val);
                    N++;
                    return;
                }

                cur = cur.next;
            }
        }

        public void AddAtIndex(int index, int val)
        {
            Node? cur = dummyhead;
            for (int i = 0; i < index; i++)
            {
                cur = cur?.next;
            }

            if (cur is null)
            {
                return;
            }
            else
            {
                Node? tmp = cur?.next;
                cur.next = new(val, tmp);
                N++;
            }

        }

        public void DeleteAtIndex(int index)
        {
            if (index >= N || index < 0)
            {
                return;
            }

            Node? cur = dummyhead;
            for (int i = 0; i < index; i++)
            {
                cur = cur?.next;
            }

            cur.next = cur.next?.next;
            N--;
        }
    }
}
