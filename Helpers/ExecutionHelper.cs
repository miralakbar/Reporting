using System;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class ExecutionHelper
    {
        public static async Task<string> SafeExecuteAsync(Func<Task<string>> action)
        {
            try
            {
                return await action();
            }
            catch
            {
                return null;
            }
        }
    }
}
