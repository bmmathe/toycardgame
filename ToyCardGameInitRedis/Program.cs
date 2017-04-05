using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using StackExchange.Redis;
using ToyCardGameLibrary;
using Newtonsoft.Json;
using ToyCardGameLibrary.ActionCards;

namespace ToyCardGameInitRedis
{
    class Program
    {
        private static ConnectionMultiplexer redis;
        private static IDatabase db;
        static void Main(string[] args)
        {
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            db = redis.GetDatabase();

            var command = string.Empty;
            while (command != "exit")
            {
                Console.WriteLine("Waiting for input.");
                command = Console.ReadLine();
                switch (command.Split(' ').First())
                {
                    case "help":
                        Console.WriteLine("Available commands: \naddcards\nremovecards\naddplayer\nlistplayers\nbuilddeck\nremoveplayer");
                        break;
                    case "addcards":
                        AddCards();
                        break;
                    case "removecards":
                        RemoveCards();
                        break;
                    case "addplayer":
                        Console.WriteLine("Enter player name:");
                        var playerName = Console.ReadLine();
                        AddPlayer(playerName);
                        break;
                    case "listplayers":
                        ListPlayer();
                        break;
                    case "removeplayer":
                        Console.WriteLine("Enter player name:");
                        RemovePlayer(Console.ReadLine());
                        break;
                    case "builddeck":
                        Console.WriteLine("Enter player name:");
                        BuildDeck(Console.ReadLine());
                        break;
                }
            }
        }

        private static void BuildDeck(string userName)
        {
            var playerKey = db.StringGet($"players:{userName}");
            if (string.IsNullOrEmpty(playerKey))
            {
                Console.WriteLine($"Player {userName} does not exist.");
                return;
            }

            var player = JsonConvert.DeserializeObject<Player>(playerKey);

            Console.WriteLine("Enter name of deck");
            var deckName = Console.ReadLine();

            var endpoints = redis.GetEndPoints();
            var server = redis.GetServer(endpoints.First());
            var keys = server.Keys(0, "cards*");
            List<Card> cards = new List<Card>();
            int index = 0;
            foreach (var key in keys)
            {
                var card = JsonConvert.DeserializeObject<Card>(db.StringGet(key));
                cards.Add(card);                
                Console.WriteLine($"{index}: {card.Name}");
                index++;
            }

            Console.WriteLine("Enter card numbers to add to deck, done to exit");
            var deck = new Deck(deckName);
            string command = string.Empty;            
            while (command != "done")
            {
                int addCardAtIndex;
                command = Console.ReadLine();
                if (int.TryParse(command, out addCardAtIndex))
                {
                   deck.Cards.Add(cards[addCardAtIndex]);
                }
            }

            player.Decks.Add(deck);
            SavePlayer(player);
        }

        private static void RemovePlayer(string userName)
        {
            db.KeyDelete($"players:{userName}");
        }

        private static void ListPlayer()
        {
            var endpoints = redis.GetEndPoints();
            var server = redis.GetServer(endpoints.First());
            var keys = server.Keys(0, "players*");
            foreach (var key in keys)
            {
                var player = JsonConvert.DeserializeObject<Player>(db.StringGet(key));
                Console.WriteLine(player.Username);
            }
        }

        private static void SavePlayer(Player player)
        {
            db.StringSet($"players:{player.Username}", JsonConvert.SerializeObject(player));
        }

        private static void AddPlayer(string playerName)
        {
            db.StringSet($"players:{playerName}", JsonConvert.SerializeObject(new Player {PlayerId = Guid.NewGuid(), Username = playerName}));
        }

        private static void RemoveCards()
        {
            var endpoints = redis.GetEndPoints();
            var server = redis.GetServer(endpoints.First());
            var keys = server.Keys(0, "cards*");
            foreach (var key in keys)
            {
                db.KeyDelete(key);
            }
            Console.WriteLine("All cards removed");
        }

        private static void AddCards()
        {            
            Console.WriteLine("Adding cards.");
            AddToyCardToRedis(Guid.NewGuid(), "Teddy Bear", 2, 3, 2);
            AddToyCardToRedis(Guid.NewGuid(), "Army Man", 1, 1, 1);
            AddToyCardToRedis(Guid.NewGuid(), "Rubber Ducky", 1, 2, 1);
            AddToyCardToRedis(Guid.NewGuid(), "Magnetic Sketch", 2, 0, 5, new [] { StaticCardProperty.Block });
            AddToyCardToRedis(Guid.NewGuid(), "Turtle Nightlight", 4, 3, 4);
            AddToyCardToRedis(Guid.NewGuid(), "Monster Truck", 3, 3, 2, new[]{ StaticCardProperty.Charge });
            AddToyCardToRedis(Guid.NewGuid(), "Toy Car", 1, 1, 1, new[] { StaticCardProperty.Charge });
            AddToyCardToRedis(Guid.NewGuid(), "Paper Airplane", 1, 2, 1, new[] { StaticCardProperty.Flying });
            AddToyCardToRedis(Guid.NewGuid(), "Spaceship", 3, 3, 2, new[] { StaticCardProperty.Flying });
            AddInstantDamageCardToRedis(Guid.NewGuid(), "Foam Dart", 1, 1, 0, 0);
            AddInstantDamageCardToRedis(Guid.NewGuid(), "Rocket", 1, 4, 1, int.MaxValue);
            AddInstantDamageCardToRedis(Guid.NewGuid(), "Shark Attack!", 3, 2, 1, 1);
            Console.WriteLine("Cards added.");
        }

        private static void AddToyCardToRedis(Guid id, string name, int cost, int attack, int health, StaticCardProperty[] properties = null)
        {
            db.StringSet($"cards:{id}", JsonConvert.SerializeObject(GetToyCard(id, name, cost, attack, health, properties)));            
        }

        private static ToyCard GetToyCard(Guid id, string name, int cost, int attack, int health, StaticCardProperty[] properties = null)
        {
            return new ToyCard()
            {
                CardId = id,
                CardType = CardType.Toy,
                Name = name,
                Cost = cost,
                Attack = attack,
                Health = health,
                Properties = properties
            };
        }

        private static void AddInstantDamageCardToRedis(Guid id, string name, int cost, int primaryTargetDamage, int splashDamage, int splashRadius)
        {
            db.StringSet($"cards:{id}", JsonConvert.SerializeObject(GetInstantDamageCard(id, name, cost, primaryTargetDamage, splashDamage, splashRadius)));
        }

        private static InstantDamage GetInstantDamageCard(Guid id, string name, int cost, int primaryTargetDamage, int splashDamage, int splashRadius)
        {
            return new InstantDamage()
            {
                CardId = id,
                CardType = CardType.Action,
                Name = name,
                Cost = cost,
                PrimaryTargetDamage = primaryTargetDamage,
                SplashDamage = splashDamage,
                SplashRadius = splashRadius
            };
        }
    }
}