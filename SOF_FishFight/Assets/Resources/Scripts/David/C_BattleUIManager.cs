using UnityEngine;

public class C_BattleUIManager : MonoBehaviour
{
    public C_HealthUI m_P1HUD;
    public C_HealthUI m_P2HUD;
    public C_HealthUI m_P3HUD;
    public C_HealthUI m_P4HUD;
    public void SetPlayer1UI(C_Jugador player)
    {
        m_P1HUD.SetPlayer(player);
    }
    public void SetPlayer2UI(C_Jugador player)
    {
        m_P2HUD.SetPlayer(player);
    }
    public void SetPlayer3UI(C_Jugador player)
    {
        m_P3HUD.SetPlayer(player);
    }
    public void SetPlayer4UI(C_Jugador player)
    {
        m_P4HUD.SetPlayer(player);
    }
}
