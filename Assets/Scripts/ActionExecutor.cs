using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PotionMaking
{
    public class ActionExecutor : MonoBehaviour
    {
        private DeskOfElements desk;
        private ScoreManager scoreManager;
        private TurnManager turnManager;
        
        [SerializeField] private GameObject composedFormulaPrefab;
        
        public void Initialize(DeskOfElements deskRef, ScoreManager scoreMgr, TurnManager turnMgr)
        {
            desk = deskRef;
            scoreManager = scoreMgr;
            turnManager = turnMgr;
        }
        
        public void PlayCardAsElement(Player player, Card card, ElementType? asElement = null)
        {
            if (!player.RemoveCardFromHand(card))
            {
                Debug.LogError("Card not in player's hand!");
                return;
            }
            
            ElementType elementType = asElement ?? card.Data.element;
            
            bool isNewElement = desk.AddElement(card, elementType);
            if (isNewElement)
            {
                scoreManager.AwardPoints(player, 1, "New element on desk");
            }
            
            if (card.Data.isSupremeCard)
            {
                int newElementsCount = 0;
                foreach (ElementType supremeElem in card.Data.supremeElements)
                {
                    if (!desk.HasElement(supremeElem))
                        newElementsCount++;
                }
                
                if (newElementsCount > 0)
                {
                    scoreManager.AwardPoints(player, newElementsCount, "Supreme card new elements");
                }
            }
            
            turnManager.PlayCard();
            Debug.Log($"{player.playerName} played {card.Data.cardName} as element {elementType}");
        }
        
        public void ComposeFormula(Player player, Card formulaCard, 
            List<Card> elementComponents, List<ComposedFormula> formulaComponents)
        {
            if (!player.RemoveCardFromHand(formulaCard))
            {
                Debug.LogError("Formula card not in hand!");
                return;
            }
            
            if (!ValidateComponents(formulaCard.Data, elementComponents, formulaComponents))
            {
                Debug.LogError("Invalid components for formula!");
                player.AddCardToHand(formulaCard); // Return to hand
                return;
            }
            
            GameObject formulaObj = Instantiate(composedFormulaPrefab);
            ComposedFormula composed = formulaObj.GetComponent<ComposedFormula>();
            composed.Initialize(formulaCard, player);
            
            foreach (Card elementCard in elementComponents)
            {
                composed.AddComponentCard(elementCard);
            }
            
            foreach (ComposedFormula usedFormula in formulaComponents)
            {
                if (usedFormula.owner != player)
                {
                    int halfPoints = formulaCard.Data.victoryPoints / 2;
                    scoreManager.AwardPoints(usedFormula.owner, halfPoints, 
                        $"Formula used by {player.playerName}");
                }
                
                List<Card> releasedCards = usedFormula.ReleaseComponents();
                desk.ReturnElements(releasedCards);
                
                usedFormula.owner.RemoveComposedFormula(usedFormula);
                
                composed.AddComponentFormula(usedFormula);
            }
            
            player.AddComposedFormula(composed);
            
            scoreManager.AwardPoints(player, formulaCard.Data.victoryPoints, 
                $"Composed {formulaCard.Data.formulaName}");
            
            turnManager.PlayCard();
            Debug.Log($"{player.playerName} composed {formulaCard.Data.formulaName} for {formulaCard.Data.victoryPoints} points!");
        }
        
        public void ExecuteSpell(Player player, Card spellCard)
        {
            if (!player.RemoveCardFromHand(spellCard))
            {
                Debug.LogError("Spell card not in hand!");
                return;
            }
            
            SpellType spellType = spellCard.Data.spellType;
            
            switch (spellType)
            {
                case SpellType.Eureka:
                    break;
                    
                case SpellType.Decomposition:
                    break;
                    
                case SpellType.Transformation:
                    break;
            }
            
            desk.ReturnElement(spellCard, spellCard.Data.element, isSpellCard: true);
            
            turnManager.SpellPlayed(spellType);
            
            Debug.Log($"{player.playerName} cast {spellType} spell");
        }
        
        public void ExecuteEureka(Player player, Card spellCard, ElementType fromElement)
        {
            if (!desk.HasElement(fromElement))
            {
                Debug.LogError($"No {fromElement} on desk!");
                return;
            }
            
            Card takenCard = desk.TakeElement(fromElement);
            
            if (takenCard == null || !takenCard.Data.CanPlayAsFormula)
            {
                Debug.LogError("Cannot take this card - not a formula!");
                if (takenCard != null)
                    desk.ReturnElement(takenCard, fromElement);
                return;
            }
            
            player.AddCardToHand(takenCard);
            
            desk.ReturnElement(spellCard, spellCard.Data.element, isSpellCard: true);
            
            turnManager.SpellPlayed(SpellType.Eureka);
            
            Debug.Log($"{player.playerName} used Eureka to take {takenCard.Data.cardName}");
        }
        
        public void ExecuteDecomposition(Player player, Card spellCard, 
            ComposedFormula targetFormula, Card chosenComponent)
        {
            if (!player.composedFormulas.Contains(targetFormula))
            {
                Debug.LogError("That formula doesn't belong to this player!");
                return;
            }
            
            if (!targetFormula.componentCards.Contains(chosenComponent) &&
                targetFormula.formulaCard != chosenComponent)
            {
                Debug.LogError("Chosen card is not part of this formula!");
                return;
            }
            
            if (chosenComponent.Data.IsSpell)
            {
                Debug.LogError("Cannot choose a spell card!");
                return;
            }
            
            List<Card> allComponents = targetFormula.ReleaseComponents();
            allComponents.Add(targetFormula.formulaCard); // Include the formula card itself
            
            foreach (Card card in allComponents)
            {
                if (card != chosenComponent)
                {
                    desk.ReturnElement(card, card.GetElementType());
                }
            }
            
            GameObject newFormulaObj = Instantiate(composedFormulaPrefab);
            ComposedFormula newFormula = newFormulaObj.GetComponent<ComposedFormula>();
            newFormula.Initialize(chosenComponent, player);
            player.AddComposedFormula(newFormula);
            
            player.RemoveComposedFormula(targetFormula);
            Destroy(targetFormula.gameObject);
            
            desk.ReturnElement(spellCard, spellCard.Data.element, isSpellCard: true);
            
            turnManager.SpellPlayed(SpellType.Decomposition);
            
            Debug.Log($"{player.playerName} decomposed formula, kept {chosenComponent.Data.cardName}");
        }
        
        public void ExecuteTransformation(Player player, Card spellCard,
            ComposedFormula targetFormula, ElementType fromElement)
        {
            if (!player.composedFormulas.Contains(targetFormula))
            {
                Debug.LogError("That formula doesn't belong to this player!");
                return;
            }
            
            if (!desk.HasElement(fromElement))
            {
                Debug.LogError($"No {fromElement} on desk!");
                return;
            }
            
            Card newFormulaCard = desk.TakeElement(fromElement);
            
            if (newFormulaCard == null || !newFormulaCard.Data.CanPlayAsFormula)
            {
                Debug.LogError("Cannot take this card - not a formula!");
                if (newFormulaCard != null)
                    desk.ReturnElement(newFormulaCard, fromElement);
                return;
            }
            
            List<Card> releasedCards = targetFormula.ReleaseComponents();
            releasedCards.Add(targetFormula.formulaCard);
            desk.ReturnElements(releasedCards);
            
            GameObject newFormulaObj = Instantiate(composedFormulaPrefab);
            ComposedFormula newFormula = newFormulaObj.GetComponent<ComposedFormula>();
            newFormula.Initialize(newFormulaCard, player);
            player.AddComposedFormula(newFormula);
            
            player.RemoveComposedFormula(targetFormula);
            Destroy(targetFormula.gameObject);
            
            desk.ReturnElement(spellCard, spellCard.Data.element, isSpellCard: true);
            
            turnManager.SpellPlayed(SpellType.Transformation);
            
            Debug.Log($"{player.playerName} transformed formula into {newFormulaCard.Data.cardName}");
        }
        
        private bool ValidateComponents(CardData formulaData, 
            List<Card> elementCards, List<ComposedFormula> formulas)
        {
            List<ComponentRequirement> requirements = new List<ComponentRequirement>(formulaData.components);
            
            foreach (Card card in elementCards)
            {
                ElementType element = card.GetElementType();
                ComponentRequirement matched = requirements.Find(r => 
                    r.isElement && r.elementType == element);
                
                if (matched != null)
                    requirements.Remove(matched);
                else
                    return false;
            }
            
            foreach (ComposedFormula formula in formulas)
            {
                ComponentRequirement matched = requirements.Find(r => 
                    !r.isElement && formula.MatchesType(r.formulaType, r.isAnyOfType));
                
                if (matched != null)
                    requirements.Remove(matched);
                else
                    return false;
            }
            
            return requirements.Count == 0;
        }
    }
}
