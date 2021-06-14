using System.Collections.Generic;
using Storage.Entities;

namespace CardShuffler.Services
{
    public interface IShufflerService
    {
        List<Card> ShuffleCards(List<Card> cards);
    }
}