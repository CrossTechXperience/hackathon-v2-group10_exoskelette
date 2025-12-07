using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int day = 0;
    public float timerDay = 300.0f;
    float t;

    public int boxHasToDelivery = 10;
    public int boxDelivered = 0;

    [Header("End of Day")]
    bool endOfDay = false;
    public GameObject endOfDayUI;

    [Header("UI")]
    public Text dayText;
    public Text timerText;
    public Text boxHasToDeliveryText;
    public Text boxDeliveredText;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        t = timerDay;
    }

    void Update()
    {
        endOfDayUI.SetActive(endOfDay);

        if(endOfDay)
        {
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                endOfDay = false;
                endOfDayUI.SetActive(false);
                NextDay();
            }
            return;
        }
        else
        {
            if(t < 0)
            {
                EndOfDay();
            }
            else
            {
                t -= Time.deltaTime;
            }
        }

        UpdateText();
    }

    public void UpdateText()
    {
        dayText.text = "Day : " + day;
        timerText.text = "Time Left : " + Mathf.FloorToInt(t) + "s";
        boxHasToDeliveryText.text = boxHasToDelivery.ToString();
        boxDeliveredText.text = boxDelivered.ToString() +" / ";
    }

    public void AddBoxDelivered()
    {
        boxDelivered++;


        if(boxDelivered == boxHasToDelivery)
        {
            EndOfDay();
        }
    }

    void EndOfDay()
    {
        PlayerStat.instance.GetComponent<OVRPlayerController>().enabled = false;
        PlayerStat.instance.enabled = false;
        endOfDay = true;
    }

    void NextDay()
    {
        day++;
        boxDelivered = 0;
        Debug.Log("Day " + day + " started!");
        PlayerStat.instance.GetComponent<OVRPlayerController>().enabled = true;
        PlayerStat.instance.enabled = true;
        PlayerStat.instance.ResetStat();
        t = timerDay;
        endOfDay = false;
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
    }
}
