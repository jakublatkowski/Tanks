using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static TankController tank;

    public float TankLeftForce
    {
        set => tank.LeftForce = value;
    }

    public float TankRightForce
    {
        set => tank.RightForce = value;
    }

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
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tank = GameObject.FindObjectOfType<TankController>();
    }
    void Update()
    {
        if (tank == null)
            tank = GameObject.FindObjectOfType<TankController>();
    }

    public IEnumerator RespawnTank(TankController damagedTank, Player attacker)
    {
        //Get Attacker's tank
        TankController attackersTank = null;
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetPhotonView().Owner == attacker)
            {
                attackersTank = player.GetComponent<TankController>();
            }
        }

        Debug.Log($"DamagedTank: {damagedTank.gameObject.GetPhotonView().Owner.NickName}  HP: {damagedTank.healthPoints}");
        Debug.Log($"Attacker: {attackersTank.gameObject.GetPhotonView().Owner.NickName}  HP: {attackersTank.healthPoints}");

        // Move tank far away from map
        //damagedTank.SetPositionAndRotation(new Vector3(0f,0f, -1000f), Quaternion.identity);

        // Set Camera to attacker wiew
        var camera = FindObjectOfType<CameraScript>();
        camera.WatchedTank = attackersTank.gameObject;
        
        // Disable Canvas
        var canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
        
        // TODO: Show sth

        // Wait
        yield return new WaitForSeconds(5);
        
        // RespawnTank
        damagedTank.ResetTank();

        // Enabe Canvas back
        canvas.SetActive(true);

        // Set Camera back to damagedTank view
        camera.WatchedTank = damagedTank.gameObject;
    }
}
