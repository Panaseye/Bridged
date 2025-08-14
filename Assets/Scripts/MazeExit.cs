using UnityEngine;

public class MazeExit : MonoBehaviour
{
    public  GameObject mazerPlayer;
    public  GameObject mazerCamera;
    public  GameObject mazeMasterPlayer;
    public  GameObject mazeMasterCamera;
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
            camFollow.mazerMode = false;
            camFollow.mazeMasterMode = false;
            camFollow.normalMode = true;
            var othercamFollow = mazeMasterCamera.GetComponent<CameraFollow>();
            othercamFollow.mazerMode = false;
            othercamFollow.mazeMasterMode = false;
            othercamFollow.normalMode = true;
        }

    }
}