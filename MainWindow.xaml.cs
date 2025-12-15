using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer gameTimer;
        private List<Rectangle> snakeBody;
        private Rectangle food;
        private int score;
        private int highScore;
        private int gridSize = 20;
        private Direction currentDirection;
        private Direction nextDirection;
        private Random random;
        private bool isGameRunning;

        public MainWindow()
        {
            InitializeComponent();
            random = new Random();
            gameTimer = new DispatcherTimer();
            gameTimer.Tick += GameTick;
            gameTimer.Interval = TimeSpan.FromMilliseconds(150);
            StartNewGame();
        }

        private void StartNewGame()
        {
            GameCanvas.Children.Clear();
            snakeBody = new List<Rectangle>();
            score = 0;
            ScoreText.Text = score.ToString();
            currentDirection = Direction.Right;
            nextDirection = Direction.Right;
            isGameRunning = true;
            GameOverPanel.Visibility = Visibility.Collapsed;

            int startX = 10;
            int startY = 10;
            for (int i = 0; i < 4; i++)
            {
                Rectangle segment = CreateSnakeSegment();
                Canvas.SetLeft(segment, (startX - i) * gridSize);
                Canvas.SetTop(segment, startY * gridSize);
                snakeBody.Add(segment);
                GameCanvas.Children.Add(segment);
            }

            SpawnFood();
            gameTimer.Start();
        }

        private Rectangle CreateSnakeSegment()
        {
            return new Rectangle
            {
                Width = gridSize - 2,
                Height = gridSize - 2,
                Fill = new SolidColorBrush(Color.FromRgb(78, 201, 176)),
                Stroke = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                StrokeThickness = 2
            };
        }

        private void SpawnFood()
        {
            if (food != null)
                GameCanvas.Children.Remove(food);

            int maxX = (int)(GameCanvas.ActualWidth / gridSize);
            int maxY = (int)(GameCanvas.ActualHeight / gridSize);

            Point foodPosition;
            do
            {
                foodPosition = new Point(random.Next(0, maxX), random.Next(0, maxY));
            } while (IsPositionOccupied(foodPosition));

            food = new Rectangle
            {
                Width = gridSize - 4,
                Height = gridSize - 4,
                Fill = new SolidColorBrush(Color.FromRgb(206, 145, 120)),
                RadiusX = 5,
                RadiusY = 5
            };

            Canvas.SetLeft(food, foodPosition.X * gridSize + 2);
            Canvas.SetTop(food, foodPosition.Y * gridSize + 2);
            GameCanvas.Children.Add(food);
        }

        private bool IsPositionOccupied(Point position)
        {
            return snakeBody.Any(segment =>
                Canvas.GetLeft(segment) / gridSize == position.X &&
                Canvas.GetTop(segment) / gridSize == position.Y);
        }

        private void GameTick(object sender, EventArgs e)
        {
            if (!isGameRunning)
                return;

            currentDirection = nextDirection;
            Rectangle head = snakeBody[0];
            double newX = Canvas.GetLeft(head);
            double newY = Canvas.GetTop(head);

            switch (currentDirection)
            {
                case Direction.Up:
                    newY -= gridSize;
                    break;
                case Direction.Down:
                    newY += gridSize;
                    break;
                case Direction.Left:
                    newX -= gridSize;
                    break;
                case Direction.Right:
                    newX += gridSize;
                    break;
            }

            if (CheckCollision(newX, newY))
            {
                GameOver();
                return;
            }

            Rectangle newHead = CreateSnakeSegment();
            Canvas.SetLeft(newHead, newX);
            Canvas.SetTop(newHead, newY);
            snakeBody.Insert(0, newHead);
            GameCanvas.Children.Add(newHead);

            double foodX = Canvas.GetLeft(food) - 2;
            double foodY = Canvas.GetTop(food) - 2;
            
            if (Math.Abs(newX - foodX) < 1 && Math.Abs(newY - foodY) < 1)
            {
                score += 10;
                ScoreText.Text = score.ToString();
                SpawnFood();
                
                if (gameTimer.Interval.TotalMilliseconds > 50)
                    gameTimer.Interval = TimeSpan.FromMilliseconds(gameTimer.Interval.TotalMilliseconds - 2);
            }
            else
            {
                Rectangle tail = snakeBody[snakeBody.Count - 1];
                snakeBody.RemoveAt(snakeBody.Count - 1);
                GameCanvas.Children.Remove(tail);
            }
        }

        private bool CheckCollision(double x, double y)
        {
            if (x < 0 || x >= GameCanvas.ActualWidth || y < 0 || y >= GameCanvas.ActualHeight)
                return true;

            return snakeBody.Skip(1).Any(segment =>
                Math.Abs(Canvas.GetLeft(segment) - x) < 1 &&
                Math.Abs(Canvas.GetTop(segment) - y) < 1);
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
                    if (currentDirection != Direction.Down)
                        nextDirection = Direction.Up;
                    break;
                case Key.Down:
                case Key.S:
                    if (currentDirection != Direction.Up)
                        nextDirection = Direction.Down;
                    break;
                case Key.Left:
                case Key.A:
                    if (currentDirection != Direction.Right)
                        nextDirection = Direction.Left;
                    break;
                case Key.Right:
                case Key.D:
                    if (currentDirection != Direction.Left)
                        nextDirection = Direction.Right;
                    break;
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
