using System.Drawing;
using System.Windows.Forms;

namespace TetrisReborn {
    /// <summary>
    /// </summary>
    public class Screen {
        protected Graphics G;
        protected Graphics GraphicsOffScreen;
        protected Image ImageOffScreen;
        public int ScreenHeight;
        public int ScreenWidth;

        public int ScreenX;
        public int ScreenY;

        public Screen(Panel p, Rectangle r) {
            G = p.CreateGraphics();
            ScreenX = r.X;
            ScreenY = r.Y;
            ScreenWidth = r.Width;
            ScreenHeight = r.Height;

            ImageOffScreen = new Bitmap(ScreenWidth, ScreenHeight);
            GraphicsOffScreen = Graphics.FromImage(ImageOffScreen);
        }

        public Screen() {
        }

        public Graphics GetGraphics() {
            return GraphicsOffScreen;
        }

        public void Erase() {
            if (!IsValidGraphics()) {
                return;
            }
            var blackBrush = new SolidBrush(Color.Black);
            GraphicsOffScreen.FillRectangle(blackBrush, 0, 0, ScreenWidth, ScreenHeight);
        }

        public void Flip() {
            G.DrawImage(ImageOffScreen, ScreenX, ScreenY);
        }

        public bool IsValidGraphics() {
            if (G != null && GraphicsOffScreen != null) {
                return true;
            }
            return false;
        }
    }
}