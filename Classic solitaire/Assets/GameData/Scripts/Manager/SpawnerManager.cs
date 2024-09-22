using GameAnalyticsSDK;
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

            //only adding start level event for now
            AddProgressionEvent(status: GAProgressionStatus.Start, "Level");
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

        private void AddProgressionEvent(GAProgressionStatus status, string detail)
        {
            GameAnalytics.NewProgressionEvent(status, detail);
        }
    }
}