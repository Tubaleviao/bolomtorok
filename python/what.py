from PIL import Image, ImageFont, ImageDraw, ImageOps
import numpy as np

def find_coeffs(pa, pb):
    matrix = []
    for p1, p2 in zip(pa, pb):
        matrix.append([p1[0], p1[1], 1, 0, 0, 0, -p2[0]*p1[0], -p2[0]*p1[1]])
        matrix.append([0, 0, 0, p1[0], p1[1], 1, -p2[1]*p1[0], -p2[1]*p1[1]])

    A = np.matrix(matrix, dtype=np.float)
    B = np.array(pb).reshape(8)

    res = np.dot(np.linalg.inv(A.T * A) * A.T, B)
    return np.array(res).reshape(8)

def rand_degree(st,en,gap):
    return (np.fix(np.random.random()* (en-st) * gap )+st)

def img_transform(img):
    width, height = img.size
    print(img.size)
    m = -0.5
    xshift = abs(m) * width
    new_width = width + int(round(xshift))
    img = img.transform((new_width, height), Image.AFFINE,
            (1, m, -xshift if m > 0 else 0, 0, 1, 0), Image.BICUBIC)

    range_n = width*0.2
    gap_n = 1

    x1 = rand_degree(0,range_n,gap_n)
    y1 = rand_degree(0,range_n,gap_n)

    x2 = rand_degree(width-range_n,width,gap_n)
    y2 = rand_degree(0,range_n,gap_n)

    x3 = rand_degree(width-range_n,width,gap_n)
    y3 = rand_degree(height-range_n,height,gap_n)

    x4 = rand_degree(0,range_n,gap_n)
    y4 = rand_degree(height-range_n,height,gap_n)

    coeffs = find_coeffs(
             [(x1, y1), (x2, y2), (x3, y3), (x4, y4)],
            [(0, 0), (width, 0), (new_width, height), (xshift, height)])

    img = img.transform((width, height), Image.PERSPECTIVE, coeffs, Image.BICUBIC)
    return img

fontcolor = (255,255,255)
fontsize  = 180
# padding rate for setting the image size of font
fimg_padding = 1.1
# check code bbox padding rate
bbox_gap = fontsize * 0.05
# Rrotation +- N degree

# Choice a font type for output---
font = ImageFont.truetype('arial.ttf', fontsize)

# the text is "Hello World"
code = "Hello world"
# Get the related info of font---
code_w, code_h = font.getsize(code)

# Setting the image size of font---
img_size = int((code_w) * fimg_padding)

# Create a RGBA image with transparent background
img = Image.new("RGBA", (img_size,img_size),(255,255,255,0))
d = ImageDraw.Draw(img)

# draw white text
code_x = (img_size-code_w)/2
code_y = (img_size-code_h)/2
d.text( ( code_x, code_y ), code, fontcolor, font=font)
#img.save('initial.png')

# Transform the image---
#img = img_transform(img)

# crop image to the size equal to the bounding box of whole text
alpha = img.split()[-1]
left = alpha.getbbox()[0]
upper = alpha.getbbox()[1]
right = 0
lower = 0

print( alpha.getbbox(), (left, upper) )
print( img.getpixel((left+1, upper+1)) ) # (255, 255, 255, 255) = branco, (255, 255, 255, 0) = transparente
img = img.crop(alpha.getbbox())

# resize the image
img.thumbnail((512,512), Image.ANTIALIAS)

img.save('myimage.png')

# what I want is to find all the bounding box of each individual word
#boxes=find_all_bbx(img)
