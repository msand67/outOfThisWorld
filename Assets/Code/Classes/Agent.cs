using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    public class Agent
    {
        public Dictionary<StatType, int> statList;
        string name;

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
        }
        public Agent(Dictionary<StatType, int> iStatList, string iName)
        {
            statList = iStatList;
            name = iName;
        }
    }
}