using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
   [SerializeField] private Image image;
    public float health = 100.0f;
    public GameObject Explosion;
    void Update()
    {
        image.fillAmount = health/100;
        if(health <=0)
        { Death();}    
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
            health -= 25.0f;
        }
    }
    void Death()
    {
        Instantiate(Explosion,transform.position,Quaternion.identity);
        Destroy(gameObject);

    }
}
