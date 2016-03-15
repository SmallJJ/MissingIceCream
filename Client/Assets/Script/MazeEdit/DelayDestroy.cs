using UnityEngine;
using System.Collections;

public class DelayDestroy : MonoBehaviour {
    [SerializeField]
    float m_Delay = 1.0f;
    public float Delay { get { return this.m_Delay; } }

    void Start () {
        Destroy(gameObject, this.m_Delay);
	}
	
 
}
