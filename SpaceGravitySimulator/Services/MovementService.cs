using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceGravitySimulator.Services
{
    internal class MovementService
    {
        public void Update(World world)
        {
            foreach (var id in world.GetEntitiesWithPositionAndDirection())
            {
                world.TryGetPosition(id, out var pos);
                world.TryGetDirection(id, out var direction);

                pos.X += direction.Vector.X;
                pos.Y += direction.Vector.Y;

                world.SetPosition(id, pos);
            }
        }
    }
}
