using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandEmeny : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f; // 이동 속도

    [SerializeField]
    private float changeDirectionInterval = 2f; // 이동 방향 변경 간격

    [SerializeField]
    private float minDistance = 1f; // 최소 이동 거리

    [SerializeField]
    private float maxDistance = 5f; // 최대 이동 거리

    private Vector3 targetPosition;
    private float nextDirectionChangeTime;

    void Start()
    {
        SetRandomTargetPosition();
    }

    void Update()
    {
        // 현재 위치에서 타겟 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 타겟 위치에 도달하면 새로운 타겟 위치를 설정
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f || Time.time >= nextDirectionChangeTime)
        {
            SetRandomTargetPosition();
        }
    }

    void SetRandomTargetPosition()
    {
        // 무작위 타겟 위치 설정
        float randomDistance = Random.Range(minDistance, maxDistance);
        float randomAngle = Random.Range(0f, 360f);
        Vector3 randomDirection = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward;
        targetPosition = transform.position + randomDirection * randomDistance;

        // 다음 방향 변경 시간 설정
        nextDirectionChangeTime = Time.time + changeDirectionInterval;
    }
}
