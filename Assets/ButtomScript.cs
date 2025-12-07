using UnityEngine;

public class ButtomScript : MonoBehaviour
{
    public GameObject prefab;

    public void DesactiveInformation()
    {
        prefab.SetActive(false);
    }
}