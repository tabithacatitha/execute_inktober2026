using System.Diagnostics;
using UnityEngine;

public class Track : MonoBehaviour
{
    void Awake()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
    }
}