using SpaceGravitySimulator.Components;
using SpaceGravitySimulator.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;

namespace SpaceGravitySimulator
{
    public class World<T> where T : notnull, INumber<T>, IConvertible
    {
        private Queue<int> _ids = new Queue<int>();
        private int _activeEntities = 0;

        public ComponentStorage<T> Components = new ComponentStorage<T>();
        
        public T WorldWidth { get; set; }
        public T WorldHeight { get; set; }
        public T G { get; set; } = default(T);
        public T TimeScale { get; set; } = default(T);

        public T GetTimeScale() => TimeScale;
        public void SetTimeScale(T value) => TimeScale = value;
        public T GetGravityConstant() => G;




        public World(T worldWidth, T worldHeight, T g)
        {
            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
            G = g;
            for (var i = 0; i < 10000; i++) _ids.Enqueue(i);
        }
        public Entity CreateEntity()
        {
            _activeEntities++;
            return new Entity { Id = _ids.Dequeue() };
        }
        public int GetActiveEntities() => _activeEntities;

    }
}
