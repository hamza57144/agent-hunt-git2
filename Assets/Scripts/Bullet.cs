/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IE.RSB;

public class Bullet : MonoBehaviour
{


    [SerializeField] private Transform m_visuals = null;
    [SerializeField] private float m_rotateSpeed = 30000f;
    [SerializeField] GameObject bulletVisuals;
    [SerializeField] GameObject bulletTrails;



    // Start is called before the first frame update
    void Start()
    {
        GameManager mgr = GameManager.instance;

        for (int i = 0; i < bulletVisuals.transform.childCount; i++) {
           bulletVisuals.transform.GetChild(i).gameObject.SetActive(false);
        }
        bulletVisuals.transform.GetChild(mgr.weaponProperties[mgr.CurrentGunIndex].bullet).gameObject.SetActive(true);


        for (int i = 0; i < bulletTrails.transform.childCount; i++)
        {
            bulletTrails.transform.GetChild(i).gameObject.SetActive(false);
        }
        bulletTrails.transform.GetChild(mgr.weaponProperties[mgr.CurrentGunIndex].trailEffect).gameObject.SetActive(true);

    }

    private void OnEnable()
    {
        SniperAndBallisticsSystem.EPenetrationInHit += SniperAndBallisticsSystem_EPenetrationInHit;
        SniperAndBallisticsSystem.EBulletTimeStarted += SniperAndBallisticsSystem_EBulletTimeStarted;
    }

    private void OnDisable()
    {
        SniperAndBallisticsSystem.EPenetrationInHit -= SniperAndBallisticsSystem_EPenetrationInHit;
        SniperAndBallisticsSystem.EBulletTimeStarted -= SniperAndBallisticsSystem_EBulletTimeStarted;
    }


    private void SniperAndBallisticsSystem_EPenetrationInHit(BulletPoint bulletPoint)
    {
        bulletVisuals.SetActive(false);
        bulletTrails.SetActive(false);
    }



    private void SniperAndBallisticsSystem_EBulletTimeStarted(Transform bullet, Transform hitTarget, ref List<BulletPoint> bulletPath, float totalDistance)
    {
        bulletVisuals.SetActive(true);
        bulletTrails.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        m_visuals.localEulerAngles += new Vector3(0, 0, -m_rotateSpeed * Time.unscaledDeltaTime * SniperAndBallisticsSystem.instance.BulletTimeVirtualTimescale);        

    }

}
*/