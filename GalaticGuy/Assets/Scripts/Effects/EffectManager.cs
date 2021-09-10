using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScreenShake))]
public class EffectManager : MonoBehaviour
{
    public static EffectManager INSTANCE = null;
    ScreenShake shake;
    IEnumerator slowFunction;
    [SerializeField]
    GameObject Explosion = null;
    [SerializeField]
    GameObject ProjectileHit = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (INSTANCE == null)
            INSTANCE = this;
        else
            Destroy(this);

        shake = GetComponent<ScreenShake>();
    }

    public void ScreenShakeBig()
    {
        shake.BigShake();
    }

    public void ScreenShakeMedium()
    {
        shake.MediumShake();
    }

    public void ScreenShakeSmall()
    {
        shake.SmallShake();
    }

    public void SlowLong()
    {
        if(slowFunction != null)
            StopCoroutine(slowFunction);
        slowFunction = SlowTime(1.0f, 0.3f);
        StartCoroutine(slowFunction);
    }

    public void SlowShort()
    {
        if (slowFunction != null)
            StopCoroutine(slowFunction);
        slowFunction = SlowTime(2f, 0.8f);
        StartCoroutine(slowFunction);
    }

    private IEnumerator SlowTime(float unscaledDuration, float percent)
    {
        Time.timeScale = percent;
        yield return new WaitForSecondsRealtime(unscaledDuration);
        Time.timeScale = 1f;
    }

    public void CreateExplosion(Vector3 position, float scale)
    {
        GameObject obj = Instantiate(Explosion);
        obj.transform.localScale *= scale;
        obj.transform.position = position;
        StartCoroutine(RemoveEffect(obj, 5f));
    }

    public void CreateProjectileHit(Vector3 position)
    {
        GameObject obj = Instantiate(ProjectileHit);
        obj.transform.position = position;
        StartCoroutine(RemoveEffect(obj, 2f));
    }

    private IEnumerator RemoveEffect(GameObject instance, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(instance);
    }
}
