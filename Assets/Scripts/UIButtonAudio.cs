using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAudio : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerClickHandler
{
    public AudioSource audioSource;

    [Header("Звуки")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    [Header("Настройки громкости")]
    [Range(0f, 1f)]
    public float volumeHover = 0.3f; 
    [Range(0f, 1f)]
    public float volumeClick = 0.6f; 

    [Header("Эффект высоты (Pitch)")]
    [Range(0.1f, 2f)]
    public float pitchRange = 0.1f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySound(hoverSound, volumeHover);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData is AxisEventData)
        {
            PlaySound(hoverSound, volumeHover);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(clickSound, volumeClick);
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.pitch = Random.Range(1f - pitchRange, 1f + pitchRange);
            audioSource.PlayOneShot(clip, volume);
        }
    }
}