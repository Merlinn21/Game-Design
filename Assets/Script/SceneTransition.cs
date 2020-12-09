using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator anim;
    public float waitTransition = 0.5f;
    public IEnumerator LoadNextScene(string nextSceneName)
    {
        anim.SetTrigger("End");
        yield return new WaitForSeconds(waitTransition);
        SceneManager.LoadScene(nextSceneName);
    }
}
