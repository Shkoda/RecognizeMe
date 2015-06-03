namespace Fakes.iOSFakeMovieTexture
{
    using UnityEngine;

    public class MovieTexture : Texture
    {
        public AudioClip audioClip { get; private set; }

        public bool loop { get; set; }

        public bool isPlaying { get; private set; }

        public bool isReadyToPlay { get; private set; }

        public float duration { get; private set; }

        public void Play()
        {
        }

        public void Stop()
        {
        }

        public void Pause()
        {
        }
    }
}