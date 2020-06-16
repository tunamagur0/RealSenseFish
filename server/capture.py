import cv2
import texture_detector
import socket
import base64


class Capture:
    def __init__(self):
        self.detector = texture_detector.TextureDetector()
        pass

    def capture(self, device_num=0):
        cap = cv2.VideoCapture(device_num)
        if not cap.isOpened():
            return None

        while(True):
            ret, frame = cap.read()
            res = self.detector.detectAndPutRect(frame)
            cv2.imshow('video', res if res is not None else frame)
            key = cv2.waitKey(1) & 0xFF
            if res is not None and key == ord('c'):
                break

        return frame

    def communicate(self, host, port, device_num=0):
        cap = cv2.VideoCapture(device_num)
        if not cap.isOpened():
            return None

        while(True):
            ret, frame = cap.read()
            res = self.detector.detectAndPutRect(frame)
            cv2.imshow('video', res if res is not None else frame)
            key = cv2.waitKey(1) & 0xFF
            if res is not None and key == ord('c'):
                with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
                    s.connect((host, port))
                    cut = self.detector.detectAndCut(frame)
                    result, dst_data = cv2.imencode('.png', cut)
                    message = base64.b64encode(dst_data)
                    s.sendall(message)
                    print("ok")
            if key == ord('q'):
                break
