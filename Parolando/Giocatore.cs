using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parolando
{
    class Giocatore
    {
        private string mNome;
        public Sacchetto sacca;


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


        public Giocatore(string nome)
        {
            this.mNome = nome;
            this.sacca = new Sacchetto();
        }
    }
}
