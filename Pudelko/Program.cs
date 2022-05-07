using P = PudelkoLibrary.Pudelko;
using PudelkoLibrary;

namespace PudelkoProgram
{
    public static class Program
    {
        public static void Main()
        {
            P p1 = new(1, 2, 3, UnitOfMeasure.meter);
            P p2 = new(300, 200, 100, UnitOfMeasure.centimeter);
            P p3 = new(3000, 3000, 3000, UnitOfMeasure.milimeter);

            Console.WriteLine(p1.A + " " + p2.B + " " + p3.C);
            Console.WriteLine(p1.ToString("m"));
            Console.WriteLine(p2.ToString("cm"));
            Console.WriteLine(p3.ToString("mm"));

            Console.WriteLine($"Objetosc p1: {p1.Objetosc}");
            Console.WriteLine($"Pole p2: {p2.Pole}");

            Console.WriteLine($"p1 == p2 ? {Equals(p1, p2)}");

            Console.WriteLine($"p1.A: {p1[0]}");

            foreach (var p in p1) Console.WriteLine($"{nameof(p)}: {p}");

            Console.WriteLine($"p1 + p2 = {p1 + p2}");

            Console.WriteLine(p1.Kompresuj());

            List<P> list = new();
            list.Add(new(2, 2, 2));
            list.Add(new(3, 3, 3));
            list.Add(new(3, 3, 3));
            list.Add(new(5, 3, 9));
            list.Add((10, 1, 1));
            foreach (var p in list) Console.WriteLine(p);
            Console.WriteLine();
            list.Sort(P.ComparePudelkos);
            foreach (var p in list) Console.WriteLine(p);
        }
    }
}