using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MetaBall : MonoBehaviour {
    public float radius = 0.09f;
    public bool negativeBall;

    [HideInInspector]
    public float factor = 1;
    Vector3 voidSize;

    public bool DrawGizmos;

    public virtual void OnDrawGizmos()
    {
        if (!DrawGizmos)
            return;

        if(voidSize != GetComponentInParent<Container>().transform.localScale)
        {
            voidSize = GetComponentInParent<Container>().transform.localScale;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius * Mathf.Min(voidSize.x, voidSize.y, voidSize.z));
    }

    public virtual void Start() {
        this.factor = (this.negativeBall ? -1 : 1) * this.radius * this.radius;
    }

    public virtual void Update()
    {
        this.factor = (this.negativeBall ? -1 : 1) * this.radius * this.radius;
    }
}
