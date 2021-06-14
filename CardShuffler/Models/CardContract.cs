using System;
using Storage.Entities;

namespace CardShuffler.Models
{
    public class CardContract
    {
        public Guid Id { get; set; }
        
        public Suit Suit { get; set; }

        public Rank Rank { get; set; }

        public int ShuffledPosition { get; set; } //0-51
    }
}