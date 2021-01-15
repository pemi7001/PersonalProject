using System.Threading.Tasks;

namespace BrownCow
{
    class Program
    {
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}
