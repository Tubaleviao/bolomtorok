from PIL import Image
import numpy as np
import math
import time
import glob
from pathlib import Path

for file in glob.glob('classes/**/*.png', recursive=True):
    print("reading file "+file)

    novice=Image.open(file)
    novice = novice.convert('RGBA')
    pixel = int(3)
    frames = []
    frames2d = []

    def passei(p):
        if str(p[0]) in positions:
            if str(p[1]) in positions[str(p[0])]:
                return True
            else:
                return False
        else:
            return False
    queroPassar = []

    def checkPixel(p):
        global top, left, right, bottom
        if str(p[0]) in positions:
            positions[str(p[0])][str(p[1])] = True
        else:
            positions[str(p[0])] = { str(p[1]): True }
        if p[0] < left:
            left = p[0]
        elif p[0] > right:
            right = p[0]
        elif p[1] < top:
            top = p[1]
        elif p[1] > bottom:
            bottom = p[1]
        for x in range(4):
            xeca(next[x](p))

    def xeca(p):
        if(not passei(p)):
            if abs(p[0]) < novice.size[0] and abs(p[1]) < novice.size[1]:
                if(novice.getpixel( (p[0], p[1]) )[3] > 0):
                    queroPassar.append( p )
                
    def esquerda(p):
        return [p[0]-1 , p[1]]
    def direita(p):
        return [p[0]+1 , p[1]]
    def acima(p):
        return [p[0] , p[1]-1]
    def abaixo(p):
        return [p[0], p[1]+1]

    next = {0: esquerda, 1: direita, 2: acima, 3: abaixo}
        
    def interact():
        global novice, pixel, top, left, right, bottom, maxh, maxw
        print(str(pixel[0])+"-"+str(pixel[1]), end=" ")
        xeca( pixel )
        while(len(queroPassar)>0):
            checkPixel(queroPassar.pop())
            
        frame = novice.crop((left, top, right, bottom))
        if(len(frames) > 0  
            # discarta nego grande
            and (frame.size[1] > frames[-1].size[1]+math.floor(frames[-1].size[1]/1.5)
            or frame.size[0] > frames[-1].size[0]+math.floor(frames[-1].size[0]/1.5)) ):
            print( "nego em "+file+" no pixel "+ str(pixel) + " descartado" )
            pixel = [ right+1, pixel[1] ]
        else:
            frames.append(frame)
            pixel = [ right+1, math.floor((bottom-top)/2 + top) ]
            if(maxh < frame.size[1]):
                maxh = frame.size[1]
            if(maxw < frame.size[0]):
                maxw = frame.size[0]
            
        # erase from image
        rgba = np.array(novice)
        rgba[top : bottom, left : right] = (0, 0, 0, 0)
        novice = Image.fromarray(rgba)

    def concat_h(ims):
        # all images need to have same height
        if(len(ims) == 0):
            return Image.new('RGBA', (100,100))
        dst = Image.new('RGBA', ( sum(map(lambda x: x.size[0], ims )), ims[0].size[1] ))
        for x in range(len(ims)):
            dst.paste( ims[x], (ims[x].size[0]*x, 0) )
        return dst

    def concat_v(ims):
        # all images need to have same height
        if(len(ims) == 0):
            return Image.new('RGBA', (100,100))
        dst = Image.new('RGBA', ( max(map(lambda x: x.size[0], ims )), sum(map(lambda x: x.size[1], ims )) ))
        for y in range(len(ims)):
            dst.paste( ims[y], ( 0, ims[y].size[1]*y ) )
        return dst

    def concat_tile(twodlist):
        return concat_v( [concat_h(hl) for hl in twodlist] )

    while pixel < min(novice.size) and novice.getpixel((pixel, pixel))[3] == 0: # transparent
        pixel += 1

    top = pixel
    left = pixel
    right = pixel
    bottom = pixel

    pixel = [ pixel, pixel ]

    positions = {}

    maxh = 0
    maxw = 0

    interact()

    while( pixel[1] < novice.size[1] ):
        # inside the image somehow
        if(pixel[0] >= novice.size[0]):
            # finished the line
            if(len(frames) > 0):
                frames2d.append(frames)
            frames = []
            # considerando 87 pixels cada frame
            pixel[1] = pixel[1] + 87 
            pixel[0] = 0
        else:
            if(novice.getpixel((pixel[0], pixel[1]))[3] == 0):
                # transparent
                pixel[0] += 1
            else:
                top = pixel[1]
                left = pixel[0]
                right = pixel[0]
                bottom = pixel[1]
                interact()

    # deixa todos os frames do mesmo tamanho
    margin = 8
    for f1d in range(len(frames2d)):
        for i in range(len(frames2d[f1d])):
            n = Image.new(mode="RGBA", size=(maxw + margin, maxh + margin), color=(0,0,0,0))
            pw = math.floor( (maxw - frames2d[f1d][i].size[0]) / 2 ) + margin
            ph = math.floor( (maxh - frames2d[f1d][i].size[1]) / 2 ) + margin
            n.paste( frames2d[f1d][i], ( pw, ph ) )
            frames2d[f1d][i] = n

    final = concat_tile( frames2d )
    Path(file).unlink()
    final.save(file) #  str(time.time())+ ".png" 

    print( file+" has " + str(sum([len(x) for x in frames2d]) ) + " frames" ) # 110
