using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip startButtonClip;
    public AudioClip exitButtonClip;

    public void OnStartButtonClick()
    {
        PlayAudio(startButtonClip);
        SceneManager.LoadScene("Puzzle1");
    }

    public void OnExitButtonClick()
    {
        PlayAudio(exitButtonClip);
        Application.Quit();
    }

    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
