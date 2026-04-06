using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SpaceGravitySimulator.Services.Visualization
{
    internal class EntityRenderer
    {
        public void Render<T>(Span<T> coordX, Span<T> coordY, Span<T> masses, int n) where T : INumber<T>, IConvertible
        {
            for (var i = 0; i < n; i++)
            {
                if (masses[i] <= T.CreateChecked(0)) continue;
                var radius = 2f + (float)Math.Log10(double.CreateChecked<T>(masses[i]) + 1) * 3f;
                var hue = (i * 137.5f) % 360.0f;
                Color planetColor = Raylib.ColorFromHSV(hue, 0.8f, 0.9f);
                Raylib.DrawCircle(int.CreateChecked<T>(coordX[i]), int.CreateChecked<T>(coordY[i]), radius, planetColor);
            }
            
        }
    }
}
