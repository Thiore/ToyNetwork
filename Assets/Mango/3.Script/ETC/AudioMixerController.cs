using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
	[SerializeField] private Slider m_MusicMasterSlider;
	[SerializeField] private Slider m_MusicBGMSlider;
	[SerializeField] private Slider m_MusicSFXSlider;

	private const float MIN_VOLUME = -80f; // 오디오 믹서의 최소 볼륨 (데시벨 단위)
	private const float MAX_VOLUME = 0f; // 오디오 믹서의 최대 볼륨 (데시벨 단위)

	private void Awake()
	{
		m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
		m_MusicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
		m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
	}

	private void Start()
	{
		// 오디오 믹서의 현재 값을 슬라이더에 설정
		float masterVolume;
		float bgmVolume;
		float sfxVolume;

		AudioManager.Instance.m_AudioMixer.GetFloat("Master", out masterVolume);
		AudioManager.Instance.m_AudioMixer.GetFloat("BGM", out bgmVolume);
		AudioManager.Instance.m_AudioMixer.GetFloat("SFX", out sfxVolume);

		m_MusicMasterSlider.value = Mathf.InverseLerp(MIN_VOLUME, MAX_VOLUME, masterVolume);
		m_MusicBGMSlider.value = Mathf.InverseLerp(MIN_VOLUME, MAX_VOLUME, bgmVolume);
		m_MusicSFXSlider.value = Mathf.InverseLerp(MIN_VOLUME, MAX_VOLUME, sfxVolume);
	}

	public void SetMasterVolume(float volume)
	{
		AudioManager.Instance.m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);

	}

	public void SetMusicVolume(float volume)
	{
		AudioManager.Instance.m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
	}

	public void SetSFXVolume(float volume)
	{
		AudioManager.Instance.m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
	}
}