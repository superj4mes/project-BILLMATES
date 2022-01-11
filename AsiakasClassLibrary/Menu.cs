using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsiakasClassLibrary
{
    public class Menu
    {
        static public void TulostaPääMenu() //uusi menun ulkonäkö
        {
            Console.Clear();
            Console.WriteLine("PÄÄMENU\n");
            
            Console.WriteLine("[1] Lisää Asiakas");
            Console.WriteLine("[2] Asiakkaiden muokkaus");
            Console.WriteLine("[3] Palvelutasojen muokkaus");
            Console.WriteLine("[4] Raportointi");
            Console.WriteLine("[5] Tallenna muutokset ja poistu ohjelmasta");

            //Console.Clear();
            //Console.WriteLine("Lisää Asiakas...................(1)");
            //Console.WriteLine("Asiakkaiden muokkaus............(2)");
            //Console.WriteLine("Palvelutasojen muokkaus.........(3)");
            //Console.WriteLine("Poistu ohjelmasta...............(4)");
        }
        static public void TulostaAsiakasMuokkausMenu() 
        {
            Console.WriteLine("Mitä tietoa haluat muokata?");
            Console.WriteLine("Nimi............................(1)");
            Console.WriteLine("Palvelutaso.....................(2)");
            Console.WriteLine("Kokeilujakson tila..............(3)");
            Console.WriteLine("Laskutuskausi...................(4)");
            Console.WriteLine("Poista asiakas..................(5)");
        }

        static public void TulostaPalveluTasoMenu()
        {
            Console.WriteLine("Mitä haluat muokata?");
            Console.WriteLine("Palvelutason nimi................(1)");
            Console.WriteLine("Palvelutason hinta...............(2)");

        }
        static public void Palvelutasovalikko()
        {
            Console.WriteLine("PALVELUTASOJEN MUOKKAUS\n");
            Console.WriteLine("Mitä haluat muokata?");
            Console.WriteLine("Lisää uusi palvelutaso...........(1)");
            Console.WriteLine("Muokkaa palvelutasoa.............(2)");
            Console.WriteLine("Poista palvelutaso...............(3)");
            Console.WriteLine("Palaa pääohjelmaan...............(4)");
        }
        static public void TulostaPalvelutasot(Palvelut non)
        {
            foreach (var _asiakas in non.Palvelutasolista)
            {
                Console.WriteLine("Palvelutaso: " + _asiakas.Nimi + " (ID: " +  _asiakas.TasoID + ")");
            }
        }
    }
}
