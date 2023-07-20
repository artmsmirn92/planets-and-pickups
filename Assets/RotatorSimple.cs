using UnityEngine;

public class RotatorSimple : MonoBehaviour
{
    [SerializeField] private float speed;

    private void FixedUpdate()
    {
        var rotVec = Vector3.forward;
        transform.Rotate(rotVec * speed * Time.fixedDeltaTime);
    }
}
