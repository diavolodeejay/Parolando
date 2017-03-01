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
        private bool mulligato = false;

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
            List<Parola> ris = albero.Cerca(string.Join("", sacchetto));
            List<Parola> copy = new List<Parola>();
            foreach (Parola pp in ris)
            {
                copy.Add(pp);
            }
            foreach (Parola p in ris)
            {
                if (p.parola.Length > 8 || p.parola.Length == 1)
                {
                    copy.Remove(p);
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
                    copy.Remove(p);
                    continue;
                }
            }
            ris.Clear();
            foreach (Parola p in copy)
            {
                ris.Add(p);
            }
            if (ris.Count == 0 && !mulligato)
            {
                return null;
            }
            else if (mulligato == true)
            {
                ris = RicercaCorta(albero, Generata);
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
                    Punti[a] += Form1.ConvertiInPunti(c);
                }
            }
            int max = 0;
            try
            {
                max = Punti.Max();
            }
            catch
            {
                return null;
            }
            int indexMax = Punti.ToList().IndexOf(max);
            
            foreach(char cc in parole[indexMax].parola)
            {
                sacchetto.Remove(cc.ToString());
            }
            return parole[indexMax].parola;
        }

        public void Mulligan()
        {
            this.Punti -= 5;
            this.sacchetto.Clear();
            this.mulligato = true;
        }

        private List<Parola> RicercaCorta(Trie albero, char[] generata)
        {
            List<Parola> ris = albero.Cerca(string.Join("", sacchetto));
            List<Parola> copy = new List<Parola>();
            foreach (Parola pp in ris)
            {
                copy.Add(pp);
            }
            int pos = 0;
            for (int a = 0; a < generata.Length; a++)
            {
                if (generata[a] != '\0')
                {
                    pos = a;
                }
            }
            pos++;
            foreach (Parola p in ris)
            {
                if (p.parola.Length >= pos || p.parola.Length == 1)
                {
                    copy.Remove(p);
                    continue;
                }
            }
            /*ris.Clear();
            foreach(Parola pp in copy)
            {
                ris.Add(pp);
            }*/
            return copy;
        }
    }
}