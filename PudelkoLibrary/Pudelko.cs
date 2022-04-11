using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PudelkoLibrary
{
    public sealed class Pudelko : IFormattable
    {
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;

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

        public Pudelko(double a = 0.1, double b = 0.1, double c = 0.1, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            if (a * (double)unit < 0 || b * (double)unit > 10000) throw new ArgumentOutOfRangeException();
            if (b * (double)unit < 0 || b * (double)unit > 10000) throw new ArgumentOutOfRangeException();
            if (c * (double)unit < 0 || c * (double)unit > 10000) throw new ArgumentOutOfRangeException();
            _a = (int)(a * (double)unit);
            _b = (int)(b * (double)unit);
            _c = (int)(c * (double)unit);
        }

        public override string ToString()
        {
            return $"{A} m × {B} m × {A} m";
        }
        public string ToString(string format)
        {
            int multiplier;
            switch (format)
            {
                case "mm":
                    multiplier = 1000;
                    break;
                case "cm":
                    multiplier = 100;
                    break;
                case "m":
                    multiplier = 1;
                    break;
                default: throw new FormatException();
            }
            return $"{A * multiplier} {format} × {B * multiplier} {format} × {C * multiplier} {format}";
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}