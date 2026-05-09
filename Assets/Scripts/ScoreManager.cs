using UnityEngine;
using System;

namespace PotionMaking
{
    public class ScoreManager : MonoBehaviour
    {
        public event Action<Player, int> OnScoreChanged;
        
        public void AwardPoints(Player player, int points, string reason = "")
        {
            player.AddScore(points);
            OnScoreChanged?.Invoke(player, player.score);
            
            if (!string.IsNullOrEmpty(reason))
            {
                Debug.Log($"{player.playerName} +{points} points ({reason})");
            }
        }
        
        public void GetScoreDisplay(int totalScore, out int onesCounter, out int tensCounter)
        {
            onesCounter = totalScore % 10;
            tensCounter = (totalScore / 10) * 10;
        }
        
        public Player[] GetWinners(Player[] allPlayers)
        {
            int highestScore = 0;
            int winnerCount = 0;
            
            foreach (Player p in allPlayers)
            {
                if (p.score > highestScore)
                {
                    highestScore = p.score;
                    winnerCount = 1;
                }
                else if (p.score == highestScore)
                {
                    winnerCount++;
                }
            }
            
            Player[] winners = new Player[winnerCount];
            int index = 0;
            foreach (Player p in allPlayers)
            {
                if (p.score == highestScore)
                {
                    winners[index++] = p;
                }
            }
            
            return winners;
        }
    }
}
