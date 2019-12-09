namespace AEMG_EX.Core
{
    public interface ICopy<T>
    {
        /// <summary>
        /// Copy from source onto self
        /// </summary>
        /// <param name="source"></param>
        void CopyOntoSelf(T source);
    }
}
