using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PudelkoLibrary
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable
    {
        private readonly double _a; // Przechowuje wymiary w milimetrach (a, b, c prostopadłościanu)
        private readonly double _b;
        private readonly double _c;

        private ArgumentOutOfRangeException argumentOutOfRange = new("Dimension must be positive and lesser or equal to 10m.");

        public double A
        {
            get => _a / 1000;
        }
        public double B
        {
            get => _b / 1000;
        }
        public double C
        {
            get => _c / 1000;
        }
        public double Objetosc
        {
            get => Math.Round(A * B * C, 9);
        }
        public double Pole
        {
            get => Math.Round(A * B * 2 + A * B * 2 + B * C * 2, 6);
        }
        public double this[int index]
        {
            get
            {
                return index switch
                {
                    0 => A,
                    1 => B,
                    2 => C,
                    _ => throw new ArgumentOutOfRangeException(nameof(index)),
                };
            }
        }

        public Pudelko()
        {
            _a = 100;
            _b = 100;
            _c = 100;
        }
        public Pudelko(double a, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            if (a * (double)unit < 1 || a * (double)unit > 10000) throw argumentOutOfRange;
            _a = (int)(a * (double)unit);
            _b = 100;
            _c = 100;
        }
        public Pudelko(double a, double b, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            if (a * (double)unit < 1 || a * (double)unit > 10000) throw argumentOutOfRange;
            if (b * (double)unit < 1 || b * (double)unit > 10000) throw argumentOutOfRange;
            _a = (int)(a * (double)unit);
            _b = (int)(b * (double)unit);
            _c = 100;
        }
        public Pudelko(double a, double b, double c, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            if (a * (double)unit < 1 || a * (double)unit > 10000) throw argumentOutOfRange;
            if (b * (double)unit < 1 || b * (double)unit > 10000) throw argumentOutOfRange;
            if (c * (double)unit < 1 || c * (double)unit > 10000) throw argumentOutOfRange;
            _a = (int)(a * (double)unit);
            _b = (int)(b * (double)unit);
            _c = (int)(c * (double)unit);
        }

        public override string ToString()
        {
            return $"{A} m × {B} m × {A} m";
        }
        public string ToString(string? format)
        {
            var multiplier = format switch
            {
                "mm" => 1000,
                "cm" => 100,
                "m" => 1,
                _ => throw new FormatException(),
            };
            return $"{A * multiplier} {format} × {B * multiplier} {format} × {C * multiplier} {format}";
        }
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var multiplier = format switch
            {
                "mm" => 1000,
                "cm" => 100,
                "m" => 1,
                _ => throw new FormatException(),
            };
            return $"{A * multiplier} {format} × {B * multiplier} {format} × {C * multiplier} {format}";
        }
        public bool Equals(Pudelko? other)
        {
            if (other == null)
                return false;

            List<double> thisDimensions = new() { A, B, C };
            List<double> otherDimensions = new() { other.A, other.B, other.C };
            return thisDimensions.OrderBy(x => x).SequenceEqual(otherDimensions.OrderBy(x => x));
        }
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (obj is not Pudelko)
                return false;

            return Equals((Pudelko)obj);
        }
        public static bool operator ==(Pudelko p1, Pudelko p2) => Equals(p1, p2);
        public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);
        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            List<double> leftDimensions = new() { p1.A, p1.B, p1.C};
            List<double> rightDimensions = new() { p2.A, p2.B, p2.C};
            leftDimensions.Sort();
            rightDimensions.Sort();
            double a = leftDimensions[2] > rightDimensions[2] ? leftDimensions[2] : rightDimensions[2];
            double b = leftDimensions[1] > rightDimensions[1] ? leftDimensions[1] : rightDimensions[1];
            double c = leftDimensions[0] + rightDimensions[0];
            return new Pudelko(a, b, c);
        }
        public static explicit operator double[](Pudelko p) => new double[3] { p.A, p.B, p.C };
        public static implicit operator Pudelko(ValueTuple<int, int, int> tuple) => new(tuple.Item1, tuple.Item2, tuple.Item3, UnitOfMeasure.milimeter);
        public static Pudelko Parse(string stringToParse)
        {
            string[] splitString = stringToParse.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (splitString.Length != 8) throw new ArgumentException("Input string not in right format.");
            int[] dimensions;
            try
            {
                dimensions = new int[3] { int.Parse(splitString[0]), int.Parse(splitString[3]), int.Parse(splitString[6]) };
            }
            catch
            {
                throw new ArgumentException("Input string not in right format.");
            }

            var unit = splitString[1] switch
            {
                "mm" => UnitOfMeasure.milimeter,
                "cm" => UnitOfMeasure.centimeter,
                "m" => UnitOfMeasure.meter,
                _ => throw new ArgumentException("Input string not in right format."),
            };
            return new Pudelko(dimensions[0], dimensions[1], dimensions[2], unit);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
        public PudelkoEnum GetEnumerator()
        {
            return new PudelkoEnum(new double[3] {A, B, C});
        }
        public override int GetHashCode()
        {
            List<double> thisDimensions = new() { A, B, C };
            return thisDimensions.GetHashCode();
        }
    }
}