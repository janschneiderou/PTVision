using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTVision
{
    public class VolumeAnalysis
    {

        #region Volume helpers
        
        private bool softSpeaking = false;
        private bool loudSpeaking = false;
        private bool isProbablySpeaking = false;
        private bool isProbablySpeakingLoud = false;
        private bool isProbablySpeakingSoft = false;

        

        private double speakingTime;
        private double pauseTime;
        private double speakTimeStart;
        private double pauseTimeStart;
        private double loudTimeStart;
        private double softTimeStart;

        private double currentTime;

        private int sentencesCounter = 0;

        #endregion

        public VolumeAnalysis()
        {

        }

        public void analyse()
        {
            isSpeakingAnalysis();
        }

        private void isSpeakingAnalysis()
        {
            currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            if (Globals.isSpeaking == false)
            {
                if (Globals.currentAudioLevel > Globals.t_isSpeakingThreshold && isProbablySpeaking == false)
                {
                    
                    if (Globals.practiceSession != null)
                    {
                        Globals.sentenceStarted = DateTime.Now - Globals.practiceSession.start;
                    }

                    speakTimeStart = currentTime;
                    isProbablySpeaking = true;
                }
                else if (Globals.currentAudioLevel > Globals.t_isSpeakingThreshold && isProbablySpeaking == true && (currentTime - speakTimeStart) > Globals.t_guessTime)
                {
                    Globals.isSpeaking = true;
                    sentencesCounter++;
                }
                if (currentTime - pauseTimeStart > Globals.t_pauseThreshold)
                {
                    Globals.m_speakingLongMistake = false;
                    sentencesCounter = 0;
                    if (!Globals.f_pausingGoodLogged)
                    {
                        Globals.f_pausingGood = true;
                    }
                }
                if (currentTime - pauseTimeStart > Globals.t_pausingTooLongTime)
                {
                    Globals.m_pausingLongMistake = true;
                    Globals.f_pausingGood = false;
                }

            }
            else
            {
                Globals.f_pausingGoodLogged = false;
                Globals.f_pausingGood = false;

                if (Globals.f_speakingLoud)
                {
                    analyseLoudSpeaking();
                }
                if (Globals.f_speakingSoft)
                {
                    analyseSoftSpeaking();
                }
                Globals.m_pausingLongMistake = false;

                if (Globals.currentAudioLevel < Globals.t_isSpeakingThreshold && isProbablySpeaking == true)
                {
                    pauseTimeStart = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    isProbablySpeaking = false;
                }
                else if (Globals.currentAudioLevel < Globals.t_isSpeakingThreshold && isProbablySpeaking == false && (currentTime - pauseTimeStart) > Globals.t_guessTime)
                {
                    Globals.isSpeaking = false;

                }
                if ((currentTime - speakTimeStart > Globals.t_speekingTooLongTime || sentencesCounter > Globals.t_sentencesWithoutPauses) && Globals.f_speakingLong == true)
                {
                    Globals.m_speakingLongMistake = true;
                }
            }
        }

        private void analyseLoudSpeaking()
        {
            if (Globals.currentAudioLevel > Globals.t_loudSpeakingThreshold && isProbablySpeakingLoud == false)
            {
                loudTimeStart = currentTime;
                isProbablySpeakingLoud = true;
            }
            else if (Globals.currentAudioLevel > Globals.t_loudSpeakingThreshold && (currentTime - loudTimeStart) > Globals.t_guessTime)
            {
                Globals.m_speakingLoudMistake = true;
            }
            else if (Globals.currentAudioLevel < Globals.t_loudSpeakingThreshold)
            {
                isProbablySpeakingLoud = false;
                Globals.m_speakingLoudMistake = false;
            }
        }

        private void analyseSoftSpeaking()
        {
            if (Globals.currentAudioLevel < Globals.t_softSpeakingThreshold && isProbablySpeakingSoft == false)
            {
                softTimeStart = currentTime;
                isProbablySpeakingSoft = true;
            }
            else if (Globals.currentAudioLevel < Globals.t_softSpeakingThreshold && (currentTime - softTimeStart) > Globals.t_guessTime)
            {
                Globals.m_speakingSoftMistake = true;
            }
            else if (Globals.currentAudioLevel > Globals.t_softSpeakingThreshold)
            {
                isProbablySpeakingSoft = false;
                Globals.m_speakingSoftMistake = false;
            }
        }

    }
}
