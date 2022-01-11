using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AsiakasClassLibrary
{
    public class AsiakasListat
    {
        public List<Asiakas> AsiakasLista { get; set; }


        public AsiakasListat()
        {
            AsiakasLista = new List<Asiakas>();
           
        }

        /*
        public IEnumerator<Asiakas> GetEnumerator()
        {
            return AsiakasLista.GetEnumerator();
        }
        */
        public void TulostaAsiakasTiedot()
        {
            foreach (var asiakas in AsiakasLista)
            {
                Console.WriteLine(asiakas.Nimi);
                Console.Write(asiakas.PalveluTaso);
            }
        }



    }
    public class Palvelut
    {
        public List<Palvelutasot> Palvelutasolista { get; set; }
        public Palvelut()
        {
            Palvelutasolista = new List<Palvelutasot>();
        }
        
    }

}
