using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P = PudelkoLibrary.Pudelko;

public static class PudelkoExtension
{
    public static P Kompresuj(this P p)
    {
        double newEdge = Math.Cbrt(p.Objetosc);
        return new P(newEdge, newEdge, newEdge);
    }
}