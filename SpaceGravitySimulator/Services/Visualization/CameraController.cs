using Raylib_cs;
using SpaceGravitySimulator.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;

namespace SpaceGravitySimulator.Services.Visualization
{
    internal class CameraController<T> where T : notnull, INumber<T>, IConvertible
    {
        private Camera2D _camera;
        public Camera2D Camera => _camera;
        //public ref Camera2D Camera => ref field;
        private bool _isPanning;
        private Vector2 _lastMousePos;

        public CameraController(T width, T height)
        {
            _camera = new Camera2D()
            {
                Zoom = 1.0f,
                Offset = new Vector2(float.CreateChecked<T>(width) / 2f, float.CreateChecked<T>(height) / 2f),
                Target = new Vector2(float.CreateChecked<T>(width) / 2f, float.CreateChecked<T>(height) / 2f),
            };
        }

        public void Update()
        {
            HandleZoom();
            HandlePanning();
        }
        
        public void HandleZoom()
        {
            float wheel = Raylib.GetMouseWheelMove();
            if (wheel == 0) return;
            Vector2 mouseWorldBefore = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _camera);
            _camera.Zoom += wheel * 0.1f;
            _camera.Zoom = Math.Clamp(_camera.Zoom, 0.1f, 10f);
            Vector2 mouseWorldAfter = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _camera);
            _camera.Target += mouseWorldBefore - mouseWorldAfter;
        }

        public void HandlePanning()
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Right))
            {
                if (!_isPanning)
                {
                    _isPanning = true;
                    _lastMousePos = Raylib.GetMousePosition();
                }
                else
                {
                    Vector2 delta = Raylib.GetMousePosition() - _lastMousePos;
                    _camera.Target -= delta / _camera.Zoom;
                    _lastMousePos = Raylib.GetMousePosition();
                }
            }
            else
            {
                _isPanning = false;
            }
        }
    }
}
