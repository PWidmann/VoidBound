using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    GameObject worldObject;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    Mesh worldMesh = new Mesh();

    int vertexIndex = 0;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    byte[,,] voxelMap = new byte[VoxelData.WorldWidth, VoxelData.WorldHeight, VoxelData.WorldWidth];

    World world;

    public bool isVoxelMapPopulated = false;

    public MapGenerator(World _world)
    {
        world = _world;
        worldObject = new GameObject();
        meshFilter = worldObject.AddComponent<MeshFilter>();
        meshRenderer = worldObject.AddComponent<MeshRenderer>();
        worldObject.name = "Terrain";
        meshRenderer.material = world.material;

        PopulateVoxelMap();
        CreateMeshData();
        CreateMesh();
    }

    void PopulateVoxelMap()
    {
        for (int y = 0; y < VoxelData.WorldHeight; y++)
        {
            for (int x = 0; x < VoxelData.WorldWidth; x++)
            {
                for (int z = 0; z < VoxelData.WorldWidth; z++)
                {
                    voxelMap[x, y, z] = world.GetVoxel(new Vector3(x, y, z));
                }
            }
        }

        isVoxelMapPopulated = true;
    }

    void AddVoxelDataToWorld(Vector3 pos)
    {
        for (int p = 0; p < 6; p++) // Loop through faces
        {
            if (!CheckVoxel(pos + VoxelData.faceChecks[p])) // Only draw face if there is no voxel next to that face
            {
                byte blockID = voxelMap[(int)pos.x, (int)pos.y, (int)pos.z];

                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 0]]);
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 1]]);
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 2]]);
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 3]]);

                AddTexture(world.blocktypes[blockID].GetTextureID(p));

                //Create triangles from simplified triangle data
                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 3);

                vertexIndex += 4;
            }
        }
    }

    bool IsVoxelInWorld(int x, int y, int z) // Local coord in chunk
    {
        if (x < 0 || x > VoxelData.WorldWidth - 1 || y < 0 || y > VoxelData.WorldHeight - 1 || z < 0 || z > VoxelData.WorldWidth - 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Checks if there is a voxel at the position of this chunk
    bool CheckVoxel(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        if (!IsVoxelInWorld(x, y, z))
            return world.blocktypes[world.GetVoxel(pos)].isSolid;

        return world.blocktypes[voxelMap[x, y, z]].isSolid;
    }

    public byte GetVoxelFromGlobalVector3(Vector3 pos)
    {
        int xCheck = Mathf.FloorToInt(pos.x);
        int yCheck = Mathf.FloorToInt(pos.y);
        int zCheck = Mathf.FloorToInt(pos.z);

        return voxelMap[xCheck, yCheck, zCheck];
    }

    void CreateMeshData()
    {
        for (int y = 0; y < VoxelData.WorldHeight; y++)
        {
            for (int x = 0; x < VoxelData.WorldWidth; x++)
            {
                for (int z = 0; z < VoxelData.WorldWidth; z++)
                {
                    AddVoxelDataToWorld(new Vector3(x, y, z));
                }
            }
        }
    }

    void CreateMesh()
    {
        
        worldMesh.vertices = vertices.ToArray();
        worldMesh.triangles = triangles.ToArray();
        worldMesh.uv = uvs.ToArray();
        worldMesh.RecalculateNormals();

        meshFilter.mesh = worldMesh;
    }

    void AddTexture(int textureID)
    {
        float y = textureID / VoxelData.textureAtlasSizeInBlocks;
        float x = textureID - (y * VoxelData.textureAtlasSizeInBlocks);

        x *= VoxelData.NormalizedBlockTextureSize;
        y *= VoxelData.NormalizedBlockTextureSize;

        y = 1f - y - VoxelData.NormalizedBlockTextureSize;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + VoxelData.NormalizedBlockTextureSize));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));
    }
}

public class ChunkCoord
{
    public int x;
    public int z;

    public ChunkCoord(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}
