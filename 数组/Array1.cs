using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace 数组 {
    public class Array1<Type> {
        private Type[] data;//可以填不满
        private int N = 0;//实际存储的数据数量

        public bool IsEmpty {
            get => N == 0;
        }

        public int Count {
            get => N;
        }

        public int Capacity {
            get => data.Length;
        }//无需维护

        public Array1(int capacity) {
            data = new Type[capacity];
            //N = capacity;
        }

        public Array1() : this(4) {

        }

        public void Add(Type element) {
            if (N < data.Length) {
                data[N++] = element;
            } else {
                Largen();
                data[N++] = element;
            }
        }

        public void Insert(Type element, int index) {//考虑数组长度导致的无法添加情况
            if (index > N || index < 0) {
                throw new ArgumentOutOfRangeException(nameof(index), index, "最小值只能为0，最大值只能是数组索引最大值+1。");
            }
            if (data.Length == N) {
                Largen();
            }
            for (int i = N; i > index; i--) {
                data[i] = data[i - 1];
            }
            data[index] = element;
            N++;
        }
        /// <summary>
        /// 注意：请在实际容量和容量相同时使用！
        /// </summary>
        private void Largen() {
            Type[] newData = new Type[N * 2];
            data.CopyTo(newData, 0);
            data = newData;
        }

        public Type Get(int index) {
            if (index >= N || index < 0) {
                throw new ArgumentOutOfRangeException(nameof(index), index, "最小值只能为0，最大值只能是数组索引最大值。");
            }
            return this.data[index];
        }

        public Type this[int index] {
            get {
                return this.Get(index);
            }
            set {
                this.Set(index, value);
            }
        }

        public void Set(int index, Type value) {
            if (index >= N || index < 0) {
                throw new ArgumentOutOfRangeException(nameof(index), index, "最小值只能为0，最大值只能是数组索引最大值。");
            }
            this.data[index] = value;
        }

        public void RemoveAll() {
            data = new Type[4];
            N = 0;
        }

        public void RemoveAt(int index) {
            if (index > N || index < 0) {
                throw new ArgumentOutOfRangeException(nameof(index), index, "最小值只能为0，最大值只能是数组索引最大值+1。");
            }
            for (int i = index; i < N - 1; i++) {
                data[i] = data[i + 1];
            }
            data[--N] = default;/*   等效于：
            data[N-1] = default;     当前最右边值释放               
            N--;                     更新N值
                                 */
        }

        public void AddFirst(Type element) {
            this.Insert(element, 0);
        }

        public override string ToString() {
            StringBuilder sb = new();
            sb.Append(String.Format("本数组基本信息：Count = {0}, Capacity = {1}. 成员列表：\n", this.Count, this.Capacity));
            for (int i = 0; i < N - 1; i++) {
                sb.Append(data[i] + ", ");
            }
            sb.Append(data[N - 1]);
            return sb.ToString();
        }

        public bool Contains(Type element) {

            for (int i = 0; i < N; i++) {
                if (data[i].Equals(element)) {
                    return true;
                }
            }
            return false;
        }

        public int IndexOf(Type element) {
            for (int i = 0; i < N; i++) {
                if (data[i].Equals(element)) {
                    return i;
                }
            }
            return -1;
        }
    }
}
