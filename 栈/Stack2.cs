using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 链表;

namespace 栈 {
    public class Stack2<T> : IStack<T> {
        LinkedList1<T> data;
        public int Count => data.Count;

        public bool IsEmpty() => data.Count == 0;

        public T Peek() => data.GetLast();

        public T Pop() {
            T last = this.Peek();
            data.RemoveAt(data.Count - 1);
            return last;
        }

        public void Push(T element) {
            data.AddLast(element);
        }

        public Stack2(params T[] values) {
            if (values == null || values.Length == 0) {
                this.data = new LinkedList1<T>();
            }
        }

        public override string ToString() => this.data.ToString();
    }
}
