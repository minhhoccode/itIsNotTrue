using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform player;
    [SerializeField] float threshold;

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;
        if (player == null)
            player = this.transform;
        cam.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = this.transform;
    }

    void Update()
    {
        /*
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (player.position + mousePos) / 2f;
        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x,
        threshold + player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + player.position.y,
        threshold + player.position.y);
        this.transform. position = targetPos;
        */
        Vector2 mousePos = Input.mousePosition;
        Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 temp = mousePos - center;
        
        temp.x = temp.x / (float)(Screen.width  / 2f) * 1.2f;
        temp.y = temp.y / (float)(Screen.height / 2f) * 1.2f;

        temp.x = Mathf.Clamp(temp.x, -1, 1);
        temp.y = Mathf.Clamp(temp.y, -1, 1);

        float angle = Mathf.Atan2(temp.y, temp.x);

        float amount = Mathf.Min(1,temp.magnitude);

        temp.x = Mathf.Cos(angle);
        temp.y = Mathf.Sin(angle);
        

        transform.localPosition = temp * (amount * threshold);

    }
}
