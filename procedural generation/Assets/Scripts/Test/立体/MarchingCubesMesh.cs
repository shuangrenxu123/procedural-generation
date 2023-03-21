using SimplexNoise;
using UnityEngine;
public class MarchingCubesMesh : ProceduralMesh
{
    public Vector3 size = new Vector3(1, 1, 1);
    public bool lerp = false;
    public Vector3Int MapSize = new Vector3Int(10, 10, 10);
    [Header("地形")]
    public float scale = 0.1f;
    public float offset = 100;
    public float threshold = 0.5f;
    public Vector3 halfSize;
    public ProceduralMeshPart main;
    public int seed = 0;
    private MarchingCube[] cubes;
    private void OnValidate()
    {
        Generate();
    }

    protected override void Generate()
    {
        base.Generate();
        int totalCount = MapSize.x * MapSize.y * MapSize.z;
        halfSize = size * 0.5f;
        main = new ProceduralMeshPart();
        cubes = new MarchingCube[totalCount];
        for (int x = 0; x < MapSize.x; x++)
        {
            for (int y = 0; y < MapSize.y; y++)
            {
                for (int z = 0; z < MapSize.z; z++)
                {
                    int index = x * MapSize.z * MapSize.y + y * MapSize.z + z;
                    cubes[index] = GenerateCube(new Vector3(x * size.x, y * size.y, z * size.z));
                    cubes[index].BuildMesh(main, threshold, lerp);
                }
            }
        }
        main.FillArray();
        mesh.vertices = main.vers;
        mesh.triangles = main.tris;
        mesh.colors = main.color;
        mesh.RecalculateNormals();
    }

    private MarchingCube GenerateCube(Vector3 centerPoint)
    {
        Vector3[] corner;
        float[] values;
        corner = new Vector3[8];
        corner[0] = centerPoint + new Vector3(-1 * halfSize.x, -1 * halfSize.y, 1 * halfSize.z);
        corner[1] = centerPoint + new Vector3(1 * halfSize.x, -1 * halfSize.y, 1 * halfSize.z);
        corner[2] = centerPoint + new Vector3(1 * halfSize.x, -1 * halfSize.y, -1 * halfSize.z);
        corner[3] = centerPoint + new Vector3(-1 * halfSize.x, -1 * halfSize.y, -1 * halfSize.z);
        corner[4] = centerPoint + new Vector3(-1 * halfSize.x, 1 * halfSize.y, 1 * halfSize.z);
        corner[5] = centerPoint + new Vector3(1 * halfSize.x, 1 * halfSize.y, 1 * halfSize.z);
        corner[6] = centerPoint + new Vector3(1 * halfSize.x, 1 * halfSize.y, -1 * halfSize.z);
        corner[7] = centerPoint + new Vector3(-1 * halfSize.x, 1 * halfSize.y, -1 * halfSize.z);
        values = new float[8];
        for (int i = 0; i < 8; i++)
        {
            values[i] = Noise.CalcPixel3D((int)corner[i].x, (int)corner[i].y, (int)corner[i].z, scale);
        }
        return new MarchingCube(centerPoint, corner, values);
    }
}
