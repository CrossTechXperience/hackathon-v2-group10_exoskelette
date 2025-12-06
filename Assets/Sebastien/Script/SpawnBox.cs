using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    public static SpawnBox instance;

    [SerializeField] private GameObject[] boxesPrefabs;
    public Dictionary<Transform, bool> spawnPoints = new Dictionary<Transform, bool>();

    public OVRGrabber rightHand;
    public OVRGrabber leftHand;

    void Awake()
    {
        // Sécurité si plusieurs instances existent
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        // On récupère tous les enfants comme spawnpoints
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform point = transform.GetChild(i);
            spawnPoints.Add(point, false);
        }

        UpdateSpawnBox();
    }

    /// <summary>
    /// Change l’état d’un point de spawn.
    /// </summary>
    public void SetSpawnPointFree(Transform point, bool isFree)
    {
        if (point == null) return;

        if (spawnPoints.ContainsKey(point))
        {
            spawnPoints[point] = isFree;
        }
    }

    /// <summary>
    /// Check chaque point libre et spawn une box.
    /// </summary>
public void UpdateSpawnBox()
{
    List<Transform> pointsToSpawn = new List<Transform>();

    foreach (var entry in spawnPoints)
    {
        Transform point = entry.Key;

        if (point == null)
            continue;

        if (!entry.Value)
        {
            pointsToSpawn.Add(point);
        }
    }

    foreach (Transform point in pointsToSpawn)
    {
        int randomBoxIndex = Random.Range(0, boxesPrefabs.Length);

        GameObject box = Instantiate(
            boxesPrefabs[randomBoxIndex],
            point.position + Vector3.up * 2f,
            point.rotation
        );

        spawnPoints[point] = true;

        HapticGrabbable hg = box.GetComponent<HapticGrabbable>();
        if (hg != null)
            {
                hg.mySpawnPoint = point;
            }
    }
}

}
