using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : Singleton<AudioController>
{
	public enum SOUND
	{
		
	}
	
	//================================================================================================================//
	
	[SerializeField, ReadOnly]
	private AudioSource audioSource;

	//================================================================================================================//
	
	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}
	
	//================================================================================================================//

	public void PlaySound(SOUND sound, float volume = 1f)
	{
		audioSource.PlayOneShot(GetAudioClip(sound), volume);
	}
	
	public void PlaySound(AudioClip clip, float volume = 1f)
	{
		audioSource.PlayOneShot(clip, volume);
	}
	

	private AudioClip GetAudioClip(SOUND sound)
	{
		switch (sound)
		{
			default:
				throw new NotImplementedException($"{sound} is missing from sounds.");
		}
	}
	
	//================================================================================================================//

}
