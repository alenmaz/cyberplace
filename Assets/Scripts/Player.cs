using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Fixers")]
    [SerializeField] private float rotationOffset = 120f;

    [Header("Dependencies")]
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private Camera Camera;
    [SerializeField] private GameManager GameManager;
    [SerializeField] private GameObject MinePrefab;

    [Header("Character stats")]
    public float Speed;
    public float ReloadTime;
    public float AidBonus;
    public Weapon WeaponEquipped;
    public int AmountOfBullets;

    [Header("Shield")]
    public GameObject shield;
    public Shield shieldTimer;

    [Header("Mine")]
    [SerializeField] private int[] countMineWaves;
    [SerializeField] private int CountMineNumber;

    [Header("AbilityImage")]
    public GameObject ImageE_Active;
    public GameObject ImageE_NoActive;

    private Rigidbody2D rb;
    private AbilityHolder AbilityHolder;
    private Stats playerStats;

    private bool shootInput;
    private bool abilityUseInput;
    private bool itemUseInput;

    private Vector2 moveInput;
    private Vector2 mousePosition;
    private Vector2 moveVelocity;

    [SerializeField] private int currentAmmo;
    private bool isReloading;
    private bool allowAttack = true;
    private float timeReloading = 0;

    void Start()
    {
        if (bulletSpawnPoint != null) bulletSpawnPoint.transform.Rotate(0, 0, rotationOffset - 90);
        SetupGameManager();
        AbilityHolder = GetComponent<AbilityHolder>();
        playerStats = GetComponent<Stats>();
        rb = GetComponent<Rigidbody2D>();
        Cursor.lockState = CursorLockMode.Confined;

        if (AbilityHolder is null) Debug.LogWarning("No AbilityHolder component attached to the player");
        if (playerStats is null) Debug.LogWarning("No Stats component attached to the player");
        if (WeaponEquipped is null) Debug.LogWarning("Player doesn't have a weapon equipped");
        else
        {
            currentAmmo = WeaponEquipped.MaxAmmo;
            ReloadTime = WeaponEquipped.ReloadTime;
            AmountOfBullets = 1;
        }
    }

    private void SetupGameManager()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (GameManager != null)
        {
            GameManager.UpdateMinesCounter(CountMineNumber);
            GameManager.UpdateBulletsCounter(currentAmmo, WeaponEquipped.MaxAmmo);
        }
        else Debug.LogWarning("No GameManager in scene");
    }

    void Update()
    {
        if (isReloading)
        {
            if (GameManager != null) GameManager.UpdateReloadSlider(true, timeReloading, WeaponEquipped.ReloadTime);
            timeReloading += Time.deltaTime;
        }
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        mousePosition = Camera.ScreenToWorldPoint(Input.mousePosition);
        shootInput = Input.GetButton("Fire1");
        abilityUseInput = Input.GetButton("Fire2");
        itemUseInput = Input.GetKeyDown(KeyCode.E);
        moveVelocity = moveInput.normalized * Speed;
        if (!shieldTimer.isCooldown) playerStats.CanBeDamaged = true;

        ProcessMineUseInput();
        ProcessShootInput();
        ProcessAbilityInput();
        SetActiveMine();
        //SetActiveAbility();
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

            var lookDirection = mousePosition - rb.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - rotationOffset;
            rb.rotation = angle;
        }
    }

    private void ProcessAbilityInput()
    {
        if (AbilityHolder != null) AbilityHolder.ProcessActive(abilityUseInput);
    }

    private void ProcessShootInput()
    {
        if (shootInput)
        {
            if (WeaponEquipped is null) Debug.Log("No weapon equipped");
            else
            {
                if (!isReloading && allowAttack)
                {
                    if (currentAmmo > 0) StartCoroutine(Attack());
                    else StartCoroutine(Reload());
                }
            }
        }
    }

    private void ProcessMineUseInput()
    {
        if (itemUseInput && CountMineNumber > 0)
        {
            if (GameManager != null)
            {
                GameManager.SpawnItemAt(transform.position, MinePrefab);
                CountMineNumber = CountMineNumber - 1 >= 0 ? CountMineNumber - 1 : 0;
                GameManager.UpdateMinesCounter(CountMineNumber);
            }
        }
    }

    IEnumerator Attack()
    {
        allowAttack = false;
        if (currentAmmo >= AmountOfBullets)
        {
            WeaponEquipped.SpawnBullet(bulletSpawnPoint.transform, AmountOfBullets, 30);
            currentAmmo -= AmountOfBullets;
        }
        else
        {
            WeaponEquipped.SpawnBullet(bulletSpawnPoint.transform, currentAmmo, 30);
            currentAmmo = 0;
        }
        if (GameManager != null) GameManager.UpdateBulletsCounter(currentAmmo, WeaponEquipped.MaxAmmo);
        yield return new WaitForSeconds(WeaponEquipped.AttackRate);
        allowAttack = true;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        GameManager.UpdateReloadSlider(true, 0, WeaponEquipped.ReloadTime);
        Debug.Log("Player is reloading");
        yield return new WaitForSeconds(WeaponEquipped.ReloadTime);
        currentAmmo = WeaponEquipped.MaxAmmo;
        GameManager.UpdateBulletsCounter(currentAmmo, WeaponEquipped.MaxAmmo);
        timeReloading = 0;
        isReloading = false;
        GameManager.UpdateReloadSlider(false, 0, WeaponEquipped.ReloadTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MinePickUp"))
        {
            CountMineNumber++;
            GameManager.UpdateMinesCounter(CountMineNumber);
            Destroy(other.gameObject);
        }
    }

    public void SetMineNumber(int waveNumber)
    {
        if (waveNumber > 0 && waveNumber < countMineWaves.Length)
        {
            CountMineNumber = countMineWaves[waveNumber];
            GameManager.UpdateMinesCounter(CountMineNumber);
        }
    }

    public void SetActiveMine()
    {
        if (CountMineNumber > 0)
        {
            ImageE_Active.SetActive(true);
            ImageE_NoActive.SetActive(false);
        }
        else
        {
            ImageE_Active.SetActive(false);
            ImageE_NoActive.SetActive(true);
        }
    }

    private void OnEnable()
    {
        GameManager.UpdateBulletsCounter(currentAmmo, WeaponEquipped.MaxAmmo);
        GameManager.UpdateReloadSlider(false, 0, WeaponEquipped.ReloadTime);
        GameManager.UpdateMinesCounter(CountMineNumber);
    }

    private void OnDisable()
    {
        currentAmmo = WeaponEquipped.MaxAmmo;
        AmountOfBullets = 1;
        isReloading = false;
        timeReloading = 0.0f;
        allowAttack = true;
    }
}
