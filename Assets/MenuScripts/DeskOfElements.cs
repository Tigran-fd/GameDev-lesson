using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PotionMaking
{
    public class DeskOfElements : MonoBehaviour
    {
        private Dictionary<ElementType, List<Card>> elementStacks = new Dictionary<ElementType, List<Card>>();
        
        [SerializeField] private Transform deskTransform;
        
        private const float ELEMENT_SPACING = 1.8f;
        
        public void Initialize()
        {
            foreach (ElementType elementType in System.Enum.GetValues(typeof(ElementType)))
            {
                elementStacks[elementType] = new List<Card>();
            }
        }
        
        public bool AddElement(Card card, ElementType asElement)
        {
            if (!elementStacks.ContainsKey(asElement))
            {
                elementStacks[asElement] = new List<Card>();
            }
            
            bool isNewElement = elementStacks[asElement].Count == 0;
            
            card.PlayAsElement(asElement);
            elementStacks[asElement].Add(card);
            
            PositionCardOnDesk(card, asElement);
            
            return isNewElement;
        }
        
        public bool HasElement(ElementType elementType)
        {
            return elementStacks.ContainsKey(elementType) && 
                   elementStacks[elementType].Count > 0;
        }
        
        public Card TakeElement(ElementType elementType)
        {
            if (!HasElement(elementType))
            {
                Debug.LogWarning($"Tried to take {elementType} but none available!");
                return null;
            }
            
            List<Card> stack = elementStacks[elementType];
            Card topCard = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);
            
            return topCard;
        }
        
        public Card PeekElement(ElementType elementType)
        {
            if (!HasElement(elementType))
                return null;
                
            List<Card> stack = elementStacks[elementType];
            return stack[stack.Count - 1];
        }
        
        public List<Card> GetAllCardsOfElement(ElementType elementType)
        {
            if (elementStacks.ContainsKey(elementType))
                return new List<Card>(elementStacks[elementType]);
            return new List<Card>();
        }
        
        public void ReturnElement(Card card, ElementType asElement, bool isSpellCard = false)
        {
            if (!elementStacks.ContainsKey(asElement))
            {
                elementStacks[asElement] = new List<Card>();
            }
            
            card.PlayAsElement(asElement);
            
            if (isSpellCard)
            {
                elementStacks[asElement].Insert(0, card);
            }
            else
            {
                elementStacks[asElement].Add(card);
            }
            
            PositionCardOnDesk(card, asElement);
        }
        
        public void ReturnElements(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                ElementType elementType = card.GetElementType();
                ReturnElement(card, elementType, card.Data.IsSpell);
            }
        }
        
        private void PositionCardOnDesk(Card card, ElementType elementType)
        {
            if (deskTransform == null) return;
            
            card.transform.SetParent(deskTransform);
            
            int elementIndex = (int)elementType;
            int stackHeight = elementStacks[elementType].Count;
            
            int columns = 4;
            int row = elementIndex / columns;
            int col = elementIndex % columns;
            
            float x = col * ELEMENT_SPACING;
            float y = -row * ELEMENT_SPACING;
            float z = -stackHeight * 0.01f;
            
            card.transform.localPosition = new Vector3(x, y, z);
        }
        
        public List<ElementType> GetAvailableElements()
        {
            return elementStacks
                .Where(kvp => kvp.Value.Count > 0)
                .Select(kvp => kvp.Key)
                .ToList();
        }
    }
}
