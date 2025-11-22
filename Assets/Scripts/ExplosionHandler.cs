using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    [SerializeField] private float _baseForce = 100f;
    [SerializeField] private float _baseRadius = 5f;

    public float BaseForce => _baseForce;
    public float BaseRadius => _baseRadius;

    public void ApplyExplosion(Vector3 center, IEnumerable<Rigidbody> targets, float force, float radius)
    {
        foreach (Rigidbody rigidBody in targets)
        {
            if (rigidBody != null)
                rigidBody.AddExplosionForce(force, center, radius);
        }
    }
}