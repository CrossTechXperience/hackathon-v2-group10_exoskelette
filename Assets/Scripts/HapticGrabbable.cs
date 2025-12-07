using UnityEngine;
using System.Collections;

public class HapticGrabbable : OVRGrabbable
{
    public float intensity = 1.0f;

    public bool isGrabbledNow = false;

    float t = 0f;
    float timer = 0.5f;

    public bool cantLeveWithoutArmor = false;

    [SerializeField] GameObject deliveryEffect;

    public Transform mySpawnPoint;

    void Start()
    {
        t = timer;
        gameObject.GetComponent<Outline>().enabled = false;
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
        if (m_grabbedBy != null)
            return;

        if (grabPoint == null || hand == null)
            return;

        bool canGrab = true;
        if(cantLeveWithoutArmor && !PlayerStat.instance.hadGilet)
        {
            canGrab = false;
            PlayerStat.instance.grabbing = true;
        }

        base.GrabBegin(hand, grabPoint);

        if (!canGrab)
        {
            m_grabbedBy.ForceRelease(this);
            return;
        }

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
            SpawnBox.instance.SetSpawnPointFree(mySpawnPoint, false);
            SpawnBox.instance.UpdateSpawnBox();
            GameManager.instance.AddBoxDelivered();
            StartCoroutine(DestroyAfterRelease());
        }

        if(other.gameObject.GetComponent<OVRGrabber>() != null)
        {
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    public void OnTriggerExit (Collider other) 
    {
        if(other.gameObject.GetComponent<OVRGrabber>() != null)
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }   
    }

IEnumerator DestroyAfterRelease()
{
    if (m_grabbedBy != null)
    {
        m_grabbedBy.ForceRelease(this);
        m_grabbedBy = null; 
    }

    gameObject.SetActive(false);

    yield return null;

    //Destroy(gameObject);
}


}
