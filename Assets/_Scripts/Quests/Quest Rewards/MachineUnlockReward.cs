using Sirenix.OdinInspector;
using SmelterGame.Crafting;
using UnityEngine;

namespace SmelterGame.Quests
{
    [CreateAssetMenu(fileName = "New Machine Unlock", menuName = "Quests/Rewards/Machine Unlock")]
    public class MachineUnlockReward : SerializedScriptableObject, IRewardable, IMachineUnlockReward
    {
        [SerializeField]
        private IProcessorDefinition _unlockedMachine;

        public IProcessorDefinition GetUnlockedProcessor() => _unlockedMachine;
    }

    public interface IMachineUnlockReward : IRewardable
    {
        IProcessorDefinition GetUnlockedProcessor();
    }
}
