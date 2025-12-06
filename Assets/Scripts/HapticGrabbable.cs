using UnityEngine;
using System.Collections;

public class HapticGrabbable : OVRGrabbable
{
    public float intensity = 1.0f;

    public bool isGrabbledNow = false;

    float t = 0f;
    float timer = 0.5f;

    [SerializeField] GameObject deliveryEffect;

        public Transform mySpawnPoint;

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

            PlayerStat.instance.TakeDamage(intensity);
        }
        else
        {
            t -= Time.deltaTime;
        }
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        if (grabPoint == null || hand == null)
            return;

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
        if(other.CompareTag("DeliveryArea") && !isGrabbledNow)
        {
            PlayerStat.instance.AddHealth(intensity * 15);
            
            SpawnBox.instance.SetSpawnPointFree(mySpawnPoint, false);
            SpawnBox.instance.UpdateSpawnBox();
            GameManager.instance.AddBoxDelivered();
            StartCoroutine(DestroyAfterRelease());
        }
    }

    IEnumerator DestroyAfterRelease()
    {
        if (m_grabbedBy != null)
        {
            m_grabbedBy.ForceRelease(this);
        }

        gameObject.SetActive(false);

        yield return null;

        Destroy(gameObject);
    }
}
