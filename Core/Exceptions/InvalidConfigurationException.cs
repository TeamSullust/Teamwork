namespace KitchenPC
{
    public class InvalidConfigurationException : KPCException
    {
        public InvalidConfigurationException()
        {
        }

        public InvalidConfigurationException(string message)
            : base(message)
        {
        }
    }
}