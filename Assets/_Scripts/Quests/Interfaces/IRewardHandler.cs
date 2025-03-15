namespace SmelterGame.Quests
{
    public interface IRewardHandler
    {
        void SetNext(IRewardHandler nextHandler);
        void HandleReward(IRewardable reward);
    }
}