using UnityEngine;
using System.Collections;

public class CameraLightController : MonoBehaviour 
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float height;

    private float lightRange = 7;
    private float lightIntensity = 4;
    private float ambientIntensity = 0.05f;
    private Color ambientColor = Color.white;
    LTDescr ambientColorTween;

    private Transform followTarget;

    private Transform follower;

    private Light targetLight;

    float SM2Factor = 1;

    private void Start()
    {
        follower = transform;
        followTarget = Camera.main.transform;

        targetLight = GetComponent<Light>();

        targetLight.intensity = 0;
        targetLight.range = 0;
        RenderSettings.ambientIntensity = 0;

        Debug.Log("SM " + SystemInfo.graphicsShaderLevel);
        if (SystemInfo.graphicsShaderLevel <= 20)
        {
            SM2Factor = 2.5f;
        }
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
        else if (GamePlay.Instance.State == GamePlay.GameplayState.GameOver)
        {
            if (GamePlay.Instance.Winner == GamePlay.Instance.StrandedPlayer)
            {
                lightIntensity = 5;
                lightRange = 40;
                if (ambientColorTween != null) LeanTween.cancel(ambientColorTween.id);

                ambientColorTween = LeanTween.value(gameObject, (v) => {
                    RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Color.white, v);
                }, 0, 1, Time.deltaTime * 5f);

                ambientIntensity = 1;
            }
            else 
            {
                lightIntensity = 0.2f;
                lightRange = 4.5f;
                if (ambientColorTween != null) LeanTween.cancel(ambientColorTween.id);

                ambientColorTween = LeanTween.value(gameObject, (v) => {
                    RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, Color.red, v);
                }, 0, 1, Time.deltaTime * 4f);

                ambientIntensity = 0.2f;
            }
        }
    }

    void Update()
    {
        targetLight.intensity = Mathf.MoveTowards(targetLight.intensity, lightIntensity, Time.deltaTime * 2);
        targetLight.range = Mathf.MoveTowards(targetLight.range, lightRange, Time.deltaTime * 2);
        RenderSettings.ambientIntensity = Mathf.MoveTowards(RenderSettings.ambientIntensity, ambientIntensity * SM2Factor, Time.deltaTime * 0.25f);
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
