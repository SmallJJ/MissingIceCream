using System;

[Serializable]
public class LevelData
{
    private GridData[,] m_CurLevelData;

    #region public methods

    public LevelData(int row,int col)
    {
        this.m_CurLevelData = new GridData[row,col];
    }
    public GridData[,] GetLevelData()
    {
        return m_CurLevelData;
    }

    public void SetLevelData(int row, int col,GridData data)
    {
        this.m_CurLevelData[row, col] = data;
    }
    #endregion
}
