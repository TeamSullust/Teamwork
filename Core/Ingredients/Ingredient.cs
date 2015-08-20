using System;

namespace KitchenPC.Ingredients
{
    public class Ingredient
    {
        private Guid id;

        private string name;

        private UnitType conversionType;

        private string unitName;

        private Weight unitWeight;

        private IngredientMetadata metadata;

        public static Ingredient FromId(Guid ingredientId)
        {
            return new Ingredient { Id = ingredientId };
        }

        public Ingredient(Guid id, string name)
        {
            Id = id;
            Name = name;
            Metadata = new IngredientMetadata();
        }

        public Ingredient(Guid id, string name, IngredientMetadata metadata)
        {
            Id = id;
            Name = name;
            Metadata = metadata;
        }

        public Ingredient()
            : this(Guid.Empty, string.Empty)
        {
        }

        //PROPERTY ZONE

        public Guid Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        public UnitType ConversionType
        {
            get
            {
                return this.conversionType;
            }

            set
            {
                this.conversionType = value;
            }
        }

        public string UnitName
        {
            get
            {
                return this.unitName;
            }

            set
            {
                this.unitName = value;
            }
        }

        public Weight UnitWeight
        {
            get
            {
                return this.unitWeight;
            }

            set
            {
                this.unitWeight = value;
            }
        }

        public IngredientMetadata Metadata
        {
            get
            {
                return this.metadata;
            }

            set
            {
                this.metadata = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            var ingredient = obj as Ingredient;
            return (ingredient != null && this.Id == ingredient.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}