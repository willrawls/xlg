using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using MetX.Standard.Library;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using WilliamPersonalMultiTool.Custom;
using User32 = Win32Interop.Methods.User32;
using Win32Interop.Enums;
using Win32Interop.Structs;

namespace WilliamPersonalMultiTool
{
    public class WindowWorker
    {
        public static RECT Offset(int left, int top, int right, int bottom)
        {
            return new RECT
            {
                left = left,
                top = top,
                right =  right,
                bottom = bottom,
            };
        }

        public static RECT Offset(int offset)
        {
            return new RECT
            {
                left = offset,
                top = offset,
                right =  -offset*2,
                bottom = -offset*2,
            };
        }

        public List<RECT> WindowOffsets { get; set; } = new()
        {
            Offset(10),
            Offset(20),
            Offset(30),
            Offset(40),
            Offset(0),
            Offset(-10),
            Offset(-20),
            Offset(-30),
            Offset(-40),
        };

        public List<RECT> WindowPositions { get; set; } = new()
        {
            new RECT {left = 108, top = 29, right = 1913, bottom = 1070},   // D1
            new RECT {left = 143, top = 63, right = 1779, bottom = 1016},   // D2
            new RECT {left = 364, top = 10, right = 1465, bottom = 600},    // D3
            new RECT {left = 364, top = 99, right = 1465, bottom = 989},    // D4
            new RECT {left = 586, top = 388, right = 1325, bottom = 818},   // D5 
            new RECT {left = 1373, top = 730, right = 1916, bottom = 1070}, // D6

            new RECT {left = 1383, top = 9, right = 1926, bottom = 731},    // D7
            new RECT {left = 1923, top = 333, right = 3180, bottom = 967},  // D8
            new RECT {left = 1935, top = 337, right = 3281, bottom = 1077}, // D9
            new RECT {left = 2000, top = 348, right = 3225, bottom = 1056}, // D0
        };

        public int CurrentPosition = 0;
        
        public int CurrentScreen = 0;
        public int CurrentCorner = 0;

        public CustomPhraseManager Manager { get; }
        public static IntPtr ParentHandle { get; private set; }
        public List<CustomKeySequence> Sequences { get; set; }

        public WindowWorker(CustomPhraseManager Manager, IntPtr parentHandle)
        {
            this.Manager = Manager;
            ParentHandle = parentHandle;
            Sequences = new List<CustomKeySequence>
            {
                new("Window to position 1", new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.ControlKey, PKey.D1}, OnMoveCurrentWindowToPosition),
                new("Window to position 2", new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.ControlKey, PKey.D2}, OnMoveCurrentWindowToPosition),
                new("Window to position 3", new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.ControlKey, PKey.D3}, OnMoveCurrentWindowToPosition),
                new("Window to position 4", new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.ControlKey, PKey.D4}, OnMoveCurrentWindowToPosition),
                new("Window to position 5", new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.ControlKey, PKey.D5}, OnMoveCurrentWindowToPosition),
                new("Window to position 6", new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.ControlKey, PKey.D6}, OnMoveCurrentWindowToPosition),
                new("Window to position 7", new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.ControlKey, PKey.D7}, OnMoveCurrentWindowToPosition),
                new("Window to position 8", new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.ControlKey, PKey.D8}, OnMoveCurrentWindowToPosition),
                new("Window to position 9", new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.ControlKey, PKey.D9}, OnMoveCurrentWindowToPosition),

                new("Window to upper left", new List<PKey> {PKey.ControlKey, PKey.Shift, PKey.D1}, OnMoveCurrentWindowToCorner),
                new("Window to upper right", new List<PKey> {PKey.ControlKey, PKey.Shift, PKey.D2}, OnMoveCurrentWindowToCorner),
                new("Window to lower right", new List<PKey> {PKey.ControlKey, PKey.Shift, PKey.D3}, OnMoveCurrentWindowToCorner),
                new("Window to lower left", new List<PKey> {PKey.ControlKey, PKey.Shift, PKey.D4}, OnMoveCurrentWindowToCorner),

                new("Window to screen 2 upper left", new List<PKey> {PKey.ControlKey, PKey.Shift, PKey.D7}, OnMoveCurrentWindowToCorner),
                new("Window to screen 2 upper right", new List<PKey> {PKey.ControlKey, PKey.Shift, PKey.D8}, OnMoveCurrentWindowToCorner),
                new("Window to screen 2 lower right", new List<PKey> {PKey.ControlKey, PKey.Shift, PKey.D9}, OnMoveCurrentWindowToCorner),
                new("Window to screen 2 lower left", new List<PKey> {PKey.ControlKey, PKey.Shift, PKey.D9}, OnMoveCurrentWindowToCorner),

                new("Window to next position", new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.RShiftKey}, OnMoveCurrentWindowToNextPosition),
                new("Window to previous position", new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.LShiftKey}, OnMoveCurrentWindowToPreviousPosition),

                new("Get window position", new List<PKey> {PKey.ControlKey, PKey.ControlKey, PKey.G, PKey.W}, OnGetWindowPosition),
            };
            Sequences.ForEach(s =>
            {
                s.BackColor = Color.Aqua;
                s.ForeColor = Color.Black;
            });
        }

        public RECT CalculateCornerForRECT(RECT rect, int screen, int corner)
        {
            Rectangle screenBounds;
            if (screen == 0)
            {
                screenBounds = Screen.PrimaryScreen.WorkingArea;
            }
            else
            {
                screenBounds = Screen.AllScreens[^1].WorkingArea;
            }

            var originalWindowPosition = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            var newWindowPosition = originalWindowPosition;
            
            if (corner > 3) corner = 0;
            if (corner < 0) corner = 3;

            //                          left, top
            switch (corner)
            {
                // 0: upper left corner is 0, 0 offset from screenBounds, width and height the same
                case 1:
                    newWindowPosition = new Rectangle(0, 0, originalWindowPosition.Width, originalWindowPosition.Height);
                    break;

                // 1: upper right corner is screen.right-window.width, 0
                case 2:
                    newWindowPosition = new Rectangle(screenBounds.Right-originalWindowPosition.Width, 0, originalWindowPosition.Width, originalWindowPosition.Height);
                    break;

                // 2: lower left corner is 0, screen.bottom-window.height offset from screenBounds
                case 3:
                    newWindowPosition = new Rectangle(0, screenBounds.Bottom - originalWindowPosition.Height, originalWindowPosition.Width, originalWindowPosition.Height);
                    break;

                // 3: lower right corner is screen.right-window.width, screen.bottom-window.height
                case 0:
                    newWindowPosition = new Rectangle(screenBounds.Right-originalWindowPosition.Width, screenBounds.Bottom-originalWindowPosition.Height, originalWindowPosition.Width, originalWindowPosition.Height);
                    break;
            }

            var cornerRECT = new RECT
            {
                left = newWindowPosition.Left,
                top = newWindowPosition.Top,
                right = newWindowPosition.Right,
                bottom = newWindowPosition.Bottom,
            };
            return cornerRECT;
        }

        public void OnGetWindowPosition(object sender, PhraseEventArguments e)
        {
            Manager.SendBackspaces(2);

            var handle = User32.GetForegroundWindow();
            if (handle != IntPtr.Zero)
            {
                WINDOWINFO info = new();
                info.cbSize = (uint) Marshal.SizeOf(info);
                User32.GetWindowInfo(handle, ref info);

                var answer = $"            new RECT {{left = {info.rcWindow.left}, top = {info.rcWindow.top}, right = {info.rcWindow.right}, bottom = {info.rcWindow.bottom}}},";
                Clipboard.SetText(answer);
            }
        }

        public void OnMoveCurrentWindowToCorner(object sender, PhraseEventArguments e)
        {
            var triggered = e.State.KeySequence;
            var entry = triggered.Sequence[^1] - PKey.D0;

            var backspaceCount = triggered.BackspacesToSend();
            Manager.SendBackspaces(backspaceCount);

            if(entry is >= 0 and <= 8)
            {
                var handle = User32.GetForegroundWindow();
                if (handle == ParentHandle)
                    return;

                if (handle == IntPtr.Zero) return;

                User32.GetWindowRect(handle, out var startingWindowPosition);
                var newPosition = CalculateCornerForRECT(startingWindowPosition, 0, entry);
                MoveForegroundWindowTo(newPosition);

            }
        }

        public void OnMoveCurrentWindowToPosition(object sender, PhraseEventArguments e)
        {
            var triggered = e.State.KeySequence;
            var entry = triggered.Sequence[^1] - PKey.D0;

            var backspaceCount = triggered.BackspacesToSend();
            Manager.SendBackspaces(backspaceCount);

            if(entry is >= 0 and <= 9)
            {
                CurrentPosition = entry;

                var p = WindowPositions[entry];
                MoveForegroundWindowTo(p);
            }
        }

        public void OnMoveCurrentWindowToPreviousPosition(object sender, PhraseEventArguments e)
        {
            CurrentPosition--;
            if (CurrentPosition < 0)
            {
                CurrentPosition = 9;
            }
            var entry = CurrentPosition;

            if (entry is < 1 or > 9) return;

            var p = WindowPositions[entry - 1];
            MoveForegroundWindowTo(p);
        }

        public void OnMoveCurrentWindowToNextPosition(object sender, PhraseEventArguments e)
        {
            CurrentPosition++;
            if (CurrentPosition > 9)
            {
                CurrentPosition = 0;
            }
            var entry = CurrentPosition;

            if (entry is < 1 or > 9) return;

            var p = WindowPositions[entry - 1];
            MoveForegroundWindowTo(p);
        }

        public void OnMoveTo00(object sender, PhraseEventArguments e)
        { 
            Manager.SendBackspaces(2);
            MoveForegroundWindowTo(null);
        }

        public static RECT? GetForegroundWindowPosition()
        {
            var handle = User32.GetForegroundWindow();

            if (handle == IntPtr.Zero)
                return null;

            return User32.GetWindowRect(handle, out var position) 
                ? position 
                : null;
        }

        public static void MoveForegroundWindowTo(RECT? p)
        {
            var handle = User32.GetForegroundWindow();

            if (handle == ParentHandle)
                return;

            if (handle != IntPtr.Zero)
            {
                SWP flags;

                if (p == null)
                {
                    flags = SWP.SWP_SHOWWINDOW | SWP.SWP_NOSIZE;
                    User32.SetWindowPos(handle, IntPtr.Zero, 50, 50, 0, 0, flags);
                }
                else
                {
                    var rect = ShiftABit(p.Value);
                    flags = SWP.SWP_SHOWWINDOW;
                    User32.SetWindowPos(handle, IntPtr.Zero, 
                        rect.left, 
                        rect.top, 
                        rect.right - rect.left, 
                        rect.bottom - rect.top,
                        flags);
                }
            }
        }

        public static int AmountOfShift = -10;
        private static RECT ShiftABit(RECT rect)
        {
            AmountOfShift += 10;
            if (AmountOfShift >= 50)
                AmountOfShift = -20;

            var shifted = new RECT
            {
                left = rect.left + AmountOfShift,
                top = rect.top + AmountOfShift,
                right = rect.right, // + amount,
                bottom = rect.bottom, // + amount,
            };
            return shifted;
        }
    }
}
