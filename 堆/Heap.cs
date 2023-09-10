using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace 堆 {
    public class Heap<Element, Priority> where Priority : notnull where Element : notnull {
        Dictionary<Element, int> mapEI = new();
        Dictionary<int, Element> mapIE = new();
        /// <summary>
        /// 存放数据的堆是以priority进行比较的
        /// </summary>
        List<Priority> data = new();
        int size = 0;
        /// <summary>
        /// 此条件大于零则进行堆化
        /// </summary>
        Comparison<Priority> method;

        /// <summary>
        /// 初始化一个堆，并使用用户指定的比较器
        /// </summary>
        /// <param name="compareMethod">指定的比较器</param>
        public Heap(Comparison<Priority> compareMethod) : this(16, compareMethod) { }

        /// <summary>
        /// 初始化一个有指定预留空间的堆，并使用用户指定的比较器
        /// </summary>
        /// <param name="size">堆的预留大小</param>
        /// <param name="compareMethod"></param>
        public Heap(int size, Comparison<Priority> compareMethod) {
            data = new(size);
            method = compareMethod;
        }

        public int Count => size;

        public bool Contains(Element element) {
            return mapEI.ContainsKey(element);
        }

        public Element Peek() => mapIE[0];

        public Priority this[Element element] {
            set {
                int idx = mapEI[element];
                data[idx] = value;
                Heapify(idx);
                Floating(idx);
            }
            get => data[mapEI[element]];
        }

        public bool Push(Element element, Priority priority) {
            if (mapEI.ContainsKey(element)) {//有了就不添加
                this[element] = priority;
                return false;
            } else {
                AddToMap(element, priority);//经过List.Add()后可以直接使用size，此时size为索引
                size++;
                return true;
            }
        }

        public void PushRange(IList<Element> elements, IList<Priority> priorities) {
            if (elements.Count != priorities.Count) {
                throw new ArgumentException("两个参数的长度不匹配");
            }
            int preSize = size;
            for (int i = 0; i < elements.Count; i++) {
                AddToMap(elements[i], priorities[i]);
                size++;
            }
            for (int i = size - 1; i >= preSize; i--) {
                Heapify(i);
            }
        }

        public Element Pop() {
            if (size == 0) {
                throw new InvalidOperationException("堆为空");
            } else {
                var tmp = mapIE[0];
                Swap(--size, 0);
                Heapify(0);
                return tmp;
            }
        }

        public void Remove(Element element) {
            if (!mapEI.ContainsKey(element)) {
                throw new InvalidOperationException("元素不存在");
            } else {
                Swap(--size, mapEI[element]);
                Heapify(mapEI[element]);
            }
        }

        /// <summary>
        /// 向字典和data中添加映射，但不增加size
        /// </summary>
        /// <param name="e"></param>
        /// <param name="p"></param>
        private void AddToMap(Element e, Priority p) {
            mapEI.Add(e, size);
            mapIE.Add(size, e);
            data.Add(p);
        }

        /// <summary>
        /// 类的内部认为此堆为大根堆
        /// </summary>
        /// <param name="idx"></param>
        private void Floating(int idx) {
            int father = (idx - 1) / 2;
            while (father >= 0) {
                if (method(data[idx], data[father]) > 0) {
                    Swap(idx, father);
                } else {
                    break;
                }
                idx = father;
                father = (idx - 1) / 2;
            }
        }

        /// <summary>
        /// 请在调用此方法前保证size正确
        /// </summary>
        /// <param name="idx"></param>
        private void Heapify(int idx) {
            int l = (idx << 1) + 1;
            while (l < size) {
                int r = l + 1;
                int choosen = r < size && method(data[r], data[l]) > 0 ? r : l;
                if (method(data[choosen], data[idx]) > 0) {
                    Swap(choosen, idx);
                } else {
                    break;
                }
                idx = choosen;
                l = (idx << 1) + 1;
            }
        }

        private void Swap(int idx1, int idx2) {
            (data[idx1], data[idx2]) = (data[idx2], data[idx1]);

            (mapIE[idx1], mapIE[idx2]) = (mapIE[idx2], mapIE[idx1]);
            Element v1 = mapIE[idx1], v2 = mapIE[idx2];
            (mapEI[v1], mapEI[v2]) = (mapEI[v2], mapEI[v1]);
        }

        public Dictionary<Element, Priority> ToDictionary() {
            Dictionary<Element, Priority> dic = new();
            foreach (var item in mapEI) {
                dic[item.Key] = data[mapEI[item.Key]];
            }
            return dic;
        }
    }
}
