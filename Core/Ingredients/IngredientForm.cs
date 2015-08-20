using System;

namespace KitchenPC.Ingredients
{
    public class IngredientForm
    {
        private Guid formId;

        private Guid ingredientId;

        private Units formUnitType;

        private string formDisplayName;

        private string formUnitName;

        private int conversionMultiplier;

        private Amount formAmount;

        public static IngredientForm FromId(Guid id)
        {
            return new IngredientForm { FormId = id };
        }

        public IngredientForm()
        {
        }

        public IngredientForm(
            Guid formid,
            Guid ingredientid,
            Units unittype,
            string displayname,
            string unitname,
            int convmultiplier,
            Amount amount)
        {
            FormId = formid;
            IngredientId = ingredientid;
            FormUnitType = unittype;
            FormDisplayName = displayname;
            FormUnitName = unitname;
            ConversionMultiplier = convmultiplier;
            FormAmount = amount;
        }

        // PROPERTY ZONE

        public Guid FormId
        {
            get
            {
                return this.formId;
            }

            set
            {
                this.formId = value;
            }
        }

        public Guid IngredientId
        {
            get
            {
                return this.ingredientId;
            }

            set
            {
                this.ingredientId = value;
            }
        }

        public Units FormUnitType
        {
            get
            {
                return this.formUnitType;
            }

            set
            {
                this.formUnitType = value;
            }
        }

        public string FormDisplayName
        {
            get
            {
                return this.formDisplayName;
            }

            set
            {
                this.formDisplayName = value;
            }
        }

        public string FormUnitName
        {
            get
            {
                return this.formUnitName;
            }

            set
            {
                this.formUnitName = value;
            }
        }

        public int ConversionMultiplier
        {
            get
            {
                return this.conversionMultiplier;
            }

            set
            {
                this.conversionMultiplier = value;
            }
        }

        public Amount FormAmount
        {
            get
            {
                return this.formAmount;
            }

            set
            {
                this.formAmount = value;
            }
        }


        public override string ToString()
        {
            return FormId.ToString();
        }
    }
}