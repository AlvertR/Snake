using Raylib_cs;
using System.Numerics;

namespace Snake
{
    public class Game
    {
        public Game() { }

        public Game(string name, int fps, int widthWindow, int heightWindow) { 
            HeightWindow = heightWindow;
            WidthWindow = widthWindow;
            Name = name;
            FPS = fps;
        }
        public int HeightWindow { get; set; }
        public int WidthWindow { get; set; }
        public string Name { get; set; }
        public int FPS { get; set; }
        public float DeltaTime { get; set; } = 0;
        public int CellSize { get; set; } = 40;
        public int Collums { get; set; }
        public int Rows { get; set; }
        public List<Vector2> SnakeBody { get; set; }
        public SnakeDirection snakeDirection { get; set; } = SnakeDirection.Right;

        public void LoadGame()
        {
            Raylib.InitWindow(this.WidthWindow, this.HeightWindow, this.Name);
            //Raylib.InitAudioDevice();
            //string basePath = AppDomain.CurrentDomain.BaseDirectory;

            //string fulPath = Path.Combine(basePath, "Resource", "logo.png");
            //Image icon = Raylib.LoadImage(fulPath);
            //Raylib.ImageFormat(ref icon, PixelFormat.UncompressedR8G8B8A8);
            //Raylib.SetWindowIcon(icon);
            //Raylib.UnloadImage(icon);

            //string soundPath = Path.Combine(basePath, "Resource", "sound.mp3");
            //Sound hitBallSound = Raylib.LoadSound(soundPath);
            Raylib.SetTargetFPS(this.FPS);

            this.SnakeBody = new List<Vector2> { new Vector2(0, CellSize), new Vector2(CellSize, CellSize) , new Vector2(CellSize*2, CellSize) };
            this.Collums = this.WidthWindow / this.CellSize;
            this.Rows = this.HeightWindow / this.CellSize;
            while (!Raylib.WindowShouldClose())
            {
                this.DeltaTime = Raylib.GetFrameTime();
                InputCheck();
                Update();
                Draw();
            }

            //Raylib.UnloadSound(hitBallSound);
            //Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }

        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            int i = 0;
            foreach (var item in this.SnakeBody)
            {
                Raylib.DrawRectangleV(item, new Vector2(this.CellSize, this.CellSize), Color.White);
                if (SnakeBody.Count -1 == i)
                {
                    Vector2 eyePosition = new Vector2(item.X + CellSize/2, item.Y+CellSize/4);
                    Raylib.DrawRectangleV(eyePosition, new Vector2(this.CellSize / 4, this.CellSize / 4), Color.Black);
                }
                i++;
            }
            Raylib.EndDrawing();
        }
        int seconds = 0;
        float running = 0;
        int newSeconds = 0;
        private void Update()
        {
            
            
            running += this.DeltaTime;
            newSeconds = (int)running;

                int length = this.SnakeBody.Count()-1;
            if(newSeconds > seconds && SnakeBody.Count > 0)
            {
                switch (this.snakeDirection) { 
                    case SnakeDirection.Right:
                        this.SnakeBody.Add(new Vector2(SnakeBody[length].X + CellSize, SnakeBody[length].Y));
                        break;
                    case SnakeDirection.Left:
                        this.SnakeBody.Add(new Vector2(SnakeBody[length].X - CellSize, SnakeBody[length].Y));
                        break;
                    case SnakeDirection.Up:
                        this.SnakeBody.Add(new Vector2(SnakeBody[length].X, SnakeBody[length].Y - CellSize));
                        break;
                    case SnakeDirection.Down:
                        this.SnakeBody.Add(new Vector2(SnakeBody[length].X, SnakeBody[length].Y + CellSize));
                        break;
                    default:
                        break;
                }   
                this.SnakeBody.RemoveAt(0);
                seconds = newSeconds;
            }
        }

        private void InputCheck()
        {
            if (Raylib.IsKeyDown(KeyboardKey.Up) && snakeDirection != SnakeDirection.Down)
                this.snakeDirection = SnakeDirection.Up;
            if (Raylib.IsKeyDown(KeyboardKey.Down) && snakeDirection != SnakeDirection.Up)
                this.snakeDirection = SnakeDirection.Down;
            if (Raylib.IsKeyDown(KeyboardKey.Left) && snakeDirection != SnakeDirection.Right)
                this.snakeDirection = SnakeDirection.Left;
            if (Raylib.IsKeyDown(KeyboardKey.Right) && snakeDirection != SnakeDirection.Left)
                this.snakeDirection = SnakeDirection.Right;
        }
    }
}
