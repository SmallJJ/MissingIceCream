public interface IPanel
{
    void Open(UIPanelType type,PanelParamBase prarm, PanelEffectType effectType);
    void Close(PanelEffectType effectType);
}
