   public abstract class ComponentBase:UIBase,IComponent
   {

       public bool IsUpdated { get; set; }  //  是否更新完成
       public bool IsOpen { get { return this.MyGameObject.activeInHierarchy; } }   //是否打开

       #region virtual methods
       public virtual void Show()
        {
            if (!this.IsInit)
                this.Init();
            this.MyGameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            this.MyGameObject.SetActive(false);
        }
       #endregion

       #region override methods
        public override void Clear()
        {
            this.IsUpdated = false;
            base.Clear();
        }
       #endregion

       #region public methods
        public void AdjustCollider()
        {
 
        }
       #endregion
   }
