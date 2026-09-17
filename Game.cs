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

            this.SnakeBody = new List<Vector2> { new Vector2(80,80), new Vector2(80+CellSize, 80) , new Vector2(80+(CellSize*2), 80) };
            while (!Raylib.WindowShouldClose())
            {
                this.DeltaTime = Raylib.GetFrameTime();
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
            foreach (var item in this.SnakeBody)
            {
                Raylib.DrawRectangleV(item, new Vector2(this.CellSize, this.CellSize), Color.White);
            }
            Raylib.EndDrawing();
        }
        int seconds = 0;
        float running = 0;
        int newSeconds = 0;
        private void Update()
        {
            this.Collums = this.WidthWindow / this.CellSize;
            this.Rows = this.HeightWindow / this.CellSize;
            
            running += this.DeltaTime;
            newSeconds = (int)running;

                int length = this.SnakeBody.Count()-1;
                Console.WriteLine("tamaño "+length);
            if(newSeconds > seconds && SnakeBody.Count > 0)
            {
                
                this.SnakeBody.Add(new Vector2(SnakeBody[length].X+CellSize, SnakeBody[0].Y));
                this.SnakeBody.RemoveAt(0);
                seconds = newSeconds;
            }


        }
    }
}
