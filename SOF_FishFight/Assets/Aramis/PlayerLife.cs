using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public string nombreJugador;
    public int playerID;
    public C_Jugador jugador;

    public void Update()
    {

        if (jugador.Lives <= 0 && !jugador.defeated)
        {
            jugador.defeated = true;
            GameManager.Instance.JugadorEliminado(this);
        }
    }
}
