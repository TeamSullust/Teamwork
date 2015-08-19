namespace KitchenPC.NLP.Interfaces
{
   public interface ITracer
   {
      void Trace(TraceLevel level, string message, params object[] args);
   }
}