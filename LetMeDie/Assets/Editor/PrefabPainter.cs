using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PrefabPainter : EditorWindow
{
    GameObject prefab;
    Transform parent;

    public float checkRadius = 1f;
    public float paintInterval = .5f;

    bool cHeld;
    double lastPaint;

    // Paint-Ziele
    bool allowTerrain = true;
    List<GameObject> paintTargets = new List<GameObject>();

    [MenuItem("Tools/Prefab Painter")]
    static void Open()
    {
        GetWindow<PrefabPainter>("Prefab Painter");
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += SceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= SceneGUI;
    }

    void OnGUI()
    {
        GUILayout.Label("Prefab Settings", EditorStyles.boldLabel);

        parent = (Transform)EditorGUILayout.ObjectField(
            "Optional Parent",
            parent,
            typeof(Transform),
            true);

        prefab = (GameObject)EditorGUILayout.ObjectField(
            "Prefab",
            prefab,
            typeof(GameObject),
            false);

        checkRadius = EditorGUILayout.FloatField("Check Radius", checkRadius);
        paintInterval = EditorGUILayout.FloatField("Paint Interval", paintInterval);

        EditorGUILayout.Space();

        GUILayout.Label("Paint Surfaces", EditorStyles.boldLabel);

        allowTerrain = EditorGUILayout.Toggle("Allow Terrain", allowTerrain);

        int newSize = Mathf.Max(0, EditorGUILayout.IntField("Allowed Objects", paintTargets.Count));

        while (paintTargets.Count < newSize)
            paintTargets.Add(null);

        while (paintTargets.Count > newSize)
            paintTargets.RemoveAt(paintTargets.Count - 1);

        for (int i = 0; i < paintTargets.Count; i++)
        {
            paintTargets[i] = (GameObject)EditorGUILayout.ObjectField(
                $"Object {i}",
                paintTargets[i],
                typeof(GameObject),
                true);
        }

        EditorGUILayout.HelpBox(
            "C gedrückt halten + Linke Maustaste halten zum Painten.\n" +
            "Es kann nur auf Terrain oder den erlaubten Objekten gepaintet werden.",
            MessageType.Info);
    }

    void SceneGUI(SceneView sv)
    {
        if (prefab == null)
            return;

        Event e = Event.current;

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.C)
            cHeld = true;

        if (e.type == EventType.KeyUp && e.keyCode == KeyCode.C)
            cHeld = false;

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        bool allowedSurface = IsAllowedSurface(hit);

        Handles.color = allowedSurface
            ? (cHeld ? Color.green : Color.gray)
            : Color.red;

        Handles.DrawWireDisc(hit.point, hit.normal, checkRadius);

        if (!allowedSurface)
        {
            SceneView.RepaintAll();
            return;
        }

        if (cHeld &&
            e.button == 0 &&
            (e.type == EventType.MouseDown || e.type == EventType.MouseDrag))
        {
            if (EditorApplication.timeSinceStartup - lastPaint >= paintInterval)
            {
                lastPaint = EditorApplication.timeSinceStartup;
                TrySpawn(hit);
            }

            e.Use();
        }

        SceneView.RepaintAll();
    }

    bool IsAllowedSurface(RaycastHit hit)
    {
        return true;
        // Terrain erlaubt?
        if (allowTerrain && hit.collider.GetComponent<Terrain>() != null)
            return true;

        // Ist das getroffene Objekt oder eines seiner Eltern in der Liste?
        Transform t = hit.collider.transform;

        while (t != null)
        {
            if (paintTargets.Contains(t.gameObject))
                return true;

            t = t.parent;
        }

        return false;
    }

    void TrySpawn(RaycastHit hit)
    {
        Collider[] overlaps = Physics.OverlapSphere(
            hit.point,
            checkRadius,
            ~0,
            QueryTriggerInteraction.Ignore);

        foreach (Collider c in overlaps)
        {
            if (parent != null && c.transform.IsChildOf(parent))
                return;

            if (PrefabUtility.IsPartOfPrefabInstance(c.gameObject))
                return;
        }

        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

        Undo.RegisterCreatedObjectUndo(go, "Paint Prefab");

        if (parent != null)
            go.transform.SetParent(parent);

        float offset = 0f;

        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            float lowest = float.MaxValue;

            foreach (Renderer r in renderers)
            {
                if (r.bounds.min.y < lowest)
                    lowest = r.bounds.min.y;
            }

            offset = go.transform.position.y - lowest;
        }

        go.transform.position = hit.point + Vector3.up * offset;

        EditorSceneManager.MarkSceneDirty(go.scene);
    }
}