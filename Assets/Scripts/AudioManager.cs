using UnityEngine;
using System.Collections;


public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip backgroundMusic;
    public AudioClip gameOver;
    public AudioClip finalStoneDoorOpen;
    public AudioClip oldWoodDoorOpen;
    public AudioClip oldWoodDoorClose;
    public AudioClip walkingOnMarble;
    public AudioClip runningOnMarble;
    public AudioClip collectGem;
    public AudioClip itemPickup;
    public AudioClip playerSpotted;
    public AudioClip enemyChasingPlayer;
    public AudioClip LevelCompleted;

    private AudioSource[] audioSources;

    void Start()
    {
        audioSources = GetComponentsInChildren<AudioSource>();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("Tentative de jouer un son, mais l'AudioClip est NULL !");
            return;
        }

        // Crée un nouvel AudioSource temporaire
        GameObject tempAudioSource = new GameObject("TempAudioSource");
        AudioSource source = tempAudioSource.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();

        // Détruit l'AudioSource après la durée du son
        Destroy(tempAudioSource, clip.length);
    }

    public void PlayLoopingMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying)
            return; // Évite de rejouer le son en boucle si déjà en cours

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayLoopingSFX(AudioClip clip)
    {
        if (SFXSource.clip == clip && SFXSource.isPlaying)
            return; // Évite de rejouer le son en boucle si déjà en cours

        SFXSource.clip = clip;
        SFXSource.loop = true;
        SFXSource.Play();
    }

    public void StopSFX()
    {
        SFXSource.Stop();
        SFXSource.clip = null;
    }

    public void StopAllAndPlay(AudioClip clip)
    {
        // Stoppe toutes les musiques et effets sonores
        musicSource.Stop();
        SFXSource.Stop();

        // Joue uniquement le son spécifié
        SFXSource.clip = clip;
        SFXSource.loop = false;
        SFXSource.Play();
    }

    // Mute tous les sons
    public void MuteAllSounds()
    {
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource source in allAudioSources)
        {
            source.mute = true;
        }
    }

    // Unmute tous les sons
    public void UnmuteAllSounds()
    {
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource source in allAudioSources)
        {
            source.mute = false;
        }
    }

    // Joue la musique de poursuite en boucle
    public void LoopChaseMusic()
    {
        if (musicSource.clip != enemyChasingPlayer || !musicSource.isPlaying)
        {
            PlayLoopingMusic(enemyChasingPlayer); // Assure-toi de ne pas redémarrer la musique si elle est déjà en cours
        }
    }

    // Arrêter la musique de poursuite
    public void StopChaseMusic()
    {
        if (musicSource.clip == enemyChasingPlayer)
        {
            musicSource.Stop();
        }
    }

    public void FadeInMusic(AudioClip clip, float duration)
    {
        if (musicSource.isPlaying)
        {
            StopAllCoroutines();  // On arrête le fade en cours pour recommencer proprement
        }

        musicSource.clip = clip;
        musicSource.Play();
        musicSource.volume = 0f;
        StartCoroutine(AdjustVolume(0f, 1f, duration));  // Fade in
    }

    public void FadeOutMusic(float duration)
    {
        StartCoroutine(AdjustVolume(1f, 0f, duration));  // Fade out
    }

    private IEnumerator AdjustVolume(float startVolume, float endVolume, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, endVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        musicSource.volume = endVolume;  // S'assurer que le volume final est atteint
    }
}
