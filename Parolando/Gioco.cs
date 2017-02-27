using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parolando
{
    class Gioco
    {
        Giocatore[] players;
        public void Gioca(int numeroPlayers)
        {
            players = new Giocatore[numeroPlayers];
            for(int count = 0; count<players.Length;count++)
            {
                players[count] = new Giocatore(count.ToString());
            }

        }
    }
}
