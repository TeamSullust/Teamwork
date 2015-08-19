using System.Collections.Generic;

namespace KitchenPC.NLP.Interfaces
{
   public interface ISynonymLoader<T>
   {
      IEnumerable<T> LoadSynonyms();
      Pairings LoadFormPairings(); //Default pairing data for forms of certain ingredients
   }
}