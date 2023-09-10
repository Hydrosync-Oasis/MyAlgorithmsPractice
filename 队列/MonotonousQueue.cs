using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 队列 {
    public class MonotonousQueue<T> where T : IComparable {
        Deque<T> data;

        public MonotonousQueue() {
            data = new();
        }

        public void Push(T element) {
            //5 3 1 <- 2:      5 3         5 3 2
            while (!data.IsEmpty && data.GetBack().CompareTo(element) <= 0) {
                data.PopBack();
            }

            data.PushBack(element);
        }

        public bool Pop(T element) {
            bool ret = !data.IsEmpty && data.GetFront().CompareTo(element) == 0;
            if (ret) {
                data.PopFront();
            }
            return ret;
        }

        public T? MaxValue => data.GetFront();
    }
}
