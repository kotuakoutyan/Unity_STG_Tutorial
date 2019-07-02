using System.Collections.Generic;
using UniRx;

namespace FlyCharacter
{
    public class FlyCharacterManager
    {
        private FlyCharacterManager() { }
        private static FlyCharacterManager instance;
        public static FlyCharacterManager Instance
        {
            get
            {
                if (instance == null) instance = new FlyCharacterManager();
                return instance;
            }
        }

        public ReactiveCollection<FlyCharacterStatus> Characters = new ReactiveCollection<FlyCharacterStatus>();

        public void ResetCharacter()
        {
            Characters = new ReactiveCollection<FlyCharacterStatus>();
        }
    }
}