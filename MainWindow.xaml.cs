using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using SnakeGame.Models;
using SnakeGame.Services;

namespace SnakeGame
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer gameTimer;
        private Snake snake;
        private FoodManager foodManager;
        private int score;
        private int highScore;
        private int gridSize = 20;
        private bool isGameRunning;

        public MainWindow()
        {
            InitializeComponent();
            
            gameTimer = new DispatcherTimer();
            gameTimer.Tick += GameTick;
            gameTimer.Interval = TimeSpan.FromMilliseconds(150);
            
            snake = new Snake(GameCanvas, gridSize);
            foodManager = new FoodManager(GameCanvas, gridSize);
            
            StartNewGame();
        }

        private void StartNewGame()
        {
            GameCanvas.Children.Clear();
            score = 0;
            ScoreText.Text = score.ToString();
            isGameRunning = true;
            GameOverPanel.Visibility = Visibility.Collapsed;
            
            snake.Initialize(10, 10);
            foodManager.SpawnFood(snake);
            
            gameTimer.Interval = TimeSpan.FromMilliseconds(150);
            gameTimer.Start();
        }

        private void GameTick(object sender, EventArgs e)
        {
            if (!isGameRunning)
                return;

            snake.Move(out double newX, out double newY);

            if (CheckCollision(newX, newY))
            {
                GameOver();
                return;
            }

            snake.AddHead(newX, newY);

            if (foodManager.CheckFoodCollision(newX, newY))
            {
                score += 10;
                ScoreText.Text = score.ToString();
                foodManager.SpawnFood(snake);
                
                if (gameTimer.Interval.TotalMilliseconds > 50)
                    gameTimer.Interval = TimeSpan.FromMilliseconds(gameTimer.Interval.TotalMilliseconds - 2);
            }
            else
            {
                snake.RemoveTail();
            }
        }

        private bool CheckCollision(double x, double y)
        {
            if (x < 0 || x >= GameCanvas.ActualWidth || y < 0 || y >= GameCanvas.ActualHeight)
                return true;

            return snake.CheckSelfCollision(x, y);
        }

        private void GameOver()
        {
            gameTimer.Stop();
            isGameRunning = false;

            if (score > highScore)
            {
                highScore = score;
                HighScoreText.Text = highScore.ToString();
            }

            FinalScoreText.Text = $"Ваш счёт: {score}";
            GameOverPanel.Visibility = Visibility.Visible;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isGameRunning)
                return;

            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    if (snake.CurrentDirection != Direction.Down)
                        snake.NextDirection = Direction.Up;
                    break;
                case Key.Down:
                case Key.S:
                    if (snake.CurrentDirection != Direction.Up)
                        snake.NextDirection = Direction.Down;
                    break;
                case Key.Left:
                case Key.A:
                    if (snake.CurrentDirection != Direction.Right)
                        snake.NextDirection = Direction.Left;
                    break;
                case Key.Right:
                case Key.D:
                    if (snake.CurrentDirection != Direction.Left)
                        snake.NextDirection = Direction.Right;
                    break;
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }
    }
}