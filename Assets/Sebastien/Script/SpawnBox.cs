using System.Threading;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    public GameObject BoxSmallPrefab;
    public GameObject BoxMediumPrefab;
    public GameObject BoxLargePrefab;
    public GameObject BoxExtraLargePrefab;
    public GameObject SpawnPoint;
    public GameObject DestroyPoint;

    private GameObject BoxActual;
    private int count = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 0)
        {
            SpawnBoxRandom();
        }
        else
        {
            if (count == 5)
            {
                Destroy(BoxActual);
                count = 0;
            }
            else
            {
                count++;
            }
        }
    }

    void SpawnBoxRandom()
    {
        GameObject[] Box = new GameObject[] { BoxSmallPrefab, BoxMediumPrefab, BoxLargePrefab, BoxExtraLargePrefab };
        int randomIndex = Random.Range(0, Box.Length);
        BoxActual = Box[randomIndex];
        Instantiate(BoxActual, SpawnPoint.transform.position, Quaternion.identity);

    }
}
