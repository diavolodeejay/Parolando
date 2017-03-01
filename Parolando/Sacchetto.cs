using System.Collections.Generic;
using System.Linq;

namespace Parolando
{
    internal class Sacchetto
    {
        private string[] msacc = new string[128];
        public List<string> SacchettoPubblico;

        public Sacchetto()
        {
            for (int a = 0; a < 48; a = a + 4)
            {
                msacc[a] = "a";
                msacc[a + 1] = "e";
                msacc[a + 2] = "i";
                msacc[a + 3] = "o";
            }
            for (int a = 48; a < 76; a = a + 7)
            {
                msacc[a] = "u";
                msacc[a + 1] = "b";
                msacc[a + 2] = "d";
                msacc[a + 3] = "f";
                msacc[a + 4] = "g";
                msacc[a + 5] = "p";
                msacc[a + 6] = "v";
            }
            for (int a = 76; a < 104; a = a + 4)
            {
                msacc[a] = "c";
                msacc[a + 1] = "r";
                msacc[a + 2] = "s";
                msacc[a + 3] = "t";
            }
            for (int a = 104; a < 122; a = a + 3)
            {
                msacc[a] = "l";
                msacc[a + 1] = "m";
                msacc[a + 2] = "n";
            }
            for (int a = 122; a < 128; a = a + 3)
            {
                msacc[a] = "h";
                msacc[a + 1] = "q";
                msacc[a + 2] = "z";
            }
            SacchettoPubblico = msacc.ToList<string>();
        }
    }
}