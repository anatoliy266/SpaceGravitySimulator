using SpaceGravitySimulator.Components;
using SpaceGravitySimulator.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceGravitySimulator
{
    internal class World
    {
        public float WorldWidth { get; set; } = 1000f;
        public float WorldHeight { get; set; } = 1000f;
        public float G { get; set; } = 0.0000001f;

        
        private Queue<int> _ids = new Queue<int>();

        private float[] _positionsX = new float[128];
        private float[] _positionsY = new float[128];
        private float[] _masses = new float[128];
        private float[] _sizes = new float[128];
        private float[] _velocities = new float[128];

        public World(float worldWidth, float worldHeight, float g)
        {
            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
            G = g;
            for (var i = 0; i < 128; i++) _ids.Enqueue(i);
        }

        public Entity CreateEntity()
        {
            return new Entity { Id = _ids.Dequeue() };
        }

        //public void AddPosition(Entity e, Position p)
        //{
        //    _positions[e.Id] = p;
        //}

        public void AddVelocity(Entity e, Velocity v)
        {
            _velocities[e.Id] = v;
        }

        public void AddSize(Entity e, Size v)
        {
            _sizes[e.Id] = v;
        }

        public void AddMass(Entity e, Mass v)
        {
            _masses[e.Id] = v;
        }

        //public bool TryGetPosition(int id, out Position p)
        //=> _positions.TryGetValue(id, out p);

        ////public bool TryGetVelocity(int id, out Velocity v)
        ////    => _velocities.TryGetValue(id, out v);

        //public bool TryGetMass(int id, out Mass m)
        //    => _masses.TryGetValue(id, out m);

        //public bool TryGetSize(int id, out Size s)
        //    => _sizes.TryGetValue(id, out s);
        //public bool TryGetDirection(int id, out Direction d) 
        //    => _directions.TryGetValue(id, out d);

        //public void SetPosition(int id, Position p)
        //    => _positions[id] = p;
        ////public void SetVelocity(int id, Velocity p)
        ////    => _velocities[id] = p;
        //public void SetMass(int id, Mass p)
        //    => _masses[id] = p;
        //public void SetSize(int id, Size p)
        //    => _sizes[id] = p;
        //public void SetDirection(int id, Direction d)
        //    => _directions[id] = d;


        public IEnumerable<int> GetEntitiesWithPosition()
        {
            foreach (var id in _positions.Keys)
            {
                yield return id;

            }
        }

        internal IEnumerable<int> GetEntitiesWithPositionAndDirection()
        {
            foreach (var id in _positions.Keys)
            {
                if (_directions.ContainsKey(id))
                    yield return id;
            }
        }

        internal IEnumerable<int> GetEntitiesWithPositionAndMass()
        {
            foreach (var id in _positions.Keys)
            {
                if (_masses.ContainsKey(id))
                    yield return id;
            }
        }

        public float GetGravityConstant() => G;
        
    }
}
