using System;
using System.Collections.Generic;

namespace Parolando
{
    internal class TrieNode
    {
        private TrieNode[] mlettere;
        private List<Parola> parole;

        //Costruttore del nodo
        public TrieNode(List<Parola> data)
        {
            parole = data;
            mlettere = new TrieNode[26];
            for (int a = 0; a < mlettere.Length; a++)
            {
                mlettere[a] = null;
            }
        }

        public TrieNode[] Lettere
        {
            get
            {
                return mlettere;
            }
        }

        public List<Parola> Parole
        {
            get
            {
                return this.parole;
            }
            set
            {
                this.parole = value;
            }
        }

        public void Insert(Parola insert, int pos = 0)
        {
            //non si sa mai
            insert.key = insert.key.ToLower();
            //la lunghezza della parola è uguale alla profondità dell'albero
            if (pos != insert.key.Length)
            {
                //la posizione nell'array è fisso per ogni lettera, quindi se ho la lattera A dovrò andare nel Nodo 0
                int value = TrieNode.CharToInt(insert.key[pos]);
                if (Lettere[value] == null)
                {
                    Lettere[value] = new TrieNode(new List<Parola>());
                }
                //Ricorsioni!
                Lettere[value].Insert(insert, pos = pos + 1);
            }
            else
            {
                this.Parole.Add(insert);
            }
        }

        //Converte il char in intero. Potere dell'ASCII
        public static int CharToInt(char v)
        {
            //ASCII
            return Convert.ToInt32(v) - 97;
        }
    }



    internal class Trie
    {
        //"I'm grooot!"
        private TrieNode groot;

        public Trie()
        {
            Root = new TrieNode(null);
        }

        public TrieNode Root
        {
            get
            {
                return groot;
            }
            set
            {
                groot = value;
            }
        }

        public void InsertNode(Parola Valore)
        {
            Root.Insert(Valore);
        }

        /// <summary>
        /// Cerca tutte le parole possibili con le lettere che passo
        /// </summary>
        /// <param name="lettere">Le lettere con cui posso costruire le parole</param>
        /// <returns>Parole possibili</returns>
        public List<Parola> Cerca(string lettere)
        {
            TrieNode nodo;
            Queue<TrieNode> coda = new Queue<TrieNode>();
            List<Parola> Trovati = new List<Parola>();
            Parola p = new Parola(lettere);
            coda.Enqueue(groot);

            foreach (char c in p.key)
            {
                int l = coda.Count;
                for (int i = 0; i < l; i++)
                {
                    nodo = coda.Dequeue();
                    if (nodo.Parole != null)
                    {
                        foreach (Parola pp in nodo.Parole)
                        {
                            Trovati.Add(pp);
                        }
                    }
                    coda.Enqueue(nodo);
                    if (nodo.Lettere[TrieNode.CharToInt(c)] != null)
                    {
                        coda.Enqueue(nodo.Lettere[TrieNode.CharToInt(c)]);
                    }
                }
            }
            List<Parola> ris = Pulisci(Trovati);
            return ris;
        }

        /// <summary>
        /// Fa una pulizia dei doppioni
        /// </summary>
        /// <param name="Trovati">Risposta</param>
        /// <returns></returns>
        private List<Parola> Pulisci(List<Parola> Trovati)
        {
            List<Parola> ris = new List<Parola>();
            bool agg = true;
            foreach (Parola p in Trovati)
            {
                agg = true;
                foreach (Parola pp in ris)
                {
                    if (pp.parola == p.parola)
                    {
                        agg = false;
                    }
                }
                if (agg)
                {
                    ris.Add(p);
                }
            }
            return ris;
        }
    }
}