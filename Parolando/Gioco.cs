using System;
using System.Collections.Generic;
using System.Xml;

namespace Parolando
{
    internal class Gioco
    {
        private Trie albero;
        private Random rnd = new Random();

        //TODO: Sposta sta cosa in Form1
        //TODO: GUI
        private Giocatore[] players;

        public void Carica()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("italian.xml");
            albero = new Trie();
            foreach(XmlNode nodo in doc.DocumentElement.ChildNodes)
            {
                if (nodo.Name.Equals("word"))
                {
                    Parola tmp = new Parola(nodo.InnerText.ToString());
                    albero.InsertNode(tmp);
                  //  Console.WriteLine(tmp.parola.ToString());
                }
            }
            Console.WriteLine("fine");
        }

        public void Gioca(int numeroPlayers)
        {
            Sacchetto sacca = new Sacchetto();
            players = new Giocatore[numeroPlayers];
            for (int count = 0; count < players.Length; count++)
            {
                players[count] = new Giocatore(count.ToString());
            }
            //metodo che fa fare a ogni giocatore la giocata
            //-Una giocata consiste in
            //--Pesca
            //--Creazione campo, con lettera vincolante e casella bonus
            //--Ricerca parole possibili tramite treenode.search
            //--Filtraggio con parole davvero utilizzabili
            //--Calcolo parola con punteggio più alto tra quelle utilizzabili
            //--Inserimento parola

            for (int a = 0; a < numeroPlayers; a++)
            {
                players[a].Pesca(sacca);
            }
            int gioc = 0;
            for (int t = 0; t < 8; t++)
            {
                char[] stringaGenerata = GeneraTurno();
                int posizioneBonus = 0;
                int Bonus = 0;
                for (int a = 0; a < 8; a++)
                {
                    if (stringaGenerata[a] == '!')
                    {
                        stringaGenerata[a] = '\0';
                        Bonus = rnd.Next(2, 5);
                        posizioneBonus = a;
                    }
                }
                int punti = 0;
                players[gioc].Pesca(sacca);
                string parolaInserita = players[gioc].Pensa(albero, stringaGenerata);
                if(parolaInserita == null)
                {
                    players[gioc].Mulligan();
                    players[gioc].Pesca(sacca);
                    parolaInserita = players[gioc].Pensa(albero, stringaGenerata);
                    if(parolaInserita == null)
                    {
                        punti = 0;
                    }
                    else
                    {
                        punti = CalcolaPunti(parolaInserita, posizioneBonus, Bonus);
                        players[gioc].Punti += punti;
                    }
                }
                else
                {
                    punti = CalcolaPunti(parolaInserita, posizioneBonus, Bonus);
                    players[gioc].Punti += punti;
                }
                Console.WriteLine("Turno {0}. Gioca il giocatore {1}.",t, gioc);
                Console.WriteLine("Il giocatore ha creato la parola {0} e ha guadagnato {1} punti", parolaInserita, punti);
                Console.WriteLine("-------");

                gioc++;
                if (gioc >= numeroPlayers)
                {
                    gioc = 0;
                }
            }
            FinePartita();
        }

        public char[] GeneraTurno()
        {
            int n = rnd.Next(0, 27);
            n = n + 97;
            char r = Convert.ToChar(n);
            int p = rnd.Next(0, 8);
            char[] ris = new char[8];
            int bonus = rnd.Next(0, 8);
            while (bonus == p)
            {
                bonus = rnd.Next(0, 8);
            }
            ris[bonus] = '!';
            ris[p] = r;
            return ris;
        }

        void FinePartita()
        {
            int max = 0;
            string NomeVincitore = "";
            foreach(Giocatore g in players)
            {
                if(g.Punti > max)
                {
                    max = g.Punti;
                    NomeVincitore = g.Nome;
                }
            }
            Console.WriteLine("Ha vinto il giocatore {0} con {1} punti!", NomeVincitore, max);
        }

        private int CalcolaPunti(string parola, int posizioneBonus, int Bonus)
        {
            int ris = 0;
            for (int a = 0; a < parola.Length; a++)
            {
                int t = 0;
                t = ConvertiInPunti(parola[a]);
                if (a == posizioneBonus)
                {
                    t = t * Bonus;
                }
                ris += t;
            }
            return ris;
        }

        public static int ConvertiInPunti(char c)
        {
            Dictionary<char, int> Converti = new Dictionary<char, int>()
            {
                {'a',1 }, {'b', 4}, {'c', 1}, {'d', 4}, {'e', 1}, {'f', 4}, {'g', 4}, {'h', 8}, {'i', 1}, {'j', 10}, {'k', 10}, {'l', 2}, {'m', 2}, {'n', 2}, {'o', 1}, {'p', 3}, {'q', 10}, {'r', 1}, {'s', 1}, {'t', 1}, {'u', 4}, {'v', 4}, {'w', 10}, {'x', 10}, {'y', 10}, {'z', 8}
            };
            int ris = Converti[c];
            return ris;
        }

    }
}