using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplostionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        new WaitForSeconds(2);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
