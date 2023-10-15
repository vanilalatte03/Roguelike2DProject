using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandEmeny : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f; // �̵� �ӵ�

    [SerializeField]
    private float changeDirectionInterval = 2f; // �̵� ���� ���� ����

    [SerializeField]
    private float minDistance = 1f; // �ּ� �̵� �Ÿ�

    [SerializeField]
    private float maxDistance = 5f; // �ִ� �̵� �Ÿ�

    private Vector3 targetPosition;
    private float nextDirectionChangeTime;

    void Start()
    {
        SetRandomTargetPosition();
    }

    void Update()
    {
        // ���� ��ġ���� Ÿ�� ��ġ�� �̵�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Ÿ�� ��ġ�� �����ϸ� ���ο� Ÿ�� ��ġ�� ����
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f || Time.time >= nextDirectionChangeTime)
        {
            SetRandomTargetPosition();
        }
    }

    void SetRandomTargetPosition()
    {
        // ������ Ÿ�� ��ġ ����
        float randomDistance = Random.Range(minDistance, maxDistance);
        float randomAngle = Random.Range(0f, 360f);
        Vector3 randomDirection = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward;
        targetPosition = transform.position + randomDirection * randomDistance;

        // ���� ���� ���� �ð� ����
        nextDirectionChangeTime = Time.time + changeDirectionInterval;
    }
}
