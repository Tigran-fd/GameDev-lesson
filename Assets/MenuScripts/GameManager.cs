using UnityEngine;

namespace PotionMaking
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Components - Assign in Inspector")]
        [SerializeField] private Player[] players;
        [SerializeField] private DeskOfElements desk;
        [SerializeField] private Deck deck;
        [SerializeField] private TurnManager turnManager;
        [SerializeField] private ActionExecutor actionExecutor;
        [SerializeField] private ScoreManager scoreManager;
        
        [Header("Settings")]
        [SerializeField] private int initialHandSize = 4;
        [SerializeField] private int initialDeskSize = 4;
        
        private void Start()
        {
            InitializeGame();
        }
        
        public void InitializeGame()
        {
            Debug.Log("=== POTION MAKING PRACTICE ===");
            Debug.Log("Initializing game...");
            
            deck.Initialize();

            desk.Initialize();

            scoreManager = GetComponent<ScoreManager>();
            if (scoreManager == null)
                scoreManager = gameObject.AddComponent<ScoreManager>();
            
            turnManager.Initialize(players, deck);
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
        
        public void RestartGame()
        {
            foreach (Player player in players)
            {
                player.hand.Clear();
                player.composedFormulas.Clear();
                player.score = 0;
            }
            
            InitializeGame();
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
                Debug.LogWarning("Cannot end turn yet! Must draw and play a card.");
                return;
            }
            
            turnManager.EndTurn();
        }
        
        public GameState GetGameState()
        {
            return new GameState
            {
                currentPlayer = turnManager.CurrentPlayer,
                canDraw = turnManager.CanDrawCard,
                canPlay = turnManager.CanPlayCard,
                canEndTurn = turnManager.CanEndTurn,
                cardsInDeck = deck.CardsRemaining
            };
        }
    }
    
    public struct GameState
    {
        public Player currentPlayer;
        public bool canDraw;
        public bool canPlay;
        public bool canEndTurn;
        public int cardsInDeck;
    }
}
