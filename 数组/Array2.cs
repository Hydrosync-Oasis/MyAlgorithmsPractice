using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 数组 {
    /// <summary>
    /// 循环数组
    /// </summary>
    /// <typeparam name="T">数组数据类型</typeparam>
    public class Array2<T> {
        T[] data;
        public Array2(int nums) {
            data = new T[nums];
        }

        public Array2() : this(4) { }

        int first;//有效数据的开始索引
        int last;//数据末尾索引 + 1
        int N;

        public int Count { get => N; }

        public bool IsEmpty { get => N == 0; }

        public void Add(T element) {
            if (N == data.Length) {
                Largen();
            }

            data[last] = element;
            last = (++last) % data.Length;
            N++;
        }

        public T RemoveFirst() {
            if (N == 0) {
                throw new InvalidOperationException("数组为空");
            }

            T val = data[first];
            data[first] = default;
            first = (++first) % data.Length;
            N--;
            return val;
        }

        public T GetFirst() => this.data[first];

        private void Largen() {
            T[] newData = new T[data.Length * 2];
            int j = 0;
            for (int i = 0; i < N; i++) {
                newData[i] = this.data[(first + i) % data.Length];
            }

            #region AnotherVersion
            //if (first > last || last == 0)
            //{
            //    for (int i = first; i < data.Length; i++)
            //    {
            //        newData[j] = data[i];
            //        j++;
            //    }
            //}
            //else
            //{
            //    for (int i = first; i < last; i++)
            //    {
            //        newData[j] = data[i];
            //        j++;
            //    }
            //} 
            #endregion

            first = 0;
            last = N;//下一位：N - 1 + 1 = N
            this.data = newData;
        }

        public override string ToString() {
            StringBuilder sb = new();
            sb.Append(String.Format("此循环数组基本信息：Count = {0}, Capacity = {1}. 该数组成员如下：\n", N, data.Length));
            for (int i = 0; i < N - 1; i++) {
                sb.Append(data[(first + i) % data.Length].ToString() + ", ");
            }
            sb.Append(data[last - 1]);
            return sb.ToString();
        }
    }
}
