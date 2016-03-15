using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalLabelComponent : ComponentBase
{
    #region public members;

    public LocalizationUtils.ResourceType Type = LocalizationUtils.ResourceType.Default;   //文字库
    public string LocalKey;     //文字键值
    public Text Label;
    public string text
    {
        get { return this.Label.text; }
        set { this.Label.text = value; }
    }
    #endregion

    #region private members

    private object[] m_Args;    //参数

    #endregion

    #region override methods

    protected override void Awake()
    {
        //Debug.Log("LocalLabelComponent Awake");
        if (this.Label == null)
        {
            this.Label = GetComponent<Text>();
        }
        this.UpdateLabel();
     
    }

    public override void Clear()
    {
        //Debug.Log("LocalLabelComponent Clear");
        this.Label.text = string.Empty;
        this.LocalKey = string.Empty;
        this.m_Args = null;
    }

    #endregion

    #region private methods

    /// <summary>
    /// 更新文字
    /// </summary>
    [ExecuteInEditMode]
    [ContextMenu("Update Label")]
    private void UpdateLabel()
    {
        if (this.Label == null)
        {
            this.Label = GetComponent<Text>();

            if (this.Label == null)
            {
                return;
            }
        }

        string str = string.Empty;
        switch (Type)
        {
            case LocalizationUtils.ResourceType.Default:
                str = LocalizationUtils.GetText(this.LocalKey, this.m_Args);
                break;
        }
        if (!string.IsNullOrEmpty(str))
        {
            this.Label.UpdateLabel(str);
        }
        else if (!string.IsNullOrEmpty(this.LocalKey))
        {
            this.Label.text = string.Empty;
        }
    }

    #endregion

    #region public methods

    /// <summary>
    /// 通过本地化键值更新文字
    /// </summary>
    /// <param name="key"></param>
    /// <param name="args"></param>
    public void UpdateLabel(string key, params object[] args)
    {
        this.LocalKey = key;
        this.m_Args = args;
        this.UpdateLabel();
    }

    /// <summary>
    /// 通过指定本地化库的键值更新文字,
    /// </summary>
    public void UpdateLabel(LocalizationUtils.ResourceType languageType, string key, params object[] args)
    {
        this.Type = languageType;
        this.LocalKey = key;
        this.m_Args = args;
        this.UpdateLabel();
    }

    /// <summary>
    /// 更新颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        this.Label.color = color;
    }

    [ExecuteInEditMode]
    [ContextMenu("Update LocalizationFile")]
    private void UpdatLocalizationFile()
    {
        LocalizationUtils.Instance.Init();
    }

    #endregion

}
