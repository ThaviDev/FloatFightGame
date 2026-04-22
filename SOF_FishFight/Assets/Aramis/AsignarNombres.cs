using UnityEngine;

public class AsignarNombres : MonoBehaviour
{
    public string nombre1;
    public string nombre2;
    public string nombre3;
    public string nombre4;

    void Start()
    {
        //PlayerLife[] jugadores = FindObjectsOfType<PlayerLife>();
        PlayerLife[] jugadores = FindObjectsByType<PlayerLife>(FindObjectsSortMode.None);

        string[] nombres = { nombre1, nombre2, nombre3, nombre4 };

        for (int i = 0; i < jugadores.Length && i < nombres.Length; i++)
        {
            jugadores[i].nombreJugador = nombres[i];
        }
    }
}