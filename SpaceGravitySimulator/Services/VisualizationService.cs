using Raylib_cs;
using SpaceGravitySimulator.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceGravitySimulator.Services
{
    internal class VisualizationService
    {
        private int _width, _height;
        private float scaleX, scaleY;
        public VisualizationService(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Update(World world)
        {
            float scaleX = (float)_width / world.WorldWidth;
            float scaleY = (float)_height / world.WorldHeight;
            Raylib.ClearBackground(Color.White);
            Raylib.BeginDrawing();
            foreach (var id in world.GetEntitiesWithPositionAndMass())
            {
                world.TryGetPosition(id, out var position);
                world.TryGetMass(id, out var mass);

                float screenX = position.X * scaleX;
                float screenY = position.Y * scaleY;

                float radius = 2f + (float)Math.Log10(mass.Val + 1) * 3f;

                float hue = (id * 137.5f) % 360.0f;
                Color planetColor = Raylib.ColorFromHSV(hue, 0.8f, 0.9f);

                Raylib.DrawCircle((int)screenX, (int)screenY, radius, planetColor);
            }
            Raylib.EndDrawing();
        }

        public void CreateWorld()
        {
            Raylib.InitWindow(_width, _height, "Gravity simulation");
        }
        public bool ShouldClose() => Raylib.WindowShouldClose();
        public void Close() => Raylib.CloseWindow();

    }
}
