using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static TankController tank;

    public bool TankRaiseBarrel
    {
        set => tank.IsBarrelRaising = value;
    }

    public void TankShot()
    {
        tank.Shot();
    }

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
    }

    // Start is called before the first frame update
    void Start()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        tank = players
            .Single(player => player.GetPhotonView().IsMine)
            .GetComponent<TankController>();
    }
    void Update()
    {
        if (tank == null)
        {
            Start();
        }
    }

    public void EndGame()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MenuScrene");
    }

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
