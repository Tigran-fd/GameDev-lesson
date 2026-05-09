using System.Collections.Generic;
using UnityEngine;

namespace PotionMaking
{
    public class Deck : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private List<CardData> allCardData;
        [SerializeField] private GameObject cardPrefab;
        
        [Header("Runtime")]
        private List<Card> cards = new List<Card>();
        
        [SerializeField] private Transform deckTransform;
        
        public int CardsRemaining => cards.Count;
        public bool IsEmpty => cards.Count == 0;
        
        public void Initialize()
        {
            cards.Clear();
            
            foreach (CardData data in allCardData)
            {
                GameObject cardObj = Instantiate(cardPrefab, deckTransform);
                Card card = cardObj.GetComponent<Card>();
                card.Initialize(data);
                card.currentLocation = CardLocation.Deck;
                cards.Add(card);
                
                cardObj.SetActive(false);
            }
            
            Shuffle();
            
            Debug.Log($"Deck initialized with {cards.Count} cards");
        }
        
        public void Shuffle()
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
            
            Debug.Log("Deck shuffled");
        }
        
        public Card DrawCard()
        {
            if (IsEmpty)
            {
                Debug.LogWarning("Tried to draw from empty deck!");
                return null;
            }
            
            Card drawnCard = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            
            drawnCard.gameObject.SetActive(true);
            
            return drawnCard;
        }
        
        public List<Card> DrawCards(int count)
        {
            List<Card> drawnCards = new List<Card>();
            
            for (int i = 0; i < count && !IsEmpty; i++)
            {
                Card card = DrawCard();
                if (card != null)
                {
                    drawnCards.Add(card);
                }
            }
            
            return drawnCards;
        }
        
        public Card PeekTop()
        {
            if (IsEmpty)
                return null;
            return cards[cards.Count - 1];
        }
    }
}
