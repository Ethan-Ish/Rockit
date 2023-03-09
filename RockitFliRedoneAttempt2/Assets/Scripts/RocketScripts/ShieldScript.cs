using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldScript : MonoBehaviour
{

    public GameObject rocket;
    public GameObject shield;
    public GameData gData;
    public PolygonCollider2D rocketCollider;
    public PolygonCollider2D shieldCollider;
    public ParticleSystem particles;
    public ParticleGenerator particleManager;
    public Image healthMeter;
    public GameObject healthMeterBack;
    public float healDelay;
    public int shieldHealth;
    public int maxShieldHealth;
    public bool isEnabled;
    public Sprite enabledShieldSprite;
    public Sprite disabledShieldSprite;

    private float count;
    private defaultRocketFunc rFunc = new defaultRocketFunc();

    private void OnEnable()
    {
        healthMeterBack.SetActive(true);
        count = 0;
        isEnabled = true;
        shieldHealth = maxShieldHealth;
    }

    private void OnDisable()
    {
        healthMeterBack.SetActive(false);
    }

    public void regenShield()
    {
        if(!gData.flying) { return; } // Don't regenerate if the rocket isn't flying
        count += Time.fixedDeltaTime;
        if (shieldHealth == maxShieldHealth)
        {
            count = 0;
        }
        if (count >= healDelay)
        {
            count = 0;
            shieldHealth += 1;
        }
    }

    public void checkShield()
    {
        isEnabled = !(shieldHealth == 0);
        if(!gData.flying) { isEnabled = false; }
        rocketCollider.enabled = !isEnabled;
        shieldCollider.enabled = isEnabled;
        shield.SetActive(isEnabled);
        if(isEnabled)
        {
            rocket.GetComponent<SpriteRenderer>().sprite = enabledShieldSprite;
        }
        else
        {
            rocket.GetComponent<SpriteRenderer>().sprite = disabledShieldSprite;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObj = collision.gameObject;
        if(isEnabled && shieldCollider.enabled && !(collidedObj.layer == 8))
        {
            shieldHealth -= 1;
            gData.rocketHealth += 1;
            Vector3 pos;
            pos.x = collision.GetContact(0).point.x;
            pos.y = collision.GetContact(0).point.y;
            pos.z = -4;
            particleManager.spawnParticles(particles, pos);
        }
    }

    private void FixedUpdate()
    {
        regenShield();
        checkShield();
        rFunc.updateBar(maxShieldHealth, shieldHealth, healthMeter, 12.4f);
    }

}
