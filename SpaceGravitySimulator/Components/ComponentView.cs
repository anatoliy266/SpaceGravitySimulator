using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceGravitySimulator.Components
{
    public readonly ref struct CoordinatesView<T>
    {
        public readonly Span<T> X;
        public readonly Span<T> Y;

        public CoordinatesView(Span<T> x, Span<T> y)
        {
            X = x;
            Y = y;
        }
    }


    public readonly ref struct VelocitiesView<T>
    {
        public readonly Span<T> X;
        public readonly Span<T> Y;

        public VelocitiesView(Span<T> x, Span<T> y)
        {
            X = x;
            Y = y;
        }
    }

    public readonly ref struct AccelerationsView<T>
    {
        public readonly Span<T> X;
        public readonly Span<T> Y;

        public AccelerationsView(Span<T> x, Span<T> y)
        {
            X = x;
            Y = y;
        }
    }

    public readonly ref struct MassView<T>
    {
        public readonly Span<T> Mass;

        public MassView(Span<T> mass)
        {
            Mass = mass;
        }
    }
}
