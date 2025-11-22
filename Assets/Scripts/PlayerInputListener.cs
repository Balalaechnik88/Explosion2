using System;
using UnityEngine;

public class PlayerInputListener : MonoBehaviour
{
    public event Action<Vector3> PrimaryActionPerformed;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PrimaryActionPerformed?.Invoke(Input.mousePosition);
        }
    }
}
