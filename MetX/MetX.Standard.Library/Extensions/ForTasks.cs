using System;
using System.Threading.Tasks;

namespace MetX.Standard.Library.Extensions;

public static class ForTasks
{
    public static Task FireAndForget(this Action action)
    {
        if(action == null)
            return null;

        var task = Task.Run(() =>
        {
            try
            {
                action();
            }
            catch
            {
                // Ignored
            }
        });
        return task;
    }
}