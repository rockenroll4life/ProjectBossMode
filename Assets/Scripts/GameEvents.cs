public enum GameEvents {
    KeyboardButton_Pressed = 0,
    //  We're reserving the first 512 ids for KeyCode support, probably don't really need this many but that's what we're doing...
    //  Just for safety we'll resume the values at 600

    //  Mouse Clicks
    Mouse_LeftClick = 600,
    Mouse_RightClick = 601,
    Mouse_MiddleClick = 602,

    //  Ability Use - 10 values reserved (QWERT + Ctrl-QWERT)
    Ability_Use = 603,

    //  Updating Ability Cooldowns - 10 values reserved (QWERT + Ctrl-QWERT)
    Ability_Cooldown_Update = 613,

    //  Set Ability Max Cooldown - 10 values reserved (QWERT + Ctrl-QWERT)
    Ability_Cooldown_Max_Update = 623,

    Health_Changed = 633,
    Mana_Changed = 634,
}
