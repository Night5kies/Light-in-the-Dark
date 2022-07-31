using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public CharacterController enemyController;
    public CharacterController Player;
    private float targetAngle;
    float turnSmoothVelocity;
    public float turnTime;
    public float enemyDetectionDistance;
    private float distance;
    private Vector3 desiredMovement;
    float groundedGravity = -0.05f;
    public float gravity = -9.8f;




    void Update()
    {
        if ((Player.transform.position - enemyController.transform.position).magnitude < enemyDetectionDistance)
        {
            targetAngle = Mathf.Atan2(Player.transform.position.x - enemyController.transform.position.x, Player.transform.position.z - enemyController.transform.position.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            desiredMovement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward * moveSpeed;
            if (enemyController.isGrounded)
            {
                desiredMovement.y = groundedGravity;
            }
            else
            {
                desiredMovement.y += gravity * Time.deltaTime;
            }

            enemyController.Move(desiredMovement * Time.deltaTime);
        }
    }
}
