using UnityEngine;

public class ParallaxStarfield : MonoBehaviour
{
    public Transform target;
    public Vector2 scrollSpeed = new Vector2(0.01f, 0.01f);

    private Material mat;
    private Vector3 lastTargetPos;
    private Vector2 offset = Vector2.zero;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        lastTargetPos = target.position;
    }

    void LateUpdate()
    {
        Vector3 delta = target.position - lastTargetPos;
        offset += new Vector2(delta.x * scrollSpeed.x, delta.y * scrollSpeed.y);
        mat.mainTextureOffset = offset;
        mat.SetVector("_CameraPosition", Camera.main.transform.position);

        lastTargetPos = target.position;

        // Keep the quad under the camera
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
