using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AsiakasClassLibrary
{
    public class Asiakas                                // public class näkyvä kaikille 
    {
        public string Nimi { get; set; }                //CONSTRUCTOR -- luodaan alle 2 erilaista constructor methodia 
        public int PalveluTaso { get; set; }            
        public DateTime TilauksenAlkupvm { get; set; }
        public bool Kokeilujakso { get; set; }
        public int TilauksenPituus { get; set; }        // 0 jos kuukausittainen maksu, muuten 3/6/12kk mitkä käsitellään laskutuksessa
        public int AsiakasID { get; set; }
          

        public Asiakas()                                
        {
            this.Nimi = "Tyhjä";                  // constructor method1 (default method kun luodaan asiakas jolla ei ole arvoja)
            this.PalveluTaso = 0;
          //  this.PalveluTasoHinta = 0.00M;
            this.TilauksenAlkupvm = DateTime.Now;
            this.Kokeilujakso = (2 == 3);  // tähän if tyyliin
            this.TilauksenPituus = 12;   // default arvo jos ei tilaukselle pituutta 
            this.AsiakasID = 0;
        }

        public Asiakas(string n, int pt, DateTime tap, bool kj, int tp, int id) // constructor method2  (parametrit sisältävä constructor method)
        {
            Nimi = n;
            PalveluTaso = pt;
            TilauksenAlkupvm = tap;
            Kokeilujakso = kj;
            TilauksenPituus = tp;
            AsiakasID = id;
        }

        public override string ToString()
        {
            return "Asiakas: " + Nimi + "Palvelutaso: " + PalveluTaso;
        }


    }
}
