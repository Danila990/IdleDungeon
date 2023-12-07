using System;
using UnityEngine;
using Spine.Unity;

public class AnimationController : MonoBehaviour
{

    #region Свойства

    public bool ExtAnimationSwitch
    {
        get => extAnimationSwitch;
        set => extAnimationSwitch = value;
    }

    #endregion

    #region Публичные Методы

    public void SetIdle() => SetState(State.Idle);
    public void SetMove() => SetState(State.Move);
    public void SetAttack() => SetState(State.Attack);
    public void SetSkill() => SetState(State.Skill);
    
    #endregion

    #region Приватные поля

    [SerializeField] private bool extAnimationSwitch;
    [SerializeField] private AnimationReferenceAsset animationSkill;
    [SerializeField] private AnimationReferenceAsset animationAttack;
    [SerializeField] private AnimationReferenceAsset animationIdle;
    [SerializeField] private AnimationReferenceAsset animationMove;
    [SerializeField] private AnimationReferenceAsset extAnimationAttack;
    [SerializeField] private AnimationReferenceAsset extAnimationIdle;
    [SerializeField] private AnimationReferenceAsset extAnimationMove;
    private SkeletonAnimation skeleton;
    private bool cyrExtAnim;
    private State cyrState;
    
    private enum State
    {
        Idle,
        Move,
        Attack,
        Skill
    }

    private float animSpeed = 1f;

    #endregion

    #region Приватные Методы

    private void Start()
    {
        cyrExtAnim = extAnimationSwitch;
        skeleton = GetComponent<SkeletonAnimation>();
        SetState(State.Idle);
    }

    private void Update()
    {
        if (extAnimationSwitch != cyrExtAnim)
        {
            cyrExtAnim = extAnimationSwitch;
            SetState(cyrState);
        }
    }

    private void SetState(State state)
    {
        cyrState = state;
        
        switch (state)
        {
            case State.Idle:
                SetAnimation(extAnimationSwitch ? extAnimationIdle : animationIdle, true, animSpeed);
                break;
            case State.Move:
                SetAnimation(extAnimationSwitch ? extAnimationMove : animationMove, true, animSpeed);
                break;
            case State.Attack:
                SetAnimation(extAnimationSwitch ? extAnimationAttack : animationAttack, true, animSpeed);
                break;
            case State.Skill:
                SetAnimation(animationSkill, false, animSpeed);
                break;
            default: break;
        }
    }

    private void SetAnimation(AnimationReferenceAsset anim, bool loop, float timeScale)
    {
        if (skeleton != null && anim != null)
        {
            Spine.TrackEntry trackEntry = skeleton.state.SetAnimation(0, anim, loop);
            trackEntry.TimeScale = timeScale;
        }
    }

    #endregion
}
