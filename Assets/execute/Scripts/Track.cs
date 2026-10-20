using System;
using System.Diagnostics;
using UnityEngine;

public class Track : MonoBehaviour
{
    static double startTime = 0f;
    static readonly float bpm = 150f;
    // static readonly float beats_per_bar = 4;
    
    public void Initialize(float volume, float time = 0f)
    {
        AudioListener listener = gameObject.AddComponent<AudioListener>();
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.volume = volume;
        AudioClip clip = Resources.Load<AudioClip>("lime"); // Limerence - Daddy
        source.clip = clip;
        startTime = AudioSettings.dspTime + 3f - time;
        source.PlayScheduled(startTime);
        if (time > 0)
        {
            source.time = time;
        }
    }

    public static double BeatsToSeconds(double beats)
    {
        return beats / (bpm / 60);
    }

    public class WaitUntilBeat : CustomYieldInstruction {
        double m_Beat;
        public override bool keepWaiting
        {
            get
            {
                double timeNow = AudioSettings.dspTime - startTime;
                return (timeNow * (bpm / 60)) < m_Beat;
            }
        }

        // FIXME: Redundant. There should only be one.
        public WaitUntilBeat(double beat)
        {
            m_Beat = beat;
        }

        // FIXME: Redundant. There should only be one.
        public WaitUntilBeat(float beat)
        {
            m_Beat = beat;
        }
    }
}