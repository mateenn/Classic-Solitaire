using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class CardManager : MonoBehaviour
    {
        private Card[] allCards;

        public void ShuffleCards(Card[] cards)
        {
            allCards = cards;
            // Fisher-Yates shuffle algorithm
            for (int i = 0; i < allCards.Length; i++)
            {
                int randomIndex = Random.Range(i, allCards.Length);
                (allCards[i], allCards[randomIndex]) = (allCards[randomIndex], allCards[i]);
            }
        }

        public Card GetNextCard()
        {
            // Implement logic to get the next card from the shuffled array
            // For now, let's return null for safety
            return null;
        }

        public Card[] GetAllCards()
        {
            return allCards;
        }
    }
}