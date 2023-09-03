using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace dataStructures
{

    [Serializable]
    public class Room : MonoBehaviour
    {
        [SerializeField]
        public Check check;
        public bool isEntrance;
        [SerializeField]
        public TMPro.TMP_Text textBox;
        public int id;
        public string description;
        private UnityEngine.UI.Image imageBackground;
        private SpriteRenderer spriteBackground;
        bool isUIVersion;

        public void init(Check newCheck, bool iIsEntrance, int iId, string iDesc)
        {

            check = newCheck;
            isEntrance = iIsEntrance;
            id = iId;
            description = iDesc;
            SetupBackground();
        }
        public void SetupBackground()
        {

            UnityEngine.UI.Image image = this.GetComponentInChildren<UnityEngine.UI.Image>();
            SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                spriteBackground = sprite;
                isUIVersion = false;
            }
            if (image != null)
            {
                imageBackground = image;
                isUIVersion = true;
            }
        }

        public void UpdateTooltipText(int roomId)
        {
            textBox.GetComponent<Tooltip>().message = $"Room {id}\nCheck type: {check.type}\nCleared: {check.isComplete}";
        }

        public void UpdatePlanningDescription(bool isStillHidden)
        {
            string s;
            if (check.isRequired) { s = ""; } else { s = "not"; };
            string desc = $"Room {id}\n";
            if (isStillHidden)
            {
                textBox.text = desc + "Hidden room, no details available.";
                return;
            }
            if (isEntrance)
            {
                desc += "(Entrance)\n";
            }
            desc += $"Type: {check.type.ToString()}\n";
            desc += $"Difficulty: {check.difficulty.ToString()}\n";

            textBox.text = desc;


        }
        public string GetRoomDescription(bool isStillHidden)
        {

            string s;
            if (check.isRequired) { s = ""; } else { s = "not"; };
            string desc = $"Room {id}\n";
            if (isStillHidden)
            {
                return desc + "Further information about this room will be hidden until an agent enters it.";
            }
            if (isEntrance)
            {
                desc += "(Entrance)\n";
            }
            desc += $"Check Details:\nType: {check.type.ToString()}\n";
            desc += $"Difficulty: {check.difficulty.ToString()}\n";
            desc += $"Penalty: {check.penalty.ToString()}\n";
            desc += $"Takes {check.timeToExecute}s to attempt\n";

            return desc;
        }

        internal void DisplayRequiredStatus(bool isRequiredSatisfied)
        {
            if (!check.isRequired)
            {
                return;
            }
            if (isRequiredSatisfied)
            {
                if (isUIVersion)
                {
                    imageBackground.color = new Color(0.3f, 0.3f, 1, 0.5f);
                }
                else
                {
                    spriteBackground.color = new Color(0.3f, 0.3f, 1, 0.5f);
                }
            }
            else
            {
                if (isUIVersion)
                {
                    imageBackground.color = new Color(1, 0.3f, 0.3f, 0.5f);
                }
                else
                {
                    spriteBackground.color = new Color(1, 0.3f, 0.3f, 0.5f);
                }

            }
        }

        internal void HideHighlight()
        {
            if (isUIVersion)
            {
                imageBackground.color = new Color(0.3f, 0.3f, 1, 0f);
            }else
            {
                spriteBackground.color = new Color(0.3f, 0.3f, 1, 0f);
            }
        }
    }
}
