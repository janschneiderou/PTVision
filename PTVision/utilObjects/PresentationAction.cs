﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PTVision.utilObjects
{
    internal class PresentationAction
    {
        bool isMistake = true;

        public enum GoodType
        {
            RESETPOSTURE,
            GESTURE,
            SMILE
        };

        public GoodType myGoodie;

        public enum MistakeType
        {
            ARMSCROSSED,
            LEGSCROSSED,
            RIGHTHANDNOTVISIBLE,
            LEFTHANDNOTVISIBLE,
            HANDS_NEAR_FACE,
            HANDS_NOT_MOVING,
            DANCING,
            LONG_PAUSE,
            LONG_TALK,
            HIGH_VOLUME,
            LOW_VOLUME,
            HMMMM,
            NOMISTAKE
        };
        public MistakeType myMistake;
        public bool hasFinished = false;
        public bool isVoiceAndMovementMistake = false;

        public double minVolume;
        public double maxVolume;
        public double averageVolume;
        public bool isSpeaking = false;

        public bool interrupt = false;

        public double totalHandMovement = 1000;
        public double averageHandMovement;

        public double timeStarted = 0;
        public double timeFinished;

        public ImageSource firstImage = null;
        public ImageSource lastImage = null;


        public string message = "";

        public double leftHandHipDistance;
        public double rightHandHipDistance;

        public PresentationAction()
        {

        }
        public PresentationAction(MistakeType myMistake)
        {
            this.myMistake = myMistake;
            setMistakeDefaults();
            isMistake = true;
            timeStarted = DateTime.Now.TimeOfDay.TotalMilliseconds;
        }

        public PresentationAction(GoodType myGoodie)
        {
            this.myGoodie = myGoodie;
            setGoodieDefaults();
            isMistake = false;
            timeStarted = DateTime.Now.TimeOfDay.TotalMilliseconds;
        }

        public PresentationAction Clone()
        {
            PresentationAction pa = new PresentationAction();

            pa.averageHandMovement = this.averageHandMovement;
            pa.averageVolume = this.averageVolume;
            if (firstImage != null)
            {
                pa.firstImage = this.firstImage.Clone();
            }

            pa.hasFinished = this.hasFinished;
            pa.interrupt = this.interrupt;
            pa.isMistake = this.isMistake;
            pa.isSpeaking = this.isSpeaking;
            pa.isVoiceAndMovementMistake = this.isVoiceAndMovementMistake;
            if (lastImage != null)
            {
                pa.lastImage = this.lastImage.Clone();
            }

            pa.maxVolume = this.maxVolume;
            pa.message = this.message;
            pa.minVolume = this.minVolume;
            pa.myGoodie = this.myGoodie;
            pa.myMistake = this.myMistake;
            pa.timeFinished = this.timeFinished;
            pa.totalHandMovement = this.totalHandMovement;

            return pa;
        }

        private void setGoodieDefaults()
        {
            switch (myGoodie)
            {
                case GoodType.RESETPOSTURE:
                    message = "Nice Posture";
                    break;
            }
        }

        public void setMistakeDefaults()
        {
            switch (myMistake)
            {
                case MistakeType.ARMSCROSSED:
                case MistakeType.LEGSCROSSED:
                case MistakeType.HANDS_NEAR_FACE:
                case MistakeType.LEFTHANDNOTVISIBLE:
                case MistakeType.RIGHTHANDNOTVISIBLE:
                    message = "Reset Posture";
                    break;
                case MistakeType.DANCING:
                    message = "Stand Still";
                    break;
                case MistakeType.HANDS_NOT_MOVING:
                    message = "Use Gestures";
                    break;
                case MistakeType.HIGH_VOLUME:
                    message = "Lower Volume";
                    break;
                case MistakeType.LOW_VOLUME:
                    message = "Increase Volume";
                    break;
                case MistakeType.LONG_PAUSE:
                    message = "Start Talking";
                    break;
                case MistakeType.LONG_TALK:
                    message = "Stop Talking";
                    break;
                case MistakeType.HMMMM:
                    message = "Stop HMMMMing";
                    break;

            }
        }

    }
}
