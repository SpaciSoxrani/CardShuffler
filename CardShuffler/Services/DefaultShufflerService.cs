using System;
using System.Collections.Generic;
using System.Linq;
using Storage.Entities;

namespace CardShuffler.Services
{
    public class DefaultShufflerService : IShufflerService
    {
        public List<Card> ShuffleCards(List<Card> cards)
        {
            // Копируем список и сортируем его
            var newCards = cards.Select(c => new Card
            {
                CardDeck = new CardDeck
                {
                    Id = c.CardDeck.Id,
                    Name = c.CardDeck.Name
                },
                Id = c.Id,
                Rank = c.Rank,
                ShuffledPosition = c.ShuffledPosition,
                Suit = c.Suit
            }).OrderBy(c => c.Suit).ThenBy(c => c.Rank).ToList();

            var random = new Random();

            //перетасовка
            for (var index = 0; index < newCards.Count; index++)
            {
                var randomIndex = index + random.Next(newCards.Count - index);

                //swapping the elements
                var temp = newCards[randomIndex];
                newCards[randomIndex] = newCards[index];
                newCards[index] = temp;
            }

            //обновление индексов
            for (var index = 0; index < newCards.Count; index++)
            {
                newCards[index].ShuffledPosition = index;
            }

            return newCards;
        }
    }
}