using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelData
{
    // Basically a lookup table class

    public static readonly int WorldWidth = 50;
    public static readonly int WorldHeight = 5;

    public static readonly int textureAtlasSizeInBlocks = 8;
    public static float NormalizedBlockTextureSize { get { return 1f / (float)textureAtlasSizeInBlocks; } }

    public static readonly Vector3[] voxelVerts = new Vector3[8]
    { 
        // 8 vertices for a cube
        new Vector3(0.0f, 0.0f, 0.0f), // Index 0 in reference
        new Vector3(1.0f, 0.0f, 0.0f), // Index 1 in reference
        new Vector3(1.0f, 1.0f, 0.0f), // Index 2 in reference
        new Vector3(0.0f, 1.0f, 0.0f), // Index 3 in reference
        new Vector3(0.0f, 0.0f, 1.0f), // Index 4 in reference
        new Vector3(1.0f, 0.0f, 1.0f), // Index 5 in reference
        new Vector3(1.0f, 1.0f, 1.0f), // Index 6 in reference
        new Vector3(0.0f, 1.0f, 1.0f)  // Index 7 in reference

    };

    // For looking if the face is visible to the player
    public static readonly Vector3[] faceChecks = new Vector3[6]
    {
        new Vector3(0.0f, 0.0f, -1.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, -1.0f, 0.0f),
        new Vector3(-1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f)
    };

    public static readonly int[,] voxelTris = new int[6, 4] // 6 vertices each face, 6 faces
    {
        // Shortened for performance 

        { 0, 3, 1, 2}, // Back Face. Original:  { 0, 3, 1, 1, 3, 2}
        { 5, 6, 4, 7}, // Front Face. Original: { 5, 6, 4, 4, 6, 7}
        { 3, 7, 2, 6}, // Top Face. Original:   { 3, 7, 2, 2, 7, 6}
        { 1, 5, 0, 4}, // Bottom Face. Original:{ 1, 5, 0, 0, 5, 4}
        { 4, 7, 0, 3}, // Left Face. Original:  { 4, 7, 0, 0, 7, 3}
        { 1, 2, 5, 6}  // Right Face. Original: { 1, 2, 5, 5, 2, 6}
    };

    // Lookup table for texture each face
    public static readonly Vector2[] voxelUVs = new Vector2[4]
    {
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 1.0f),
    };
}
