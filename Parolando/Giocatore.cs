using System;
using System.Collections.Generic;
using System.Linq;

namespace Parolando
{
    internal class Giocatore
    {
        private Random rnd = new Random();
        private string mNome;
        public List<string> sacchetto;
        private int mPunti;

        public string Nome
        {
            get
            {
                return mNome;
            }
            set
            {
                mNome = value;
            }
        }

        public int Punti
        {
            get
            {
                return mPunti;
            }
            set
            {
                mPunti = value;
            }
        }

        public Giocatore(string nome)
        {
            this.mNome = nome;
            sacchetto = new List<string>();
            this.mPunti = 0;
        }

        public void Pesca(Sacchetto sacca)
        {
            while (sacchetto.Count < 8)
            {
                int n = rnd.Next(0, sacca.SacchettoPubblico.Count);
                sacchetto.Add(sacca.SacchettoPubblico.ElementAt(n));
                sacca.SacchettoPubblico.RemoveAt(n);
            }
        }

        public string Pensa(Trie albero, char[] Generata)
        {
            //TODO: 
            //-Mulligan
            //-Se anche dopo mulligan non ho niente, taglio tutte le parole più lunghe o uguali alla posizione della lettera bonus e penso intensamente.
            List<Parola> ris = albero.Cerca(sacchetto.ToString());
            foreach (Parola p in ris)
            {
                if (p.parola.Length > 8 && p.parola.Length == 1)
                {
                    ris.Remove(p);
                    continue;
                }
                char[] tmp = p.parola.ToArray();
                bool compatibile = false;
                for (int a = 0; a < tmp.Length; a++)
                {
                    if (tmp[a] == Generata[a])
                    {
                        compatibile = true;
                    }
                }
                if (compatibile == false)
                {
                    ris.Remove(p);
                    continue;
                }
            }
            string r = PensaDiPiu(ris);
            return r;
        }

        public string PensaDiPiu(List<Parola> parole)
        {
            int[] Punti = new int[parole.Count];
            for (int a = 0; a < parole.Count; a++)
            {
                Parola p = parole[a];
                foreach (char c in p.parola)
                {
                    Punti[a] += Gioco.ConvertiInPunti(c);
                }
            }
            int max = Punti.Max();
            int indexMax = Punti.ToList().IndexOf(max);
            return parole[indexMax].parola;
        }
    }
}