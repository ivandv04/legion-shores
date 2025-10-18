using GenerationTools;
using WP = GenerationTools.WorldPopulator;
using SeedTools;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Random = System.Random;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;

public class PerlinManager : MonoBehaviour
{
    private byte[,] constr;

    /*
     * GIANT PROCEDURAL SETTINGS LIST
     * 
     * Set these to something and the world might look decent.
     */
    public Tilemap terrMap;

    public int mapSize;
    public int mapSeed;

    public Tile cloudTile;
    public Tile upperCloudTile;
    public Tile oceanTile;
    public Tile swampTile;
    public Tile wetlandsTile;
    public Tile plainsTile;
    public Tile mountainsTile;
    public Tile forestTile;
    public Tile shallowTile;
    public Tile desertTile;
    public Tile dryForestTile;

    public int elevationLayers;
    public float genDensity;
    public int fbOctaves;
    public float fbFrequency; 
    public float fbDamping;

    /*
     * GENERATION EXECUTORS
     */

    [ContextMenu("Sample Text")]
    void SampleText()
    {
        constr = EnhancedConstructor.Export(mapSize, mapSeed,
            (c) => c.BuildTopographySimple().RawTerrainConvert());

        string text = "";

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                text += constr[x, y].ToString();
            }
            text += "\n";
        }

        string path = "output.txt";

        System.IO.File.WriteAllText(path, text);
    }

    
     
    [ContextMenu("Clear Map")]
    void ClearMap() { terrMap.ClearAllTiles(); }

    [ContextMenu("Build Map (given seed)")]
    void ConstructMap()
    {
        // run terrain algorithm via WorldConstructor
        var watch = Stopwatch.StartNew();
        constr = EnhancedConstructor.Export(mapSize, mapSeed,
           (c) => c
           .BuildTopography(elevationLayers,genDensity,fbOctaves,fbFrequency,fbDamping)
           .RawTerrainConvert());
        // clear map and paint
        AssetDatabase.Refresh();
        ClearMap();
        Tile[] types = MapUtil.GetTileSet(
            cloudTile, upperCloudTile,
            oceanTile, swampTile, wetlandsTile, shallowTile,
            plainsTile, mountainsTile, forestTile,
            desertTile, dryForestTile);
        for (int x = 0; x < mapSize; x++)
            for (int y = 0; y < mapSize; y++)
            {
                terrMap.SetTile(new(x, y), types[constr[x, y]]);
            }
        watch.Stop();
        // log time
        Debug.Log($"Terr exe time: {watch.ElapsedMilliseconds} ms");
    }
    
    
}