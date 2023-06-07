using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrate : MonoBehaviour
{
    [SerializeField]
    private bool isVibrating = false;
    [SerializeField]
    private float vibratingIntensity = 1f;
    [SerializeField]
    [Min(0.01f)]
    private float vibratingFrequency = 10f;
    [SerializeField]
    [Range(0f,1f)]
    private float vibrationIntensityVariance = 0.5f;
    private Vector3 startPosition;
    private Vector3 lastPosition;
    private float timeTillNextPosition = 0f;
    private Vector3 nextPosition;

    private List<Vector3> positions = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isVibrating)
        {
            if (timeTillNextPosition <= 0f)
            {
                timeTillNextPosition = 1f / vibratingFrequency;
                lastPosition = transform.localPosition;
                float angle = Random.Range(0, 2 * Mathf.PI);
                float xpos = Mathf.Cos(angle);
                float ypos = Mathf.Sin(angle);
                float radius = Random.Range(vibrationIntensityVariance * vibratingIntensity, vibratingIntensity) / 100f;
                nextPosition = startPosition + new Vector3(xpos, ypos) * radius;
                // positions.Add(nextPosition);
            }
            else
            {
                float percentRemaining = 1f - (timeTillNextPosition * vibratingFrequency);
                transform.localPosition = Vector3.Lerp(lastPosition, nextPosition, percentRemaining);
            }
            timeTillNextPosition -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = startPosition;
        }
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawSphere(startPosition, 0.05f);
    //     Gizmos.color = Color.red;

    //     foreach (Vector3 pos in positions)
    //     {
    //         Gizmos.DrawSphere(pos, 0.05f);
    //     }
    // }
}
