import numpy as np
import cv2
import matplotlib.pyplot as plt

data = np.fromfile('./d_sv0_1.raw', dtype=np.float32)
print(data.shape)
print(np.sum(data != 0.0))
data = data.reshape((720, 1080, 4))
# data = data[..., :3]
print(np.min(data), np.max(data))
# plt.imshow(data[::-1, ...])
# plt.show()
print(np.unique(data[..., 0]).shape)

img = cv2.imread('./rgb_sv0_1.png', cv2.IMREAD_UNCHANGED)
print(np.unique(img[..., 0]).shape)

plt.subplot(121)
plt.imshow(data[::-1, ...])
plt.subplot(122)
plt.imshow(img)
plt.show()