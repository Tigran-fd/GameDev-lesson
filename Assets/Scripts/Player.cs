using UnityEngine;
using System.Collections.Generic;
using Mirror; 

namespace PotionMaking
{
    public class Player : NetworkBehaviour
    {
        [Header("Player Identity")]
        public string playerName;
        public Color playerColor;
        public int playerIndex;
        
        [Header("Game State")]
        [SyncVar] public int score = 0;
        
        public List<Card> hand = new List<Card>();
        public List<ComposedFormula> composedFormulas = new List<ComposedFormula>();
        
        [Header("Visual References")]
        [SerializeField] private Transform handTransform;
        [SerializeField] private Transform formulaAreaTransform;
        
        private CharacterController characterController;
        private MonoBehaviour firstPersonController;
        
        public bool IsHandFull => hand.Count >= 5;
        public int HandCount => hand.Count;
        
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.RegisterPlayer(this);
            }
            
            DisableMovement();
            
            Debug.Log("Local player spawned and registered!");
        }
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            if (!isLocalPlayer)
            {
                GameManager gm = FindObjectOfType<GameManager>();
                if (gm != null)
                {
                    gm.RegisterPlayer(this);
                }
            }
        }
        
        private void OnDestroy()
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.UnregisterPlayer(this);
            }
        }
        
        private void DisableMovement()
        {
            characterController = GetComponent<CharacterController>();
            
            firstPersonController = GetComponent<MonoBehaviour>();
            
            if (characterController != null)
            {
                characterController.enabled = false;
            }
        }
        
        public void EnableMovement()
        {
            if (characterController != null)
            {
                characterController.enabled = true;
            }
        }
        
        
        public void AddCardToHand(Card card)
        {
            if (IsHandFull)
            {
                Debug.LogWarning($"Player {playerName}'s hand is full!");
                return;
            }
            
            hand.Add(card);
            card.SetInHand();
            
            if (handTransform != null)
            {
                card.transform.SetParent(handTransform);
                LayoutHand();
            }
        }
        
        public bool RemoveCardFromHand(Card card)
        {
            if (hand.Remove(card))
            {
                LayoutHand();
                return true;
            }
            return false;
        }
        
        public void AddComposedFormula(ComposedFormula formula)
        {
            composedFormulas.Add(formula);
            
            if (formulaAreaTransform != null)
            {
                formula.transform.SetParent(formulaAreaTransform);
                LayoutFormulas();
            }
        }
        
        public bool RemoveComposedFormula(ComposedFormula formula)
        {
            if (composedFormulas.Remove(formula))
            {
                LayoutFormulas();
                return true;
            }
            return false;
        }
        
        [Command]
        public void CmdAddScore(int points)
        {
            score += points;
            Debug.Log($"{playerName} earned {points} points! Total: {score}");
        }
        
        public void AddScore(int points)
        {
            if (isServer)
            {
                score += points;
            }
            else
            {
                CmdAddScore(points);
            }
        }
        
        private void LayoutHand()
        {
            if (handTransform == null) return;
            
            float cardWidth = 1.5f;
            float spacing = 0.3f;
            float totalWidth = hand.Count * cardWidth + (hand.Count - 1) * spacing;
            float startX = -totalWidth / 2f;
            
            for (int i = 0; i < hand.Count; i++)
            {
                float x = startX + i * (cardWidth + spacing);
                hand[i].transform.localPosition = new Vector3(x, 0, 0);
            }
        }
        
        private void LayoutFormulas()
        {
            if (formulaAreaTransform == null) return;
            
            float formulaWidth = 1.5f;
            float spacing = 0.3f;
            
            for (int i = 0; i < composedFormulas.Count; i++)
            {
                float x = i * (formulaWidth + spacing);
                composedFormulas[i].transform.localPosition = new Vector3(x, 0, 0);
            }
        }
        
        public ComposedFormula FindFormula(FormulaType type)
        {
            return composedFormulas.Find(f => f.FormulaType == type);
        }
    }
}
