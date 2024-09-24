using System.Collections.Generic;
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

            // Combine all the cards into one array
            List<GameObject> combinedCards = new List<GameObject>();
            combinedCards.AddRange(spadeCards);
            combinedCards.AddRange(heartCards);
            combinedCards.AddRange(diamondCards);
            combinedCards.AddRange(clubsCards);

            // Shuffle all the combined cards
            GameObject[] shuffledCombinedCards = ShuffleArray(combinedCards.ToArray());

            // Spawn the shuffled combined cards
            Spawn(shuffledCombinedCards);
        }

        private void Spawn(GameObject[] cards)
        {
            foreach (GameObject cardPrefab in cards)
            {
                allCards[index] = Instantiate(cardPrefab, cardSpawnPosition, Quaternion.identity, _cardsParent)
                    .GetComponent<VisualCard>();

                Card card = allCards[index].SetCard(stackSlot);

                // Add the card to the stack slot
                stackSlot.AssignCard(card);

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