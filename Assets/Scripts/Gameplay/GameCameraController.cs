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
    [SerializeField]
    private float targetHeight = 7;

    private float maxMovement = 7;

    private Vector3 lastMousePos;

    private Vector3 momentum;

    void Update()
    {
        var moverPos = mover.position;

        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - lastMousePos;

            delta /= Screen.height;

            moverPos -= Vector3.forward * delta.y * speed + Vector3.right * delta.x * speed;

            momentum = delta;
        }
        else if (momentum.sqrMagnitude > 0)
        {
            moverPos -= Vector3.forward * momentum.y * speed + Vector3.right * momentum.x * speed;

            momentum = Vector3.MoveTowards(momentum, Vector3.zero, Time.deltaTime * damping);
        }

        moverPos.x = Mathf.Clamp(moverPos.x, -maxMovement, maxMovement);
        moverPos.y = Mathf.MoveTowards(moverPos.y, targetHeight, Time.deltaTime);
        moverPos.z = Mathf.Clamp(moverPos.z, -maxMovement, maxMovement);

        mover.position = moverPos;

        lastMousePos = Input.mousePosition;
    }
}
