using UnityEngine;

public class OrbitAroundTree : MonoBehaviour
{
    public GameObject tree;     // The tree (or central object) to orbit around
    public float radius = 5f;   // Radius of the circle
    public float speed = 2f;    // Speed of the movement
    public float initialAngle;  // The initial angle offset for each character
    public bool isRotate;
    private float angle;

    void Start()
    {
        // Initialize the angle with the given initial angle
        angle = initialAngle;
    }

    void Update()
    {
       if (isRotate)
        {
            rotate();
        }
    }
    void rotate()
    {
        // Increment the angle based on time and speed
        angle += speed * Time.deltaTime;

        // Calculate the new position around the tree
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        // Set the character's position relative to the tree
        transform.position = new Vector3(tree.transform.position.x + x, transform.position.y, tree.transform.position.z + z);

        // Rotate the character to face the direction it's moving
        Vector3 direction = (new Vector3(Mathf.Sin(angle), 0, -Mathf.Cos(angle))).normalized;
        transform.forward = direction;
        // transform.LookAt(tree.transform.position);

    }
}
