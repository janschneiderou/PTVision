import numpy as np
import json
import cv2
import io
from PIL import Image
import socket
from ultralytics import YOLO
import mediapipe as mp

mp_pose = mp.solutions.pose

#application_path = os.path.dirnmae(sys.executable)
print('loading model')
model = YOLO('yolov8s-pose.pt')
print('model loaded')


def analyse(data):
    #results = model(source=1, show=True, conf=0.3, save=False)
    # Define path to the image file
    #image = applyImage(data)
    #source = 'test1.JPG'


    # As the URL above tells, its size is 512x512
    img = Image.open(io.BytesIO(data))
    #print(img.size)

    results = model.predict(img)
    print(model.info())

    #print("hello")

    #put it in JSON
    body = Body()
    for x in range(17):
        posi = results[0].keypoints[0].xy[0, x]
        key = keypoint()
        key.X = posi[0].item()
        key.Y = posi[1].item()
        body.keypoints.append(key)

    json_str = body.toJSON()

    #print("return value =" + json_str)
    return " " + json_str

def analyse2(data):
    with mp_pose.Pose(min_detection_confidence=0.5, min_tracking_confidence=0.5) as pose:
        #source = 'test1.JPG' #just for debug
        # Recolor image to RGB
        #output1 = cv2.imread(source) #just for debug

        #uncomment these
        image = np.asarray(bytearray(data), dtype="uint8")
        img = cv2.imdecode(image, cv2.IMREAD_COLOR)

        # As the URL above tells, its size is 512x512
        #img = Image.open(io.BytesIO(data))
        #image = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        #image.flags.writeable = False
        #output = cv2.imread(img)
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


def giveMeKeypoints():
    global result_keypoints
    return result_keypoints

def applyImage(data):
	decoded = np.frombuffer(data, dtype=np.uint8)
	decoded = decoded.reshape((1288, 757,3))
	return decoded;

# Define a helper function to receive all data
def recvall(sock, count):
    buf = b''
    while count:
        newbuf = sock.recv(count)
        if not newbuf: return None
        buf += newbuf
        count -= len(newbuf)
    return buf

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


class Position:
    def __init__(self):
        self.X = None
        self.Y = None


class keypoint:
    def __init__(self):
        self.X = None
        self.Y = None
    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__,
                          sort_keys=True, indent=4)


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

#analyse('xx')
serverSocket()
#analyse2('xx')

