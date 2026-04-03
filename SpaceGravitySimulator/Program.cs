using SpaceGravitySimulator;
using SpaceGravitySimulator.Services;
using SpaceGravitySimulator.Entities;
using SpaceGravitySimulator.Components;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

var cnt = Vector<float>.Count;


var world = new World();
var movementSystem = new MovementService();
var gravitySystem = new GravityService();
var visualisationSystem = new VisualizationService(800, 600);
visualisationSystem.CreateWorld();

// ========== НАСТРОЙКИ (работают без timeStep) ==========
float G = world.GetGravityConstant();     // гравитационная постоянная (подобрана)
float sunMass = 1000000f;      // масса Солнца
Vector2 sunPos = new Vector2(500, 500);

// Солнце
var sun = world.CreateEntity();
world.SetPosition(sun.Id, new Position() { X = 500, Y = 500 });
world.SetDirection(sun.Id, new Direction() { Vector = Vector2.Zero });
world.SetMass(sun.Id, new Mass() { Val = (int)sunMass });

// Функция создания планеты
void CreatePlanet(float distance, int mass, float angleDeg)
{
    float angleRad = angleDeg * MathF.PI / 180f;
    Vector2 offset = new Vector2(MathF.Cos(angleRad), MathF.Sin(angleRad)) * distance;
    Vector2 position = sunPos + offset;

    // Касательная скорость (перпендикуляр к радиусу)
    Vector2 tangent = new Vector2(-offset.Y, offset.X);
    float orbitalSpeed = MathF.Sqrt(G * sunMass / distance);
    Vector2 velocity = Vector2.Normalize(tangent) * orbitalSpeed;

    var planet = world.CreateEntity();
    world.SetPosition(planet.Id, new Position() { X = position.X, Y = position.Y });
    world.SetDirection(planet.Id, new Direction() { Vector = velocity });
    world.SetMass(planet.Id, new Mass() { Val = mass });
}

// ========== ПЛАНЕТЫ (реальные пропорции, но влезают в экран) ==========
CreatePlanet(65f, 10, 0);   // Меркурий
CreatePlanet(90f, 20, 30);   // Венера
CreatePlanet(120f, 30, 60);   // Земля
CreatePlanet(150f, 15, 90);   // Марс
CreatePlanet(220f, 200, 120);  // Юпитер
CreatePlanet(280f, 180, 150);  // Сатурн
CreatePlanet(340f, 80, 180);  // Уран
CreatePlanet(400f, 75, 210);  // Нептун
CreatePlanet(450f, 5, 240);  // Плутон

// ========== ЗАПУСК ==========
while (!visualisationSystem.ShouldClose())
{
    gravitySystem.Update(world);
    movementSystem.Update(world);
    visualisationSystem.Update(world);
}



//using SpaceGravitySimulator;
//using SpaceGravitySimulator.Services;
//using SpaceGravitySimulator.Entities;
//using SpaceGravitySimulator.Components;
//using System.Numerics;

//var world = new World();
//world.WorldWidth = 6000.0f;
//world.WorldHeight = 6000.0f;
//var movementSystem = new MovementService();
//var gravitySystem = new GravityService();
//var visualisationSystem = new VisualizationService(1000, 1000);
//visualisationSystem.CreateWorld();
