using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private AudioSource audioSource;

    public AudioClip menuMusic;
    public AudioClip[] gameMusic;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // если сцена уже загружена
        HandleMusic(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleMusic(scene.name);
    }

    void HandleMusic(string sceneName)
    {
        if (sceneName == "main menu")
        {
            PlayMenuMusic();
        }
        else if (sceneName == "Game")
        {
            PlayRandomGameMusic();
        }
    }

    void PlayMenuMusic()
    {
        if (menuMusic == null) return;

        audioSource.clip = menuMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    void PlayRandomGameMusic()
    {
        if (gameMusic.Length == 0) return;

        int index = Random.Range(0, gameMusic.Length);

        audioSource.loop = false;
        audioSource.clip = gameMusic[index];
        audioSource.Play();
    }

    void Update()
    {
        // только дл€ game music Ч переключение треков
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (!audioSource.isPlaying)
            {
                PlayRandomGameMusic();
            }
        }
    }
}