using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class TestMaze: MonoBehaviour
{
    public GameObject WallGo;
    public Transform MazeParent;
    public int MazeWidth;
    public int MazeHeight;
    private int m_MeshWidth;
    private int m_MashHeight;
    private PathFindingMesh m_PathFindingMesh;
    private List<PathFindingGrid> m_OptimalPath;
    private LineRenderer m_LineRenderer;
    private bool[,] m_MazeArray;

    private bool m_Creating = false;
    private bool m_Cleaning = false;


    #region private methods
    public void ClearMaze()
    {
        this.m_Cleaning = true;
        Debug.Log("ChildCount:" + this.MazeParent.childCount);
        int count = this.MazeParent.childCount;
        for (int i = 0; i < count; i++)
		{
            Destroy(this.MazeParent.GetChild(i).gameObject);
		}
        this.m_Cleaning = false;
    }

    public void CreateMaze()
    {
        float startTime=Time.realtimeSinceStartup;
        Debug.Log("CreateMeze StartTime: " + startTime);

        this.m_Creating = true;
        this.m_MeshWidth = this.MazeWidth * 2 + 1;
        this.m_MashHeight = this.MazeHeight * 2 + 1;
        this.m_PathFindingMesh = new PathFindingMesh(this.m_MashHeight, this.m_MeshWidth);

        Maze maze = new Maze(this.MazeHeight,this.MazeWidth);
        this.m_MazeArray = maze.GetBoolArray();
        for (int z = 0; z < this.m_MashHeight; z++)
        {
            for (int x = 0; x < this.m_MeshWidth; x++)    
            {
                GameObject go = (GameObject)GameObject.Instantiate(this.WallGo, Vector3.zero, Quaternion.identity);
                go.transform.parent = this.MazeParent;
                go.transform.localPosition = new Vector3(x, 0, -z);
                go.transform.localScale = new Vector3(1, 2, 1);
                if (this.m_MazeArray[z, x])
                {
                    this.m_PathFindingMesh.SetBarrier(z, x, true);
                    go.gameObject.tag = "Barrier";
                }
                else
                {
                    MeshRenderer render = go.GetComponent<MeshRenderer>();
                    render.enabled = false;
                    go.gameObject.tag = "Path";
                }
            }
        }
        this.m_Creating = false;
        float endTime=Time.realtimeSinceStartup;
        Debug.Log("CreateMeze EndTime: " + endTime + "  Cost Time: " + (endTime-startTime));
    }

    public PathFindingGrid VectorPosConvertToMatrixPos(Vector3 pos)
    {
       return this.m_PathFindingMesh.GetGridByPos(-pos.z, pos.x);
    }

    /// <summary>
    /// 为了寻路的快速,找过的都设为障碍了,再次寻路的时候需要开启
    /// </summary>
    public void ReSetMazeBarrier()
    {
        this.m_PathFindingMesh = new PathFindingMesh(this.m_MashHeight, this.m_MeshWidth);
        for (int z = 0; z < this.m_MashHeight; z++)
        {
            for (int x = 0; x < this.m_MeshWidth; x++)
            {
                if (this.m_MazeArray[z, x])
                    this.m_PathFindingMesh.SetBarrier(z, x, true);
            }
        }
    }

    public List<Vector3> GetOptimalPath(Vector3 startPos,Vector3 endPos)
    {
        Debug.Log("startGrid: " + startPos);
        Debug.Log("endGrid: " + endPos);

        this.ReSetMazeBarrier();
        this.m_PathFindingMesh.SetStart((int)-startPos.z,(int) startPos.x);
        this.m_PathFindingMesh.SetTarget((int)-endPos.z, (int)endPos.x);
        this.m_OptimalPath = this.m_PathFindingMesh.GetOptimalPath();

        if (this.m_OptimalPath == null)
        {
            Debug.Log("Not Find a OptimalPath");
            return null;
        }

        List<Vector3> pathsList = new List<Vector3>();
        foreach (PathFindingGrid grid in this.m_OptimalPath)
        {
            pathsList.Add(new Vector3(grid.X,1,-grid.Y));
        }
        return pathsList;
    }
    #endregion

}
