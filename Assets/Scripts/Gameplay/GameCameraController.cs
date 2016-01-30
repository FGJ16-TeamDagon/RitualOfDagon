using UnityEngine;
using System.Collections;

public class GameCameraController : MonoBehaviour 
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform mover;
    [SerializeField]
    private float damping = 2;

    private Vector3 lastMousePos;

    private Vector3 momentum;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - lastMousePos;

            delta /= Screen.height;

            mover.position -= Vector3.forward * delta.y * speed + Vector3.right * delta.x * speed;

            momentum = delta;
        }
        else if (momentum.sqrMagnitude > 0)
        {
            mover.position -= Vector3.forward * momentum.y * speed + Vector3.right * momentum.x * speed;

            momentum = Vector3.MoveTowards(momentum, Vector3.zero, Time.deltaTime * damping);
        }

        lastMousePos = Input.mousePosition;
    }
}
