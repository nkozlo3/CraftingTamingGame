using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// This class is responsible for generating the terrain
/// The terrain will be generated in a grid pattern
/// The terrain will be 'infinite'
/// </summary>
public class TerrainGeneration : MonoBehaviour
{
    // shape of the terrain
    // will be a grid

    public int terrainWidth = 1000;
    public int terrainHeight = 1000;

    public int terrainDepth = 6;

    public float offsetX = 100f;
    public float offsetY = 100f;

    public float scale = 20f;

    // tree prefabs
    public GameObject tree1;
    public GameObject tree2;
    public GameObject tree3;

    //ground material
    public Material groundMaterial;

    Terrain terrain;

    private int numberOfTrees;


    private void Start()
    {
        terrain = GetComponent<Terrain>();
        
        terrain.terrainData.treePrototypes = new TreePrototype[3] { new TreePrototype { prefab = tree1 }, new TreePrototype { prefab = tree2 }, new TreePrototype { prefab = tree3 } };

        offsetX = Random.Range(0f, 99999f);
        offsetY = Random.Range(0f, 99999f);

        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    private void Update()
    {
        // if the player wants to generate a new terrain:
        if (Input.GetKeyDown(KeyCode.P))
        {
            offsetX = Random.Range(0f, 99999f);
            offsetY = Random.Range(0f, 99999f);

            terrain.terrainData = GenerateTerrain(terrain.terrainData);
        }
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {

        terrainData.heightmapResolution = terrainWidth + 1;
        terrainData.size = new Vector3(terrainWidth, terrainDepth, terrainHeight);


        terrainData.SetHeights(0, 0, GenerateHeights());

        //terrainData.SetTreeInstances(GenerateTrees(), true);
        GenerateTrees();

        // set the ground material
        terrain.materialTemplate = groundMaterial;

        // generate grass on the terrain
        terrainData.SetDetailLayer(0, 0, 0, GenerateGrass());

        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[terrainWidth, terrainHeight];

        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = 0; y < terrainHeight; y++)
            {
                // use perlin noise to generate the terrain
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / terrainWidth * scale;
        float yCoord = (float)y / terrainWidth * scale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    // generate trees on the terrain
    void GenerateTrees()
    {
        // delete any trees that are already on the terrain
        terrain.terrainData.treeInstances = new TreeInstance[0];

        // generate trees
        numberOfTrees = Random.Range(100, 3500);
        Debug.Log("Number of trees in GenerateTrees: " + numberOfTrees);
        
        // TreeInstance 
        TreeInstance[] trees = new TreeInstance[numberOfTrees];
        terrainWidth = 1000;
        terrainHeight = 1000;

        for (int i = 0; i < numberOfTrees; i++)
        {
            // generate a random position for the tree
            Vector3 position_ = new Vector3(Random.value, 0, Random.value);

            // create a new tree instance
            TreeInstance tree = new()
            {
                // set the prototype index of the tree
                prototypeIndex = Random.Range(0, 3),

                // set the position of the tree
                position = position_,

                // set the width of the tree
                widthScale = Random.Range(1, 3),

                // set the height of the tree
                heightScale = Random.Range(1, 3),

                // set the rotation of the tree
                rotation = Random.Range(0, 360),

                // set the color of the tree
                color = Color.white,

                // set the lightmap color of the tree
                lightmapColor = Color.white
                
            };

            // add the tree to the array
            trees[i] = tree;
            Debug.Log("trees length in GenerateTrees: " + trees.Length);
        }
        terrain.terrainData.SetTreeInstances(trees, true);

        // get terrains collider
        TerrainCollider terrainCollider = GetComponent<TerrainCollider>();
        // tree colliders are not working properly
        // to fix this, we need to restart their colliders
        // I know this is not the best way to do this // but until I find a better way, I will turn off then immediately turn the terrainCollider back on
        terrainCollider.enabled = false;
        terrainCollider.enabled = true;
    }
    // generate grass on terrain using GenerateGrass function
    int[,] GenerateGrass()
    {
        int[,] grass = new int[terrainWidth, terrainHeight];

        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = 0; y < terrainHeight; y++)
            {
                grass[x, y] = Random.Range(0, 10);
            }
        }
        return grass;
    }

}
