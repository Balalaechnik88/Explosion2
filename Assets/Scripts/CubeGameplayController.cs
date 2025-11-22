using System.Collections.Generic;
using UnityEngine;

public class CubeGameplayController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CubeSpawner _spawner;
    [SerializeField] private ExplosionHandler _explosionHandler;

    [Header("Explosion Settings")]
    [SerializeField] private float _minCubeSize = 0.01f;
    [SerializeField] private float _minExplosionForce = 30f;
    [SerializeField] private float _maxExplosionForce = 1000f;
    [SerializeField] private float _minExplosionRadius = 1f;
    [SerializeField] private float _maxExplosionRadius = 50f;

    private readonly List<Cube> _activeCubes = new List<Cube>();

    private void Awake()
    {
        if (_spawner == null)
            Debug.LogError("CubeGameplayController: CubeSpawner не назначен!");

        if (_explosionHandler == null)
            Debug.LogError("CubeGameplayController: ExplosionHandler не назначен!");
    }

    private void Start()
    {
        var initialCubes = _spawner.SpawnInitialCubes();
        RegisterCubes(initialCubes);
    }

    public void OnCubeHit(Cube cube)
    {
        if (cube == null || !_activeCubes.Contains(cube))
            return;

        if (cube.ShouldSplit())
            SplitCube(cube);
        else
            ExplodeAndDestroyCube(cube);
    }

    private void RegisterCubes(IEnumerable<Cube> cubes)
    {
        foreach (var cube in cubes)
            RegisterCube(cube);
    }

    private void RegisterCube(Cube cube)
    {
        _activeCubes.Add(cube);
        cube.Split += SplitCube;
        cube.Destroyed += ExplodeAndDestroyCube;
    }

    private void UnregisterCube(Cube cube)
    {
        cube.Split -= SplitCube;
        cube.Destroyed -= ExplodeAndDestroyCube;
        _activeCubes.Remove(cube);
    }

    private void SplitCube(Cube cube)
    {
        UnregisterCube(cube);

        Vector3 newScale = cube.transform.localScale / cube.DecreaseValue;
        float newChance = cube.SplitChance / Mathf.Max(cube.DecreaseChance, 1);

        var spawned = _spawner.SpawnCubesAfterSplit(cube.transform.position, newScale, newChance);
        RegisterCubes(spawned);

        Destroy(cube.gameObject);
    }

    private void ExplodeAndDestroyCube(Cube cube)
    {
        List<Rigidbody> targets = new List<Rigidbody>();

        foreach (var other in _activeCubes)
        {
            if (other != cube && other.CubePhysicsComponent != null)
                targets.Add(other.CubePhysicsComponent);
        }

        float size = Mathf.Max(cube.transform.localScale.x, _minCubeSize);
        float factor = 1f / size;

        float force = Mathf.Clamp(
            _explosionHandler.BaseForce * factor,
            _minExplosionForce,
            _maxExplosionForce
        );

        float radius = Mathf.Clamp(
            _explosionHandler.BaseRadius * factor,
            _minExplosionRadius,
            _maxExplosionRadius
        );

        if (targets.Count > 0)
            _explosionHandler.ApplyExplosion(cube.transform.position, targets, force, radius);

        UnregisterCube(cube);
        Destroy(cube.gameObject);
    }
}