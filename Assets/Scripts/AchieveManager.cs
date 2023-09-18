using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;
    WaitForSecondsRealtime wait;
    enum Achieve { UnlockPotato,UnlockBean}
    Achieve[] achieves;
    private void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("MyData"))
            Init();
    }
    private void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);
        foreach(Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }
    private void Start()
    {
        UnlockCharacter();
    }
    private void UnlockCharacter()
    {
        for(int i = 0;i<lockCharacter.Length;i++)
        {
            string achieveName = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            Debug.Log(isUnlock);
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }
    private void LateUpdate()
    {
        foreach(Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }
    private void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;
        switch(achieve)
        {
            case Achieve.UnlockPotato:
                isAchieve = GameManager.instance.kill >= 10;
                break;
            case Achieve.UnlockBean:
                isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }
        if(isAchieve&&PlayerPrefs.GetInt(achieve.ToString())==0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);
            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achieve;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        yield return new WaitForSeconds(5);
        uiNotice.SetActive(false);
    }
}
