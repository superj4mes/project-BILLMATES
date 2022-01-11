using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiakasClassLibrary;
using Newtonsoft.Json;

namespace project_BILLMATES
{
    class Program
    {
        static void Main(string[] args)
        {

            bool jatkaOhjelmaa = true;
            Console.WriteLine("Initializing...");
            //Alustetaan asiakaslistat "listat"
            AsiakasListat listat = new AsiakasListat();

            //Alustetaan palvelutaso-objektien lista "palvelulistaus"
            Palvelut palvelulistaus = new Palvelut();
            
            //Testataan onko olemassa varasto.json ennestään, jos on niin luetaan se äsken alustetun listat-listan päälle.
            string curFile = @"varasto.json";
            if (File.Exists(curFile))
            {
                string asiakkaatLuettuTiedostosta = File.ReadAllText(curFile); // laitettu muuttuja polun sijaan
                listat = JsonConvert.DeserializeObject<AsiakasListat>(asiakkaatLuettuTiedostosta);
            }

            //Tehdään sama palvelulistaus-listalle.e
            string nextFile = @"palvelut.json";
            if (File.Exists(nextFile))
            {
                string palvelutLuettuTiedostosta = File.ReadAllText(nextFile); //laitettu muuttuja polun sijaan
                palvelulistaus = JsonConvert.DeserializeObject<Palvelut>(palvelutLuettuTiedostosta);

            }

            Console.WriteLine("Done.");
            
            while (jatkaOhjelmaa)

            {
                Menu.TulostaPääMenu();
                string menu = Console.ReadLine();

                switch (menu)
                {
                    case "1":
                        Console.WriteLine("Valitsit komennon 1: Lisää asiakas");   

                        string restart = string.Empty;

                        do
                        {   //Luodaan asiakas objekti joka käyttää luokan parametreja
                            Asiakas asiakas = new Asiakas();

                            //Pyydetään syötteitä täyttämään kysyttyjä paremetreja
                            Console.WriteLine("Syötä asiakkaan nimi.");
                            asiakas.Nimi = Console.ReadLine();                      // Sitten lopuksi vain TulostaAsiakasTiedot() ja saadaan kaikki lisätyt esille ennen poistumista loopista
                            Console.WriteLine("Anna palvelutason ID");

                            if (palvelulistaus.Palvelutasolista.Count == 0)
                            {
                                Console.WriteLine("Lisää ensin palvelutasoja! Paina mitä tahansa palataksesi päävalikkoon...");
                                Console.ReadKey();
                                break;
                            }
                            else
                            {
                                foreach (var _palvelut in palvelulistaus.Palvelutasolista)
                                {
                                    Console.WriteLine("Taso " + _palvelut.Nimi + " = " + _palvelut.TasoID);
                                }
                            }
                            asiakas.PalveluTaso = Int32.Parse(Console.ReadLine());

                            Console.WriteLine("Onko asiakkaalla 30 päivän koejakso? (Y/N)");
                            var kokeilujakso = Console.ReadLine().ToUpper();
                            if (kokeilujakso == "Y") asiakas.Kokeilujakso = true;
                            else if (kokeilujakso == "N") asiakas.Kokeilujakso = false;
                            else 
                            { 
                                Console.WriteLine("Virheellinen syöte, asiakkaalle ei aseteta kokeilujaksoa.");
                                asiakas.Kokeilujakso = false;                            
                            }


                            Console.WriteLine("Minkä tilausvaihtoehdon asiakas on ostanut? Tuettuja tilausvaihtoehtoja on 1kk (kuukausitilaus), 3kk (kertaosto), 6kk (kertaosto) ja 12kk (kertaosto).");
                            Console.WriteLine("Kuukausitilaukselle ei tule alennusta. 3kk kertatilauksella tulee 5% alennus, 6kk 10% alennus ja 12kk 18% alennus.");
                            asiakas.TilauksenPituus = Int32.Parse(Console.ReadLine());

                            //haetaan listan viimeisen objektin ID ja lisätään +1
                            if (listat.AsiakasLista.Count == 0)
                            {   
                                asiakas.AsiakasID = 1;
                            }
                            else
                            { 
                                var tempID = listat.AsiakasLista.Last();
                                asiakas.AsiakasID = tempID.AsiakasID + 1;
                            }
                            //Muodostetaan lista johon kerätään asiakkaasta saatu tieto
                            //Hyödynnetään listaluokkaa, joka käyttää asiakasluokkaa
                            listat.AsiakasLista.Add(asiakas);

                            //Käydään läpi listan tietoja ja tulostetaan jokaisen lisäyksen päätteeksi kaikki lisäykset
                            foreach (var jokuasiakas in listat.AsiakasLista)
                            {
                                Console.WriteLine("=========");
                                Console.WriteLine(jokuasiakas.Nimi);
                                foreach (var _palvelut in palvelulistaus.Palvelutasolista)
                                {
                                    if (_palvelut.TasoID == jokuasiakas.PalveluTaso)
                                    {
                                        Console.WriteLine("Taso " + _palvelut.Nimi);
                                    }
                                }
                            }



                            //Looppi uuden asiakkaan lisäystä varten. Vastauksen täytyy sisältää yes tai no
                            Console.WriteLine("\nJatketaanko lisäämistä? Y/N");

                            restart = Console.ReadLine().ToUpper();

                            while ((restart != "Y") && (restart != "N"))
                            {
                                Console.WriteLine("Valitse uudestaan.");                
                                Console.WriteLine("\nJatketaanko lisäämistä? Y/N");
                                restart = Console.ReadLine().ToUpper();
                            }

                        }

                        while (restart == "Y");

                        break;

                    case "2":  
                        Console.WriteLine("Minkä asiakkaan tietoja haluat muokata? Anna AsiakasID.");
                        //Tähän lista asiakkaista?
                        foreach (var _asiakkaat in listat.AsiakasLista)
                        {
                            Console.WriteLine("Asiakas: " + _asiakkaat.Nimi + "(ID: " + _asiakkaat.AsiakasID +")");
                        }
                        var ketaMuokataan = Int32.Parse(Console.ReadLine());

                        Asiakas PoistettavaAsiakas = new Asiakas();
                        
                        Asiakas MuokattavaAsiakas = new Asiakas();
                        var poisto_operaatio = 0;

                        /*
                            UUDEN MENURAKENTEEN RIVEJÄ. KEHITYS KESKEN TÄMÄN OSALTA! ÄLÄ POISTA (28.2.2021).
                         
                         var muokattavaAsiakas = Int32.Parse(Console.ReadLine());
                       // Tehdään väliaikainen asiakasobjekti, mihin saadaan tallennettua tiedot

                        // Kopioidaan muokattavan asiakkaan tiedot lambda-ilmaisulla väliaikaiseen objektiin
                        MuokattavaAsiakas = listat.AsiakasLista.Single(r=>r.AsiakasID == ketaMuokataan);

                        MuokattavaAsiakas = listat.AsiakasLista.Single(r=>r.AsiakasID == ketaMuokataan);
                        

                        
                        Uusi menurakenne:
                        Console.WriteLine("Mitä asiakastietoa muokataan?")
                        switch(asiakasvalikko):
                            case "1":
                            // Asiakkaan nimen muokkaus

                            Console.WriteLine("Anna asiakkaan uusi nimi:");
                            var _uusiNimi = Console.ReadLine();
                            MuokattavaAsiakas.Nimi = _uusiNimi;
                      
                       
                            case "2": 
                            // Asiakkaan palvelutason muokkaus
                            Console.WriteLine("Määritä asiakkaalle uusi palvelutaso:\n");
                            Menu.TulostaPalvelutasot(palvelulistaus);
                            var _uusiTaso = Console.ReadLine();
                            MuokattavaAsiakas.PalveluTaso = _uusiTaso;

                            case "3":
                            // Kokeilujakso true/false muokkaus

                            Console.WriteLine("Onko asiakkaalla kokeilujakso? Y/N (default: N)");
                            var _kokeiluJakso = Console.ReadLine().ToUpper();
                            if (_kokeiluJakso == "Y") { 
                            MuokattavaAsiakas.Kokeilujakso = true;
                            }    
                            else
                            {
                                MuokattavaAsiakas.Kokeilujakso = false;
                            }
                            
                            case "4":
                            // Tilaustyypin (bulk/recurring singlemonth) muokkaus
                            Console.WriteLine("Kuinka pitkä kertasopimus asiakkaalla on?")
                            MuokattavaAsiakas.TilauksenPituus = Int32.Parse(Console.ReadLine);
                            
                            case "5":
                            // Asiakkaan poisto-operaatio
                            Console.WriteLine("Haluatko varmasti poistaa asiakkaan " + MuokattavaAsiakas.Nimi + "?");
                            listat.AsiakasLista.Remove(MuokattavaAsiakas);
                            
                            
                            case "6":
                            // Palataan takaisin main-menuun.
                            */
                        foreach (var _asiakas in listat.AsiakasLista)
                        {
                            if (_asiakas.AsiakasID == ketaMuokataan)
                            {

                                Console.WriteLine("\nAsiakas löytyi! [Nimi: " + _asiakas.Nimi + ", palvelutaso: " + palvelulistaus.Palvelutasolista.Single(r=>r.TasoID == _asiakas.PalveluTaso).Nimi + ", kokeilujakso: " + _asiakas.Kokeilujakso + ", tilauksen pituus: " + _asiakas.TilauksenPituus + ".]" +  "\n");
                                Menu.TulostaAsiakasMuokkausMenu();
                                var uusiTieto = "";
                                var muokattavaTieto = Console.ReadLine();

                                //Tulostetaan käyttäjälle lisätietoa valinnan perusteella.
                                if (muokattavaTieto == "1")
                                {
                                    Console.WriteLine("Vaihdetaan asiakkaan nimitieto. Syötä uusi nimi.");
                                }
                                else if (muokattavaTieto == "2")
                                {
                                    Console.WriteLine("Valitse asiakkaan uusi palvelutaso. Anna halutun palvelutason ID:\n");
                                    Menu.TulostaPalvelutasot(palvelulistaus);
                                }
                                else if (muokattavaTieto == "3")
                                {
                                    Console.WriteLine("Muutetaan kokeilujakson tilaa. Hyväksytyt syötteet ovat K (kokeilujakso on) ja E (ei kokeilujaksoa).");
                                }
                                else if (muokattavaTieto == "4")
                                {
                                    Console.WriteLine("Tuetut tilausjaksot ovat 1, 3, 6 ja 12 (kk). Anna haluttu tilausjakso numerona.");
                                }
                                if (muokattavaTieto != "5") { 
                                // Console.WriteLine("Anna uusi arvo");
                                uusiTieto = Console.ReadLine();
                                }
                                // Muokataan tietueet annetun valinnan perusteella.
                                if (muokattavaTieto == "1") _asiakas.Nimi = uusiTieto;
                                else if (muokattavaTieto == "2") _asiakas.PalveluTaso = Int32.Parse(uusiTieto);
                                else if (muokattavaTieto == "3")
                                {
                                    if (uusiTieto.ToUpper() == "K") _asiakas.Kokeilujakso = true;
                                    else if (uusiTieto.ToUpper() == "E") _asiakas.Kokeilujakso = false;
                                }
                                else if (muokattavaTieto == "4") _asiakas.TilauksenPituus = Int32.Parse(uusiTieto);
                                else if (muokattavaTieto == "5")
                                {
                                    PoistettavaAsiakas = listat.AsiakasLista.Single(r=>r.AsiakasID == _asiakas.AsiakasID);
                                    poisto_operaatio = 1;
                                }   
                            }
                        }
                        if (poisto_operaatio == 1)
                        {
                            Console.WriteLine("Lukitaanko vastaus? Haluatko aivan varmasti poistaa asiakkaan " + PoistettavaAsiakas.Nimi + "? (Valinnat K/E)");
                            string valinta = String.Empty;
                            do
                            {
                                valinta = Console.ReadLine().ToUpper();
                                if (valinta == "K")
                                {
                                    listat.AsiakasLista.Remove(PoistettavaAsiakas);
                                }
                            }
                            while (valinta != "K" && valinta != "E");
                            
                        }
                        Console.WriteLine("Asiakkaan tietoja muokattu onnistuneesti. Paina mitä vain palataksesi päävalikkoon.");
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.Clear();
                        Menu.Palvelutasovalikko();


                        string jatka3 = string.Empty;
                        jatka3 = Console.ReadLine();
                        int mikaPalvelutaso;
                        do
                        {

                            if (jatka3 == "1")
                            {
                                Palvelutasot uusiTaso = new Palvelutasot();

                                //lisää palvelutaso
                                Console.WriteLine("Anna palvelutasolle nimi");
                                uusiTaso.Nimi = Console.ReadLine();
                                Console.WriteLine("Anna palvelutason hinta");
                                uusiTaso.PalveluTasoHinta = decimal.Parse(Console.ReadLine());
                                if (palvelulistaus.Palvelutasolista.Count == 0)
                                {
                                    uusiTaso.TasoID = 1;
                                }
                                else
                                {
                                    var tempID = palvelulistaus.Palvelutasolista.Last();
                                    uusiTaso.TasoID = tempID.TasoID + 1;
                                }
                                palvelulistaus.Palvelutasolista.Add(uusiTaso);
                                Console.WriteLine("Lisäsit uuden palvelutason: " + uusiTaso.Nimi);
                            }
                            else if (jatka3 == "2")
                            {
                                //muokkaa palvelutasoa
                                Console.WriteLine("Mitä palvelutasoa haluat muokata?");
                                Menu.TulostaPalvelutasot(palvelulistaus);
                                Console.WriteLine("Valitse palvelutaso syöttämällä muokattavan palvelutason ID.");
                                mikaPalvelutaso = Int32.Parse(Console.ReadLine());

                                foreach (var _taso in palvelulistaus.Palvelutasolista)
                                {
                                    if (_taso.TasoID == mikaPalvelutaso)
                                    {
                                        Console.WriteLine("Muokataan palvelutasoa " + _taso.Nimi + " (ID: " + _taso.TasoID + ")");
                                        Menu.TulostaPalveluTasoMenu();
                                        var muokattavaTieto = Console.ReadLine();
                                        Console.WriteLine("Anna uusi arvo");
                                        var uusiTieto = Console.ReadLine();
                                        if (muokattavaTieto == "1") _taso.Nimi = uusiTieto;
                                        else if (muokattavaTieto == "2") _taso.PalveluTasoHinta = decimal.Parse(uusiTieto);
                                        Console.WriteLine("\nMuokkasit palvelutasoa ID: " + _taso.TasoID + ". Palvelutason tiedot ovat nyt:" );
                                        Console.WriteLine("Nimi: " + _taso.Nimi);
                                        Console.WriteLine("Hinta: " + _taso.PalveluTasoHinta);
                                    }
                                }
                            }
                            //Palvelutason poisto-operaatio
                            else if (jatka3 == "3")
                            {
                                Palvelutasot _PoistoTaso = new Palvelutasot();
                                var _poistetaanko = 0;
                                Console.WriteLine("Poistetaan palvelutaso. Valitse poistettava palvelutaso syöttämällä palvelutason ID. Alla olemassaolevat palvelutasot.");
                                foreach (var _taso in palvelulistaus.Palvelutasolista)
                                {
                                    Console.WriteLine("Taso: " + _taso.Nimi + " (ID: " + _taso.TasoID + ")");
                                }
                                var poistettava = Int32.Parse(Console.ReadLine());
                                foreach (var _tasot in palvelulistaus.Palvelutasolista)
                                {
                                    if (_tasot.TasoID == poistettava)
                                    {
                                        _PoistoTaso = palvelulistaus.Palvelutasolista.Single(r => r.TasoID == _tasot.TasoID);
                                        _poistetaanko = 1;
                                    }

                                }
                                if (_poistetaanko == 1)
                                {
                                    Console.WriteLine("Lukitaanko vastaus? Haluatko aivan varmasti poistaa asiakkaan " + _PoistoTaso.Nimi + "? (Y/N)");
                                    string valinta = String.Empty;
                                    do
                                    {
                                        valinta = Console.ReadLine().ToUpper();
                                        if (valinta == "Y")
                                        {
                                            palvelulistaus.Palvelutasolista.Remove(_PoistoTaso);
                                        }
                                    }
                                    while (valinta != "Y" && valinta != "N");
                                }
                            }
                            Console.WriteLine("Muokkaus onnistunut. Paina mitä vain näppäintä jatkaaksesi...");
                            Console.ReadKey();
                            Console.Clear();
                            Menu.Palvelutasovalikko();

                            jatka3 = Console.ReadLine();

                            if ((jatka3 != "1") && (jatka3 != "2") && (jatka3 != "3") && (jatka3 != "4"))
                            {
                                Console.WriteLine("Valitse uudestaan.");
                                jatka3 = Console.ReadLine();
                            }


                        }

                        while (jatka3 != "4");


                        break;

                    case "4":
                        //Raportointi
                        decimal tulevatMaksut = 0;
                        Console.WriteLine("Moneltako kuukaudelta lasketaan maksut?");
                        var ajanjakso = Int32.Parse(Console.ReadLine());
                        decimal maksutYht = 0M;
                        foreach (var _asiakas in listat.AsiakasLista)
                        {
                            var _taso1 = palvelulistaus.Palvelutasolista.Single(r => r.TasoID == _asiakas.PalveluTaso);

                            //Menu.PalautaAsiakkaanMaksut(_asiakas, _taso1.PalveluTasoHinta);
                            tulevatMaksut = PalautaAsiakkaanMaksutv2(_asiakas, _taso1.PalveluTasoHinta, ajanjakso);
                            maksutYht += tulevatMaksut;
                            //Tulostaa asiakkaan tulevat maksut yhteensä mainiltulta 
                            Console.WriteLine(_asiakas.Nimi + ": " + tulevatMaksut);
                        }
                        Console.WriteLine("Yhteensä maksuja ajanjaksolle on: " + maksutYht);
                        Console.WriteLine("Paina mitä vain palataksesi päävalikkoon.");
                        Console.ReadKey();

                        break;

                    case "5":
                        //Konvertoidaan lista json - muotoon
                        Console.WriteLine("Tallennetaan...");
                        string json = JsonConvert.SerializeObject(listat, Formatting.Indented);

                        //tallennetaan ja pyyhitään lista seuraavaa asiakaslisäystä varten
                        using (StreamWriter tiedosto = new StreamWriter(@"varasto.json", false))
                        {
                            tiedosto.WriteLine(json);
                        }
                        //Konvertoidaan palvelutasolista jsoniksi
                        string json2 = JsonConvert.SerializeObject(palvelulistaus, Formatting.Indented);
                        //Tallennetaan lista tiedostoon
                        using (StreamWriter tiedosto2 = new StreamWriter(@"palvelut.json", false))
                        {
                            tiedosto2.WriteLine(json2);
                        }
                        Console.WriteLine("Muutokset tallennettu.");
                        jatkaOhjelmaa = false;
                        break;
                    default:
                        Console.WriteLine("Invalid expression");
                        break;
                }
            }


            Console.WriteLine("Suljit ohjelman!");
        
            Console.ReadKey();

           
        }

        static decimal PalautaAsiakkaanMaksutv2(Asiakas f, decimal kkhinta, int raportinpituus)
        {
            decimal pmaksut = 0M;
            var raportpaiva = DateTime.Now;
            var ilmainentesti = DateTime.Now;
            var kertaosto = f.TilauksenAlkupvm.AddDays((f.TilauksenPituus * 30));
            var kerta_maksettu = false;
            if (f.Kokeilujakso)
            {
                ilmainentesti.AddDays(30);
                kertaosto = kertaosto.AddDays(30);
            }
            
            for (int i = 0; i<raportinpituus; i++)
            {
                decimal alennus = 1M;
                
                switch (f.TilauksenPituus)
                {
                    case 1:
                        if (f.Kokeilujakso && (kertaosto > raportpaiva) && (raportpaiva <= ilmainentesti))
                        {
                            //Jos kokeilujakso meneillään, ei lisätä raporttiin maksuja
                        }
                        else
                        {
                            //muuten lisätään kuukausimaksu raportille
                            pmaksut += kkhinta;
                        }

                        break;

                    case 3:
                        alennus = 0.95M;
                        if (f.Kokeilujakso && (kertaosto > raportpaiva) && (raportpaiva <= ilmainentesti))
                        {
                            //Jos kokeilujakso meneillään, ei lisätä raporttiin maksuja
                        }
                        else if (kertaosto > raportpaiva)
                        {
                            //Tarkistetaan onko pitkän laskutusajanjakson kausi meneillään
                            if (kerta_maksettu)
                            {
                                //Tarkistetaan onko jo maksettu kertamaksu, jos on niin sitten ei lisätä maksuja raportille 
                            }
                            else
                               //muussa tapauksessa lisätään raportille kertaluontoinen maksu alennus huomioiden
                            pmaksut = kkhinta*alennus*f.TilauksenPituus;
                            kerta_maksettu = true;
                        }
                        //Jos pitkä kertatilaus on päättynyt, jatketaan tilausta normaalilla kuukausihinnalla.
                        else
                        {
                            pmaksut += kkhinta;
                        }

                        break;
                    case 6:
                        alennus = 0.90M;
                        if (f.Kokeilujakso && (kertaosto > raportpaiva) && (raportpaiva <= ilmainentesti))
                        {

                        }
                        else if (kertaosto > raportpaiva)
                        {
                            if (kerta_maksettu)
                            {

                            }
                            else
                                pmaksut = kkhinta * alennus * f.TilauksenPituus;
                            kerta_maksettu = true;
                        }
                        else
                        {
                            pmaksut += kkhinta;
                        }

                        break;

                    case 12:
                        alennus = 0.82M;
                        if (f.Kokeilujakso && (kertaosto > raportpaiva) && (raportpaiva <= ilmainentesti))
                        {

                        }
                        else if (kertaosto > raportpaiva)
                        {
                            if (kerta_maksettu)
                            {

                            }
                            else
                                pmaksut = kkhinta * alennus * f.TilauksenPituus;
                            kerta_maksettu = true;
                        }
                        else
                        {
                            pmaksut += kkhinta;
                        }

                        break;
                    case 0:
                        break;
                }
                raportpaiva = raportpaiva.AddDays(30);

            }

            return pmaksut;
        }

    }
}
