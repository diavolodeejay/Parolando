using System;
using System.Collections.Generic;
using System.Xml;

namespace Parolando
{
    public delegate void EventHandler(object sender, EventArgs e);

    internal class Gioco
    {
        //Per comunicare con il form!
        public EventHandler Comunica;

        //Metodo che carica il file XML
        public void Carica()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("italian.xml");
            albero = new Trie();
            foreach (XmlNode nodo in doc.DocumentElement.ChildNodes)
            {
                if (nodo.Name.Equals("word"))
                {
                    Parola tmp = new Parola(nodo.InnerText.ToString());
                    albero.InsertNode(tmp);
                }
            }
            Console.WriteLine("fine");
        }

        private Trie albero;
        private Random rnd = new Random();
        private Giocatore[] players;

        //Fa avviare il gioco.
        public void Gioca(int numeroPlayers)
        {
            ComunicaConForm(new StringEventArgs(""));
            ComunicaConForm(new StringEventArgs("Inizia il gioco...\n"));
            Sacchetto sacca = new Sacchetto();
            players = new Giocatore[numeroPlayers];
            for (int count = 0; count < players.Length; count++)
            {
                players[count] = new Giocatore(count.ToString());
            }
            //Pescaggio iniziale
            for (int a = 0; a < numeroPlayers; a++)
            {
                players[a].Pesca(sacca);
            }
            int gioc = 0;
            //8 turni! Quindi il for si ripete 8 volte... no?
            for (int t = 0; t < 8; t++)
            {
                //Genera la stringa, e poi rimuove il "!" e salva la sua posizione e genera il moltiplicatore bonus per la casella.
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
                //fa pescare al giocatore
                int punti = 0;
                //Tramite gli eventi comunico con il form
                ComunicaConForm(new StringEventArgs("Turno " + (t + 1).ToString() + ". Gioca il giocatore " + (gioc + 1).ToString() + ".\n"));
                players[gioc].Pesca(sacca);
                //Fa pensare al giocatore la parola
                string parolaInserita = players[gioc].Pensa(albero, stringaGenerata);
                //Se il giocatore restituisce null perchè non ha trovato nessuna parola
                if (parolaInserita == null)
                {
                    //se il giocatore può scambiare tutte le lettere allora lo fa
                    if (players[gioc].mulligato == false)
                    {
                        ComunicaConForm(new StringEventArgs("Il Giocatore ha ripescato nuove lettere!\n"));
                        players[gioc].Mulligan();
                        players[gioc].Pesca(sacca);
                        parolaInserita = players[gioc].Pensa(albero, stringaGenerata);
                    }
                    //se non può ripescare (perchè lo aveva già fatto) oppure non trova neppure dopo aver ripescato niente, allora gli do 0 punti
                    if (parolaInserita == null)
                    {
                        punti = 0;
                    }
                    else
                    {
                        //calcolo dei punti
                        punti = CalcolaPunti(parolaInserita, posizioneBonus, Bonus);
                        players[gioc].Punti += punti;
                    }
                }
                else
                {
                    //calcolo dei punti
                    punti = CalcolaPunti(parolaInserita, posizioneBonus, Bonus);
                    players[gioc].Punti += punti;
                }
                //Preparo la stringa per comunicare, devo rimpiazzare i caratteri nulli con trattini sennò poi vengono persi quando converto in stringa per stampare
                List<char> supert = new List<char>();
                foreach (char c in stringaGenerata)
                {
                    if (c == '\0')
                    {
                        supert.Add('-');
                    }
                    else
                    {
                        supert.Add(c);
                    }
                }
                ComunicaConForm(new StringEventArgs("La lettera vincolante è " + string.Join(" ", supert) + " e la posizone bonus è " + posizioneBonus + " con un moltiplicatore di " + Bonus + " punti.\n"));
                ComunicaConForm(new StringEventArgs("Il giocatore ha creato la parola \"" + parolaInserita + "\" e ha guadagnato " + punti.ToString() + " punti.\n\n"));

                //faccio avanzare il giocatore successivo. Se raggiungo il limite giocatori riparte il giocatore 0
                gioc++;
                if (gioc >= numeroPlayers)
                {
                    gioc = 0;
                }
            }
            //Funzione di fine partita!
            FinePartita();
        }

        //Genera la lettera vincolante e mette un "!" nella casella bonus
        public char[] GeneraTurno()
        {
            int n = rnd.Next(0, 5);
            Dictionary<int, char> Converti = new Dictionary<int, char>()
            {
                {0,'a'}, {1,'e'}, {2,'i'}, {3,'o'}, {4,'u'}
            };
            char r = Converti[n];
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

        //Finita la partita trovo il giocatore con punteggio massimo e lo annuncio
        private void FinePartita()
        {
            int max = 0;
            string NomeVincitore = "";
            foreach (Giocatore g in players)
            {
                if (g.Punti > max)
                {
                    max = g.Punti;
                    NomeVincitore = (int.Parse(g.Nome) + 1).ToString();
                }
            }
            ComunicaConForm(new StringEventArgs("Ha vinto il giocatore " + NomeVincitore + " con " + max.ToString() + " punti!\n"));
        }

        //Calcolo i punti tramite un dizionario
        private int CalcolaPunti(string parola, int posizioneBonus, int Bonus)
        {
            int ris = 0;
            for (int a = 0; a < parola.Length; a++)
            {
                int t = 0;
                t = ConvertiInPunti(parola[a]);
                //se è la posizione bonus moltiplico per il bonus
                if (a == posizioneBonus)
                {
                    t = t * Bonus;
                }
                ris += t;
            }
            return ris;
        }

        //Dizionario di conversione
        public static int ConvertiInPunti(char c)
        {
            Dictionary<char, int> Converti = new Dictionary<char, int>()
            {
                {'a',1 }, {'b', 4}, {'c', 1}, {'d', 4}, {'e', 1}, {'f', 4}, {'g', 4}, {'h', 8}, {'i', 1}, {'j', 10}, {'k', 10}, {'l', 2}, {'m', 2}, {'n', 2}, {'o', 1}, {'p', 3}, {'q', 10}, {'r', 1}, {'s', 1}, {'t', 1}, {'u', 4}, {'v', 4}, {'w', 10}, {'x', 10}, {'y', 10}, {'z', 8}
            };
            int ris = Converti[c];
            return ris;
        }

        //Eventi
        private void ComunicaConForm(StringEventArgs e)
        {
            EventHandler handler = Comunica;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}