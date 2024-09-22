using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace TheSyedMateen.ClassicSolitaire
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Slot[] foundationSlots; // Array of foundation slots
        [SerializeField] private Slot[] tableauSlots; // Array of tableau slots

        public static GameManager Instance;
        [SerializeField] private Slot stackSlot, wasteSlot;
        //[SerializeField] private VisualCard[] allCards; // All cards you have in the game
        [SerializeField] private SpawnerManager spawnerManager;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Invoke(nameof(InitializeGame),.3f);
        }

        private void InitializeGame()
        {
            ShuffleAndDealCards();
            //MoveCardsToFoundations(); // Call if you want to move certain cards to foundations
        }

        private void ShuffleAndDealCards()
        {
            // Shuffle the cards (you can use the shuffle function in SpawnerManager)
            //spawnerManager.ShuffleAllCards();

            int tableauCount = 7; // Number of tableau slots

            // Keep track of which card we are assigning
            int cardIndex = 0;

            // Loop through each tableau slot and assign increasing number of cards
            for (int i = 0; i < tableauCount; i++)
            {
                int cardsInThisPile = i + 1; // First tableau gets 1 card, second gets 2, and so on

                for (int j = 0; j < cardsInThisPile; j++)
                {
                    if (cardIndex >= spawnerManager.allCards.Length)
                    {
                        break; // Ensure we don't go out of bounds
                    }

                    VisualCard card = spawnerManager.allCards[cardIndex]; // Get the next card
                    cardIndex++;

                    // Place the card in the tableau slot
                    tableauSlots[i].AssignCard(card.GetCard());

                    // Set the card's position in a stacked manner
                    card.transform.position = tableauSlots[i].transform.position + new Vector3(0, -j * 0.65f, 0); // Slight offset for stacking

                    // Flip the top card face up, others face down
                    if (j == cardsInThisPile - 1)
                    {
                        card.FlipCard(true); // Top card should be face up
                    }
                    else
                    {
                        card.FlipCard(false); // All other cards should be face down
                    }
                }
            }
        }

        private void MoveCardsToFoundations()
        {
            foreach (VisualCard card in spawnerManager.allCards)
            {
                if (ShouldGoToFoundation(card))
                {
                    PlaceCardInFoundation(card);
                }
            }
        }

        private void ShuffleArray(VisualCard[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                int randomIndex = Random.Range(i, cards.Length);
                // Swap cards
                (cards[i], cards[randomIndex]) = (cards[randomIndex], cards[i]);
            }
        }

        private bool ShouldGoToFoundation(VisualCard card)
        {
            // Logic to determine if a card goes to the foundation
            return card.cardType == Variables.CardTypes.Ace; // Simplified example
        }

        private void PlaceCardInFoundation(VisualCard card)
        {
            Slot targetFoundation = GetFoundationSlot(card.suit);
            if (targetFoundation != null)
            {
                targetFoundation.AssignCard(card.GetCard());
                card.transform.position = targetFoundation.transform.position;
            }
        }

        private Slot GetFoundationSlot(Variables.Suits suit)
        {
            switch (suit)
            {
                case Variables.Suits.Spade:
                    return foundationSlots[0];
                case Variables.Suits.Heart:
                    return foundationSlots[1];
                case Variables.Suits.Diamond:
                    return foundationSlots[2];
                case Variables.Suits.Club:
                    return foundationSlots[3];
                default:
                    return null;
            }
        }

        public Slot WasteSlot => wasteSlot;

        public Slot StackSlot => stackSlot;
    }
}
