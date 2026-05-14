using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemHose : MonoBehaviour
{
    void Awake()
    {
        EventSystem[] systems = Object.FindObjectsOfType<EventSystem>();
        
        if (systems.Length > 1)
        {
            Debug.Log("Found extra EventSystem, destroying duplicate...");
            Destroy(gameObject);
        }
    }
}