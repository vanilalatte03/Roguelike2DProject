using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo
{
    public string name;

    public int X;

    public int Y; 
    // START ������ (0,0)�̶�� �� ��, ���������� ���� (1,0) ���� ���� (0,1) �̷� ������ �����ϱ� ����.
}
public class RoomController : MonoBehaviour
{

    public static RoomController instance;

    string currentWorldName = "Basement";

    RoomInfo currentLoadRoomData;

    Queue<RoomInfo> IoadRoomQueue = new Queue<RoomInfo>();
    // ���Լ��� ���� 

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;

    private void Awake()
    {
        instance = this;
    }

       public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }
}
