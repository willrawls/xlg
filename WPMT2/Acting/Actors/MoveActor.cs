using System;
using System.Windows.Forms;
using MetX.Standard.Strings;
using NHotPhrase.Phrase;
using Win32Interop.Structs;

namespace WilliamPersonalMultiTool.Acting.Actors
{
    public class MoveActor : BaseActor
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Verb Add { get; set; }        
        public Verb To { get; set; }        
        public Verb Relative { get; set; }
        public Verb Resize { get; set; }
        public Verb Percent { get; set; }   

        public MoveActor()
        {
            ActionableType = ActionableType.Move;

            Add = AddLegalVerb("add");
            To = AddLegalVerb("to");
            Percent = AddLegalVerb("percent", To);
            Relative = AddLegalVerb("relative");
            Resize = AddLegalVerb("size");

            OnAct = Act;
        }

        //
        //  Add      = Add/subtract to/from current position and optionally resize on current or target screen
        //  To       = Move to position and optionally resize on current or target screen
        //  Relative = Move to relative position and optionally resize on current or target screen
        //  Resize     = Resize on current or target screen
        //             2 Coordinates= width, height 
        //
        //Modifiers
        // +Percent  = Coordinates are percentages of the target/current screen
        //
        // 2 coordinates= left, top
        // 3 coordinates= left, top, screen
        // 4 coordinates= left, top, width, height   (primary screen)
        // 5 coordinates= left, top, width, height, screen
        //

        public override bool Initialize(string item)
        {
            if (!base.Initialize(item))
                return false;

            var tokens = Arguments.AllTokens(" ", StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Count != 4)
            {
                Errors = "Move actor: wrong number of arguments";
                return false;
            }

            if (Resize.Mentioned)
            {
                if(tokens.Count == 2)
                {
                    Width = tokens[0].AsInteger();
                    Height = tokens[1].AsInteger();
                }
                else if (tokens.Count == 3)
                {
                    Width = tokens[0].AsInteger();
                    Height = tokens[1].AsInteger();
                    TargetScreen = tokens[2].AsInteger() - 1;
                }
                else
                {
                    throw new Exception("Too many or too few parameters for the mentioned verbs.");
                }
            }
            else if(tokens.Count == 4)
            {
                Left = tokens[0].AsInteger();
                Top = tokens[1].AsInteger();
                Width = tokens[2].AsInteger();
                Height = tokens[3].AsInteger();
                Right = Left + Width;
                Bottom = Top + Height;
            }
            else if(tokens.Count == 5)
            {
                Left = tokens[0].AsInteger();
                Top = tokens[1].AsInteger();
                Width = tokens[2].AsInteger();
                Height = tokens[3].AsInteger();
                TargetScreen = tokens[4].AsInteger() - 1;

                Right = Left + Width;
                Bottom = Top + Height;
            }
            else
            {
                throw new Exception("Too many or too few parameters for the mentioned verbs.");
            }

            return true;
        }

        public int TargetScreen { get; set; }

        public int Bottom { get; set; }

        public int Right { get; set; }

        public bool Act(PhraseEventArguments phraseEventArguments)
        {
            if (Errors.IsNotEmpty())
                return true;
            
            var origin = WindowWorker.GetForegroundWindowPosition();
            if (!origin.HasValue) return false;

            var mousePosition = Cursor.Position;
            var currentScreen = Screen.FromPoint(mousePosition);
            var currentScreenIndex = currentScreen.Index();
            var newPosition = CalculateNewPosition(currentScreenIndex, origin.Value);
            WindowWorker.MoveForegroundWindowTo(newPosition);
            return true;
        }

        public RECT CalculateNewPosition(int currentScreen, RECT origin)
        {
            var targetScreen = TargetScreen < 1
                ? Screen.AllScreens[currentScreen]
                : Screen.AllScreens[TargetScreen];

            if (Add.Mentioned)
            {
                return CalculateNewAddPosition(origin, targetScreen);
            }

            if (To.Mentioned)
            {
                return CalculateNewToPosition(targetScreen);
            }

            if (Relative.Mentioned)
            {
                return CalculateNewRelativePosition(origin, targetScreen);
            }

            if (Resize.Mentioned)
            {
                return CalculateNewSize(origin, targetScreen);
            }
            
            RECT newPosition;
            if(To.Mentioned) // To means not Percent
            {
                if(Relative.Mentioned)
                {
                    // To + Relative (-Percent)
                    // Additive
                    newPosition = new RECT
                    {
                        left = origin.left + Left,
                        top = origin.top + Top,
                        right = origin.right + Width,
                        bottom = origin.bottom + Height,
                    };
                }
                else
                {
                    // To alone (-Percent -Relative)
                    // Absolute
                    newPosition = new RECT
                    {
                        left = Left,
                        top = Top,
                        right = Right,
                        bottom = Bottom,
                    };
                }
            }
            else if(Relative.Mentioned)
            {
                // Relative alone (-Top +Percent)
                // Percent == true
                // Relative
                var screen = Screen.AllScreens[TargetScreen];
                var onePercentX = screen.WorkingArea.Width / 100;
                var onePercentY = screen.WorkingArea.Height / 100;

                newPosition = new RECT
                {
                    left = origin.left + (Left * onePercentX),
                    top = origin.top + (Top * onePercentY),
                    right = origin.right + (Right * onePercentX),
                    bottom = origin.bottom + (Bottom * onePercentY),
                };
            }
            else
            {
                // Percent == true (-Top -Relative)
                // Absolute percent
                var onePercentX = targetScreen.PercentX();
                var onePercentY = targetScreen.PercentY();

                newPosition = new RECT
                {
                    left =  (Left * onePercentX),
                    top = (Top * onePercentY),
                    right = (Right * onePercentX),
                    bottom = (Bottom * onePercentY),
                };
            }

            return newPosition;
        }

        private RECT CalculateNewSize(RECT origin, Screen targetScreen)
        {
            RECT newPosition;

            if (Percent.Mentioned)
            {
                var onePercentX = targetScreen.WorkingArea.Width / 100;
                var onePercentY = targetScreen.WorkingArea.Height / 100;

                newPosition = new RECT
                {
                    left =  origin.left + (Left * onePercentX),
                    top = origin.top + (Top * onePercentY),
                    right = origin.right + (Right * onePercentX),
                    bottom = origin.bottom + (Bottom * onePercentY),
                };

            }
            else
            {
                newPosition = new RECT
                {
                    left = origin.left + Left,
                    top = origin.top + Top,
                    right = origin.right + Right,
                    bottom = origin.bottom + Bottom,
                };
            }            
            return newPosition;
        }

        private RECT CalculateNewRelativePosition(RECT origin, Screen targetScreen)
        {
            if (!Percent.Mentioned)
                return new RECT
                {
                    left = origin.left + Left,
                    top = origin.top + Top,
                    right = origin.left + Left + (origin.right - origin.left) + Width,
                    bottom = origin.top + Top + (origin.bottom - origin.top) + Height,
                };

            var onePercentX = ((int) (targetScreen.Bounds.Width / 100f));
            var onePercentY = ((int) (targetScreen.Bounds.Height / 100f));

            var relativeLeft = onePercentX * Left;
            var relativeRight = onePercentX * Right;
            var relativeTop = onePercentY * Top;
            var relativeBottom = onePercentY * Bottom;
            var relativeWidth = relativeRight - relativeLeft;
            var relativeHeight = relativeBottom - relativeTop;

            return new RECT
            {
                left = origin.left + relativeLeft,
                top = origin.top + relativeTop,
                right = origin.left + relativeLeft + (origin.right - origin.left) + relativeWidth,
                bottom = origin.top + relativeTop + (origin.bottom - origin.top) + relativeHeight,
            };
        }

        private RECT CalculateNewToPosition(Screen targetScreen)
        {
            if (!Percent.Mentioned)
                return new RECT
                {
                    left = Left,
                    top = Top,
                    right = Right,
                    bottom = Bottom,
                };
            
            var onePercentX = ((int) (targetScreen.Bounds.Width / 100f));
            var onePercentY = ((int) (targetScreen.Bounds.Height / 100f));

            var relativeLeft = onePercentX * Left;
            var relativeRight = onePercentX * Right;
            var relativeTop = onePercentY * Top;
            var relativeBottom = onePercentY * Bottom;

            return new RECT
            {
                left = relativeLeft,
                top = relativeTop,
                right = relativeRight,
                bottom = relativeBottom,
            };
        }

        private RECT CalculateNewAddPosition(RECT origin, Screen targetScreen)
        {
            if (!Percent.Mentioned)
                return new RECT
                {
                    left = origin.left + Left,
                    top = origin.top + Top,
                    right = origin.left + Left + (origin.right - origin.left) + Width,
                    bottom = origin.top + Top + (origin.bottom - origin.top) + Height,
                };

            var onePercentX = ((int) (targetScreen.Bounds.Width / 100f));
            var onePercentY = ((int) (targetScreen.Bounds.Height / 100f));

            var relativeLeft = onePercentX * Left;
            var relativeRight = onePercentX * Right;
            var relativeTop = onePercentY * Top;
            var relativeBottom = onePercentY * Bottom;
            var relativeWidth = relativeRight - relativeLeft;
            var relativeHeight = relativeBottom - relativeTop;

            return new RECT
            {
                left = origin.left + relativeLeft,
                top = origin.top + relativeTop,
                right = origin.left + relativeLeft + (origin.right - origin.left) + relativeWidth,
                bottom = origin.top + relativeTop + (origin.bottom - origin.top) + relativeHeight,
            };
        }
    }
}