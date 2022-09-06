using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 数组;

namespace 队列
{
    /// <summary>
    /// 基于动态数组实现的队列，性能较慢
    /// </summary>
    /// <typeparam name="T">队列数据类型</typeparam>
    public class Queue1<T> : IQueue<T>
    {
        private Array1<T> data;

        public Queue1()
        {
            data = new();
        }

        public Queue1(T element):this()
        {
            this.Enqueue(element);
        }

        public int Count => data.Count;

        public bool IsEmpty => data.IsEmpty;

        public T Dequeue()
        {
            T first = this.data.Get(0);
            this.data.RemoveAt(0);
            return first;
        }

        public void Enqueue(T element) => data.Add(element);

        public T Peek() => data.Get(0);

        public override string ToString() => this.data.ToString();
    }
}
