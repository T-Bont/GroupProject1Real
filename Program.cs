using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace GroupProject1
{
    public enum Suits
    {
        hearts = 1,
        spades,
        clubs,
        diamonds,
    }

    public class Card //Card class
    {
        public int value; //Value to hold the point value of each card
        public Suits suit; //suit
        public string name; //name of the card


        public Card(int v, Suits s, string n) //constructor
        {
            value = v; suit = s; name = n;
        }

        public override string ToString()
        {
            return $"{name} of {suit}";
            //return $"{name} of {suit}, value: {value}"; //in case you need to see the value as well
        }

    }

    public class Players //class to stroe all the players
    {
        public string name; //player name
        public List<Card> hand; //List of cards that will be their hand
        public bool busted; //keeps track if the player has busted or not
        public int totalScore; //stores the players total score

        public Players(List<Card> hand, string name, bool b, int r) //construcotr 
        {
            this.hand = hand; this.name = name; this.busted = b; totalScore = r;
        }
        public Players() { }
        public void ShowPlayer()
        {
            Console.WriteLine($"name: {this.name}");
            foreach (Card c in hand)                 //have to use a loop to display the cards
            {
                Console.WriteLine(c.ToString());
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            int i;

            Console.WriteLine("===========================================================");
            Console.WriteLine("Welcome to BlackJack!");
            Console.WriteLine("===========================================================\n\n\n");

            Console.Write("How many players will be playing?: ");
            int numPlayers;
            while (!int.TryParse(Console.ReadLine(), out numPlayers) || numPlayers < 1)
            {
                Console.Write("Invalid input. Please enter a valid number of players: ");
            }

            string[] playerNames = new string[numPlayers];


            Console.WriteLine("Select a gamemode: \n- Player vs Player\n- Player vs Dealer (1/2): ");
            string gameModeSelect = Console.ReadLine();
            while (gameModeSelect != "1" && gameModeSelect != "2")
            {
                Console.WriteLine("Invalid input. Please select gamemode (1/2): ");
                gameModeSelect = Console.ReadLine();
            }

            string name = Console.ReadLine();
            for (i = 0; i < numPlayers; i++)
            {
                Console.Write($"\nEnter the name of Player {i + 1}: ");
                name = Console.ReadLine();
                playerNames[i] = name;
            }


            Console.WriteLine("\n===========================================================");
            Console.WriteLine("Get ready to play Blackjack!");
            Console.WriteLine("===========================================================\n\n\n");
            CardDealing cd = new CardDealing(); //initilaizng Methods class

            //List<Players> players = cd.CreateHands(playerNames); //creating the hands, which just adds players to the Players class
            //List<Card> deck = cd.CreateDeck(); //creating the deck
            //cd.PrintDeck(deck); //printing the deck
            //Console.WriteLine(deck.Count);
            List<Players> players = cd.AddPlayers(playerNames); //Adding all the players to the players class
            List<Card> deck = new List<Card>();
            int points = 0; //initializing points to store their points
            bool Valid = false;
            Players leader = players[0]; //have to initialize the value

            if (gameModeSelect == "1")
            {
                for (i = 0; i < 2; i++) //loops through the game x amount of times
                {
                    Console.WriteLine($"Start of round {i + 1}.\n");
                    deck = cd.CreateDeck(); //filling the deck

                    foreach (Players p in players) //giving each player two cards to start with 
                    {
                        cd.DealCard(deck, p.hand);
                        cd.DealCard(deck, p.hand);
                    }

                    foreach (Players p in players) //Displaying their hands and their points
                    {
                        cd.PlayRoundPVP(p, deck);
                        Console.WriteLine($"Cards in deck: {deck.Count()}"); //this is for debugging
                    }
                    
                    cd.ShowAllPlayers(players);

                    leader = cd.FindLeader(players);
                    Console.WriteLine($"{leader.name} is in the lead with a score of {leader.totalScore}\n");

                    cd.ClearDeckandHands(players, deck); //emptying the deck and hands.
                }

                cd.ShowScores(players);
                Console.WriteLine($"{leader.name} is the winner with a score of {leader.totalScore}!");
            }//End of PVP portion
            
            else
            {
                for (i = 0; i < 2; i++) //loops through the game x amount of times
                {
                    Console.WriteLine($"Start of round {i + 1}.\n");
                    deck = cd.CreateDeck(); //filling the deck

                    foreach (Players p in players) //giving each player two cards to start with 
                    {
                        cd.DealCard(deck, p.hand);
                        cd.DealCard(deck, p.hand);
                    }

                    foreach (Players p in players) //Displaying their hands and their points
                    {
                        cd.PlayRoundPVD(p, deck);
                        //Console.WriteLine($"Cards in deck: {deck.Count()}"); //this is for debugging
                    }

                    cd.ShowAllPlayers(players);

                    cd.ClearDeckandHands(players, deck); //emptying the deck and hands.
                }
                Console.WriteLine("Scores:");
                cd.ShowScores(players);
            }
        }
    }

    public class CardDealing
    {
        public List<Card> CreateDeck()
        {
            List<Card> deck = new List<Card>();
            foreach (Suits suit in Enum.GetValues(typeof(Suits)))
            {
                for (int i = 1; i < 14; i++)
                {
                    switch (i)
                    {
                        case 1:
                            deck.Add(new Card(11, suit, "Ace"));
                            break;
                        case 11:
                            deck.Add(new Card(10, suit, "Jack"));
                            break;
                        case 12:
                            deck.Add(new Card(10, suit, "Queen"));
                            break;
                        case 13:
                            deck.Add(new Card(10, suit, "King"));
                            break;
                        default:
                            deck.Add(new Card(i, suit, $"{i}"));
                            break;
                    }
                }
            }
            return deck;
        }

        public void PrintDeck(List<Card> d)
        {
            foreach (Card c in d)
            {
                Console.WriteLine(c.ToString());
            }
        }

        public void DealCard(List<Card> deck, List<Card> hand)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, deck.Count);
            hand.Add(deck[randomNumber]);
            deck.RemoveAt(randomNumber);
        }

        public int GetScore(List<Card> hand)
        {
            int score = 0;
            int numAces = 0;
            foreach (Card c in hand)
            {
                score += c.value;
                if (c.name == "Ace")
                {
                    numAces++;
                }
            }
            if (score > 21 && numAces > 0) //handles one ace
            {
                score -= 10;
                if (score > 21 && numAces > 1) //2 aces
                {
                    score -= 10;
                    if (score > 21 && numAces > 2) //3 aces
                    {
                        score -= 10;
                        if (score > 21 && numAces > 3) //4 aces
                        {
                            score -= 10;
                        }
                    }
                }
            }
            return score;
        }

        public void ShowCards(List<Card> hand)
        {
            foreach (Card c in hand)
            {
                Console.WriteLine(c.ToString());
            }
        }

        public List<Players> AddPlayers(string[] players)
        {
            List<Players> list = new List<Players>();
            foreach (string p in players)
            {
                List<Card> hand = new List<Card>();

                list.Add(new Players(hand, p, false, 0));
            }
            return list;
        }

        public bool HitOrStand(string m, Players p, List<Card> deck)
        {
            bool isValid = false;
            ConsoleKeyInfo keyInfo;
            string input = "";
            while (!isValid)
            {
                Console.WriteLine(m);
                keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.H:
                        input = "h";
                        isValid = true;
                        break;
                    case ConsoleKey.S:
                        input = "s";
                        isValid = true;
                        break;
                    default:
                        Console.WriteLine("Enter h or s");
                        break;
                }
            }
            if (input == "h")
            {
                DealCard(deck, p.hand);
                return false;
            }
            else { return true; }


        }

        public void ShowAllPlayers(List<Players> players)
        {
            foreach (Players p in players)
            {
                p.ShowPlayer();
                Console.WriteLine($"Score: {GetScore(p.hand)}\n");
            }
        }

        public void ShowScores(List<Players> players)
        {
            foreach (Players p in players)
            {
                Console.WriteLine($"{p.name}'s score: {p.totalScore}");
            }
        }

        public Players FindLeader(List<Players> players)
        {
            Players leader = players[0];
            foreach (Players p in players)
            {
                if (p.totalScore > leader.totalScore)
                {
                    leader = p;
                }
            }
            return leader;
        }

        public void ClearDeckandHands(List<Players> players, List<Card> deck)
        {
            foreach (Players p in players)
            {
                p.hand.Clear();
            }
            deck.Clear();
        }

        public void PlayRoundPVP(Players p, List<Card> deck)
        {
            Console.WriteLine($"It is {p.name}'s turn.");
            p.busted = false;
            int points = 0; //reset points variable to 0
            bool Valid = false;
            points = GetScore(p.hand); //set points variable equal to the amount of points the player has
            while (!p.busted && !Valid) //looping through until either the player busts or stands
            {
                p.ShowPlayer();
                if (points > 21)
                {
                    Console.WriteLine("You Bust!\n");
                    p.busted = true;
                    break;
                }
                else if (points == 21)
                {
                    Console.WriteLine("Blackjack!");
                    break;
                }

                Console.WriteLine($"points: {GetScore(p.hand)}\n");
                Valid = HitOrStand("Hit or Stand? (h/s)\n", p, deck);
                points = GetScore(p.hand);
            }
            if (p.busted == false)
            {
                p.totalScore += points; //adding the points to the players total tally if they didn't bust
            }
        }

        public void PlayRoundPVD(Players p, List<Card> deck)
        {
            Console.WriteLine($"It is {p.name}'s turn.");

            List<Card> dealerHand = new List<Card>();
            DealCard(deck, dealerHand); //Adding the first two cards to the dealers hand
            DealCard(deck, dealerHand);
            Console.WriteLine("\nDealers Hand:");
            ShowCards(dealerHand);
            Console.WriteLine($"Dealers score: {GetScore(dealerHand)}");

            p.busted = false;
            int points = 0; //reset points variable to 0
            bool Valid = false;
            points = GetScore(p.hand); //set points variable equal to the amount of points the player has
            while (!p.busted && !Valid) //looping through until either the player busts or stands
            {
                Console.WriteLine("");
                p.ShowPlayer();
                if (points > 21)
                {
                    Console.WriteLine("You Bust!\n");
                    p.busted = true;
                    break;
                }
                else if (points == 21)
                {
                    Console.WriteLine("Blackjack!");
                    break;
                }

                Console.WriteLine($"points: {GetScore(p.hand)}\n");
                Valid = HitOrStand("Hit or Stand? (h/s)\n", p, deck);
                points = GetScore(p.hand);
            }
            int dealerPoints = 0;
            Valid = false;
            bool dealerBust = false;

            if (p.busted == false)
            {
                Console.WriteLine("\nDealer Hand:");
                ShowCards(dealerHand);
                Console.WriteLine($"Dealer score: {GetScore(dealerHand)}");

                while (!Valid)
                {
                    dealerPoints = GetScore(dealerHand);
                    Thread.Sleep(2000);
                    if (dealerPoints < 17)
                    {
                        Console.WriteLine("\nDealer hits.");
                        DealCard(deck, dealerHand);
                        Thread.Sleep(2000);
                        Console.WriteLine("\nDealer Hand:");
                        ShowCards(dealerHand);
                        Console.WriteLine($"Dealer score: {GetScore(dealerHand)}");

                    }
                    else if (dealerPoints >= 17 && dealerPoints <= 21)
                    {
                        Console.WriteLine("\nDealer stands.");
                        Thread.Sleep(2000);
                        Valid = true;
                        Console.WriteLine("\nDealer Hand:");
                        ShowCards(dealerHand);
                        Console.WriteLine($"Dealer score: {GetScore(dealerHand)}");

                    }
                    else if (dealerPoints > 21)
                    {
                        Console.WriteLine("\nDealer busts.");
                        Thread.Sleep(2000);
                        Valid = true;
                        dealerBust = true;
                    }
                }
            }
            dealerPoints = GetScore(dealerHand);

            if (p.busted == false && (points > dealerPoints || dealerBust == true))
            {
                p.totalScore += points; //adding the points to the players total tally if they beat the dealer
                Console.WriteLine("You beat the dealer this round!\n");
            }
            else { Console.WriteLine("Dealer wins!\n"); }
        }
    }
}

