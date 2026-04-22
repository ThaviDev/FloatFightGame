using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawn")]
    public GameObject playerPrefab;
    public Transform[] spawnPoints;

    [Header("Nombres jugadores")]
    public string[] nombres = new string[4];

    private List<PlayerLife> jugadores = new List<PlayerLife>();
    public List<string> ranking = new List<string>();

    int contador = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        contador = 3;
        SpawnJugadores();
    }

    void SpawnJugadores()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject clon = Instantiate(playerPrefab, spawnPoints[i].position, Quaternion.identity);

            PlayerLife pl = clon.GetComponent<PlayerLife>();

            pl.playerID = i;
            pl.nombreJugador = nombres[i]; // AQUÍ es donde se deben nombrar a los jugadores

            jugadores.Add(pl);
        }
    }

    public void JugadorEliminado(PlayerLife jugador)
    {
        contador --;
        if (!ranking.Contains(jugador.nombreJugador))
        {
            ranking.Add(jugador.nombreJugador);
        }
        if (contador <= 0)
        {
            VerificarFin(jugador);
        }
    }

    void VerificarFin(PlayerLife ultimoJugador)
    {
        int vivos = 0;
        PlayerLife ultimo = null;

        foreach (var j in jugadores)
        {
            if (j.jugador.Lives > 0)
            {
                vivos++;
                ultimo = j;
            }
        }

        if (vivos <= 1)
        {
            /*if (!ranking.Contains(ultimo.nombreJugador))
            {
                ranking.Add(ultimo.nombreJugador);
            }*/

            SceneManager.LoadScene("Scores");
        }
    }
}