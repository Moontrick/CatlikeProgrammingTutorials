using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour
{


    [SerializeField]
    private Mesh[] meshes;

    [SerializeField]
    private Material material;

    [SerializeField]
    private int maxDepth;

    [SerializeField]
    private float childScale;

    [SerializeField]
    private float spawnProbability;

    private float rotationSpeed;

    [SerializeField]
    private float maxRotationSpeed;

    [SerializeField]
    private float maxTwist;

    private int depth;

    private static Vector3[] childDirections =  
    { 
        Vector3.up, 
        Vector3.right, 
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    private static Quaternion[] childOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

    private Material[,] materials;



    // -----------------------------------------------------
    // Mono Functions



    private void Start()
    {
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);

        if (materials == null)
            InitializeMaterials();

        // Add the mesh and set the material in the renderer
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0, 2)];

        // Recursively called until depth == maxDepth
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }


    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }



    // ---------------------------------------------------------
    // Private Functions



    // Coroutine for staggering the creation of children so we can watch the fractal grow
    private IEnumerator CreateChildren()
    {
        for (int i = 0; i < childDirections.Length; i++)
        {
            if (Random.value < spawnProbability)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("Fractal_Child").AddComponent<Fractal>().Initialize(this, i);
            }
        }
    }


    // Initialize settings for all children created for the fractal, including incrementing depth
    private void Initialize(Fractal parent, int childIndex)
    {
        meshes = parent.meshes;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        spawnProbability = parent.spawnProbability;
        maxRotationSpeed = parent.maxRotationSpeed;
        maxTwist = parent.maxTwist;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[childIndex];
    }


    // Cache materials used for each depth
    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1, 2];
        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            t *= t;

            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.Lerp(Color.red, Color.blue, t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.red, Color.yellow, t);
        }

        materials[maxDepth, 0].color = Color.magenta;
        materials[maxDepth, 1].color = Color.cyan;
    }


}
