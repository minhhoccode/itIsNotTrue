using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Weapon primary, secondary;
    public SpriteRenderer srP, srS;
    public bool canShoot = true;
    public int selectedWeapon = -1;
    public GameObject destroyFX;

    private void Start()
    {
        srP = primary.GetComponent<SpriteRenderer>();
        srS = secondary.GetComponent<SpriteRenderer>();

        if (canShoot)
        {
            ShowWeapons();
        }
        else
        {
            primary.Stow(true);
            secondary.Stow(true);
        }
    }

    private void Update()
    {
        if (!canShoot)
        {
            return;
        }
        else if (selectedWeapon < 0)
        {
            ShowWeapons();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (selectedWeapon == 0)
                primary.Reload();
            else if (selectedWeapon == 1)
                secondary.Reload();
        }
        if(canShoot && selectedWeapon < 0 && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            ShowWeapons();
        }


        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            if (selectedWeapon == 0)
            {
                primary.isShooting = true;
            }
            else if(selectedWeapon == 1)
            {
                if(primary.shootWait <= 0 && secondary.shootWait <=0)
                {
                    primary.isShooting = true;
                    secondary.isShooting = false;
                    selectedWeapon = 0;
                    ShowWeapons();
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (selectedWeapon == 0)
            {
                primary.isShooting = false;
            }
        }
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            if (selectedWeapon == 1)
            {
                secondary.isShooting = true;
            }
            else if (selectedWeapon == 0)
            {
                if (secondary.shootWait <= 0 && primary.shootWait <=0)
                {
                    primary.isShooting = false;
                    secondary.isShooting = true;
                    selectedWeapon = 1;
                    ShowWeapons();
                }
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (selectedWeapon == 1)
            {
                secondary.isShooting = false;
            }
        }
        /*
        if (selectedWeapon == 0)
            primary.HandleInput();
        else if (selectedWeapon == 1)
            secondary.HandleInput();
            */
        primary.HandleInput();
        secondary.HandleInput();
    }

    public void HideWeapons()
    {
        canShoot = false;
        selectedWeapon = -1;
        //srP.color = new Color(0, 0, 0, 0);
        //srS.color = new Color(0, 0, 0, 0);
    }

    public void GetRidOfWeapons()
    {
        canShoot = false;
        selectedWeapon = -1;
        Destroy(primary.gameObject);
        Destroy(secondary.gameObject);
        Instantiate(destroyFX, transform.position, Quaternion.identity);
    }

    public void ShowWeapons()
    {
        if (!canShoot)
        {
            canShoot = true;
            //Instantiate(destroyFX, transform.position, Quaternion.identity);
        }

        if(selectedWeapon < 0)
            selectedWeapon = 0;

        if(selectedWeapon == 0)
        {
            srP.color = new Color(1, 1, 1, 1);
            srS.color = new Color(1, 1, 1, 1);
            primary.Stow(false);
            secondary.Stow(true);
        }
        else if(selectedWeapon == 1)
        {
            srP.color = new Color(1, 1, 1, 1);
            srS.color = new Color(1, 1, 1, 1);
            primary.Stow(true);
            secondary.Stow(false);
        }
    }
}
