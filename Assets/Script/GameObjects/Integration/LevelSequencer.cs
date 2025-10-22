using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSequencer : MonoBehaviour
{
    [Header("Referencias")]
    public FadeScreen fadeScreen;
    public ProgressBarUpdater progressBarUpdater;
    public TMP_Text countdownText;

    [Header("Tiempos y Escenas")]
    [Tooltip("Tiempo que la barra de progreso se muestra.")]
    public float progressDisplayDuration = 2.0f;

    [Tooltip("Duración de la cuenta regresiva en segundos.")]
    public int countdownSeconds = 3;

    [Tooltip("Nombre EXACTO de la escena del nivel a cargar.")]
    public string firstLevelSceneName = "Escenas/Parejas";

    void Start()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        if (fadeScreen != null)
        {
            fadeScreen.FadeIn();
        }

        if (progressBarUpdater != null)
        {
            progressBarUpdater.UpdateGlobalProgress();
        }

        StartCoroutine(LoadFirstLevelRoutine());
    }

    private IEnumerator LoadFirstLevelRoutine()
    {
        // 1. Esperar el tiempo de visualización de la barra de progreso.
        yield return new WaitForSeconds(progressDisplayDuration);

        // --- CAMBIO DE LÓGICA ---
        // 2. OSCURECER la pantalla PRIMERO (Fade Out).
        if (fadeScreen != null)
        {
            fadeScreen.FadeOut();
            // Esperar a que la transición termine y la pantalla esté completamente negra.
            yield return new WaitForSeconds(fadeScreen.fadeDuration);
        }

        // 3. Ahora que la pantalla está en negro, iniciar la cuenta regresiva.
        if (countdownText != null)
        {
            // Opcional: Ocultar la barra de progreso, ya que no se verá de todos modos.
            if (progressBarUpdater != null)
            {
                progressBarUpdater.gameObject.SetActive(false);
            }
            yield return StartCoroutine(CountdownRoutine());
        }
        // --- FIN DEL CAMBIO ---

        // 4. Cargar el primer nivel. La nueva escena se encargará de hacer el FadeIn para revelarse.
        SceneManager.LoadScene(firstLevelSceneName);
    }

    private IEnumerator CountdownRoutine()
    {
        // Activa el objeto de texto para que sea visible sobre la pantalla negra.
        countdownText.gameObject.SetActive(true);

        for (int i = countdownSeconds; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "¡Go!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
    }
}