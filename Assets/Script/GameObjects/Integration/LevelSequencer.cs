using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSequencer : MonoBehaviour
{
    [Header("Referencias")]
    public FadeScreen fadeScreen; // Arrastra el objeto FadeScreen aquí
    public ProgressBarUpdater progressBarUpdater; // Arrastra el script de la barra de progreso aquí

    [Header("Tiempos y Escenas")]
    [Tooltip("Tiempo que la barra se muestra antes de pasar al juego.")]
    public float progressDisplayDuration = 3.0f;

    [Tooltip("Nombre EXACTO del primer nivel.")]
    public string firstLevelSceneName = "Escenas/Parejas";

    void Start()
    {
        // 1. Mostrar la nueva escena con un FadeIn
        if (fadeScreen != null)
        {
            fadeScreen.FadeIn(); // Asumiendo que FadeScreen.Start() no lo hace si lo llamas aquí.
            // Si FadeScreen.fadeOnStart es true, puedes omitir esta línea.
        }

        // 2. Asegurarse de que la barra de progreso se actualice al cargar
        if (progressBarUpdater != null)
        {
            progressBarUpdater.UpdateGlobalProgress();
        }

        // 3. Iniciar la secuencia de espera y carga
        StartCoroutine(LoadFirstLevelRoutine());
    }

    private IEnumerator LoadFirstLevelRoutine()
    {
        // 4. Esperar el tiempo de visualización de la barra
        yield return new WaitForSeconds(progressDisplayDuration);

        // 5. Fade OUT (Oscurecer)
        if (fadeScreen != null)
        {
            fadeScreen.FadeOut();
            // Esperar a que el FadeOut termine antes de cargar la escena.
            yield return new WaitForSeconds(fadeScreen.fadeDuration);
        }

        // 6. Cargar el primer nivel
        SceneManager.LoadScene(firstLevelSceneName);
    }
}