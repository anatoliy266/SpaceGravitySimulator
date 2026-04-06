using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SpaceGravitySimulator.Components
{
    
    public class ComponentStorage<T> where T: notnull, INumber<T>, IConvertible
    {
        private T[] _positionsX;
        private T[] _positionsY;
        private T[] _masses;
        private T[] _velocitiesX;
        private T[] _velocitiesY;
        private T[] _accelerationX;
        private T[] _accelerationY;

        private int _capacity;

        public ComponentStorage(int initialCapacity = 1024)
        {
            _capacity = initialCapacity;
            _positionsX = new T[_capacity];
            _positionsY = new T[_capacity];
            _masses = new T[_capacity];
            _velocitiesX = new T[_capacity];
            _velocitiesY = new T[_capacity];
            _accelerationX = new T[_capacity];
            _accelerationY = new T[_capacity];
        }

        // Метод для автоматического расширения, если ID превышает текущую емкость
        public void EnsureCapacity(int id)
        {
            if (id < _capacity) return;

            int newCapacity = _capacity * 2;
            while (id >= newCapacity) newCapacity *= 2;

            Array.Resize(ref _positionsX, newCapacity);
            Array.Resize(ref _positionsY, newCapacity);
            Array.Resize(ref _masses, newCapacity);
            // Resize остальных...

            _capacity = newCapacity;
        }



        
        
        public CoordinatesView<T> GetRawCoordinates(int count) => new(_positionsX.AsSpan(0, count), _positionsY.AsSpan(0, count));
        public Span<T> GetMasses(int count) => _masses.AsSpan<T>(0, count);
        public VelocitiesView<T> GetRawVelocities(int count) => new(_velocitiesX.AsSpan<T>(0, count), _velocitiesY.AsSpan<T>(0, count));
        public AccelerationsView<T> GetRawAccelerations(int count) => new (_accelerationX.AsSpan<T>(0, count), _accelerationY.AsSpan<T>(0, count));

        public void SetPosition(int id, T posX, T posY)
        {
            EnsureCapacity(id);
            _positionsX[id] = posX;
            _positionsY[id] = posY;
        }


        public void SetMass(int id, T mass)
        {
            EnsureCapacity(id);
            _masses[id] = mass;
        }

        public void SetVelocity(int id, T velocityX, T velocityY)
        {
            EnsureCapacity(id);
            _velocitiesX[id] = velocityX;
            _velocitiesY[id] = velocityY;
        }

        public void SetAcceleration(int id, T accelerationX, T accelerationY)
        {
            EnsureCapacity(id);
            _accelerationX[id] = accelerationX;
            _accelerationY[id] = accelerationY;
        }
    }
}
