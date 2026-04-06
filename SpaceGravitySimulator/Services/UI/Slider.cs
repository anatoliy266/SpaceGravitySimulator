using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SpaceGravitySimulator.Services.UI
{
    internal class Slider
    {
        public Rectangle Bounds;
        public float MinValue;
        public float MaxValue;
        public float CurrentValue;
        private bool _isDragging;

        public void Update()
        {
            Vector2 mousePoint = Raylib.GetMousePosition();
            bool isMouseOver = Raylib.CheckCollisionPointRec(mousePoint, Bounds);

            if (isMouseOver && Raylib.IsMouseButtonDown(MouseButton.Left))
                _isDragging = true;

            if (_isDragging && Raylib.IsMouseButtonUp(MouseButton.Left))
                _isDragging = false;

            if (_isDragging)
            {
                float t = (mousePoint.X - Bounds.X) / Bounds.Width;
                t = Math.Clamp(t, 0.0f, 1.0f);
                CurrentValue = MinValue + t * (MaxValue - MinValue);
            }
        }

        public void Draw()
        {
            Raylib.DrawRectangleRec(Bounds, Color.LightGray);

            float t = (CurrentValue - MinValue) / (MaxValue - MinValue);
            float thumbX = Bounds.X + t * Bounds.Width;
            Rectangle thumb = new Rectangle(thumbX - 5, Bounds.Y, 10, Bounds.Height);
            Raylib.DrawRectangleRec(thumb, Color.DarkGray);
        }
    }
}
