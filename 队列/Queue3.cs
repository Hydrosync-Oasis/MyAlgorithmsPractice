using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 数组;

namespace 队列
{
    /// <summary>
    /// 基于动态循环数组实现的队列，性能较优
    /// </summary>
    /// <typeparam name="T">队列数据类型</typeparam>
    public class Queue3<T> : IQueue<T>
    {
        private Array2<T> data;

        public Queue3()
        {
            data = new();
        }

        public Queue3(T element) : this()
        {
            this.Enqueue(element);
        }

        public int Count => data.Count;

        public bool IsEmpty => data.IsEmpty;

        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            T first = this.data.GetFirst();
            this.data.RemoveFirst();
            return first;
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="element"></param>
        public void Enqueue(T element) => data.Add(element);

        public T Peek() => data.GetFirst();

        public override string ToString() => this.data.ToString();
    }
}
