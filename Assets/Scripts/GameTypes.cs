using System;
using System.Collections.Generic;

namespace PotionMaking
{
    public enum ElementType
    {
        OgnenniySvet,
        RodnikovayaVoda,
        Belladonna,
        CvetokPaporotnika,
        GlazZmei,
        KriloLetucheyMishi,
        EnergiaMisli,
        KristalVozduxa,
        KamenKrovi,
        KorenMandragori,
        Mushrumi,
        PeroFeniksa,
        ZubDrakona,
        KvintessenciaVoli,
        VolniEfira,
        AstralnayaEnergia
    }

    public enum FormulaType
    {
        SimpleElixir,      // 2 elements
        ComplexElixir,     // 3 elements
        GreatElixir,       // 2 simple elixirs
        Powder,            // 1 simple elixir + 1 element
        Talisman,          // 1 simple elixir + 1 complex elixir
        Creature,          // 1 simple elixir + (1 powder OR 1 complex elixir)
        SupremeTalisman,   // any 2 talismans
        SupremeElixir      // any 2 great elixirs
    }

    public enum SpellType
    {
        None,   
        Eureka,
        Decomposition,
        Transformation
    }

    [Serializable]
    public class ComponentRequirement
    {
        public bool isElement;
        public ElementType elementType;
        public FormulaType formulaType;
        
        public bool isAnyOfType;
        
        public static ComponentRequirement Element(ElementType type)
        {
            return new ComponentRequirement 
            { 
                isElement = true, 
                elementType = type 
            };
        }
        
        public static ComponentRequirement Formula(FormulaType type, bool anyOfType = false)
        {
            return new ComponentRequirement 
            { 
                isElement = false, 
                formulaType = type,
                isAnyOfType = anyOfType
            };
        }
    }

    public static class ComponentHelper
    {
        public static bool CanFulfill(ComponentRequirement requirement, object component)
        {
            if (requirement.isElement)
            {
                if (component is ElementType element)
                {
                    return element == requirement.elementType;
                }
                return false;
            }
            else
            {
                if (component is FormulaType formulaType)
                {
                    if (requirement.isAnyOfType)
                    {
                        return formulaType == requirement.formulaType;
                    }
                    return formulaType == requirement.formulaType;
                }
                return false;
            }
        }
    }
}
