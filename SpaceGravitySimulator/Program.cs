using Raylib_cs;
using SpaceGravitySimulator;
using SpaceGravitySimulator.Components;
using SpaceGravitySimulator.Entities;
using SpaceGravitySimulator.Services;
using SpaceGravitySimulator.Services.Visualization;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

var cnt = Vector<float>.Count;

var world = new World<double>(10000.0f, 10000.0f, 0.0001f);
var movementSystem = new MovementService<double>(10000);
var gravitySystem = new GravityService<double>();
var visualisationSystem = new VisualizationService<double>(1000, 1000);

//3 ТЕЛА, ДОЛЖЕН ВЫПИСЫВАТЬ ВОСЬМЕРКИ, В ПРИНЦИПЕ ВЫПИСЫВАЕТ, НО НЕДОЛГО
//double[] x = { 0.970, -0.970, 0.0 };
//double[] y = { -0.243, 0.243, 0.0 };
//double[] vx = { 0.466, 0.466, -0.932 };
//double[] vy = { 0.433, 0.433, -0.866 };
//for (int i = 0; i < 3; i++)
//{
//    var e = world.CreateEntity();
//    world.SetPosition(e.Id, x[i] * 100 + 1000, y[i] * 100 + 1000); // масштабируй под экран
//    world.SetMass(e.Id, 60000.0);
//    world.SetVelocity(e.Id, vx[i] * 10, vy[i] * 10); // подбери масштаб скорости
//}

//ТИПА СОЛНЕЧНАЯ СИСТЕМА
//double[] x = { 0.0, 0.387, 0.723, 1.0, 1.524, 5.203, 9.537, 19.191, 30.069 };
//double[] y = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
//double[] vx = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
//double[] vy = { 0.0, 1.607, 1.176, 1.0, 0.810, 0.438, 0.324, 0.228, 0.182 };
//double[] masses = { 100000.0, 0.055, 0.815, 1.0, 0.107, 317.8, 95.2, 14.5, 17.1 };

//for (int i = 0; i < x.Length; i++)
//{
//    var e = world.CreateEntity();
//    world.SetPosition(e.Id, x[i] * 15 + 1000, y[i] * 15 + 1000);
//    world.SetMass(e.Id, masses[i]);
//    world.SetVelocity(e.Id, vx[i] / 300, vy[i] / 30); // множитель скорости подобран для красивой анимации
//}

// СОЛНЕЧНАЯ СИСТЕМА — ПРАВИЛЬНЫЕ СКОРОСТИ

double G = world.GetGravityConstant();        // =1
double M_sun = 100000.0;                     // масса Солнца
double scale = 650.0;                         // 1 а.е. в пикселях
double centerX = 1000.0, centerY = 1000.0;

double[] distAU = { 0.0, 0.387, 0.723, 1.0, 1.524, 5.203, 9.537, 19.191, 30.069 };
double[] masses = { M_sun, 0.055, 0.815, 1.0, 0.107, 317.8, 95.2, 14.5, 17.1 };

// Множитель скорости (подбери вручную, если нужно)
double speedFactor = 0.7;   // <--- МЕНЯЙ ЭТО ЧИСЛО (0.5, 0.8, 1.2...)

for (int i = 0; i < distAU.Length; i++)
{
    var e = world.CreateEntity();
    double r_px = distAU[i] * scale;
    double x = centerX + r_px;
    double y = centerY;
    world.Components.SetPosition(e.Id, x, y);
    world.Components.SetMass(e.Id, masses[i]);

    if (i == 0)  // Солнце
    {
        world.Components.SetVelocity(e.Id, 0.0, 0.0);
        continue;
    }

    // Круговая скорость в твоих единицах
    double v_circ = Math.Sqrt(G * M_sun / r_px) * speedFactor;
    world.Components.SetVelocity(e.Id, 0.0, v_circ);
}

while (!visualisationSystem.ShouldClose())
{
    var frametime = Raylib.GetFrameTime() * world.GetTimeScale();
    var safeStep = 0.01f;

    int subSteps = (int)Math.Ceiling(frametime / safeStep);
    if (subSteps <= 0) subSteps = 1;

    var stepDt = frametime / subSteps;

    
    
    for (int i = 0; i < subSteps; i++)
    {
        movementSystem.HalfStep(world, stepDt);
        gravitySystem.Update(world);
        movementSystem.SecondHalf(world, stepDt);
    }
    visualisationSystem.Update(world);
    //uiSystem.Update(world);
    //uiSystem.Draw();
}

