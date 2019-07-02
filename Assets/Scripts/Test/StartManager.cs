using Barrage.ObjectPool;
using FlyCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public void StartGame()
    {
        ObjectPools.Instance.ResetPools();
        FlyCharacterManager.Instance.ResetCharacter();
        SceneManager.LoadScene("GameScene");
    }

    public void FinishGame()
    {
        Application.Quit();
    }
}
