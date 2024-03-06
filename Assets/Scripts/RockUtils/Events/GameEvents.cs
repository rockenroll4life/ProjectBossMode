namespace RockUtils {
    namespace GameEvents {
        public enum GameEvents {
            //========================================================
            //  Input Game Events
            //========================================================
            //  We're reserving the first 512 ids for KeyCode support, probably don't really need this many but that's what we're doing...
            //  Just for safety we'll resume the values at 600
            KeyboardButton_Pressed = 0,

            //  We're reserving the next 512 ids for KeyCode support, probably don't really need this many but that's what we're doing...
            //  Just for safety we'll resume the values at 1200
            KeyboardButton_Released = 600,

            //  We're reserving the next 512 ids for KeyCode support, probably don't really need this many but that's what we're doing...
            //  Just for safety we'll resume the values at 1800
            KeyboardButton_Held = 1200,
            
            //  Mouse Actions
            Mouse_Left_Press = 1800,
            Mouse_Right_Press = 1801,
            Mouse_Middle_Press = 1802,

            Mouse_Left_Release = 1803,
            Mouse_Right_Release = 1804,
            Mouse_Middle_Release = 1805,

            Mouse_Left_Held = 1806,
            Mouse_Right_Held = 1807,
            Mouse_Middle_Held = 1808,

            Mouse_Left_Move_X = 1809,
            Mouse_Right_Move_X = 1810,
            Mouse_Middle_Move_X = 1811,

            Mouse_Left_Move_Y = 1812,
            Mouse_Right_Move_Y = 1813,
            Mouse_Middle_Move_Y = 1814,

            Mouse_Scroll_Wheel = 1815,

            //========================================================
            //  Entity Game Events
            //========================================================
            //  Ability Actions - 5 values reserved for each (QWERT)
            Ability_Press = 1900,

            Ability_Release = 1905,

            Ability_Held = 1910,

            //  NOTE: [Rock]: Currently used by the UI to set the toggle...however I don't like it...we should setup this as a generic
            //  Ability_Use event with a separate enum list of what the param id means
            Ability_Toggle = 1925,
            Ability_Channel_Start = 1930,
            Ability_Channel_Stop = 1935,

            //  Entity Data Changed - 20 values reserved
            Entity_Data_Changed = 1940,

            Entity_Stop_Movement = 1960,

            //  Targeting Game Events
            Targeted_Entity = 1961,
            Targeted_World = 1962,

            LivingEntity_Hurt = 1963,

            //========================================================
            //  Global Game Events
            //========================================================
            Game_Paused = 2000,
            Keybindings_Changed = 2001,
            GameplaySettings_Changed = 2002,
        }
    }
}