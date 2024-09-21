using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public class SpawnerManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] spadeCards, heartCards, diamondCards, clubsCards;

        [SerializeField] private Vector3 cardSpawnPosition;

        private Transform _cardsParent;

        public VisualCard[] allCards;
        private int index = 0;
        private void Awake()
        {
            CreateCardsParent();
            // Shuffle and spawn all cards
            SpawnAllCards();
        }

        private void SpawnAllCards()
        {
            allCards = new VisualCard[52];
            // Shuffle and spawn cards for each suit
            Spawn(ShuffleArray(spadeCards));
            Spawn(ShuffleArray(heartCards));
            Spawn(ShuffleArray(diamondCards));
            Spawn(ShuffleArray(clubsCards));
        }

        private void Spawn(GameObject[] cards)
        {
            foreach (GameObject cardPrefab in cards)
            {
                allCards[index] = Instantiate(cardPrefab, cardSpawnPosition, Quaternion.identity, _cardsParent).GetComponent<VisualCard>();
                index += 1;
                // Assuming VisualCard is attached to the card prefab
                /*VisualCard visualCard = cardInstance.GetComponent<VisualCard>();
                if (visualCard != null)
                {
                    Card cardData = new Card(/* parameters for suit and type #1#); // Initialize with correct values
                    visualCard.InitializeCard(cardData); // Initialize the visual representation
                }*/
            }
        }


        private void CreateCardsParent()
        {
            _cardsParent = new GameObject("Cards Parent").transform;
            _cardsParent.position = Vector3.zero;
        }

        private GameObject[] ShuffleArray(GameObject[] cards)
        {
            GameObject[] shuffledCards = (GameObject[])cards.Clone();
            for (int i = 0; i < shuffledCards.Length; i++)
            {
                int randomIndex = Random.Range(i, shuffledCards.Length);
                (shuffledCards[i], shuffledCards[randomIndex]) = (shuffledCards[randomIndex], shuffledCards[i]);
            }

            return shuffledCards;
        }
    }
}