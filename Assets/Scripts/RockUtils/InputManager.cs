using UnityEngine;
using XInputDotNetPure;

public class InputManager
{
    public enum Buttons {
        A,
        B,
        X,
        Y,

        Left,
        Right,
        Up,
        Down,

        Left_Bumper,
        Right_Bumper,

        Reset,
        Select,
    }

    public enum Stick {
        Left,
        Right,
    }

    readonly PlayerIndex indexID;
    GamePadState prevState;
    GamePadState state;

    public InputManager(PlayerIndex indexID) {
        this.indexID = indexID;
    }

    public void update() {
        prevState = state;
        state = GamePad.GetState(indexID);
    }

    //  Returns a Vector3 for ease of use
    public Vector3 getStick(Stick stick) {
        if (stick == Stick.Left) {
            return new Vector3(state.ThumbSticks.Left.X, 0, state.ThumbSticks.Left.Y);
        } else {
            return new Vector3(state.ThumbSticks.Right.X, 0, state.ThumbSticks.Right.Y);
        }
    }

    public bool isUp(Buttons button) {
        if (state.IsConnected) {
            switch (button) {
                case Buttons.A:
                    return isReleased(state.Buttons.A);
                case Buttons.B:
                    return isReleased(state.Buttons.B);
                case Buttons.X:
                    return isReleased(state.Buttons.X);
                case Buttons.Y:
                    return isReleased(state.Buttons.Y);

                case Buttons.Left:
                    return isReleased(state.DPad.Left);
                case Buttons.Right:
                    return isReleased(state.DPad.Right);
                case Buttons.Up:
                    return isReleased(state.DPad.Up);
                case Buttons.Down:
                    return isReleased(state.DPad.Down);

                case Buttons.Left_Bumper:
                    return isReleased(state.Buttons.LeftShoulder);
                case Buttons.Right_Bumper:
                    return isReleased(state.Buttons.RightShoulder);

                case Buttons.Reset:
                    return isReleased(state.Buttons.Back);
                case Buttons.Select:
                    return isReleased(state.Buttons.Start);
            }
        }
        return false;
    }

    public bool isDown(Buttons button) {
        if (state.IsConnected) {
            switch (button) {
                case Buttons.A:
                    return isPressed(state.Buttons.A);
                case Buttons.B:
                    return isPressed(state.Buttons.B);
                case Buttons.X:
                    return isPressed(state.Buttons.X);
                case Buttons.Y:
                    return isPressed(state.Buttons.Y);

                case Buttons.Left:
                    return isPressed(state.DPad.Left);
                case Buttons.Right:
                    return isPressed(state.DPad.Right);
                case Buttons.Up:
                    return isPressed(state.DPad.Up);
                case Buttons.Down:
                    return isPressed(state.DPad.Down);

                case Buttons.Left_Bumper:
                    return isPressed(state.Buttons.LeftShoulder);
                case Buttons.Right_Bumper:
                    return isPressed(state.Buttons.RightShoulder);

                case Buttons.Reset:
                    return isPressed(state.Buttons.Back);
                case Buttons.Select:
                    return isPressed(state.Buttons.Start);
            }
        }
        return false;
    }

    public bool wasPressed(Buttons button) {
        if (state.IsConnected) {
            switch (button) {
                case Buttons.A:
                    return isReleased(prevState.Buttons.A) && isPressed(state.Buttons.A);
                case Buttons.B:
                    return isReleased(prevState.Buttons.B) && isPressed(state.Buttons.B);
                case Buttons.X:
                    return isReleased(prevState.Buttons.X) && isPressed(state.Buttons.X);
                case Buttons.Y:
                    return isReleased(prevState.Buttons.Y) && isPressed(state.Buttons.Y);

                case Buttons.Left:
                    return isReleased(prevState.DPad.Left) && isPressed(state.DPad.Left);
                case Buttons.Right:
                    return isReleased(prevState.DPad.Right) && isPressed(state.DPad.Right);
                case Buttons.Up:
                    return isReleased(prevState.DPad.Up) && isPressed(state.DPad.Up);
                case Buttons.Down:
                    return isReleased(prevState.DPad.Down) && isPressed(state.DPad.Down);

                case Buttons.Left_Bumper:
                    return isReleased(prevState.Buttons.LeftShoulder) && isPressed(state.Buttons.LeftShoulder);
                case Buttons.Right_Bumper:
                    return isReleased(prevState.Buttons.RightShoulder) && isPressed(state.Buttons.RightShoulder);

                case Buttons.Reset:
                    return isReleased(prevState.Buttons.Back) && isPressed(state.Buttons.Back);
                case Buttons.Select:
                    return isReleased(prevState.Buttons.Start) && isPressed(state.Buttons.Start);
            }
        }
        return false;
    }

    private bool isPressed(ButtonState state) {
        return state == ButtonState.Pressed;
    }

    private bool isReleased(ButtonState state) {
        return state == ButtonState.Released;
    }
}