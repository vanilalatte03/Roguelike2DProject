using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo
{
    public string name;
    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{

    public static RoomController instance;

    string currentWorldName = "Basement";
    public int stageNum = 0;

    RoomInfo currentLoadRoomData;

    Room currRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;
    bool spawnedBossRoom = false;
    bool updatedRooms = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //LoadRoom("Start", 0, 0);
        //LoadRoom("Empty", 1, 0);
        //LoadRoom("Empty", -1, 0);
        //LoadRoom("Empty", 0, 1);
        //LoadRoom("Empty", 0, -1);
    }

    void Update()
    {
        switch(stageNum)
        {
            case 0:
                currentWorldName = "Basement";
                break;
            case 1:
                currentWorldName = "Stone";
                break;
            default:
                break;
        }

        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }

        if (loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if (spawnedBossRoom && !updatedRooms)
            {
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                UpdateRooms();
                updatedRooms = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);
        }
    }

    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y) == true)
        {
            return;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0
            );

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currRoom = room;
            }

            loadedRooms.Add(room);
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }

    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[] {
            "Empty",
            "Basic1"
        };

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        currRoom = room;

        StartCoroutine(RoomCoroutine());
    }

    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateRooms();
    }

    public void UpdateRooms()
    {
        foreach (Room room in loadedRooms)
        {
            if (currRoom != room)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                GuardianEnemy[] guardians = room.GetComponentsInChildren<GuardianEnemy>();
                GiantEnemy[] giants = room.GetComponentsInChildren<GiantEnemy>();
                WarmEnemy[] warms = room.GetComponentsInChildren<WarmEnemy>();
                FireEnemy fire = room.GetComponentInChildren<FireEnemy>();      // 불의 정령은 하나이므로
                GameObject[] guardianBullets = GameObject.FindGameObjectsWithTag("GuardianBullet");     // 플레이어들이 방을 이동해도 가디언 불렛이 따라오는 현상이 있으므로 여기서 처리

                // 일반 몹
                if (enemies.Length > 0)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true;
                       // Debug.Log("Not in room");
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        //door.doorCollider.SetActive(false);
                    }
                }

                // 거인몹
                if (giants.Length > 0)
                {
                    foreach(GiantEnemy giant in giants)
                    {
                        giant.notInRoom = true;
                    }
                }

                // 웜몬스터
                if (warms.Length > 0)
                {
                    foreach (WarmEnemy warm in warms)
                    {
                        warm.notInRoom = true;
                    }
                }      

                // 가디언
                if (guardians.Length > 0)
                {
                    foreach (GuardianEnemy guardian in guardians)
                    {
                        guardian.notInRoom = true;
                    }
                }

                // 불의 정령
                if (fire != null)
                {
                    fire.notInRoom = true;
                }

                if (guardianBullets.Length > 0)
                {
                    foreach (GameObject bullet in guardianBullets)
                    {
                      
                        bullet.GetComponent<Bullet>().isGuardianBulletNotInRoom = true;
                    }                    
                }           

                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        //door.doorCollider.SetActive(false);
                    }
                }
            }
            else
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                GuardianEnemy[] guardians = room.GetComponentsInChildren<GuardianEnemy>();
                GiantEnemy[] giants = room.GetComponentsInChildren<GiantEnemy>();
                WarmEnemy[] warms = room.GetComponentsInChildren<WarmEnemy>();
                FireEnemy fire = room.GetComponentInChildren<FireEnemy>();                      // 불의 정령은 하나이므로           

                // 일반몹
                if (enemies.Length > 0)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false;
                        Debug.Log("In room");
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        //door.doorCollider.SetActive(true);
                    }
                }                 

                // 거인몹
                if (giants.Length > 0)
                {
                    foreach (GiantEnemy giant in giants)
                    {
                        giant.notInRoom = false;
                    }
                }

                // 웜몬스터
                if (warms.Length > 0)
                {
                    foreach (WarmEnemy warm in warms)
                    {
                        warm.notInRoom = false;
                    }
                }

                // 가디언
                if (guardians.Length > 0)
                {
                    foreach (GuardianEnemy guardian in guardians)
                    {
                        guardian.notInRoom = false;
                    }
                }

                // 불의 정령
                if (fire != null)
                {
                    fire.notInRoom = false;
                }    

                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        //door.doorCollider.SetActive(false);
                    }
                }
            }
        }
    }
}