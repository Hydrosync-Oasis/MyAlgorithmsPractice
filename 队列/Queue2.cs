using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 链表;

namespace 队列 {
    /// <summary>
    /// 基于链表实现的队列
    /// </summary>
    /// <typeparam name="T">队列数据类型</typeparam>
    public class Queue2<T> : IQueue<T> {
        LinkedList1<T> data;

        public Queue2() {
            this.data = new();
        }

        public Queue2(T element) {
            data = new(element);
        }

        public int Count => data.Count;

        public bool IsEmpty => data.IsEmpty;

        public T Dequeue() {
            T first = this.data.GetFirst();
            this.data.RemoveFirst();
            return first;
        }

        public void Enqueue(T element) => this.data.AddLast(element);

        public T PeekFront() => this.data.GetFirst();
    }
}
