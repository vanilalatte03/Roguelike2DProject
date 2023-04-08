using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int Width;

    public int Height;

    public int X;

    public int Y; 
    void Start()
    {
        if(RoomController.instance == null)
        {
            Debug.Log("잘못된 씬입니다.");
            return;
        }

        RoomController.instance.RegisterRoom(this);
    }

    private void OnDrawGizmos()  
    {
        // 기즈모는 scene에서 게임 오브젝트와 관련된 그래픽, 시각적인 디버깅을 위해 본 구문이 사용
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));

    }

    public Vector3 GetRoomCenter()
    {
        return new Vector3(X * Width, Y * Height);
    }

}
