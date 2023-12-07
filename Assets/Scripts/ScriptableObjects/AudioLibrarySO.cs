using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IDLE-DUNGEON/AudioList")]
public class AudioLibrarySO : ScriptableObject
{
	public static AudioLibrarySO instance;

	[SerializeField] private AudioSource audioSourceOriginal;
	[SerializeField] private AudioClip buttonSound;
	[SerializeField] private AudioClip buttonUpgradeSound;
	[SerializeField] private AudioClip collapseMenuSound;
	[SerializeField] private AudioClip defeatSound;
	[SerializeField] private AudioClip victorySound;
	[SerializeField] private AudioClip telepOutSound;
	[SerializeField] private AudioClip telepInSound;
	[SerializeField] private AudioClip charcterChooseSound;
	[SerializeField] private List<AudioClip> menuMusic;
	[SerializeField] private List<AudioClip> gameMusic;

	private AudioSource currentMusic;

	public void PlayMenuMusic()
	{
		PlayMusicInternal(menuMusic[UnityEngine.Random.Range(0, menuMusic.Count)], 200);
	}

	public void PlayGameMusic()
	{
		PlayMusicInternal(gameMusic[UnityEngine.Random.Range(0, gameMusic.Count)], 200);
	}

	public void PlayClip(AudioClip clip, int priority)
	{
		PlaySoundInternal(clip, priority);
	}

	public void PlayButtonSound()
	{
		PlaySoundInternal(buttonSound, 10);
	}

	public void PlayButtonUpgradeSound()
	{
		PlaySoundInternal(buttonUpgradeSound, 3);
	}

	public void PlayCollapseSound()
	{
		PlaySoundInternal(collapseMenuSound, 4);
	}

	public void PlayDefeatSound()
	{
		StopMusic();
		PlaySoundInternal(defeatSound, 30);
	}

	public void PlayVictorySound()
	{
		StopMusic();
		PlaySoundInternal(victorySound, 30);
	}

	public void PlayTelepInSound()
	{
		PlaySoundInternal(telepInSound, 5);
	}

	public void PlayTelepOutSound()
	{
		PlaySoundInternal(telepOutSound, 5);
	}

	public void PlayCharacterChooseSound()
	{
		PlaySoundInternal(charcterChooseSound, 10);
	}


	private void PlaySoundInternal(AudioClip sound, int priority)
	{
		var audioSource = Instantiate(audioSourceOriginal, Camera.main.transform.position, Quaternion.identity);
		audioSource.clip = sound;
		audioSource.priority = priority;
		audioSource.Play();
		Destroy(audioSource.gameObject, audioSource.clip.length);
	}

	private void PlayMusicInternal(AudioClip sound, int priority)
	{
		StopMusic();
		var audioSource = Instantiate(audioSourceOriginal, Camera.main.transform.position, Quaternion.identity);
		audioSource.clip = sound;
		audioSource.priority = priority;
		audioSource.loop = true;
		audioSource.Play();
		audioSource.volume = 0.6f;
		currentMusic = audioSource;
	}

	private void StopMusic()
	{
		if (currentMusic)
		{
			Destroy(currentMusic.gameObject);
		}
	}
}
