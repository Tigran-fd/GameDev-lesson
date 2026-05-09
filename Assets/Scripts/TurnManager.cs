using System;
using UnityEngine;

namespace PotionMaking
{
    public class TurnManager : MonoBehaviour
    {
        public event Action<Player> OnTurnStarted;
        public event Action<Player> OnTurnEnded;
        public event Action OnGameEnded;
        
        private Player[] players;
        private int currentPlayerIndex = 0;
        private Deck deck;
        
        private bool hasDrawnThisTurn = false;
        private bool hasPlayedThisTurn = false;
        private int extraCardsToDrawNextTurn = 0;
        
        public Player CurrentPlayer => players[currentPlayerIndex];
        public bool CanDrawCard => !hasDrawnThisTurn && !deck.IsEmpty;
        public bool CanPlayCard => !hasPlayedThisTurn;
        public bool CanEndTurn => hasDrawnThisTurn && hasPlayedThisTurn;
        
        public void Initialize(Player[] gamePlayers, Deck gameDeck)
        {
            players = gamePlayers;
            deck = gameDeck;
            currentPlayerIndex = 0;
        }
        
        public void StartTurn()
        {
            hasDrawnThisTurn = false;
            hasPlayedThisTurn = false;
            
            Debug.Log($"=== {CurrentPlayer.playerName}'s turn ===");
            OnTurnStarted?.Invoke(CurrentPlayer);
        }
        
        public void DrawPhase()
        {
            if (hasDrawnThisTurn)
            {
                Debug.LogWarning("Already drew this turn!");
                return;
            }
            
            Player current = CurrentPlayer;
            
            int cardsToDraw = 1;
            
            if (extraCardsToDrawNextTurn > 0)
            {
                cardsToDraw = extraCardsToDrawNextTurn;
                extraCardsToDrawNextTurn = 0;
            }
            
            while (current.HandCount < 5 && cardsToDraw > 0 && !deck.IsEmpty)
            {
                Card drawn = deck.DrawCard();
                if (drawn != null)
                {
                    current.AddCardToHand(drawn);
                    cardsToDraw--;
                }
            }
            
            hasDrawnThisTurn = true;
        }
        
        public void PlayCard()
        {
            hasPlayedThisTurn = true;
        }
        
        public void SpellPlayed(SpellType spellType)
        {
            hasPlayedThisTurn = false;
            
            if (spellType == SpellType.Decomposition || 
                spellType == SpellType.Transformation)
            {
                extraCardsToDrawNextTurn++;
            }
        }
        
        public void EndTurn()
        {
            if (!CanEndTurn)
            {
                Debug.LogWarning("Cannot end turn yet!");
                return;
            }
            
            OnTurnEnded?.Invoke(CurrentPlayer);
            
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
            
            if (deck.IsEmpty && AllPlayersHandsEmpty())
            {
                EndGame();
                return;
            }
            
            StartTurn();
        }
        
        private bool AllPlayersHandsEmpty()
        {
            foreach (Player p in players)
            {
                if (p.HandCount > 0)
                    return false;
            }
            return true;
        }
        
        private void EndGame()
        {
            Debug.Log("=== GAME OVER ===");
            
            int highestScore = 0;
            foreach (Player p in players)
            {
                if (p.score > highestScore)
                    highestScore = p.score;
            }
            
            Debug.Log($"Winning score: {highestScore}");
            foreach (Player p in players)
            {
                if (p.score == highestScore)
                    Debug.Log($"WINNER: {p.playerName}");
            }
            
            OnGameEnded?.Invoke();
        }
    }
}
