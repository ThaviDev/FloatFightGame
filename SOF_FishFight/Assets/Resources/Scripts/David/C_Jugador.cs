using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using static UnityEngine.ParticleSystem;

public class C_Jugador : MonoBehaviour
{
    private PlayerInput m_PlayerInput;
    private InputAction m_MoveInput;
    private InputAction m_GoUpInput;
    private InputAction m_GoDownInput;
    private InputAction m_AtkFastInput;
    private InputAction m_AtkStrongInput;

    private Rigidbody m_rb;
    [SerializeField] LayerMask m_PlayerLayerMask;
    GameObject m_Visual;

    [Header("Character Selection")]
    [SerializeField] int m_CharacterID;
    //public int CharacterID { get { return m_CharacterID; } set { m_CharacterID = value; } }
    [SerializeField] Material[] m_CharacterMaterial;
    //private Vector2 m_MoveDir2DRaw;
    private Vector3 m_MoveDir;
    [Header("Movement Settings")]
    [SerializeField] float m_MoveMaxSpeed;
    [SerializeField] float m_MoveAccel;
    [SerializeField] float m_MoveDecel;
    [Header("Attack Settings")]
    [SerializeField] Vector3 m_ColOffset;

    [SerializeField] float m_AtkFastDamage;
    [SerializeField] float m_AtkFastRange;
    [SerializeField] float m_AtkFastSetCooldown;
    public float AtkFastSetCooldown { get { return m_AtkFastSetCooldown; } }
    private float m_AtkFastCooldown;
    public float AtkFastCooldown { get { return m_AtkFastCooldown; } }

    [SerializeField] float m_AtkStrongDamage;
    [SerializeField] float m_AtkStrongRange;
    [SerializeField] float m_AtkStrongSetCooldown;
    public float AtkStrongSetCooldown { get { return m_AtkStrongSetCooldown; } }
    private float m_AtkStrongCooldown;
    public float AtkStrongCooldown { get { return m_AtkStrongCooldown; } }

    [Header("Health Settings")]
    [SerializeField] float m_MaxHealth = 20f;
    public float MaxHealth { get { return m_MaxHealth; } }
    private float m_CurrentHealth;
    // IVONNE!, accede a la vida actual con CurrentHealth
    public float CurrentHealth { get { return m_CurrentHealth; } }
    private int m_Lives = 3;
    // IVONNE!, accede a las vidas del jugador con Lives
    public int Lives { get { return m_Lives; } }

    // Bool de jugador eliminado-Aramis
    public bool defeated = false;
    public string nombreJugador;
    public int playerID;
    //public AudioSource deathsound;
    //public ParticleSystem particlesFastAttack;
    //public ParticleSystem particlesStrongAttack;
    //public ParticleSystem particlesDeath;


    [Header("Respawn Settings")]
    [SerializeField] float m_RespawnDuration = 2f;
    float m_CurrentRespawnTime = 0;
    [SerializeField] Transform m_RespawnPoint;
    [SerializeField] private bool m_CanControl;
    public bool CanControl { get { return m_CanControl; } set { m_CanControl = value; } }

    private void Awake()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        //deathsound = GetComponent<AudioSource>();
        //particlesDeath = GetComponent<ParticleSystem>();
        //particlesFastAttack = GetComponent<ParticleSystem>();
        //particlesStrongAttack = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        m_RespawnPoint = FindAnyObjectByType<RespawnPoint>().transform;
        transform.position = m_RespawnPoint.position;
        m_MoveInput = m_PlayerInput.actions["Move"];
        m_GoUpInput = m_PlayerInput.actions["GoUp"];
        m_GoDownInput = m_PlayerInput.actions["GoDown"];
        m_AtkFastInput = m_PlayerInput.actions["AttackFast"];
        m_AtkStrongInput = m_PlayerInput.actions["AttackStrong"];

        //jumpInput.performed += OnJump() *EJEMPLO DE SUSCRIPCION DE EVENTO*
    }
    private void OnDisable()
    {
        //jumpInput.performed -= OnJump() *POR SI SE USA SUSCRIPCION*
    }
    void Start()
    {
        m_CanControl = true;
        m_CurrentHealth = m_MaxHealth;
        m_Visual = this.gameObject.transform.GetChild(0).gameObject;
        m_rb = GetComponent<Rigidbody>();
        if (m_PlayerLayerMask == 0)
        {
            m_PlayerLayerMask = LayerMask.GetMask("Player");
        }
        ChangeCharacter();
    }

    void Update()
    {
        if (m_CurrentRespawnTime > 0)
        {
            m_CurrentRespawnTime -= Time.deltaTime;
            if (m_CurrentRespawnTime <= 0)
            {
                Respawn();
            }
        }
        if (!m_CanControl)
        {
            m_MoveDir = Vector3.zero;
            return;
        }
        Vector2 m_HorMovement = m_MoveInput.ReadValue<Vector2>();
        //Vector2 m_VertMovement = new Vector2 (Convert.ToInt32(m_JumpInput.ReadValue<bool>()), Convert.ToInt32(m_CrouchInput.ReadValue<bool>()));
        //print("Jump" + m_GoUpInput.ReadValue<float>());
        //print("Crouch" + m_GoDownInput.ReadValue<float>());
        float m_VertMovement = m_GoUpInput.ReadValue<float>() - m_GoDownInput.ReadValue<float>();
        //print("MovimientoVertical" + m_VertMovement);
        //m_MoveDir = m_MoveInput.ReadValue<Vector2>();

        if (Gamepad.current != null)
        {
            m_HorMovement.y *= -1; // Invertir el eje Y para gamepad
        }
        //Quaternion isRotation = Quaternion.Euler(0, -45f, 0);
        //m_HorMovement = isRotation * m_HorMovement; // Rotar el movimiento horizontal 90 grados

        m_MoveDir = new Vector3(m_HorMovement.x, m_VertMovement, m_HorMovement.y);

        if (m_AtkFastInput.ReadValue<float>() > 0)
        {
            FastAttack();
        }
        if (m_AtkStrongInput.ReadValue<float>() > 0)
        {
            StrongAttack();
        }

        if (m_AtkFastCooldown > 0)
        {
            m_AtkFastCooldown -= Time.deltaTime;
        }
        if (m_AtkStrongCooldown > 0)
        {
            m_AtkStrongCooldown -= Time.deltaTime;
        }
    }

    // ACCEDAN A ESTA FUNCION PARA CAMBIO DE PERSONAJE

    public void ChangeCharacter()
    {
        int characterID;

        if (m_PlayerInput.playerIndex == 0)
        {
            print("Accedo a mi personaje 1");
            characterID = FindAnyObjectByType<C_SetCharacters>().m_p1.CharacterID;
            FindAnyObjectByType<C_BattleUIManager>().SetPlayer1UI(this);
        }
        else if (m_PlayerInput.playerIndex == 1)
        {
            print("Accedo a mi personaje 2");
            characterID = FindAnyObjectByType<C_SetCharacters>().m_p2.CharacterID;
            FindAnyObjectByType<C_BattleUIManager>().SetPlayer2UI(this);
        }
        else if (m_PlayerInput.playerIndex == 2)
        {
            print("Accedo a mi personaje 3");
            characterID = FindAnyObjectByType<C_SetCharacters>().m_p3.CharacterID;
            FindAnyObjectByType<C_BattleUIManager>().SetPlayer3UI(this);
        }
        else if (m_PlayerInput.playerIndex == 3)
        {
            print("Accedo a mi personaje 4");
            characterID = FindAnyObjectByType<C_SetCharacters>().m_p4.CharacterID;
            FindAnyObjectByType<C_BattleUIManager>().SetPlayer4UI(this);
        }
        else
        {
            characterID = 0; // Valor por defecto o error
        }
        //ARAMIS: Efecto de cambio de personaje (Pantalla de seleccion de personaje)
        print("Cambio de material " + characterID);
        m_Visual.GetComponent<Renderer>().material = m_CharacterMaterial[characterID];
    }
    private void FastAttack()
    {
        //ARAMIS: Efecto de ataque rápido
        if (m_AtkFastCooldown <= 0)
        {
            print("Fast Attack!");
            Vector3 center = transform.position + m_ColOffset;
            Collider[] hit = Physics.OverlapBox(center, new Vector3(m_AtkFastRange, m_AtkFastRange, m_AtkFastRange), Quaternion.identity, m_PlayerLayerMask);
            //particlesFastAttack.Play();
            if (hit.Length > 0)
            {
                foreach (var h in hit)
                {
                    if (h.gameObject != this.gameObject.transform.GetChild(0).gameObject)
                    {
                        print("Attacked: " + h.gameObject.name);
                        h.gameObject.transform.parent.GetComponent<C_Jugador>()?.RecieveDamage(m_AtkFastDamage);
                    }
                }
            }
            m_AtkFastCooldown = m_AtkFastSetCooldown;
        }
    }
    private void StrongAttack()
    {
        //ARAMIS: Efecto de ataque fuerte
        if (m_AtkStrongCooldown <= 0)
        {
            print("Strong Attack!");
            Vector3 center = transform.position + m_ColOffset;
            Collider[] hit = Physics.OverlapBox(center, new Vector3(m_AtkStrongRange, m_AtkStrongRange, m_AtkStrongRange), Quaternion.identity, m_PlayerLayerMask);
            //particlesStrongAttack.Play();
            if (hit.Length > 0)
            {
                foreach (var h in hit)
                {
                    if (h.gameObject != this.gameObject.transform.GetChild(0).gameObject)
                    {
                        print("Attacked: " + h.gameObject.name);
                        h.gameObject.transform.parent.GetComponent<C_Jugador>()?.RecieveDamage(m_AtkStrongDamage);
                    }
                }
            }
            m_AtkStrongCooldown = m_AtkStrongSetCooldown;
        }
    }

    private void OnDrawGizmos()
    {
        // Gizmos para visualizar el rango de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + m_ColOffset, m_AtkFastRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + m_ColOffset, m_AtkStrongRange);
    }
    private void OnTriggerEnter(Collider collision)
    {
        //print("Colisione con " + collision.gameObject.name);
        if (collision.CompareTag("Game Area"))
        {
            m_CanControl = true;
            // Lógica para cuando colisiona con otro jugador
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Game Area"))
        {
            m_CanControl = false;
            // Lógica para cuando colisiona con otro jugador
        }
        if (collision.CompareTag("Hazard"))
        {
            print("damage");
            RecieveDamage(999f);
        }
        //print("Salí de la colision con " + collision.gameObject.name);
    }

    public void RecieveDamage(float damage)
    {
        print("Recibi danio");
        //ARAMIS: Efecto de recibir danio
        m_CurrentHealth -= damage;
        if (m_CurrentHealth <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        //ARAMIS: Efecto de muerte
        m_Lives -= 1;
        m_CurrentHealth = m_MaxHealth;
        m_Visual.SetActive(false);
        m_CanControl = false;
        print("Player Died! Lives left: " + m_Lives);
        //particlesDeath.Play();
        //deathsound.Play();
        if (m_Lives > 0)
        {
            m_CurrentRespawnTime = m_RespawnDuration;
        }
        
    }
    public void Respawn()
    {
        //ARAMIS: Efecto de respawn
        print("Player Respawned!");
        m_CanControl = true;
        transform.position = m_RespawnPoint.position;
        m_Visual.SetActive(true);
    }
    private void FixedUpdate()
    {
        MovimientoFisico();
    }

    private void MovimientoFisico()
    {
        if (!m_CanControl)
            return;
        if (m_MoveDir != Vector3.zero)
        {
            m_rb.AddForce(m_MoveDir * m_MoveAccel, ForceMode.Force);
            // Limitar Velocidad Maxima
            if (m_rb.linearVelocity.magnitude > m_MoveMaxSpeed)
            {
                m_rb.linearVelocity = m_rb.linearVelocity.normalized * m_MoveMaxSpeed;
            }
        }
        else
        {
            m_rb.AddForce(m_rb.linearVelocity * -m_MoveDecel, ForceMode.Force);
        }
    }
}
