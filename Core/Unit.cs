using System;

namespace KitchenPC
{
    public static class Unit
    {
        private static readonly string[] Singular =
            {
                string.Empty, "tsp", "Tbl", "fl oz", "cup", "pt", "qt", "gal", "g",
                "oz", "lb"
            };

        private static readonly string[] Plural =
            {
                string.Empty, "tsp", "Tbl", "fl oz", "cups", "pts", "qts", "gal",
                "g", "oz", "lbs"
            };

        public static string GetSingular(Units unitType)
        {
            return Singular[(int)unitType];
        }

        public static string GetPlural(Units unitType)
        {
            return Plural[(int)unitType];
        }

        public static UnitType GetConvType(Units unitType)
        {
            switch (unitType)
            {
                case Units.Unit:
                    return UnitType.Unit;
                case Units.Teaspoon:
                case Units.Tablespoon:
                case Units.FluidOunce:
                case Units.Cup:
                case Units.Pint:
                case Units.Quart:
                case Units.Gallon:
                    return UnitType.Volume;
                case Units.Gram:
                case Units.Ounce:
                case Units.Pound:
                    return UnitType.Weight;
            }

            throw new ArgumentException("Invalid unitType specified.");
        }

        public static Units GetDefaultUnitType(UnitType convType)
        {
            switch (convType)
            {
                case UnitType.Unit:
                    return Units.Unit;
                case UnitType.Volume:
                    return Units.Cup;
                case UnitType.Weight:
                    return Units.Ounce;
            }

            throw new ArgumentException("Invalid convType passed in to GetDefaultUnitType.");
        }

        public static T? ParseNullable<T>(object value) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an Enum type.");
            }

            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            if (value is string)
            {
                return Enum.Parse(typeof(T), value.ToString()) as T?;
            }

            return Enum.ToObject(typeof(T), value) as T?;
        }

        public static T Parse<T>(object value) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an Enum type.");
            }

            if (value == null || value == DBNull.Value)
            {
                throw new ArgumentException("Cannot parse enum, value is null.");
            }

            if (value is string)
            {
                return (T)Enum.Parse(typeof(T), value.ToString());
            }

            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}