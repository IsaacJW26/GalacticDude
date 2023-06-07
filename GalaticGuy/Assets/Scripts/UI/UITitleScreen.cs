using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITitleScreen : MonoBehaviour
{
    // Start is called before the first frame update

    private bool sceneTransitionStarted = false;
    private float timeStarted = 0f;

    [SerializeField]
    private float animationLength = 3f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown && !sceneTransitionStarted)
        {
            sceneTransitionStarted = true;
            timeStarted = Time.time;
        }
        
        // start scene after delay
        if (sceneTransitionStarted && timeStarted + animationLength <= Time.time)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
