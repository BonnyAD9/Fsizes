using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsizes;

internal class TimeThrottledProgress<T> : IProgress<T>
{
    public TimeThrottledProgress(IProgress<T> progress, TimeSpan delta)
    {
        this.delta = delta;
        this.progress = progress;
        lastTime = DateTime.MinValue;
    }

    private readonly TimeSpan delta;
    private readonly IProgress<T> progress;
    private DateTime lastTime;

    public void Report(T value)
    {
        if (DateTime.Now - lastTime < delta)
            return;
        lastTime = DateTime.Now;
        progress.Report(value);
    }
}
