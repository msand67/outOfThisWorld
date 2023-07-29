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
        private void Start()
        {

            check = new Check(CheckType.Breach);
            isEntrance = false;
        }

        public void Init(Check newCheck, bool iIsEntrance)
        {

            check = newCheck;
            isEntrance = iIsEntrance;
        }

        public void UpdateTooltipText(int roomId)
        {
            textBox.GetComponent<Tooltip>().message = $"Room {roomId}\nCheck type: {check.type}\nCleared: {check.isComplete}";
        }


    }
}
