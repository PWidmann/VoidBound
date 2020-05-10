using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Material material;
    public BlockType[] blocktypes;

    MapGenerator mapGenerator;

    private void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        mapGenerator = new MapGenerator(this);
    }

    public byte GetVoxel(Vector3 pos)
    {
        if (!isVoxelInWorld(pos))
            return 0;

        if (pos.y == 0)
        {
            return 1; // Default Block
        }
        else
        {
            return 2; // Stone Block
        }
    }

    bool isVoxelInWorld(Vector3 pos)
    {
        if (pos.x >= 0 && pos.x < VoxelData.WorldWidth && pos.y >= 0 && pos.y < VoxelData.WorldHeight && pos.z >= 0 && pos.z < VoxelData.WorldWidth)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForVoxel(Vector3 pos)
    {

        if (pos.y < 0 || pos.y > VoxelData.WorldHeight)
            return false;

        //if (mapGenerator.isVoxelMapPopulated)
        //    return blocktypes[chunks[thisChunk.x, thisChunk.z].GetVoxelFromGlobalVector3(pos)].isSolid;

        return blocktypes[GetVoxel(pos)].isSolid;
    }
}



[System.Serializable]
public class BlockType
{
    public string blockName;
    public bool isSolid;

    [Header("Texture Values")]
    public int backFaceTexture;
    public int frontFaceTexture;
    public int topFaceTexture;
    public int bottomFaceTexture;
    public int leftFaceTexture;
    public int rightFaceTexture;


    // Back, Front, Top, Bottom, Left, Right

    public int GetTextureID(int faceIndex)
    {
        switch (faceIndex)
        {
            case 0:
                return backFaceTexture;
            case 1:
                return frontFaceTexture;
            case 2:
                return topFaceTexture;
            case 3:
                return bottomFaceTexture;
            case 4:
                return leftFaceTexture;
            case 5:
                return rightFaceTexture;
            default:
                Debug.Log("Error in GetTextureID; invalid face index");
                return 0;
        }
    }
}
