using System;
using System.Collections.Generic;
using Storage.Entities;

namespace CardShuffler.Models
{
    public class CardDeckWithCards
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public List<CardContract> Cards { get; set; }
    }
}