namespace KitchenPC
{
    using System;

    public class UnknownTokenException : Exception
    {
        public string token { get; private set; }

        public UnknownTokenException(string token)
        {
            this.token = token;
        }
    }
}