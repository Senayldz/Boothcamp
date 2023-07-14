using System;
using UnityEngine;

public class MouthDetector : MonoBehaviour
{
    public Action onTriggerEnter;
    public Action onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            onTriggerEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            onTriggerExit?.Invoke();
    }
}
