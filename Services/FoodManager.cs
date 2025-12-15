using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SnakeGame.Models;

namespace SnakeGame.Services
{
    public class FoodManager
    {
        private Canvas canvas;
        private int gridSize;
        private Random random;
        private Rectangle food;

        public Rectangle Food => food;

        public FoodManager(Canvas canvas, int gridSize)
        {
            this.canvas = canvas;
            this.gridSize = gridSize;
            this.random = new Random();
        }

        public void SpawnFood(Snake snake)
        {
            if (food != null)
                canvas.Children.Remove(food);

            int maxX = (int)(canvas.ActualWidth / gridSize);
            int maxY = (int)(canvas.ActualHeight / gridSize);

            Point foodPosition;
            do
            {
                foodPosition = new Point(random.Next(0, maxX), random.Next(0, maxY));
            } while (snake.IsPositionOccupied(foodPosition));

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
            canvas.Children.Add(food);
        }

        public bool CheckFoodCollision(double snakeX, double snakeY)
        {
            if (food == null) return false;

            double foodX = Canvas.GetLeft(food) - 2;
            double foodY = Canvas.GetTop(food) - 2;

            return Math.Abs(snakeX - foodX) < 1 && Math.Abs(snakeY - foodY) < 1;
        }
    }
}
