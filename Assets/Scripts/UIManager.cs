using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI Image i�in
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameObject panel; // A��lacak/kapanacak panel
    public Image fadeImage;  // Fade efektini yapacak Image (Canvas i�inde olmal�)

    private void Start()
    {
        fadeImage.gameObject.SetActive(true); // Fade Image'� ba�ta g�r�n�r yap
        StartCoroutine(FadeOut()); // Ba�lang��ta fade-out efekti
    }

    // Panelin g�r�n�rl���n� de�i�tirir
    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }

    // Devam Et Butonu - Sadece paneli kapat�r
    public void ClosePanel()
    {
        StartCoroutine(ClosePanelWithDelay());
    }

    private IEnumerator ClosePanelWithDelay()
    {
        // 0.5 saniye bekle
        yield return new WaitForSeconds(0.3f);

        // Paneli kapat
        panel.SetActive(false);
    }

    // Ana Men� Butonu - Fade efekti ile sahneyi y�kler
    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneWithFade("MainMenu"));
    }

    // SampleScene Butonu - Fade efekti ile sahneyi y�kler
    public void LoadScene()
    {
        StartCoroutine(LoadSceneWithFade("SampleScene"));
    }

    // Fade efekti ile sahne y�kler
    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        yield return StartCoroutine(FadeIn()); // Fade-in yap
        SceneManager.LoadScene(sceneName); // Sahneyi y�kle
        yield return StartCoroutine(FadeOut()); // Fade-out yap
    }

    // Fade-in efekti (siyah ekranda g�r�n�rl�k artar)
    private IEnumerator FadeIn()
    {
        float timeElapsed = 0f;
        float fadeDuration = 0.5f; // 1 saniye s�resince fade in

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            fadeImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration)); // Opakl�k artt�r
            yield return null;
        }
    }

    // Fade-out efekti (siyah ekranda g�r�n�rl�k azal�r)
    private IEnumerator FadeOut()
    {
        float timeElapsed = 0f;
        float fadeDuration = 0.5f; // 1 saniye s�resince fade out

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            fadeImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration)); // Opakl�k azalt
            yield return null;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
