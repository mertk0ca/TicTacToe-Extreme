using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // Slider referansý
    private AudioSource musicSource;

    void Start()
    {
        // "Music" tag'li objenin AudioSource'unu al
        GameObject musicObject = GameObject.FindGameObjectWithTag("Music");
        if (musicObject != null)
        {
            musicSource = musicObject.GetComponent<AudioSource>();
            volumeSlider.value = musicSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        else
        {
            Debug.LogWarning("Music object not found! Make sure your music GameObject has the 'Music' tag.");
        }
    }

    void SetVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }
}
