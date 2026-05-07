using UnityEngine;
using UnityEngine.SceneManagement; // Нужно для переключения уровней

public class MenuController : MonoBehaviour
{
    // 1. Начать игру (загрузить сцену по имени)
    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 2. Открыть настройки (например, включить панель настроек)
    public void OpenSettings(GameObject settingsPanel)
    {
        settingsPanel.SetActive(true);
    }

    // 3. Закрыть настройки
    public void CloseSettings(GameObject settingsPanel)
    {
        settingsPanel.SetActive(false);
    }

    // 4. Выход из игры
    public void QuitGame()
    {
        Debug.Log("Игра закрывается...");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}