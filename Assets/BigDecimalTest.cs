using System;
using System.Numerics;
using Data;
using UnityEngine;

public class BigDecimalTest : MonoBehaviour
{
    public BigInteger dekiisnichtsoCool;
    public BigDecimal DekiIsCool = new BigDecimal(100,0);
    
    void Start()
    {
        Debug.Log("Testing Power Function:");

        BigDecimal baseValue = new BigDecimal(BigInteger.Parse("100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000009999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999"), 0);
        BigDecimal exponent = new BigDecimal(25, -2); // 0.1

        BigDecimal result = baseValue.Power(exponent);

        Debug.Log($"Result of {baseValue}^{exponent}: {result}");

        // Testing integer exponent
        BigDecimal intExponent = new BigDecimal(3, 0); // 3
        BigDecimal intResult = baseValue.Power(intExponent);

        Debug.Log($"Result of {baseValue}^{intExponent}: {intResult}");
        
        Debug.Log(DekiIsCool);
        // Initializing BigDecimal Instances
        BigDecimal d1 = new BigDecimal(100000, -3);
        BigDecimal d2 = new BigDecimal(1200, -3);
        BigDecimal d3 = new BigDecimal(500, -3);

        Debug.Log("Initial Values:");
        Debug.Log($"d1: {d1}");
        Debug.Log($"d2: {d2}");
        Debug.Log($"d3: {d3}");

        // Testing Addition
        BigDecimal d4 = d2 + d1 + d3;
        Debug.Log("Addition:");
        Debug.Log($"d4 (d1: {d1} + d2: {d2} + d3: {d3}): {d4}");

        // Testing Multiplication and Rounding
        BigDecimal d5 = (d1 * d3 * d2).Round(3);
        BigDecimal d6 = (d1 * d2).Round(3);
        Debug.Log("Multiplication and Rounding:");
        Debug.Log($"d5 ((d1: {d1} * d2: {d2} * d3: {d3}), rounded to 3 decimal places): {d5}");
        Debug.Log($"d6 ((d1: {d1} * d2: {d2}), rounded to 3 decimal places): {d6}");

        // Testing Subtraction
        BigDecimal d7 = d2 - d1 - d3;
        BigDecimal d10 = d1 - d2 - d3;
        Debug.Log("Subtraction:");
        Debug.Log($"d7 (d2: {d2} - d1: {d1} - d3: {d3}): {d7}");
        Debug.Log($"d10 (d1: {d1} - d2: {d2} - d3: {d3}): {d10}");

        // Testing Division
        BigDecimal d8 = d1 / d2;
        Debug.Log("Division:");
        Debug.Log($"d8 (d1: {d1} / d2: {d2}): {d8}");

        // Testing Rounding
        BigDecimal d9 = new BigDecimal(100000, -3);
        Debug.Log("Rounding Tests:");
        Debug.Log($"d9 ({d9}) rounded to 6 decimal places: {d9.Round(6)}");
        Debug.Log($"d9 ({d9}) rounded to 2 decimal places: {d9.Round(2)}");
        Debug.Log($"d9 ({d9}) rounded to 1 decimal place: {d9.Round(1)}");
        Debug.Log($"d9 ({d9}) rounded to 0 decimal places: {d9.Round(0)}");

        // Testing Modulo
        BigDecimal d13 = d8 % d10;
        Debug.Log("Modulo Operation:");
        Debug.Log($"d13 (d8: {d8} % d10: {d10}): {d13}");

        // Testing Absolute Value
        BigDecimal d11 = new BigDecimal(-5000, -2);
        Debug.Log("Absolute Value:");
        Debug.Log($"Abs(d11: {d11}): {d11.Abs()}");

        // Testing Power
        BigDecimal d12 = d3.Power(2);
        Debug.Log("Power Operation:");
        Debug.Log($"d12 (d3: {d3}^2): {d12}");

        // Testing Min and Max
        Debug.Log("Min and Max:");
        Debug.Log($"Min(d1: {d1}, d2: {d2}): {BigDecimal.Min(d1, d2)}");
        Debug.Log($"Max(d1: {d1}, d2: {d2}): {BigDecimal.Max(d1, d2)}");

        // Testing Clamp
        BigDecimal clampedValue = BigDecimal.Clamp(d2, new BigDecimal(800, -3), new BigDecimal(1500, -3));
        Debug.Log("Clamping:");
        Debug.Log($"Clamped d2 ({d2}) between 800e-3 and 1500e-3: {clampedValue}");

        // Testing Comparison
        Debug.Log("Comparison Tests:");
        Debug.Log($"d1: {d1} > d2: {d2}: {d1 > d2}");
        Debug.Log($"d1: {d1} < d2: {d2}: {d1 < d2}");
        Debug.Log($"d1: {d1} == d9: {d9}: {d1 == d9}");
        Debug.Log($"d1: {d1} != d3: {d3}: {d1 != d3}");
        Debug.Log($"d1: {d1} >= d2: {d2}: {d1 >= d2}");
        Debug.Log($"d1: {d1} <= d2: {d2}: {d1 <= d2}");

        // Testing Parsing and TryParse
        Debug.Log("Parsing Tests:");
        BigDecimal parsedValue;
        if (BigDecimal.TryParse("12345.6789", out parsedValue))
        {
            Debug.Log($"Parsed '12345.6789' successfully: {parsedValue}");
        }
        else
        {
            Debug.Log("Failed to parse '12345.6789'");
        }

        try
        {
            BigDecimal parsedValueExact = BigDecimal.Parse("9876.54321");
            Debug.Log($"Parsed '9876.54321': {parsedValueExact}");
        }
        catch (FormatException e)
        {
            Debug.Log($"Parse failed: {e.Message}");
        }

        // Testing Random Generation
        BigDecimal randomValue = BigDecimal.Random(new BigDecimal(100, -1), new BigDecimal(200, -1));
        Debug.Log("Random Generation:");
        Debug.Log($"Random BigDecimal between 100e-1 and 200e-1: {randomValue}");

        // Testing Type Conversions
        Debug.Log("Implicit and Explicit Type Conversions:");
        BigDecimal intConversion = 123;
        Debug.Log($"Implicit int to BigDecimal (123): {intConversion}");

        BigDecimal longConversion = (BigDecimal)456L;
        Debug.Log($"Implicit long to BigDecimal (456L): {longConversion}");

        BigDecimal floatConversion = (BigDecimal)123.45f;
        Debug.Log($"Implicit float to BigDecimal (123.45f): {floatConversion}");

        BigDecimal doubleConversion = (BigDecimal)678.91;
        Debug.Log($"Implicit double to BigDecimal (678.91): {doubleConversion}");

        int explicitInt = (int)d1;
        Debug.Log($"Explicit BigDecimal to int (d1: {d1}): {explicitInt}");

        long explicitLong = (long)d2;
        Debug.Log($"Explicit BigDecimal to long (d2: {d2}): {explicitLong}");

        double explicitDouble = (double)d3;
        Debug.Log($"Explicit BigDecimal to double (d3: {d3}): {explicitDouble}");

        float explicitFloat = (float)d1;
        Debug.Log($"Explicit BigDecimal to float (d1: {d1}): {explicitFloat}");

        // Testing Clone
        BigDecimal clone = d1.Clone();
        Debug.Log("Clone Test:");
        Debug.Log($"Clone of d1: {d1}: {clone}");

        // Testing Equals
        Debug.Log("Equality Test:");
        Debug.Log($"d1: {d1}.Equals(d9: {d9}): {d1.Equals(d9)}");
        Debug.Log($"d1: {d1}.Equals(d3: {d3}): {d1.Equals(d3)}");

        // Testing ToString
        Debug.Log("ToString Test:");
        Debug.Log($"d1.ToString(): {d1.ToString()}");
        Debug.Log($"d2.ToString(): {d2.ToString()}");
        Debug.Log($"d3.ToString(): {d3.ToString()}");
        
        
                // Initializing BigDecimal Instances for testing ToNumberSuffix
        Debug.Log("Suffix Tests:");

        BigDecimal value1 = new BigDecimal(BigInteger.Parse("1200"), 0); // 1.2K
        Debug.Log($"Value: {value1} -> ToNumberSuffix: {value1.ToNumberSuffix()}");

        BigDecimal value2 = new BigDecimal(BigInteger.Parse("1250000"), 0); // 1.25M
        Debug.Log($"Value: {value2} -> ToNumberSuffix: {value2.ToNumberSuffix()}");

        BigDecimal value3 = new BigDecimal(BigInteger.Parse("3500000000"), 0); // 3.5B
        Debug.Log($"Value: {value3} -> ToNumberSuffix: {value3.ToNumberSuffix()}");

        BigDecimal value4 = new BigDecimal(BigInteger.Parse("5000000000000"), 0); // 5T
        Debug.Log($"Value: {value4} -> ToNumberSuffix: {value4.ToNumberSuffix()}");

        BigDecimal value5 = new BigDecimal(BigInteger.Parse("9000000000000000"), 0); // 9Qa (Quadrillion)
        Debug.Log($"Value: {value5} -> ToNumberSuffix: {value5.ToNumberSuffix()}");

        BigDecimal value6 = new BigDecimal(BigInteger.Parse("7300000000000000000"), 0); // 7.3Qi (Quintillion)
        Debug.Log($"Value: {value6} -> ToNumberSuffix: {value6.ToNumberSuffix()}");

        BigDecimal value7 = new BigDecimal(BigInteger.Parse("1200"), -3); // 1.2, without suffix since it’s below 1K
        Debug.Log($"Value: {value7} -> ToNumberSuffix: {value7.ToNumberSuffix()}");

        // Testing edge cases for very large numbers with suffix
        BigDecimal value8 = new BigDecimal(BigInteger.Parse("1000000000000000000000000"), 0); // Sx (Sextillion)
        Debug.Log($"Value: {value8} -> ToNumberSuffix: {value8.ToNumberSuffix()}");

        BigDecimal value9 = new BigDecimal(BigInteger.Parse("5000000000000000000000000000"), 0); // Sp (Septillion)
        Debug.Log($"Value: {value9} -> ToNumberSuffix: {value9.ToNumberSuffix()}");

        BigDecimal value10 = new BigDecimal(BigInteger.Parse("999999999999999999999999999999999"), 0); // Oc (Octillion)
        Debug.Log($"Value: {value10} -> ToNumberSuffix: {value10.ToNumberSuffix()}");

        BigDecimal test = d1 * 1.2f;
        Debug.Log($"test: {test}");
        
        Debug.Log("Suffix Tests Complete.");
    }
    
}
