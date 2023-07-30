using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace dataStructures {
    public class Room : MonoBehaviour
    {
        public Check check;
        public bool isEntrance;
        [SerializeField]
        TMPro.TextMeshProUGUI textBox;
        public int id;
        private void Start()
        {

            check = new Check(CheckType.Breach);
            isEntrance = false;
            id = -1;
        }

        public void Init(Check newCheck, bool iIsEntrance, int iId)
        {

            check = newCheck;
            isEntrance = iIsEntrance;
            id = iId;
        }

        public void UpdateTooltipText(int roomId)
        {
            textBox.GetComponent<Tooltip>().message = $"Room {roomId}\nCheck type: {check.type}\nCleared: {check.isComplete}";
        }


    }
}
