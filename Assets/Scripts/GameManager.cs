using UnityEngine;
using System.Collections.Generic;
using PotionMaking;

namespace PotionMaking
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Components - Assign in Inspector")]
        private List<Player> players = new List<Player>();
        
        [SerializeField] private DeskOfElements desk;
        [SerializeField] private Deck deck;
        [SerializeField] private TurnManager turnManager;
        [SerializeField] private ActionExecutor actionExecutor;
        [SerializeField] private ScoreManager scoreManager;
        
        [Header("Settings")]
        [SerializeField] private int initialHandSize = 4;
        [SerializeField] private int initialDeskSize = 4;
        
        private bool gameInitialized = false;
        
        public Player[] Players => players.ToArray();
        
        public void RegisterPlayer(Player player)
        {
            if (players.Contains(player))
                return;
            
            players.Add(player);
            player.playerIndex = players.Count - 1;
            player.playerName = $"Player {player.playerIndex + 1}";
            player.playerColor = player.playerIndex == 0 ? Color.blue : Color.red;
            
            Debug.Log($"Registered {player.playerName}");
            
            if (players.Count == 2 && !gameInitialized)
            {
                StartCoroutine(InitializeGameDelayed());
            }
        }
        
        public void UnregisterPlayer(Player player)
        {
            players.Remove(player);
            Debug.Log($"Unregistered player, {players.Count} remaining");
        }
        
        private System.Collections.IEnumerator InitializeGameDelayed()
        {
            yield return new WaitForSeconds(0.5f);
            InitializeGame();
        }
        
        public void InitializeGame()
        {
            if (gameInitialized)
                return;
                
            gameInitialized = true;
            
            Debug.Log("=== POTION MAKING PRACTICE ===");
            Debug.Log("Initializing game...");
            
            deck.Initialize();
            
            desk.Initialize();
            
            if (scoreManager == null)
                scoreManager = gameObject.AddComponent<ScoreManager>();
            
            turnManager.Initialize(Players, deck);
            actionExecutor.Initialize(desk, scoreManager, turnManager);
            
            foreach (Player player in players)
            {
                for (int i = 0; i < initialHandSize; i++)
                {
                    Card card = deck.DrawCard();
                    if (card != null)
                    {
                        player.AddCardToHand(card);
                    }
                }
                Debug.Log($"{player.playerName} drew {player.HandCount} cards");
            }
            
            for (int i = 0; i < initialDeskSize; i++)
            {
                Card card = deck.DrawCard();
                if (card != null)
                {
                    ElementType element = card.Data.element;
                    desk.AddElement(card, element);
                }
            }
            
            Debug.Log($"Placed {initialDeskSize} initial cards on desk");
            
            turnManager.StartTurn();
            
            Debug.Log("Game initialized! Ready to play.");
        }
        
        public void DrawCard()
        {
            if (!turnManager.CanDrawCard)
            {
                Debug.LogWarning("Cannot draw card right now!");
                return;
            }
            
            turnManager.DrawPhase();
        }
        
        public void PlayCardAsElement(Card card, ElementType? asElement = null)
        {
            if (!turnManager.CanPlayCard)
            {
                Debug.LogWarning("Cannot play card right now!");
                return;
            }
            
            actionExecutor.PlayCardAsElement(turnManager.CurrentPlayer, card, asElement);
        }
        
        public void ComposeFormula(Card formulaCard, 
            System.Collections.Generic.List<Card> elementComponents,
            System.Collections.Generic.List<ComposedFormula> formulaComponents)
        {
            if (!turnManager.CanPlayCard)
            {
                Debug.LogWarning("Cannot play card right now!");
                return;
            }
            
            actionExecutor.ComposeFormula(turnManager.CurrentPlayer, 
                formulaCard, elementComponents, formulaComponents);
        }
        
        public void EndTurn()
        {
            if (!turnManager.CanEndTurn)
            {
                Debug.LogWarning("Cannot end turn yet!");
                return;
            }
            
            turnManager.EndTurn();
        }
    }
}
