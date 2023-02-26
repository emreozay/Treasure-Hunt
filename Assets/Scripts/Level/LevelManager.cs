using NavMeshPlus.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameLevel gameLevel;
    [SerializeField]
    private GameObject mapPrefab;
    [SerializeField]
    private GameObject mapBoundaries;

    private GameObject environmentParent;
    private GameObject holeParent;
    private GameObject boostParent;

    private List<GameLevel> gameLevels = new List<GameLevel>();

    [SerializeField]
    private NavMeshSurface navMeshSurface;

    [SerializeField]
    private EnemyMovement[] enemyMovements;

    [SerializeField]
    private List<SaveLevelPrefab> prefabList = new List<SaveLevelPrefab>();

    [SerializeField]
    private AILevel enemy1;
    [SerializeField]
    private AILevel enemy2;
    [SerializeField]
    private AILevel enemy3;

    private GameObject backgroundMap;
    private GameObject boundaries;

    private Vector2 mapSizeMultiplier = Vector2.one;

    private Vector2 defaulMapBorder = new Vector2(15f, 20f);

    private bool newLevel = false;
    private int level = 1;

    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameLevels = Resources.LoadAll<GameLevel>("Levels").ToList();
        gameLevels = gameLevels.OrderBy(w => w.levelIndex).ToList();

        level = PlayerPrefs.GetInt("Level", 1);

        environmentParent = GameObject.Find("Environment");
        holeParent = GameObject.Find("Holes");
        boostParent = GameObject.Find("Boosts");

        LoadCurrentLevel(false);
    }

    public void SaveLevel()
    {
        if (gameLevel == null)
        {
            Debug.LogError("No Game Level Object!");
            return;
        }

        gameLevel.ClearLevelObjectList();

        SaveLevelObject[] levelObjects = FindObjectsOfType<SaveLevelObject>();
        foreach (SaveLevelObject levelObject in levelObjects)
        {
            gameLevel.AddLevelObjectInfo(levelObject);
        }

        gameLevel.mapSizeMultiplier = mapSizeMultiplier;
        gameLevel.enemy1 = enemy1;
        gameLevel.enemy2 = enemy2;
        gameLevel.enemy3 = enemy3;

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameLevel);
#endif
    }

    public void LoadCurrentLevel(GameLevel level, bool loadAsSandbox = false)
    {
        if (level == null)
        {
            Debug.LogError("No Game Level Object!");
            return;
        }

        ClearLevel();

        newLevel = true;
        LoadMap();

        foreach (LevelObjectInfo levelObject in level.levelObjectList)
        {
            GameObject prefab = null;
            foreach (SaveLevelPrefab levelPrefab in prefabList)
            {
                if (levelObject.type == levelPrefab.type)
                {
                    prefab = levelPrefab.prefab;
                    break;
                }
            }

            if (prefab == null)
            {
                Debug.LogWarning("Couldn't find prefab of type: " + levelObject.type);
                continue;
            }

            GameObject newInstance = Instantiate(prefab); //You can use object pooling for this

            newInstance.transform.position = levelObject.position;
        }
    }

    public void LoadCurrentLevel(bool isEditor)
    {
        if (!isEditor)
            gameLevel = gameLevels[level - 1];

        if (gameLevel == null)
        {
            Debug.LogError("No Game Level Object!");
            return;
        }

        ClearLevel();
        newLevel = true;
        LoadMap();

        foreach (LevelObjectInfo levelObject in gameLevel.levelObjectList)
        {
            GameObject prefab = null;
            foreach (SaveLevelPrefab levelPrefab in prefabList)
            {
                if (levelObject.type == levelPrefab.type)
                {
                    prefab = levelPrefab.prefab;
                    break;
                }
            }

            if (prefab == null)
            {
                Debug.LogWarning("Couldn't find prefab of type: " + levelObject.type);
                continue;
            }

            GameObject newInstance = Instantiate(prefab); //You can use object pooling for this
            int levelObjectTypeIndex = (int)levelObject.type;
            SetParent(newInstance.transform, levelObjectTypeIndex);

            newInstance.transform.position = levelObject.position;
        }

        enemy1 = gameLevel.enemy1;
        enemy2 = gameLevel.enemy2;
        enemy3 = gameLevel.enemy3;
        SetEnemyAIs();

        navMeshSurface?.BuildNavMesh();
    }

    public void LoadLevel(GameLevel level)
    {
        this.gameLevel = level;
        LoadCurrentLevel(false); //bak buna
    }

    public Texture GetPrefabTexture(int textureIndex)
    {
        return prefabList[textureIndex].texture;
    }

    public int PrefabLength()
    {
        return prefabList.Count;
    }

    public void CreateObject(int objectIndex)
    {
        Vector3 newPosition = GetRandomPosition();

        GameObject newObject = Instantiate(prefabList[objectIndex].prefab, newPosition, Quaternion.identity);

        SetParent(newObject.transform, objectIndex);
    }

    public Vector3 GetRandomPosition()
    {
        Vector2 newMapBorder = defaulMapBorder * mapSizeMultiplier;
        Vector3 newPosition = new Vector3(Random.Range(-newMapBorder.x, newMapBorder.x), Random.Range(-newMapBorder.y, newMapBorder.y), 0);

        return newPosition;
    }

    public Vector2 SetMapSize(Vector2 sizeMultiplier)
    {
        if (newLevel)
        {
            newLevel = false;
            return mapSizeMultiplier;
        }

        mapSizeMultiplier = sizeMultiplier;

        if (backgroundMap != null)
            backgroundMap.transform.localScale = mapSizeMultiplier;

        if (boundaries != null)
            boundaries.transform.localScale = mapSizeMultiplier;

        return mapSizeMultiplier;
    }

    public void LoadMap()
    {
        mapSizeMultiplier = gameLevel.mapSizeMultiplier;
        backgroundMap = Instantiate(mapPrefab);
        backgroundMap.transform.localScale = mapSizeMultiplier;

        boundaries = Instantiate(mapBoundaries);
        boundaries.transform.localScale = mapSizeMultiplier;
    }

    public void ClearLevel()
    {
        SaveLevelObject[] levelObjects = FindObjectsOfType<SaveLevelObject>();
        foreach (SaveLevelObject levelObject in levelObjects)
        {
            if (levelObject == null)
                continue;

            if (Application.isEditor)
                DestroyImmediate(levelObject.gameObject);
            else
                Destroy(levelObject.gameObject);
        }

        GameObject[] mapObjects = GameObject.FindGameObjectsWithTag("Map");
        GameObject[] boundaryObjects = GameObject.FindGameObjectsWithTag("Boundary");

        for (int i = 0; i < mapObjects.Length; i++)
        {
            if (Application.isEditor)
                DestroyImmediate(mapObjects[i]);
            else
                Destroy(mapObjects[i]);
        }

        for (int i = 0; i < boundaryObjects.Length; i++)
        {
            if (Application.isEditor)
                DestroyImmediate(boundaryObjects[i]);
            else
                Destroy(boundaryObjects[i]);
        }
    }

    public Vector2 GetMapSizeMultiplier()
    {
        return mapSizeMultiplier;
    }

    public Vector2 GetMapSize()
    {
        return mapSizeMultiplier * defaulMapBorder;
    }

    public void NextLevel()
    {
        if (gameLevels.Count > level)
            level++;
    }

    public void CreateNewLevel()
    {
#if UNITY_EDITOR

        gameLevels = Resources.LoadAll<GameLevel>("Levels").ToList();
        int levelIndex = gameLevels.Count + 1;

        GameLevel newLevelAsset = ScriptableObject.CreateInstance<GameLevel>();
        newLevelAsset.levelIndex = levelIndex;

        AssetDatabase.CreateAsset(newLevelAsset, "Assets/Resources/Levels/Level" + levelIndex + ".asset");
        AssetDatabase.SaveAssets();

        gameLevel = newLevelAsset;
        ClearLevel();
        LoadCurrentLevel(true);

        EditorUtility.FocusProjectWindow();
#endif   
    }

    private void SetParent(Transform newObject, int index)
    {
        if (environmentParent == null)
            environmentParent = GameObject.Find("Environment");
        if (holeParent == null)
            holeParent = GameObject.Find("Holes");
        if (boostParent == null)
            boostParent = GameObject.Find("Boosts");

        if (index < 9)
            newObject.SetParent(environmentParent.transform);
        else if (index < 11)
            newObject.SetParent(boostParent.transform);
        else if (index == 11)
            newObject.SetParent(holeParent.transform);
    }

    private void SetEnemyAIs()
    {
        enemyMovements[0].SetAILevel(enemy1);
        enemyMovements[1].SetAILevel(enemy2);
        enemyMovements[2].SetAILevel(enemy3);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("Level", level);
    }
}