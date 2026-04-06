using Raylib_cs;
using SpaceGravitySimulator.Components;
using SpaceGravitySimulator.Services.Visualization;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SpaceGravitySimulator.Services
{
    internal class VisualizationService<T> where T : notnull, INumber<T>, IConvertible
    {
        private readonly WindowManager _window;
        private readonly CameraController<T> _cameraController;
        private readonly TrailRenderer _trailManager;
        private readonly EntityRenderer _entityRenderer;
        private readonly UIService<T> _uiService;

        public VisualizationService(int width, int height)
        {
            _window = new WindowManager(width, height, "Gravity Simulation");
            _cameraController = new CameraController<T>(T.CreateChecked(width), T.CreateChecked(height));
            _trailManager = new TrailRenderer(200, 500, 3);
            _entityRenderer = new EntityRenderer();
            _uiService = new UIService<T>(width, height);
        }

        public void Update(World<T> world)
        {
            var n = world.GetActiveEntities();
            var coords = world.Components.GetRawCoordinates(n);
            var coordX = coords.X;
            var coordY = coords.Y;
            var masses = world.Components.GetMasses(n);

            _cameraController.Update();
            _trailManager.Update(coordX, coordY, masses, n);
            _uiService.Update(world);

            _window.BeginDrawing();
            _window.ClearBackground(Color.White);
            _window.BeginMode2D(_cameraController.Camera);

            _trailManager.Render();
            _entityRenderer.Render(coordX, coordY, masses, n);

            _window.EndMode2D();
            _uiService.Draw();
            _window.EndDrawing();
        }

        public bool ShouldClose() => _window.ShouldClose();
    }
}
