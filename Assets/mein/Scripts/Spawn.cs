using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

    public float CubeEvery;
    private float timerC;

    public void SpawnCube()
    {
        GameObject cube = transform.FindChild("Cube").gameObject;
        GameObject spawning=Instantiate<GameObject>(cube);
        spawning.transform.position = this.transform.position;
        spawning.SetActive(true);
    }
	// Use this for initialization
	void Start () {
        timerC = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timerC += Time.deltaTime;
        if (timerC >= CubeEvery && timerC>0)
        {
            SpawnCube();
            timerC = 0;
        }
	}
}
