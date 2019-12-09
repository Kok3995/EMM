using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EMM.Core.Converter
{
    /// <summary>
    /// This class is used to map properties of one object to another
    /// </summary>
    public class SimpleAutoMapper
    {
        /// <summary>
        /// Methods to map property with the same name from one class to another
        /// </summary>
        /// <typeparam name="T">source's type</typeparam>
        /// <typeparam name="TU">destination's type</typeparam>
        /// <param name="source">the source to convert from</param>
        /// <returns></returns>
        public TU SimpleAutoMap<T, TU>(T source) where T : class where TU : class, new()
        {
            return SimpleAutoMap<T, TU>(source, ignoreTypes:null);
        }

        /// <summary>
        /// Methods to map property with the same name from one class to another, with ignore type
        /// </summary>
        /// <typeparam name="T">source's type</typeparam>
        /// <typeparam name="TU">destination's type</typeparam>
        /// <param name="source">the source to convert from</param>
        /// <param name="ignoreTypes">list of <see cref="Type"/> to ignore</param>
        /// <returns></returns>
        public TU SimpleAutoMap<T, TU>(T source, IList<Type> ignoreTypes) where T : class where TU : class, new()
        {
            TU destination = new TU();
            
            this.SimpleAutoMap(source, destination, ignoreTypes);

            return destination;
        }

        /// <summary>
        /// Methods to map property with the same name from one class to another, destination class already existed
        /// </summary>
        /// <typeparam name="T">source's type</typeparam>
        /// <typeparam name="TU">destination's type</typeparam>
        /// <param name="source">the source to convert from</param>
        /// <param name="destination">the destination to convert to</param>
        /// <returns></returns>
        public void SimpleAutoMap<T, TU>(T source, TU destination) where T : class where TU : class
        {
            this.SimpleAutoMap<T, TU>(source, destination, null);
        }

        /// <summary>
        /// Methods to map property with the same name from one class to another, destination class already existed, ignore type
        /// </summary>
        /// <typeparam name="T">source's type</typeparam>
        /// <typeparam name="TU">destination's type</typeparam>
        /// <param name="source">the source to convert from</param>
        /// <param name="destination">the destination to convert to</param>
        /// <returns></returns>
        public void SimpleAutoMap<T, TU>(T source, TU destination, IList<Type> ignoreTypes) where T : class where TU : class
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            PropertyInfo[] destinationPropertyInfos = typeof(TU).GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                //Check if setter exist
                if (propertyInfo.GetSetMethod() == null)
                    continue;

                if (ignoreTypes?.Contains(propertyInfo.PropertyType) == true)
                    continue;
                try
                {
                    if (!destinationPropertyInfos.Where(i => i.Name.Equals(propertyInfo.Name)).Any())
                        continue;

                    typeof(TU).GetProperty(propertyInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
                        .SetValue(destination, propertyInfo.GetValue(source));
                }
                catch { /*Ignore errors.... */ }
            }
        }
    }
}
