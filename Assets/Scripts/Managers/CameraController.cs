using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 30f;
    public float panBorderScreen = 30f;
    public float ScroolSpeed = 0.5f;
    public LayerMask NodeMask;

    [Header("Limits")]
    public float minY = 3.5f;
    public float maxY = 30f;

    private PlayerInput playerInput;
    private Vector2 mousePos;
    private RaycastHit lastHit;
    private IMouse lastMouseMove;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Camera.MousePosition.performed += Move;
        playerInput.Camera.Scrolling.performed += scroll;
        playerInput.GamePlay.MouseLeftClick.performed += MouseLeftClick;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Camera.MousePosition.performed -= Move;
        playerInput.Camera.Scrolling.performed -= scroll;
        playerInput.GamePlay.MouseLeftClick.performed -= MouseLeftClick;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameOver)
        {
            this.enabled = false;
            return;
        }

        if (playerInput.Camera.ForwardMove.IsPressed() || mousePos.y >= Screen.height - panBorderScreen)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (playerInput.Camera.BackwardMoce.IsPressed() || mousePos.y <= panBorderScreen)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        if (playerInput.Camera.RightMove.IsPressed() || mousePos.x >= Screen.width - panBorderScreen)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (playerInput.Camera.LeftMove.IsPressed() || mousePos.x <= panBorderScreen)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
        /*
        */
    }

    void Move(InputAction.CallbackContext ctx)
    {
        mousePos = ctx.ReadValue<Vector2>();
        if (ctx.phase == InputActionPhase.Performed &&
            Physics.Raycast(
                Camera.main.ScreenPointToRay(mousePos),
                out RaycastHit hit, NodeMask))
        {
            IMouse mouseMove = hit.collider.gameObject.GetComponent<IMouse>();
            if (!hit.collider.Equals(lastHit.collider))
            {
                mouseMove?.OnMouseEnter();
                lastMouseMove?.OnMouseExit();
            }

            lastHit = hit;
            lastMouseMove = mouseMove;
        }
    }
    void scroll(InputAction.CallbackContext ctx)
    {
        //float scroll = Input.GetAxis("Mouse ScrollWheel");
        float scroll = ctx.ReadValue<float>();
        Vector3 pos = transform.position;

        pos.y -= scroll * 1000 * ScroolSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
    
    public void MouseLeftClick(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed &&
            Physics.Raycast(
                Camera.main.ScreenPointToRay(
                    Mouse.current.position.ReadValue()),
                out RaycastHit hit, NodeMask))
        {
            IMouse mouseDown = hit.collider.gameObject.GetComponent<IMouse>();

            mouseDown?.OnMouseDown();
        }
    }

}
