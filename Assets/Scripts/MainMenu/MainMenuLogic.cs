using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Apple;
using System;
public class MainMenuLogic : MenuBase
{
    public TMP_InputField datalogName_InputField;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void updateDataloggerName() { 
        string name = datalogName_InputField.text;
        Debug.Log("Name change to: " + name);
    }

    public void createNewDataloggerCollection() {
        Debug.Log("Create a new datalogger collection");
    }
    /*
    public void setBotType(bool nextbot)
    {
        GameManager.Instance.setUsingNextBot(nextbot);
    }
    */
    /// <summary>
    /// Sets the bot type based on the specified integer value.
    /// Acceptable values: 
    /// 0 - GOAP Bot
    /// 1 - NextBot
    /// 2 - MemeBot
    /// </summary>
    /// <param name="botType">
    /// Integer representing the bot type. 
    /// Must be 0 (GOAP), 1 (NextBot), or 2 (MemeBot).
    /// </param>
    public void SetBotTypeEnum(int botType)
    {
        // hacky way of using buttons to set the bot type
        if (botType < 0 || botType > 2)
        {
            throw new ArgumentOutOfRangeException(nameof(botType), "Bot type must be between 0 and 2.");
        }

        switch (botType)
        {
            case 0:
                GameManager.Instance.setBotType(BotType.Goap);
                break;
            case 1:
                GameManager.Instance.setBotType(BotType.NextBot);
                break;
            case 2:
                GameManager.Instance.setBotType(BotType.MemeBot);
                break;
        }
    }




}
