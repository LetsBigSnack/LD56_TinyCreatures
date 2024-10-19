using System;
using System.Collections.Generic;
using UnityEngine;

namespace Helper.Util
{
    public class Util_LargeNumberDisplay
    {
        public static Dictionary<double, string> Suffixes = new Dictionary<double, string>()
        {
            { 1000, "K"},
            { 1000000, "M"},
            { 1000000000, "B"},
            //TODO: will never reach with int max 2 billion ish --> 4 billion ish if we have uint
            {1000000000000, "T"}
        };
        
        public static string LargerNumberConversion(double number, bool fill = true)
        {
            number = Math.Round(number, 3);
            
            
            string finalNumber = fill? number.ToString("0.000"): number+ "";
            
            foreach(var item in Suffixes)
            {
                double tempNumber = Math.Round(number / item.Key, 3);

                if (tempNumber >= 1) 
                {
                    string format = item.Value == Suffixes[1000]? "0.000" : "0.00";
                    
                    finalNumber = tempNumber.ToString(format) + item.Value;
                    
                }
                else
                {
                    break;
                }
            }
            
            return finalNumber;
        }
    }
}