using System;
using NUnit.Framework;

namespace BadUnitTest {
	[TestFixture]
	public class BadUnitTestExample {
		/// <summary>
		/// Due to its dependencies of the current time, 
		/// this is actually an integration test.
		/// </summary>
		[Test]
		public void AgeMustReturnCorrectAge_Bad() {
			var aPerson = new PersonBad { TimeOfBirth = new DateTime(1983, 9, 25) };

			Assert.AreEqual(29, aPerson.Age);
		}

		/// <summary>
		/// This is a proper unit test with only explicit external dependencies 
		/// which can be stubbed out manually or by using a mock framework.
		/// </summary>
		[Test]
		public void AgeMustReturnCorrectAge_Good() {
			IClock clock = new StubClock(new DateTime(2013, 9, 2));

			var aPerson = new PersonGood(clock) { TimeOfBirth = new DateTime(1983, 9, 8) };

			Assert.AreEqual(29, aPerson.Age);
		}
	}

	/// <summary>
	/// A person implementation which uses DateTime internally.
	/// </summary>
	public class PersonBad {
		public DateTime TimeOfBirth { get; set; }

		public int Age { get {
				var today = DateTime.Today;
				int age = today.Year - TimeOfBirth.Year;
				if (TimeOfBirth > today.AddYears(-age)) --age;
				return age;
			}
		}
	}

	/// <summary>
	/// A person implementation with an explicit dependency
	/// on the current day.
	/// </summary>
	public class PersonGood {
		private readonly IClock clock;

		public PersonGood(IClock clock = null) {
			this.clock = clock ?? new Clock();
		}

		public DateTime TimeOfBirth { get; set; }

		public int Age { 
			get {
				var today = clock.Today;
				int age = today.Year - TimeOfBirth.Year;
				if (TimeOfBirth > today.AddYears(-age)) --age;
				return age;
			}
		}
	}

	/// <summary>
	/// Production implementation.
	/// </summary>
	public class Clock : IClock {
		public DateTime Today { get { return DateTime.Today; } }
	}

	/// <summary>
	/// Stub to be used in tests.
	/// </summary>
	public class StubClock : IClock {
		private readonly DateTime now;

		public StubClock(DateTime now) {
			this.now = now;
		}

		public DateTime Today {
			get {
				return now.Date;
			}
		}
	}

	/// <summary>
	/// A real abstraction over DateTime would contain more
	/// properties and methods. NodaTime anyone?
	/// </summary>
	public interface IClock {
		DateTime Today { get; }
	}
}

