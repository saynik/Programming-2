using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SpaceInvaders {
    class Menu {
        const int distance = 20;

        int height;
        int width;
        PictureBox canvas;
        string[] items;
        Item start;

        int activeItem; // aktivní položka menu
        int fontSize;

        public delegate void Item();

        PrivateFontCollection pfc;

        // vytvoření menu
        public Menu(PictureBox canvas, Item start, int fontSize = 30) {
            height = canvas.Height;
            width = canvas.Width;

            this.canvas = canvas;

            this.start = start;
            this.fontSize = fontSize;

            items = new string[] { "Start", "Quit" };

            // načteme písmo
            pfc = new PrivateFontCollection();

            byte[] fontBytes = Properties.Resources.FreePixel;
            var fontData = Marshal.AllocCoTaskMem(fontBytes.Length);
            Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);
            pfc.AddMemoryFont(fontData, fontBytes.Length);

            Draw();
        }

        // vykreslování menu
        public void Draw(bool thread = false) {
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            Font itemFont = new Font(pfc.Families[0], 50);
            Font activeFont = new Font(itemFont, FontStyle.Bold);
            Font bigFont = new Font(pfc.Families[0], 60);
            Font smallfont = new Font(pfc.Families[0], 16);
            Brush itemBrush = new SolidBrush(Color.White);
            Brush activeItemBrush = new SolidBrush(Color.Red);
            Brush headBrush = new SolidBrush(Color.Red);

            g.Clear(Color.Black);

            SizeF[] sizes = new SizeF[items.Length];

            float totalHeight = -distance;

            for (int i = 0; i < items.Length; i++) {
                sizes[i] = g.MeasureString(items[i], i == activeItem ? activeFont : itemFont);

                totalHeight += sizes[i].Height + distance;
            }

            float y = (height - totalHeight) / 2;

            for (int i = 0; i < items.Length; i++) {
                Brush brush = i == activeItem ? activeItemBrush : itemBrush;
                Font font = i == activeItem ? activeFont : itemFont;

                float x = (width - sizes[i].Width) / 2;

                g.DrawString(items[i], font, brush, x, y);

                y += sizes[i].Height + distance;
            }

            // instrukce
            string instructions = "↑ - nahoru, ↓ - dolů, enter - výběr";

            SizeF size = g.MeasureString(instructions, smallfont);
            g.DrawString(instructions, smallfont, itemBrush, width - 10 - size.Width, height - 10 - size.Height);

            size = g.MeasureString("Space invaders", bigFont);
            g.DrawString("Space invaders", bigFont, headBrush, (width - size.Width) / 2, 50);

            if (thread) {
                canvas.Invoke(new Action(() => {
                    canvas.Width = width;
                    canvas.Height = height;
                }));
            }
            else {
                canvas.Width = width;
                canvas.Height = height;
            }

            canvas.Image = bitmap;
        }

        // výběr položky menu
        public void KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Down) {
                activeItem = (activeItem + 1) % items.Length;
                Draw();
            }

            if (e.KeyCode == Keys.Up) {
                activeItem = (activeItem - 1 + items.Length) % items.Length;
                Draw();
            }

            if (e.KeyCode == Keys.Enter) {
                if (activeItem == 0) {
                    start();
                }
                else {
                    Application.Exit();
                }
            }
        }
    }
}
