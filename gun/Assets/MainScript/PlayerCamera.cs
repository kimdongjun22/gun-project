using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // 따라갈 대상 (플레이어)
    public Vector3 offset = new Vector3(0, 5, -10);  // 카메라 위치 오프셋
    public float followSpeed = 5f; // 따라가는 속도

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.LookAt(target); // 플레이어를 바라보게 설정 (선택)
        }
    }
}
