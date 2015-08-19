namespace KitchenPC.Recipes
{
    public class ReadRecipeOptions
    {
        public ReadRecipeOptions(bool returnCommentCount, bool returnUserRating, bool returnCookbookStatus, bool returnMethod, bool returnPermalink)
        {
            this.ReturnCommentCount = returnCommentCount;
            this.ReturnUserRating = returnUserRating;
            this.ReturnCookbookStatus = returnCookbookStatus;
            this.ReturnMethod = returnMethod;
            this.ReturnPermalink = returnPermalink;
        }

        public ReadRecipeOptions()
        {
        }

        public bool ReturnCommentCount { get; set; }

        public bool ReturnUserRating { get; set; }

        public bool ReturnCookbookStatus { get; set; }

        public bool ReturnMethod { get; set; }

        public bool ReturnPermalink { get; set; }
    }
}