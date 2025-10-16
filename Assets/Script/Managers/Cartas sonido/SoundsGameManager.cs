using NUnit.Framework;
using System;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SoundsGameManager : MonoBehaviour
{

    [Header("Texto de prueba")]
    [SerializeField] TextMeshProUGUI texto;

    [Header("Jugador")]
    [SerializeField] XROrigin playerRig;

    [Serializable]
    public class level
    {
        [Header("Sockets del nivel")]
        public XRSocketInteractor[] sockets;

        [Header("Orden esperado")]
        public int[] nivelX;

        [Header("Cartas disponibles de este nivel")]
        public SoundCardFather[] cards;

        [Header("Posicion del siguiente nivel")]
        public Transform spawnPoint;

        [HideInInspector] public int colocadas;
    }

    [Header("Niveles")]
    [SerializeField] private level[] levels;
    private int nivelActual = 0;


    private void Awake()
    {

        foreach(level lvl in levels)
        {
            if (lvl.sockets == null || lvl.nivelX == null || lvl.sockets.Length != lvl.nivelX.Length)
            {
                Debug.LogError($"[SoundsGameManager] Level {lvl} mal configurado: sockets y expectedOrder deben tener mismo largo.");
                continue;
            }

            foreach(var s in lvl.sockets)
            {
                if (s == null)
                {
                    continue;
                }

                s.selectEntered.AddListener(OnSocketSelectEntered);
            }
        }

        //socket = GetComponent<XRSocketInteractor>();
    }

    private void OnDestroy()
    {
        // Limpieza
        foreach (var lvl in levels)
        {
            if (lvl?.sockets == null) continue;
            foreach (var s in lvl.sockets)
            {
                if (s == null) continue;
                s.selectEntered.RemoveListener(OnSocketSelectEntered);
            }
        }
    }

    void OnSocketSelectEntered(SelectEnterEventArgs args)
    {
        texto.text = $"Si detecto la carta";

        var socket = args.interactorObject as XRSocketInteractor;
        var cardGO = args.interactableObject.transform.gameObject;
        SoundCardFather card = cardGO.GetComponent<SoundCardFather>();

        if (socket == null && card == null) return;

        if (card.GetColocada()) return; // if true

        var lvl = levels[nivelActual];

        int idx = Array.IndexOf(lvl.sockets, socket);
        if (idx < 0)
        {
            return;
        }

        bool esCorrecta = (card.GetIDCard() == lvl.nivelX[idx]);
        card.SetCorrecta(esCorrecta);

        if (esCorrecta)
        {
            card.BloquearEncontrada();
        }
        else
        {
            card.BloquearErronea();
        }

        lvl.colocadas++;
        if (lvl.colocadas >= lvl.sockets.Length)
        {
            NivelTerminado();
        }

        texto.text = $"Si detecto la carta {card.GetIDCard()}, colocada: {card.GetColocada()}, correcta: {card.GetCorrecta()}";
    }

    public void NivelTerminado()
    {
        if (nivelActual + 1 < levels.Length)
        {
            foreach (var s in levels[nivelActual].sockets)
            {
                if (s == null) continue;
                s.enabled = false;
            }

            nivelActual++;
            texto.text = $"Nivel {nivelActual + 1} de {levels.Length}...";

            var nextSpawn = levels[nivelActual + 1].spawnPoint;
            if (nextSpawn != null)
                playerRig.MoveCameraToWorldLocation(nextSpawn.position);
        }
        else
        {
            JuegoTerminado();
        }


    }

    public void JuegoTerminado()
    {
        Debug.Log("Juego terminado");
        texto.text = "Juego terminado";
    }
}
