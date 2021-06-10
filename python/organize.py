from PIL import Image
import os, math, time
import glob

max_sprites_row = 3.0
frames = []
tile_width = 0
tile_height = 0
spritesheet_width = 0
spritesheet_height = 0

novice=Image.open("novice.png")

pixel = int(3)

while novice.getpixel((pixel, pixel))[3] == 0: # se for transparente
    pixel += 1

top = pixel
left = pixel
right = pixel
bottom = pixel

pixel = [pixel, pixel]

def checkPixel(p):
    if novice.getpixel((p[0], p[1]))[3] > 0 :# se não for transparente
        if p[0] < left:
            left = p[0]
        elif p[0] > right:
            right = p[0]
        elif p[1] < top:
            top = p[1]
        elif p[1] > bottom:
            bottom = p[1]
        checkPixel( [p[0], p[1]+1] )
        checkPixel( [p[0] , p[1]-1] )
        checkPixel( [p[0]+1 , p[1]] )
        checkPixel( [p[0]-1 , p[1]] )


novice = novice.crop((left, top, right, bottom))

novice.save("haahaha.png")
