using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceGravitySimulator.Services.Visualization
{
    internal class WindowManager
    {
        public WindowManager(int width, int height, string title)
        {
            Raylib.InitWindow(width, height, title);
            Raylib.SetTargetFPS(60);
        }
        public bool ShouldClose() => Raylib.WindowShouldClose();
        public void Close() => Raylib.CloseWindow();
        public void BeginDrawing() => Raylib.BeginDrawing();
        public void EndDrawing() => Raylib.EndDrawing();
        public void Dispose() => Close();
        internal void ClearBackground(Color white) => Raylib.ClearBackground(white);
        internal void BeginMode2D(Camera2D camera) => Raylib.BeginMode2D(camera);
        internal void EndMode2D() => Raylib.EndMode2D();
    }
}
