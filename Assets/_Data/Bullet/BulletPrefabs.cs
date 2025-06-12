using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrefabs : ImpBehaviour
{
    [Header("BulletPrefabs")]
    private Dictionary<string, Color> rainbowColors = new Dictionary<string, Color>()
    {
        { "Red", new Color(1f, 0f, 0f) },
        { "Orange", new Color(1f, 0.5f, 0f) },
        { "Yellow", new Color(1f, 1f, 0f) },
        { "Green", new Color(0f, 1f, 0f) },
        { "Blue", new Color(0f, 0f, 1f) },
        { "Indigo", new Color(0.29f, 0f, 0.51f) },
        { "Violet", new Color(0.56f, 0f, 1f) }
    };

    [SerializeField] private Material partMaterial;
    [SerializeField] private GameObject basePrefab;
    [SerializeField] private float _scale = 0.45f;

    protected override void Awake()
    {
        base.Awake();
        GenerateAllCombinations();
    }

    void GenerateAllCombinations()
    {
        var names = new List<string>(rainbowColors.Keys);

        for (int size = 1; size <= names.Count; size++)
        {
            var combinations = GetCombinations(names, size);
            foreach (var combo in combinations)
            {
                CreateBullet(combo);
            }
        }
    }

    void CreateBullet(List<string> colorNames)
    {
        // Clone base prefab
        GameObject bullet = Instantiate(basePrefab, this.transform);
        bullet.name = string.Join(" ", colorNames);
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        bullet.SetActive(false);

        // Xóa các con cũ trong basePrefab (nếu có)
        //foreach (Transform child in bullet.transform)
        //{
        //    DestroyImmediate(child.gameObject);
        //}

        // Tạo GameObject tên "Sprite"
        GameObject spriteGO = new GameObject("Sprite");
        spriteGO.transform.SetParent(bullet.transform, false);
        spriteGO.transform.localScale = Vector3.one * _scale;

        // Tạo các phần hình quạt (sector)
        int n = colorNames.Count;
        float anglePerPart = 360f / n;

        for (int i = 0; i < n; i++)
        {
            GameObject part = new GameObject("Part_" + colorNames[i]);
            part.transform.SetParent(spriteGO.transform, false);

            part.transform.localRotation = Quaternion.Euler(0, 180f, 0);  // Xoay nếu cần

            MeshFilter mf = part.AddComponent<MeshFilter>();
            MeshRenderer mr = part.AddComponent<MeshRenderer>();
            mr.material = new Material(partMaterial);
            mr.material.color = rainbowColors[colorNames[i]];

            mr.sortingLayerName = "Bullet";
            mr.sortingOrder = 1;

            mf.mesh = CreateSectorMesh(1f, i * anglePerPart, anglePerPart);
        }

        // Thêm controller nếu cần
        //ColorBulletCtrl ctrl = bullet.GetComponent<ColorBulletCtrl>();
        //if (ctrl == null)
        //{
        //    ctrl = bullet.AddComponent<ColorBulletCtrl>();
        //}
        // ctrl.Initialize(colorNames); // Nếu có init
    }

    Mesh CreateSectorMesh(float radius, float startAngleDeg, float angleDeg)
    {
        int segments = 10;
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        vertices.Add(Vector3.zero); // Center

        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngleDeg + (angleDeg * i / segments);
            float rad = Mathf.Deg2Rad * angle;
            vertices.Add(new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0f));
        }

        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        return mesh;
    }

    List<List<string>> GetCombinations(List<string> items, int length)
    {
        List<List<string>> result = new List<List<string>>();
        CombineRecursive(items, length, 0, new List<string>(), result);
        return result;
    }

    void CombineRecursive(List<string> items, int length, int start, List<string> current, List<List<string>> result)
    {
        if (current.Count == length)
        {
            result.Add(new List<string>(current));
            return;
        }

        for (int i = start; i < items.Count; i++)
        {
            current.Add(items[i]);
            CombineRecursive(items, length, i + 1, current, result);
            current.RemoveAt(current.Count - 1);
        }
    }
}
