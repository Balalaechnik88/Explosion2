using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Raycaster : MonoBehaviour
{
    [SerializeField] private PlayerInputListener _inputListener;

    private Camera _camera;

    public event Action<Cube> OnCubeHit;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        if (_inputListener == null)
            Debug.LogError("Raycaster: PlayerInputListener не назначен!");
    }

    private void OnEnable()
    {
        if (_inputListener != null)
            _inputListener.PrimaryActionPerformed += HandleInputAction;
    }

    private void OnDisable()
    {
        if (_inputListener != null)
            _inputListener.PrimaryActionPerformed -= HandleInputAction;
    }

    private void HandleInputAction(Vector3 screenPos)
    {
        Ray ray = _camera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Cube cube))
                OnCubeHit?.Invoke(cube);
        }
    }
}