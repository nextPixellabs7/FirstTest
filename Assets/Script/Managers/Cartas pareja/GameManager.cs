using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.XR.CoreUtils;

public class GameManager : MonoBehaviour
{
    [Header("Spawn inicial del jugador")]
    [SerializeField] private Transform startSpawnPoint;

    [Header("Jugador")]
    [SerializeField] XROrigin playerRig;

    // Singleton
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Juego")]
    [SerializeField] private float tiempo = 120f;
    [SerializeField] private int totalPairs = 6;

    private float tiempoRestante;
    private bool gameOver;
    private bool inputLocked;

    private readonly List<Card> reveladas = new List<Card>(2);
    private int parejasEncontradas;

    public bool InputLocked => inputLocked || gameOver;

    void Awake()
    {
        Instance = this;

        // Tepea el jugador a la posicion inicial donde debe aparecer
        if (playerRig != null && startSpawnPoint != null)
        {
            playerRig.MoveCameraToWorldLocation(startSpawnPoint.position);
            playerRig.MatchOriginUpCameraForward(startSpawnPoint.up, startSpawnPoint.forward); // opcional, para rotación
        }
    }

    void Start()
    {
        tiempoRestante = tiempo;
        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (!gameOver)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0f)
            {
                tiempoRestante = 0f;
                EndGame(false);
                yield break;
            }
            UpdateTimerLabel();
            yield return null;
        }
    }

    private void UpdateTimerLabel()
    {
        if (timerText)
        {
            int t = Mathf.CeilToInt(tiempoRestante);
            int m = t / 60;
            int s = t % 60;
            timerText.SetText($"{m:0}:{s:00}");
        }
    }

    public void NotifyReveal(Card card)
    {
        if (InputLocked || gameOver) return;
        if (reveladas.Contains(card)) return;

        reveladas.Add(card);
        if (reveladas.Count == 2)
            StartCoroutine(ResolvePair());
    }

    private IEnumerator ResolvePair()
    {
        inputLocked = true;
        yield return new WaitForSeconds(0.15f);

        var a = reveladas[0];
        var b = reveladas[1];

        if (a.PairId == b.PairId && a != b)
        {
            a.SetMatched(true);
            b.SetMatched(true);
            parejasEncontradas++;

            if (statusText)
                statusText.SetText($"¡Pareja encontrada! ({parejasEncontradas}/{totalPairs})");

            if (parejasEncontradas >= totalPairs)
            {
                EndGame(true);
                yield break;
            }
        }
        else
        {
            yield return new WaitForSeconds(0.7f);
            a.Flip(false, snapHome: true);
            b.Flip(false, snapHome: true);
            if (statusText)
                statusText.SetText("No coinciden, intentalo de nuevo");
        }

        reveladas.Clear();
        inputLocked = false;
    }

    private void EndGame(bool win)
    {
        gameOver = true;
        inputLocked = true;
        if (statusText)
            statusText.SetText(win ? "¡Ganaste!" : "¡Tiempo agotado!");
    }

    public bool CanInteract(Card card)
    {
        return !(gameOver || inputLocked) && !card.Matched;
    }
}
