using UnityEngine;
using Bhaptics.SDK2;

public class HapticGrabbable : OVRGrabbable
{
    public float intensity = 1.0f;

    public bool isGrabbledNow = false;

    float t = 0f;
    float timer = 0.5f;

    [SerializeField] GameObject deliveryEffect;

    void Start()
    {
        t = timer;
    }

    void Update()
    {
        if(!isGrabbledNow)
            return;

        if (t < 0)
        {
            t = timer;

            BhapticsLibrary.Play("douleur", 0, intensity);
            PlayerStat.instance.TakeDamage(intensity);
        }
        else
        {
            t -= Time.deltaTime;
        }
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        isGrabbledNow = true;
        PlayerStat.instance.grabbing = true;
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
        isGrabbledNow = false;
        PlayerStat.instance.grabbing = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DeliveryArea"))
        {
            PlayerStat.instance.AddHealth(intensity * 15);

            Destroy(gameObject);
        }
    }
}
