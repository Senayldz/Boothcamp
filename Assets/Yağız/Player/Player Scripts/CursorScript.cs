using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{

    public Texture2D cursorTextureDrag;
    public Texture2D cursorTextureNormal;
    private CursorMode mode = CursorMode.ForceSoftware;
    private Vector2 hotSpotNormal;
    private Vector2 hotSpotDrag;

    private void Start()
    {
        hotSpotNormal = new Vector2(cursorTextureNormal.width / 2, cursorTextureNormal.height / 2);
        hotSpotDrag = new Vector2(cursorTextureDrag.width / 2, cursorTextureDrag.height / 2);
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
        }
    }


}
