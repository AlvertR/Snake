using Raylib_cs;
using System.Numerics;

namespace Snake
{
    public class Game
    {
        public Game() { }

        public Game(string name, int fps, int widthWindow, int heightWindow)
        {
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
        public int Columns { get; set; }
        public int Rows { get; set; }
        public List<Vector2> SnakeBody { get; set; }
        public SnakeDirection SnakeDirection { get; set; } = SnakeDirection.Right;
        public GameStatus GameStatus { get; set; } = GameStatus.Start;
        public float MovementTimer { get; set; } = 0f;
        public float MovementInterval { get; set; } = 0.15f;
        public float FoodTimer { get; set; } = 0f;
        public float FoodInterval { get; set; } = 1.25f;
        public int MaxFood { get; set; } = 5;
        public int CurrentFood { get; set; } = 0;
        public int Score { get; set; } = 0;
        public int[,] Grid { get; set; }
        private Random Random = new Random();

        public void LoadGame()
        {
            Raylib.InitWindow(this.WidthWindow, this.HeightWindow, this.Name);
            //Raylib.InitAudioDevice();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string fulPathIcon = Path.Combine(basePath, "Resources", "snake-icon-2.png");
            Image icon = Raylib.LoadImage(fulPathIcon);
            Raylib.ImageFormat(ref icon, PixelFormat.UncompressedR8G8B8A8);
            Raylib.SetWindowIcon(icon);
            Raylib.UnloadImage(icon);

            //string soundPath = Path.Combine(basePath, "Resource", "sound.mp3");
            //Sound hitBallSound = Raylib.LoadSound(soundPath);
            Raylib.SetTargetFPS(this.FPS);

            this.SnakeBody = new List<Vector2> { new Vector2(0, CellSize), new Vector2(CellSize, CellSize), new Vector2(CellSize * 2, CellSize) };
            this.Columns = this.WidthWindow / this.CellSize;
            this.Rows = this.HeightWindow / this.CellSize;
            this.Grid = new int[this.Columns, this.Rows];

            while (!Raylib.WindowShouldClose())
            {
                this.DeltaTime = Raylib.GetFrameTime();
                HandleInput();
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
            switch (this.GameStatus)
            {
                case GameStatus.Start:
                    Raylib.DrawText("Snake",WidthWindow/3,HeightWindow/4, 52, Color.White);
                    Raylib.DrawText("Presione S para comenzar",WidthWindow/4,HeightWindow/3, 42, Color.White);
                    break;
                case GameStatus.Playing:
                    int i = 0;
                    foreach (var item in this.SnakeBody)
                    {
                        Raylib.DrawRectangleV(item, new Vector2(this.CellSize, this.CellSize), Color.White);
                        if (SnakeBody.Count - 1 == i)
                        {
                            Vector2 eyePosition = new Vector2(item.X + CellSize / 2, item.Y + CellSize / 4);
                            Raylib.DrawRectangleV(eyePosition, new Vector2(this.CellSize / 4, this.CellSize / 4), Color.Black);
                        }
                        i++;
                    }
                    for (int c = 0; c < Grid.GetLength(0); c++)
                    {
                        for (int r = 0; r < Grid.GetLength(1); r++)
                        {
                            if (Grid[c, r] == 1)
                            {
                                Vector2 foodPosition = new Vector2((c * CellSize) + CellSize / 3, (r * CellSize) + CellSize / 3);
                                Raylib.DrawRectangleV(foodPosition, new Vector2(this.CellSize / 2, this.CellSize / 2), Color.Red);
                            }
                        }
                    }
                    break;
                case GameStatus.Paused:
                    Raylib.DrawText("Presione C para continuar", WidthWindow/3, HeightWindow / 4, 42, Color.White);
                    break;
                case GameStatus.GameOver:
                    Raylib.DrawText("Fin del juego", WidthWindow/3, HeightWindow / 4, 42, Color.White);
                    Raylib.DrawText("puntuación: " + Score, WidthWindow/3, HeightWindow / 3, 42, Color.White);
                    Raylib.DrawText("Presione R para juegar de nuevo", 10, HeightWindow / 2, 42, Color.White);
                    break;
                case GameStatus.End:
                    Raylib.DrawText("Fin del juego", WidthWindow / 3, HeightWindow / 4, 42, Color.White);
                    Raylib.DrawText("felicidades obtubo la puntuacion mas alta", 10, HeightWindow / 3, 42, Color.White);
                    Raylib.DrawText("puntuación: " + Score, WidthWindow / 3, HeightWindow / 2, 42, Color.White);
                    Raylib.DrawText("Presione R para juegar de nuevo", 10, (HeightWindow / 2)+70, 42, Color.White);
                    break;
                default:
                    break;
            }
            Raylib.EndDrawing();
        }

        private void Update()
        {
            switch (this.GameStatus) {
                case GameStatus.Playing:
                    MovementTimer += DeltaTime;
                    int length = this.SnakeBody.Count() - 1;
                    FoodTimer += DeltaTime;

                    if (MovementTimer >= MovementInterval)
                    {
                        switch (this.SnakeDirection)
                        {
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
                        if (!this.IsFoodEated())
                            this.SnakeBody.RemoveAt(0);
                        this.IsEndGrid();
                        this.IsCollision();
                        this.IsEndGame();
                        this.SetSnakeGrid();
                        MovementTimer = 0;
                    }
                    if (FoodTimer >= FoodInterval)
                    {
                        this.SetFood();
                        FoodTimer = 0;
                    }
                    break;
                case GameStatus.Reset:
                    this.ResetGame();
                    break;
                default:
                    break;
            }
        }

        public void ResetSnakeGrid()
        {
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    if(Grid[i, j] != 1)
                        Grid[i, j] = 0;
                }
            }
        }

        public void SetFood()
        {
            if (CurrentFood >= MaxFood)
                return;

            List<(int col, int row)> freeCells = new();

            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    if (Grid[i, j] == 0)
                        freeCells.Add((i,j));
                }
            }

            if(freeCells.Count == 0) 
                return;

            var position = freeCells[Random.Next(freeCells.Count)];
            Grid[position.col, position.row] = 1;
            CurrentFood++;
        }

        public void SetSnakeGrid()
        {
            this.ResetSnakeGrid();
            foreach (var item in SnakeBody)
            {
                int col = (int)item.X / CellSize;
                int row = (int)item.Y / CellSize;
                if (col < Grid.GetLength(0) && row < Grid.GetLength(1) && col >= 0 && row >= 0)
                {
                    Grid[col, row] = 2;
                }
            }
        }

        public void IsCollision()
        {
            var head = this.SnakeBody.Last();
            if (SnakeBody.Take(SnakeBody.Count - 1).Any(x => x == head))
                this.GameStatus = GameStatus.GameOver;
        }

        public void IsEndGame()
        {
            if(SnakeBody.Count == Columns * Rows)
                GameStatus = GameStatus.End;
        }

        public bool IsFoodEated()
        {
            bool isEatead = false;
            var head = SnakeBody.Last();
            int colHead = (int)head.X / CellSize;
            int rowHead = (int)head.Y / CellSize;
            if(colHead < Grid.GetLength(0) &&  rowHead < Grid.GetLength(1) && colHead >= 0 && rowHead >= 0)
                if (Grid[colHead, rowHead] == 1)
                {
                    Grid[colHead, rowHead] = 0;
                    CurrentFood--;
                    isEatead = true;
                    Score++;
                }
            return isEatead;
        }

        public void IsEndGrid()
        {
            var head = SnakeBody.Last();
            int colHead = (int)head.X / CellSize;
            int rowHead = (int)head.Y / CellSize;
            if (colHead >= Grid.GetLength(0) || rowHead >= Grid.GetLength(1) || colHead < 0 || rowHead < 0)
                this.GameStatus = GameStatus.GameOver;
        }

        private void HandleInput()
        {
            switch (this.GameStatus)
            {
                case GameStatus.Start:
                    if (Raylib.IsKeyDown(KeyboardKey.S))
                        GameStatus = GameStatus.Playing;
                    break;
                case GameStatus.Playing:
                    if (Raylib.IsKeyDown(KeyboardKey.Up) && SnakeDirection != SnakeDirection.Down)
                        this.SnakeDirection = SnakeDirection.Up;
                    if (Raylib.IsKeyDown(KeyboardKey.Down) && SnakeDirection != SnakeDirection.Up)
                        this.SnakeDirection = SnakeDirection.Down;
                    if (Raylib.IsKeyDown(KeyboardKey.Left) && SnakeDirection != SnakeDirection.Right)
                        this.SnakeDirection = SnakeDirection.Left;
                    if (Raylib.IsKeyDown(KeyboardKey.Right) && SnakeDirection != SnakeDirection.Left)
                        this.SnakeDirection = SnakeDirection.Right;
                    if (Raylib.IsKeyDown(KeyboardKey.P))
                        GameStatus = GameStatus.Paused;
                    break;
                case GameStatus.Paused:
                    if (Raylib.IsKeyDown(KeyboardKey.C))
                        GameStatus = GameStatus.Playing;
                    break;
                case GameStatus.GameOver:
                case GameStatus.End:
                    if (Raylib.IsKeyDown(KeyboardKey.R))
                        GameStatus = GameStatus.Reset;
                    break;
                default:
                    break;
            }
        }

        private void ResetGame()
        {
            this.SnakeBody = new List<Vector2> { new Vector2(0, CellSize), new Vector2(CellSize, CellSize), new Vector2(CellSize * 2, CellSize) };
            this.CurrentFood = 0;
            this.Score = 0;
            this.SnakeDirection = SnakeDirection.Right;
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    Grid[i, j] = 0;
                }
            }
            this.SetSnakeGrid();
            this.MovementTimer = 0f;
            this.FoodTimer = 0f;
            this.GameStatus = GameStatus.Playing;
        }
    }
}
