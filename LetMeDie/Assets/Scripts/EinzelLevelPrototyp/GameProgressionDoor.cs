using Essentials;
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
    [SerializeField] private bool isEndDoor;
    [SerializeField] private Sprite goBackSprite;

    public void Init(SGameProgressionManager.RewardTyp typ)
    {
        animator = GetComponent<Animator>();
        rewardTyp = typ;
        if (!forceInteractable)
        {
            col = GetComponent<Collider>();
            col.enabled = false;
        }
        else
        {
            animator.SetTrigger("Open");
        }
        lootImage.gameObject.SetActive(false);
    }

    internal void ShowDoors()
    {
        if (isEndDoor) {
            lootImage.sprite = goBackSprite;
            lootImage.gameObject.SetActive(true);
        }
        else
        {

            if(rewardTyp == SGameProgressionManager.RewardTyp.None)
            {
                lootImage.sprite = SGameProgressionManager.Instance.GetBoonIcon(rewardTyp);
                lootImage.gameObject.SetActive(true);
            }    

        }
        col.enabled = true;
        animator.SetTrigger("Open");
    }

    public void OnInteract(Transform player)
    {
        if (isEndDoor)
        {
            SUIManager.Instance.ChangeToUIState("GameEnd");
        }
        else
        {
            SGameProgressionManager.Instance.LoadNextRoom(rewardTyp);
        }
    }
}
