using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PotionMaking;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public GameManager gameManager;
    
    [Header("UI Elements")]
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public Button drawButton;
    public Button endTurnButton;
    public GameObject deskPanel;
    
    private void Start()
    {
        drawButton.onClick.AddListener(OnDrawClicked);
        endTurnButton.onClick.AddListener(OnEndTurnClicked);
        
        TurnManager turnMgr = FindObjectOfType<TurnManager>();
        turnMgr.OnTurnStarted += UpdateTurnDisplay;
        
        ScoreManager scoreMgr = FindObjectOfType<ScoreManager>();
        scoreMgr.OnScoreChanged += UpdateScoreDisplay;
        
        deskPanel.SetActive(false);
    }
    
    private void OnDrawClicked()
    {
        gameManager.DrawCard();
    }
    
    private void OnEndTurnClicked()
    {
        gameManager.EndTurn();
    }
    
    private void UpdateTurnDisplay(Player currentPlayer)
    {
        turnText.text = $"{currentPlayer.playerName}'s Turn";
        turnText.color = currentPlayer.playerColor;
    }
    
    private void UpdateScoreDisplay(Player player, int newScore)
    {
        if (player.playerIndex == 0)
            player1ScoreText.text = $"{player.playerName}: {newScore}";
        else if (player.playerIndex == 1)
            player2ScoreText.text = $"{player.playerName}: {newScore}";
    }
    
    public void OpenDeskPanel()
    {
        deskPanel.SetActive(true);
    }
    
    public void CloseDeskPanel()
    {
        deskPanel.SetActive(false);
    }
}
