using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 数组;

namespace 栈
{
    public class Stack1<T> : IStack<T>
    {
        int N;
        Array1<T> data;

        public Stack1(params T[] values)
        {
            if (values == null || values.Length == 0)
            {
                data = new Array1<T>();
            }
            else
            {
                foreach (var item in values)
                {
                    Push(item);
                }
            }
        }

        public int Count => N;

        public bool IsEmpty() => N == 0;

        public T Peek() => data.Get(N - 1);

        public T Pop()
        {
            T last = data.Get(--N);
            data.RemoveAt(N);
            return last;
        }

        public void Push(T element)
        {
            data.Add(element);
            N++;
        }

        public override string ToString() => this.data.ToString();
    }
}
