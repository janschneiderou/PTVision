using PTVision.utilObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTVision.LogObjects;
using System.Net.Sockets;

namespace PTVision
{
    public class Globals
    {

        #region dimensions
        public static int SourceTop;
        public static int SourceLeft;
        public static int SourceWidth;
        public static int SourceHeight;
        #endregion

        #region speech
        public static int currentWord = 0;
        /// <summary>
        /// list of predefined commands
        /// </summary>
        public static List<Word> words = new List<Word>();
        #endregion

        #region process
        public static Process MediaPProcess;
        #endregion

        #region control
        public static bool ShuttingDown = false;
        #endregion

        #region Configurations
        public static int selectedCamera = 0;
        #endregion

        #region utilStrings
        public static string string_FFmpeg = "\\HelpExes\\FFmpeg\\bin\\ffmpeg.exe";
        public static string scriptPath="";
        public static string SlidesPath;
        #endregion

        #region Volume

        #region Volume helpers
        public static bool isSpeaking = false;
        public static int currentAudioLevel;
        public static int audioFactor = 5;


        #endregion

        #region Volume mistakes

        public static bool m_speakingLongMistake = false;
        public static bool m_speakingLoudMistake = false;
        public static bool m_speakingSoftMistake = false;
        public static bool m_pausingLongMistake = false;

        #endregion

        #region Volume flags
        public static bool f_speakingLong = true;
        public static bool f_speakingLoud = true;
        public static bool f_speakingSoft = true;
        public static bool f_pausingLong = true;
        public static bool f_pausingGood = false;
        public static bool f_pausingGoodLogged = false;

        #endregion


        #region Volume thresholds
        public static int t_isSpeakingThreshold = 2;
        public static int t_softSpeakingThreshold = 5;
        public static int t_loudSpeakingThreshold = 35;

        public static double t_pauseThreshold = 1000;

        public static double t_speekingTooLongTime = 7000;
        public static double t_pausingTooLongTime = 5000;
        public static TimeSpan sentenceStarted;
        public static double t_guessTime = 500;
        public static int t_sentencesWithoutPauses = 3;

        #endregion

        #endregion

        #region posturestuff

        #region posture flags
        public static bool f_CrossedArms = true;
        public static bool f_CrossedLegs = true;
        public static bool f_dancing = true;
        public static bool f_noGestures = true;
        public static bool f_handsFace = true;
        public static bool f_noHands = true;
        public static bool f_gestureStarted = false;
        #endregion

        #region mistakes
        public static bool m_CrossedArms = false;
        public static bool m_CrossedLegs = false;
        public static bool m_dancing = false;
        public static bool m_noGestures = false;
        public static bool m_handsFace = false;
        public static bool m_noHands = false;

        #endregion


        #region Thresholds
        public static int t_handSize = 10;
        public static double t_posture = 500;
        public static double t_gesture = 3000;
        public static int t_sentencesWithoutGestures = 3;
        public static int t_dances = 4;
        #endregion


        #region angles
        public static double angLeftLowArm;
        public static double angRightLowArm;
        public static double angleftUpArm;
        public static double angRightUpArm;
        #endregion

        #endregion

        #region userPAths
        public static string usersPath = "";
        public static string presentationPath = "";
        public static string usersPathScripts = "";
        public static string usersPathVideos = "";
        public static string usersPathLogs = "";
        #endregion

        public static List<SlideConfig> SlideConfigs = new List<SlideConfig>();
        public static PracticeSession practiceSession;

        public static bool readyToPresent = false;

        public static TcpClient MediaPclient;
        
    }
}
