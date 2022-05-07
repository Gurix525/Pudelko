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

        private readonly ArgumentOutOfRangeException argumentOutOfRange = new("Dimension of a new Pudelko must be positive and lesser or equal to 10m.");

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
            get => Math.Round(A * B * 2 + A * C * 2 + B * C * 2, 6);
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
            return $"{String.Format("{0:0.000}", A)} m × {String.Format("{0:0.000}", B)} m × {String.Format("{0:0.000}", C)} m";
        }
        public string ToString(string? format)
        {
            if (format == null) format = "m";
            var multiplier = format switch
            {
                "mm" => 1000,
                "cm" => 100,
                "m" => 1,
                _ => throw new FormatException()
            };
            var stringFormat = format switch
            {
                "mm" => "{0:0}",
                "cm" => "{0:0.0}",
                "m" => "{0:0.000}",
                _ => throw new FormatException()
            };
            return $"{String.Format(stringFormat, A * multiplier)} {format} × {String.Format(stringFormat, B * multiplier)} {format} × {String.Format(stringFormat, C * multiplier)} {format}";
        }
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return ToString(format);
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
            List<double> leftDimensions = new List<double>((double[])p1).OrderByDescending(x => x).ToList();
            List<double> rightDimensions = new List<double>((double[])p2).OrderByDescending(x => x).ToList();
            Pudelko greater = p1;
            Pudelko lesser = p2;
            for (int i = 0; i < 3; i++)
            {
                if (leftDimensions[i] == rightDimensions[i]) continue;
                else if (leftDimensions[i] > rightDimensions[i])
                {
                    greater = p1;
                    lesser = p2;
                    break;
                }
                else if (leftDimensions[i] < rightDimensions[i])
                {
                    greater = p2;
                    lesser = p1;
                    break;
                }
            }
            List<Pudelko> temporalPudelko = new();
            temporalPudelko.Add(new(greater.A + lesser.A, greater.B, greater.C));
            temporalPudelko.Add(new(greater.A + lesser.B, greater.B, greater.C));
            temporalPudelko.Add(new(greater.A + lesser.C, greater.B, greater.C));
            temporalPudelko.Add(new(greater.A, greater.B + lesser.A, greater.C));
            temporalPudelko.Add(new(greater.A, greater.B + lesser.B, greater.C));
            temporalPudelko.Add(new(greater.A, greater.B + lesser.C, greater.C));
            temporalPudelko.Add(new(greater.A, greater.B, greater.C + lesser.A));
            temporalPudelko.Add(new(greater.A, greater.B, greater.C + lesser.B));
            temporalPudelko.Add(new(greater.A, greater.B, greater.C + lesser.C));
            temporalPudelko = temporalPudelko.OrderBy(x => x.Objetosc).ToList();

            for (int i = 0; i < temporalPudelko.Count; i++)
            {
                if (temporalPudelko[i].Objetosc < greater.Objetosc + lesser.Objetosc)
                {
                    temporalPudelko.RemoveAt(i);
                    i--;
                }
            }

            if (temporalPudelko.Count > 0) return temporalPudelko[0];
            else throw new Exception("It is not possible to create a pudelko containing the two given pudelkos.");
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
            return new PudelkoEnum(new double[3] { A, B, C });
        }
        public override int GetHashCode()
        {
            List<double> thisDimensions = new() { A, B, C };
            return thisDimensions.GetHashCode();
        }
        public static int ComparePudelkos(Pudelko p1, Pudelko p2)
        {
            if (p1 == null)
            {
                if (p2 == null)
                {
                    return 0;
                }
                else return -1;
            }
            else
            {
                if (p2 == null) return 1;

                if (p1.Objetosc != p2.Objetosc) {
                    int result = p1.Objetosc - p2.Objetosc > 0 ? 1 : -1;
                    return result;
                }
                if (p1.Pole != p2.Pole)
                {
                    int result = p1.Pole - p2.Pole > 0 ? 1 : -1;
                    return result;
                }
                if ((p1.A + p1.B + p1.C) - (p2.A + p2.B + p2.C) > 0) return 1;
                else if ((p1.A + p1.B + p1.C) - (p2.A + p2.B + p2.C) < 0) return -1;
                else return 0;
            }
        }
    }
}