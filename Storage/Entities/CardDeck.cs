using System;

namespace Storage.Entities
{
    public class CardDeck
    {
        public Guid Id { get; set; }
        
        public bool IsShuffled { get; set; }
        
        public string Name { get; set; }
    }
}