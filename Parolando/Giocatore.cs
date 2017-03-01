using System;
using System.Collections.Generic;
using System.Linq;

namespace Parolando
{
    internal class Giocatore
    {
        //variabili
        private Random rnd = new Random();

        private string mNome;
        public List<string> sacchetto;
        private int mPunti;
        public bool mulligato = false;

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

        //costruttore
        public Giocatore(string nome)
        {
            this.mNome = nome;
            sacchetto = new List<string>();
            this.mPunti = 0;
        }

        //funzione di pesca, continua a ripeterla finchè non ha 8 lettere in mano.
        //Il sacchetto è comune a tutti quindi viene passato da Gioco
        public void Pesca(Sacchetto sacca)
        {
            while (sacchetto.Count < 8)
            {
                //Calcola un numero casuale, piglia la lettera con index == numero casuale e la rimuove dal sacchetto
                int n = rnd.Next(0, sacca.SacchettoPubblico.Count);
                sacchetto.Add(sacca.SacchettoPubblico.ElementAt(n));
                sacca.SacchettoPubblico.RemoveAt(n);
            }
        }

        //Pensa cosa fare!
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
                //Se la parola è più lunga di 8 non ci sta nel campo. Se è lunga 1 non è valida
                if (p.parola.Length > 8 || p.parola.Length == 1)
                {
                    copy.Remove(p);
                    continue;
                }
                //Controllo se il carattere vincolante è in comune con la parola, se lo è aggiungo la parola a una nuova lista
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
            //risposto tutto su ris.
            foreach (Parola p in copy)
            {
                ris.Add(p);
            }
            //se ris è vuoto e in questo turno non ho ancora svuotato il sacchetto, ritorno null così posso farlo
            if (ris.Count == 0 && !mulligato)
            {
                return null;
            }
            //altrimenti cerco una parola valida che finisca prima del carattere vincolante
            else if (mulligato == true)
            {
                ris = RicercaCorta(albero, Generata);
            }
            string r = PensaDiPiu(ris);
            return r;
        }

        //cerca, tra le parole rimaste, quella che vale più punti, e dopo averla scelta rimuove le sue lettere dal sacchetto del giocatore.
        //Restituisce la parola
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

            foreach (char cc in parole[indexMax].parola)
            {
                sacchetto.Remove(cc.ToString());
            }
            return parole[indexMax].parola;
        }

        //in caso il giocatore decida di svuotare il proprio sacchetto
        public void Mulligan()
        {
            this.Punti -= 5;
            this.sacchetto.Clear();
            this.mulligato = true;
        }

        //Ricerca solo le parole che ci "stanno" prima del carattere vincolante e sono più lunghe di 1
        private List<Parola> RicercaCorta(Trie albero, char[] generata)
        {
            List<Parola> ris = albero.Cerca(string.Join("", sacchetto));
            List<Parola> copy = new List<Parola>();
            foreach (Parola pp in ris)
            {
                copy.Add(pp);
            }
            int pos = 0;
            //trovo la posizione del carattere vincolante
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
            return copy;
        }
    }
}