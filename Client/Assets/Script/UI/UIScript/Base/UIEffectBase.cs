﻿using UnityEngine;
using DG.Tweening;

/// <summary>
/// UI效果的基类
/// </summary>
public abstract class UIEffectBase : MonoBehaviour, IUIEffect
{
    private Transform m_Transform;
    public Transform MyTransform { get { if (this.m_Transform == null) this.m_Transform = this.transform; return this.m_Transform; } }

    private GameObject m_GameObject;
    public GameObject MyGameObject { get { if (this.m_GameObject == null) this.m_GameObject = this.gameObject; return this.m_GameObject; } }

    protected float EffectTime=0.3f;

    public bool IsPlaying { get; protected set; }
    public abstract void Play();
    public abstract void Play(object obj);

    public virtual void Stop()
    {
        this.MyTransform.DOComplete();
        this.IsPlaying = false;
    }
}
