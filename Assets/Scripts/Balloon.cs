using UnityEngine;
using System;

public class Balloon : MonoBehaviour
{
    public event Action<int> OnPop;
    private AudioSource audioSource;
    public int points = 100;
    private bool isPopped = false;
    public float destroyDelay = 0.1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PopBalloon()
    {
        if (isPopped) return;
        isPopped = true;

        OnPop?.Invoke(points);

        if (audioSource != null)
        {
            audioSource.Play();
            StartCoroutine(DestroyAfterAudio());
        }
        else
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    private System.Collections.IEnumerator DestroyAfterAudio()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}
