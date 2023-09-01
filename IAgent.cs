using FarmGame.DataStorage.Inventory;
using FarmGame.Farming;
using FarmGame.Input;
using FarmGame.Interactions;
using FarmGame.Tools;

namespace FarmGame.Agent
{
    public interface IAgent
    {
        AgentAnimation AgentAnimation { get; }
        PlayerInputFarm AgentInput { get; }
        AgentMover AgentMover { get; }
        bool Blocked { get; set; }
        FieldDetector FieldDetectorObject { get; }
        InteractionDetector InteractionDetector { get; }
        ToolsBag ToolsBag { get; }
        FieldController FieldController { get; }

        public Inventory Inventory { get; }
        AgentDataSO AgentData { get; }
    }

}