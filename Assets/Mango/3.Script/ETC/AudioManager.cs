using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance{ get; private set; }
    public AudioMixer m_AudioMixer;

    private void Awake()
    {
        // ΩÃ±€≈Ê ¿ŒΩ∫≈œΩ∫ º≥¡§
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // æ¿ ¿¸»Ø Ω√ ∞¥√º∏¶ ¿Ø¡ˆ
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

}
