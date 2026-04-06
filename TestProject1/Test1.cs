using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceGravitySimulator;
using SpaceGravitySimulator.Components;
using SpaceGravitySimulator.Services;
using System.Numerics;

namespace SpaceGravitySimulator.Tests
{
    [TestClass]
    public class ComponentStorageTests
    {
        [TestMethod]
        public void SetAndGetPosition_ValidId_StoresCorrectly()
        {
            // Arrange
            var storage = new ComponentStorage<double>();
            int id = 5;
            double x = 10.5, y = 20.3;

            // Act
            storage.SetPosition(id, x, y);
            var coords = storage.GetRawCoordinates(id + 1); // +1 чтобы включить id

            // Assert
            Assert.AreEqual(x, coords.X[id]);
            Assert.AreEqual(y, coords.Y[id]);
        }

        [TestMethod]
        public void SetMass_ValidId_StoresCorrectly()
        {
            var storage = new ComponentStorage<double>();
            int id = 3;
            double mass = 123.456;

            storage.SetMass(id, mass);
            var masses = storage.GetMasses(id + 1);

            Assert.AreEqual(mass, masses[id]);
        }

        [TestMethod]
        public void EnsureCapacity_AutoExpands_WhenIdOutOfRange()
        {
            var storage = new ComponentStorage<double>(initialCapacity: 2);
            int id = 100; // beyond initial capacity

            storage.SetPosition(id, 1.0, 2.0);
            var coords = storage.GetRawCoordinates(id + 1);

            Assert.AreEqual(1.0, coords.X[id]);
            Assert.AreEqual(2.0, coords.Y[id]);
        }
    }

    [TestClass]
    public class GravityServiceTests
    {
        private World<double> CreateWorldWithTwoBodies()
        {
            var world = new World<double>(1000, 1000, 1.0);
            var e1 = world.CreateEntity();
            var e2 = world.CreateEntity();

            world.Components.SetPosition(e1.Id, 0, 0);
            world.Components.SetMass(e1.Id, 10.0);
            world.Components.SetVelocity(e1.Id, 0, 0);

            world.Components.SetPosition(e2.Id, 10, 0);
            world.Components.SetMass(e2.Id, 10.0);
            world.Components.SetVelocity(e2.Id, 0, 0);

            return world;
        }

        [TestMethod]
        public void Update_TwoBodies_ComputesSymmetricAccelerations()
        {
            // Arrange
            var world = CreateWorldWithTwoBodies();
            var gravity = new GravityService<double>();
            int n = world.GetActiveEntities();

            // Act
            gravity.Update(world);
            var accs = world.Components.GetRawAccelerations(n);

            // Assert: силы равны по модулю и противоположны по направлению
            Assert.AreEqual(-accs.X[0], accs.X[1], 1e-10);
            Assert.AreEqual(accs.Y[0], accs.Y[1]); // y-компоненты нулевые
            Assert.AreNotEqual(0, accs.X[0]);      // ускорение ненулевое
        }

        [TestMethod]
        public void Update_SingleBody_NoAcceleration()
        {
            var world = new World<double>(1000, 1000, 1.0);
            var e = world.CreateEntity();
            world.Components.SetPosition(e.Id, 5, 5);
            world.Components.SetMass(e.Id, 100.0);

            var gravity = new GravityService<double>();
            gravity.Update(world);

            var accs = world.Components.GetRawAccelerations(1);
            Assert.AreEqual(0, accs.X[0]);
            Assert.AreEqual(0, accs.Y[0]);
        }

        [TestMethod]
        public void Update_ThreeBodies_ConservesMomentum()
        {
            // Три тела с нулевым суммарным импульсом (покоящиеся)
            var world = new World<double>(1000, 1000, 1.0);
            var e1 = world.CreateEntity();
            var e2 = world.CreateEntity();
            var e3 = world.CreateEntity();

            world.Components.SetPosition(e1.Id, -1, 0);
            world.Components.SetMass(e1.Id, 1);
            world.Components.SetPosition(e2.Id, 1, 0);
            world.Components.SetMass(e2.Id, 1);
            world.Components.SetPosition(e3.Id, 0, 1);
            world.Components.SetMass(e3.Id, 1);

            var gravity = new GravityService<double>();
            gravity.Update(world);

            var accs = world.Components.GetRawAccelerations(3);
            // Сумма ускорений, взвешенных по массе, должна быть близка к нулю (2-й закон Ньютона)
            double sumMassAccX = accs.X[0] * 1 + accs.X[1] * 1 + accs.X[2] * 1;
            double sumMassAccY = accs.Y[0] * 1 + accs.Y[1] * 1 + accs.Y[2] * 1;

            Assert.IsTrue(Math.Abs(sumMassAccX) < 1e-10);
            Assert.IsTrue(Math.Abs(sumMassAccY) < 1e-10);
        }
    }

    [TestClass]
    public class MovementServiceTests
    {
        [TestMethod]
        public void HalfStep_UpdatesPositionAndVelocity()
        {
            var world = new World<double>(1000, 1000, 1.0);
            var e = world.CreateEntity();
            world.Components.SetPosition(e.Id, 0, 0);
            world.Components.SetVelocity(e.Id, 1, 2);
            world.Components.SetAcceleration(e.Id, 0.5, 0.5);

            var movement = new MovementService<double>(maxEntities: 100);
            double dt = 0.1;

            movement.HalfStep(world, dt);

            var pos = world.Components.GetRawCoordinates(1);
            var vel = world.Components.GetRawVelocities(1);

            // Ожидаемая скорость: v_new = v + a*(dt/2)
            Assert.AreEqual(1 + 0.5 * dt / 2, vel.X[0], 1e-10);
            Assert.AreEqual(2 + 0.5 * dt / 2, vel.Y[0], 1e-10);
            // Положение: x_new = x + v_new * dt
            Assert.AreEqual((1 + 0.5 * dt / 2) * dt, pos.X[0], 1e-10);
            Assert.AreEqual((2 + 0.5 * dt / 2) * dt, pos.Y[0], 1e-10);
        }

        [TestMethod]
        public void SecondHalf_UpdatesVelocityUsingAccelerationChange()
        {
            var world = new World<double>(1000, 1000, 1.0);
            var e = world.CreateEntity();
            world.Components.SetVelocity(e.Id, 1, 1);
            world.Components.SetAcceleration(e.Id, 0.2, 0.3);

            var movement = new MovementService<double>(maxEntities: 100);
            double dt = 0.1;

            // Симулируем предыдущее ускорение (через приватное поле не можем, но метод ожидает, что оно уже сохранено)
            // Вызовем HalfStep сначала, чтобы заполнить _prevAcc, затем изменим ускорение и вызовем SecondHalf
            movement.HalfStep(world, dt);
            // Меняем ускорение
            world.Components.SetAcceleration(e.Id, 0.5, 0.6);
            movement.SecondHalf(world, dt);

            var vel = world.Components.GetRawVelocities(1);
            // Изменение скорости: delta_v = (a_new - a_old) * dt/2
            // a_old было 0.2,0.3; a_new 0.5,0.6
            double expectedVx = 1 + 0.2 * dt / 2 + (0.5 - 0.2) * dt / 2;
            double expectedVy = 1 + 0.3 * dt / 2 + (0.6 - 0.3) * dt / 2;
            Assert.AreEqual(expectedVx, vel.X[0], 1e-10);
            Assert.AreEqual(expectedVy, vel.Y[0], 1e-10);
        }
    }

    [TestClass]
    public class WorldTests
    {
        [TestMethod]
        public void CreateEntity_ReturnsUniqueIds()
        {
            var world = new World<double>(100, 100, 1.0);
            var e1 = world.CreateEntity();
            var e2 = world.CreateEntity();
            Assert.AreNotEqual(e1.Id, e2.Id);
            Assert.AreEqual(2, world.GetActiveEntities());
        }

        [TestMethod]
        public void Components_AreAccessibleViaWorld()
        {
            var world = new World<double>(100, 100, 1.0);
            var e = world.CreateEntity();
            world.Components.SetPosition(e.Id, 1.5, 2.5);
            world.Components.SetMass(e.Id, 99.9);
            world.Components.SetVelocity(e.Id, 0.1, 0.2);

            var coords = world.Components.GetRawCoordinates(1);
            var masses = world.Components.GetMasses(1);
            var vels = world.Components.GetRawVelocities(1);

            Assert.AreEqual(1.5, coords.X[0]);
            Assert.AreEqual(2.5, coords.Y[0]);
            Assert.AreEqual(99.9, masses[0]);
            Assert.AreEqual(0.1, vels.X[0]);
            Assert.AreEqual(0.2, vels.Y[0]);
        }

        [TestMethod]
        public void GravityConstant_IsSetCorrectly()
        {
            double expectedG = 6.67430e-11;
            var world = new World<double>(1000, 1000, expectedG);
            Assert.AreEqual(expectedG, world.GetGravityConstant());
        }

        [TestMethod]
        public void TimeScale_CanBeChanged()
        {
            var world = new World<double>(100, 100, 1.0);
            world.SetTimeScale(2.5);
            Assert.AreEqual(2.5, world.GetTimeScale());
        }
    }
}