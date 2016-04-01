using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class MeshMgr: MonoBehaviour
{
    private int m_MazeCols;  //迷宫列数
    private int m_MazeRows;   //迷宫行数
    private int m_MeshCols;    //网格列数
    private int m_MeshRows;   //网格行数
    private PathFindingMesh m_PathFindingMesh;  //寻路网格
    private List<PathFindingGrid> m_OptimalPath;    //最优路径
    private bool[,] m_MazeArray;    //迷宫映射Bool数组

    private bool m_Creating = false;

    #region public methods

    public MeshMgr(int row,int col)
    {
        this.m_MazeRows = row;
        this.m_MazeCols = col;
    }
    /// <summary>
    /// 创建迷宫
    /// </summary>
    public bool[,] CreateMaze()
    {
        this.m_Creating = true;
        float startTime = Time.realtimeSinceStartup;
        Debug.Log("CreateMeze StartTime: " + startTime);
        this.m_MeshCols = this.m_MazeCols * 2 + 1;
        this.m_MeshRows = this.m_MazeRows * 2 + 1;
        this.m_PathFindingMesh = new PathFindingMesh(this.m_MeshRows, this.m_MeshCols);
        Maze maze = new Maze(this.m_MazeRows, this.m_MazeCols);
        this.m_MazeArray = maze.GetBoolArray();
        this.SetMashBarrier();
        this.m_Creating = false;
        float endTime=Time.realtimeSinceStartup;
        Debug.Log("CreateMeze EndTime: " + endTime + "  Cost Time: " + (endTime-startTime));
        return this.m_MazeArray;
    }

    public PathFindingGrid VectorPosConvertToMatrixPos(Vector3 pos)
    {
       return this.m_PathFindingMesh.GetGridByPos(-pos.z, pos.x);
    }

   /// <summary>
   /// 迷宫数据映射到网格上
   /// </summary>
    public void SetMashBarrier()
    {
        for (int row = 0; row < this.m_MeshRows; row++)
        {
            for (int col = 0; col < this.m_MeshCols; col++)
            {
                this.m_PathFindingMesh.SetBarrier(row, col, this.m_MazeArray[row, col]);
            }
        }
    }

    /// <summary>
    /// 获取最优路径
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    public List<Vector3> GetOptimalPath(Vector3 startPos,Vector3 endPos)
    {
        Debug.Log("startGrid: " + startPos);
        Debug.Log("endGrid: " + endPos);

        this.m_PathFindingMesh.SetStart((int)-startPos.y,(int) startPos.x);
        this.m_PathFindingMesh.SetTarget((int)-endPos.y, (int)endPos.x);
        this.m_OptimalPath = this.m_PathFindingMesh.GetOptimalPath();

        if (this.m_OptimalPath == null)
        {
            Debug.Log("Not Find a OptimalPath");
            return null;
        }

        List<Vector3> pathsList = new List<Vector3>();
        foreach (PathFindingGrid grid in this.m_OptimalPath)
        {
            pathsList.Add(new Vector3(grid.X,-grid.Y,0));
        }
        return pathsList;
    }

    /// <summary>
    /// 通过位置设置状态
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="isSetBarrier"></param>
    public void SetStatusByPos(int row, int col, bool isSetBarrier)
    {
        if (row < this.m_MazeArray.GetLength(1)
            && col < this.m_MazeArray.GetLength(0))
        {
            this.m_MazeArray[row, col] = isSetBarrier;
            this.m_PathFindingMesh.SetBarrier(row, col, isSetBarrier);
        }
        else
        {
            Debug.LogError(string.Format("数据错误：行 {0} , 列 {1} , 是障碍 {2} ", row, col, isSetBarrier));
        }
    }
    #endregion

}
