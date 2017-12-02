namespace Interfaces
{
    public interface IMovable
    {
        float TurnSpeed { get; }
        float MovementSpeed { get; }
        void Move(float targetFactor);
        void Turn(float targetFactor);
        void Disable();
    }
}
