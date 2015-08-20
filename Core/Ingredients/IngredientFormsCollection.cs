using System.Collections.Generic;

namespace KitchenPC.Ingredients
{
    public class IngredientFormsCollection
    {
        private readonly List<IngredientForm> forms;

        public IngredientForm[] Forms
        {
            get
            {
                return this.forms.ToArray();
            }

            set
            {
                this.forms.Clear();
                foreach (var form in value)
                {
                    this.forms.Add(form);
                }
            }
        }

        public IngredientFormsCollection()
        {
            this.forms = new List<IngredientForm>();
        }

        public IngredientFormsCollection(IEnumerable<IngredientForm> forms)
        {
            this.forms = new List<IngredientForm>(forms);
        }

        public void AddForm(IngredientForm form)
        {
            this.forms.Add(form);
        }
    }
}