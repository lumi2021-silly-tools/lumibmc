# BoobmapConverter

This is a little program that I use to extract data from images as simple
bitmaps.

I usually use it with my OS to extract the image pixel data and render it
without need to parse a funky image format.

## BoobMap File Format:

The BoobmapConverter converts any image format in a boobmap (kindly named by a friend). A boobmap is a simple bitmap file with a header, nothing more, nothing less.

The standard extension for boobmap images is `.bm`.

Header structure:
| addr | size | description |
|:----:|:----:|:------------|
| 0x00 | 4 bytes | bytes per pixel |
| 0x04 | 4 bytes | image width |
| 0x08 | 4 bytes | image heigh |
| 0x0C | 4 bytes | not used |

All the header values are, for convention, in big endian.


The image data value can be acessed after the header, beguinning at `0x10`.

