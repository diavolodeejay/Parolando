using System;
using System.Xml;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Parolando
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //Carica subito l'XML prima della GUI, in modo da avere il trie già fatto appena l'utente può interagire con il programma
            Carica();
            InitializeComponent();
        }

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



        private void button1_Click(object sender, EventArgs e)
        {
            //Fa partire la partita con 2 giocatori!
            Gioca(2);
        }

        private Trie albero;
        private Random rnd = new Random();
        private Giocatore[] players;

        //Fa avviare il gioco. 
        public void Gioca(int numeroPlayers)
        {
            label2.Text = "Inizia il gioco...\n";
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
                if (parolaInserita == null)
                {
                    players[gioc].Mulligan();
                    players[gioc].Pesca(sacca);
                    parolaInserita = players[gioc].Pensa(albero, stringaGenerata);
                    if (parolaInserita == null)
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
                label2.Text += "Turno " + t.ToString() + ". Gioca il giocatore " + (gioc + 1).ToString() + ".\n";
                List<char> supert = new List<char>();
                foreach(char c in stringaGenerata)
                {
                    if(c == '\0')
                    {
                        supert.Add('-');
                    }
                    else
                    {
                        supert.Add(c);
                    }
                }
                label2.Text += "La lettera vincolante è " + string.Join(" ", supert) + " e la posizone bonus è " + posizioneBonus + " con un moltiplicatore di " + Bonus + " punti.\n";
                label2.Text += "Il giocatore ha creato la parola \"" + parolaInserita + "\" e ha guadagnato " + punti.ToString() + " punti.\n\n";

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
            int n = rnd.Next(0, 26);
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

        private void FinePartita()
        {
            int max = 0;
            string NomeVincitore = "";
            foreach (Giocatore g in players)
            {
                if (g.Punti > max)
                {
                    max = g.Punti;
                    NomeVincitore = (int.Parse(g.Nome)+1).ToString();
                }
            }
            label2.Text += "Ha vinto il giocatore " + NomeVincitore + " con " + max.ToString() + " punti!\n";
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