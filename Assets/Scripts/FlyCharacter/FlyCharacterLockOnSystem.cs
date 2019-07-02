using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace FlyCharacter
{
    public class FlyCharacterLockOnSystem : MonoBehaviour
    {
        private FlyCharacterStatus Status;
        private List<FlyCharacterStatus> Characters = new List<FlyCharacterStatus>();
        private float LockOnCircle = 300;

        [SerializeField] private GameObject LockOnCursor = null;

        void Start()
        {
            Status = GetComponent<FlyCharacterStatus>();
            Characters = FlyCharacterManager.Instance.Characters.Where(c => c.AttackerType != Status.AttackerType).ToList();

            FlyCharacterManager.Instance.Characters.ObserveAdd()
                .Where(c => c.Value.AttackerType != Status.AttackerType)
                .Subscribe(x => Characters.Add(x.Value));

            FlyCharacterManager.Instance.Characters.ObserveRemove()
                .Where(c => c.Value.AttackerType != Status.AttackerType)
                .Subscribe(x => Characters.Remove(x.Value));
        }

        void Update()
        {
            LockOnUpdate();

            if(Status.Target != null) Debug.DrawLine(transform.position, Status.Target.transform.position, Color.red);
        }


        //ロックオン処理
        private void LockOnUpdate()
        {
            if (Characters.Count != 0)
            {
                var orderedCharacters = Characters.OrderBy(c => GetDisatanceFromCenter(c.gameObject.transform.position));
                foreach (FlyCharacterStatus character in orderedCharacters)
                {
                    //ロックオンサークル内の場合
                    if (GetDisatanceFromCenter(character.transform.position) < LockOnCircle)
                    {
                        //敵との間に障害物はとりあえず無視
                        //if (Physics.Linecast(transform.position, character.gameObject.transform.position) == false)
                        {
                            Status.Target = character.gameObject;
                            LockOnCursor.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, character.transform.position);
                            return;
                        }
                    }
                }
            }
            Status.Target = default;
            LockOnCursor.transform.position = new Vector3(-100, -100, 0);
        }
        
        private float GetDisatanceFromCenter(Vector3 position)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
            screenPoint.x = screenPoint.x - (Screen.width / 2);
            screenPoint.y = screenPoint.y - (Screen.height / 2);
            return screenPoint.magnitude;
        }
    }
}