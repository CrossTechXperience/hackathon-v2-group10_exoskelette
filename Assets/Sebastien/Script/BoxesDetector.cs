using UnityEngine;

public class BoxesDetector : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Box"))
        {
            SpawnBox.instance.SetSpawnPointFree(other.transform.parent, false);
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Box"))
        {
            SpawnBox.instance.SetSpawnPointFree(other.transform.parent, true);
        }
    }
}
