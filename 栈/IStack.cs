using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 栈 {
    public interface IStack<T> {
        void Push(T element);

        T Pop();

        int Count { get; }

        T Peek();

        bool IsEmpty();
    }
}
