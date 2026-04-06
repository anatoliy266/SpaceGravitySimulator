using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SpaceGravitySimulator.Extensions
{
    public static class Camera2DExtension
    {
        extension(Camera2D camera)
        {
            public void SetZoom<T>(T value) where T: INumber<T>, IConvertible => camera.Zoom = float.CreateChecked<T>(value);
            public void SetTarget<T>(Vector2 value) where T : INumber<T>, IConvertible => camera.Target += value;
        }
    }
}
