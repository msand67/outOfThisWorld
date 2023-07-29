using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    public class Agent
    {
        public Dictionary<StatType, int> statList;
        public string name;
        public int id;

        public Agent()
        {
            statList = new Dictionary<StatType, int>();
            statList.Add(StatType.Breach, 0);
            statList.Add(StatType.Egghead, 0);
            statList.Add(StatType.FastHands, 0);
            statList.Add(StatType.Footwork, 0);
            statList.Add(StatType.Gumshoe, 0);
            statList.Add(StatType.Smoothtalk, 0);

            name = "Murphy";
            id = -1;
        }
        public Agent(Dictionary<StatType, int> iStatList, string iName, int iId)
        {
            statList = iStatList;
            name = iName;
            id = iId;
        }
        public int GetCheckMod(CheckType type)
        {
            return statList[(StatType)type];
        }

        internal double ComputeTime(double time, AgentAction action)
        {
            switch(action){
                case AgentAction.MakeCheck:
                    return time * (1-(statList[StatType.FastHands] * 0.05));
                case AgentAction.Move:
                    return time * (1-(statList[StatType.Footwork] * 0.05));
                default:
                    return time;
            }
        }
    }
}