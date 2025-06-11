using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSkin
{
    sword,
    bow = 2,
    magic
}

public class Title_Player : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    public PlayerSkin mySkin;

    // Start is called before the first frame update
    void Start()
    {
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.skeleton;

        skeleton.ScaleX = -1f;
        spineAnimationState.SetAnimation(0, "attack_sword", true);
        spineAnimationState.TimeScale = 0.5f;
    }
}
