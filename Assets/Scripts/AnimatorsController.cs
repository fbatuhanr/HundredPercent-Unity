using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorsController : MonoBehaviour
{
    [SerializeField] private Animator canvasAnimator, squaresAnimator;

    public void CanvasAnimatorPlay(string anim)
    {
        canvasAnimator.Play(anim);
    }

    public void SquaresAnimatorPlay(string anim)
    {
        squaresAnimator.Play(anim);
    }
}
