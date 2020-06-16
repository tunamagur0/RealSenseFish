import capture
import argparse

parser = argparse.ArgumentParser()

parser.add_argument('--host', help='hostname', default='localhost')
parser.add_argument('--port', help='port', default=9090, type=int)
parser.add_argument('--device', help='camera device num', default=0, type=int)

args = parser.parse_args()

if __name__ == "__main__":
    capture = capture.Capture()
    capture.communicate(args.host, args.port, args.device)
    # detector = texture_detector.TextureDetector()
    # image = capture.capture()
    # res = detector.detectAndCut(image)
    # if res is not None:
    #     plt.imshow(res)
    #     plt.show()
