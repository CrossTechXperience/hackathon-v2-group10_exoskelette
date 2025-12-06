using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    public static SpawnBox instance;

    [SerializeField] private GameObject[] boxesPrefabs;
    public Dictionary<Transform, bool> spawnPoints = new Dictionary<Transform, bool>();

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

    // 1. On parcourt le dictionnaire SANS le modifier
    foreach (var entry in spawnPoints)
    {
        Transform point = entry.Key;

        // Vérification sécurité
        if (point == null)
            continue;

        if (!entry.Value) // Si libre
        {
            pointsToSpawn.Add(point); // On l'ajoute à la liste
        }
    }

    // 2. On instancie APRÈS la boucle foreach
    foreach (Transform point in pointsToSpawn)
    {
        int randomBoxIndex = Random.Range(0, boxesPrefabs.Length);

        GameObject box = Instantiate(
            boxesPrefabs[randomBoxIndex],
            point.position + Vector3.up * 2f,
            point.rotation
        );

        // Marquer comme occupé
        spawnPoints[point] = true;

        // Ajouter le point d’origine dans la box
        HapticGrabbable hg = box.GetComponent<HapticGrabbable>();
        if (hg != null)
            hg.mySpawnPoint = point;
    }
}

}
