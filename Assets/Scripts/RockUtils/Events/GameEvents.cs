namespace RockUtils {
    namespace GameEvents {
        public enum GameEvents {
            //========================================================
            //  Input Game Events
            //========================================================
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

            //========================================================
            //  Entity Game Events
            //========================================================
            //  Ability Actions - 5 values reserved for each (QWERT)
            Ability_Press = 700,

            Ability_Release = 705,

            Ability_Held = 710,

            //  Updating Ability Cooldowns - 5 values reserved for each (QWERT)
            Ability_Cooldown_Update = 715,

            //  Set Ability Max Cooldown - 5 values reserved for each (QWERT)
            Ability_Cooldown_Max_Update = 720,

            //  NOTE: [Rock]: Currently used by the UI to set the toggle...however I don't like it...we should setup this as a generic
            //  Ability_Use event with a separate enum list of what the param id means
            Ability_Toggle = 725,

            //  TODO: [Rock]: We should change this to "Stat_Changed" and generify this
            Health_Changed = 730,
            Mana_Changed = 731,

            Entity_Stop_Movement = 732,

            //  Targeting Game Events
            Targeted_Entity = 733,
            Targeted_World = 734,

            //========================================================
            //  Global Game Events
            //========================================================
            Game_Paused = 800,
        }
    }
}