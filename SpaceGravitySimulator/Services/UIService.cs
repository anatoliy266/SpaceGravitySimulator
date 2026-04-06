using Raylib_cs;
using SpaceGravitySimulator.Services.UI;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Numerics;
using System.Text;

namespace SpaceGravitySimulator.Services
{
    internal class UIService<T> where T: notnull, INumber<T>, IConvertible
    {
        private Slider _timeScaleSlider;
        public UIService(int screenWidth, int screenHeight)
        {
            _timeScaleSlider = new Slider()
            {
                Bounds = new Rectangle(screenWidth - 250, 20, 200, 20),
                CurrentValue = 1.0f,
                MinValue = 0.0f,
                MaxValue = 1000.0f,
            };

        }
        public void Update(World<T> world)
        {
            _timeScaleSlider.Update();
            world.SetTimeScale(T.CreateChecked(_timeScaleSlider.CurrentValue));
        }

        public void Draw()
        {
            _timeScaleSlider.Draw();
        }
    }
}
