namespace TheSyedMateen.ClassicSolitaire
{
    public class Card
    {
        public Variables.Suits Suit { get; private set; }
        public Variables.CardTypes CardType { get; private set; }
        public bool IsFaceUp { get; private set; }
        public Slot Slot { get; set; }

        public Card(Variables.Suits suit, Variables.CardTypes cardType)
        {
            Suit = suit;
            CardType = cardType;
            IsFaceUp = false; // Initially face down
        }

        public void SetFaceUp(bool isFaceUp)
        {
            IsFaceUp = isFaceUp;
        }

        public void Flip()
        {
            IsFaceUp = !IsFaceUp;
        }

        public bool IsMoveable()
        {
            // Logic to determine if the card is moveable
            //Slot.slotType
            
            return IsFaceUp && (Slot != null && Slot.CanMoveCard(this));
        }

        /*public override string ToString()
        {
            return $"{Suit} {CardType}";
        }*/
    }
}