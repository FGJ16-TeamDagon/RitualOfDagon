using UnityEngine;
using System.Collections;
using System;

public class RitualEffect : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private GameObject deepOneParticlePrefab;

    private Vector3[] lineVertexPositions;

    public void Start()
    {
        transform.position = GamePlay.Instance.DeepOnesPlayer.characters[0].transform.position;

        gameObject.SetActive(true);

        lineVertexPositions = new Vector3[GamePlay.Instance.DeepOnesPlayer.characters.Count];
        
        lineRenderer.SetVertexCount(lineVertexPositions.Length);

        int count = 0;
        foreach (var deepOne in GamePlay.Instance.DeepOnesPlayer.characters)
        {
            lineVertexPositions[count] = deepOne.transform.position + Vector3.up * 0.5f;

            var particleGo = Instantiate(deepOneParticlePrefab);
            particleGo.transform.position = deepOne.transform.position;
            count++;
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
                    UnityEngine.Random.value * 0.2f - 0.1f,
                    UnityEngine.Random.value * 0.2f,
                    UnityEngine.Random.value * 0.2f - 0.1f
                    ));
            }
        }
    }
}
