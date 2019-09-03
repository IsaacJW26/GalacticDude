using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySpawner))]
public class GameManager : MonoBehaviour
{
    public static GameManager INST = null;
    EnemySpawner spawner;

    // Start is called before the first frame update
    void Awake()
    {
        if (INST == null)
            INST = this;
        else
            Destroy(this);
        DontDestroyOnLoad(gameObject);

        spawner

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
