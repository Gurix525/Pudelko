using P = PudelkoLibrary.Pudelko;

namespace PudelkoProgram
{
    public static class Program
    {
        public static void Main()
        {
            Console.ReadLine();

            List<P> list = new();
            list.Add(new(1, 2, 3));
            list.Add(new(2, 3, 1));
            list.Add(new(0.5, 1, 0.05));
            list.Add((3, 5, 5));
            list.Add((1, 1, 1));
            list.Sort(P.ComparePudelkos);

            foreach (P p in list) Console.WriteLine(p.ToString());
        }
    }
}