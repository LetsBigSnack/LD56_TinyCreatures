using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    using System;
    using System.Numerics;
    
    [Serializable]
    public struct BigDecimal: IComparable<BigDecimal>, IEquatable<BigDecimal>
    {
        
        private static readonly Dictionary<BigDecimal, string> Suffixes = new Dictionary<BigDecimal, string>()
        {
            { new BigDecimal(BigInteger.Parse("1000000"), -3), "K" },
            { new BigDecimal(BigInteger.Parse("1000000000"), -3), "M" },
            { new BigDecimal(BigInteger.Parse("1000000000000"), -3), "B" },
            { new BigDecimal(BigInteger.Parse("1000000000000000"), -3), "T" },
            { new BigDecimal(BigInteger.Parse("1000000000000000000"), -3), "Qa" }, // Quadrillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000"), -3), "Qi" }, // Quintillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000"), -3), "Sx" }, // Sextillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000"), -3), "Sp" }, // Septillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000"), -3), "Oc" }, // Octillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000000"), -3), "No" }, // Nonillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000000000"), -3), "Dc" }, // Decillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000000000000"), -3), "Ud" }, // Undecillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000000000000000"), -3), "Dd" }, // Duodecillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000000000000000000"), -3), "Td" }, // Tredecillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000000000000000000000"), -3), "Qad" }, // Quattuordecillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000000000000000000000000"), -3), "Qid" }, // Quindecillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000000000000000000000000000"), -3), "Sxd" }, // Sexdecillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000000000000000000000000000000"), -3), "Spd" }, // Septendecillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000000000000000000000000000000000"), -3), "Ocd" }, // Octodecillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000000000000000000000000000000000000"), -3), "Nod" }, // Novemdecillion
            { new BigDecimal(BigInteger.Parse("1000000000000000000000000000000000000000000000000000000000000000000"), -3), "Vg" }  // Vigintillion
        };

        
        [SerializeField]
        private string mantissaString;
        [SerializeField]
        private string exponentString;
        
        // Core Fields
        [SerializeField]
        private BigInteger mantissa;
        [SerializeField]
        private BigInteger exponent;
        
        // Constructor
        public BigDecimal(BigInteger mantissa, BigInteger exponent)
        {
            this.mantissa = mantissa;
            this.exponent = exponent;
            mantissaString = mantissa.ToString();
            exponentString = exponent.ToString();
        }
        
        // Operators
        #region Operators

        public static BigDecimal operator +(BigDecimal a, BigDecimal b) => a.Add(b);
        public static BigDecimal operator -(BigDecimal a, BigDecimal b) => a.Subtract(b);
        public static BigDecimal operator *(BigDecimal a, BigDecimal b) => a.Multiply(b);
        public static BigDecimal operator /(BigDecimal a, BigDecimal b) => a.Divide(b);
        public static BigDecimal operator %(BigDecimal a, BigDecimal b) => a.Mod(b);
        public static BigDecimal operator ++(BigDecimal a) => a.Add(1);
        public static BigDecimal operator --(BigDecimal a) => a.Subtract(-1);
        public static bool operator >(BigDecimal a, BigDecimal b) => a.CompareTo(b) > 0;
        public static bool operator <(BigDecimal a, BigDecimal b) => a.CompareTo(b) < 0;
        public static bool operator >=(BigDecimal a, BigDecimal b) => a.CompareTo(b) >= 0;
        public static bool operator <=(BigDecimal a, BigDecimal b) => a.CompareTo(b) <= 0;
        public static bool operator ==(BigDecimal a, BigDecimal b) => a.Equals(b);
        public static bool operator !=(BigDecimal a, BigDecimal b) => !a.Equals(b);

        public static implicit operator BigDecimal(int value) => FromBigInt(value);
        public static implicit operator BigDecimal(long value) => FromBigInt(value);
        public static implicit operator BigDecimal(BigInteger value) => FromBigInt(value);
        public static implicit operator BigDecimal(float value) => FromDouble(value);
        public static implicit operator BigDecimal(double value) => FromDouble(value);

        public static explicit operator int(BigDecimal value) => (int)value.ToBigInteger();
        public static explicit operator long(BigDecimal value) => (long)value.ToBigInteger();
        public static explicit operator BigInteger(BigDecimal value) => value.ToBigInteger();
        public static explicit operator double(BigDecimal value) => double.Parse(value.ToString());
        public static explicit operator float(BigDecimal value) => float.Parse(value.ToString());

        #endregion

        // Core Arithmetic Operations
        #region Arithmetic Operations

        public BigDecimal Add(BigDecimal other)
        {
            BigInteger expDifference = exponent - other.exponent;
            if (expDifference < 0)
            {
                BigInteger adjustedOtherMantissa = other.mantissa * BigInteger.Pow(10, (int)-expDifference);
                return new BigDecimal(mantissa + adjustedOtherMantissa, exponent);
            }
            else if (expDifference > 0)
            {
                BigInteger adjustedMantissa = mantissa * BigInteger.Pow(10, (int)expDifference);
                return new BigDecimal(adjustedMantissa + other.mantissa, other.exponent);
            }
            else
            {
                return new BigDecimal(mantissa + other.mantissa, exponent);
            }
        }

        public BigDecimal Subtract(BigDecimal other)
        {
            return Add(new BigDecimal(-other.mantissa, other.exponent));
        }

        public BigDecimal Multiply(BigDecimal other)
        {
            BigInteger newMantissa = mantissa * other.mantissa;
            BigInteger newExponent = exponent + other.exponent;
            return new BigDecimal(newMantissa, newExponent);
        }

        public BigDecimal Divide(BigDecimal other)
        {
            if (other.mantissa == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero.");
            }

            BigInteger newMantissa = mantissa * BigInteger.Pow(10, (int)(-other.exponent));
            BigInteger resultMantissa = newMantissa / other.mantissa;

            return new BigDecimal(resultMantissa, exponent);
        }

        public BigDecimal Mod(BigDecimal other)
        {
            if (other.mantissa == 0)
            {
                throw new DivideByZeroException("Cannot modulo by zero.");
            }

            BigDecimal divResult = Divide(other);
            BigInteger integerPart = divResult.mantissa / BigInteger.Pow(10, Math.Abs((int)divResult.exponent));
            BigDecimal integerPartBigDecimal = other.Multiply(new BigDecimal(integerPart, 0));
            return Subtract(integerPartBigDecimal);
        }

        #endregion

        // Utility Math Functions
        #region Utility Math Functions

        public BigDecimal Abs()
        {
            return new BigDecimal(BigInteger.Abs(mantissa), exponent);
        }

        public BigDecimal Sqrt()
        {
            if (mantissa < 0)
            {
                throw new ArithmeticException("Cannot compute the square root of a negative number.");
            }

            BigInteger adjustedMantissa = mantissa;
            BigInteger adjustedExponent = exponent;
            if (adjustedExponent % 2 != 0)
            {
                adjustedMantissa *= 10;
                adjustedExponent--;
            }

            BigInteger sqrtMantissa = Sqrt(adjustedMantissa);
            BigInteger sqrtExponent = adjustedExponent / 2;

            return new BigDecimal(sqrtMantissa, sqrtExponent);
        }

        private static BigInteger Sqrt(BigInteger value)
        {
            if (value < 0)
                throw new ArgumentException("Cannot compute the square root of a negative number.");

            if (value == 0 || value == 1)
                return value;

            BigInteger n = value;
            BigInteger approx = value / 2;

            while (approx > 0)
            {
                BigInteger betterApprox = (approx + n / approx) / 2;
                if (betterApprox == approx || betterApprox == approx - 1)
                    return betterApprox;
                approx = betterApprox;
            }

            return approx;
        }

        public BigDecimal Power(int power)
        {
            if (power < 0) throw new ArgumentOutOfRangeException(nameof(power), "Power must be a non-negative integer.");

            BigInteger newMantissa = BigInteger.Pow(mantissa, power);
            BigInteger newExponent = exponent * power;

            return new BigDecimal(newMantissa, newExponent).Normalize();
        }

        public BigDecimal Round(BigInteger decimalPlace)
        {
            if (decimalPlace < BigInteger.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(decimalPlace));
            }
            BigInteger exponentDiff = exponent + decimalPlace;

            if (exponentDiff < 0)
            {
                BigInteger newMantissa = mantissa / BigInteger.Pow(10, (int)-exponentDiff);
                return new BigDecimal(newMantissa, -decimalPlace);

            }
            else if (exponentDiff > 0)
            {
                BigInteger newMantissa = mantissa * BigInteger.Pow(10, (int)exponentDiff);
                return new BigDecimal(newMantissa, -decimalPlace);
            }
            else
            {
                return this;
            }
        }

        #endregion

        // Conversion Functions
        #region Conversion Functions

        public BigInteger ToBigInteger()
        {
            return this.mantissa * BigInteger.Pow(10, (int)-exponent);
        }

        public override string ToString()
        {
            if (mantissa == 0)
            {
                return "0";
            }

            if (exponent >= 0)
            {
                string mantissaStr = mantissa.ToString();
                string scaledNumber = mantissaStr + new string('0', (int)exponent);
                return scaledNumber;
            }
            else
            {
                string mantissaStr = mantissa.ToString();
                int decimalPosition = mantissaStr.Length + (int)exponent;

                if (decimalPosition > 0)
                {
                    return mantissaStr.Insert(decimalPosition, ".");
                }
                else
                {
                    return "0." + new string('0', -decimalPosition) + mantissaStr;
                }
            }
        }
        
        public string ToNumberSuffix(bool fill = true, int decimalPlaces = 3)
        {

            BigDecimal bigDecimal = this.Round(decimalPlaces);
            string finalNumber = bigDecimal.ToString();
            
            bigDecimal= fill? bigDecimal: bigDecimal.Round(0);



            foreach (var item in Suffixes)
            {
                BigDecimal tempNumber = bigDecimal / item.Key;


                //probably this is fucked
                if (tempNumber >= 1)
                {
                    
                    finalNumber = tempNumber.ToString() + item.Value;

                }
                else
                {
                    break;
                }
            }
            return finalNumber;
        }



        private static BigDecimal FromBigInt(BigInteger value)
        {
            return new BigDecimal(value, BigInteger.Zero);
        }

        private static BigDecimal FromDouble(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Cannot convert NaN or Infinity to BigDecimal.");

            bool isNegative = value < 0;
            value = Math.Abs(value);

            string valueStr = value.ToString("R");
            int decimalIndex = valueStr.IndexOf('.');

            BigInteger mantissa;
            BigInteger exponent;

            if (decimalIndex == -1)
            {
                mantissa = BigInteger.Parse(valueStr);
                exponent = 0;
            }
            else
            {
                string integerPart = valueStr.Substring(0, decimalIndex);
                string fractionalPart = valueStr.Substring(decimalIndex + 1);

                mantissa = BigInteger.Parse(integerPart + fractionalPart);
                exponent = -fractionalPart.Length;
            }

            if (isNegative) mantissa = -mantissa;

            return new BigDecimal(mantissa, exponent);
        }

        #endregion

        // Parsing Functions
        #region Parsing Functions

        public static BigDecimal Parse(string s)
        {
            if (TryParse(s, out BigDecimal result))
            {
                return result;
            }
            throw new FormatException("Input string was not in a correct format.");
        }

        public static bool TryParse(string s, out BigDecimal result)
        {
            result = default;

            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            s = s.Trim();

            int decimalIndex = s.IndexOf('.');
            string integerPart;
            string fractionalPart = "";

            if (decimalIndex >= 0)
            {
                integerPart = s.Substring(0, decimalIndex);
                fractionalPart = s.Substring(decimalIndex + 1);
            }
            else
            {
                integerPart = s;
            }

            string mantissaStr = integerPart + fractionalPart;

            if (!BigInteger.TryParse(mantissaStr, out BigInteger mantissa))
            {
                return false;
            }

            BigInteger exponent = -fractionalPart.Length;

            result = new BigDecimal(mantissa, exponent);
            return true;
        }

        #endregion

        // Comparison Functions
        #region Comparison Functions

        public int CompareTo(BigDecimal other)
        {
           BigInteger expDifference = this.exponent - other.exponent;


            if (expDifference > 0)
            {
                BigInteger adjustedMantissa = this.mantissa * BigInteger.Pow(10, (int)(expDifference));
                return adjustedMantissa.CompareTo(other.mantissa);
            }
            else if (expDifference < 0)
            {
                BigInteger adjustedOtherMantissa = other.mantissa * BigInteger.Pow(10, (int)-expDifference);
                return this.mantissa.CompareTo(adjustedOtherMantissa);
            }
            else
            {
                // If exponents are the same, directly compare mantissas
                return this.mantissa.CompareTo(other.mantissa);
            }
        }


        public bool Equals(BigDecimal other)
        {
            BigDecimal normalizedThis = Normalize();
            BigDecimal normalizedOther = other.Normalize();

            return normalizedThis.mantissa == normalizedOther.mantissa && normalizedThis.exponent == normalizedOther.exponent;
        }

        public static BigDecimal Min(BigDecimal a, BigDecimal b)
        {
            return a < b ? a : b;
        }

        public static BigDecimal Max(BigDecimal a, BigDecimal b)
        {
            return a > b ? a : b;
        }

        public static BigDecimal Clamp(BigDecimal value, BigDecimal min, BigDecimal max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        #endregion

        // Randomization and Utility Functions
        #region Randomization and Utility Functions

        public static BigDecimal Random(BigDecimal min, BigDecimal max, Random rng = null)
        {
            rng ??= new Random();

            BigInteger rangeMantissa = max.mantissa - min.mantissa;
            BigInteger randomMantissa = min.mantissa + new BigInteger(rng.Next()) % rangeMantissa;

            BigInteger randomExponent = min.exponent + rng.Next() % (max.exponent - min.exponent + 1);

            return new BigDecimal(randomMantissa, randomExponent);
        }

        public BigDecimal Clone()
        {
            return new BigDecimal(mantissa, exponent);
        }

        private BigDecimal Normalize()
        {
            if (mantissa == 0)
            {
                return new BigDecimal(0, 0);
            }

            while (mantissa % 10 == 0)
            {
                mantissa /= 10;
                exponent += 1;
            }

            return this;
        }

        #endregion
    }
}
