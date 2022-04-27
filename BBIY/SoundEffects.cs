using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace BBIY
{
    class SoundEffects
    {
        public static SoundEffect m_levelComplete;
        public static SoundEffect m_objectSink;
        public static SoundEffect m_playerDeath;
        public static SoundEffect m_playerMove;
        public static SoundEffect m_winConditionChanged;
        public static Song m_backgroundMusic;

        public static float m_backgroundMusicDuration;
        public static float m_elapsedTime;

        public static void LoadContent(ContentManager content)
        {
            m_levelComplete = content.Load<SoundEffect>("Audio/level-complete");
            m_objectSink = content.Load<SoundEffect>("Audio/object-sink");
            m_playerDeath = content.Load<SoundEffect>("Audio/player-death");
            m_playerMove = content.Load<SoundEffect>("Audio/player-move");
            m_winConditionChanged = content.Load<SoundEffect>("Audio/win-condition-changed");
            m_backgroundMusic = content.Load<Song>("Audio/background-music");

            m_backgroundMusicDuration = m_backgroundMusic.Duration.Seconds;
            m_elapsedTime = 0;
            MediaPlayer.IsRepeating = true;
        }

        public static void playBackgroundMusic()
        {
            MediaPlayer.Play(m_backgroundMusic);
        }

        public static void stopBackgroundMusic()
        {
            MediaPlayer.Stop();
        }

        public static void levelComplete()
        {
            m_levelComplete.Play();
        }

        public static void objectSink()
        {
            m_objectSink.Play();
        }

        public static void playerDeath()
        {
            m_playerDeath.Play();
        }

        public static void playerMove()
        {
            m_playerMove.Play();
        }

        public static void winConditionChanged()
        {
            m_winConditionChanged.Play();
        }
    }
}
