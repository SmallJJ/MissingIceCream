using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class FindPathControll : MonoBehaviour 
{
    public TestMaze TestMaze;
    public float Speed;
    public GameObject TargetArray;
    public LineRenderer m_LineRenderer;
    private Animation m_Animation;
    private Vector3 m_TargetPoint;
    private bool m_HasNewTarget;

    #region MonoBehaviour Methods
	void Awake () {
        this.m_HasNewTarget = false;
        this.m_Animation = this.GetComponent<Animation>();
	}
	
	void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                Debug.Log("tag: "+hit.collider.gameObject.tag);
                if (!hit.collider.gameObject.tag.Equals("Path"))
                {
                    return;
                }
                this.m_TargetPoint = hit.collider.transform.localPosition;
                this.ShowTargetArray(hit.collider.transform);
                this.RunToTarget();
            }
        }
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 100, 120, 50), "CreateMaze"))
        {
            this.TestMaze.ClearMaze();
            this.TestMaze.CreateMaze();
            this.transform.position = new Vector3(1, 1, -1);
        }
    }

    #endregion

    #region public methods
    private void RunToTarget()
    {
        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit hit;
        Vector3 hitStartPos = new Vector3(transform.localPosition.x, 3, transform.localPosition.z);
        RaycastHit[] faycastHits=Physics.RaycastAll(hitStartPos, Vector3.down, 3);
        hit=faycastHits.First(a => a.transform.tag == "Path");
        Vector3 startPoint=hit.collider.transform.localPosition;
        List < Vector3 > paths = this.TestMaze.GetOptimalPath(startPoint,this.m_TargetPoint);
        if (paths != null)
        {
            this.ShowLinePath(paths);
            this.ITweenMoveTo(paths);
        }
    }

    private void ITweenMoveTo(List<Vector3> paths)
    {
        this.transform.DOPath(paths.ToArray(), 0.5f).OnUpdate(this.DoPathUpdate).OnComplete(this.DoPathComplete);
    }

    private void DoPathUpdate()
    {
        this.m_Animation.Play("run");
    }

    private void DoPathComplete()
    {
        this.m_Animation.Play("idle");
    }

    private void ShowLinePath(List<Vector3> paths)
    {
        //画直线显示路径
        for (int i = 0; i < paths.Count-1; i++)
        {
            Debug.DrawLine(paths[i], paths[i+1], Color.red, 3f);
        }

        //贝塞尔曲线
        //int pointCount = paths.Count % 4;
        //pointCount = paths.Count - pointCount;
        //this.m_LineRenderer.SetVertexCount(pointCount / 3 * 10);
        // List<Vector3> bezierPath=this.GetBezierPath(paths,5);
        // for (int i = 0; i < bezierPath.Count; i++)
        // {
        //     this.m_LineRenderer.SetPosition(i, bezierPath[i]);
        // }
    }

    private void ShowTargetArray(Transform pathGridTrans)
    {
        GameObject targetArray = (GameObject)Instantiate(this.TargetArray);
        targetArray.transform.parent = pathGridTrans.parent;
        targetArray.transform.localPosition = pathGridTrans.localPosition;
    }

    private List<Vector3> GetBezierPath(List<Vector3> paths,int loop)
    {
        List<Vector3> bezierPath = new List<Vector3>(); ;
        //贝塞尔曲线
        int pointCount = paths.Count % 3;
        pointCount = paths.Count - pointCount;
        
        for (int i = 0; i < pointCount; i += 3)
        {
            Vector3 pos0 = paths[i];
            Vector3 pos1 = paths[i + 1];
            pos1 = pos1 - pos0;
            Vector3 pos2 = paths[i + 2];
            Bezier bezier = new Bezier(pos0, pos1, Vector3.zero, pos2);
            for (int j =0; j < 10; j++)
            {
                bezierPath.Add(bezier.GetPointAtTime(0.1f));
            }
        }
        if(loop>0)
        {
            this.GetBezierPath(bezierPath,loop-1);
        }
        return bezierPath;
    }

#endregion
}
