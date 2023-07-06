using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{

    public Texture2D cursorTextureDrag;
    public Texture2D cursorTextureNormal;
    public Texture2D cursorTextureError;
    private CursorMode mode = CursorMode.ForceSoftware;
    private Vector2 hotSpotNormal;
    private Vector2 hotSpotDrag;
    private Vector2 hotSpotError;

    PlayerController playercontrol;

    private void Start()
    {
        playercontrol = GetComponent<PlayerController>();
        hotSpotNormal = new Vector2(cursorTextureNormal.width / 2, cursorTextureNormal.height / 2);
        hotSpotDrag = new Vector2(cursorTextureDrag.width / 2, cursorTextureDrag.height / 2);
        hotSpotError = new Vector2(cursorTextureError.width / 2, cursorTextureError.height / 2);
    }
    void Update()
    {
        CursorChanger();
    }

    void CursorChanger()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray,out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Draggable"))
            {
                Cursor.SetCursor(cursorTextureDrag, hotSpotDrag, mode);
            }
            else
            {
                Cursor.SetCursor(cursorTextureNormal, hotSpotNormal, mode);
            }
            if ((hit.point.x > transform.position.x && !playercontrol.FacingRight) | (hit.point.x < transform.position.x && playercontrol.FacingRight) && Input.GetMouseButton(1))
            {
                Cursor.SetCursor(cursorTextureError, hotSpotError, mode);
            }
        }
    }


}
