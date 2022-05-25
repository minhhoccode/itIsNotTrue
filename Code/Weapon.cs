using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float speed;
    public float damage = 2f;
    public float pushBack = 1f;
    public float shootWait = 0;
    public bool isShooting = false;

    public bool usesAmmo = true;
    public float magazine = 20;
    float currentAmmo;
    public bool canReload = true;
    public float reloadTime = 2f;
    public int reloadSpins = 2;
    float currentReloadTime = 0;
    float baseReloadAngle;

    public float knockback = 0f;
    Vector2 applyKnockback = Vector2.zero;

    public bool isRaycast = false;
    public LayerMask raycastMask;

    public Vector2 offset;
    public bool useOffset = false;
    public float offsetAmount = 0;
    float offsetAnim = 0;

    public Transform aimDir;
    public GameObject attackObj;
    public GameObject fireSfx;
    public Transform attackTransform;

    bool offsetStage = false; // switches between true and false based on whether it has swung

    float aimAngle;

    public SpriteRenderer weaponRenderer;
    public Rigidbody2D playerRB;

    bool isStowed = false;
    public Vector2 stowedOffset = Vector2.zero;
    public float stowedRotation = 0;

    Vector3 oldv3pos = Vector3.zero;

    void Start()
    {
        if (weaponRenderer == null)
        {
            weaponRenderer = GetComponent<SpriteRenderer>();
        }
        if (aimDir == null)
            aimDir = transform.parent;
        transform.localPosition = offset;
        if (usesAmmo)
            currentAmmo = magazine;
        if(playerRB == null)
        {
            playerRB = GetComponentInParent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if(currentReloadTime > 0)
            currentReloadTime = Mathf.Max(0, currentReloadTime - Time.deltaTime);
        if (offsetAnim > 0)
            offsetAnim = Mathf.Max(0, offsetAnim - Time.deltaTime / reloadTime);
        if (shootWait > 0)
            shootWait = Mathf.Max(0, shootWait - Time.deltaTime);
    }


    public void HandleInput()
    {
        if (isStowed)
            return;

        // aim cursor angle logic
        Vector3 v3pos = Camera.main.WorldToScreenPoint(aimDir.transform.position);
        v3pos = (Input.mousePosition - v3pos);
        Vector3 tempv3pos = v3pos;
        if (oldv3pos != Vector3.zero)
        {
            v3pos = Vector3.Lerp(oldv3pos, v3pos, 0.3f);
        }
        oldv3pos = v3pos;
        float angle = Mathf.Atan2(v3pos.y, v3pos.x) * Mathf.Rad2Deg;
        float trueAngle = Mathf.Atan2(tempv3pos.y, tempv3pos.x) * Mathf.Rad2Deg;
        if (angle < 0.0f) angle += 360.0f;
        if (trueAngle < 0.0f) trueAngle += 360.0f;
        // weapon rendering logic
        aimAngle = trueAngle;
        
        RotateGun(angle);

        if (isShooting)
            Fire();
    }

    public void Fire()
    {
        if(shootWait == 0)
        {
            if ((!usesAmmo || currentAmmo > 0 && currentReloadTime == 0))
            {
                shootWait = 1f / speed;
                currentAmmo--;
                if (!isRaycast)
                {
                    if (attackTransform != null)
                    {
                        Instantiate(attackObj, attackTransform.position, attackTransform.rotation);
                        if(fireSfx != null)
                            Instantiate(fireSfx, attackTransform.position, attackTransform.rotation);
                    }
                    else
                    {
                        Instantiate(attackObj, aimDir.position, Quaternion.Euler(0, 0, aimAngle));
                        if (fireSfx != null)
                            Instantiate(fireSfx, aimDir.position, Quaternion.Euler(0, 0, aimAngle));
                    }
                    if (useOffset)
                    {
                        offsetStage = !offsetStage;
                        if (reloadTime > 0)
                            offsetAnim = 1f;
                    }
                }
                else
                {
                    RaycastHit2D rh = Physics2D.Raycast(aimDir.position, (Vector2)(Quaternion.Euler(0, 0, aimAngle) * Vector2.right), 50f, raycastMask);
                    if(rh.collider != null)
                    {
                        Instantiate(attackObj, rh.point, Quaternion.Euler(0, 0, aimAngle));
                        if (fireSfx != null)
                        {
                            if(attackTransform != null)
                                Instantiate(fireSfx, attackTransform.position, Quaternion.Euler(0, 0, aimAngle));
                            else
                                Instantiate(fireSfx, aimDir.position, Quaternion.Euler(0, 0, aimAngle));
                        }
                        Health hp = rh.collider.GetComponent<Health>();
                        if(hp != null)
                        {
                            hp.TakeDamage(damage);
                        }
                        Rigidbody2D rhrb = rh.collider.GetComponent<Rigidbody2D>();
                        if(rhrb != null)
                        {
                            rhrb.AddForceAtPosition((rhrb.transform.position - (Vector3)rh.point).normalized * pushBack, rh.point, ForceMode2D.Impulse);
                        }
                    }
                }


                if(knockback != 0)
                {
                    applyKnockback = (Quaternion.Euler(0, 0, aimAngle) * Vector2.right).normalized * knockback;
                    playerRB.transform.position += (Vector3)applyKnockback;
                }
            }
            else
            {
                Reload();
            }
        }
    }
    public void Reload()
    {
        if (canReload && currentReloadTime == 0)
        {
            if (usesAmmo)
                currentAmmo = magazine;
            currentReloadTime = reloadTime;
            baseReloadAngle = aimDir.localRotation.eulerAngles.z;
        }
    }

    public void RotateGun(float angle)
    {

        float reloadAmount;
        reloadAmount = 1f - (currentReloadTime / reloadTime);

        if (useOffset)
        {
            if (angle > 180)
            {
                weaponRenderer.sortingOrder = 11;
            }
            else
            {
                weaponRenderer.sortingOrder = 9;
            }

            if (offsetStage)
            {
                if(offsetAnim > 0)
                {
                    angle = angle + offsetAmount - Sslerp(0, offsetAmount * 2, offsetAnim);
                    weaponRenderer.flipY = true;
                }
                else
                {
                    angle += offsetAmount;
                    weaponRenderer.flipY = false;
                }
            }
            else
            {
                if (offsetAnim > 0)
                {
                    angle = angle - offsetAmount + Sslerp(0, offsetAmount * 2, offsetAnim);
                    weaponRenderer.flipY = false;
                }
                else
                {
                    angle -= offsetAmount;
                    weaponRenderer.flipY = true;
                }
                
            }
            if (angle < 0.0f) angle += 360.0f;
        }
        else
        {
            if (currentReloadTime == 0 || reloadAmount > 0.5f)
            {
                if (angle > 90 && angle < 270)
                {
                    weaponRenderer.flipY = true;
                }
                else
                {
                    weaponRenderer.flipY = false;
                }
            }
            
            if (angle > 165 || angle < 15)
            {
                weaponRenderer.sortingOrder = 11;
            }
            else
            {
                weaponRenderer.sortingOrder = 9;
            }
        }

        if (currentReloadTime > 0)
        {
            float temp;
            temp = Mathf.LerpAngle(baseReloadAngle, angle, reloadAmount);
            if (baseReloadAngle<90 || baseReloadAngle > 270)
            {
                angle = temp + Slerp(0, 360f * reloadSpins, reloadAmount);
            }
            else
            {
                angle = temp - Slerp(0, 360f * reloadSpins, reloadAmount);
            }
            
        }

        aimDir.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Stow(bool s)
    {
        if (s)
        {
            isStowed = true;
            weaponRenderer.flipX = false;
            weaponRenderer.flipY = false;
            weaponRenderer.sortingOrder = 9;
            aimDir.localPosition = stowedOffset;
            aimDir.rotation = Quaternion.Euler(0, 0, stowedRotation);
        }
        else
        {
            isStowed = false;
            aimDir.rotation = Quaternion.identity;
            aimDir.localPosition = Vector3.zero;
            transform.localPosition = offset;
        }
    }

    float Slerp(float a, float b, float t)
    {
        //t = t * t * t * (t * (6f * t - 15f) + 10f); //smoother step
        //t = t * t * (3f - 2f * t); // smooth step
        t = Mathf.Sin(t * Mathf.PI * 0.5f); //ease out
        return Mathf.Lerp(a, b, t);
    }
    float Sslerp(float a, float b, float t)
    {
        //t = t * t * t * (t * (6f * t - 15f) + 10f); //smoother step
        //t = t * t * (3f - 2f * t); // smooth step
        t = Mathf.Sin(t * Mathf.PI * 0.5f); //ease out
        return Mathf.Lerp(a, b, t);
    }
}
