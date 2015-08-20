using System;

namespace KitchenPC
{
    public class Amount : IEquatable<Amount>
    {
        public Single? SizeLow;

        public Single SizeHigh;

        public Units Unit;

        /// <summary>Attempts to find a more suitable unit for this amount.</summary>
        public static Amount Normalize(Amount amount, float multiplier)
        {
            var result = new Amount(amount) * multiplier;
            var sizeLow = result.SizeLow.GetValueOrDefault();
            var sizeHigh = result.SizeHigh;

            if (KitchenPC.Unit.GetConvType(result.Unit) == UnitType.Weight)
            {
                if (result.Unit == Units.Ounce && (sizeLow % 16 + sizeHigh % 16) == 0)
                {
                    result /= 16;
                    result.Unit = Units.Pound;
                }
            }

            if (KitchenPC.Unit.GetConvType(result.Unit) == UnitType.Volume)
            {
                //If teaspoons, convert to Tlb (3tsp in 1Tbl)
                if (result.Unit == Units.Teaspoon && (sizeLow % 3 + sizeHigh % 3) == 0)
                {
                    result /= 3;
                    result.Unit = Units.Tablespoon;
                }

                //If Fl Oz, convert to cup (8 fl oz in 1 cup)
                if (result.Unit == Units.FluidOunce && (sizeLow % 8 + sizeHigh % 8) == 0)
                {
                    result /= 8;
                    result.Unit = Units.Cup;
                }

                //If pints, convert to quarts (2 pints in a quart)
                if (result.Unit == Units.Pint && (sizeLow % 2 + sizeHigh % 2) == 0)
                {
                    result /= 2;
                    result.Unit = Units.Quart;
                }

                //If quarts, convert to gallons (4 quarts in a gallon)
                if (result.Unit == Units.Quart && (sizeLow % 4 + sizeHigh % 4) == 0)
                {
                    result /= 4;
                    result.Unit = Units.Gallon;
                }
            }

            return result;
        }

        public Amount(Single size, Units unit)
        {
            SizeHigh = size;
            Unit = unit;
        }

        public Amount(Single? receivedSize, Single receivingSize, Units unit)
        {
            SizeLow = receivedSize;
            SizeHigh = receivingSize;
            Unit = unit;
        }

        public Amount(Amount amount)
        {
            SizeLow = amount.SizeLow;
            SizeHigh = amount.SizeHigh;
            Unit = amount.Unit;
        }

        public Amount()
            : this(0, Units.Unit)
        {
        }

        public static Amount operator *(Amount amount, float coefficient)
        {
            return new Amount(amount.SizeLow * coefficient, amount.SizeHigh * coefficient, amount.Unit);
        }

        public static Amount operator /(Amount amount, float denominator)
        {
            return new Amount(amount.SizeLow / denominator, amount.SizeHigh / denominator, amount.Unit);
        }

        public static Amount operator +(Amount amount, float operand)
        {
            return new Amount(amount.SizeLow + operand, amount.SizeHigh + operand, amount.Unit);
        }

        public static Amount operator -(Amount amount, float operand)
        {
            return new Amount(amount.SizeLow - operand, amount.SizeHigh - operand, amount.Unit);
        }

        public static Amount operator +(Amount firstAmount, Amount secondAmount)
        {
            if (firstAmount.Unit == secondAmount.Unit) //Just add
            {
                if (firstAmount.SizeLow.HasValue && secondAmount.SizeLow.HasValue)
                {
                    //Combine the lows, combine the highs
                    return new Amount(
                        firstAmount.SizeLow + secondAmount.SizeLow,
                        firstAmount.SizeHigh + secondAmount.SizeHigh,
                        firstAmount.Unit);
                }
                
                if (firstAmount.SizeLow.HasValue)
                {
                    //(1-2) + 1 = (2-3)
                    return new Amount(
                        firstAmount.SizeLow + secondAmount.SizeHigh,
                        firstAmount.SizeHigh + secondAmount.SizeHigh,
                        firstAmount.Unit);
                }
                
                if (secondAmount.SizeLow.HasValue)
                {
                    //1 + (1-2) = (2-3)
                    return new Amount(
                        firstAmount.SizeHigh + secondAmount.SizeLow,
                        firstAmount.SizeHigh + secondAmount.SizeHigh,
                        firstAmount.Unit);
                }

                //just combine the highs
                return new Amount(firstAmount.SizeHigh + secondAmount.SizeHigh, firstAmount.Unit);
            }

            if (UnitConverter.CanConvert(firstAmount.Unit, secondAmount.Unit))
            {
                //TODO: Handle range + nonrange
                var newLow = secondAmount.SizeLow.HasValue
                                 ? (float?)UnitConverter.Convert(secondAmount.SizeLow.Value, secondAmount.Unit, firstAmount.Unit)
                                 : null;
                var newHigh = firstAmount.SizeHigh + UnitConverter.Convert(secondAmount.SizeHigh, secondAmount.Unit, firstAmount.Unit);
                return new Amount(newLow, newHigh, firstAmount.Unit);
            }

            throw new IncompatibleAmountException();
        }

        public Amount Round(int decimalPlaces)
        {
            var roundedResult = new Amount(this)
                             {
                                 SizeLow =
                                     this.SizeLow.HasValue
                                         ? (float?)Math.Round(this.SizeLow.Value, decimalPlaces)
                                         : null,
                                 SizeHigh = (float)Math.Round(this.SizeHigh, decimalPlaces)
                             };

            return roundedResult;
        }

        public Amount RoundUp(Single nearestMultiple)
        {
            var roundedUpResult = new Amount(this)
                                      {
                                          SizeLow =
                                              this.SizeLow.HasValue
                                                  ? (float?)
                                                    (Math.Ceiling(this.SizeLow.Value / nearestMultiple)
                                                     * nearestMultiple)
                                                  : null,
                                          SizeHigh =
                                              (float)Math.Ceiling(this.SizeHigh / nearestMultiple)
                                              * nearestMultiple
                                      };

            return roundedUpResult;
        }

        public override string ToString()
        {
            return
                ToString(
                    (SizeLow.HasValue || SizeHigh > 1)
                        ? KitchenPC.Unit.GetPlural(this.Unit)
                        : KitchenPC.Unit.GetSingular(this.Unit));
        }

        public string ToString(string unit)
        {
            string sizeHigh;
            string sizeLow;

            if (KitchenPC.Unit.GetConvType(Unit) == UnitType.Weight) // Render in decimal
            {
                sizeHigh = Math.Round(SizeHigh, 2).ToString();
                sizeLow = SizeLow.HasValue ? Math.Round(SizeLow.Value, 2).ToString() : null;
            }
            else // Render in fractions
            {
                sizeHigh = Fractions.FromDecimal((decimal)SizeHigh);
                sizeLow = SizeLow.HasValue ? Fractions.FromDecimal((decimal)SizeLow.Value) : null;
            }

            var amount = (sizeLow != null) ? String.Format("{0} - {1}", sizeLow, sizeHigh) : sizeHigh;
            return String.Format("{0} {1}", amount, unit).Trim();
        }

        public bool Equals(Amount other)
        {
            if (other is Amount)
            {
                //Check for null
                return (other.SizeLow == SizeLow && other.SizeHigh == SizeHigh && other.Unit == Unit);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Amount;
            if (other != null)
            {
                return this.Equals(other);
            }

            return false;
        }

        public static bool operator ==(Amount x, Amount y)
        {
            if (x is Amount)
            {
                return x.Equals(y);
            }

            return !(y is Amount);
        }

        public static bool operator !=(Amount x, Amount y)
        {
            if (x is Amount)
            {
                return !x.Equals(y);
            }

            return y is Amount;
        }

        public override int GetHashCode()
        {
            return SizeLow.GetHashCode() ^ SizeHigh.GetHashCode();
        }
    }
}