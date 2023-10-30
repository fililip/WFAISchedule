﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFAISchedule
{
    public class ImageProcessing
    {
        public Image CropWhitespace(Image<Rgba32> sourceImage)
        {
            //find whitespace
            int threshold = 120;
            int scanHeight = sourceImage.Height / 2;
            int scanWidth = sourceImage.Width / 2;
            int xLhs = 0;
            while (xLhs < sourceImage.Width)
            {
                if (sourceImage[xLhs, scanHeight].R < threshold) break;
                xLhs++;
            }
            int xRhs = sourceImage.Width;
            while (xRhs > 0)
            {
                if (sourceImage[xRhs - 1, scanHeight].R < threshold) break;
                xRhs--;
            }
            int yLhs = 0;
            while (yLhs < sourceImage.Height)
            {
                if (sourceImage[scanHeight, yLhs].R < threshold) break;
                yLhs++;
            }
            int yRhs = sourceImage.Height;
            while (yRhs > 0)
            {
                if (sourceImage[scanHeight - 1, yRhs - 1].R < threshold) break;
                yRhs--;
            }
            //prepare crop dimensions
            Rectangle cropRectangle = new Rectangle(xLhs, yLhs, xRhs - xLhs, yRhs - yLhs);
            sourceImage.Mutate(x => x.Crop(cropRectangle));
            return sourceImage;
        }
        public Vector2 FindCellDimensions(Image<Rgba32> sourceImage)
        {
            Vector2 dimensions = new Vector2(0, 0);
            Vector2 startPosition = new Vector2(sourceImage.Width - 4, sourceImage.Height / 2);
            int y = startPosition.y;
            int dir = 1;
            int i = 0;
            int bottomEdge = 0;
            while(y < sourceImage.Height)
            {
                if(sourceImage[startPosition.x, y].R < 120)
                {
                    if (dir > 0)
                    {
                        dir = -1;
                        y = startPosition.y - 1;
                    }
                    else
                    {
                        bottomEdge = y;
                        break;
                    }
                }
                y += dir;
                i++;
            }
            dimensions.y = i;
            dir = 1;
            int x = startPosition.x;
            i = 0;
            while (x < sourceImage.Width)
            {
                if (sourceImage[x, bottomEdge + 1].R < 120)
                {
                    if (dir > 0)
                    {
                        dir = -1;
                        x = startPosition.x - 1;
                    }
                    else break;
                }
                x += dir;
                i++;
            }
            dimensions.x = i;
            return dimensions;
        }
    }
}
