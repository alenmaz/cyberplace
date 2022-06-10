using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Character stats")]
    [SerializeField] private int MaxHealth;
    [SerializeField] private int MaxShields;

    public int CurrentHealth;
    public int CurrentShields;

    [Header("Dependencies")]
    public string TeamID;
    [SerializeField] private Bar HealthBar;
    [SerializeField] private Bar ShieldBar;
    [SerializeField] private ObjectPooler ObjectPooler;
    [SerializeField] private GameManager GameManager;

    [Header("Bools")]
    public bool CanBeDamaged = true;

    [Header("Effects")]
    [SerializeField] private GameObject[] effectsOnDeath;

    void Start()
    {
        ObjectPooler = GameObject.FindWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        GameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        Reset();

        if (HealthBar is null) Debug.LogWarning("There's no healthbar attached");
        if (ShieldBar is null) Debug.LogWarning("There's no shieldbar attached");
    }

    void Update()
    {
        if(CurrentHealth == 0) Die();
    }

    public void UpdateShields(int amount)
    {
        CurrentShields = Mathf.Clamp(CurrentShields + amount, 0, MaxShields);
        if(ShieldBar != null) ShieldBar.SetValue(CurrentShields);
    }

    ///<summary>
    ///Метод увеличивает или уменьшает текущее количество здоровье на amount единиц в пределах от 0 до максимального здоровья
    ///</summary>
    ///<param name="amount">На сколько единиц увеличивать щиты</param>
    public void UpdateHealth(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        if(HealthBar != null) HealthBar.SetValue(CurrentHealth);
    }

    ///<summary>
    ///Метод перманентно увеличивает максимальные щиты игрока на amount
    ///</summary>
    ///<param name="amount">На сколько единиц увеличивать щиты</param>
    ///<returns>Значение MaxShields до изменений</returns>
    public int IncreaseMaxShields(int amount)
    {
        var temp = MaxShields;
        MaxShields += amount;
        if (ShieldBar != null)
        {
            ShieldBar.SetMaxValue(MaxShields);
            ShieldBar.SetValue(CurrentShields);
        }
        return temp;
    }

    ///<summary>
    ///Метод перманентно увеличивает максимальное здоровье игрока на amount
    ///</summary>
    ///<param name="amount">На сколько единиц увеличивать здоровье</param>
    ///<returns>Значение Maxhealth до изменений</returns>
    public int IncreaseMaxHealth(int amount)
    {
        var temp = MaxHealth;
        MaxHealth += amount;
        if(HealthBar != null)
        {
            HealthBar.SetMaxValue(MaxHealth);
            HealthBar.SetValue(CurrentHealth);
        }
        return temp;
    }

    /// <summary>
    /// Метод наносит определенное количество урона щитам, если они больше 0, или здоровью, если щиты равны 0
    /// </summary>
    /// <param name="amount">Количество единиц урона</param>
    /// <param name="type">Тип урона</param>
    public void TakeDamage(int amount, DamageType type) 
    {
       switch (type) {
           case DamageType.Acid:
                break;
           case DamageType.Fire:
                if (CanBeDamaged)
                {
                    if (CurrentShields > 0) UpdateShields(-amount);
                    else UpdateHealth(-amount);
                    break;
                }
                break;
           case DamageType.Azot:
                break;
           case DamageType.None:
                if(CanBeDamaged)
                {
                    if (CurrentShields > 0) UpdateShields(-amount);
                    else UpdateHealth(-amount);
                    break;
                }
                break;
           default:
               break;
       }
    }

    public void Reset()
    {
        CurrentHealth = MaxHealth;
        CurrentShields = MaxShields;

        if (HealthBar != null) HealthBar.SetMaxValue(MaxHealth);
        if (ShieldBar != null) ShieldBar.SetMaxValue(MaxShields);
    }

    private void Die() 
    {
        if (this.gameObject.tag == "Enemy")
        {
            if (GameManager != null) 
                foreach (var effect in effectsOnDeath) 
                    GameManager.SpawnItemAt(transform.position, effect);
            if (ObjectPooler != null) ObjectPooler.ReturnInstance(gameObject);
            else Debug.Log("Object pooler is not assigned");           
        }
        if (this.gameObject.tag == "Player" && GameManager != null)
        {
            GameManager.isPlayerDead = true; 
            gameObject.SetActive(false);
        }
        else gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Reset();
        if (this.gameObject.tag == "Player" && GameManager != null) GameManager.isPlayerDead = false;
    }
}
