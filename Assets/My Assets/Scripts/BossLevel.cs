using UnityEngine;

public class BossLevel : MonoBehaviour
{
    [SerializeField] Nav_Movement nav_Movement;
   
    private void Awake()
    {
        nav_Movement.isReached = true;
    }
    public void StartMoving()
    {
        nav_Movement.isReached = false;
        nav_Movement.StartMoving();
    }

   
}
