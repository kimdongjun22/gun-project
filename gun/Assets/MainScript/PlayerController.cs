using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;

    private CharacterController controller;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);
        moveDirection = move.normalized * moveSpeed;

        // 실제 이동
        controller.SimpleMove(moveDirection);

        // 애니메이션용 속도 전달
        float speed = moveDirection.magnitude;
        animator.SetFloat("Speed", speed);

        // 이동 방향으로 회전 (선택)
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
    }
}
