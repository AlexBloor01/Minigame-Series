using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GridNumbers : MonoBehaviour
{
    [Header("Text and Numbers for the Grid")]
    //Grid Size Numbers.
    //start them both at 3 to give players the idea of changing the size.
    public int x = 3;
    public int y = 3;

    //Text in scenes for x and y value.
    [SerializeField] private TMP_InputField textAreaX;
    [SerializeField] private TMP_InputField textAreaY;


    //Update the grid size in Game Manager by editing and returning user inputted grid sizes.
    public IntVector2 GridSize()
    {
        //This is a quick fix, before Implementation of multi-size Noughts and Crosses. Add FIX later.
        //For the time being this stops having to edit two values for the sake of one.
        // y = x;

        //Create a new value based on user inputted x and y.
        IntVector2 newSize = new IntVector2(x, y);

        //Make the grid square.
        // newSize = ConvertToHighestNumber(newSize);

        //Convert new value.
        x = newSize.x;
        y = newSize.y;

        //Update Text to new Grid size.
        UpdateTextAreas();

        return newSize;
    }

    //Button to Plus X.
    public void PlusX()
    {
        Increment(ref x);
    }

    //Button to Minus X.
    public void MinusX()
    {
        Decrement(ref x);
    }

    //Button to Set X to 3.
    public void ResetX()
    {
        x = 3;
        UpdateTextAreas();
    }

    //Button to Plus Y.
    public void PlusY()
    {
        Increment(ref y);
    }

    //Button to Minus Y.
    public void MinusY()
    {
        Decrement(ref y);
    }

    //Button to Set Y to 3.
    public void ResetY()
    {
        y = 3;
        UpdateTextAreas();
    }

    //Increase Value selected.
    private void Increment(ref int value)
    {
        //Stop values from exceeding below 1 and above infinity.
        ClampValues();
        value++;

        //Assign new Value to text area.
        UpdateTextAreas();
    }

    //Decrease Value selected.
    private void Decrement(ref int value)
    {
        //Stop values from exceeding below 1 and above infinity.
        ClampValues();
        value--;

        //Assign new Value to text area.
        UpdateTextAreas();
    }

    public void OnEndEditInputField()
    {
        TMP_InputField thisInputField = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();

        int which = 0;
        if (thisInputField.text == "")
        {
            thisInputField.text = which.ToString();
        }
        else
        {
            int newValue = int.Parse(thisInputField.text);
            which = newValue;
            thisInputField.text = newValue.ToString();

            if (thisInputField == textAreaX)
            {
                x = which;
            }
            else
            {
                y = which;
            }
        }

        UpdateTextAreas();
    }

    //Update both text areas to show new values.
    public void UpdateTextAreas()
    {
        textAreaX.text = x.ToString();
        textAreaY.text = y.ToString();
    }

    //Stop values from exceeding below 1 and above infinity.
    void ClampValues()
    {
        x = (int)Mathf.Clamp(x, 1, Mathf.Infinity);
        y = (int)Mathf.Clamp(y, 1, Mathf.Infinity);
    }

    //Converts to the grid to the highest number out of the two in size, turns it into a square.
    IntVector2 ConvertToHighestNumber(IntVector2 size)
    {
        if (size.x > size.y)
        {
            size.y = size.x;
        }
        if (size.y > size.x)
        {
            size.x = size.y;
        }
        return size;
    }

}
