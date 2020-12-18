using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator anim;
    public float waitTransition = 0.5f;
    AudioScript audioScript = new AudioScript();
    public AudioSource audioSource;

    public IEnumerator LoadNextScene(string nextSceneName)
    {
        anim.SetTrigger("End");
        audioScript.FadeOut(audioSource, 0.3f);
        yield return new WaitForSeconds(waitTransition);
        SceneManager.LoadScene(nextSceneName);
    }
}
