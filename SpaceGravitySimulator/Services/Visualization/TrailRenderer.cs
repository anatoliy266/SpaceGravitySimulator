//using Raylib_cs;
//using System;
//using System.Collections.Generic;
//using System.Numerics;
//using System.Text;

//namespace SpaceGravitySimulator.Services.Visualization
//{
//    public class TrailRenderer
//    {
//        private int MAX_TRAIL_LENGTH;
//        private int MAX_TRAILS;
//        private Vector2[][] _trails;
//        private int _frameCounter;

//        private int TRAIL_STEP;

//        public TrailRenderer(int maxBodies, int maxTrailLength, int updateStep)
//        {
//            MAX_TRAIL_LENGTH = maxTrailLength;
//            TRAIL_STEP = updateStep;
//            MAX_TRAILS = maxBodies;
//            _trails = new Vector2[maxBodies][];
//            for (int i = 0; i < MAX_TRAILS; i++)
//                _trails[i] = new Vector2[MAX_TRAIL_LENGTH];
//        }

//        public void Update<T>(Span<T> posX, Span<T> posY, Span<T> masses, int n) where T : notnull, INumber<T>, IConvertible
//        {
//            _frameCounter++;
//            if (_frameCounter % TRAIL_STEP == 0)
//            {
//                for (int i = 0; i < n; i++)
//                {
//                    if (i >= MAX_TRAILS) break;
//                    if (masses[i] == T.CreateChecked(0)) continue;
//                    // Сдвиг и добавление позиции
//                    for (int j = MAX_TRAIL_LENGTH - 1; j > 0; j--)
//                        _trails[i][j] = _trails[i][j - 1];
//                    _trails[i][0] = new Vector2(float.CreateChecked<T>(posX[i]), float.CreateChecked<T>(posY[i]));
//                }
//                _frameCounter = 0;
//            }
//        }

//        public void Render<T>() where T : INumber<T>, IConvertible
//        {
//            for (var i = 0; i < _trails.Length; i++)
//            {
//                if (_trails[i].Length > 0)
//                {
//                    for (int j = 1; j < MAX_TRAIL_LENGTH; j++)
//                        if (_trails[i][j] != Vector2.Zero)
//                            Raylib.DrawLineV(_trails[i][j - 1], _trails[i][j], Color.Gray);
//                }
//            }
//        }
//    }
//}


using Raylib_cs;
using System.Numerics;

public class TrailRenderer
{
    private readonly int _maxTrailLength;
    private readonly int _maxTrails;
    private readonly Vector2[] _trails;
    private readonly int[] _heads;
    private int _frameCounter;
    private readonly int _updateStep;

    public TrailRenderer(int maxBodies, int maxTrailLength, int updateStep)
    {
        _maxTrailLength = maxTrailLength;
        _maxTrails = maxBodies;
        _updateStep = updateStep;
        _trails = new Vector2[maxBodies * maxTrailLength];
        _heads = new int[maxBodies];
    }

    public void Update<T>(ReadOnlySpan<T> posX, ReadOnlySpan<T> posY, ReadOnlySpan<T> masses, int n)
        where T : notnull, INumber<T>
    {
        if (++_frameCounter % _updateStep != 0) return;
        _frameCounter = 0;

        int bodiesToUpdate = Math.Min(n, _maxTrails);

        for (int i = 0; i < bodiesToUpdate; i++)
        {
            if (T.IsZero(masses[i])) continue;

            _heads[i] = (_heads[i] + 1) % _maxTrailLength;

            int offset = i * _maxTrailLength + _heads[i];
            _trails[offset] = new Vector2(
                float.CreateTruncating(posX[i]),
                float.CreateTruncating(posY[i])
            );
        }
    }

    public void Render()
    {
        for (int i = 0; i < _maxTrails; i++)
        {
            int bodyOffset = i * _maxTrailLength;
            int head = _heads[i];

            for (int j = 0; j < _maxTrailLength - 1; j++)
            {
                if (j == head) continue;

                int idx1 = bodyOffset + j;
                int idx2 = bodyOffset + j + 1;

                Vector2 p1 = _trails[idx1];
                Vector2 p2 = _trails[idx2];

                if (p1 != Vector2.Zero && p2 != Vector2.Zero)
                {
                    Raylib.DrawLineV(p1, p2, Color.Gray);
                }
            }
        }
    }
}