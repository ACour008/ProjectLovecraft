using UnityEngine;

public static class GizmoCircleDrawer
{
    public static void Draw(Vector3 center, float radius, Color color)
    {
        Gizmos.color = color;
        int segments = 50; // Number of segments to approximate the circle
        float angle = 0f;

        Vector3 lastPoint = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        Vector3 nextPoint = Vector3.zero;

        for (int i = 1; i <= segments; i++)
        {
            angle += (2 * Mathf.PI) / segments;
            nextPoint.x = center.x + Mathf.Cos(angle) * radius;
            nextPoint.y = center.y + Mathf.Sin(angle) * radius;
            nextPoint.z = center.z;

            Gizmos.DrawLine(lastPoint, nextPoint);

            lastPoint = nextPoint;
        }
    }
}