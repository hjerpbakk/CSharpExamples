using System.Collections.Generic;
using NUnit.Framework;
using Infrastructure.IoC;

namespace InfrastructureTests.IoC
{
	[TestFixture]
	public class SimpleContainerTests
	{
		private ISimpleContainer m_simpleContainer;

		[SetUp]
		public void Init()
		{
			m_simpleContainer = new SimpleContainer();
		}

		[Test]
		public void Resolve_ClassRegistered_ReturnsNewObject()
		{
			m_simpleContainer.Register<ICalculator, Calculator>();

			var calculator = m_simpleContainer.Resolve<ICalculator>();

			var sum = calculator.Add(1, 2);
			Assert.AreEqual(3, sum);
		}

		[Test]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void Resolve_ClassNotRegistered_ThrowsException()
		{
			m_simpleContainer.Resolve<ICalculator>();
		}

		[Test]
		public void ResolveTwoTimes_ClassRegistered_TwoObjectsCreated()
		{
			m_simpleContainer.Register<ICalculator, Calculator>();
			var calculator = m_simpleContainer.Resolve<ICalculator>();

			var calculator2 = m_simpleContainer.Resolve<ICalculator>();

			Assert.AreNotSame(calculator, calculator2);
		}

		[Test]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void Singleton_ClassNotRegistered_ThrowsException()
		{
			m_simpleContainer.Singleton<ICalculator>();
		}

		[Test]
		public void SingletonTwoTimes_ClassRegistered_OneObjectCreated()
		{
			m_simpleContainer.Register<ICalculator, Calculator>();
			var calculator = m_simpleContainer.Singleton<ICalculator>();

			var calculator2 = m_simpleContainer.Singleton<ICalculator>();

			Assert.AreSame(calculator, calculator2);
		}

		private interface ICalculator
		{
			int Add(int a, int b);
		}

		private class Calculator : ICalculator
		{
			public int Add(int a, int b)
			{
				return a + b;
			}
		}
	}
}
