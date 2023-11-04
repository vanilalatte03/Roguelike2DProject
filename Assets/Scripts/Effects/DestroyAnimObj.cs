using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimObj : MonoBehaviour
{
    // 이펙트 애니메이션 종료시 호출되는 함수
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
