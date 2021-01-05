using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Moving : NetworkBehaviour
{

    struct CubeState
    {
        public float x;
        public float y;
        public float z;
    }

    [SyncVar]CubeState state;

    void Awake()
    {
        InitState();
    }
    [Server]
    void InitState()
    {
        state = new CubeState
        {
            x = 0,
            y = 0,
            z = 0
        };
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            KeyCode[] arrowKeys = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow };
            foreach (KeyCode arrowKey in arrowKeys)
            {
                if (!Input.GetKey(arrowKey)) continue;
                CmdMoveOnServer(arrowKey);
            }
        }
        SyncState();
    }

    void SyncState()
    {
        transform.position = new Vector3(state.x, state.y, state.z);
    }
    [Command]
    void CmdMoveOnServer(KeyCode arrowKey)
    {
        state = Move(state, arrowKey);
    }

    CubeState Move(CubeState previous, KeyCode arrowKey)
    {
        float dx = 0;
        float dy = 0;
        float dz = 0;

        switch (arrowKey)
        {
            case KeyCode.UpArrow:
                dz = Time.deltaTime;
                break;
            case KeyCode.DownArrow:
                dz = -Time.deltaTime;
                break;
            case KeyCode.RightArrow:
                dx = Time.deltaTime;
                break;
            case KeyCode.LeftArrow:
                dx = -Time.deltaTime;
                break;
        }

        return new CubeState
        {
            x = dx + previous.x,
            y = dy + previous.y,
            z = dz + previous.z
        };
    }
}
