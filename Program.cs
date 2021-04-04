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
            
            var mediaProperties = await mediaControls.GetMediaInfo();
            if (mediaProperties != null)
            {
                Console.WriteLine("Title:  {0}", mediaProperties.Title);
                Console.WriteLine("Artist: {0}", mediaProperties.Artist);
            }
        }
    }
}
