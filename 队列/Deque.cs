using System.Diagnostics;

namespace 队列 {
    [DebuggerTypeProxy(typeof(DequeDebugView<>))]
    [DebuggerDisplay("Count: {Count}, Capacity: {Capacity}")]
    public class Deque<T> : IQueue<T> {
        T?[] data;
        /// <summary>
        /// 此队列元素的实际数量
        /// </summary>
        int N;
        int start = 0;
        int end = 0;

        private class DequeDebugView<T> {
            //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Deque<T> deque;

            public DequeDebugView(Deque<T> deque) {
                this.deque = deque;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public string[] Data {
                get {
                    string[] arr = new string[deque.Count];
                    for (int i = 0; i < arr.Length; i++) {
                        arr[i] = deque.data[i + deque.start]!.ToString()!;
                    }
                    return arr;
                }
            }
        }


        public Deque() : this(16) { }

        public Deque(int capacity) {
            data = new T[capacity];
            N = 0;
        }

        public void PushBack(T? element) {
            if (N == data.Length) {
                SetCapacity();
            }
            data[end] = element;
            end = (end + 1) % data.Length;


            N++;
        }

        public void PushFront(T? element) {
            if (N == data.Length) {
                SetCapacity();
            }
            //先更新start值
            start = (start + data.Length - 1) % data.Length;

            data[start] = element;
            N++;
        }

        private void SetCapacity() {
            T?[] larger;
            larger = new T[data.Length * 2];
            for (int i = 0; i < N; i++) {
                larger[i] = data[(i + start) % data.Length];
            }


            data = larger;
            start = 0;
            end = N;
        }

        public T? PopFront() {
            if (N == 0) {
                throw new InvalidOperationException("此队列元素为空");
            }
            T? tmp = data[start];
            data[start] = default;
            start = (start + 1) % data.Length;

            N--;
            return tmp;
        }

        public T? PopBack() {
            if (N == 0) {
                throw new InvalidOperationException("此队列元素为空");
            }
            end = (data.Length + end - 1) % data.Length;
            T? tmp = data[end];
            data[end] = default;
            N--;
            return tmp;
        }

        public T? GetFront() {
            if (N == 0) {
                throw new InvalidOperationException("此队列元素为空");
            }
            return data[start];
        }

        public T? GetBack() {
            if (N == 0) {
                throw new InvalidOperationException("此队列元素为空");
            }
            return data[(end + data.Length - 1) % data.Length];

        }

        public int Count => N;

        public bool IsEmpty => N == 0;

        public int Capacity => data.Length;

        public void Enqueue(T? element) => PushBack(element);

        public T? Dequeue() => PopFront();

        public T? PeekFront() => GetFront();

        public T? PeekBack() => GetBack();


    }
}
