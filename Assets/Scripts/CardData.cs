using UnityEngine;
using System.Collections.Generic;

namespace PotionMaking
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "Potion Making/Card Data")]
    public class CardData : ScriptableObject
    {
        [Header("Card Identity")]
        public string cardName;
        public Sprite cardArt;
        
        [Header("Element (bottom half of card)")]
        public ElementType element;
        
        [Header("Formula (top half of card)")]
        public bool hasFormula = true;
        public string formulaName;
        public FormulaType formulaType;
        public int victoryPoints;
        public List<ComponentRequirement> components = new List<ComponentRequirement>();
        
        [Header("Spell (if this is a spell card)")]
        public SpellType spellType = SpellType.None;
        
        [Header("Supreme Cards Special")]
        public bool isSupremeCard = false;
        public List<ElementType> supremeElements = new List<ElementType>();
        
        public bool CanPlayAsElement => true;
        public bool CanPlayAsFormula => hasFormula;
        public bool IsSpell => spellType != SpellType.None;
    }
}
