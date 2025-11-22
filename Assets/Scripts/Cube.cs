using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private float _splitChance = 1.0f;
    [SerializeField] private int _decreaseValue = 2;
    [SerializeField] private int _decreaseChance = 2;

    public float SplitChance => _splitChance;
    public int DecreaseValue => _decreaseValue;
    public int DecreaseChance => _decreaseChance;

    public event Action<Cube> Split;
    public event Action<Cube> Destroyed;

    public Renderer CubeRenderer { get; private set; }
    public Rigidbody CubePhysicsComponent { get; private set; }

    private void Awake()
    {
        CubeRenderer = GetComponent<Renderer>();
        CubePhysicsComponent = GetComponent<Rigidbody>();
    }

    public void Initialize(float initialSplitChance)
    {
        _splitChance = initialSplitChance;
    }

    public bool ShouldSplit()
    {
        return UnityEngine.Random.value <= _splitChance;
    }

    public void TriggerSplit() => Split?.Invoke(this);
    public void TriggerDestroy() => Destroyed?.Invoke(this);
}