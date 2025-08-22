// This script controls the player character's movement and rotation.

using UnityEngine;

public class Player : MonoBehaviour
{
    // === Inspector Variables ===
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotSpeed = 5f;
    [SerializeField] private GameInput gameInput;

    // === Private Variables ===
    private Vector3 moveDir = Vector3.zero;
    private float moveDistance;
    private bool isWalking;

    // === Unity Lifecycle Methods ===
    private void Update()
    {

        Vector2 inputVect = gameInput.GetMovementVector2Normalized();

        float inputMagnitude = inputVect.magnitude;// ingor minor fluctuations

        // If the input is significant, we're in the "moving" state.
        if (inputMagnitude > 0.1f)
        {
            // Set the movement direction from the input vector.
            moveDir = new Vector3(inputVect.x, 0, inputVect.y);

            isWalking = true;
        }
        else
        {
            //explicitly setting the direction to zero.
            moveDir = Vector3.zero;

            moveDistance = moveSpeed * Time.deltaTime; 
            isWalking = false;
        }

        float playerHeight = 2f;
        float playerRadius = .7f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move in any direction
                }
            }
           
        }
        // Move the character in the determined direction.
        if (canMove)
        {
            transform.position += moveDistance * moveDir;
        }
        // Only rotate if there is actual movement to avoid jerky rotation when standing still.
        if (moveDir.magnitude > 0)
        {
            // Rotate the character smoothly towards the direction of movement.
            // Using a Slerp ensures a natural, non-instantaneous turn.
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotSpeed);
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
