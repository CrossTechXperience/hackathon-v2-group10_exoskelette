using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    [SerializeField] private float delay = 1f;
    void Start()
    {
        Destroy(gameObject, delay);
    }

}
