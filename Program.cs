using System.Net.NetworkInformation;
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

    public class Card
    {
        public int value;
        public Suits suit;
        public string name;

        public Card(int v, Suits s, string n)
        {
            value = v; suit = s; name = n;
        }

        public override string ToString()
        {
            //return $"{name} of {suit}";
            return $"{name} of {suit}, value: {value}"; //in case you need to see the value as well
        }

    }

    public class Players
    {
        public string name;
        public List<Card> hand;

        public Players(List<Card> hand, string name)
        {
            this.hand = hand; this.name = name;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {

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
                 Console.Write("Invalid input. Please select gamemode (1/2): ");
                 gameModeSelect = Console.ReadLine();
             }

             string name = Console.ReadLine();
             for (int i = 0; i < numPlayers; i++)
             {
                 Console.Write($"\nEnter the name of Player {i + 1}: ");
                 name = Console.ReadLine();
                 playerNames[i] = name;
             }

             Console.WriteLine("\n===========================================================");
             Console.WriteLine("Get ready to play Blackjack!");
             Console.WriteLine("===========================================================\n\n\n");

            
            CardDealing cd = new CardDealing();
            List<Card> deck = cd.CreateDeck();
            //cd.PrintDeck(deck);
            List<Card> playerHand = new List<Card>();
            List<Card> dealerHand = new List<Card>();
            //Console.WriteLine(deck.Count);
            int cardsRemaining = 52;
            Console.WriteLine(cardsRemaining);
            cardsRemaining = cd.DealCard(deck, playerHand, cardsRemaining);
            Console.WriteLine(cardsRemaining);
            cardsRemaining = cd.DealCard(deck, playerHand, cardsRemaining);
            Console.WriteLine(cardsRemaining);


            Console.WriteLine("Your hand:");
            cd.ShowCards(playerHand);
            
            int points = cd.GetScore(playerHand);
            Console.WriteLine($"You have {points} points");
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
                            deck.Add(new Card(i, suit, "Ace"));
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

        public int DealCard(List<Card> deck, List<Card> hand, int cardsRemaining)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, cardsRemaining - 1);//subtracting one because we want to use the index
            hand.Add(deck[randomNumber]);
            deck.RemoveAt(randomNumber);
            cardsRemaining -= 1;
            return cardsRemaining;
        }

        public int GetScore(List<Card> hand)
        {
            int score = 0;
            foreach(Card c in hand)
            {
                score += c.value;
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

        public List<Players> CreateHands(string[] players)
        {
            List<Players> list = new List<Players>();
            foreach(string p in players)
            {
                list.Add()
            }
        }
    }
}

