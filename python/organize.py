from PIL import Image

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

positions = {}

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
    if novice.getpixel( (p[0], p[1]) )[3] > 0 :
        if(not passei(p)):
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

xeca(pixel)

while(len(queroPassar)>0):
    checkPixel(queroPassar.pop())

print(top, left, right, bottom)

novice = novice.crop((left, top, right, bottom))
novice.save("haahaha.png")
