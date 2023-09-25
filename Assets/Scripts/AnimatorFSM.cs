using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class AnimatorFSM : MonoBehaviour
{
    private Animator animator;
    [SerializeField] public Transform player;
    [SerializeField] Transform[] waypoints;
    [SerializeField]
    private float rotationSpeed = 10.0f;
    [SerializeField] private float movementSpeed = 5.0f;

    [SerializeField] public Transform currentWaypoint;

    private float attackmodetimer = 0;
    private float shottimer = 0;

    public GameObject bullet;
    private Transform turret;
    private Transform bulletSpawnPoint;

    private int HP = 8;
    public GameObject Explosion;


    void Start()
    {
        animator = GetComponent<Animator>();
        SetWaypoint();


        turret = gameObject.transform.GetChild(0).transform;
        bulletSpawnPoint = turret.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateDistance(currentWaypoint, "distanceFromWaypoint");
        CalculateDistance(player, "distanceFromPlayer");
        AttackTimer();
        LifeHandler();
    }

    public void MoveTo(Transform target)
    {


            Quaternion targetRotation = Quaternion.LookRotation(target.position -
    transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                rotationSpeed * Time.deltaTime);
            //Make our tank move forward since it should already face the target
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);





    }



    public void SetWaypoint()
    {
        //randomize the target position and move towards it
        int randomIndex = Random.Range(0, waypoints.Length);

        //Make sure that the new target is not the same as the previous target
        while (waypoints[randomIndex] == currentWaypoint)
            randomIndex = Random.Range(0, waypoints.Length);

        //set our target to the random waypoint
        currentWaypoint = waypoints[randomIndex];
    }

    private void CalculateDistance(Transform target, string parameterId)
    {
        float distance = Vector3.Distance(
            this.transform.position,
            target.position);
        animator.SetFloat(parameterId, distance);
    }

    private void AttackTimer()
    {
        attackmodetimer += 5f * Time.deltaTime;
        if(attackmodetimer >= 75f)
        {
            attackmodetimer = 75f;
        }
        animator.SetFloat("AttackTimer",attackmodetimer);

    }

    public void AttackTimerReset()
    {
        attackmodetimer = 0;
    }

    public void TurretLook(bool atplayer)
    {
        Quaternion targetRotation;
        if (atplayer == true)
        {
            targetRotation = Quaternion.LookRotation(new Vector3(player.position.x, turret.transform.position.y, player.position.z) -
                 turret.transform.position);
        }
        else
        {
            targetRotation = transform.rotation;
        }


        turret.rotation=  Quaternion.Slerp(transform.GetChild(0).transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);
    }

    public void ShootInterval()
    {
        shottimer += 5f * Time.deltaTime;

        if (shottimer >= 3f)
        {
            Shoot();
            shottimer = 0;

        }

    }

    private void Shoot()
    {
        Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }

    private void LifeHandler()
    {
        transform.GetChild(1).transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3( HP /8f, 1, 1);

        if(HP<=0)
        {
            this.gameObject.SetActive(false);
            Instantiate(Explosion,this.transform.position, Quaternion.identity);
        }    
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Bullet>() != null && collision.gameObject.GetComponent<Bullet>() == true)
        {
            HP -= 1;
        }
    }
}
