namespace KitchenPC
{
    using KitchenPC.NLP;

    public class CouldNotParseUsageException : KPCException
    {
        public CouldNotParseUsageException(Result result, string usage)
        {
            Result = result;
            Usage = usage;
        }

        public Result Result { get; private set; }

        public string Usage { get; private set; }
    }
}