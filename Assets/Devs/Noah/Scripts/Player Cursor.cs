using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public int playerNum;


    private UIControls controls;
    private RectTransform rectTransform;

    private Vector2 moveDirection = new Vector2(0f,0f);
    [SerializeField] float moveSpeed = 1f;

    private GameObject canvas;

    private void Awake()
    {
        controls = new UIControls();

        controls.Menus.Move.performed += context => Move(context.ReadValue<Vector2>());

        rectTransform = GetComponent<RectTransform>();

        canvas = GameObject.Find("Canvas");

        transform.SetParent(canvas.transform, false);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        rectTransform.Translate(moveDirection * moveSpeed);
    }

    private void Move(Vector2 _direction)
    {
        moveDirection = _direction;
    }
}