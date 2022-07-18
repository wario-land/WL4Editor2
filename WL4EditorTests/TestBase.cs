namespace WL4EditorTests
{
    public class TestBase
    {
        protected static string TestDataDirectory = "..\\..\\..\\TestData";
        protected delegate void ThreadedTestCallback(ThreadedTestResult result);

        protected class ThreadedTestResult
        {
            public object Data = null;
            public bool Success = false;
            public string FailureMessage = null;
            public ThreadedTestCallback Callback = (_) => { };
        }

        protected static void RunThreadedTests(ThreadedTestCallback callback, IEnumerable<object> dataList)
        {
            var mutex = new Mutex();
            var runningThreads = dataList.Count();
            var testResults = new List<ThreadedTestResult>();

            // Queue test threads
            foreach (var data in dataList)
            {
                var testResult = new ThreadedTestResult
                {
                    Data = data,
                    Callback = callback
                };
                testResults.Add(testResult);
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    var testResult = o as ThreadedTestResult ?? throw new Exception("Internal testing exception: ThreadedTestResult object null");
                    try
                    {
                        testResult.Callback(testResult);
                    }
                    catch (Exception e)
                    {
                        testResult.FailureMessage = e.Message;
                        testResult.Success = false;
                    }
                    mutex.WaitOne();
                    {
                        --runningThreads;
                    }
                    mutex.ReleaseMutex();
                }, testResult);
            }

            // Set a timeout timer in case a test gets stuck
            bool termination = false;
            Timer timeout = new Timer((_) =>
            {
                mutex.WaitOne();
                {
                    termination = true;
                }
                mutex.ReleaseMutex();
            }, null, 30 * 1000, Timeout.Infinite);

            // Wait for threads to complete
            while(true)
            {
                Thread.Yield();
                mutex.WaitOne();
                {
                    if(runningThreads == 0)
                    {
                        break;
                    }
                    else if(termination)
                    {
                        throw new Exception($"Multithreaded test with {dataList.Count()} threads timed out ({runningThreads} threads still active)");
                    }
                }
                mutex.ReleaseMutex();
            }

            // Aggregate results
            var failures = testResults.Count((t) => !t.Success);
            var tr = testResults.FirstOrDefault((t) => !t.Success);
            Assert.AreEqual(0, failures, $"{failures}/{dataList.Count()} threads resulted in test failure. One such failure: {tr?.FailureMessage} (data: {tr?.Data})");
        }
    }
}