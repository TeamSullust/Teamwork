namespace KitchenPC.Recipes
{
    public class SearchResults
    {
        public SearchResults(RecipeBrief[] briefs, long total)
        {
            this.Briefs = briefs;
            this.TotalCount = total;
        }

        public SearchResults()
        {
        }

        public RecipeBrief[] Briefs { get; set; }

        public long TotalCount { get; set; }
    }
}