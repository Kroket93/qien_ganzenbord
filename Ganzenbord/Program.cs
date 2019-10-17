using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//to do:
//officiele regels
//gebruikers spelers laten aanmaken


namespace Ganzenbord
{
    class Program
    {
        static void Main(string[] args)
        {
            //random object instancen, zodat we de functie next kunnen gebruiken (die geeft een random integer)
            Random random = new Random();


            //spelers instancen
            List<Player> playerList = new List<Player>(); //we maken een lijst, en daarin komen Player objecten (de class wordt defined in player.cs)
            Player player1 = new Player("hans"); //maak hans aan
            Player player2 = new Player("annie"); //maak annie aan
            Player player3 = new Player("peter"); // maak peter aan
            Player player4 = new Player("sara"); // maak sara aan



            // voeg de spelers toe aan de spelers lijst, zodat we er later overheen kunnen loopen om voor elke speler dingen te doen
            playerList.Add(player1);
            playerList.Add(player2);
            playerList.Add(player3);
            playerList.Add(player4);

            bool gameDone = false; // deze variabele houdt bij of het spel is afgelopen of niet

            int round = 1; // deze variabele houdt bij welke ronde het is



            while (!gameDone) //zolang gameDone niet true is doen we dingen:
            {
                Player playerToDelete = null;
                Console.WriteLine("Ronde "+Convert.ToString(round)); // we printen naar de console welke ronde het is
                foreach (Player player in playerList) // voor elke speler in spelerlijst doen we dingen:
                {
                    writeGameState(player.name, player.location); //we roepen de functie writeGameState aan, en proppen er argumenten in. de functie is verder naar onder defined, comments bij functie voor uitleg
                    int g = rollDice(random); //we roepen de rolldice functie aan, die geeft een int terug (again zie comments bij functie voor uitleg
                    Console.WriteLine(Convert.ToString(player.name)+" heeft +" + Convert.ToString(g) + " gegooid."); //we laten weten wat er gegooid is
                    player.location = movePlayer(player.name, player.location, g); //we roepen de movePlayer functie aan, wederom zie comments bij de functie definitie
                    checkSpecial(ref gameDone, player, g, ref playerToDelete); // we roepen checkspecial aan, zie comments bij functie definitie, geeft speler terug die eventueel uit het spel moet
                    Console.WriteLine(" "); // voor extra leesbaarheid printen we een lege line
                    Console.ReadKey();
                    


                } //einde van foreach statement, elke speler is nu dus een keer aan de beurt geweest en we zijn klaar om naar de volgende ronde te gaan
                if (playerToDelete != null) // checken of er een speler verwijderd moet worden (playerToDelete zou dan een player als waarde hebben gekregen door checkSpecial()
                {
                    playerList.Remove(playerToDelete);
                    playerToDelete = null; // reset playerToDelete naar null, zodat ie bij de volgende iteratie niet nogmaals de player gaat proberen te deleten uit playerList
                }
                
                round += 1; //we tellen 1 bij round op
                //je zou dus een iteratie van de while loop kunnen zien als een ronde, waarbij iedereen aan de beurt komt.




            };



            

        }


        //checkSpecial kijkt of de speler op een speciale plek staat, en zo ja; doe dingen.
        //de functie heeft een aantal argumenten nodig
        // ref bool gameDone: we moeten op een gegeven moment zeggen dat het spel klaar is als er iemand op 63 staat. daarvoor moeten we ref gebruiken (zowel bij parameter als argument).
        // ref zorgt ervoor dat er geen kopie wordt gemaakt van in dit geval gameDone, maar dat de functie de variabele gameDone manipuleert
        // Player player: dit is de speler waarop de functie dingen moet doen
        // int g: dit was onze gooi van de dobbelsteen
        // ref zie je ook terug bij de parameter ref List<Player> playerlist, omdat we een speler uit het spel (dus ook uit de playerList) willen halen als ie 23 heeft gegooid
    
        private static void checkSpecial(ref bool gameDone, Player player, int g, ref Player playerToDelete)
        {
            switch(player.location) // een switch statement is eigenlijk een if statement op steroiden, we voeren een waarde in (in dit geval player.location), en dan kunnen we met case: x zeggen dat er iets
                // moet gebeuren op het moment dat die player.location hetzelfde is als x.
            {
                case 23:
                    Console.WriteLine("BOEF! "+Convert.ToString(player.name)+" is de gevangenis in gegooid. Game Over.");
                    playerToDelete = player; // sla de te verwijderen speler op in de playerToDelete variabele, zodat we die na de forloop kunnen verwijderen uit playerList
                    //aangezien het spel elke ronde over de playerList itereert, is dit genoeg om de speler niet meer mee te laten doen.
                    
                    break; //break zorgt ervoor dat we uit onze switch statement gaan. In dit geval zijn we klaar omdat onze speler niet meer meedoet, en is de functie checkSpecial dus klaar
                case 63:
                    Console.WriteLine("Gewonnen! Het spel eindigt");
                    gameDone = true; // we zetten onze gameDone naar true, zodat de while loop niet meer verder gaat
                    Console.ReadKey();
                    break; // we breaken uit de switch statement, de functie is klaar
                case int n when (n > 63): // int n when (n > 63): hier staat eigenlijk: als player.location groter is dan 63. n kun je vervanger door player.location (de waarde die we invoerde in de switch statement)
                    int tooMuch = player.location - 63; // hier slaan we op hoeveel plaatsen de speler te veel is gegaan
                    Console.WriteLine("Oeps! te ver gegaan, ga "+Convert.ToString(tooMuch)+" plaatsen terug..");
                    player.location = 63 - tooMuch; // we zetten player.location naar tooMuch plaatsen terug
                    break;
                case 25:
                    Console.WriteLine("25! Terug naar start..");
                    player.location = 0; //deze is makkelijker, als je op 25 terecht komt dan ga je naar start (dus naar 0)
                    break;
                case 45:
                    Console.WriteLine("45! Terug naar start..");
                    player.location = 0; //zelfde als 25
                    break;
                case int n when (n % 10 == 0): // n % 10 == 0 <- dit betekent: als je de waarde die we invoerde in de switch statement (dus player.location) (en dan in dit specifieke geval n) door 10 deelt,
                    // en hetgeen wat dan overblijft 0 is: doe dingen
                    //voor meer uitleg google -> modulo (nee echt doe het is mega handig)
                    Console.WriteLine("10, 20, 30, 40, 50, of 60! Loop nogmaals " + Convert.ToString(g) + " stappen, naar "+Convert.ToString(player.location+g)+"!");
                    player.location += g; //we mogen nog een keer lopen, dus doen we gewoon += g
                    break;
            }

        }


        private static void writeGameState(string playerName, int playerLocation) // deze functie print informatie over het spel naar de console
        {
            Console.WriteLine(playerName + " current location: " + playerLocation); // we printen dus de naam van de speler en de locatie
        }

        private static int movePlayer(string playerName, int playerLocation, int g) //deze functie beweegt de speler
        {
            playerLocation += g; //speler locatie + g
            Console.WriteLine(Convert.ToString(playerName) + " staat nu op plek " + Convert.ToString(playerLocation)); // we laten weten op welke plek de speler nu staat
            return playerLocation; //in dit geval returnen we de nieuwe speler locatie. notice dat er voor de functienaam int staat ipv void (void betekent niks returnen)
        }
        private static int rollDice(Random random) //deze functie geeft een random nummer tussen 1 en 6
        {
            Console.WriteLine("Press any key to roll the dice!"); //we geven de speler info over wat er gaat gebeuren
            Console.ReadKey(); //dit zorgt ervoor dat de speler op een key moet drukken om verder te gaan, zonder dit zou het spel te snel doorgaan
            int g = random.Next(1, 6); // we gebruiken onze eerder initialized random object om daarvan de Next() functie te gebruiken,
            return g; // we geven de random waarde terug

        }
    }
}
