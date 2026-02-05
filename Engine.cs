namespace Consyl_Lite
{
    public static class Engine
    {
        const int WIDTH = 60;
        const int HEIGHT = 30;
        static readonly Graphics gfx = new(WIDTH, HEIGHT, DrawPixel, DrawText, DrawLine);
        static readonly char[] drawBuffer = new char[WIDTH * HEIGHT];

        static double timeSinceStart = 0.0;
        static double deltaTime = 0.0;
        static bool isRunning = true;
        static Game game = new(deltaTime, timeSinceStart, EndGame, IsKeyDown);

        static void Main()
        {
            Code.Start(game);
            
            while (isRunning)
            {
                long startTime = DateTimeOffset.UtcNow.Ticks;

                Code.Update(game);
                Code.Render(game, gfx);

                // Render to Screen
                Console.CursorVisible = false;
                string screenBuffer = "";
                for (int y = 0; y < HEIGHT; y++)
                    for (int x = 0; x < WIDTH; x++)
                        screenBuffer += (drawBuffer[WIDTH * y + x] < 32 ? ' ' : drawBuffer[WIDTH * y + x]).ToString() + 
                            (x == WIDTH - 1 && y != HEIGHT - 1 ? '\n' : ' ');
                Console.Write(screenBuffer);
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
                for (int y = 0; y < HEIGHT; y++)
                    for (int x = 0; x < WIDTH; x++)
                        drawBuffer[WIDTH * y + x] = (char)0;

                // Delta Time Calculation 
                deltaTime = new TimeSpan(DateTimeOffset.UtcNow.Ticks - startTime).TotalNanoseconds / 1000000000;
                timeSinceStart += deltaTime;
                game.deltaTime = deltaTime;
                game.timeSinceStart = timeSinceStart;
            }
            Code.End(game);
        }
        
        static void EndGame() => isRunning = false;

        static bool IsKeyDown(Key key) => EZInput.Keyboard.IsKeyPressed((EZInput.Key)key);

        static void DrawPixel(int x, int y, char c)
        {
            if (c < 32 || x < 0 || x >= WIDTH || y < 0 || y >= HEIGHT) return;
            drawBuffer[WIDTH * y + x] = c;
        }

        static void DrawText(int x, int y, string message)
        {
            int charX = x;
            int charY = y;

            foreach (char c in message)
            {
                switch (c)
                {
                    case '\n':
                        charX = x;
                        charY++;
                        continue;

                    case '\t':
                        charX = ((charX - x + 4) / 4) * 4 + x;
                        continue;
                }

                if (charX >= 0 && charX < WIDTH && charY >= 0 && charY < HEIGHT)
                    DrawPixel(charX, charY, c);

                charX++;
            }
        }

        static void DrawLine(int x0, int y0, int x1, int y1, char c)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;

            for (; ; )
            {
                DrawPixel(x0, y0, c);

                if (x0 == x1 && y0 == y1)
                    break;

                e2 = err;

                if (e2 > -dx)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dy)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }
    }

    public struct Game(
        double deltaTime, 
        double timeSinceStart, 
        Action EndGame,
        Func<Key, bool> IsKeyDown
    )
    {
        /// <summary>
        /// The time it takes in seconds to render a frame
        /// </summary>
        public double deltaTime = deltaTime;
        /// <summary>
        /// The time in seconds has passed since the beginning
        /// </summary>
        public double timeSinceStart = timeSinceStart;
        /// <summary>
        /// <para>Stops and ends game</para>
        /// <para>void EndGame()</para>
        /// </summary>
        public readonly Action EndGame = EndGame;
        /// <summary>
        /// <para>Returns true if the provided key is being pressed</para>
        /// <para>bool IsKeyDown(Key key)</para>
        /// </summary>
        public readonly Func<Key, bool> IsKeyDown = IsKeyDown;
    }

    public readonly struct Graphics(
        int width, 
        int height, 
        Action<int, int, char> DrawPixel, 
        Action<int, int, string> DrawText,
        Action<int, int, int, int, char> DrawLine
    )
    {
        /// <summary>
        /// Width of rendered screen
        /// </summary>
        public readonly int width = width;
        /// <summary>
        /// Height of rendered screen
        /// </summary>
        public readonly int height = height;
        /// <summary>
        /// <para>Draws an ASCII pixel c at (x, y) position</para>
        /// <para>void DrawPixel(int x, int y, char c)</para>
        /// </summary>
        public readonly Action<int, int, char> DrawPixel = DrawPixel;
        /// <summary>
        /// <para>Draws a message at (x, y) position</para>
        /// <para>void DrawText(int x, int y, string message)</para>
        /// </summary>
        public readonly Action<int, int, string> DrawText = DrawText;
        /// <summary>
        /// <para>Draws an ASCII line c between (x0, y0) and (x1, y1)</para>
        /// <para>void DrawLine(int x0, int y0, int x1, int y1, char c)</para>
        /// </summary>
        public readonly Action<int, int, int, int, char> DrawLine = DrawLine;
    }

    public enum Key
    {
        Backspace = EZInput.Key.Backspace,
        Tab = EZInput.Key.Tab,
        Enter = EZInput.Key.Enter,
        Shift = EZInput.Key.Shift,
        Control = EZInput.Key.Control,
        Escape = EZInput.Key.Escape,
        CapsLock = EZInput.Key.CapsLock,
        Space = EZInput.Key.Space,
        PageUp = EZInput.Key.PageUp,
        PageDown = EZInput.Key.PageDown,
        End = EZInput.Key.End,
        Home = EZInput.Key.Home,
        LeftArrow = EZInput.Key.LeftArrow,
        UpArrow = EZInput.Key.UpArrow,
        RightArrow = EZInput.Key.RightArrow,
        DownArrow = EZInput.Key.DownArrow,
        Select = EZInput.Key.Select,
        Printscreen = EZInput.Key.Printscreen,
        Insert = EZInput.Key.Insert,
        Delete = EZInput.Key.Delete,
        Key0 = EZInput.Key.Key0,
        Key1 = EZInput.Key.Key1,
        Key2 = EZInput.Key.Key2,
        Key3 = EZInput.Key.Key3,
        Key4 = EZInput.Key.Key4,
        Key5 = EZInput.Key.Key5,
        Key6 = EZInput.Key.Key6,
        Key7 = EZInput.Key.Key7,
        Key8 = EZInput.Key.Key8,
        Key9 = EZInput.Key.Key9,
        A = EZInput.Key.A,
        B = EZInput.Key.B,
        C = EZInput.Key.C,
        D = EZInput.Key.D,
        E = EZInput.Key.E,
        F = EZInput.Key.F,
        G = EZInput.Key.G,
        H = EZInput.Key.H,
        I = EZInput.Key.I,
        J = EZInput.Key.J,
        K = EZInput.Key.K,
        L = EZInput.Key.L,
        M = EZInput.Key.M,
        N = EZInput.Key.N,
        O = EZInput.Key.O,
        P = EZInput.Key.P,
        Q = EZInput.Key.Q,
        R = EZInput.Key.R,
        S = EZInput.Key.S,
        T = EZInput.Key.T,
        U = EZInput.Key.U,
        V = EZInput.Key.V,
        W = EZInput.Key.W,
        X = EZInput.Key.X,
        Y = EZInput.Key.Y,
        Z = EZInput.Key.Z,
        WinLeft = EZInput.Key.WinLeft,
        WinRight = EZInput.Key.WinRight,
        Num0 = EZInput.Key.Num0,
        Num1 = EZInput.Key.Num1,
        Num2 = EZInput.Key.Num2,
        Num3 = EZInput.Key.Num3,
        Num4 = EZInput.Key.Num4,
        Num5 = EZInput.Key.Num5,
        Num6 = EZInput.Key.Num6,
        Num7 = EZInput.Key.Num7,
        Num8 = EZInput.Key.Num8,
        Num9 = EZInput.Key.Num9,
        F1 = EZInput.Key.F1,
        F2 = EZInput.Key.F2,
        F3 = EZInput.Key.F3,
        F4 = EZInput.Key.F4,
        F5 = EZInput.Key.F5,
        F6 = EZInput.Key.F6,
        F7 = EZInput.Key.F7,
        F8 = EZInput.Key.F8,
        F9 = EZInput.Key.F9,
        F10 = EZInput.Key.F10,
        F11 = EZInput.Key.F11,
        F12 = EZInput.Key.F12,
        NumLock = EZInput.Key.NumLock,
        ScrollLock = EZInput.Key.ScrollLock
    }
}