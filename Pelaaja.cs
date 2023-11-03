using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_work
{
    // Luokka pelaaja tietoja varten
    public class Pelaaja
    {
        public string Etunimi { get; set; }
        public string Sukunimi { get; set; }
        public int Syntymavuosi { get; set; }
        public int Voitot { get; set; }
        public int Tappiot { get; set; }
        public int Tasapelit { get; set; }
        public double PelienYhteiskesto { get; set; }
        public string PelienYhteiskestoMuoto
        {
            get
            {
                int pelienYhteiskestoSekunneissa = (int)Math.Floor(PelienYhteiskesto);
                int minuutit = pelienYhteiskestoSekunneissa / 60;
                int sekunnit = pelienYhteiskestoSekunneissa % 60;
                return $"{minuutit} min {sekunnit} s";
            }
        }
    }
}
