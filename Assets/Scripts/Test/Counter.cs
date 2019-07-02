using Barrage.ObjectPool;
using FlyCharacter;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    private Counter() { }
    private static Counter instance;
    public static Counter Instance
    {
        get
        {
            if (instance == null) instance = new Counter();
            return instance;
        }
    }

    [SerializeField] private Text TimerText = null;
    [SerializeField] private Text AttackCountText = null;
    [SerializeField] private Text AttackedCountText = null;
    [SerializeField] private Text ResultText = null;

    private float timerCount = 60.0f;
    private readonly ReactiveProperty<int> attackCount = new ReactiveProperty<int>(0);
    private readonly ReactiveProperty<int> attackedCount = new ReactiveProperty<int>(0);

    public IReadOnlyReactiveProperty<int> AttackCount => attackCount;
    public IReadOnlyReactiveProperty<int> AttackedCount => attackedCount;

    public void Attack() => attackCount.SetValueAndForceNotify(attackCount.Value + 1);
    public void Attacked() => attackedCount.SetValueAndForceNotify(attackedCount.Value + 1);

    void Start()
    {
        instance = this;

        this.UpdateAsObservable().Where(_ => timerCount > 0).Subscribe(_ =>
        {
            timerCount -= Time.deltaTime;
            TimerText.text = "Time：" + ((int)timerCount / 60).ToString() + ":" + (timerCount % 60).ToString();
            if (timerCount < 0)
            {
                ShowResult();
                Destroy(gameObject, 5f);
            }
        },

            () =>
            {
                ObjectPools.Instance.ResetPools();
                FlyCharacterManager.Instance.ResetCharacter();
                SceneManager.LoadScene("StartScene");
            }
        );

        attackCount.DistinctUntilChanged().Subscribe(x => AttackCountText.text = "Attack" + x.ToString());
        attackedCount.DistinctUntilChanged().Subscribe(x => AttackedCountText.text = "Attacked" + x.ToString());
    }

    private void ShowResult()
    {
        var result = "";
        if (AttackCount.Value > AttackedCount.Value) result += "YOU WIN \n";
        else if (AttackCount.Value < AttackedCount.Value) result += "YOU LOSE \n";
        else result += "DROW \n";

        result += "please wait. you'll be back to start menu.";

        ResultText.gameObject.SetActive(true);
        ResultText.text = result;
    }
}
