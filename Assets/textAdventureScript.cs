using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using System.Text.RegularExpressions;

public class textAdventureScript : MonoBehaviour
{
    public KMAudio audio;
    public KMBombInfo bomb;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;
   {
    public bool typingActive; //This bool will determine whether the typing interface is active.
    public TextMesh textToEdit; //This is the physical textmesh that is going to display your text

    private void Start() //Runs once at the Start
    {
        textToEdit.text = ""; //Sets the textmesh text to blank/empty
    }

    public void EnableTextInterface() //Link this to a button on the module that you want to press in order to activate the input interface.
    {
        typingActive = !typingActive; //sets true if false and false if true
    }

    void Update() //This updates every frame
    {
        if (typingActive) //If the bool is active (the typing interface will not work if this is false)
        {
            foreach (char c in Input.inputString) //Magic clever coding get all the letters stuff
            {
                if (c == '\b') //If character is backspace...
                {
                    if (textToEdit.text.Length != 0) //If text mesh text length is not 0
                    {
                        textToEdit.text = textToEdit.text.Substring(0, textToEdit.text.Length - 1); //Remove last letter from string
                    }
                }
                else if ((c == '\n') || (c == '\r')) //Or, if you press enter (optional for KTaNE - could do a button or similar)
                {
                    typingActive = false; //Disable the typing interface
                }
                else if (textToEdit.text.Length > 25) //If your text length is greater than 25 (edit accordingly)
                {
                    return; //Cancel the operation and do nothing
                }
                else //Otherwise
                {
                    textToEdit.text += c; //Add the letter you pressed to the string
                }
            }
        }
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} press yes [Presses the yes button] | !{0} press no [Presses the no button] | !{0} reset [Resets all inputs] | Yes's and No's can be chained";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*reset\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            madeAns = "";
            Debug.LogFormat("[Faulty Digital Root #{0}] Reset of inputs triggered! (TP)", moduleId);
            inputs.GetComponent<MeshRenderer>().material = ledmats[0];
            yield break;