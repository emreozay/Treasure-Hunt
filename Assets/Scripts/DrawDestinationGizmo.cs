using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDestinationGizmo : MonoBehaviour
{
    [SerializeField]
    private Transform falan;
    private Vector3 dest;
    private Vector3 dest2;
    private Vector3 dest3;

    private void Start()
    {
        print("Sin: " + Mathf.Sin(0) + "   Cos: " + Mathf.Cos(0));
    }

    void Update()
    {
        //dest = falan.position - transform.position;
        Dene();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(dest, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(dest2, 0.5f);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(dest3, 0.5f);
    }

    private void Dene()
    {
        Vector3 positionDifference = falan.position - transform.position;

        dest = transform.position - (positionDifference.normalized * 3f);
        dest2 = dest;

        dest = new Vector3(Mathf.Clamp(dest.x, -15f, 15f), Mathf.Clamp(dest.y, -20f, 20f), 0);
        dest3 = dest;

        dest.x = Mathf.Cos(Random.Range(180f, 360f) / (180f / Mathf.PI)) * 2f + dest.x;
        dest.y = Mathf.Sin(Random.Range(180f, 360f) / (180f / Mathf.PI)) * 2f + dest.y;
        print("1: " + dest + " - 2: " + dest2 + " - 3: " + dest3);
    }
}
