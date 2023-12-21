using ExtendedNumerics;
using System;
using System.Collections.Generic;
using System.Windows;

namespace D21
{
    public class Polynomial
    {
        public double YScale = 1;
        public double XScale = 1;
        public int Degree { get; }
        BigDecimal[] coefs;
        public double fitness = double.MaxValue;
        public BigDecimal this[int idx] { get => coefs[idx]; set => coefs[idx] = value; }
        public Polynomial(int degree)
        {
            coefs = new BigDecimal[degree + 1];
            Degree = degree;
        }

        public Polynomial(BigDecimal[] values)
        {
            Degree = values.Length - 1;
            coefs = values;
        }
        public BigDecimal EvaluateAt(double x)
        {
            BigDecimal fx = 0;
            for (int i = 0; i <= Degree; i++)
                fx += YScale * (this[i] * Math.Pow(x * XScale, Degree - i));
            return fx;
        }
       
    }
}
