using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PrayPal.Common
{
    public class UsageCounter
    {
        private int _count;
        private readonly Action _actionOnZero;

        public UsageCounter(Action actionOnZero)
        {
            _actionOnZero = actionOnZero;
        }

        public IDisposable IncreaseCount()
        {
            Interlocked.Increment(ref _count);
            return new Disposable(this);
        }

        public int Count { get { return _count; } }

        private class Disposable : IDisposable
        {
            private readonly UsageCounter _usageCounter;

            public Disposable(UsageCounter usageCounter)
            {
                _usageCounter = usageCounter;
            }

            public void Dispose()
            {
                if (Interlocked.Decrement(ref _usageCounter._count) == 0)
                {
                    _usageCounter._actionOnZero?.Invoke();
                }
            }
        }
    }
}
