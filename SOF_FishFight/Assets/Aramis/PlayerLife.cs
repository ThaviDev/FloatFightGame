using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public string nombreJugador;
    public int playerID;
    public C_Jugador jugador;

    private void Awake()
    {
        // Ensure the link to the C_Jugador component so other systems can safely read Lives
        if (jugador == null)
        {
            jugador = GetComponent<C_Jugador>();
        }
    }

    public void Update()
    {
        if (jugador == null)
            return;

        if (jugador.Lives <= 0 && !jugador.defeated)
        {
            jugador.defeated = true;
            GameManager.Instance.JugadorEliminado(this);
        }
    }
}
