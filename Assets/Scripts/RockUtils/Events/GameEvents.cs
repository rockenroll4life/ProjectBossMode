﻿namespace RockUtils {
    namespace GameEvents {
        public enum GameEvents {
            //========================================================
            //  Input Game Events: 0 - 1899
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

            //  Controller Actions
            Controller_Stick_Left_X = 1820,
            Controller_Stick_Left_Y = 1821,

            Controller_Stick_Right_X = 1822,
            Controller_Stick_Right_Y = 1823,

            Controller_Trigger_Left = 1824,
            Controller_Trigger_Right = 1825,

            //  12 controller buttons reserved
            Controller_Button_Press = 1826,

            //  12 controller buttons reserved
            Controller_Button_Release = 1838,

            //  12 controller buttons reserved
            Controller_Button_Held = 1850,

            //========================================================
            //  Entity Game Events: 1900 - 1999
            //========================================================
            //  Ability Actions
            Ability_Press = 1900,
            Ability_Release = 1901,
            Ability_Held = 1902,

            Ability_Toggle = 1903,
            Ability_Channel_Start = 1904,
            Ability_Channel_Stop = 1905,

            //  Entity Data Changed - 20 values reserved
            Entity_Data_Changed = 1910,

            Entity_Stop_Movement = 1930,

            //  Targeting Game Events
            Targeted_Entity = 1940,
            Targeted_World = 1941,

            LivingEntity_Hurt = 1942,

            //========================================================
            //  Global Game Events: 2000 - ???
            //========================================================
            Game_Paused = 2000,
            Keybindings_Changed = 2001,
            GameplaySettings_Changed = 2002,
        }
    }
}