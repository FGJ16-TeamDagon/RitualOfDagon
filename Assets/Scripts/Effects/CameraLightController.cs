using UnityEngine;
using System.Collections;

public class CameraLightController : MonoBehaviour 
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float height;

    private float lightRange = 6;
    private float lightIntensity = 4;
    private float ambientIntensity = 1f;

    private Transform followTarget;

    private Transform follower;

    private Light targetLight;

    private void Start()
    {
        follower = transform;
        followTarget = Camera.main.transform;

        targetLight = GetComponent<Light>();

        targetLight.intensity = 0;
        targetLight.range = 0;
        RenderSettings.ambientIntensity = 0;
    }

    void Update()
    {
        targetLight.intensity = Mathf.MoveTowards(targetLight.intensity, lightIntensity, Time.deltaTime * 2);
        targetLight.range = Mathf.MoveTowards(targetLight.range, lightRange, Time.deltaTime * 2);
        RenderSettings.ambientIntensity = Mathf.MoveTowards(RenderSettings.ambientIntensity, ambientIntensity, Time.deltaTime * 2);
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
