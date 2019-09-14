using System.Collections;
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

    public IEnumerator RespawnTank(TankController caller, TankController attacker)
    {
        Debug.Log($"Caller: {caller.gameObject.transform.position}");
        Debug.Log($"Attacker: {attacker.gameObject.transform.position}");

        // Move tank far away from map
        caller.SetPositionAndRotation(new Vector3(0f,0f, -1000f), Quaternion.identity);

        // Set Camera to attacker wiew
        var camera = FindObjectOfType<CameraScript>();
        camera.WatchedTank = attacker.gameObject;
        
        // Disable Canvas
        var canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
        
        // TODO: Show sth

        // Wait
        yield return new WaitForSeconds(5);
        
        // RespawnTank
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        var index = Random.Range(0, spawnPoints.Length);

        caller.SetPositionAndRotation(
            spawnPoints[index].transform.position, 
            spawnPoints[index].transform.rotation);

        // Enabe Canvas back
        canvas.SetActive(true);

        // Set Camera back to caller view
        camera.WatchedTank = caller.gameObject;
    }
}
