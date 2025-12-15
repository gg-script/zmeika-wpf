using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

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
