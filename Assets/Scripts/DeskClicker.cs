using UnityEngine;

public class DeskClicker : MonoBehaviour
{
    public UIManager uiManager;
    
    private void OnMouseDown()
    {
        Debug.Log("Desk clicked!");
        uiManager.OpenDeskPanel();
    }
}
