using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Control;

namespace UwpCompanion
{
    class MediaControls
    {
        private bool isInitialized = false;
        private GlobalSystemMediaTransportControlsSession currentSession = null;

        public async Task Initialize()
        {
            // API courtesy of https://stackoverflow.com/a/63099881.
            var sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            var currentSession = sessionManager.GetCurrentSession();

            if (currentSession == null)
            {
                throw new Exception("Unable to obtain current session.");
            }

            this.currentSession = currentSession;
            isInitialized = true;
        }
        public async Task Pause()
        {
            EnsureInitialized();
            await currentSession.TryPauseAsync();
        }

        public async Task Play()
        {
            EnsureInitialized();
            await currentSession.TryPlayAsync();
        }

        public async Task PlayPause()
        {
            EnsureInitialized();
            await currentSession.TryTogglePlayPauseAsync();
        }

        public async Task<MediaProperties> GetMediaInfo()
        {
            var systemMediaProperties = await currentSession.TryGetMediaPropertiesAsync();
            return new MediaProperties()
            {
                Artist = systemMediaProperties.Artist,
                Title = systemMediaProperties.Title
            };
        }

        private void EnsureInitialized()
        {
            if (!isInitialized)
            {
                throw new Exception("Media controls must be initialized first.");
            }
        }

    }
}
