using SpaceGravitySimulator.Components;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SpaceGravitySimulator.Services
{
    public class MovementService<T> where T: notnull, INumber<T>, IConvertible
    {
        private T[] _prevAccX;
        private T[] _prevAccY;

        public MovementService(int maxEntities)
        {
            _prevAccX = new T[maxEntities];
            _prevAccY = new T[maxEntities];
        }

        public void HalfStep(World<T> world, T dt)
        {
            int n = world.GetActiveEntities();
            var coords = world.Components.GetRawCoordinates(n);
            var coordX = coords.X;
            var coordY = coords.Y;
            var accs = world.Components.GetRawAccelerations(n);
            var accX = accs.X;
            var accY = accs.Y;
            var vels = world.Components.GetRawVelocities(n);
            var velX = vels.X;
            var velY = vels.Y;

            for (int i = 0; i < n; i++)
            {
                _prevAccX[i] = accX[i];
                _prevAccY[i] = accY[i];

                velX[i] += accX[i] * (dt * T.CreateChecked(0.5));
                velY[i] += accY[i] * (dt * T.CreateChecked(0.5));
                coordX[i] += velX[i] * dt;
                coordY[i] += velY[i] * dt;
            }
        }

        public void SecondHalf(World<T> world, T dt)
        {
            int n = world.GetActiveEntities();
            var accs = world.Components.GetRawAccelerations(n);
            var accX = accs.X;
            var accY = accs.Y;
            var vels = world.Components.GetRawVelocities(n);
            var velX = vels.X;
            var velY = vels.Y;

            for (int i = 0; i < n; i++)
            {
                velX[i] += (accX[i] - _prevAccX[i]) * (dt * T.CreateChecked(0.5));
                velY[i] += (accY[i] - _prevAccY[i]) * (dt * T.CreateChecked(0.5));
            }
        }
    }
}
