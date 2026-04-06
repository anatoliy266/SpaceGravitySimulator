using SpaceGravitySimulator.Components;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SpaceGravitySimulator.Services
{
    public class GravityService<T> where T : notnull, INumber<T>, IConvertible
    {

        public void Update(World<T> world)
        {
            var step = Vector<T>.Count;
            var G = world.GetGravityConstant();
            var epsilon = T.CreateChecked(0.0001f);
            var n = world.GetActiveEntities();

            var vG = new Vector<T>(G);
            var vEpsilon = new Vector<T>(T.CreateChecked(0.01f));
            var vOne = new Vector<T>(T.CreateChecked(1.0f));

            
            var coords = world.Components.GetRawCoordinates(world.GetActiveEntities());
            var coordX = coords.X;
            var coordY = coords.Y;
            var masses = world.Components.GetMasses(world.GetActiveEntities());
            var accs = world.Components.GetRawAccelerations(world.GetActiveEntities());
            var accX = accs.X;
            var accY = accs.Y;

            accX.Clear();
            accY.Clear();


            var vLimit = new Vector<float>(0.01f);
            var masks = new Vector<T>[step];
            for (int offset = 0; offset < step; offset++)
            {
                var values = new T[step];
                for (int k = 0; k < step; k++)
                    values[k] = (k == offset) ? T.Zero : T.One;
                masks[offset] = new Vector<T>(values);
            }

            for (var i = 0; i < n; i++)
            {
                var vMassA = new Vector<T>(masses[i]);
                var vXA = new Vector<T>(coordX[i]);
                var vYA = new Vector<T>(coordY[i]);

                var vTotalAccX = Vector<T>.Zero;
                var vTotalAccY = Vector<T>.Zero;

                var j = 0;
                for (; j < n - step; j += step)
                {
                    var vMassB = new Vector<T>(masses.Slice(j));
                    var vXB = new Vector<T>(coordX.Slice(j));
                    var vYB = new Vector<T>(coordY.Slice(j));

                    var vDX = vXB - vXA;
                    var vDY = vYB - vYA;

                    var vDistSq = (vDX * vDX) + (vDY * vDY) + vEpsilon;

                    var vInvDist = Vector.SquareRoot(vOne / vDistSq);
                    var vMag = (vG * vMassB / vDistSq) * vInvDist;

                    if (i >= j && i < j + step)
                    {
                        int offset = i - j;
                        vMag *= masks[offset];
                    }

                    vTotalAccX += vDX * vMag;
                    vTotalAccY += vDY * vMag;
                }

                var totalAccX = Vector.Sum(vTotalAccX);
                var totalAccY = Vector.Sum(vTotalAccY);

                for (; j < n; j++)
                {
                    if (j == i) continue;
                    var dx = coordX[j] - coordX[i];
                    var dy = coordY[j] - coordY[i];
                    var distSq = (dx * dx) + (dy * dy) + epsilon;
                    var invDist = T.CreateChecked(1.0f) / T.CreateChecked(Math.Sqrt(double.CreateChecked<T>(distSq)));
                    var mag = (G * masses[j] / distSq) * invDist;
                    totalAccX += dx * mag;
                    totalAccY += dy * mag;
                }
                accX[i] = totalAccX;
                accY[i] = totalAccY;
            }

        }
    }
}
