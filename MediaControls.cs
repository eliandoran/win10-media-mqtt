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
            currentSession.MediaPropertiesChanged += CurrentSession_MediaPropertiesChanged;
            currentSession.PlaybackInfoChanged += CurrentSession_PlaybackInfoChanged;
            isInitialized = true;
        }

        private void CurrentSession_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
        {
            Console.WriteLine("Playback info changed.");
        }

        private async void CurrentSession_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
        {
            Console.WriteLine("Media properties changed.");

            var newMediaInfo = await GetMediaInfo();
            OnMediaInfoChanged(new MediaInfoChangedEventArgs()
            {
                mediaInfo = newMediaInfo
            });
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

        public async Task<MediaInfo> GetMediaInfo()
        {
            var systemMediaProperties = await currentSession.TryGetMediaPropertiesAsync();
            return new MediaInfo()
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

        protected virtual void OnMediaInfoChanged(MediaInfoChangedEventArgs e)
        {
            var handler = MediaInfoChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<MediaInfoChangedEventArgs> MediaInfoChanged;

        public class MediaInfoChangedEventArgs : EventArgs
        {
            public MediaInfo mediaInfo { get; set; }
        }

    }
}
