using UnityEngine;
using System.Collections.Generic;

namespace MetaBalls
{
    [ExecuteInEditMode]
    public class Container : MonoBehaviour
    {

        [Header("Variables")]
        public float safeZone = 0.8f;
        public float threshold = 1;
        public float updateDelay = 0.03f;

        public ComputeShader computeShader;
        public bool calculateNormals = true;
        public Material material;

        public float resolutionX = 0.2f;
        public float resolutionY = 0.2f;
        public float resolutionZ = 0.2f;

        float lastsafeZone = 0.04f;
        float lastResolutionX = 0.2f;
        float lastResolutionY = 0.2f;
        float lastResolutionZ = 0.2f;
        float lastthreshold = 1;
        float nextUpdate;
        Vector3 lastscale = new Vector3(1, 1, 1);

        MetaBall[] balls; //hehe
        Vector3[] lastBallsPos;
        List<Vector3> lastPosBalls = new List<Vector3>();
        MeshFilter meshFilter;

        private bool message = true;
        private CubeGrid grid;

        Mesh mesh;
        Mesh mySharedMesh;
        Mesh myMesh;

        bool CheckForChangedResolution()
        {
            if ((resolutionX != lastResolutionX) || (resolutionY != lastResolutionY) || (resolutionZ != lastResolutionZ))
            {
                lastResolutionX = resolutionX;
                lastResolutionY = resolutionY;
                lastResolutionZ = resolutionZ;
                return true;
            }
            return false;
        }

        bool CheckForBallsPos()
        {
            for (int i = 0; i <= balls.Length - 1; i++)
            {
                if (lastPosBalls[i] != balls[i].transform.position)
                {
                    lastPosBalls[i] = balls[i].gameObject.transform.position;
                    return true;
                }
            }
            return false;
        }

        Vector3[] Smooth(Vector3[] verts, int[] triangles)
        {
            Vector3[] normals = new Vector3[verts.Length];
            List<Vector3>[] vertexNormals = new List<Vector3>[verts.Length];
            for (int i = 0; i < vertexNormals.Length; i++)
            {
                vertexNormals[i] = new List<Vector3>();
            }
            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 currNormal = Vector3.Cross(
                    (verts[triangles[i + 1]] - verts[triangles[i]]).normalized,
                    (verts[triangles[i + 2]] - verts[triangles[i]]).normalized);

                vertexNormals[triangles[i]].Add(currNormal);
                vertexNormals[triangles[i + 1]].Add(currNormal);
                vertexNormals[triangles[i + 2]].Add(currNormal);
            }
            for (int i = 0; i < vertexNormals.Length; i++)
            {
                normals[i] = Vector3.zero;
                float numNormals = vertexNormals[i].Count;
                for (int j = 0; j < numNormals; j++)
                {
                    normals[i] += vertexNormals[i][j];
                }
                normals[i] /= numNormals;
            }

            return normals;
        }

        Vector3[] CalculateNormals(Mesh mesh)
        {

            Vector3[] normals = mesh.normals;
            int[] trigs = mesh.triangles;

            for (int i = 0; i < trigs.Length; i += 3)
            {

                Vector3 avg = (normals[trigs[i]] + normals[trigs[i + 1]] + normals[trigs[i + 2]]) / 3;
                normals[trigs[i]] = avg;
                normals[trigs[i + 1]] = avg;
                normals[trigs[i + 2]] = avg;

            }

            return normals;

        }

        public void Start()
        {
            balls = GetComponentsInChildren<MetaBall>();

            meshFilter = GetComponent<MeshFilter>();

            mesh = meshFilter.sharedMesh;

            mesh = new Mesh();
            mesh.name = "MetaBalls " + Random.Range(int.MinValue, int.MaxValue);

            this.GetComponent<MeshFilter>().sharedMesh = mesh;
            this.GetComponent<MeshFilter>().mesh = mesh;

            nextUpdate = Time.time;
            for (int i = 0; i <= balls.Length - 1; i++)
            {
                lastPosBalls.Add(balls[i].transform.position);
            }

            lastResolutionX = resolutionX;
            lastResolutionY = resolutionY;
            lastResolutionZ = resolutionZ;
        }

        private void Awake()
        {
            Start();
        }

        private void OnDrawGizmos()
        {

            Vector3 scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(gameObject.transform.position, scale);

            Gizmos.color = Color.red;
            Vector3 safeScale = new Vector3(scale.x - safeZone, scale.y - safeZone, scale.z - safeZone);
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
            if (Time.time >= nextUpdate)
            {
                if (Application.isPlaying)
                {

                    if (CheckForBallsPos() || (lastsafeZone != safeZone) || (lastthreshold != threshold) || (lastscale != transform.localScale) || CheckForChangedResolution())
                    {
                        if (grid == null || (lastscale != transform.localScale))
                        {
                            this.grid = new CubeGrid(this, this.computeShader);
                        }


                        this.grid.evaluateAll(balls);

                        GetComponent<MeshRenderer>().material = material;

                        mesh.Clear();
                        mesh.vertices = this.grid.vertices.ToArray();
                        mesh.triangles = this.grid.getTriangles();
                        meshFilter.mesh = mesh;

                        if (this.calculateNormals)
                        {
                            mesh.RecalculateNormals();
                        }

                        lastsafeZone = safeZone;
                        lastthreshold = threshold;
                        lastscale = transform.localScale;
                    }

                }
                else
                {
                    balls = GetComponentsInChildren<MetaBall>();
                    if (balls == null)
                        return;

                    GetComponent<MeshRenderer>().sharedMaterial = material;

                    this.grid = new CubeGrid(this, this.computeShader);

                    this.grid.evaluateAll(balls);

                    mesh.Clear();
                    mesh.vertices = this.grid.vertices.ToArray();
                    mesh.triangles = this.grid.getTriangles();
                    meshFilter.sharedMesh = mesh;

                    if (this.calculateNormals)
                    {
                        mesh.RecalculateNormals();
                    }


                }

                nextUpdate = Time.time + updateDelay;

            }
        }
    }
}