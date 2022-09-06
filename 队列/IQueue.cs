using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 队列
{
    /// <summary>
    /// 用于自定义多种原理不同的队列
    /// </summary>
    /// <typeparam name="T">队列数据类型</typeparam>
    public interface IQueue<T>
    {
        void Enqueue(T element);

        T Dequeue();

        T Peek();

        int Count { get; }

        bool IsEmpty { get; }
    }
}
