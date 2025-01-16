using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Modules
{
    public class Timer
    {
        private bool _isTimerRunning;
        private CancellationTokenSource _cancellationTokenSource;
        
        public int RemainingTime { get; private set; }

        public async UniTask StartTimer(int durationSeconds, Action<int> onTick = null)
        {
            if (_isTimerRunning) return;

            _isTimerRunning = true;
            _cancellationTokenSource = new CancellationTokenSource();
            RemainingTime = durationSeconds;

            try
            {
                while (RemainingTime > 0)
                {
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    onTick?.Invoke(RemainingTime);
                    await UniTask.Delay(1000, cancellationToken: _cancellationTokenSource.Token);
                    RemainingTime--;
                }

                onTick?.Invoke(0);
            }
            catch (OperationCanceledException)
            {
                onTick?.Invoke(0);
            }
            finally
            {
                _isTimerRunning = false;
                _cancellationTokenSource.Dispose();
            }
        }

        public void UpdateTimer(int remainingTime)
        {
            RemainingTime = remainingTime;
        }
        
        public void StopTimer()
        {
            if (_isTimerRunning && _cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }
        }
    }
}