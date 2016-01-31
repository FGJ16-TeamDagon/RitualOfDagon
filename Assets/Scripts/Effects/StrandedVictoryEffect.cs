using UnityEngine;
using System.Collections;

public class StrandedVictoryEffect : MonoBehaviour 
{
    [SerializeField]
    private LineRenderer lineRenderer;

    private Vector3[] lineVertexPositions;

    public void Start()
    {
        transform.position = GamePlay.Instance.StrandedPlayer.characters[0].transform.position;

        gameObject.SetActive(true);

        lineVertexPositions = new Vector3[GamePlay.Instance.Ritual.pattern.Length];

        lineRenderer.SetVertexCount(lineVertexPositions.Length);

        StartCoroutine(PlayEffect_Coroutine());
    }

    IEnumerator PlayEffect_Coroutine()
    {
        for (int i = 0; i < 10; i++)
        {
            int count = 0;
            Vector3 offset = new Vector3(Random.Range(-5, 4), Random.Range(0, 2), Random.Range(-5, 4));
            foreach (var position in GamePlay.Instance.Ritual.pattern)
            {
                lineVertexPositions[count] = offset + new Vector3(position.X, 0, position.Z) + Vector3.up * 0.5f;

                count++;
            }

            lineRenderer.SetPositions(lineVertexPositions);

            yield return new WaitForSeconds(0.5f);
        }
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (lineVertexPositions != null)
        {
            for (int i = 0; i < lineVertexPositions.Length; i++)
            {
                lineRenderer.SetPosition(i, lineVertexPositions[i] + new Vector3(
                    UnityEngine.Random.value * 0.6f - 0.03f,
                    UnityEngine.Random.value * 0.3f,
                    UnityEngine.Random.value * 0.6f - 0.03f
                    ));
            }
        }
    }
}
