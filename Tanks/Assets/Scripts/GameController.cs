using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static TankController tank;

    public bool TankRaiseBarrel
    {
        set => tank.IsBarrelRaising = value;
    } // Done

    public void TankShot()
    {
        tank.Shot();
    } // Done

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    } // Done

    // Start is called before the first frame update
    void Start()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        tank = players
            .Single(player => player.GetPhotonView().IsMine)
            .GetComponent<TankController>();
    } // Done
    void Update()
    {
        if (tank == null)
        {
            Start();
        }
    } // Done

    public void EndGame()
    {
        StartCoroutine(DisconectAndReturn());
    } // Done
    private IEnumerator DisconectAndReturn()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        PhotonNetwork.LoadLevel("MenuScrene");
    } // Done

    public IEnumerator RespawnTank(TankController damagedTank, Player attacker)
    {
        //Get Attacker's tank
        var players = GameObject.FindGameObjectsWithTag("Player");
        TankController attackersTank = players.Single(player => player.GetPhotonView().Owner == attacker)
            .GetComponent<TankController>();
        
        // Set Camera to attacker wiew
        var camera = FindObjectOfType<CameraScript>();
        camera.WatchedTank = attackersTank.gameObject;

        // Disable Canvas
        var canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);

        //Big explosion
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "BigExplosion"), damagedTank.gameObject.transform.position, damagedTank.gameObject.transform.rotation);
        damagedTank.GetComponentInParent<PhotonView>().RPC(nameof(damagedTank.PlaySound), RpcTarget.All);

        // TODO: Show sth

        // TODO: Change damaged tank texture        

        // Wait for enable UI
        yield return new WaitForSeconds(5);

        // RespawnTank
        damagedTank.ResetTank(5f);

        // Enabe Canvas back
        canvas.SetActive(true);

        // Set Camera back to damagedTank view
        camera.WatchedTank = damagedTank.gameObject;
    }
}
