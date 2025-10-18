using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public GameObject titleScreenCanvas;
    public GameObject splashScreenCanvas;
    public float displayTime = 5f;

    private void Start()
    {
        StartCoroutine(DisplaySplashScreen());
    }

    private IEnumerator DisplaySplashScreen()
    {
       
        splashScreenCanvas.SetActive(true);
        titleScreenCanvas.SetActive(false);
        yield return new WaitForSeconds(displayTime);
        splashScreenCanvas.SetActive(false);
        titleScreenCanvas.SetActive(true);
       
    }

}
