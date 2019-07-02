using Barrage.ObjectPool;
using FlyCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        ObjectPools.Instance.ResetPools();
        FlyCharacterManager.Instance.ResetCharacter();
        SceneManager.LoadScene("3DBarrageTest");
    }
}
