using System;
using System.Collections.Generic;
using System.IO;
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

        private void CurrentSession_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, Windows.Media.Control.PlaybackInfoChangedEventArgs args)
        {
            Console.WriteLine("Playback status changed.");

            var newPlaybackStatus = GetPlaybackStatus();
            OnPlaybackStatusChanged(new PlaybackStatusEventArgs()
            {
                PlaybackStatus = newPlaybackStatus
            });
        }

        private async void CurrentSession_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
        {
            Console.WriteLine("Media properties changed.");

            var newMediaInfo = await GetMediaInfo();
            OnMediaInfoChanged(new MediaInfoChangedEventArgs()
            {
                MediaInfo = newMediaInfo
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

        public async Task SkipPrevious()
        {
            EnsureInitialized();
            await currentSession.TrySkipPreviousAsync();
        }

        public async Task SkipNext()
        {
            EnsureInitialized();
            await currentSession.TrySkipNextAsync();
        }

        public async Task<MediaInfo> GetMediaInfo()
        {
            var systemMediaProperties = await currentSession.TryGetMediaPropertiesAsync();
            var thumbnailData = systemMediaProperties.Thumbnail;
            var thumbnail = "";

            if (thumbnailData != null)
            {
                var thumbnailStream = (await thumbnailData.OpenReadAsync()).AsStreamForRead();
                byte[] bytes;
                using (var memoryStream = new MemoryStream())
                {
                    thumbnailStream.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                thumbnail = Convert.ToBase64String(bytes);
            }

            return new MediaInfo()
            {
                Artist = systemMediaProperties.Artist,
                Title = systemMediaProperties.Title,
                Thumbnail = thumbnail
            };
        }

        public PlaybackStatus GetPlaybackStatus()
        {
            var systemPlaybackProperties = currentSession.GetPlaybackInfo();
            return new PlaybackStatus()
            {
                IsPlaying = (systemPlaybackProperties.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
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

        protected virtual void OnPlaybackStatusChanged(PlaybackStatusEventArgs e)
        {
            var handler = PlaybackStatusChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<MediaInfoChangedEventArgs> MediaInfoChanged;
        public event EventHandler<PlaybackStatusEventArgs> PlaybackStatusChanged;

        public class MediaInfoChangedEventArgs : EventArgs
        {
            public MediaInfo MediaInfo { get; set; }
        }

        public class PlaybackStatusEventArgs : EventArgs
        {
            public PlaybackStatus PlaybackStatus { get; set; }
        }

    }
}
