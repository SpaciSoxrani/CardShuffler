using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Storage;
using Storage.Entities;
using Storage.Repositories;

namespace Tests
{
    public class BaseFiller
    {
        [Test]
        public void ClearBase()
        {
            var context = new StorageContext();
            context.Database.ExecuteSqlRaw("DROP SCHEMA public CASCADE");
            context.Database.ExecuteSqlRaw("CREATE SCHEMA public");
            context.Database.Migrate();
        }

        [Test]
        public void FillBase()
        {
            var contextFactory = new ContextFactory();
            var cardRepository = new Repository<Card>(contextFactory);
            var cardDeckRepository = new Repository<CardDeck>(contextFactory);

            var decks = cardDeckRepository.Find(c => c.Name == "deck-test");
            Console.WriteLine(decks[0].Name + " "+decks[0].Id);

            var cards = cardRepository.Find(c => c.CardDeck.Name == "deck-test");
            foreach (var u in cards)
            {
                Console.WriteLine($"{u.CardDeck.Name} {u.Rank} {u.Suit} {u.ShuffledPosition}");
             }
            
                //Console.WriteLine("result");
            //var CARDS = cardRepository.Find(c => c. == "deck-1");
            //var listSorted = CARDS.OrderBy(c => c.Suit).ThenBy(c => c.Rank);

            //  foreach (var u in listSorted)
            // {
            //    Console.WriteLine($"{u.DeckName} {u.Rank} {u.Suit} {u.ShuffledPosition}");
            // }


        }
    }
}