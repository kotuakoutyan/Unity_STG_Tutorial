using UnityEngine;
using Inputs;
using UniRx;
using UniRx.Triggers;

public class AutoInputEventProvider : MonoBehaviour, IInputEventProvider
{
    [SerializeField] GameObject Player = null;

    private readonly ReactiveProperty<bool> a_Button = new ReactiveProperty<bool>(false);
    private readonly ReactiveProperty<bool> b_Button = new ReactiveProperty<bool>(false);
    private readonly ReactiveProperty<bool> x_Button = new ReactiveProperty<bool>(false);
    private readonly ReactiveProperty<bool> y_Button = new ReactiveProperty<bool>(false);
    private readonly ReactiveProperty<bool> l_Button = new ReactiveProperty<bool>(false);
    private readonly ReactiveProperty<bool> r_Button = new ReactiveProperty<bool>(false);
    private ReactiveProperty<Vector3> l_Stick = new ReactiveProperty<Vector3>();
    private ReactiveProperty<Vector3> r_Stick = new ReactiveProperty<Vector3>();

    public IReadOnlyReactiveProperty<bool> A_Button => a_Button;
    public IReadOnlyReactiveProperty<bool> B_Button => b_Button;
    public IReadOnlyReactiveProperty<bool> X_Button => x_Button;
    public IReadOnlyReactiveProperty<bool> Y_Button => y_Button;
    public IReadOnlyReactiveProperty<bool> L_Button => l_Button;
    public IReadOnlyReactiveProperty<bool> R_Button => r_Button;
    public IReadOnlyReactiveProperty<Vector3> L_Stick => l_Stick;
    public IReadOnlyReactiveProperty<Vector3> R_Stick => r_Stick;
    
    private Timer Attack_Timer = new Timer(3.0f, 10.0f);
    private Timer Move_Timer = new Timer(1.0f, 3.0f);
    private Quaternion Direction;
    private ReactiveProperty<bool>[] ReactiveProperties;
    private ReactiveProperty<bool> GetRandomReactiveProperty => ReactiveProperties[Random.Range(0, ReactiveProperties.Length)];
    
    void Start()
    {
        Player = GameObject.FindWithTag("Player");

        ReactiveProperties = new ReactiveProperty<bool>[]
        {
            x_Button, y_Button, l_Button, r_Button
        };
        this.UpdateAsObservable().Select(_ => Attack_Timer.UpdateTimer()).DistinctUntilChanged().Subscribe(x => GetRandomReactiveProperty.Value = x);
        this.UpdateAsObservable().Where(_ => Move_Timer.UpdateTimer()).Select(_ => GetVelocity(transform.position - Player.transform.position)).Subscribe(x => l_Stick.SetValueAndForceNotify(x));
    }

    private struct Timer
    {
        public float timer;
        private float timer_min;
        private float timer_max;

        public Timer(float timer_max, float timer_min)
        {
            this.timer = 0.0f;
            this.timer_min = timer_min;
            this.timer_max = timer_max;
        }

        public bool UpdateTimer()
        {
            if (timer > 0) { timer -= Time.deltaTime; return false; }
            else { timer = Random.Range(timer_min, timer_max); return true; }
        }
    }

    [SerializeField] int CloseRange = 40;
    [SerializeField] int LeaveRange = 20;
    private Vector3 GetVelocity(Vector3 input)
    {
        var distance = input.magnitude;
        if (distance < LeaveRange) Direction = Quaternion.Euler(Mathf.Max(180 - (LeaveRange - distance) * 2.5f, 135f), 0, 0);
        else if (distance > CloseRange) Direction = Quaternion.Euler(Mathf.Max(90 - (distance - CloseRange) * 2.5f, 0f), 0, 0);
        else Direction = Quaternion.Euler(Random.Range(-45, 45), 0, 0);

        var velocity = Direction * Vector3.forward;

        velocity =Quaternion.Euler(0, 0, Random.Range(0, 360)) * velocity;
        
        //上下移動へのはとりあえず消しておく
        return new Vector3(velocity.x, 0, velocity.z);
    }
}
