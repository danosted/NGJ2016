using System.Linq;
using Assets.Code.Common.BaseClasses;
using UnityEngine;

namespace Assets.Code.PrefabAccess
{
    public class AudioHandler : MonoBehaviour
    {
        public int CurrentTrack { get; set; }
        public AudioSource[] AudioSources { get; set; }

        void Awake()
        {
            AudioSources = gameObject.GetComponents<AudioSource>();
        }

        void Update()
        {
            if (AudioSources.Any(x => x.isPlaying)) return;
            CurrentTrack = SelectNextTrack();
            AudioSources[CurrentTrack].Play();
        }

        private int SelectNextTrack()
        {
            if (CurrentTrack == 0 || CurrentTrack == 2)
            {
                return 1;
            }
            return Random.Range(0, 2) == 0 ? 0 : 2;
        }
    }
}
