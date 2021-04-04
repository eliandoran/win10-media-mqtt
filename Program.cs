using System;
using System.Threading.Tasks;

namespace UwpCompanion
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var mediaControls = new MediaControls();
            await mediaControls.Initialize();
            await mediaControls.Pause();
        }
    }
}
