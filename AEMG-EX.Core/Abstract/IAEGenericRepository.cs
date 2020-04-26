using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEMG_EX.Core
{
    public interface IAEGenericRepository<T> where T : class
    {
        /// <summary>
        /// Find all items
        /// </summary>
        /// <returns></returns>
        List<T> FindAll();

        /// <summary>
        /// Add to collection
        /// </summary>
        /// <param name="item"></param>
        void Add(T item);

        /// <summary>
        /// Remove the collection
        /// </summary>
        /// <param name="item"></param>
        void Remove(T item);

        /// <summary>
        /// Persist the change
        /// </summary>
        bool SaveChange();
    }
}
