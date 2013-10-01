using System;
using NUnit.Framework;
using System.ComponentModel;
using Hjerpbakk.AsyncMethodCaller;
using Hjerpbakk.AsyncMethodCaller.TestUtility;

namespace ViewModelWithAsync {
	public class ViewModelWithBackgroundWorker {
		private readonly BackgroundWorker worker;

		public ViewModelWithBackgroundWorker() {
			worker = new BackgroundWorker();
			worker.DoWork += DoSomething;
			worker.RunWorkerCompleted += WorkCompleted;
		}

		public string Message { get; set; }
		public string Result { get; set; }

		public void ExecuteAsync() {
			Message = "Loading...";
			worker.RunWorkerAsync();
		}

		private void DoSomething(object sender, DoWorkEventArgs e) {
			e.Result = "Result";
		}

		private void WorkCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if (e.Error != null) {
				HandleError(e.Error);
			} else {
				Message = "Completed";
				Result = (string)e.Result;
			}
		}

		private void HandleError(Exception exception) {
			Message = exception.Message;
		}
	}

	public class ViewModelWithAsyncMethodCaller {
		private readonly AsyncMethodCaller asyncMethodCaller;

		public ViewModelWithAsyncMethodCaller(AsyncMethodCaller asyncMethodCaller) {
			this.asyncMethodCaller = asyncMethodCaller ?? new AsyncMethodCaller();
		}

		public string Message { get; set; }
		public string Result { get; set; }

		public void ExecuteAsync() {
			Message = "Loading...";
			asyncMethodCaller.CallMethodAndContinue(DoSomething, WorkCompleted, HandleError);
		}

		private string DoSomething() {
			return "Result";
		}

		private void WorkCompleted(string result) {
			Message = "Completed";
			Result = result;
		}

		private void HandleError(Exception exception) {
			Message = exception.Message;
		}
	}

	[TestFixture]
	public class ViewModelTests {
		[Test]
		public void DoSomething() {
			var asyncMethodCaller = new TestAsyncMethodCaller();
			var viewModel = new ViewModelWithAsyncMethodCaller(asyncMethodCaller);

			viewModel.ExecuteAsync();

			Assert.AreEqual("Loading...", viewModel.Message);

			asyncMethodCaller.StartServiceAndWait();

			Assert.AreEqual("Result", viewModel.Result);
			Assert.AreEqual("Completed", viewModel.Message);
		}
	}
}

