using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTVision.utilObjects
{
    public enum YoloBodyParts
    {
        NOSE,
        LEFT_EYE,
        RIGHT_EYE,
        LEFT_EAR,
        RIGHT_EAR,
        LEFT_SHOULDER,
        RIGHT_SHOULDER,
        LEFT_ELBOW,
        RIGHT_ELBOW,
        LEFT_WRIST,
        RIGHT_WRIST,
        LEFT_HIP,
        RIGHT_HIP,
        LEFT_KNEE,
        RIGHT_KNEE,
        LEFT_ANKLE,
        RIGHT_ANKLE
    }

    public enum MediaBodyParts
    {
        NOSE,
        LEFT_EYE_INNER,
        LEFT_EYE,
        LEFT_EYE_OUTER,
        RIGHT_EYE_INNER,
        RIGHT_EYE,
        RIGHT_EYE_OUTER,
        LEFT_EAR,
        RIGHT_EAR,
        MOUTH_LEFT,
        MOUTH_RIGHT,
        LEFT_SHOULDER,
        RIGHT_SHOULDER,
        LEFT_ELBOW,
        RIGHT_ELBOW,
        LEFT_WRIST,
        RIGHT_WRIST,
        LEFT_PINKY,
        RIGHT_PINKY,
        LEFT_INDEX,
        RIGHT_INDEX,
        LEFT_THUMB,
        RIGHT_THUMB,
        LEFT_HIP,
        RIGHT_HIP,
        LEFT_KNEE,
        RIGHT_KNEE,
        LEFT_ANKLE,
        RIGHT_ANKLE,
        LEFT_HEEL,
        RIGHT_HEEL,
        LEFT_FOOT_INDEX,
        RIGHT_FOOT_INDEX
    }

    public class YoloKeypoint
    {
        public int bodyPart = -1;
        public float X;
        public float Y;


        public YoloKeypoint() { }
        public void reset()
        {
            bodyPart = -1;
            X = -1;
            Y = -1;

        }
    }

    public class MediaKeypoint
    {
        public int bodyPart = -1;
        public float X;
        public float Y;
        public float Z;
        public float visibility = 0.0f;

        public MediaKeypoint() { }
        public void reset()
        {
            bodyPart = -1;
            X = -1;
            Y = -1;
            Z = -1;

            visibility = 0.0f;
        }
    }

    public class MediaBody
    {
        public MediaKeypoint[] keypoints;

        public MediaBody()
        {
            keypoints = new MediaKeypoint[33];
        }
    }

    public class YoloBody
    {
        public YoloKeypoint[] keypoints;

        public YoloBody()
        {
            keypoints = new YoloKeypoint[17];
        }
    }


    public class BodyDescription
    {



    }
}
