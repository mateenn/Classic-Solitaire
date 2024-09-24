using GameAnalyticsSDK;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheSyedMateen.ClassicSolitaire
{
    public class SpawnerManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] spadeCards, heartCards, diamondCards, clubsCards;

        [SerializeField] private Slot stackSlot;
        [SerializeField] private Vector3 cardSpawnPosition;

        private Transform _cardsParent;

        public VisualCard[] allCards;
        private int index = 0;

        private void Awake()
        {
            CreateCardsParent();
            // Shuffle and spawn all cards
            SpawnAllCards();

            //only adding start level event for now
            AddProgressionEvent(status: GAProgressionStatus.Start, "Level");
        }


        private void OnEnable()
        {
            EventManager.OnLevelComplete += LevelCompleted;
        }

        private void OnDisable()
        {
            EventManager.OnLevelComplete -= LevelCompleted;
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
                allCards[index] = Instantiate(cardPrefab, cardSpawnPosition, Quaternion.identity, _cardsParent)
                    .GetComponent<VisualCard>();
                // Set the card to the stack slot
                /*Card card = allCards[index].SetCard(stackSlot);

                // Add the card to the stack slot
                stackSlot.AssignCard(card);

                // Increment the index after processing the card
                index += 1;*/
                
                Card card = allCards[index].SetCard(stackSlot);

                // Add the card to the stack slot
                stackSlot.AssignCard(card);

                // Increment the index after processing the card
                index += 1;
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

        private void LevelCompleted()
        {
            AddProgressionEvent(GAProgressionStatus.Complete, "Level");
        }
        private void AddProgressionEvent(GAProgressionStatus status, string detail)
        {
            GameAnalytics.NewProgressionEvent(status, detail);
        }
    }
}