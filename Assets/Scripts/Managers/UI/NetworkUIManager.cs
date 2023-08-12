using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIManager : MonoBehaviour
{

    [SerializeField] private Button _hostBtn;
    [SerializeField] private Button _serverBtn;
    [SerializeField] private Button _clientBtn;

    private void Awake() {

        // Network Host button
        _hostBtn.onClick.AddListener( () => {
            NetworkManager.Singleton.StartHost();
        });

        // Network Server button
        _serverBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });

        // Network Client button
        _clientBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });

    }

}
