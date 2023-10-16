using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTVision.utilObjects
{
    internal class CalcArmsMovement2D
    {

        public enum Gesture { nogesture, small, medium, big };//TODO
        public static Gesture currentGesture;
        public Gesture previousGesture;
        public Gesture prePreviousGesture;

        public double MinAngleLeftArmShoulderLineA = 0;
        public double MinAngleLeftArmShoulderLineB = 0;


        public double MinAngleRightArmShoulderLineA = 0;
        public double MinAngleRightArmShoulderLineB = 0;



        public double MaxAngleLeftArmShoulderLineA = 0;
        public double MaxAngleLeftArmShoulderLineB = 0;


        public double MaxAngleRightArmShoulderLineA = 0;
        public double MaxAngleRightArmShoulderLineB = 0;



        public double MinAngleLeftForearmLeftArmA = 0;
        public double MinAngleLeftForearmLeftArmB = 0;


        public double MinAngleRightForearmRightArmA = 0;
        public double MinAngleRightForearmRightArmB = 0;


        public double MaxAngleLeftForearmLeftArmA = 0;
        public double MaxAngleLeftForearmLeftArmB = 0;


        public double MaxAngleRightForearmRightArmA = 0;
        public double MaxAngleRightForearmRightArmB = 0;


        bool GrowingAngleLeftArmShoulderLineA = true;
        // bool GrowingAngleLeftArmShoulderLineB = true;


        bool GrowingAngleRightArmShoulderLineA = true;
        // bool GrowingAngleRightArmShoulderLineB = true;


        bool GrowingAngleLeftForearmLeftArmA = true;
        // bool GrowingAngleLeftForearmLeftArmB = true;


        bool GrowingAngleRightForearmRightArmA = true;
        // bool GrowingAngleRightForearmRightArmB = true;


        public int SwingAngleLeftArmShoulderLineA = 0;
        public int SwingAngleLeftArmShoulderLineB = 0;


        public int SwingAngleRightArmShoulderLineA = 0;
        public int SwingAngleRightArmShoulderLineB = 0;


        public int SwingAngleLeftForearmLeftArmA = 0;
        public int SwingAngleLeftForearmLeftArmB = 0;


        public int SwingAngleRightForearmRightArmA = 0;
        public int SwingAngleRightForearmRightArmB = 0;



        public double leftArmAngleChange = 0;
        public double rightArmAngleChange = 0;
        public double gestureSize = 0;

        double preAngLeftUpArm;
        double preAngRightUpArm;
        double preAngLeftLowArm;
        double preAngRightLowArm;

        public CalcArmsMovement2D()
        {

        }

        /// <summary>
        /// Call when after a gesture was found and you want to reset to start analysing when a new gesture happens
        /// </summary>
        public void resetMaxAndMin()
        {
            setPreviousAngles();
            currentGesture = Gesture.nogesture;

            MinAngleLeftArmShoulderLineA = Globals.angleftUpArm;
            // MinAngleLeftArmShoulderLineB = parent.bfpa.prevAngleLeftArmShoulderLineB;


            MinAngleRightArmShoulderLineA = Globals.angRightUpArm;
            // MinAngleRightArmShoulderLineB = parent.bfpa.angleRightArmShoulderLineB;


            MaxAngleLeftArmShoulderLineA = Globals.angleftUpArm;
            // MaxAngleLeftArmShoulderLineB = parent.bfpa.angleRightArmShoulderLineB;


            MaxAngleRightArmShoulderLineA = Globals.angRightUpArm;
            //  MaxAngleRightArmShoulderLineB = parent.bfpa.prevAngleLeftArmShoulderLineB;



            MinAngleLeftForearmLeftArmA = Globals.angLeftLowArm;
            // MinAngleLeftForearmLeftArmB = parent.bfpa.angleLeftForearmLeftArmB;


            MinAngleRightForearmRightArmA = Globals.angRightLowArm;
            // MinAngleRightForearmRightArmB = parent.bfpa.angleRightForearmRightArmB;


            MaxAngleLeftForearmLeftArmA = Globals.angLeftLowArm;
            //  MaxAngleLeftForearmLeftArmB = parent.bfpa.angleLeftForearmLeftArmB;


            MaxAngleRightForearmRightArmA = Globals.angRightLowArm;
            //  MaxAngleRightForearmRightArmB = parent.bfpa.angleRightForearmRightArmB;



            GrowingAngleLeftArmShoulderLineA = true;
            //  GrowingAngleLeftArmShoulderLineB = true;


            GrowingAngleRightArmShoulderLineA = true;
            // GrowingAngleRightArmShoulderLineB = true;


            GrowingAngleLeftForearmLeftArmA = true;
            // GrowingAngleLeftForearmLeftArmB = true;


            GrowingAngleRightForearmRightArmA = true;
            //  GrowingAngleRightForearmRightArmB = true;
            // GrowingAngleRightForearmRightArmC = true;

            SwingAngleLeftArmShoulderLineA = 0;
            SwingAngleLeftArmShoulderLineB = 0;
            // SwingAngleLeftArmShoulderLineC = 0;

            SwingAngleRightArmShoulderLineA = 0;
            SwingAngleRightArmShoulderLineB = 0;
            // SwingAngleRightArmShoulderLineC = 0;

            SwingAngleLeftForearmLeftArmA = 0;
            SwingAngleLeftForearmLeftArmB = 0;
            //  SwingAngleLeftForearmLeftArmC = 0;

            SwingAngleRightForearmRightArmA = 0;
            SwingAngleRightForearmRightArmB = 0;
            //  SwingAngleRightForearmRightArmC = 0;
        }

        void setPreviousAngles()
        {
            preAngLeftUpArm = Globals.angleftUpArm;

            preAngRightUpArm = Globals.angRightUpArm;

            preAngLeftLowArm = Globals.angLeftLowArm;

            preAngRightLowArm = Globals.angRightLowArm;

        }

        public void calcArmMovements()
        {

            getGrowingValues();
            setMaxMinValues();
            calcCurrentGestureSize();
            setPreviousAngles();

            prePreviousGesture = previousGesture;
            previousGesture = currentGesture;

        }

        void getGrowingValues()
        {
            bool growingVariable;

            /////////// left shoulder
            growingVariable = calcIsGrowing(Globals.angleftUpArm,
                preAngLeftUpArm);
            if (GrowingAngleLeftArmShoulderLineA != growingVariable)
            {
                GrowingAngleLeftArmShoulderLineA = growingVariable;
                if (MaxAngleLeftArmShoulderLineA - MinAngleLeftArmShoulderLineA > 10)
                {
                    SwingAngleLeftArmShoulderLineA++;
                }
                MaxAngleLeftArmShoulderLineA = Globals.angleftUpArm;
                MinAngleLeftArmShoulderLineA = Globals.angleftUpArm;
            }


            ///////////right shoulder
            growingVariable = calcIsGrowing(Globals.angRightUpArm,
                preAngRightUpArm);
            if (GrowingAngleRightArmShoulderLineA != growingVariable)
            {
                GrowingAngleRightArmShoulderLineA = growingVariable;
                if (MaxAngleRightArmShoulderLineA - MinAngleRightArmShoulderLineA > 10)
                {
                    SwingAngleRightArmShoulderLineA++;
                }
                MaxAngleRightArmShoulderLineA = Globals.angRightUpArm;
                MinAngleRightArmShoulderLineA = Globals.angRightUpArm;
            }


            ///////////left forearm
            growingVariable = calcIsGrowing(Globals.angLeftLowArm,
               preAngLeftLowArm);
            if (GrowingAngleLeftForearmLeftArmA != growingVariable)
            {
                GrowingAngleLeftForearmLeftArmA = growingVariable;
                if (MaxAngleLeftForearmLeftArmA - MinAngleLeftForearmLeftArmA > 10)
                {
                    SwingAngleLeftForearmLeftArmA++;
                }
                MaxAngleLeftForearmLeftArmA = Globals.angLeftLowArm;
                MinAngleLeftForearmLeftArmA = Globals.angLeftLowArm;
            }


            /////////// right forearm
            growingVariable = calcIsGrowing(Globals.angRightLowArm,
              preAngRightLowArm);
            if (GrowingAngleRightForearmRightArmA != growingVariable)
            {
                GrowingAngleRightForearmRightArmA = growingVariable;
                if (MaxAngleRightForearmRightArmA - MinAngleRightForearmRightArmA > 10)
                {
                    SwingAngleRightForearmRightArmA++;
                }
                MaxAngleRightForearmRightArmA = Globals.angRightLowArm;
                MinAngleRightForearmRightArmA = Globals.angRightLowArm;
            }


        }

        void setMaxMinValues()
        {
            //////// Left shoulder
            if (Globals.angleftUpArm < MinAngleLeftArmShoulderLineA)
            {

                MinAngleLeftArmShoulderLineA = Globals.angleftUpArm;
            }
            if (Globals.angleftUpArm > MaxAngleLeftArmShoulderLineA)
            {
                MaxAngleLeftArmShoulderLineA = Globals.angleftUpArm;
            }



            ////// Right Shoulder
            if (Globals.angRightUpArm < MinAngleRightArmShoulderLineA)
            {
                MinAngleRightArmShoulderLineA = Globals.angRightUpArm;
            }
            if (Globals.angRightUpArm > MaxAngleRightArmShoulderLineA)
            {
                MaxAngleRightArmShoulderLineA = Globals.angRightUpArm;
            }




            /////// LeftForearm
            if (Globals.angLeftLowArm < MinAngleLeftForearmLeftArmA)
            {
                MinAngleLeftForearmLeftArmA = Globals.angLeftLowArm;
            }
            if (Globals.angLeftLowArm > MaxAngleLeftForearmLeftArmA)
            {
                MaxAngleLeftForearmLeftArmA = Globals.angLeftLowArm;
            }



            ///////Right Forearm
            if (Globals.angRightLowArm < MinAngleRightForearmRightArmA)
            {
                MinAngleRightForearmRightArmA = Globals.angRightLowArm;
            }
            if (Globals.angRightLowArm > MaxAngleRightForearmRightArmA)
            {
                MaxAngleRightForearmRightArmA = Globals.angRightLowArm;
            }



        }



        void calcCurrentGestureSize()
        {



            ///leftArm
            leftArmAngleChange = Math.Abs(MaxAngleLeftArmShoulderLineA - MinAngleLeftArmShoulderLineA) +
                Math.Abs(MaxAngleLeftForearmLeftArmA - MinAngleLeftForearmLeftArmA);

            ///// Right Arm
            rightArmAngleChange = Math.Abs(MaxAngleRightArmShoulderLineA - MinAngleRightArmShoulderLineA) +
                Math.Abs(MaxAngleRightForearmRightArmA - MinAngleRightForearmRightArmA);

            if (rightArmAngleChange > leftArmAngleChange)
            {
                gestureSize = rightArmAngleChange;
            }
            else
            {
                gestureSize = leftArmAngleChange;
            }
            // gestureSize = rightArmAngleChange;


            if (gestureSize < 10)
            {
                currentGesture = Gesture.nogesture;
            }
            else if (gestureSize < 20)
            {
                currentGesture = Gesture.small;
            }
            else if (gestureSize < 30)
            {
                currentGesture = Gesture.medium;
            }
            else
            {
                currentGesture = Gesture.big;
            }

        }


        bool calcIsGrowing(double current, double previous)
        {
            bool isGrowing = true;
            if (current < previous)
            {
                isGrowing = false;
            }
            return isGrowing;
        }

    }
}
