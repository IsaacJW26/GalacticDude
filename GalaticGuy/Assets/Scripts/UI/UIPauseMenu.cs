using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPauseMenu : MonoBehaviour
{
    GameObject[] pauseObjects;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Find another way to do this - time scale can be changed without pausing (i.e. charged beam)
            if(Time.timeScale == 1)
            {
                StartPause();
            } else if(Time.timeScale == 0)
            {
                StopPause();
            }
        }
    }

    // Reloads the level
    public void Reload()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    // Shows objects with the ShowOnPause tag
    public void showPaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(true);
            if(g.name == "PlayButton")
            {
                g.GetComponent<Selectable>().Select();
            }
        }
    }

    // Hides objects with ShowOnPause tag
    public void hidePaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    // Loads inputted level
    public void LoadLevel(string level)
    {
        Application.LoadLevel(level);
    }

    public void StartPause()
    {
        Time.timeScale = 0;
        showPaused();
    }

    public void StopPause()
    {
        Time.timeScale = 1;
        hidePaused();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void MainMenu()
    {
        Application.LoadLevel(0);
    }

}
