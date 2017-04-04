using System;

namespace Banana.Common
{
    public class PerformanceTimerHelper : IDisposable
    {
        private readonly Action<double> _secondsTaken;
        private readonly PerformanceTimer _timer;

        public PerformanceTimerHelper(
            Action<double> secondsTaken
            )
        {
            if (secondsTaken == null)
            {
                throw new ArgumentNullException("secondsTaken");
            }
            _secondsTaken = secondsTaken;
            _timer = new PerformanceTimer();
        }

        public void Dispose()
        {
            var secondsTaken = _timer.Seconds;
            _secondsTaken(secondsTaken);
        }
    }
}