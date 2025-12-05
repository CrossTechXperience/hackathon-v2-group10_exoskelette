using UnityEngine;

public class VRMovement : MonoBehaviour
{
    public float speed = 3.0f;
    public Transform head; // CenterEyeAnchor

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (head == null)
            head = GameObject.Find("CenterEyeAnchor").transform;
    }

    void Update()
    {
        // Joystick Gauche
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Direction basée sur l’orientation du casque
        Vector3 move = head.forward * input.y + head.right * input.x;
        move.y = 0;  // pas de mouvement vertical

        controller.Move(move * speed * Time.deltaTime);
    }
}
