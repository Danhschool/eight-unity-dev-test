using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float xMove;
    [SerializeField] private float sensitivity = 40f;
    public Vector2 lockAxis;


    [SerializeField] private Transform playerBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xMove = lockAxis.x * sensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * xMove);
    }
}
