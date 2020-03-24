public enum GameEvents {
    KeyboardButton_Pressed = 0,
    //  We're reserving the first 512 ids for KeyCode support, probably don't really need this many but that's what we're doing...
    //  Just for safety we'll resume the values at 600

    //  Mouse Actions
    Mouse_Left_Press = 600,
    Mouse_Right_Press = 601,
    Mouse_Middle_Press = 602,

    Mouse_Left_Release = 603,
    Mouse_Right_Release = 604,
    Mouse_Middle_Release = 605,

    Mouse_Left_Held = 606,
    Mouse_Right_Held = 607,
    Mouse_Middle_Held = 608,

    //  Ability Use - 10 values reserved (QWERT + Ctrl-QWERT)
    Ability_Use = 613,

    //  Updating Ability Cooldowns - 10 values reserved (QWERT + Ctrl-QWERT)
    Ability_Cooldown_Update = 623,

    //  Set Ability Max Cooldown - 10 values reserved (QWERT + Ctrl-QWERT)
    Ability_Cooldown_Max_Update = 633,

    //  Toggle Ability - 10 value reserved (QWERT + Ctrl-QWERT)
    Ability_Toggle = 643,

    Health_Changed = 700,
    Mana_Changed = 701,

    Entity_Stop_Movement = 702,
}
