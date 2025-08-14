using UnityEngine;

public class MazeEntrance : MonoBehaviour
{
    public  GameObject mazerPlayer;
    public  GameObject mazerCamera;
    public  GameObject mazeMasterPlayer;
    public  GameObject mazeMasterCamera;
    public  GameObject mazeMasterSpawnpoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == mazerPlayer)
        {
            var camFollow = mazerCamera.GetComponent<CameraFollow>();
            camFollow.mazerMode = true;
            camFollow.mazeMasterMode = false;
            camFollow.normalMode = false;
        }

        if (other.gameObject == mazeMasterPlayer)
        {
            mazeMasterPlayer.transform.position = mazeMasterSpawnpoint.transform.position;
            var camFollow = mazeMasterCamera.GetComponent<CameraFollow>();
            camFollow.mazerMode = false;
            camFollow.mazeMasterMode = true;
            camFollow.normalMode = false;
        }
    }
}
