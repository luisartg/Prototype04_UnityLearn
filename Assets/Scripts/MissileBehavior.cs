using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    private GameObject target = null;
    private bool targetLocked = false;
    public float speed = 10;
    public float initialBoost = 2;
    public float explosionForce = 10;
    private bool targetDestroyed = false;
    private ParticleSystem explosionPS;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        explosionPS = gameObject.transform.Find("Fx_OilSplashHIGH_Root").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetLocked && !targetDestroyed)
        {
            if (target == null)
            {
                Destroy(gameObject);
            }
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        try
        {
            direction = target.transform.position - transform.position;
        }
        catch (Exception) 
        {
            Destroy(gameObject);
            return;
        }
        MissileLookDirection();
        MissileTranslation();
        CheckForEffect();
    }

    private void MissileTranslation()
    {
        if (direction.magnitude < 2)
        {
            transform.Translate(direction.normalized * (speed * initialBoost * Time.deltaTime), Space.World);
        }
        else
        {
            transform.Translate(direction.normalized * (speed * Time.deltaTime), Space.World);
        }
    }

    private void MissileLookDirection()
    {
        Vector3 norm = direction.normalized;
        transform.localRotation = Quaternion.LookRotation(norm, norm);
    }

    private void CheckForEffect()
    {
        if (direction.magnitude < 0.5)
        {
            targetDestroyed = true;
            ActivateExplosionVisuals();
            PushTarget();
            StartCoroutine(DestroyAfterWait());
        }
    }

    private void ActivateExplosionVisuals()
    {
        GameObject missileVisual = gameObject.transform.Find("RotationVisual").gameObject;
        missileVisual.SetActive(false);
        explosionPS.Play();
    }

    private void PushTarget()
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        Vector3 forceDirection = new Vector3(direction.normalized.x, Mathf.Sin(45), direction.normalized.z);
        targetRb.AddForce(forceDirection * explosionForce, ForceMode.Impulse);
    }

    IEnumerator DestroyAfterWait()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public void SetTarget(GameObject t)
    {
        target = t;
        targetLocked = true;
    }
}
