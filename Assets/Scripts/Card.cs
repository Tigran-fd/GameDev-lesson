using UnityEngine;

namespace PotionMaking
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private CardData data;
        
        public CardLocation currentLocation;
        public ElementType? elementOnDesk;
        
        [SerializeField] private SpriteRenderer cardRenderer;
        [SerializeField] private GameObject elementHighlight;
        [SerializeField] private GameObject formulaHighlight;
        
        public CardData Data => data;
        
        public void Initialize(CardData cardData)
        {
            data = cardData;
            if (cardRenderer != null && cardData.cardArt != null)
            {
                cardRenderer.sprite = cardData.cardArt;
            }
        }
        
        public void PlayAsElement(ElementType asElement)
        {
            elementOnDesk = asElement;
            currentLocation = CardLocation.DeskOfElements;
            
            if (elementHighlight != null)
                elementHighlight.SetActive(true);
            if (formulaHighlight != null)
                formulaHighlight.SetActive(false);
        }
        
        public void SetInHand()
        {
            currentLocation = CardLocation.Hand;
            if (elementHighlight != null)
                elementHighlight.SetActive(false);
            if (formulaHighlight != null)
                formulaHighlight.SetActive(false);
        }
        
        public ElementType GetElementType()
        {
            if (elementOnDesk.HasValue)
                return elementOnDesk.Value;
            return data.element;
        }
    }
    
    public enum CardLocation
    {
        Deck,
        Hand,
        DeskOfElements,
        ComposedFormula
    }
}
