using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy stats")]
    public float moveSpeed;
    public int Damage;
    public DamageType DamageType;
    public float AttackRate;
    public bool CanMove;

    [Header("Dependencies")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameManager gameManager;

    private Rigidbody2D rb;
    private bool allowAttack;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        SetupNavmesh();
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        allowAttack = true;
        CanMove = true;

        if (player is null) Debug.LogWarning("Enemy doesn't see player (possible that there's no player in the scene)");
        if (rb is null) Debug.LogWarning("Enemy doesn't have RigidBody2D component");
        if (gameManager is null) Debug.LogWarning("Enemy couldn't find Game Manager in this scene");
    }

    private void SetupNavmesh()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        if (navMeshAgent is null) Debug.LogWarning("Enemy doesn't have NavMeshAgent component");
        else
        {
            navMeshAgent.updateUpAxis = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.speed = moveSpeed;
            navMeshAgent.stoppingDistance = 0.7f;
        }
    }

    void Update()
    {
        if (CanMove && player != null && rb != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            rb.rotation = angle;
        }
    }

    private void FixedUpdate()
    {
        if (CanMove && player != null && navMeshAgent != null && navMeshAgent.isOnNavMesh) navMeshAgent.SetDestination(player.transform.position);
    }

    public void Stun(float seconds)
    {
        StartCoroutine(CantMoveStatic(seconds));
    }

    IEnumerator CantMoveStatic(float seconds)
    {
        CanMove = false;
        if (navMeshAgent != null) navMeshAgent.isStopped = true;
        if (rb != null) rb.velocity = Vector3.zero;
        Debug.Log($"Enemy is stunned for {seconds} s");
        yield return new WaitForSeconds(seconds);
        if (navMeshAgent != null) navMeshAgent.isStopped = false;
        CanMove = true;
    }

    public void DisableMove(float seconds, float stunTime)
    {
        StartCoroutine(CantMoveDynamic(seconds, stunTime));
    }

    IEnumerator CantMoveDynamic(float seconds, float stunTime)
    {
        CanMove = false;
        if (rb != null) rb.isKinematic = false;
        Debug.Log($"Enemy is unable to move for {seconds} s");
        yield return new WaitForSeconds(seconds);
        if (rb != null) rb.velocity = Vector3.zero;
        if (rb != null) rb.isKinematic = true;
        if (stunTime > 0) Stun(stunTime);
        CanMove = true;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (allowAttack)
                StartCoroutine(Attack(other.gameObject.GetComponent<Stats>()));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (allowAttack)
                StartCoroutine(Attack(other.gameObject.GetComponent<Stats>()));
        }
    }

    IEnumerator Attack(Stats target)
    {
        allowAttack = false;
        CanMove = false;
        //rb.isKinematic = true;
        Debug.Log($"Enemy deals {Damage} points of damage to target");
        if(target != null) target.TakeDamage(Damage, DamageType);
        yield return new WaitForSeconds(AttackRate);
        allowAttack = true;
        //rb.isKinematic = false;
        CanMove = true;
    }

    private void OnEnable()
    {
        CanMove = true;
        if(navMeshAgent != null) navMeshAgent.speed = moveSpeed;
    }

    private void OnDisable() {
        if (gameManager != null && !gameManager.isQuitting && !gameManager.isPlayerDead)
        {
            Vector2 enemyPos = new Vector2(this.transform.position.x, this.transform.position.y);
            gameManager.UpdateKills(1);
            int turnBouns = gameManager.GetKills() % 15;
            gameManager.SpawnItemAt(enemyPos, turnBouns);
            CanMove = false;
            rb.isKinematic = true;
        }
    }
}
