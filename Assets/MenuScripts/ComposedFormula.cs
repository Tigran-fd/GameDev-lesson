using System.Collections.Generic;
using UnityEngine;

namespace PotionMaking
{
    public class ComposedFormula : MonoBehaviour
    {
        public Card formulaCard;
        public List<Card> componentCards = new List<Card>();
        public List<ComposedFormula> componentFormulas = new List<ComposedFormula>();
        public Player owner;
        
        [SerializeField] private Transform componentStack;
        
        public FormulaType FormulaType => formulaCard.Data.formulaType;
        public int VictoryPoints => formulaCard.Data.victoryPoints;
        
        public void Initialize(Card formula, Player formulaOwner)
        {
            formulaCard = formula;
            owner = formulaOwner;
            formula.currentLocation = CardLocation.ComposedFormula;
        }
        
        public void AddComponentCard(Card card)
        {
            componentCards.Add(card);
            card.currentLocation = CardLocation.ComposedFormula;
            
            if (componentStack != null)
            {
                card.transform.SetParent(componentStack);
                card.transform.localPosition = Vector3.zero;
            }
        }
        
        public void AddComponentFormula(ComposedFormula formula)
        {
            componentFormulas.Add(formula);
        }
        
        public List<Card> ReleaseComponents()
        {
            List<Card> allCards = new List<Card>(componentCards);
            
            foreach (var subFormula in componentFormulas)
            {
                allCards.AddRange(subFormula.ReleaseComponents());
            }
            
            componentCards.Clear();
            componentFormulas.Clear();
            
            return allCards;
        }
        
        public bool MatchesType(FormulaType type, bool anyOfType = false)
        {
            if (anyOfType)
            {
                return FormulaType == type;
            }
            return FormulaType == type;
        }
    }
}
