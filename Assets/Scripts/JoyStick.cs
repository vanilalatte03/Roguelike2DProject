using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform rect_Background;
    [SerializeField] private RectTransform rect_JoyStick; // 원이 따라나오면 안되니깐 사각형에 가둬놓는 구문. 
    [SerializeField] private GameObject go_Player;
    [SerializeField] private float MoveSpeed;
    public Vector2 playerDir;

    private bool isTouch = false;
    private Vector3 movePosition;

    private float radius;

    void Start()
    {
        radius = rect_Background.rect.width * 0.5f;
    }

    void Update()
    {
        if (isTouch)
        {
            go_Player.transform.position += movePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData) // 눌렀을 때 
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData) // 뗐을 때 
    {
        isTouch = false; 
        rect_JoyStick.localPosition = Vector3.zero;

    }

    public void OnDrag(PointerEventData eventData) // 드래그 할 때 
    {
        Vector2 value = eventData.position - (Vector2)rect_Background.position;
        // position은 x,y축만 존재하기에 vector2이지만 rect.position은 vector3 이기에 강제변환. 
        value = Vector2.ClampMagnitude(value, radius);
        // 가둬두기 구문. 
        rect_JoyStick.localPosition = value;

        value = value.normalized;
        playerDir = value;
        movePosition = new Vector3(value.x * MoveSpeed * Time.deltaTime, value.y * MoveSpeed * Time.deltaTime, 0f);
    }
}
