namespace KitchenPC
{
    using System;

    public class DataLoadException : Exception
    {
        public DataLoadException(string message)
            : base(message)
        {
        }

        public DataLoadException(Exception inner)
            : base(inner.Message, inner)
        {
        }
    }
}