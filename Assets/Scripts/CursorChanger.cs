using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChanger : MonoBehaviour
{
    Texture2D cursor;
    Texture2D cursorMouseL;
    Texture2D cursorMouseLM;

    bool overUI;
    bool resetCursor;

    void Start()
    {
        cursor = Resources.Load<Texture2D>("Sprites/cursor");
        cursorMouseL = Resources.Load<Texture2D>("Sprites/cursor_mouse_l");
        cursorMouseLM = Resources.Load<Texture2D>("Sprites/cursor_mouse_l_and_m");
    }

    void Update()
    {
        overUI = false;
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            resetCursor = false;
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(position.x, position.y), Vector2.zero, 0, (1 << 8) | (1 << 9) | (1 << 11));
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    Unit u = hits[i].collider.gameObject.GetComponent<Unit>();
                    if (u != null && (u is Customer))
                    {
                        Cursor.SetCursor(cursorMouseLM, Vector2.zero, CursorMode.Auto);
                        break;
                    }
                    Cursor.SetCursor(cursorMouseL, Vector2.zero, CursorMode.Auto);
                }
            }
            else
            {
                Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
            }
        }
        else
        {
            overUI = true;
        }
        if (overUI && !resetCursor)
        {
            resetCursor = true;
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
