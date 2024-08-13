using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float mouseSense;
    [SerializeField] Transform player, playerArms;

    float xAxisClamp = 0;

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        float rotateX = Input.GetAxis("Mouse X") * mouseSense;
        float rotateY = Input.GetAxis("Mouse Y") * mouseSense;

        xAxisClamp -= rotateY;

        Vector3 rotPlayerArms = playerArms.rotation.eulerAngles;
        Vector3 rotPlayer = player.rotation.eulerAngles;

        rotPlayerArms.x -= rotateY;
        if (rotPlayerArms.x > 360)
            rotPlayerArms.x -= 360;
        else if (rotPlayerArms.x < 0)
            rotPlayerArms.x += 360;      
            
        rotPlayerArms.z = 0;

        rotPlayer.y += rotateX;
        if(rotPlayer.y > 360)
            rotPlayer.y -= 360;
        else if(rotPlayer.y < 0)
            rotPlayer.y += 360;


        if(xAxisClamp > 90)
        {
            xAxisClamp = 90;
            rotPlayerArms.x = 90;
        }
        else if(xAxisClamp < -90)
        {
            xAxisClamp = -90;
            rotPlayerArms.x = 270;
        }
        
        playerArms.rotation = Quaternion.Euler(rotPlayerArms);
        player.rotation = Quaternion.Euler(rotPlayer);
    }
}
