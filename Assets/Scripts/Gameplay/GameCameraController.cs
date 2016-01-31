using UnityEngine;
using System.Collections;

public class GameCameraController : MonoBehaviour 
{
    private float speed = 4;

    private Transform mover;
    
    private float damping = 2;
    
    private float targetHeight = 7;

    private float maxMovement = 7;

    private Vector3 lastMousePos;

    private Vector3 momentum;

    private Transform shakeRoot;

    private float maxShakeAmount = 0.2f;
    private float shakeAmount = 0;
    private float shakeReduction = 0.25f;

    void Start()
    {
        var rootGO = new GameObject("Game Camera Rig");
        rootGO.transform.position = transform.position;
        rootGO.transform.rotation = transform.rotation;

        transform.SetParent(rootGO.transform);

        shakeRoot = transform;
        mover = rootGO.transform;
    }

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

        shakeAmount = Mathf.MoveTowards(shakeAmount, 0, Time.deltaTime * shakeReduction);

        if (shakeAmount > 0)
        {
            shakeRoot.localPosition = new Vector3(
                Random.value * shakeAmount - shakeAmount * 0.5f,
                0,
                Random.value * shakeAmount - shakeAmount * 0.5f
                );
        }
        else
        {
            shakeRoot.localPosition = Vector3.zero;
        }
        lastMousePos = Input.mousePosition;
    }

    public void Shake()
    {
        shakeAmount = maxShakeAmount;
    }
}
