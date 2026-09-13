using System;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressionDoor : MonoBehaviour, IInteractable
{


    [SerializeField] private Image lootImage;
    private SGameProgressionManager.RewardTyp rewardTyp;
    [SerializeField] private bool forceInteractable;
    private Collider col;
    private Animator animator;

    public void Init(SGameProgressionManager.RewardTyp typ)
    {
        rewardTyp = typ;
        if (!forceInteractable)
        {
            col = GetComponent<Collider>();
            col.enabled = false;
        }
        lootImage.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    internal void ShowDoors()
    {
        lootImage.sprite = SGameProgressionManager.Instance.GetBoonIcon(rewardTyp);
        lootImage.gameObject.SetActive(true);
        col.enabled = true;
        animator.SetTrigger("Open");
    }

    public void OnInteract(Transform player)
    {
        SGameProgressionManager.Instance.LoadNextRoom(rewardTyp);
    }
}
