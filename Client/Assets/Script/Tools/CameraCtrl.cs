using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class CameraCtrl:MonoBehaviourSingleton<CameraCtrl>
{
    public Camera MainCamera;
    public Vector3 CameraInitPos = new Vector3(0,0,-1);
    public int ScaleFactor = 300;

    public CameraCtrlType CurrentCtrlType { get; private set;}
    private int m_SwipeHorizontalOffset;
    private int m_SwipeVerticalOffset;
    private Vector3 m_CameraPos;
    private Gesture m_StartGesture;
    private int m_MapWidth;
    private int m_MapHeight;


    void OnEnable()
    {
        EasyTouch.On_SwipeStart += this.On_SwipeStart;
        EasyTouch.On_Swipe += this.On_Swipe;
        EasyTouch.On_SwipeEnd += this.On_SwipeEnd;
        EasyTouch.On_PinchIn += this.On_PinchIn;
        EasyTouch.On_PinchOut += this.On_PinchOut;
        EasyTouch.On_PinchEnd += this.On_PinchEnd;
        this.transform.localPosition = this.CameraInitPos;
    }


    void OnDisable()
    {
        EasyTouch.On_SwipeStart -= this.On_SwipeStart;
        EasyTouch.On_Swipe -= this.On_Swipe;
        EasyTouch.On_SwipeEnd -= this.On_SwipeEnd;
        EasyTouch.On_PinchIn -= this.On_PinchIn;
        EasyTouch.On_PinchOut -= this.On_PinchOut;
        EasyTouch.On_PinchEnd -= this.On_PinchEnd;
    }

    /// <summary>
    /// 放大
    /// </summary>
    /// <param name="gesture"></param>
    private void On_PinchOut(Gesture gesture)
    {
            this.CurrentCtrlType = CameraCtrlType.Scale;
            this.ScaleCameraSize(gesture.deltaPinch, false);
    }

    /// <summary>
    /// 缩小
    /// </summary>
    /// <param name="gesture"></param>
    private void On_PinchIn(Gesture gesture)
    {
            this.CurrentCtrlType = CameraCtrlType.Scale;
            this.ScaleCameraSize(gesture.deltaPinch, true);
    }

    /// <summary>
    /// 缩放结束
    /// </summary>
    /// <param name="gesture"></param>
    private void On_PinchEnd(Gesture gesture)
    {
        this.OperationEnd();
    } 

    /// <summary>
    /// 拖动开始
    /// </summary>
    /// <param name="geture"></param>
    private void On_SwipeStart(Gesture geture)
    {
        if (this.IsCanOperation())
        {
            this.CurrentCtrlType = CameraCtrlType.Move;
            this.m_StartGesture = geture;
            this.m_CameraPos = this.MainCamera.transform.position;
            this.UpdateLimitOffset();
        }
    }

    /// <summary>
    /// 拖动中
    /// </summary>
    /// <param name="gesture"></param>
    private void On_Swipe(Gesture gesture)
    {
        if (this.CurrentCtrlType == CameraCtrlType.Move)
        {
            Vector3 posFirst = this.m_StartGesture.GetTouchToWorldPoint(this.CameraInitPos.z);
            Vector3 posSecond = gesture.GetTouchToWorldPoint(this.CameraInitPos.z);
            Vector3 offset = posSecond - posFirst;
            Vector3 newPos = this.m_CameraPos;
            newPos.x = this.m_CameraPos.x - offset.x;
            newPos.x = Mathf.Clamp(newPos.x, -this.m_SwipeHorizontalOffset, this.m_SwipeHorizontalOffset);
            newPos.y = this.m_CameraPos.y - offset.y;
            newPos.y = Mathf.Clamp(newPos.y, -this.m_SwipeVerticalOffset, this.m_SwipeVerticalOffset);
            this.MainCamera.transform.position = newPos;
        }
    }

    /// <summary>
    /// 拖动结束
    /// </summary>
    /// <param name="gesture"></param>
    private void On_SwipeEnd(Gesture gesture)
    {
        this.OperationEnd();
    }

    /// <summary>
    /// 计算拖动限制区域
    /// </summary>
    private void UpdateLimitOffset()
    {
        float size = this.MainCamera.orthographicSize;
        this.m_SwipeHorizontalOffset = (int)Mathf.Abs(size - this.m_MapWidth) / 2;
       this.m_SwipeVerticalOffset = (int)Mathf.Abs(size - this.m_MapHeight) / 2;
    }

    /// <summary>
    /// 推近摄像机 放大
    /// </summary>
    private void ScaleCameraSize(float deltaPinch,bool isInPinch)
    {
        float size = deltaPinch * Time.deltaTime*this.ScaleFactor;
        if (isInPinch)
        {
            size += this.MainCamera.orthographicSize;
        }
        else
        {
            size = this.MainCamera.orthographicSize-size;
        }
        size = Mathf.Clamp(size, UIConst.CameraSizeMin, UIConst.CameraSizeMax);
        this.MainCamera.orthographicSize = size;
    }

    #region public methods

    /// <summary>
    /// 设置相机Size
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void SetCameraSize(int width,int height)
    {
        this.m_MapWidth = width;
        this.m_MapHeight = height;
        this.MainCamera.orthographicSize = Mathf.Max(width, height);
        this.UpdateLimitOffset();
    }

    public void SetStatus(CameraCtrlType type)
    {
        this.CurrentCtrlType = type;
    }

    public bool IsCanOperation()
    {
        return this.CurrentCtrlType == CameraCtrlType.None;
    }

    public void OperationEnd()
    {
        this.SetStatus(CameraCtrlType.None);
    }
#endregion
}
