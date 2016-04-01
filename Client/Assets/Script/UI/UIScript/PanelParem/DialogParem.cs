using System;

public class DialogParem : PanelParamBase
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DialogType Type { get; set; }
    public event Action<bool> ResultEvent;

    public void ExecuteResultEvent(bool result)
    {
        if (this.ResultEvent != null)
            this.ResultEvent(result);
    }
}
