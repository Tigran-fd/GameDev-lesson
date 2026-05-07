using UnityEngine;
using UnityEngine.EventSystems; // Обязательно для работы с событиями

public class UISoundHandler : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [Header("Настройки звука")]
    public AudioSource audioSource; // Ссылка на компонент AudioSource
    public AudioClip hoverSound;    // Файл звука наведения

    [Range(0.1f, 2f)]
    public float pitchRange = 0.1f; // Разброс высоты звука, чтобы не надоедало

    // Срабатывает при наведении мыши
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySound();
    }

    // Срабатывает при выделении кнопками клавиатуры или геймпада
    public void OnSelect(BaseEventData eventData)
    {
        PlaySound();
    }

    private void PlaySound()
    {
        if (audioSource != null && hoverSound != null)
        {
            // Небольшая вариация высоты звука для "живости"
            audioSource.pitch = Random.Range(1f - pitchRange, 1f + pitchRange);
            audioSource.PlayOneShot(hoverSound);
        }
    }
}