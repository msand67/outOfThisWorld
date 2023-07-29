using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    [Serializable]
    public class Agent
    {
        public List<Stat> statList;
        public string name;
        public int id;

        public Agent()
        {
            statList = new List<Stat>();
            statList.Add(new Stat(StatType.Breach, 0));
            statList.Add(new Stat(StatType.Egghead, 0));
            statList.Add(new Stat(StatType.Smoothtalk, 0));
            statList.Add(new Stat(StatType.Gumshoe, 0));
            statList.Add(new Stat(StatType.Footwork, 0));
            statList.Add(new Stat(StatType.FastHands, 0));

            name = "Murphy";
            id = -1;
        }
        public Agent(List<Stat> iStatList, string iName, int iId)
        {
            statList = iStatList;
            name = iName;
            id = iId;
        }
        public int GetCheckMod(CheckType type)
        {
            return (int)statList[(int)type].type;
        }

        internal double ComputeTime(double time, AgentAction action)
        {
            switch(action){
                case AgentAction.MakeCheck:
                    return time * (1-(statList[(int)StatType.FastHands].level * 0.05));
                case AgentAction.Move:
                    return time * (1-(statList[(int)StatType.Footwork].level * 0.05));
                default:
                    return time;
            }
        }
    }

    [Serializable]
    public class Stat
    {
        public StatType type;
        public int level;

        public Stat(StatType iType, int iLevel)
        {
            type = iType;
            level = iLevel;
        }
    }
}