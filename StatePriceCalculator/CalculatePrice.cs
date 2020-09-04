using System;
using System.Collections.Generic;
using System.Text;

namespace StatePriceCalculator
{
    public static class CalculatePrice
    {
        public static int ItemPrice = 500;
        public static double PriceAfterDiscount = 0;
        public static bool IsStateCodeWrong = false;

        public static double GetDiscountRate(double NrOfItems)
        {
            double TotaltPrice = NrOfItems * ItemPrice;

            if (TotaltPrice >= 1000 && TotaltPrice < 5000)
                TotaltPrice = TotaltPrice / 1.03;

            if (TotaltPrice >= 5000 && TotaltPrice < 7000)
                TotaltPrice = TotaltPrice / 1.05;

            if (TotaltPrice >= 7000 && TotaltPrice < 10000)
                TotaltPrice = TotaltPrice / 1.07;

            if (TotaltPrice >= 10000 && TotaltPrice < 50000)
                TotaltPrice = TotaltPrice / 1.10;

            if (TotaltPrice >= 50000)
                TotaltPrice = TotaltPrice / 1.15;
            return TotaltPrice;
        }
        public static double CalculateStateTax(string stateCode, double priceAfterDiscount)
        {
            Dictionary<string, double> TaxForStates = new Dictionary<string, double>()
            {
                {"UT",0.0685},
                {"NV",0.08},
                {"TX",0.0625},
                {"AL",0.04},
                {"CA",0.0825}
            };

            if (TaxForStates.ContainsKey(stateCode))
            {
                foreach (KeyValuePair<string, double> state in TaxForStates)
                {
                    if (stateCode == state.Key)
                    {
                        priceAfterDiscount += priceAfterDiscount * state.Value;
                    }
                }
                IsStateCodeWrong = false;
            }
            else
            {
                IsStateCodeWrong = true;
            }
            return priceAfterDiscount;
        }
    }
}
