using UnityEngine;

/// <summary>
/// Simple parallax background controller.
/// Attach this to a parent background GameObject. Each direct child will be treated
/// as a parallax layer (its own children move with it). You can also manually
/// configure layers. Lower multiplier values move slower (appear farther away).
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform root;                          // Root transform of the layer
        [Range(0f, 1f)] public float multiplier = 0.5f;  // 0 = static relative to world, 1 = moves with camera
        public Vector2 axisMultiplier = new Vector2(1f, 1f); // (1,0) = horizontal only, (1,1) both axes
    }

    [Header("Camera / Movement Source")] 
    public Transform cameraTransform;          // Defaults to main camera if left null

    [Header("Layers (auto-populated if empty)")]
    public ParallaxLayer[] layers;             // Direct children become layers if array empty

    [Header("Options")] 
    public bool autoPopulateIfEmpty = true;    // Auto fill layers from direct children if layers empty
    public bool updateInEditMode = true;       // Allow preview in editor when not playing

    private Vector3 _cameraStartPos;
    private Vector3[] _layerStartPositions;
    private bool _initialized;

    void Awake()
    {
        Initialize();
    }

    void Reset()
    {
        TryAutoPopulate();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (autoPopulateIfEmpty && (layers == null || layers.Length == 0))
        {
            TryAutoPopulate();
        }
    }
#endif

    void Initialize()
    {
        if (_initialized) return;

        if (cameraTransform == null)
        {
            if (Camera.main != null)
                cameraTransform = Camera.main.transform;
            else
                return; // Cannot init without a camera
        }

        if (layers == null || layers.Length == 0)
        {
            TryAutoPopulate();
        }

        _cameraStartPos = cameraTransform.position;
        _layerStartPositions = new Vector3[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i] != null && layers[i].root != null)
                _layerStartPositions[i] = layers[i].root.position;
        }
        _initialized = true;
    }

    void LateUpdate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && !updateInEditMode)
            return;
#endif
        if (!_initialized)
            Initialize();
        if (!_initialized) return; // still failed (no camera)

        Vector3 camDelta = cameraTransform.position - _cameraStartPos;

        for (int i = 0; i < layers.Length; i++)
        {
            var layer = layers[i];
            if (layer == null || layer.root == null) continue;

            Vector3 basePos = _layerStartPositions[i];
            // Apply per-axis parallax
            float px = camDelta.x * layer.multiplier * layer.axisMultiplier.x;
            float py = camDelta.y * layer.multiplier * layer.axisMultiplier.y;
            layer.root.position = new Vector3(basePos.x + px, basePos.y + py, layer.root.position.z);
        }
    }

    private void TryAutoPopulate()
    {
        int childCount = transform.childCount;
        if (childCount == 0)
        {
            layers = new ParallaxLayer[0];
            return;
        }

        layers = new ParallaxLayer[childCount];
        if (childCount == 1)
        {
            layers[0] = new ParallaxLayer { root = transform.GetChild(0), multiplier = 0.2f };
            return;
        }

        // Distribute multipliers across children (farther back = smaller multiplier)
        for (int i = 0; i < childCount; i++)
        {
            float t = childCount <= 1 ? 0f : (float)i / (childCount - 1); // 0..1
            // Invert so first child is far (0.1) last is near (0.9)
            float mult = Mathf.Lerp(0.1f, 0.9f, t);
            layers[i] = new ParallaxLayer
            {
                root = transform.GetChild(i),
                multiplier = mult,
                axisMultiplier = new Vector2(1f, 1f)
            };
        }
    }
}
