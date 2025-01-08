using CoverShooter;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance { get; set; }
    public GameManager gameManager;
    public Camera mainCamera;
    public Camera cineCamera;
    [SerializeField] Nav_Movement player;
    [SerializeField] Bullet bulletPrefab;
    private Transform bulletSpawnTransform;
    [SerializeField] private float minDistanceToPlayAnimation;
    [SerializeField] BulletTimeController bulletTimeController;
    [SerializeField] private float shootingForce;
    [SerializeField] ThirdPersonCamera thirdPersonCamera;
    private AIMovement aIMovement;
    private int enemyCount;
   
    public  bool finalShot;
    
    public bool shoot;
    private bool isHead;
    public bool lastEnemy { get { return enemyCount==1; } }
    public bool levelCompleted { get { return enemyCount == 0; } }
    bool once;
    
    public LayerMask layerMask;
    private bool isAiming;

    private Animator animator;

    
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        finalShot = false;
        enemyCount = player.TotalEnemies;
        CharacterMotor.EnemyDie += OnEnemyDie;

        
    }
    
    private void OnEnemyDie(object sender, System.EventArgs e)
    {

        Invoke(nameof(CountEnemies), 1f);
        
    }

    private void Update()
    {
        if (levelCompleted && !once)
        {
            Invoke(nameof(LevelComplete),5f);   
            once = true;
        }
        if (lastEnemy)
        {
         
            DetectBodyParts();

            Enemy enemy = FindObjectOfType<Enemy>();
            animator= enemy.gameObject.GetComponentInParent<Animator>();
            if (enemy != null)
            {
                if (gameManager.PlayerMotor.ActiveWeapon.Gun != null)
                {
                    if (gameManager.PlayerMotor.ActiveWeapon.Gun.myScope != null)
                    {
                        if (enemy.CheckHealth() < 75 || isHead)
                        {
                            if (isAiming)
                            {
                                finalShot = true;
                            }
                            else
                            {
                                finalShot = false;
                            }

                            if (shoot)
                            {
                                enemy.gameObject.GetComponentInParent<AIMovement>().enabled=false;
                              
                                Shoot();
                            }

                        }
                    }

                }
            }
        }
        
    }

    private void LevelComplete()
    {
        gameManager.LevelComplete();
    }
    
    private void DetectBodyParts()
    {
       
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
       /* Gizmos.color = Color.green;
        Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 100);*/
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            
            BodyPartDetection bodyPart = hit.collider.GetComponent<BodyPartDetection>();
            if (bodyPart != null)
            {


                if (bodyPart.bodyPart == BodyPart.head)
                {
                    
                    isHead = true;
                }
                else
                {
                    
                    isHead = false;
                }
            }
            else
            {
               
                isHead = false;
            }

        }

        if (Physics.Raycast(ray, out RaycastHit _hit))
        {
            if (_hit.collider.CompareTag("Enemy"))
            {
                isAiming = true;
            }
            else
            {
                isAiming = false;
                finalShot = false;
            }

        }
    }
    public void Shoot()
    {
        animator.enabled = false;

        finalShot = false;
        shoot = false;
        bulletSpawnTransform = GameManager.instance.PlayerMotor.ActiveWeapon.Gun.Aim.transform;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {

            BodyPartDetection bodyPart = hit.collider.GetComponent<BodyPartDetection>();
            
            Vector3 direction = hit.point - bulletSpawnTransform.position;
            if (bodyPart)
            {
              
                if (direction.magnitude >= minDistanceToPlayAnimation)
                {
                    gameManager.HideArrow();
                    Bullet bulletInstance = Instantiate(bulletPrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);
                    bulletInstance.Launch(shootingForce, hit.collider.transform, hit.point);
                    bulletTimeController.StartSequence(bulletInstance, hit.point);
                }
                else
                {
                  //  controller.OnEnemyShot(direction, hit.collider.GetComponent<Rigidbody>());
                }
            }
            else { Debug.Log("No Enemy"); }
        }
    }
    private void CountEnemies()
    {
        enemyCount--;
    }
 
    public void EnableAnimator()
    {
        if (animator!=null)
        {
            animator.enabled = true;
        }
        
    }
}
