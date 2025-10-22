using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSequencer : MonoBehaviour
{
    private const string PROGRESS_KEY = "HighestUnlockedLevel";

    [Header("Referencias")]
    public FadeScreen fadeScreen; 
    public ProgressBarUpdater progressBarUpdater; 

    [Header("Tiempos y Escenas")]
    // [ESTA LÍNEA FUE REUBICADA PARA CORREGIR EL ERROR CS0103]
    [Tooltip("Tiempo que la barra se muestra antes de pasar al juego.")]
    public float progressDisplayDuration = 3.0f;
    
    [Tooltip("Lista ORDENADA de las escenas de nivel. (Elemento 0 = Nivel 1, Elemento 1 = Nivel 2, etc.)")]
    public string[] LevelSceneNames = new string[]
    {
        "Escenas/Actividades/Parejas", 
        "Escenas/Actividades/EscucharYOrdenar", 
        "Escenas/Actividades/SonidosYCartas", 
        "Escenas/Actividades/Pompones", 
        "Escenas/Actividades/Ordenar", 
    };

    void Start()
    {
        // --- CÓDIGO AÑADIDO PARA EL REINICIO EN EL EDITOR ---
        #if UNITY_EDITOR
        if (PlayerPrefs.HasKey(PROGRESS_KEY))
        {
             PlayerPrefs.DeleteKey(PROGRESS_KEY);
             Debug.Log("Progreso de simulación reseteado a Nivel 1.");
        }
        #endif
        // --------------------------------------------------

        // El Fade In se maneja mejor en el FadeScreen.Start() si 'fadeOnStart' está true.
        if (fadeScreen != null && !fadeScreen.fadeOnStart)
        {
             fadeScreen.FadeIn();
        }

        // Asegurarse de que la barra de progreso se actualice al cargar
        if (progressBarUpdater != null)
        {
            progressBarUpdater.UpdateGlobalProgress();
        }

        // Iniciar la secuencia de espera y carga del siguiente nivel
        StartCoroutine(LoadNextLevelRoutine());
    }

    private IEnumerator LoadNextLevelRoutine()
    {
        // 1. Esperar el tiempo de visualización de la barra
        yield return new WaitForSeconds(progressDisplayDuration);

        // 2. Determinar el nivel a cargar basado en el progreso guardado
        int nextLevelID = PlayerPrefs.GetInt(PROGRESS_KEY, 1); // Lee el ID del siguiente nivel a jugar
        
        // El ID del nivel 1 corresponde al índice 0 del array.
        int sceneArrayIndex = nextLevelID - 1; 

        // 3. Verificar si hay más niveles en la lista
        if (sceneArrayIndex >= 0 && sceneArrayIndex < LevelSceneNames.Length)
        {
            string sceneToLoad = LevelSceneNames[sceneArrayIndex];

            // 4. Fade OUT y cargar escena
            if (fadeScreen != null)
            {
                fadeScreen.FadeOut();
                // Esperar a que el FadeOut termine antes de cargar la escena.
                yield return new WaitForSeconds(fadeScreen.fadeDuration);
            }
            
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            // 5. Todos los niveles completados (ir al menú o escena de final de juego)
            Debug.Log("¡Todos los niveles completados! Volviendo al Menú.");
            if (fadeScreen != null)
            {
                fadeScreen.FadeOut();
                yield return new WaitForSeconds(fadeScreen.fadeDuration);
            }
            SceneManager.LoadScene("Escenas/Menu"); 
        }
    }
}