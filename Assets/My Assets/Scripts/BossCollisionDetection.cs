using UnityEngine;

public class BossCollisionDetection : MonoBehaviour
{
    public GameObject[] playerCover;
    public GameObject[] bossCover;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagsHandler.Enemy))
        {
            PlayerCoverEnable(false);
            BossCoverEnable(false );
        }
        if (other.gameObject.CompareTag(TagsHandler.Player))
        {
           PlayerCoverEnable(true);
           BossCoverEnable(true );
        }
    }
    private void PlayerCoverEnable(bool enable)
    {
        foreach (var item in playerCover)
        {
            item.SetActive(enable);
        }
    }
    private void BossCoverEnable(bool enable)
    {
        foreach (var item in bossCover)
            { item.SetActive(enable); }
    }
}
