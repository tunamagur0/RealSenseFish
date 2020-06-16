import cv2
import numpy as np

class TextureDetector:
    def __init__(self):
        pass

    def detect(self, image):
        """
        画像からテクスチャに当たる部分を認識し、認識位置を返す。

        Parameters
        ----------
        image : np.array
            入力画像。
            チャンネルはBGR。
            
        Returns
        -------
        detected_pos : np.array
            認識して得られた位置。
            失敗した場合はNone。
        """
        if len(image.shape) != 3:
            return None
        
        gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
        gray = cv2.GaussianBlur(gray, (11, 11), 0)
        th = cv2.adaptiveThreshold(gray, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY_INV, 11, 2)
        # _, th = cv2.threshold(gray, 0, 255, cv2.THRESH_BINARY_INV+cv2.THRESH_OTSU)
        
        # 領域抽出
        contours = cv2.findContours(th, cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)[0]
        th_area = gray.shape[0] * gray.shape[1] / 50
        contours_large = list(filter(lambda c:cv2.contourArea(c) > th_area, contours))
        approxes = []

        for cnt in contours_large:
            arclen = cv2.arcLength(cnt, True)
            approx = cv2.approxPolyDP(cnt, 0.02*arclen, True)
            if len(approx) != 4:
                continue
            approxes.append(approx)

        if len(approxes) < 1:
            return None

        return approxes[0]

    def detectAndPutRect(self, image):
        """
        画像からテクスチャに当たる部分を認識し、四角で囲んだもを返す。

        Parameters
        ----------
        image : np.array
            入力画像。
            チャンネルはBGR。
            
        Returns
        -------
        detected_image : np.array
            認識して得られた画像。
            失敗した場合はNone。
        """

        detected_pos = self.detect(image)
        if detected_pos is None:
            return None
        rectPos = self.getRectPos(detected_pos)
        detected_image = cv2.rectangle(image.copy(), tuple(rectPos[0][0]), tuple(rectPos[3][0]),
                                      (0, 255, 0), min(image.shape[0], image.shape[1]) // 50)
        return detected_image

    def compensatePosition(self, image, detected_pos):
        """
        画像からテクスチャに当たる部分を認識し、認識位置を補正して切り取った画像を返す。

        Parameters
        ----------
        image : np.array
            入力画像。
            チャンネルはBGR。

        detected_pos : np.array
            detectによって得られた認識位置。

        Returns
        -------
        detected_image : np.array
            認識位置を補正して切り取った画像。
            失敗した場合はNone。
        """
        # 台形補正
        rectPos = self.getRectPos(detected_pos)
        p_original = np.float32(rectPos)
        p_trans = np.float32([[0,0], [0,1024], [1024,0], [1024,1024]])

        M = cv2.getPerspectiveTransform(p_original, p_trans)
        detected_image = cv2.warpPerspective(image, M, (1024, 1024))
        return detected_image

    def getRectPos(self, detected_pos):
        """
        認識位置から各頂点の座標を返す。

        Parameters
        ----------
        detected_pos : np.array

        Returns
        -------
        rect_pos : np.array
            認識位置を展開して得られた各頂点の位置。
            [left_top, left_buttom, right_top, right_buttom]
        """
        sorted_by_x = sorted(detected_pos, key=lambda x: x[0][0])
        left_top, left_buttom = sorted(sorted_by_x[:2], key=lambda x: x[0][1])
        right_top, right_buttom = sorted(sorted_by_x[2:], key=lambda x: x[0][1])

        return np.array([left_top, left_buttom, right_top, right_buttom])

    def detectAndCut(self, image):
        """
        画像からテクスチャに当たる部分を認識し、切り取ったものを返す。

        Parameters
        ----------
        image : np.array
            入力画像。
            チャンネルはBGR。
            
        Returns
        -------
        detected_image : np.array
            認識して得られた画像。
            失敗した場合はNone。
        """
        detected_pos = self.detect(image)
        if detected_pos is None:
            return None
        detected_image = self.compensatePosition(image, detected_pos)
  
        return detected_image
