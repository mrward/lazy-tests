using System;
using NUnit.Framework;
using System.Threading;

namespace LazyTests
{
	[TestFixture]
	public class ThreadedLazyTests
	{
		[Test]
		public void LazyValueCalledBySecondThreadWhilstFirstThreadInLock ()
		{
			var goEvent = new AutoResetEvent (false);

			var lazy = new Lazy<string> (() => {
				goEvent.Set ();
				Thread.Sleep (2000);
				throw new Exception ("Lazy exception");
			});

			Exception firstLazyEx = null;
			var thread = new Thread (() => {
				try {
					string value = lazy.Value;
				} catch (Exception ex) {
					firstLazyEx = ex;
				}
			});
			thread.Start ();

			goEvent.WaitOne ();
			Exception secondLazyEx = null;
			try {
				string value = lazy.Value;
			} catch (Exception ex) {
				secondLazyEx = ex;
			}

			Assert.AreEqual ("Lazy exception", secondLazyEx.Message);
			Assert.AreEqual ("Lazy exception", firstLazyEx.Message);
			Assert.AreSame (firstLazyEx, secondLazyEx);
		}
	}
}

