
public class HistoryPanelData
{
    public UIPanelType Panel { get; private set; }
    public PanelParamBase PanelParam { get; private set; }
    public HistoryPanelData SecondPanel { get; private set; }

    public HistoryPanelData(UIPanelType panel)
    {
        this.Panel = panel;
    }

    public HistoryPanelData(UIPanelType panel, PanelParamBase param)
    {
        this.Panel = panel;
        this.SetPanelParam(param);
    }

    public HistoryPanelData(UIPanelType panel, PanelParamBase param, HistoryPanelData secondPanel)
    {
        this.Panel = panel;
        this.SetPanelParam(param);
        this.SetSecondPanel(secondPanel);
    }

    public void SetPanelParam(PanelParamBase param)
    {
        this.PanelParam = param;
    }

    public void SetSecondPanel(HistoryPanelData data)
    {
        this.SecondPanel = data;
    }
}

