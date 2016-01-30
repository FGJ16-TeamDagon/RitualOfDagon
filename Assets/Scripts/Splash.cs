using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{
    private float splashTime = 0;

	void Update ()
    {
	    if (Time.time > splashTime)
        {
            AppManager.Instance.GoToStartMenu();
        }
	}
}
