using UnityEngine;

public class CubeHitHandler : MonoBehaviour
{
    [SerializeField] private Raycaster _raycaster;
    [SerializeField] private CubeGameplayController _gameplayController;

    private void Awake()
    {
        if (_raycaster == null)
            Debug.LogError("CubeHitHandler: Raycaster не назначен!");

        if (_gameplayController == null)
            Debug.LogError("CubeHitHandler: CubeGameplayController не назначен!");
    }

    private void OnEnable()
    {
        if (_raycaster != null)
            _raycaster.OnCubeHit += HandleCubeHit;
    }

    private void OnDisable()
    {
        if (_raycaster != null)
            _raycaster.OnCubeHit -= HandleCubeHit;
    }

    private void HandleCubeHit(Cube cube)
    {
        _gameplayController?.OnCubeHit(cube);
    }
}