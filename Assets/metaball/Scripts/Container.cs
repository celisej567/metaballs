using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Container : MonoBehaviour {
    public float safeZone = 0.8f;
    public float resolution = 0.2f;
    public float threshold = 1;
    public ComputeShader computeShader;
    public bool calculateNormals = true;
    public Material material;

    float lastsafeZone = 0.04f;
    float lastresolution = 0.15f;
    float lastthreshold = 1;
    Vector3 lastscale = new Vector3(1,1,1);

    MetaBall[] balls; //hehe
    Vector3[] lastBallsPos;
    bool update;
    List<Vector3> lastPosBalls = new List<Vector3>();

    private bool message = true;

    private CubeGrid grid;

    public void Start() {
        balls = GetComponentsInChildren<MetaBall>();
        
        for (int i = 0; i<=balls.Length-1; i++)
        {
            lastPosBalls.Add(balls[i].transform.position);
        }

        this.grid = new CubeGrid(this, this.computeShader);
    }

    private void Awake()
    {
        Start();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(gameObject.transform.position, gameObject.transform.localScale);

        Gizmos.color = Color.red;
        Vector3 safeScale = new Vector3(gameObject.transform.localScale.x - safeZone, gameObject.transform.localScale.y - safeZone, gameObject.transform.localScale.z - safeZone);
        Gizmos.DrawWireCube(gameObject.transform.position, safeScale);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 2));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.right * 2));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * 2));
    }

    public void LateUpdate() 
    {

        if (Application.isPlaying)
        {

            for (int i = 0; i <= balls.Length - 1; i++)
            {
                if (lastPosBalls[i] != balls[i].transform.position)
                {
                    update = true;
                    lastPosBalls[i] = balls[i].gameObject.transform.position;
                }
            }

            if (update || (lastsafeZone!=safeZone) || (lastresolution != resolution) || (lastthreshold!=threshold) || (lastscale != transform.localScale))
            {
                if (grid == null || (lastscale != transform.localScale) || (resolution != lastresolution))
                {
                    this.grid = new CubeGrid(this, this.computeShader);
                }

                this.grid.evaluateAll(balls);

                Mesh mesh = this.GetComponent<MeshFilter>().mesh;
                mesh.Clear();
                mesh.vertices = this.grid.vertices.ToArray();
                mesh.triangles = this.grid.getTriangles();

                if (this.calculateNormals)
                {
                    mesh.RecalculateNormals();
                }

                lastsafeZone = safeZone;
                lastresolution = resolution;
                lastthreshold = threshold;
                lastscale = transform.localScale;
                update = false;
            }
            
        }
        else
        {
            balls = GetComponentsInChildren<MetaBall>();
            if (balls == null)
                return;

            if (GetComponent<MeshRenderer>().material != material)
                GetComponent<MeshRenderer>().material = material;

            this.grid = new CubeGrid(this, this.computeShader);

            this.grid.evaluateAll(balls);
            Mesh mesh;

            mesh = this.GetComponent<MeshFilter>().mesh;
            if (!Application.isPlaying && message)
            {
                Debug.Log("If you got \"Please use MeshFilter.sharedMesh instead.\" warning - ignore it.");
                message = false;
            }

            mesh.Clear();
            mesh.vertices = this.grid.vertices.ToArray();
            mesh.triangles = this.grid.getTriangles();

            if (this.calculateNormals)
            {
                mesh.RecalculateNormals();
            }
        }
    }
}