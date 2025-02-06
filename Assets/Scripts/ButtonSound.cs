using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    private AudioSource audioSource; // Butonun kendi AudioSource bile�eni

    void Start()
    {
        // Ayn� GameObject i�indeki AudioSource bile�enini al
        audioSource = GetComponent<AudioSource>();

        // Buton bile�enini bul ve t�klanma olay�na PlaySound fonksiyonunu ekle
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioSource.clip); // AudioSource i�inde atanm�� sesi bir kez �al
        }
        else
        {
            Debug.LogWarning("AudioSource bulunamad�! L�tfen butona AudioSource ekledi�inizden emin olun.");
        }
    }
}
