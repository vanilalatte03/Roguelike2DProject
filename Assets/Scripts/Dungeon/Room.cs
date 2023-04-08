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
            Debug.Log("�߸��� ���Դϴ�.");
            return;
        }

        RoomController.instance.RegisterRoom(this);
    }

    private void OnDrawGizmos()  
    {
        // ������ scene���� ���� ������Ʈ�� ���õ� �׷���, �ð����� ������� ���� �� ������ ���
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));

    }

    public Vector3 GetRoomCenter()
    {
        return new Vector3(X * Width, Y * Height);
    }

}
