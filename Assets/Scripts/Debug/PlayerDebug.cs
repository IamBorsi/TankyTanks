using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDebug : MonoBehaviour
{

    [SerializeField] private bool _cursorVisibility = false;

    public void Update() {
        Cursor.visible = _cursorVisibility;
    }

}
