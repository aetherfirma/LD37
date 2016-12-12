using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class Message
    {
        public string Text;
        public AudioClip Audio;


        public Message(string text, AudioClip clip)
        {
            Text = text;
            Audio = clip;
        }
    }
}