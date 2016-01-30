using UnityEngine;
using System.Collections;

public class CameraLightController : MonoBehaviour 
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float height;

    private Transform followTarget;

    private Transform follower;

    private void Start()
    {
        follower = transform;
        followTarget = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (followTarget)
        {
            var targetPos = followTarget.position;
            targetPos.y = height;

            //follower.position = Vector3.MoveTowards(follower.position, targetPos, Time.deltaTime * speed);
            follower.position = Vector3.Lerp(follower.position, targetPos, Time.deltaTime * speed);
        }
    }
}
