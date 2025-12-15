using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame.Models
{
    public class Snake
    {
        private List<Rectangle> snakeBody;
        private int gridSize;
        private Direction currentDirection;
        private Direction nextDirection;
        private Canvas canvas;

        public Direction CurrentDirection => currentDirection;
        public Direction NextDirection
        {
            get => nextDirection;
            set => nextDirection = value;
        }

        public List<Rectangle> Body => snakeBody;

        public Snake(Canvas canvas, int gridSize)
        {
            this.canvas = canvas;
            this.gridSize = gridSize;
            snakeBody = new List<Rectangle>();
        }

        public void Initialize(int startX, int startY)
        {
            snakeBody.Clear();
            currentDirection = Direction.Right;
            nextDirection = Direction.Right;

            for (int i = 0; i < 4; i++)
            {
                Rectangle segment = CreateSegment();
                Canvas.SetLeft(segment, (startX - i) * gridSize);
                Canvas.SetTop(segment, startY * gridSize);
                snakeBody.Add(segment);
                canvas.Children.Add(segment);
            }
        }

        public void Move(out double newX, out double newY)
        {
            currentDirection = nextDirection;
            Rectangle head = snakeBody[0];
            newX = Canvas.GetLeft(head);
            newY = Canvas.GetTop(head);

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
        }

        public void AddHead(double x, double y)
        {
            Rectangle newHead = CreateSegment();
            Canvas.SetLeft(newHead, x);
            Canvas.SetTop(newHead, y);
            snakeBody.Insert(0, newHead);
            canvas.Children.Add(newHead);
        }

        public void RemoveTail()
        {
            Rectangle tail = snakeBody[snakeBody.Count - 1];
            snakeBody.RemoveAt(snakeBody.Count - 1);
            canvas.Children.Remove(tail);
        }

        public bool CheckSelfCollision(double x, double y)
        {
            return snakeBody.Skip(1).Any(segment =>
                Math.Abs(Canvas.GetLeft(segment) - x) < 1 &&
                Math.Abs(Canvas.GetTop(segment) - y) < 1);
        }

        public bool IsPositionOccupied(Point position)
        {
            return snakeBody.Any(segment =>
                Canvas.GetLeft(segment) / gridSize == position.X &&
                Canvas.GetTop(segment) / gridSize == position.Y);
        }

        private Rectangle CreateSegment()
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
    }
}
