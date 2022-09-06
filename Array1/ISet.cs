using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 数组
{
    public interface ISet<T>
    {
        bool IsEmpty { get; }

        int Count { get; }

        void Add(T element);

        void Remove(T element);

        bool Contais(T element); 

    }
}
