using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoughtsAndCrosses
{
    public class GameAssets : MonoBehaviour
    {
        public static GameAssets iGameAssets;

        //Contains the Different states of each button on the screen, blank, naught, and cross.
        public static Sprite[] img_States; //Used for all scripts.
        public Sprite[] img_StatesNonStatic; //Assigned to img_States on awake.

        private void Awake()
        {
            iGameAssets = this;

            //Must assign these before game starts aka awake.
            AssignImageStates();
        }

        //Assigns image states to a static variable that can be grabbed more easily.
        public void AssignImageStates()
        {
            img_States = img_StatesNonStatic;
        }

    }
}