using System;
using System.Windows.Forms;

namespace Parolando
{
    public partial class Form1 : Form
    {
        //By Gabriele Di Nuovo, 4AII
        //SE CI SONO COSE TROPPO STRANE. Tipo l'internal nella classe li sotto, è "colpa" di CodeMaid http://www.codemaid.net/ che mette il codice in orine (spazi, e linee vuote).
        private Gioco game = new Gioco();

        public Form1()
        {
            //Carica subito l'XML prima della GUI, in modo da avere il trie già fatto appena l'utente può interagire con il programma
            game.Carica();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            output.Text = "";
            //Fa partire la partita con 2 giocatori!
            game.Comunica = Stampa;
            game.Gioca(2);
        }

        private void Stampa(object sender, EventArgs e)
        {
            StringEventArgs args = (StringEventArgs)e;
            //Aggiunge alla label il nuovo testo
            output.Text += args.value;
        }
    }

    internal class StringEventArgs : EventArgs
    {
        public StringEventArgs(string value)
        {
            this.value = value;
        }

        public string value { get; set; }
    }
}