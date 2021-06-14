using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Entities
{
    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public enum Rank
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

    public class Card
    {
        public Guid Id { get; set; }

        [ForeignKey("CardDeckForeignKey")] public virtual CardDeck CardDeck { get; set; }

        public Suit Suit { get; set; }

        public Rank Rank { get; set; }

        public int ShuffledPosition { get; set; } //0-51
    }
}