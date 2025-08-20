using Components.Characters;

namespace Services.Player
{
    public interface IPlayerService
    {
        PlayerProxy PlayerProxy { get; }
    }
}