using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BuildingCreator : Singleton<BuildingCreator>
{
    
    [SerializeField] Tilemap previewMap;
    PlayerInput playerInput;

    TileBase tileBase;
    Blueprint selectedBlueprint;

    Camera _camera;

    Vector2 mousePos;
    Vector3Int currentGridPosition;
    Vector3Int lastGridPosition;
    Brush brush;

    
    protected override void Awake()
    {
        base.Awake();
        playerInput = new PlayerInput();
        brush = Brush.CreateInstance<Brush>();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.GamePlay.MousePosition.performed += OnMouseMove;
        playerInput.GamePlay.MouseLeftClick.performed += OnLeftClick;
        playerInput.GamePlay.MouseRightClick.performed += OnRightClick;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.GamePlay.MousePosition.performed -= OnMouseMove;
        playerInput.GamePlay.MouseLeftClick.performed -= OnLeftClick;
        playerInput.GamePlay.MouseRightClick.performed -= OnRightClick;
    }

    private void Update()
    {
        if(selectedBlueprint != null)
        {
            Vector3 pos = _camera.ScreenToWorldPoint(mousePos);
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 1000))
            {
                pos = hitData.point;
            }
            Vector3Int gridPos = previewMap.WorldToCell(pos);
            //Debug.Log(gridPos.x+" "+gridPos.y+" "+gridPos.z);
            if (gridPos != currentGridPosition || currentGridPosition==null)
            {
                lastGridPosition = currentGridPosition;
                currentGridPosition = gridPos;
            }
            UpdatePreview();
        }
    }

    private Blueprint SelectedBlueprint
    {
        set
        {
            selectedBlueprint = value;
            //tileBase = TileBase.Instantiate((selectedBlueprint != null ? selectedBlueprint.prefabs[0] : null), Vector3.zero, Quaternion.identity);
            UpdatePreview();
        }
    }

    private void OnMouseMove(InputAction.CallbackContext ctx)
    {
        mousePos = ctx.ReadValue<Vector2>();
    }

    private void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (selectedBlueprint == null)
            return;
        Debug.Log("paint");
        brush.Paint(previewMap,selectedBlueprint.prefabs[0].gameObject,currentGridPosition);
    }

    private void OnRightClick(InputAction.CallbackContext ctx)
    {

    }

    public void BlueprintSelected(Blueprint blueprint)
    {
        SelectedBlueprint = blueprint;

        //Set preview Where mouse is

        //On click draw
        previewMap.SetTile(currentGridPosition,tileBase);
        //On right click delete
    }

    private void UpdatePreview()
    {
        if (selectedBlueprint != null)
        {

        }
        //Remove old tile if existing
        //previewMap.SetTile(lastGridPosition,null);
        //Erase(previewMap.GetComponentInParent<Grid>(),selectedBlueprint.prefabs[0],lastGridPosition);
        //Set current tile to curren mouse position tile
        //previewMap.SetTile (currentGridPosition,tileBase);
        //Paint(previewMap.GetComponentInParent<Grid>(),selectedBlueprint.prefabs[0],currentGridPosition);
    }

}
