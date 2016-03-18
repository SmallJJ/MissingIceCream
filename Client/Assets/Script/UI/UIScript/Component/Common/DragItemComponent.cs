using UnityEngine;
public class DragItemComponent:ComponentBase
{
    private BoxCollider2D m_Collider;

    #region override methods
    protected override void Awake()
    {
        this.m_Collider = this.MyGameObject.GetComponent<BoxCollider2D>();
        if (this.m_Collider == null)
        {
            this.m_Collider = this.MyGameObject.AddComponent<BoxCollider2D>();
        }
        base.Awake();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        UIEventListener.Get(this.MyGameObject).onDrag += this.OnDrag;
    }
    #endregion


    #region private methods

    private void OnDrag(GameObject go, Vector2 delta)
    {
        if(go.Equals(this.gameObject))
        {
             Vector3 pos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
#if UNITY_Editer

             pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#elif And
             this.MyTransform.position = new Vector3(pos.x, pos.y, this.MyTransform.position.z);
        }
    }

    private void CloneGo()
    {
 
    }
    #endregion
}
