namespace KitchenPC.Recipes
{
    using System;

    public class RecipeBrief
    {
        private Uri recipeimg;

        public RecipeBrief()
        {
        }

        public RecipeBrief(Recipe r)
        {
            this.Id = r.Id;
            this.OwnerId = r.OwnerId;
            this.Title = r.Title;
            this.Description = r.Description;
            this.ImageUrl = r.ImageUrl;
            this.Author = r.OwnerAlias;
            this.PrepTime = r.PrepTime;
            this.CookTime = r.CookTime;
            this.AvgRating = r.AvgRating;
        }

        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public string Permalink { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public short PrepTime { get; set; }

        public short CookTime { get; set; }

        public short AvgRating { get; set; }

        public string ImageUrl
        {
            get
            {
                return this.recipeimg == null ? "/Images/img_placeholder.png" : this.recipeimg.ToString();
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.recipeimg = null;
                    return;
                }

                var builder = new UriBuilder { Path = "Thumb_" + value };
                this.recipeimg = builder.Uri;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Title, this.Id);
        }
    }
}