using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds some state for the guard.
/// </summary>
public class Guard : MonoBehaviour
{
    public float Radius { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Collider2D coll2D = GetComponent<Collider2D>();
        if (coll2D != null)
        {
            setRadius2D();
        } else
        {
            setRadius3D();
        }
    }

    /// <summary>
    /// Initializes the radius of the guard given that it has a 3D collider
    /// </summary>
    private void setRadius3D()
    {
        SphereCollider collSphere = GetComponent<SphereCollider>();
        if (collSphere != null)
        {
            Radius = collSphere.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        }
        else
        {
            Vector2 halfSizes = GetComponent<Collider>().bounds.extents;
            Radius = halfSizes.magnitude;
        }
    }

    /// <summary>
    /// Initializes the radius of the guard given that it has a 2D collider
    /// </summary>
    private void setRadius2D()
    {
        CircleCollider2D collCircle = GetComponent<CircleCollider2D>();
        if (collCircle != null)
        {
            Radius = collCircle.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
        } else
        {
            Vector2 halfSizes = GetComponent<Collider2D>().bounds.extents;
            Radius = halfSizes.magnitude;
        }
    }
}
