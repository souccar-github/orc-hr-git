#region

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

#endregion

namespace UI.Areas.Personnel.Helpers
{
    public class ImageHandler
    {
        #region ColorFilterTypes enum

        public enum ColorFilterTypes
        {
            Red,
            Green,
            Blue
        } ;

        #endregion

        private string _bitmapPath;
        private Bitmap _bitmapPrevCropArea;
        private Bitmap _bitmapbeforeProcessing;
        private Bitmap _currentBitmap;

        public Bitmap CurrentBitmap
        {
            get { return _currentBitmap ?? (_currentBitmap = new Bitmap(1, 1)); }
            set { _currentBitmap = value; }
        }

        public Bitmap BitmapBeforeProcessing
        {
            get { return _bitmapbeforeProcessing; }
            set { _bitmapbeforeProcessing = value; }
        }

        public string BitmapPath
        {
            get { return _bitmapPath; }
            set { _bitmapPath = value; }
        }

        public void ResetBitmap()
        {
            if (_currentBitmap != null && _bitmapbeforeProcessing != null)
            {
                var bitmap = (Bitmap) _currentBitmap.Clone();
                _currentBitmap = (Bitmap) _bitmapbeforeProcessing.Clone();
                _bitmapbeforeProcessing = (Bitmap) bitmap.Clone();
            }
        }

        public void SaveBitmap(string saveFilePath, int saveFileName)
        {
            int last = saveFilePath.LastIndexOf('\\');
            int length = saveFilePath.Length - last;
            string fileName = saveFilePath.Substring(last + 1, length - 1);
            string filePath = saveFilePath.Substring(0, last);

            string newFilePath = filePath + "\\" + saveFileName + ".jpg";

            _bitmapPath = newFilePath;

            if (File.Exists(newFilePath))
                File.Delete(newFilePath);
            _currentBitmap.Save(newFilePath);
        }

        public void ClearImage()
        {
            _currentBitmap = new Bitmap(1, 1);
        }

        public void RestorePrevious()
        {
            _bitmapbeforeProcessing = _currentBitmap;
        }

        public void SetColorFilter(ColorFilterTypes colorFilterType)
        {
            Bitmap currentBitmap = _currentBitmap;
            Bitmap bitmap = (Bitmap) currentBitmap.Clone();
            Color c;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    c = bitmap.GetPixel(i, j);
                    int nPixelR = 0;
                    int nPixelG = 0;
                    int nPixelB = 0;
                    if (colorFilterType == ColorFilterTypes.Red)
                    {
                        nPixelR = c.R;
                        nPixelG = c.G - 255;
                        nPixelB = c.B - 255;
                    }
                    else if (colorFilterType == ColorFilterTypes.Green)
                    {
                        nPixelR = c.R - 255;
                        nPixelG = c.G;
                        nPixelB = c.B - 255;
                    }
                    else if (colorFilterType == ColorFilterTypes.Blue)
                    {
                        nPixelR = c.R - 255;
                        nPixelG = c.G - 255;
                        nPixelB = c.B;
                    }

                    nPixelR = Math.Max(nPixelR, 0);
                    nPixelR = Math.Min(255, nPixelR);

                    nPixelG = Math.Max(nPixelG, 0);
                    nPixelG = Math.Min(255, nPixelG);

                    nPixelB = Math.Max(nPixelB, 0);
                    nPixelB = Math.Min(255, nPixelB);

                    bitmap.SetPixel(i, j, Color.FromArgb((byte) nPixelR, (byte) nPixelG, (byte) nPixelB));
                }
            }
            _currentBitmap = (Bitmap) bitmap.Clone();
        }

        public void SetGamma(double red, double green, double blue)
        {
            Bitmap currentBitmap = _currentBitmap;
            Bitmap bitmap = (Bitmap) currentBitmap.Clone();
            Color c;

            byte[] redGamma = CreateGammaArray(red);
            byte[] greenGamma = CreateGammaArray(green);
            byte[] blueGamma = CreateGammaArray(blue);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    c = bitmap.GetPixel(i, j);
                    bitmap.SetPixel(i, j, Color.FromArgb(redGamma[c.R], greenGamma[c.G], blueGamma[c.B]));
                }
            }
            _currentBitmap = (Bitmap) bitmap.Clone();
        }

        private byte[] CreateGammaArray(double color)
        {
            byte[] gammaArray = new byte[256];

            for (int i = 0; i < 256; ++i)
            {
                gammaArray[i] = (byte) Math.Min(255, (int) ((255.0*Math.Pow(i/255.0, 1.0/color)) + 0.5));
            }
            return gammaArray;
        }

        public void SetBrightness(int brightness)
        {
            Bitmap currentBitmap = _currentBitmap;
            Bitmap bitmap = (Bitmap) currentBitmap.Clone();

            if (brightness < -255) brightness = -255;
            if (brightness > 255) brightness = 255;
            Color c;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    c = bitmap.GetPixel(i, j);
                    int cR = c.R + brightness;
                    int cG = c.G + brightness;
                    int cB = c.B + brightness;

                    if (cR < 0) cR = 1;
                    if (cR > 255) cR = 255;

                    if (cG < 0) cG = 1;
                    if (cG > 255) cG = 255;

                    if (cB < 0) cB = 1;
                    if (cB > 255) cB = 255;

                    bitmap.SetPixel(i, j, Color.FromArgb((byte) cR, (byte) cG, (byte) cB));
                }
            }
            _currentBitmap = (Bitmap) bitmap.Clone();
        }

        public void SetContrast(double contrast)
        {
            Bitmap currentBitmap = _currentBitmap;
            Bitmap bitmap = (Bitmap) currentBitmap.Clone();

            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0 + contrast)/100.0;
            contrast *= contrast;
            Color c;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    c = bitmap.GetPixel(i, j);
                    double pR = c.R/255.0;
                    pR -= 0.5;
                    pR *= contrast;
                    pR += 0.5;
                    pR *= 255;
                    if (pR < 0) pR = 0;
                    if (pR > 255) pR = 255;

                    double pG = c.G/255.0;
                    pG -= 0.5;
                    pG *= contrast;
                    pG += 0.5;
                    pG *= 255;
                    if (pG < 0) pG = 0;
                    if (pG > 255) pG = 255;

                    double pB = c.B/255.0;
                    pB -= 0.5;
                    pB *= contrast;
                    pB += 0.5;
                    pB *= 255;
                    if (pB < 0) pB = 0;
                    if (pB > 255) pB = 255;

                    bitmap.SetPixel(i, j, Color.FromArgb((byte) pR, (byte) pG, (byte) pB));
                }
            }
            _currentBitmap = (Bitmap) bitmap.Clone();
        }

        public void SetGrayscale()
        {
            Bitmap currentBitmap = _currentBitmap;
            Bitmap bitmap = (Bitmap) currentBitmap.Clone();
            Color c;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    c = bitmap.GetPixel(i, j);
                    byte gray = (byte) (.299*c.R + .587*c.G + .114*c.B);

                    bitmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            _currentBitmap = (Bitmap) bitmap.Clone();
        }

        public void SetInvert()
        {
            Bitmap currentBitmap = _currentBitmap;
            Bitmap bitmap = (Bitmap) currentBitmap.Clone();
            Color c;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    c = bitmap.GetPixel(i, j);
                    bitmap.SetPixel(i, j, Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
                }
            }
            _currentBitmap = (Bitmap) bitmap.Clone();
        }

        public void Resize(int newWidth, int newHeight)
        {
            if (newWidth != 0 && newHeight != 0)
            {
                Bitmap currentBitmap = _currentBitmap;
                Bitmap bitmap = new Bitmap(newWidth, newHeight, currentBitmap.PixelFormat);

                double nWidthFactor = currentBitmap.Width/(double) newWidth;
                double nHeightFactor = currentBitmap.Height/(double) newHeight;

                double fx, fy, nx, ny;
                int cx, cy, fr_x, fr_y;
                Color color1 = new Color();
                Color color2 = new Color();
                Color color3 = new Color();
                Color color4 = new Color();
                byte nRed, nGreen, nBlue;
                byte bp1, bp2;

                for (int x = 0; x < bitmap.Width; ++x)
                {
                    for (int y = 0; y < bitmap.Height; ++y)
                    {
                        fr_x = (int) Math.Floor(x*nWidthFactor);
                        fr_y = (int) Math.Floor(y*nHeightFactor);
                        cx = fr_x + 1;
                        if (cx >= currentBitmap.Width) cx = fr_x;
                        cy = fr_y + 1;
                        if (cy >= currentBitmap.Height) cy = fr_y;
                        fx = x*nWidthFactor - fr_x;
                        fy = y*nHeightFactor - fr_y;
                        nx = 1.0 - fx;
                        ny = 1.0 - fy;

                        color1 = currentBitmap.GetPixel(fr_x, fr_y);
                        color2 = currentBitmap.GetPixel(cx, fr_y);
                        color3 = currentBitmap.GetPixel(fr_x, cy);
                        color4 = currentBitmap.GetPixel(cx, cy);

                        // Blue
                        bp1 = (byte) (nx*color1.B + fx*color2.B);

                        bp2 = (byte) (nx*color3.B + fx*color4.B);

                        nBlue = (byte) (ny*(bp1) + fy*(bp2));

                        // Green
                        bp1 = (byte) (nx*color1.G + fx*color2.G);

                        bp2 = (byte) (nx*color3.G + fx*color4.G);

                        nGreen = (byte) (ny*(bp1) + fy*(bp2));

                        // Red
                        bp1 = (byte) (nx*color1.R + fx*color2.R);

                        bp2 = (byte) (nx*color3.R + fx*color4.R);

                        nRed = (byte) (ny*(bp1) + fy*(bp2));

                        bitmap.SetPixel(x, y, Color.FromArgb(255, nRed, nGreen, nBlue));
                    }
                }
                _currentBitmap = (Bitmap) bitmap.Clone();
            }
        }

        public void RotateFlip(RotateFlipType rotateFlipType)
        {
            Bitmap currentBitmap = _currentBitmap;
            Bitmap bitmap = (Bitmap) currentBitmap.Clone();
            bitmap.RotateFlip(rotateFlipType);
            _currentBitmap = (Bitmap) bitmap.Clone();
        }

        public void Crop(int xPosition, int yPosition, int width, int height)
        {
            Bitmap currentBitmap = _currentBitmap;
            Bitmap bitmap = (Bitmap) currentBitmap.Clone();

            if (xPosition + width > _currentBitmap.Width)
                width = _currentBitmap.Width - xPosition;
            if (yPosition + height > _currentBitmap.Height)
                height = _currentBitmap.Height - yPosition;

            var rectangle = new Rectangle(xPosition, yPosition, width, height);

            _currentBitmap = bitmap.Clone(rectangle, bitmap.PixelFormat);
        }

        public void DrawOutCropArea(int xPosition, int yPosition, int width, int height)
        {
            _bitmapPrevCropArea = _currentBitmap;

            Bitmap image = (Bitmap) _bitmapPrevCropArea.Clone();
            Graphics graphics = Graphics.FromImage(image);
            Brush cBrush = new Pen(Color.FromArgb(150, Color.White)).Brush;

            Rectangle rect1 = new Rectangle(0, 0, _currentBitmap.Width, yPosition);
            Rectangle rect2 = new Rectangle(0, yPosition, xPosition, height);
            Rectangle rect3 = new Rectangle(0, (yPosition + height), _currentBitmap.Width, _currentBitmap.Height);
            Rectangle rect4 = new Rectangle((xPosition + width), yPosition, (_currentBitmap.Width - xPosition - width),
                                            height);
            graphics.FillRectangle(cBrush, rect1);
            graphics.FillRectangle(cBrush, rect2);
            graphics.FillRectangle(cBrush, rect3);
            graphics.FillRectangle(cBrush, rect4);

            _currentBitmap = (Bitmap) image.Clone();
        }

        public void RemoveCropAreaDraw()
        {
            _currentBitmap = (Bitmap) _bitmapPrevCropArea.Clone();
        }

        public void InsertText(string text, int xPosition, int yPosition, string fontName, float fontSize,
                               string fontStyle, string colorName1, string colorName2)
        {
            Bitmap currentBitmap = _currentBitmap;
            Bitmap image = (Bitmap) currentBitmap.Clone();

            Graphics gr = Graphics.FromImage(image);

            if (string.IsNullOrEmpty(fontName))
                fontName = "Times New Roman";
            if (fontSize.Equals(null))
                fontSize = 10.0F;

            Font font = new Font(fontName, fontSize);
            if (!string.IsNullOrEmpty(fontStyle))
            {
                FontStyle fStyle = FontStyle.Regular;
                switch (fontStyle.ToLower())
                {
                    case "bold":
                        fStyle = FontStyle.Bold;
                        break;
                    case "italic":
                        fStyle = FontStyle.Italic;
                        break;
                    case "underline":
                        fStyle = FontStyle.Underline;
                        break;
                    case "strikeout":
                        fStyle = FontStyle.Strikeout;
                        break;
                }
                font = new Font(fontName, fontSize, fStyle);
            }
            if (string.IsNullOrEmpty(colorName1))
                colorName1 = "Black";
            if (string.IsNullOrEmpty(colorName2))
                colorName2 = colorName1;
            Color color1 = Color.FromName(colorName1);
            Color color2 = Color.FromName(colorName2);

            int gW = (int) (text.Length*fontSize);
            gW = gW == 0 ? 10 : gW;
            LinearGradientBrush LGBrush = new LinearGradientBrush(new Rectangle(0, 0, gW, (int) fontSize), color1,
                                                                  color2, LinearGradientMode.Vertical);
            gr.DrawString(text, font, LGBrush, xPosition, yPosition);
            _currentBitmap = (Bitmap) image.Clone();
        }

        public void InsertImage(string imagePath, int xPosition, int yPosition)
        {
            Bitmap currentBitmap = _currentBitmap;
            Bitmap image = (Bitmap) currentBitmap.Clone();

            Graphics graphics = Graphics.FromImage(image);

            if (!string.IsNullOrEmpty(imagePath))
            {
                Bitmap i_bitmap = (Bitmap) Image.FromFile(imagePath);
                Rectangle rect = new Rectangle(xPosition, yPosition, i_bitmap.Width, i_bitmap.Height);
                graphics.DrawImage(Image.FromFile(imagePath), rect);
            }
            _currentBitmap = (Bitmap) image.Clone();
        }

        public void InsertShape(string shapeType, int xPosition, int yPosition, int width, int height, string colorName)
        {
            Bitmap currentBitmap = _currentBitmap;
            Bitmap image = (Bitmap) currentBitmap.Clone();
            Graphics graphics = Graphics.FromImage(image);

            if (string.IsNullOrEmpty(colorName))
                colorName = "Black";
            Pen pen = new Pen(Color.FromName(colorName));

            switch (shapeType.ToLower())
            {
                case "filledellipse":
                    graphics.FillEllipse(pen.Brush, xPosition, yPosition, width, height);
                    break;
                case "filledrectangle":
                    graphics.FillRectangle(pen.Brush, xPosition, yPosition, width, height);
                    break;
                case "ellipse":
                    graphics.DrawEllipse(pen, xPosition, yPosition, width, height);
                    break;
                case "rectangle":
                default:
                    graphics.DrawRectangle(pen, xPosition, yPosition, width, height);
                    break;
            }
            _currentBitmap = (Bitmap) image.Clone();
        }
    }
}