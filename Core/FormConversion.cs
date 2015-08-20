using System;
using KitchenPC.Ingredients;

namespace KitchenPC
{
    public static class FormConversion
    {
        public static Amount GetWeightForUsage(IngredientUsage usage)
        {
            if (Unit.GetConvType(usage.Form.FormUnitType) == UnitType.Weight)
            {
                // Already there, just convert to grams
                return UnitConverter.Convert(usage.Amount, Units.Gram);
            }

            if (usage.Ingredient.ConversionType == UnitType.Weight)
            {
                // Ingredient is sold in weight, so we can use its native amount
                var amt = GetNativeAmountForUsage(usage.Ingredient, usage);
                return UnitConverter.Convert(amt, Units.Gram);
            }

            if (usage.Ingredient.ConversionType == UnitType.Unit && usage.Ingredient.UnitWeight > 0)
            {
                // Ingredient sold in units, but we know weight of each
                var amount = GetNativeAmountForUsage(usage.Ingredient, usage);
                amount.Unit = Units.Gram;
                amount *= usage.Ingredient.UnitWeight;

                return amount;
            }

            if (Unit.GetConvType(usage.Form.FormAmount.Unit) == UnitType.Weight && usage.Form.FormAmount.SizeHigh > 0)
            {
                // This form has a gram weight
                var amount = UnitConverter.Convert(usage.Amount, usage.Form.FormUnitType);
                return new Amount(amount.SizeHigh * usage.Form.FormAmount.SizeHigh, Units.Gram);
            }

            return null;
        }

        public static Amount GetNativeAmountForUsage(Ingredient ingredient, IngredientUsage usage)
        {
            var amount = new Amount();
            var usageConvType = Unit.GetConvType(usage.Form.FormUnitType);

            switch (ingredient.ConversionType)
            {
                // This is the type we must convert to
                case UnitType.Unit:
                    amount.Unit = Units.Unit;

                    switch (usageConvType)
                    {
                        case UnitType.Unit:
                            // Unit to unit version
                            var equivGrams = UnitConverter.Convert(usage.Form.FormAmount, Units.Gram);

                            // Grams this form is equivelent to
                            amount.SizeHigh =
                                (float)Math.Ceiling((equivGrams.SizeHigh * usage.Amount.SizeHigh) / ingredient.UnitWeight);
                            
                            return amount;
                        case UnitType.Weight:
                            var grams = UnitConverter.Convert(usage.Amount, Units.Gram);
                            amount.SizeHigh = (float)Math.Ceiling(grams.SizeHigh / ingredient.UnitWeight); 
                            
                            return amount;
                        case UnitType.Volume:
                            var likeAmount = UnitConverter.Convert(usage.Amount, usage.Form.FormUnitType);
                            amount.SizeHigh =
                                (float)
                                Math.Ceiling(
                                    (likeAmount.SizeHigh * usage.Form.FormAmount.SizeHigh) / usage.Ingredient.UnitWeight);
                            
                            // Round up when dealing with whole units
                            return amount;
                    }

                    break;

                case UnitType.Weight:
                    amount.Unit = Units.Gram;

                    switch (usageConvType)
                    {
                        case UnitType.Unit:
                            amount.SizeHigh = usage.Amount.SizeHigh * usage.Form.FormAmount.SizeHigh;
                            
                            //NOTE: FormAmount will always be in Grams when Ingredient ConvType is weight
                            return amount;
                        case UnitType.Volume:
                            var likeAmount = UnitConverter.Convert(usage.Amount, usage.Form.FormUnitType);
                            amount.SizeHigh = likeAmount.SizeHigh * usage.Form.FormAmount.SizeHigh;
                            
                            //NOTE: FormAmount will always be in Grams when Ingredient ConvType is weight
                            return amount;
                    }

                    break;
                case UnitType.Volume:
                    amount.Unit = Units.Teaspoon;

                    switch (usageConvType)
                    {
                        case UnitType.Unit:
                            amount.SizeHigh = usage.Amount.SizeHigh * usage.Form.FormAmount.SizeHigh;
                            
                            //NOTE: FormAmount will always be in tsp when Ingredient ConvType is volume
                            return amount;
                        case UnitType.Weight:
                            var likeAmount = UnitConverter.Convert(usage.Amount, usage.Form.FormUnitType);
                            amount.SizeHigh = likeAmount.SizeHigh * usage.Form.FormAmount.SizeHigh;
                            
                            //NOTE: FormAmount will always be in teaspoons when Ingredient ConvType is Volume
                            return amount;
                    }

                    break;
            }

            //throw new IngredientAggregationDatabaseException("Cannot convert an IngredientUsage into its native form.", ingredient, usage);
            throw new Exception("Cannot convert an IngredientUsage into its native form.");
        }
    }
}