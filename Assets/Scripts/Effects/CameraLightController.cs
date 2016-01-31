using UnityEngine;
using System.Collections;

public class CameraLightController : MonoBehaviour 
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float height;

    private float lightRange = 5;
    private float lightIntensity = 4;
    private float ambientIntensity = 0.05f;
    private Color ambientColor = Color.white;
    LTDescr ambientColorTween;

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

    void OnEnable()
    {
        GamePlay.GamestateChanged += GamePlay_GamestateChanged;
    }

    void OnDisable()
    {
        GamePlay.GamestateChanged -= GamePlay_GamestateChanged;
    }

    private void GamePlay_GamestateChanged()
    {
        if (GamePlay.Instance.State == GamePlay.GameplayState.Playing)
        {
            if (GamePlay.Instance.CurrentPlayer == GamePlay.Instance.DeepOnesPlayer)
            {
                ambientColor = Player.DeepOneColorLight;
            }
            else
            {
                ambientColor = Player.StrandedColorLight;
            }

            if (ambientColorTween != null) LeanTween.cancel(ambientColorTween.id);

            ambientColorTween = LeanTween.value(gameObject, (v) => {
                RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, ambientColor, v);
            }, 0, 1, Time.deltaTime * 3f);
        }
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
