﻿using FarmGame.Farming;
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
        Tool SelectedTool { get; }
        FieldController FieldController { get; }
    }
}