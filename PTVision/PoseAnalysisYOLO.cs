using Newtonsoft.Json;
using PTVision.utilObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoloDotNet;
using YoloDotNet.Enums;
using YoloDotNet.Models;
using YoloDotNet.Extensions;


namespace PTVision
{
    internal class PoseAnalysisYOLO
    {

        YoloBody previous_Body;
        YoloBody current_Body;


        #region Thresholds
        public static int t_handSize = 10;
        public static double t_posture = 500;
        public static double t_gesture = 3000;
        public static int t_sentencesWithoutGestures = 3;
        public static int t_dances = 4;
        #endregion

        #region helpers
        double currentTime;
        double startCrossedArms;
        double startCrossedLegs;
        double startDancing;
        double startGestures;
        double startHandsFace;
        double startNoHands;
        double startRaisedArms;

        int startSpeakingCounter;
        bool startedSpeaking = false;

        bool possibleCrossedArms = false;
        bool possibleCrossedLegs = false;
        bool possibleRaisedArms = false;
        bool possibleDanding = false;
        bool possibleGestures = false;
        bool possibleHandsFace = false;
        bool possibleNoHands = false;

        bool gestureReseted = true;

        bool doAnalysis = true;

        public CalcArmsMovement2D calcArmsMovement2D;
        public PeriodicMovements periodicMovements;

        #endregion

        public PoseAnalysisYOLO()
        {
            current_Body = new YoloBody();
            previous_Body = new YoloBody();
            calcArmsMovement2D = new CalcArmsMovement2D();
            periodicMovements = new PeriodicMovements();
        }

        public void analysePose(PoseEstimation pose)
        {
            try
            {
                for (int i = 0; i < 17; i++)
                {
                    current_Body.keypoints[i] = new YoloKeypoint();
                    current_Body.keypoints[i].X = pose.KeyPoints[i].X;
                    current_Body.keypoints[i].Y = pose.KeyPoints[i].Y;
                    current_Body.keypoints[i].visibility = pose.KeyPoints[i].Confidence;
                }

            }
            catch (Exception ex)
            {
                int y = 0;
                doAnalysis = false;

            }
            currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;

            if (current_Body != null)
            {

                analyseCrossedArms();
                analyseCrossedLegs();
                analyseHandsCloseToFace();
                analyseNoHands();
                analyseGestureHappen();
                analyseDancing();
            }

        }

        public void analyseCrossedArms()
        {

            if (current_Body.keypoints[(int)YoloBodyParts.LEFT_ELBOW].visibility > 0.5)
            {
                if (current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].X < current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].X)
                {
                    if (current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].X > current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].X &&
                        current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].X < current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].X &&
                    current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y < current_Body.keypoints[(int)YoloBodyParts.LEFT_HIP].Y &&
                    current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y > current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].Y &&
                    current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y < current_Body.keypoints[(int)YoloBodyParts.RIGHT_HIP].Y &&
                    current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y > current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].Y
                    && possibleCrossedArms == false)
                    {
                        startCrossedArms = currentTime;
                        possibleCrossedArms = true;
                    }
                    else if (current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].X > current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].X &&
                        current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].X < current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].X &&
                    current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y < current_Body.keypoints[(int)YoloBodyParts.LEFT_HIP].Y &&
                    current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y > current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].Y &&
                    current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y < current_Body.keypoints[(int)YoloBodyParts.RIGHT_HIP].Y &&
                    current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y > current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].Y
                        && possibleCrossedArms == true && currentTime - startCrossedArms > t_posture)
                    {
                        Globals.m_CrossedArms = true;
                    }
                    else if (current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].X < current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].X
                        || current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].X > current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].X)
                    {
                        Globals.m_CrossedArms = false;
                        possibleCrossedArms = false;

                    }
                }
                else
                {
                    if (current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].X < current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].X &&
                         current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].X > current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].X &&
                     current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y < current_Body.keypoints[(int)YoloBodyParts.LEFT_HIP].Y &&
                     current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y > current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].Y &&
                     current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y < current_Body.keypoints[(int)YoloBodyParts.RIGHT_HIP].Y &&
                     current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y > current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].Y
                     && possibleCrossedArms == false)
                    {
                        startCrossedArms = currentTime;
                        possibleCrossedArms = true;
                    }
                    else if (current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].X < current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].X &&
                        current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].X > current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].X &&
                    current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y < current_Body.keypoints[(int)YoloBodyParts.LEFT_HIP].Y &&
                    current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y > current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].Y &&
                    current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y < current_Body.keypoints[(int)YoloBodyParts.RIGHT_HIP].Y &&
                    current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y > current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].Y
                        && possibleCrossedArms == true && currentTime - startCrossedArms > t_posture)
                    {
                        Globals.m_CrossedArms = true;
                    }
                    else if (current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].X > current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].X
                        || current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].X < current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].X)
                    {
                        Globals.m_CrossedArms = false;
                        possibleCrossedArms = false;

                    }
                }
            }
            else
            {
                Globals.m_CrossedArms = false;
                possibleCrossedArms = false;
            }

        }

        public void analyseCrossedLegs()
        {
            if (current_Body.keypoints[(int)YoloBodyParts.LEFT_ANKLE].visibility > 0.5 && current_Body.keypoints[(int)YoloBodyParts.RIGHT_ANKLE].visibility > 0.5)
            {
                if (current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].X < current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].X)
                {
                    if (current_Body.keypoints[(int)YoloBodyParts.LEFT_ANKLE].X > current_Body.keypoints[(int)YoloBodyParts.RIGHT_ANKLE].X &&
                    possibleCrossedLegs == false)
                    {
                        startCrossedLegs = currentTime;
                        possibleCrossedLegs = true;
                    }
                    else if (current_Body.keypoints[(int)YoloBodyParts.LEFT_ANKLE].X > current_Body.keypoints[(int)YoloBodyParts.RIGHT_ANKLE].X &&
                        possibleCrossedLegs == true && currentTime - startCrossedLegs > t_posture)
                    {
                        Globals.m_CrossedLegs = true;
                    }
                    else if (current_Body.keypoints[(int)YoloBodyParts.LEFT_ANKLE].X < current_Body.keypoints[(int)YoloBodyParts.RIGHT_ANKLE].X)
                    {
                        Globals.m_CrossedLegs = false;
                        possibleCrossedLegs = false;
                    }
                }
                else
                {
                    if (current_Body.keypoints[(int)YoloBodyParts.LEFT_ANKLE].X < current_Body.keypoints[(int)YoloBodyParts.RIGHT_ANKLE].X &&
                    possibleCrossedLegs == false)
                    {
                        startCrossedLegs = currentTime;
                        possibleCrossedLegs = true;
                    }
                    else if (current_Body.keypoints[(int)YoloBodyParts.LEFT_ANKLE].X < current_Body.keypoints[(int)YoloBodyParts.RIGHT_ANKLE].X &&
                        possibleCrossedLegs == true && currentTime - startCrossedLegs > t_posture)
                    {
                        Globals.m_CrossedLegs = true;
                    }
                    else if (current_Body.keypoints[(int)YoloBodyParts.LEFT_ANKLE].X > current_Body.keypoints[(int)YoloBodyParts.RIGHT_ANKLE].X)
                    {
                        Globals.m_CrossedLegs = false;
                        possibleCrossedLegs = false;
                    }
                }
            }
            else
            {
                Globals.m_CrossedLegs = false;
                possibleCrossedLegs = false;
            }

        }
        public void analyseNoHands()
        {
            if (current_Body.keypoints[(int)YoloBodyParts.LEFT_HIP].visibility > 0.5 && current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].visibility > 0.5)
            {
                if ((current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].visibility < 0.7 || current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].visibility < 0.7) &&
              possibleNoHands == false)
                {
                    startNoHands = currentTime;
                    possibleNoHands = true;
                }
                else if ((current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].visibility < 0.7 || current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].visibility < 0.7) &&
                   possibleNoHands == true && currentTime - startNoHands > t_posture)
                {
                    Globals.m_noHands = true;
                }
                else if (current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].visibility > 0.7 && current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].visibility > 0.7)
                {
                    Globals.m_noHands = false;
                    possibleNoHands = false;
                }
            }
            else
            {
                Globals.m_noHands = false;
                possibleNoHands = false;
            }


        }

        public void analyseHandsCloseToFace()
        {
            if ((current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].visibility > 0.7
                 || current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].visibility > 0.7)
                 && (current_Body.keypoints[(int)YoloBodyParts.NOSE].visibility > 0.5 ||
              
                 current_Body.keypoints[(int)YoloBodyParts.LEFT_EYE].visibility > 0.5 ||
                 current_Body.keypoints[(int)YoloBodyParts.RIGHT_EYE].visibility > 0.5))
            {


                float bottom = current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].Y;
                float top = current_Body.keypoints[(int)YoloBodyParts.LEFT_EYE].Y;
                float left = current_Body.keypoints[(int)YoloBodyParts.LEFT_EAR].X;
                float right = current_Body.keypoints[(int)YoloBodyParts.RIGHT_EAR].X;

                if (((current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y < bottom &&
                    current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y > top  &&
                    (current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].X < left &&
                    current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].X > right && current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].visibility > 0.7)) ||

                    (current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y < bottom &&
                    current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y > top) &&
                    (current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].X < left &&
                    current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].X > right &&  current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].visibility > 0.7))
                    && possibleHandsFace == false)
                {
                    startHandsFace = currentTime;
                    possibleHandsFace = true;
                }
                else if (((current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y < bottom &&
                    current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y > top &&
                    (current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].X < left &&
                    current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].X > right && current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].visibility > 0.7)) ||

                    (current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y < bottom &&
                    current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y > top) &&
                    (current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].X < left &&
                    current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].X > right && current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].visibility > 0.7))
                    && possibleHandsFace == true)
                {
                    Globals.m_handsFace = true;
                    possibleHandsFace = false;
                }

            }
            else
            {
                Globals.m_handsFace = false;
                possibleHandsFace = false;
            }
        }

        void analyseDancing()
        {
            if (current_Body.keypoints[(int)YoloBodyParts.LEFT_HIP].visibility > 0.7 && current_Body.keypoints[(int)YoloBodyParts.RIGHT_HIP].visibility > 0.7)
            {
                if (periodicMovements.checPeriodicMovements(current_Body.keypoints[(int)YoloBodyParts.LEFT_HIP].X, current_Body.keypoints[(int)YoloBodyParts.RIGHT_HIP].X))
                {
                    startDancing = currentTime;
                    Globals.m_dancing = true;
                }
                else
                {
                    if (currentTime - startDancing > t_dances)
                    {
                        Globals.m_dancing = false;
                    }
                }
            }

        }


        void analyseGestureHappen()
        {
            if (current_Body.keypoints[(int)YoloBodyParts.LEFT_ELBOW].visibility > 0.7
                || current_Body.keypoints[(int)YoloBodyParts.RIGHT_ELBOW].visibility > 0.7)
            {
                calcArmAngles();
                calcArmsMovement2D.calcArmMovements();
                if (Globals.isSpeaking && startedSpeaking == false)
                {
                    startedSpeaking = true;
                    startGestures = currentTime;
                    startSpeakingCounter++;
                    gestureReseted = true;

                }
                else if (Globals.isSpeaking)
                {
                    if (CalcArmsMovement2D.currentGesture == CalcArmsMovement2D.Gesture.nogesture)
                    {

                        if ((startSpeakingCounter > t_sentencesWithoutGestures && calcArmsMovement2D.prePreviousGesture == CalcArmsMovement2D.Gesture.nogesture)
                            || currentTime - startGestures > t_gesture)
                        {
                            Globals.m_noGestures = true;
                        }
                        Globals.f_gestureStarted = false;


                    }
                    else
                    {
                        if (CalcArmsMovement2D.currentGesture == CalcArmsMovement2D.Gesture.big)
                        {
                            if (gestureReseted)
                            {
                                Globals.f_gestureStarted = true;
                                gestureReseted = false;
                            }
                            else
                            {
                                Globals.f_gestureStarted = false;
                            }
                        }
                        else
                        {
                            Globals.f_gestureStarted = false;
                        }

                        Globals.m_noGestures = false;
                        startSpeakingCounter = 0;
                        calcArmsMovement2D.resetMaxAndMin();
                    }




                }
                else
                {
                    startedSpeaking = false;
                    Globals.f_gestureStarted = false;

                }
            }


        }

        public void analyseRaiseArms(PoseEstimation pose)
        {
            try
            {
                
                for(int i=0; i<17; i++)
                {
                    current_Body.keypoints[i] = new YoloKeypoint();
                    current_Body.keypoints[i].X = pose.KeyPoints[i].X;
                    current_Body.keypoints[i].Y = pose.KeyPoints[i].Y;
                    current_Body.keypoints[i].visibility = pose.KeyPoints[i].Confidence;
                }
            }
            catch (Exception ex)
            {
                int y = 0;
                doAnalysis = false;

            }

            currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;


            if (current_Body.keypoints[0] != null)
            {
                if (current_Body.keypoints[(int)YoloBodyParts.LEFT_ELBOW].visibility > 0.7 && current_Body.keypoints[(int)YoloBodyParts.RIGHT_ELBOW].visibility > 0.7)
                {
                    if (current_Body.keypoints[(int)YoloBodyParts.LEFT_EYE].Y > current_Body.keypoints[(int)YoloBodyParts.LEFT_ELBOW].Y &&
                        current_Body.keypoints[(int)YoloBodyParts.RIGHT_EYE].Y > current_Body.keypoints[(int)YoloBodyParts.RIGHT_ELBOW].Y
                        )
                    {

                        startRaisedArms = currentTime;
                        possibleRaisedArms = true;
                        Globals.f_RaisedArms = true;
                    }
                    else if (current_Body.keypoints[(int)YoloBodyParts.LEFT_EYE].Y > current_Body.keypoints[(int)YoloBodyParts.LEFT_ELBOW].Y &&
                            current_Body.keypoints[(int)YoloBodyParts.RIGHT_EYE].Y > current_Body.keypoints[(int)YoloBodyParts.RIGHT_ELBOW].Y
                            && possibleRaisedArms == true && currentTime - startRaisedArms > t_posture)
                    {
                        Globals.f_RaisedArms = true;
                    }
                    else
                    {
                        Globals.f_RaisedArms = false;
                        possibleRaisedArms = false;
                    }


                }
                else
                {
                    Globals.f_RaisedArms = false;
                    possibleRaisedArms = false;
                }
            }
        }

        public void calcArmAngles()
        {
            double upArmLineX = current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].X - current_Body.keypoints[(int)YoloBodyParts.LEFT_ELBOW].X;
            double upArmLineY = current_Body.keypoints[(int)YoloBodyParts.LEFT_SHOULDER].Y - current_Body.keypoints[(int)YoloBodyParts.LEFT_ELBOW].Y;

            Globals.angleftUpArm = Math.Atan(upArmLineX / upArmLineY) * 180 / Math.PI;

            upArmLineX = current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].X - current_Body.keypoints[(int)YoloBodyParts.RIGHT_ELBOW].X;
            upArmLineY = current_Body.keypoints[(int)YoloBodyParts.RIGHT_SHOULDER].Y - current_Body.keypoints[(int)YoloBodyParts.RIGHT_ELBOW].Y;

            Globals.angRightUpArm = Math.Atan(upArmLineX / upArmLineY) * 180 / Math.PI;

            double lowArmLineX = current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].X - current_Body.keypoints[(int)YoloBodyParts.LEFT_ELBOW].X;
            double lowArmLineY = current_Body.keypoints[(int)YoloBodyParts.LEFT_WRIST].Y - current_Body.keypoints[(int)YoloBodyParts.LEFT_ELBOW].Y;

            Globals.angLeftLowArm = Math.Atan(lowArmLineX / lowArmLineY) * 180 / Math.PI;

            lowArmLineX = current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].X - current_Body.keypoints[(int)YoloBodyParts.RIGHT_ELBOW].X;
            lowArmLineY = current_Body.keypoints[(int)YoloBodyParts.RIGHT_WRIST].Y - current_Body.keypoints[(int)YoloBodyParts.RIGHT_ELBOW].Y;

            Globals.angRightLowArm = Math.Atan(lowArmLineX / lowArmLineY) * 180 / Math.PI;

            //int x = 0;

        }

    }
}
