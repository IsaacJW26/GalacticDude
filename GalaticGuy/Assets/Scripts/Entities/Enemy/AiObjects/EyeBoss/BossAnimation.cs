using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation
{
    private List<BossAnimationPart> partsList;
    private IEnumerator<BossAnimationPart> partsIterator;
    private bool animationRunning = true;
    public delegate void AnimationFunction(int currentFrame);
    private int currentFrame = 0;
    private float remainderFrames = 0f;
    private float? attackSpeed = null;

    private struct BossAnimationPart
    {
        public BossAnimationPart(int animationDurationInFrames, AnimationFunction animationFunction)
        {
            this.Duration = animationDurationInFrames;
            this.AnimateUpdate = animationFunction;
        }

        public int Duration { get; private set; }
        public AnimationFunction AnimateUpdate { get; private set; }
    }

    public BossAnimation()
    {
        partsList = new List<BossAnimationPart>();
    }

    private BossAnimation AddAnimationPart(BossAnimationPart animationPart)
    {
        partsList.Add(animationPart);
        return this;
    }

    public BossAnimation AddAnimationPart(int animationDuration, AnimationFunction animationFunction)
    {
        return AddAnimationPart(new BossAnimationPart(animationDuration, animationFunction));
    }

    public BossAnimation AddDelay(int delayDuration)
    {
        return AddAnimationPart(delayDuration, null);
    }

    public BossAnimation AddAnimationFrame(AnimationFunction animationFunction)
    {
        return AddAnimationPart(new BossAnimationPart(1, animationFunction));
    }

    public void FrameUpdate()
    {
        Debug.Log("frup");
        if(partsIterator == null)
        {
            partsIterator = partsList.GetEnumerator();
            partsIterator.MoveNext();
        }

        if(animationRunning)
        {
            if(currentFrame >= partsIterator.Current.Duration)
            {
                animationRunning = partsIterator.MoveNext();
                currentFrame = 0;
            }
            else
            {
                if(partsIterator.Current.AnimateUpdate != null)
                    partsIterator.Current.AnimateUpdate(currentFrame);

                if(!attackSpeed.HasValue)
                {
                    currentFrame++;
                    remainderFrames = 0f;
                }
                else
                {
                    // get decimal remainder of frames
                    remainderFrames += attackSpeed.Value - (float)(int)attackSpeed.Value;
                    // add remainder frames when it exceeds 1
                    int newFrames = (int)attackSpeed.Value + (remainderFrames >= 1f ? (int)remainderFrames : 0);
                }
            }
        }
    }

    public void SetTimeScale(float timeScale)
    {
        this.attackSpeed = timeScale;
    }
}