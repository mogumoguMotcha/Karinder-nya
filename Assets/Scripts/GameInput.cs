using UnityEngine;

public class GameInput : MonoBehaviour
{

    public Vector2 GetMovementVector2Normalized()
    {
        Vector2 inputVect = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        return inputVect;
    }
}
