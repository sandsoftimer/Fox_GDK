using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;
    public bool useCustomCamera;
    Camera _camera;

    public Camera sceneCam
    {
        get
        {
            if (_camera == null)
                _camera = Camera.main;
            return _camera;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(sceneCam.transform.position - offset);
    }
}
