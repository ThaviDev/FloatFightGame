using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public string nombreJugador;
    public int playerID;
    public int vidas = 3;

    private bool eliminado = false;

    public void PerderVida()
    {
        vidas--;

        if (vidas <= 0 && !eliminado)
        {
            eliminado = true;
            GameManager.Instance.JugadorEliminado(this);
        }
    }
}
