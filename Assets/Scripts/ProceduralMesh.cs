using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour {

    public Camera camera;
    public int xSize, ySize;
    public int numVertsCircle = 42;
    public int numVertsBorder = 8;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] tris;// = new int[(numVerts * 3)];
    private Mesh mesh;

    private Vector3 bottomLeftViewport;
    private Vector3 topRightViewport;
    private Vector3 centerPointViewport;
    public GameObject spherePrefab;
    private float viewportWidth;
    private float viewportHeight;
    private float xStep; //how much to increment every x vertex
    private float yStep; //how much to increment every y vertex
    private float scaler; //scale the vertex sphere so that they fit on the screen
    private float radius; //half the width of the viewport

    private void Awake()
    {
        if(camera == null)
        {
            camera = FindObjectOfType<Camera>();
        }
        bottomLeftViewport = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        topRightViewport = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        centerPointViewport = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera.nearClipPlane));
        viewportWidth = Mathf.Abs(topRightViewport.x - bottomLeftViewport.x);
        radius = viewportWidth * 0.5f;
        viewportHeight = Mathf.Abs(topRightViewport.y - bottomLeftViewport.y);
        xStep = viewportWidth / (float)xSize;
        yStep = viewportHeight / (float)ySize;
        scaler = xStep * 0.25f;
        spherePrefab.transform.localScale = new Vector3(scaler, scaler, scaler);
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[numVertsCircle + numVertsBorder];
        uvs = new Vector2[numVertsCircle + numVertsBorder];
        tris = new int[(numVertsCircle + numVertsBorder) * 3];
        // The first vert is in the center of the triangle  
       // vertices[0] = centerPointViewport;
       // uvs[0] = new Vector2(0.5f, 0.5f);
        float angle = 360.0f / (float)(numVertsCircle - 1);

        WaitForSeconds wait = new WaitForSeconds(0.05f);
        /* vertices = new Vector3[(xSize + 1) * (ySize + 1)];
         for (int i = 0, y = 0; y <= ySize; y++)
         {
             for (int x = 0; x <= xSize; x++, i++)
             {
                 vertices[i] = bottomLeftViewport + new Vector3(x * xStep, y * yStep, 0.5f*scaler);
                 Instantiate(spherePrefab, vertices[i], Quaternion.identity);
                 yield return wait;
             }
         }
         mesh.vertices = vertices;*/
        vertices[0] = bottomLeftViewport;
        vertices[1] = bottomLeftViewport + new Vector3(viewportWidth*0.5f, 0, 0);
        vertices[2] = bottomLeftViewport + new Vector3(viewportWidth, 0, 0);
        vertices[3] = bottomLeftViewport + new Vector3(0, viewportHeight * 0.5f, 0);
        vertices[4] = topRightViewport - new Vector3(0, viewportHeight * 0.5f, 0);
        vertices[5] = topRightViewport - new Vector3(viewportHeight, 0, 0);
        vertices[6] = topRightViewport - new Vector3(viewportWidth * 0.5f, 0, 0);
        vertices[7] = topRightViewport;

       
        for (int i = 0; i < 8; ++i)
        {
            Instantiate(spherePrefab, vertices[i], Quaternion.identity);
        }
        Instantiate(spherePrefab, centerPointViewport, Quaternion.identity);
        for (int i = 8; i < vertices.Length; ++i)
        {
            Vector3 qwert = Quaternion.AngleAxis(angle * (float)(i - 1), Vector3.back) * new Vector3(0,radius,0);
            qwert.z = centerPointViewport.z;
            vertices[i] = qwert;
            float normedHorizontal = (vertices[i].x + 1.0f) * 0.5f;
            float normedVertical = (vertices[i].x + 1.0f) * 0.5f;
            uvs[i] = new Vector2(normedHorizontal, normedVertical);
            Instantiate(spherePrefab, vertices[i], Quaternion.identity);
            yield return wait;
        }
        int index = 0;
        int numTris = 0;
        //for each vertex
        for (int i = 8; i + 2 < vertices.Length; ++i)
        {
            //if the vertex is in the lower left of the circle
            if(vertices[i].x < centerPointViewport.x && vertices[i].y < centerPointViewport.y)
            {
                index = numTris * 3;
                tris[index + 0] = 0; //bottom left border vertex
                tris[index + 1] = i;
                if (vertices[i + 1].x < centerPointViewport.x && vertices[i + 1].y < centerPointViewport.y)
                {
                    tris[index + 2] = i + 1;
                }
                else
                {
                    tris[index + 2] = i - 1;
                }
                
            }
            else if(vertices[i].x < centerPointViewport.x && vertices[i].y >= centerPointViewport.y)
            {
                index = numTris * 3;
                tris[index + 0] = 5; //top left border vertex
                tris[index + 1] = i ;
                tris[index + 2] = i + 1;
            }
            else if (vertices[i].x >= centerPointViewport.x && vertices[i].y < centerPointViewport.y)
            {
                index = numTris * 3;
                tris[index + 0] = 2; //bottom right border vertex
                tris[index + 1] = i;
                tris[index + 2] = i + 1;
            }
            else
            {
                index = numTris * 3;
                tris[index + 0] = 7; //top right border vertex
                tris[index + 1] = i;
                tris[index + 2] = i + 1;
            }
            numTris++;
            // int index = i * 3;
            // tris[index + 0] = 0;
            // tris[index + 1] = i + 1;
            // tris[index + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = tris;
        

    }

}
