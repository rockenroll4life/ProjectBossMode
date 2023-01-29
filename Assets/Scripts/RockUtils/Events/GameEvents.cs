namespace RockUtils
{
    namespace GameEvents
    {
        public enum GameEvents
        {
            //  TODO: [Rock]: We need to have these values not as hard coded so it's easier to add new entries without having to adjust a lot of numbers

            KeyboardButton_Pressed = 0,
            //  We're reserving the first 512 ids for KeyCode support, probably don't really need this many but that's what we're doing...
            //  Just for safety we'll resume the values at 600

            //  TODO: [Rock]: We should add another two blocks of events for button Released and Held

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

            Mouse_Left_Move_X = 609,
            Mouse_Right_Move_X = 610,
            Mouse_Middle_Move_X = 611,

            Mouse_Left_Move_Y = 612,
            Mouse_Right_Move_Y = 613,
            Mouse_Middle_Move_Y = 614,

            Mouse_Scroll_Wheel = 615,

            //  Ability Use - 10 values reserved (QWERT + Ctrl-QWERT)
            Ability_Use = 616,

            //  Updating Ability Cooldowns - 10 values reserved (QWERT + Ctrl-QWERT)
            Ability_Cooldown_Update = 626,

            //  Set Ability Max Cooldown - 10 values reserved (QWERT + Ctrl-QWERT)
            Ability_Cooldown_Max_Update = 636,

            //  Toggle Ability - 10 value reserved (QWERT + Ctrl-QWERT)
            Ability_Toggle = 646,

            Health_Changed = 703,
            Mana_Changed = 704,

            Entity_Stop_Movement = 705,

            //  Targetting Game Events
            Targeted_Entity = 706,
            Targeted_World = 707,
        }
    }
}