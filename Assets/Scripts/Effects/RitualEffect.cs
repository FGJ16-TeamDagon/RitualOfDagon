using UnityEngine;
using System.Collections;
using System;

public class RitualEffect : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    private Vector3[] lineVertexPositions;

    public void StartEffect(Ritual ritual)
    {
        gameObject.SetActive(true);

        lineVertexPositions = new Vector3[GamePlay.Instance.DeepOnesPlayer.characters.Count];
        
        lineRenderer.SetVertexCount(lineVertexPositions.Length);

        int i = 0;
        foreach (var deepOne in GamePlay.Instance.DeepOnesPlayer.characters)
        {
            lineVertexPositions[i] = deepOne.transform.position + Vector3.up * 0.5f;
            i++;
        }

        lineRenderer.SetPositions(lineVertexPositions);
    }

    void Update()
    {
        if (lineVertexPositions != null)
        {
            for (int i = 0; i < lineVertexPositions.Length; i++)
            {
                lineRenderer.SetPosition(i, lineVertexPositions[i] + new Vector3(
                    UnityEngine.Random.value * 0.01f - 0.005f,
                    UnityEngine.Random.value * 0.01f,
                    UnityEngine.Random.value * 0.01f - 0.005f
                    ));
            }
        }
    }
}
