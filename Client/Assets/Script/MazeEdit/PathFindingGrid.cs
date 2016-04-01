using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 该类作为自动寻路的基本格子
/// </summary>
public class PathFindingGrid
{
    #region Attribute
    /// <summary>
    /// X坐标
    /// </summary>
    private int x;
    public int X
    {
        get { return x; }
        set { x = value; }
    }
    
   /// <summary>
    /// Y坐标
    /// </summary>
    private int y;
    public int Y
    {
        get { return y; }
        set { y = value; }
    }

    /// <summary>
    /// 是否作为起点检查过周围的格子
    /// </summary>
    private bool isChecked = false;
    public bool IsChecked
    {
        get { return isChecked; }
        set { isChecked = value; }
    }

    /// <summary>
    /// 该方格是否为本次寻路的终点
    /// </summary>
    private bool isTarget = false;
    public bool IsTarget
    {
        get { return isTarget; }
        set { isTarget = value; }
    }

    /// <summary>
    /// 该方格是否为障碍
    /// </summary>
    private bool isBarrier = false;
    public bool IsBarrier
    {
        get { return isBarrier; }
        set { isBarrier = value; }


    }

    /// <summary>
    /// 可以到达该方格的所有路径,作为到达该方格最优路径的可选路径
    /// </summary>
    private List<List<PathFindingGrid>> paths = new List<List<PathFindingGrid>>();
    public List<List<PathFindingGrid>> Paths
    {
        get { return paths; }
        set { paths = value; }
    }

    /// <summary>
    /// 该格子上方的格子
    /// </summary>
    private PathFindingGrid topGrid = null;
    public PathFindingGrid TopGrid
    {
        get { return topGrid; }
        set { topGrid = value; }
    }


    /// <summary>
    /// 该格子下方的格子
    /// </summary>
    private PathFindingGrid bottomGrid = null;
    public PathFindingGrid BottomGrid
    {
        get { return bottomGrid; }
        set { bottomGrid = value; }
    }

    /// <summary>
    /// 该格子左方的格子
    /// </summary>
    private PathFindingGrid leftGrid = null;
    public PathFindingGrid LeftGrid
    {
        get { return leftGrid; }
        set { leftGrid = value; }
    }

    /// <summary>
    /// 该格子右方的格子
    /// </summary>
    private PathFindingGrid rightGrid = null;
    public PathFindingGrid RightGrid
    {
        get { return rightGrid; }
        set { rightGrid = value; }
    }

    #endregion

    #region private methods

    /// <summary>
    /// 添加可用的路径
    /// </summary>
    /// <param name="grid"></param>
    private void AddEnabeldPath(PathFindingGrid grid)
    {
        if(this.IsValidGrid(grid))
        {
            List<PathFindingGrid> cloneOptimalPath = new List<PathFindingGrid>();
            List<PathFindingGrid> optimalPath = this.GetOptimalPath();
            //克隆最优路径
            foreach (PathFindingGrid gridOfOptimalPath in optimalPath)
            {
                cloneOptimalPath.Add(gridOfOptimalPath);
            }
            //添加路径
            cloneOptimalPath.Add(grid);
            // (从开始点->当前的格子->当前格子周围的格子) 这段路径赋给个周围的这个格子,  (也就是每个格子都存储了从起点到这个格子的多种路径)
            grid.Paths.Add(cloneOptimalPath);
        }
    }
    #endregion

    #region public methods

    /// <summary>
    /// 检查周围的格子
    /// </summary>
    public void CheckNearby()
    {
        //如果某方格可以到达,将这个方格中添加到到达这个方格的最优路径之一
        this.AddEnabeldPath(this.TopGrid);
        this.AddEnabeldPath(this.BottomGrid);
        this.AddEnabeldPath(this.LeftGrid);
        this.AddEnabeldPath(this.RightGrid);
        //标记为已经检查过
        this.IsChecked = true;
        //消除本方格的待选路径
        this.Paths = null;
    }

    /// <summary>
    /// 格子是否可用的,(没有检查过,并且不是障碍)
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public bool IsValidGrid(PathFindingGrid grid)
    {
        if (grid != null && (!grid.IsChecked) && (!grid.IsBarrier))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 从在可以到达该方格的所有路径中选择一条最优路径
    /// </summary>
    /// <returns></returns>
    public List<PathFindingGrid> GetOptimalPath()
    {
        List<PathFindingGrid> optimalPath = null;
        foreach (List<PathFindingGrid> path in this.Paths)
        {
            if (optimalPath == null)
            {
                optimalPath = path;
            }
            else
            {
                if (path.Count < optimalPath.Count)
                {
                    optimalPath = path;
                }
            }
        }
        return optimalPath;
    }

    /// <summary>
    /// 获取所有起点格子链表
    /// </summary>
    /// <returns></returns>
    public List<PathFindingGrid> GetStartGrids()
    {
        List<PathFindingGrid> newStartGrids = new List<PathFindingGrid>();
        if (this.IsValidGrid(this.TopGrid))
        {
            newStartGrids.Add(this.TopGrid);
        }
        if (this.IsValidGrid(this.BottomGrid))
        {
            newStartGrids.Add(this.BottomGrid);
        }
        if (this.IsValidGrid(this.LeftGrid))
        {
            newStartGrids.Add(this.LeftGrid);
        }
        if (this.IsValidGrid(this.RightGrid))
        {
            newStartGrids.Add(this.RightGrid);
        }
        return newStartGrids;
    }

    public bool IsTest()
    {
        return this.x > 0;
    }

    #endregion
}
