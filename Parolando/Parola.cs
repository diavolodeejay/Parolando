    namespace Parolando
{
    internal class Parola
    {
        //La parola
        public string parola { get; set; }

        //Chiave della parola (le lettere in ordine alfabetico)
        public string key { get; set; }

        /// <summary>
        /// Costruttore di parola
        /// </summary>
        /// <param name="parola">La parola che verrà inserita</param>
        public Parola(string parola)
        {
            this.parola = parola;
            key = CalcolaChiave();
        }

        /// <summary>
        /// Calcola la chiave della parola
        /// </summary>
        /// <returns>La chiave</returns>
        private string CalcolaChiave()
        {
            char[] chi = new char[parola.Length];
            for (int a = 0; a < parola.Length; a++)
            {
                chi[a] = parola[a];
            }
            int b = 0, c = 1;
            int lun = chi.Length - 1;
            while (lun > 0)
            {
                while (c <= lun)
                {
                    if (chi[b] > chi[c])
                    {
                        char t = chi[b];
                        chi[b] = chi[c];
                        chi[c] = t;
                    }
                    b++;
                    c++;
                }
                lun--;
                b = 0;
                c = 1;
            }
            string ris = new string(chi);
            return ris;
        }
    }
}