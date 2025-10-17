using System;
using System.Net.Sockets;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SortSizeGame : MonoBehaviour
{

    [Header("Spawn inicial del jugador")]
    [SerializeField] private Transform startSpawnPoint;

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

        [Header("Objetos disponibles de este nivel")]
        public ObjectSort[] cards;

        [Header("Posicion del siguiente nivel")]
        public Transform spawnPoint;

        [HideInInspector] public int colocadas;
    }

    [Header("Niveles")]
    [SerializeField] private level[] levels;
    private int nivelActual = 0;

    private void Awake()
    {
        // Tepea el jugador a la posicion inicial donde debe aparecer
        if (playerRig != null && startSpawnPoint != null)
        {
            playerRig.MoveCameraToWorldLocation(startSpawnPoint.position);
            playerRig.MatchOriginUpCameraForward(startSpawnPoint.up, startSpawnPoint.forward); // opcional, para rotación
        }

        foreach (level lvl in levels)
        {
            if (lvl.sockets == null || lvl.nivelX == null || lvl.sockets.Length != lvl.nivelX.Length)
            {
                Debug.LogError($"[SoundsGameManager] Level {lvl} mal configurado: sockets y expectedOrder deben tener mismo largo.");
                continue;
            }

            foreach (var s in lvl.sockets)
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

    public void EntroEnSocket(SelectEnterEventArgs args) => OnSocketSelectEntered(args);

    void OnSocketSelectEntered(SelectEnterEventArgs args)
    {
        var socket = args.interactorObject as XRSocketInteractor;
        var objGO = args.interactableObject.transform.gameObject;
        ObjectSort objeto = objGO.GetComponent<ObjectSort>();

        if (socket == null || objeto == null) return;

        if (objeto.GetColocada()) return; // if true

        var lvl = levels[nivelActual];

        int idx = Array.IndexOf(lvl.sockets, socket);
        if (idx < 0)
        {
            return;
        }

        // Alinea el objeto con el socket por si acaso
        var attach = socket.attachTransform != null ? socket.attachTransform : socket.transform;
        objeto.AlinearEn(attach);

        bool esCorrecta = (objeto.GetIDCard() == lvl.nivelX[idx]);
        objeto.SetCorrecta(esCorrecta);

        if (esCorrecta)
        {
            objeto.BloquearEncontrada();
        }
        else
        {
            objeto.BloquearErronea();
        }

        socket.allowHover = false;
        socket.allowSelect = false;

        lvl.colocadas++;
        if (lvl.colocadas >= lvl.sockets.Length)
        {
            NivelTerminado();
        }

        texto.text = $"Si detecto el objeto {objeto.GetIDCard()}, colocada: {objeto.GetColocada()}, correcta: {objeto.GetCorrecta()}";
    }

    public void NivelTerminado()
    {
        // Desactiva los sockets por si no se hubiesen desactivado
        foreach (var s in levels[nivelActual].sockets)
        {
            if (s)
            {
                s.allowHover = false;
                s.allowSelect = false;
            }
        }

        if (nivelActual + 1 < levels.Length)
        {
            nivelActual++;
            texto.text = $"Nivel {nivelActual + 1} de {levels.Length}...";

            var nextSpawn = levels[nivelActual].spawnPoint;
            if (nextSpawn)
            {
                playerRig.MoveCameraToWorldLocation(nextSpawn.position);
            }
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
