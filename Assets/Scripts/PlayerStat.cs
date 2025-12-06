using UnityEngine;
using UnityEngine.UI;
using Bhaptics.SDK2;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    [Header("Health")]
    public float health = 100.0f;
    public float maxHealth = 100.0f;
    bool isDead = false;

    [Header("Gilet")]
    public bool hadGilet = false;
    public float giletDiminutionFactor = 0.25f;
    public float curStamina = 100.0f;
    public float maxStamina = 100.0f;

    public float maxBattery = 100.0f;
    public float curBattery = 100.0f;

    [Header("UI Elements")]
    [SerializeField] private Image healthImage;
    [SerializeField] private Image staminaBar;
    [SerializeField] private Image batteryBar;

    [SerializeField] private Image shiedIcon;
    public bool grabbing;

    private void Awake() {
        instance = this;
    }

    void Update()
    {
        healthImage.fillAmount = health / maxHealth;
        staminaBar.fillAmount = curStamina / maxStamina;
        batteryBar.fillAmount = curBattery / maxBattery;

        healthImage.color = hadGilet ? Color.gray : Color.red;
        shiedIcon.gameObject.SetActive(hadGilet);

        OVRPlayerController pc = GetComponent<OVRPlayerController>();
        pc.EnableLinearMovement = curStamina > 0.0f;

        Debug.Log(curStamina > 0.0f);

        if(!grabbing && curStamina < maxStamina)
        {
            curStamina += Time.deltaTime * 10.0f;
            if(curStamina > maxStamina)
            {
                curStamina = maxStamina;
            }
        }

        if(hadGilet)
        {
            curBattery -= Time.deltaTime * 5.0f;
            if(curBattery < 0.0f)
            {
                curBattery = 0.0f;
                hadGilet = false;
            }
        }
        /*else
        {
            curBattery += Time.deltaTime * 5.0f;
            if(curBattery > maxBattery)
            {
                curBattery = maxBattery;
            }
        }*/

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            SetGilet();
        }
    }

    public void AddHealth(float amount)
    {
        if (isDead)
            return;

        health += amount;
        if (health > maxHealth)
            health = maxHealth;

        Debug.Log("Player healed: " + amount + ", current health: " + health);
    }

    public void TakeDamage(float damage)
    {
        if(hadGilet)
        {
            Debug.Log("Gilet absorbed some damage." + (damage *= giletDiminutionFactor));
            damage *= giletDiminutionFactor;
        }

        health -= damage;
        curStamina -= damage * 10.0f;

        Debug.Log("Player took damage: " + damage + ", current health: " + health);


        BhapticsLibrary.Play("douleur", 0, damage);

        if (health < 0.0f)
        {
            isDead = true;
            Debug.Log("Player is dead.");
        }
    }

    public void SetGilet()
    {
        hadGilet = !hadGilet;
        Debug.Log("Player gilet status: " + hadGilet);
    }

    public void ResetStat()
    {
        health += maxHealth * 0.25f;
        curStamina = maxStamina;
        curBattery = maxBattery;
        hadGilet = false;
    }
}
