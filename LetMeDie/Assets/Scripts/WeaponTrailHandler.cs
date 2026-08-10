using UnityEngine;

public class WeaponTrailHandler : MonoBehaviour
{
    private const int NUM_VERTICES = 12;
    private const int NUM_TRIANGLES = 24; // doppelseitig!

    public Transform Tip;
    public Transform Base;
    [SerializeField] private GameObject _meshParent;
    [SerializeField] private int _trailFrameLength = 10;
    [SerializeField] private Material meshMaterial;
    private Material currentMaterial;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;

    private int _frameCount;

    private Vector3 _previousTipPosition;
    private Vector3 _previousBasePosition;

    void Awake()
    {
        _mesh = new Mesh();
        _mesh.name = "WeaponTrailMesh";

        _meshParent.GetComponent<MeshFilter>().mesh = _mesh;
        currentMaterial = new(meshMaterial);
        _meshParent.GetComponent<MeshRenderer>().material = currentMaterial;

        _vertices = new Vector3[_trailFrameLength * NUM_VERTICES];
        _triangles = new int[_trailFrameLength * NUM_TRIANGLES];

        transform.SetParent(null);
        gameObject.SetActive(false);
    }

    public void ShowTrail()
    {
        _previousTipPosition = Tip.transform.position;
        _previousBasePosition = Base.transform.position;
        gameObject.SetActive(true);
    }

    public void HideTrail() {
        gameObject.SetActive(false);
    }

    public void SetColor(Color color)
    {
        currentMaterial.color = color;
    }

    void LateUpdate()
    {
        int frameIndex = _frameCount % _trailFrameLength;

        int v = frameIndex * NUM_VERTICES;
        int t = frameIndex * NUM_TRIANGLES;

        Vector3 currentTip = Tip.position;
        Vector3 currentBase = Base.position;

        // ===== VERTICES =====

        _vertices[v + 0] = currentBase;
        _vertices[v + 1] = currentTip;
        _vertices[v + 2] = _previousTipPosition;

        _vertices[v + 3] = currentBase;
        _vertices[v + 4] = _previousTipPosition;
        _vertices[v + 5] = currentTip;

        _vertices[v + 6] = _previousTipPosition;
        _vertices[v + 7] = currentBase;
        _vertices[v + 8] = _previousBasePosition;

        _vertices[v + 9] = _previousTipPosition;
        _vertices[v + 10] = _previousBasePosition;
        _vertices[v + 11] = currentBase;

        // ===== TRIANGLES (DOUBLE SIDED) =====

        int tri = 0;

        AddDoubleTriangle(v + 0, v + 1, v + 2, t + tri); tri += 6;
        AddDoubleTriangle(v + 3, v + 4, v + 5, t + tri); tri += 6;
        AddDoubleTriangle(v + 6, v + 7, v + 8, t + tri); tri += 6;
        AddDoubleTriangle(v + 9, v + 10, v + 11, t + tri); tri += 6;

        // ===== APPLY =====

        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        _previousTipPosition = currentTip;
        _previousBasePosition = currentBase;

        _frameCount++;

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
    }

    void AddDoubleTriangle(int a, int b, int c, int index)
    {
        // Front
        _triangles[index + 0] = a;
        _triangles[index + 1] = b;
        _triangles[index + 2] = c;

        // Back
        _triangles[index + 3] = c;
        _triangles[index + 4] = b;
        _triangles[index + 5] = a;
    }
}