namespace Barrage.Bullet
{
    public enum AttackerType
    {
        Player, Enemy, The3rdParty
    }
    
    public interface IAttacker
    {
        AttackerType GetAttackerType();
    }
}
