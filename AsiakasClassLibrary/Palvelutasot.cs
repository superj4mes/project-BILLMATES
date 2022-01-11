using System;
using System.Collections.Generic;
using System.Text;

namespace AsiakasClassLibrary
{
    public class Palvelutasot
    {
        public string Nimi { get; set; }                //CONSTRUCTOR -- luodaan alle 2 erilaista constructor methodia 
        public decimal PalveluTasoHinta { get; set; }
        public int TasoID { get; set; }

        public Palvelutasot()
        {
            this.Nimi = "Tyhjä";                  // constructor method1 (default method kun luodaan asiakas jolla ei ole arvoja)
            this.PalveluTasoHinta = 0.00M;
            this.TasoID = 0;
        }

        public Palvelutasot(string n, decimal pth, int idp) // constructor method2  (parametrit sisältävä constructor method)
        {
            Nimi = n;
            PalveluTasoHinta = pth;
            TasoID = idp;
        }

    }
}
