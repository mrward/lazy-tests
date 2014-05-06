using System;
using NUnit.Framework;

namespace LazyTests
{
	[TestFixture]
	public class LazyTests
	{
		[Test]
		public void UseLazyValueTwiceWhenLazyFactoryThrowsException ()
		{
			var lazy = new Lazy<string>(() => {
				throw new Exception("My lazy error message");
			});

			string value;
			try {
				value = lazy.Value;
			} catch {
			}

			Exception lazyEx = null;
			try {
				value = lazy.Value;
			} catch (Exception ex) {
				lazyEx = ex;
			}

			Assert.AreEqual ("My lazy error message", lazyEx.Message);
		}
	}
}

