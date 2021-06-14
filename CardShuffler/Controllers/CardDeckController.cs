using System;
using System.Collections.Generic;
using System.Linq;
using CardShuffler.Models;
using CardShuffler.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Storage.Entities;
using Storage.Repositories;

namespace CardShuffler.Controllers
{
    [ApiController]
    [Route("api/card-decks")]
    public class AppController : ControllerBase
    {
        private readonly IRepository<Card> cardRepository;
        private readonly IRepository<CardDeck> cardDeckRepository;
        private readonly IShufflerService shufflerService;

        public AppController(IRepository<Card> cardRepository, IRepository<CardDeck> cardDeckRepository,
            IShufflerService shufflerService)
        {
            this.cardRepository = cardRepository;
            this.cardDeckRepository = cardDeckRepository;
            this.shufflerService = shufflerService;
        }

        [HttpPost("{deckName}")]
        public ActionResult<Guid> CreateCardDeck(string deckName)
        {
            var existingCardDecks = cardDeckRepository.Find(c => c.Name == deckName);
            if (existingCardDecks != null && existingCardDecks.Length > 0)
                return StatusCode(StatusCodes.Status406NotAcceptable);
            var cards = new List<Card>();
            var position = 0;
            var cardDeck = new CardDeck {Name = deckName, IsShuffled = false};
            cardDeckRepository.Create(cardDeck);
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card
                    {
                        CardDeck = cardDeck,
                        Suit = suit,
                        Rank = rank,
                        ShuffledPosition = position
                    });
                    position++;
                }
            }

            cardRepository.CreateRange(cards);
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpDelete("{deckName}")]
        public ActionResult<Guid> DeleteCardDeck(string deckName)
        {
            var cardDecks = cardDeckRepository.Find(c => c.Name == deckName);
            if (cardDecks == null || cardDecks.Length == 0)
                return StatusCode(StatusCodes.Status406NotAcceptable);
            var cards = cardRepository.Find(c => c.CardDeck.Name == deckName);
            cardRepository.DeleteRange(cards);
            cardDeckRepository.Delete(cardDecks.First());
            return StatusCode(StatusCodes.Status200OK);
        }

        // request as application/json
        [HttpGet("names")]
        public ActionResult<string[]> GetCardDeckNames()
        {
            var cardDecks = cardDeckRepository.GetAll();
            if (cardDecks == null)
                return StatusCode(StatusCodes.Status406NotAcceptable);
            return cardDecks.Select(cardDeck => cardDeck.Name).ToArray();
        }


        [HttpPut("{deckName}/shuffle")]
        public ActionResult<string> ShuffleCardDeck(string deckName)
        {
            var cardDecks = cardDeckRepository.Find(c => c.Name == deckName);
            if (cardDecks == null || cardDecks.Length == 0)
                return StatusCode(StatusCodes.Status406NotAcceptable);
            var cardDeck = cardDecks.First();
            var cards = cardRepository.Find(c => c.CardDeck.Name == deckName).ToList();
            var shuffledCards = shufflerService.ShuffleCards(cards);
            
            shuffledCards.ForEach(c => cardRepository.Update(c));
            cardDeck.IsShuffled = true;
            cardDeckRepository.Update(cardDeck);
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpGet("{deckName}")]
        public ActionResult<CardDeckWithCards> GetCardDeck(string deckName, [FromQuery] string status)
        {
            var cardDecks = cardDeckRepository.Find(c => c.Name == deckName);
            if (cardDecks == null || cardDecks.Length == 0)
                return StatusCode(StatusCodes.Status406NotAcceptable);
            var cardDeck = cardDecks.First();
            var cards = cardRepository.Find(c => c.CardDeck.Name == deckName).ToList();
            switch (status)
            {
                case "sorted":
                {
                    var listSorted = cards
                        .OrderBy(c => c.Suit)
                        .ThenBy(c => c.Rank)
                        .Select(c => new CardContract
                        {
                            Id = c.Id,
                            Rank = c.Rank,
                            ShuffledPosition = c.ShuffledPosition,
                            Suit = c.Suit
                        })
                        .ToList();
                    return new CardDeckWithCards
                    {
                        Id = cardDeck.Id,
                        Name = cardDeck.Name,
                        Cards = listSorted
                    };
                }
                case "shuffled":
                {
                    if (!cardDeck.IsShuffled)
                        return StatusCode(StatusCodes.Status417ExpectationFailed);
                    var listShuffled = cards.OrderBy(c => c.ShuffledPosition)
                        .Select(c => new CardContract
                        {
                            Id = c.Id,
                            Rank = c.Rank,
                            ShuffledPosition = c.ShuffledPosition,
                            Suit = c.Suit
                        })
                        .ToList();
                    return new CardDeckWithCards
                    {
                        Id = cardDeck.Id,
                        Name = cardDeck.Name,
                        Cards = listShuffled
                    };
                }
                default:
                    return StatusCode(StatusCodes.Status406NotAcceptable);
            }
        }
    }
}