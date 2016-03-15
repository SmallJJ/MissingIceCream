using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 寻路网格类,用于实现寻路算法
/// </summary>
public class PathFindingMesh
{
    private int m_Heigth;   //网格宽度
    private int m_Width;  //网格高度
    private PathFindingGrid[,] m_Grids; //寻路方格的二维数组
    private List<PathFindingGrid> m_StartGrids = new List<PathFindingGrid>();   //寻路起点的链表
    private PathFindingGrid m_StartGrid;    //寻路起点
    private PathFindingGrid m_TargetGrid; //目标点
    private bool m_StartGridsValid = false; //起点是否可用
    private bool m_TargetGridIsValid = false;   //目标点是否可用

    #region public methods
    public PathFindingMesh(int height,int width)
    {
        this.m_Heigth = height;
        this.m_Width = width;
        this.m_Grids = new PathFindingGrid[height, width];
        this.InstantiateGrids();
        this.OrganizeGrids();
    }

    /// <summary>
    /// 设置寻路起点
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetStart(int y, int x)
    {
        if(this.IsInRange(y,x)&&this.IsValidGrid(this.m_Grids[y,x]))
        {
            this.m_StartGrid = this.m_Grids[y,x];
            List<PathFindingGrid> originalPath = new List<PathFindingGrid>();
            originalPath.Add(this.m_StartGrid);
            this.m_StartGrid.Paths.Add(originalPath);
            this.m_StartGridsValid = true;
        }
    }

    /// <summary>
    /// 设置寻路终点
    /// </summary>
    public void SetTarget(int y, int x)
    {
        if (this.IsInRange(y, x) && this.IsValidGrid(this.m_Grids[y, x]))
        {
            this.m_TargetGrid = this.m_Grids[y, x];
            this.m_TargetGrid.IsTarget = true;
            this.m_TargetGridIsValid = true;
        }
    }

    /// <summary>
    /// 检测一个格子是否有效
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public bool IsValidGrid(PathFindingGrid grid)
    {
        return new PathFindingGrid().IsValidGrid(grid);
    }

    /// <summary>
    /// 设置目标格子是否为障碍
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="isBarrier"></param>
    public void SetBarrier(int y, int x, bool isBarrier)
    {
        this.m_Grids[y, x].IsBarrier = isBarrier;
    }

    /// <summary>
    /// 获取起点到终点的最优路径
    /// </summary>
    public List<PathFindingGrid> GetOptimalPath()
    {
        if (!this.m_StartGridsValid && !this.m_TargetGridIsValid)
        {
            return null;
        }
        this.m_StartGrids.Add(this.m_StartGrid);
        while (this.m_StartGrids.Count>0)
        {
            List<PathFindingGrid> newStartGrids = new List<PathFindingGrid>();
            foreach (PathFindingGrid  startGridNode in this.m_StartGrids)
            {
                List<PathFindingGrid> optimalPath = startGridNode.GetOptimalPath();
                foreach (PathFindingGrid target in optimalPath)
                {
                    if (target.IsTarget)
                    {
                        return optimalPath;
                    }
                }
                startGridNode.CheckNearby();
                List<PathFindingGrid> nextTimeStartGrids = startGridNode.GetStartGrids();
                foreach (PathFindingGrid nextTimeStartGrid in nextTimeStartGrids)
                {
                    if (!newStartGrids.Contains(nextTimeStartGrid))
                    {
                        newStartGrids.Add(nextTimeStartGrid);
                    }
                }
            }
            this.m_StartGrids = newStartGrids;
        }
        return null;
    }

    /// <summary>
    /// 获取一个坐标点上的格子
    /// </summary>
    /// <returns></returns>
    public PathFindingGrid GetGridByPos(float y,float x)
    {
        for (int i = 0; i < this.m_Grids.GetLength(0); i++)
        {
            for (int j = 0; j < this.m_Grids.GetLength(1); j++)
            {
                float  xDistance =Math.Abs(this.m_Grids[i, j].X - x);
                float yDistance = Math.Abs(this.m_Grids[i, j].Y- y);
                if (xDistance < 1 && yDistance < 1)
                {
                    Debug.Log(String.Format("ClickPos Y: {0}  X: {1}  GirdPos: Y: {2}  X : {3}",y,x,this.m_Grids[i,j].Y,this.m_Grids[i,j].X));
                    return this.m_Grids[i, j];
                }
            }
        }
        return null;
    }

    #endregion

    #region private methods

    /// <summary>
    /// 要检查的格子是否在要寻路的网格的范围内
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool IsInRange(int y ,int x)
    {
        if (y >= 0 && y< this.m_Heigth && x >= 0 && x<this.m_Width)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 实例化寻路网格中的每一个格子,并且设置坐标
    /// </summary>
    private void InstantiateGrids()
    {
        for (int y = 0; y < this.m_Grids.GetLength(0); y++)
        {
            for (int x = 0; x < this.m_Grids.GetLength(1); x++)
            {
                this.m_Grids[y, x] = new PathFindingGrid();
                this.m_Grids[y, x].X = x;
                this.m_Grids[y, x].Y = y;
            }
        }
    }

    //给每个格子的上下左右格子(属性)赋值
    private void OrganizeGrids()
    {
        //把每个格子下方的格子(属性)设为下方的格子(第一行->倒数第二行)
        for (int i = 0; i < this.m_Grids.GetLength(0)-1; i++)
        {
            for (int j = 0; j < this.m_Grids.GetLength(1); j++)
            {
                this.m_Grids[i, j].BottomGrid = this.m_Grids[i+1, j];
            }
        }
        //把每个格子上方格子(属性)赋值为上方的格子(第二行->最后一行)
        for (int i = 1; i < this.m_Grids.GetLength(0); i++)
        {
            for (int j = 0; j < this.m_Grids.GetLength(1); j++)
            {
                this.m_Grids[i, j].TopGrid = this.m_Grids[i-1, j];
            }
        }
        //把每个格子右面的格子(属性)赋值为右边的格子(第一列->倒数第二列)
        for (int i = 0; i < this.m_Grids.GetLength(0); i++)
        {
            for (int j = 0; j < this.m_Grids.GetLength(1)-1; j++)
            {
                this.m_Grids[i, j].RightGrid = this.m_Grids[i, j+1];
            }
        }
        //把每个格子左边的格子(属性)赋值为左边的格子(第二列->最后一列)
        for (int i = 0; i < this.m_Grids.GetLength(0); i++)
        {
            for (int j = 1; j < this.m_Grids.GetLength(1); j++)
            {
                this.m_Grids[i, j].LeftGrid = this.m_Grids[i, j-1];
            }
        }
    }

    #endregion

}
