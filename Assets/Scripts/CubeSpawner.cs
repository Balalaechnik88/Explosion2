using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private Cube _cubePrefab;

    [Header("Initial spawn")]
    [SerializeField] private int _initialMinCount = 1;
    [SerializeField] private int _initialMaxCount = 3;
    [SerializeField] private Vector3 _initialPosition = Vector3.zero;
    [SerializeField] private float _initialSpacing = 0.1f;
    [SerializeField] private Vector3 _initialScale = Vector3.one;
    [SerializeField] private float _initialSplitChance = 1.0f;

    [Header("Split spawn")]
    [SerializeField] private int _minCubesOnSplit = 2;
    [SerializeField] private int _maxCubesOnSplit = 6;
    [SerializeField] private Vector3 _randomOffsetRange = new Vector3(0.2f, 0.2f, 0.2f);

    private void Awake()
    {
        if (_cubePrefab == null)
            Debug.LogError("CubeSpawner: Cube Prefab не назначен!");
    }

    public List<Cube> SpawnInitialCubes()
    {
        int count = Random.Range(_initialMinCount, _initialMaxCount + 1);
        var spawned = new List<Cube>(count);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = _initialPosition + new Vector3(i * _initialSpacing, 0, 0);
            Cube cube = SpawnCube(pos, _initialScale, _initialSplitChance);

            if (cube != null)
                spawned.Add(cube);
        }

        return spawned;
    }

    public List<Cube> SpawnCubesAfterSplit(Vector3 position, Vector3 newScale, float newSplitChance)
    {
        int count = Random.Range(_minCubesOnSplit, _maxCubesOnSplit + 1);
        var spawned = new List<Cube>(count);

        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-_randomOffsetRange.x, _randomOffsetRange.x),
                Random.Range(-_randomOffsetRange.y, _randomOffsetRange.y),
                Random.Range(-_randomOffsetRange.z, _randomOffsetRange.z)
            );

            Cube cube = SpawnCube(position + offset, newScale, newSplitChance);

            if (cube != null)
                spawned.Add(cube);
        }

        return spawned;
    }

    public Cube SpawnCube(Vector3 position, Vector3 scale, float splitChance)
    {
        if (_cubePrefab == null)
            return null;

        Cube cube = Instantiate(_cubePrefab, position, Random.rotation);
        cube.transform.localScale = scale;
        cube.Initialize(splitChance);

        if (cube.CubeRenderer != null)
        {
            cube.CubeRenderer.material.color =
                new Color(Random.value, Random.value, Random.value);
        }

        return cube;
    }
}