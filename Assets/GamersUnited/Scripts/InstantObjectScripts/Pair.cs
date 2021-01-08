using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public struct Pair<T, U>
    {
        public T first;
        public U second;
        public Pair(T first,U second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
