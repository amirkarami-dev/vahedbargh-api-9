using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Common.Utility
{
    public static class CollectionHelper
    {
        public static IEnumerable<T> AsOneList<T>(this IEnumerable<IEnumerable<T>> nestedList)
        {
            foreach(var list in nestedList)
                foreach (var item in list)
                    yield return item;
        }
    }
}
