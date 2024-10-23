#import atexit
import numpy as np
import json
import cv2
import socket
import mediapipe as mp


print(mp.__file__)

print('loading model')
mp_pose = mp.solutions.pose
print('model loaded')

def analyse(data):
    with mp_pose.Pose(min_detection_confidence=0.5, min_tracking_confidence=0.5) as pose:

        #source = 'test1.JPG' #just for debug
        # Recolor image to RGB
        #output1 = cv2.imread(source) #just for debug

        #uncomment these
        image = np.asarray(bytearray(data), dtype="uint8")
        img = cv2.imdecode(image, cv2.IMREAD_COLOR)


        image_in_RGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        #image_in_RGB = cv2.cvtColor(output1, cv2.COLOR_BGR2RGB) #just for debug
        results = pose.process(image_in_RGB)


        # put it in JSON
        body = BodyMedia()
        try:
            for x in range(33):
                key = keypointMedia()
                key.X = results.pose_landmarks.landmark[x].x
                key.Y = results.pose_landmarks.landmark[x].y
                key.Z = results.pose_landmarks.landmark[x].z
                key.visibility = results.pose_landmarks.landmark[x].visibility
                body.keypoints.append(key)

            json_str = body.toJSON()
            print(json_str)
            #print(results.pose_landmarks)
            return(" " + json_str)
        except:
            return(" error")


def serverSocket():
    print('setting up host and port')
    HOST = "127.0.0.1"  # Standard loopback interface address (localhost)
    PORT = 65432  # Port to listen on (non-privileged ports are > 1023)
    print('host and port set')

    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        print('binding port and host')
        s.bind((HOST, PORT))
        print('binded')
        s.listen()
        conn, addr = s.accept()
        with conn:
            print(f"Connected by {addr}")
            while True:
                data = conn.recv(56578)
                if not data:
                    break
                result = analyse(data)
                str_1_encoded = bytes(result, 'ASCII')
                #print(len(str_1_encoded))
                conn.sendall(str_1_encoded)


class keypointMedia:
    def __init__(self):
        self.X = None
        self.Y = None
        self.Z = None
        self.visibility = None
    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__,
                          sort_keys=True, indent=4)


class Body:
    def __init__(self):
        self.keypoints = []
    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__,
                          sort_keys=True, indent=4)

class BodyMedia:
    def __init__(self):
        self.keypoints = []
    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__,
                          sort_keys=True, indent=4)


#def exit_handler():


#atexit.register(exit_handler)

analyse('xx')
#serverSocket()

