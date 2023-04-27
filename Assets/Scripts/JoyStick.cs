using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform rect_Background;
    [SerializeField] private RectTransform rect_JoyStick; // ���� ���󳪿��� �ȵǴϱ� �簢���� ���ֳ��� ����. 
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

    public void OnPointerDown(PointerEventData eventData) // ������ �� 
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData) // ���� �� 
    {
        isTouch = false; 
        rect_JoyStick.localPosition = Vector3.zero;

    }

    public void OnDrag(PointerEventData eventData) // �巡�� �� �� 
    {
        Vector2 value = eventData.position - (Vector2)rect_Background.position;
        // position�� x,y�ุ �����ϱ⿡ vector2������ rect.position�� vector3 �̱⿡ ������ȯ. 
        value = Vector2.ClampMagnitude(value, radius);
        // ���ֵα� ����. 
        rect_JoyStick.localPosition = value;

        value = value.normalized;
        playerDir = value;
        movePosition = new Vector3(value.x * MoveSpeed * Time.deltaTime, value.y * MoveSpeed * Time.deltaTime, 0f);
    }
}
