using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimObj : MonoBehaviour
{
    // ����Ʈ �ִϸ��̼� ����� ȣ��Ǵ� �Լ�
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
