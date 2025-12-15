using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace SnakeGame
{
    public partial class MainWindow : Window
    {
        private List<Rectangle> snakeBody;
        private int gridSize = 20;
        private Direction currentDirection;
        private Random random;

        public MainWindow()
        {
            InitializeComponent();
            random = new Random();
            StartNewGame();
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

        private void StartNewGame()
        {
            GameCanvas.Children.Clear();
            snakeBody = new List<Rectangle>();
            currentDirection = Direction.Right;
            
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
