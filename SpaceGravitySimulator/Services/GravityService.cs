using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SpaceGravitySimulator.Services
{
    internal class GravityService
    {
        
        public void Update(World world)
        {
            var G = world.GetGravityConstant();
            var ids = world.GetEntitiesWithPositionAndMass();
            
            
            
            
            
            
            
            
            
            
            foreach (var id in ids)
            {
                world.TryGetPosition(id, out var pos1);
                world.TryGetMass(id, out var mass1);
                world.TryGetDirection(id, out var direction);
                var tempDir = Vector2.Zero;
                foreach (var i in ids)
                {
                    if (i == id) continue;
                    world.TryGetPosition(i, out var pos2);
                    world.TryGetMass(i, out var mass2);

                    var dx = pos2.X - pos1.X;
                    var dy = pos2.Y - pos1.Y;

                    var r2 = dx * dx + dy * dy;
                    if (dx * dx + dy * dy < 0.1f) continue;
                    var r = (float)Math.Sqrt(r2);

                    var gravityPower = (G * (mass1.Val * mass2.Val) / r2);

                    var xnorm = dx / r;
                    var ynorm = dy / r;

                    var dirVec = new Vector2 { X = xnorm*gravityPower, Y = ynorm*gravityPower };
                    dirVec /= mass1.Val;
                    tempDir += dirVec;
                }
                direction.Vector += tempDir;
                world.SetDirection(id, direction);
            }
        }
    }
}
