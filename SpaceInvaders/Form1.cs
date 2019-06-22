using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SpaceInvaders {
    public partial class Form1 : Form {
        const int gameHeight = 300; // výška hracího pole
        const int gameWidth = 300; // šířka hracího pole

        const int alienRows = 5; // počet řádků vetřelcu
        const int alienCols = 11; // počet sloupců vetřelcu
        const int alienWidth = 11; // horizontální počet buněk pro vetřelci
        const int alienHeight = 8; // vertikální počet buněk pro vetřelci
        const int alienInitPosition = 25; // počáteční pozice vetřelcu vertikálně
        const int alienDstHorizontal = 7; // horizontální vzdálenost mezi vetřelci
        const int alienDstVertical = 8; // vertikální vzdálenost mezi vetřelci
        const int alienMoveBorder = 5; // hranice pohybu podél osy X

        const int bunkersCount = 4; // počet bunkru
        const int bunkerHeight = 17; // výška bunkru
        const int bunkerWidth = 22; // šířka bunkru
        const int bunkerDst = 18; // vzdálenost mezi bunkry
        const int bunkerPosition = gameHeight - bunkerHeight - 50; // pozice bunkrů

        const int gunWidth = 15; // šířka zbraně
        const int gunHeight = 11; // výška zbraně
        const int gunPosition = gameHeight - 40; // pozice zbraně

        const int ufoHeight = 7; // výška UFO
        const int ufoWidth = 16; // šířka UFO
        const int ufoPosition = 10; // poloha UFO
        const double ufoProbably = 0.005; // pravděpodobnost výskytu UFO

        const int alienShotSpeed = 3; // rychlost výstřelů vetřelcu
        const int shotSpeed = 5; // rychlost výstřelů

        readonly int[] alienScores = { 30, 25, 20, 15, 10 }; // počet bodů za vetřelce
        const int ufoScores = 100; // body za UFO

        const int x0 = 20; // posunutí ve směru osy X
        const int y0 = 50; // posunutí ve směru osy Y

        const int startLifes = 3; // počet životů

        const float startTime = 100; // čas začátku pohybu vetřelcu
        const float endTime = 2; // konečný čas pohybu vetřelcu
        const float deltaTime = (startTime - endTime) / (alienRows * alienCols);

        const int cellHeight = 2; // výška buňky
        const int cellWidth = 2; // šířka buňky

        Color background = Color.Black; // barva pozadí
        Color textColor = Color.White; // barva textu

        Color alienColor = Color.White; // vetřelici barva
        Color alienShotColor = Color.Red; // barva výstřelu vetřelcu
        Color alienHitColor = Color.Red; // barva zraněnych vetřelcu
        Color alienShotEndColor = Color.FromArgb(60, 255, 60);

        Color bunkerColor = Color.FromArgb(32, 255, 32); // barva bunkru

        Color gunColor = Color.FromArgb(32, 255, 32); // barva zbraně

        Color shotColor = Color.FromArgb(32, 255, 32); // barva výstřelu
        Color shotEndColor = Color.Red; // barva výstřelu pri dosažení okraje

        Color ufoColor = Color.OrangeRed; // barva UFO
        Color ufoHitColor = Color.Red; // barva zraněného UFO

        int height; // výška obrázku s polem
        int width; // šířka obrázku s polem

        Random random;
        Bitmap field;
        PrivateFontCollection pfc;
        Font baseFont;

        List<Keys> pressedKeys;

        System.Windows.Forms.Timer timer;
        System.Windows.Forms.Timer keyTimer;

        int[][,] aliensCells = {
            new int[alienHeight,alienWidth] {
                { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                { 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0 },
                { 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0 }
            },

            new int[alienHeight,alienWidth] {
                { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
                { 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                { 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1 },
                { 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0 }
            },

            new int[alienHeight,alienWidth] {
                { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
                { 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                { 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 }
            },

            new int[alienHeight,alienWidth] {
                { 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 1, 1, 0, 1, 0, 1, 1, 0, 0, 0 },
                { 0, 1, 1, 0, 1, 0, 1, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0 },
            },

            new int[alienHeight,alienWidth] {
                { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0 },
                { 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 0 },
                { 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0 },
            }
        };

        int[][,] aliensCells2 = {
            new int[alienHeight,alienWidth] {
                { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                { 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
                { 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0 }
            },

            new int[alienHeight,alienWidth] {
                { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1 },
                { 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
                {  0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 }
            },

            new int[alienHeight,alienWidth] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1 },
                { 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                { 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1 },
                { 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0 }
            },

            new int[alienHeight,alienWidth] {
                { 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 1, 1, 0, 1, 0, 1, 1, 0, 0, 0 },
                { 0, 1, 1, 0, 1, 0, 1, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
            },

            new int[alienHeight,alienWidth] {
                { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 0, 1, 0, 1, 1, 0, 0 },
                { 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1 }
            }
        };

        // bunkr
        int[,] bunkerCells = new int[bunkerHeight, bunkerWidth] {
            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },

            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },

            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 }
        };

        // zbraň
        int[,] gunCells = new int[gunHeight, gunWidth] {
            { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        // kulka
        int[,] shotCells = {
            { 1 },
            { 1 },
            { 1 },
        };

        int[,] shotEndCells = {
            { 1, 0, 0, 1, 0, 0, 1, 0 },
            { 1, 1, 1, 1, 0, 1, 0, 0 },
            { 0, 1, 0, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 0, 1, 1, 0, 1, 0 },
            { 0, 0, 1, 0, 1, 0, 1, 0 },
        };

        int[,] alienShotEndCells = {
            { 1, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 0 },
            { 1, 1, 1, 0, 1 },
            { 0, 0, 1, 1, 0 },
            { 1, 0, 1, 0, 1 },
        };

        // kulky vetřelcu
        int[,] alienShotCells = {
            { 0, 0, 1 },
            { 0, 1, 0 },
            { 1, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 1 },
            { 0, 1, 0 },
            { 1, 0, 0 },
        };

        // zásah do vetřelcu
        int[,] hitCells = {
            { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 },
            { 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0 },
            { 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 },
            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
            { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0 },
            { 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0 },
            { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 },
        };

        // UFO
        int[,] ufoCells = new int[ufoHeight, ufoWidth] {
            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
        };

        // rozbité UFO
        int[,] ufoHitCells = {
            { 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1 },
            { 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0 },
            { 0, 1, 1, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 1, 1, 1 },
            { 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0 },
            { 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 1 },
        };

        class Alien {
            public int index;
            public int x;
            public int y;
            public int shooted;
            public bool state;
            public int score;
            public Color color;
        }

        struct Bunker {
            public int[,] cells;
            public int x;
            public int y;
        }

        struct Gun {
            public int x;
            public int y;
            public int dx;
        }

        struct Shot {
            public int x;
            public int y;
            public int dy;
            public Color color;
            public int[,] cells;
            public int status;
        }

        class UFO {
            public int x;
            public int y;
            public int shooted;
        }

        Alien[][] aliens; // seznam vetřelcu
        Bunker[] bunkers; // seznam bunkru
        List<Shot> shots; // seznam výstřelu
        List<Shot> alienShots; // seznam výstřelu vetřelcu
        Gun gun; // laserová zbraň
        UFO ufo; // UFO

        bool isPaused;
        bool isQuit; // chceme jít ven?
        float gameTime;
        int lifes; // život
        int scores;
        int gunShooted; //  je-li byl tam zásah do zbraně

        Thread gameThread;
        Menu menu;

        public Form1() {
            InitializeComponent();

            menu = new Menu(Canvas, InitSpaceInvadersGame);
            KeyDown += menu.KeyDown;

            height = cellHeight * gameHeight + y0 + x0;
            width = cellWidth * gameWidth + 2 * x0;

            field = new Bitmap(width, height);

            random = new Random(DateTime.Now.Millisecond);

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Tick += Draw;

            keyTimer = new System.Windows.Forms.Timer();
            keyTimer.Interval = 50;
            keyTimer.Tick += KeyProcessing;

            // načteme písmo
            pfc = new PrivateFontCollection();

            byte[] fontBytes = Properties.Resources.FreePixel;
            var fontData = Marshal.AllocCoTaskMem(fontBytes.Length);
            Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);
            pfc.AddMemoryFont(fontData, fontBytes.Length);

            baseFont = new Font(pfc.Families[0], 25);

            pressedKeys = new List<Keys>();
        }

        void InitSpaceInvadersGame() {
            Init();

            timer.Start();
            keyTimer.Start();

            pressedKeys.Clear();

            KeyDown -= menu.KeyDown;
            KeyDown += GameKeyDown;
            KeyUp += GameKeyUp;

            gameThread = new Thread(() => {
                Game();

                KeyDown -= GameKeyDown;
                KeyUp -= GameKeyUp;
                KeyDown += menu.KeyDown;

                timer.Stop();
                keyTimer.Stop();

                menu.Draw(true);
            });

            gameThread.Start();
        }

        // vytváření vetřelcu
        void InitAliens() {
            aliens = new Alien[alienRows][];

            int w = alienWidth + alienDstHorizontal;
            int h = alienHeight + alienDstVertical;

            for (int i = 0; i < alienRows; i++) {
                aliens[i] = new Alien[alienCols];

                for (int j = 0; j < alienCols; j++) {
                    Alien alien = new Alien {
                        index = i,
                        score = alienScores[i],
                        color = alienColor,

                        x = (gameWidth - w * alienCols) / 2 + j * w,
                        y = alienInitPosition + i * h,
                        shooted = 0,
                        state = random.Next(2) == 1
                    };

                    aliens[i][j] = alien;
                }
            }
        }

        // vytváření bunkrů
        void InitBunkers() {
            bunkers = new Bunker[bunkersCount];

            int w = bunkerWidth + bunkerDst;

            for (int k = 0; k < bunkersCount; k++) {
                Bunker bunker = new Bunker {
                    x = k * w + (gameWidth - w * bunkersCount) / 2 + bunkerWidth / 2,
                    y = bunkerPosition,
                    cells = new int[bunkerCells.GetLength(0), bunkerCells.GetLength(1)]
                };

                for (int i = 0; i < bunkerCells.GetLength(0); i++)
                    for (int j = 0; j < bunkerCells.GetLength(1); j++)
                        bunker.cells[i, j] = bunkerCells[i, j];

                bunkers[k] = bunker;
            }
        }

        // vytváření zbraňe
        void InitGun() {
            gun = new Gun {
                x = gameWidth / 2,
                y = gunPosition,
                dx = 0
            };
        }

        // inicializace
        void Init() {
            InitAliens();
            InitBunkers();
            InitGun();

            shots = new List<Shot>();
            alienShots = new List<Shot>();

            gameTime = startTime;
            lifes = startLifes;
            scores = 0;

            isPaused = false;
            isQuit = false;
        }

        // kreslení buněk
        void DrawCells(Graphics g, Brush brush, int x, int y, int[,] cells) {
            for (int i = 0; i < cells.GetLength(0); i++) {
                for (int j = 0; j < cells.GetLength(1); j++) {
                    if (cells[i, j] == 0)
                        continue;

                    g.FillRectangle(brush, x0 + (x + j) * cellWidth, y0 + (y + i) * cellHeight, cellWidth, cellHeight);
                }
            }
        }

        // kreslení výstřelu
        void DrawShots(Graphics g, List<Shot> shots) {
            for (int i = 0; i < shots.Count; i++) {
                DrawCells(g, new SolidBrush(shots[i].color), shots[i].x, shots[i].y, shots[i].cells);
            }
        }

        // kreslení bunkru
        void DrawBunkers(Graphics g) {
            Brush bunkerBrush = new SolidBrush(bunkerColor);

            for (int i = 0; i < bunkersCount; i++)
                DrawCells(g, bunkerBrush, bunkers[i].x, bunkers[i].y, bunkers[i].cells);
        }

        // kreslení vetřelcu
        void DrawAliens(Graphics g) {
            Brush alienHitBrush = new SolidBrush(alienHitColor);

            for (int row = 0; row < alienRows; row++) {
                for (int col = 0; col < alienCols; col++) {
                    if (aliens[row][col] == null)
                        continue;

                    Alien alien = aliens[row][col];
                    Brush brush = alien.shooted > 0 ? alienHitBrush : new SolidBrush(alien.color);
                    int[,] cells = alien.shooted > 0 ? hitCells : alien.state ? aliensCells[alien.index] : aliensCells2[alien.index];

                    DrawCells(g, brush, alien.x, alien.y, cells);
                }
            }
        }

        // kreslení bodů
        void DrawScores(Graphics g) {
            Brush brush = new SolidBrush(textColor);

            g.DrawString(scores.ToString("D4"), baseFont, brush, x0 + 140, y0 - 38);
            g.DrawString(Properties.Settings.Default.spaceInvadersBest.ToString("D4"), baseFont, brush, x0 + 240, y0 - 38);
        }

        // kreslení zbraňe
        void DrawGun(Graphics g, int life_x = -1) {
            DrawCells(g, new SolidBrush(gunColor), (life_x == -1 ? gun.x : life_x) - 7, life_x == -1 ? gun.y : -15, gunCells);
        }

        // kreslení ufo
        void DrawUFO(Graphics g) {
            if (ufo == null)
                return;

            Brush ufoBrush = new SolidBrush(ufoColor);
            int[,] cells = ufo.shooted > 0 ? ufoHitCells : ufoCells;

            DrawCells(g, ufoBrush, ufo.x, ufo.y, cells);
        }

        // kreslení hry
        void Draw(object sender, EventArgs e) {
            Graphics g = Graphics.FromImage(field);
            Pen pen = new Pen(textColor);
            Brush brush = new SolidBrush(textColor);

            g.Clear(background);

            g.DrawRectangle(pen, x0, y0, gameWidth * cellWidth, gameHeight * cellHeight); // rámeček pole

            DrawBunkers(g); // kreslení bunkru
            DrawAliens(g); // kreslení vetřelcu
            DrawUFO(g); // kreslení ufo

            // kreslení životů
            for (int i = 0; i < lifes; i++)
                DrawGun(g, 20 * i + 7);

            // kreslení zbraňe
            if (gunShooted % 5 == 0)
                DrawGun(g);

            DrawShots(g, shots); // kreslení vlastních výstřelu
            DrawShots(g, alienShots); // kreslení vetřelcovych výstřelu

            DrawScores(g); // kreslení bodů

            if (isPaused) {
                g.DrawString("Pause", baseFont, brush, width - 110, y0 - 38);
            }

            try {
                Canvas.Image = field; // aktualizujeme obrazku
            }
            catch (Exception) { }

            if (isQuit)
                return;

            if (!isPaused) {
                UpdateState(); // aktualizujeme všechny prvky hry

                HitProcessing(); // zkontrolujeme zásahy
            }
        }

        // kreslení textu (písmenko za písmenkem)
        void DrawByLetter(Graphics g, Font font, Brush brush, string s, int y) {
            float w = 0;

            for (int i = 0; i < s.Length; i++)
                w += g.MeasureString(s[i].ToString(), font).Width;

            float x0 = (width - w) / 2;

            for (int i = 0; i < s.Length; i++) {
                SizeF size = g.MeasureString(s[i].ToString(), font);
                g.DrawString(s[i].ToString(), font, brush, x0, y);
                x0 += size.Width;

                Canvas.Image = field;
                Thread.Sleep(100);
            }
        }

        // kreslení "konec hry"
        void DrawGameOver(bool win) {
            int dw = (width - 100) / 11;
            int dh = (height - 210) / 8;

            Graphics g;

            while (true) {
                try {
                    g = Graphics.FromImage(field);
                    break;
                }
                catch (Exception) {

                }
            }

            Brush brush = win ? Brushes.White : Brushes.Red;
            Brush textBrush = new SolidBrush(textColor);

            Font smallFont = new Font(baseFont.FontFamily, 20);

            g.Clear(background);

            for (int i = 0; i < aliensCells[1].GetLength(0); i++) {
                for (int j = 0; j < aliensCells[1].GetLength(1); j++) {
                    if (aliensCells[1][i, j] == 0)
                        continue;

                    int x = 50 + j * dw;
                    int y = 20 + i * dh;

                    g.FillRectangle(brush, x, y, dw, dh);
                }
            }

            Canvas.Image = field;

            string s = win ? "You destroyed them!" : "Aliens captured the planet!";
            DrawByLetter(g, smallFont, textBrush, s, height - 180);
            DrawByLetter(g, baseFont, textBrush, "GAME OVER", height - 150);

            string s1 = "Your scores: " + scores;
            string s2 = "Best scores: " + Properties.Settings.Default.spaceInvadersBest;

            SizeF size1 = g.MeasureString(s1, baseFont);
            SizeF size2 = g.MeasureString(s2, baseFont);

            g.DrawString(s1, baseFont, textBrush, (width - size1.Width) / 2, height - 100);
            g.DrawString(s2, baseFont, textBrush, (width - size2.Width) / 2, height - 60);

            Canvas.Image = field;
            Thread.Sleep(1500);
        }

        // aktualizace výstřely
        void UpdateShots(List<Shot> shots) {
            for (int k = shots.Count - 1; k >= 0; k--) {
                Shot shot = shots[k];

                for (int j = 0; j < shot.cells.GetLength(1); j++) {
                    for (int i = 0; i < shot.cells.GetLength(0) - 1; i++)
                        shot.cells[i, j] = shot.cells[i + 1, j];
                }

                for (int j = 0; j < shot.cells.GetLength(1); j++) {
                    shot.cells[shot.cells.GetLength(0) - 1, j] = shot.cells[0, shot.cells.GetLength(1) - 1 - j];
                }

                if (shot.status > 0) {
                    shot.status--;

                    if (shot.status == 0)
                        shots.RemoveAt(k);
                    else
                        shots[k] = shot;

                    return;
                }

                // pokud výstřel dosáhl konce
                if (shot.y < 1) {
                    shot.status = 5;
                    shot.cells = shotEndCells;
                    shot.color = shotEndColor;
                    shot.x -= shotEndCells.GetLength(1) / 2;

                    shots[k] = shot;
                }
                else if (shot.y >= gameHeight - alienShotCells.GetLength(0)) { // pokud výstřel vetřelcu dosáhl konce
                    shot.status = 10;
                    shot.cells = alienShotEndCells;
                    shot.color = alienShotEndColor;
                    shot.x -= alienShotEndCells.GetLength(1) / 2;

                    shots[k] = shot;
                }
                else {
                    shot.y += shot.dy;

                    shots[k] = shot;
                }
            }
        }

        void UpdateState() {
            // aktualizujeme výstřely
            UpdateShots(shots);
            UpdateShots(alienShots);

            // aktualizujeme vetřelci
            for (int row = 0; row < alienRows; row++) {
                for (int col = 0; col < alienCols; col++) {
                    if (aliens[row][col] == null)
                        continue;

                    Alien alien = aliens[row][col];

                    if (alien.shooted > 0) {
                        alien.shooted--;

                        if (alien.shooted == 0)
                            alien = null;
                    }

                    aliens[row][col] = alien;
                }
            }

            // aktualizujeme zbraň
            if (gunShooted > 0)
                gunShooted--;

            gun.x += gun.dx;
            gun.x = Math.Max(gunWidth / 2, gun.x);
            gun.x = Math.Min(gameWidth - gunWidth / 2, gun.x);
            gun.dx = gun.dx - Math.Sign(gun.dx);

            // aktualizujeme ufo
            if (ufo != null) {
                if (ufo.shooted > 0) {
                    ufo.shooted--;

                    if (ufo.shooted == 0)
                        ufo = null;
                }
                else {
                    ufo.x++;

                    if (ufo.x >= gameWidth - ufoWidth)
                        ufo = null;
                }
            }
        }

        // vytvoření výstřelu
        Shot CreateShot(int x, int y, bool alien) {
            Shot shot = new Shot {
                x = alien ? x - 1 : x,
                y = y,
                color = alien ? alienShotColor : shotColor,
                dy = alien ? alienShotSpeed : -shotSpeed,
                status = 0
            };

            int[,] cells = alien ? alienShotCells : shotCells;
            shot.cells = new int[cells.GetLength(0), cells.GetLength(1)];

            for (int i = 0; i < cells.GetLength(0); i++)
                for (int j = 0; j < cells.GetLength(1); j++)
                    shot.cells[i, j] = cells[i, j];

            return shot;
        }

        // zasah do bunkru
        void ShotBunker(ref Bunker bunker, Shot shot) {
            int r = 3;

            for (int i0 = 0; i0 < shot.cells.GetLength(0); i0++) {
                for (int j0 = 0; j0 < shot.cells.GetLength(1); j0++) {
                    if (shot.cells[i0, j0] == 0)
                        continue;

                    for (int i = 0; i <= r; i++) {
                        for (int j = 0; j <= r; j++) {
                            int x = shot.x + j0 - bunker.x + j - r / 2;
                            int y = shot.y + i0 - bunker.y + i - r / 2;

                            if (x < 0 || x >= bunker.cells.GetLength(1) || y < 0 || y >= bunker.cells.GetLength(0))
                                continue;

                            if (random.NextDouble() > 0.4)
                                bunker.cells[y, x] = 0;
                        }
                    }
                }
            }
        }

        // kontrola křížení buněk
        bool CheckHit(int x1, int y1, int[,] cells1, int x2, int y2, int[,] cells2) {
            for (int i1 = 0; i1 < cells1.GetLength(0); i1++) {
                for (int j1 = 0; j1 < cells1.GetLength(1); j1++) {
                    if (cells1[i1, j1] == 0)
                        continue;

                    for (int i2 = 0; i2 < cells2.GetLength(0); i2++) {
                        for (int j2 = 0; j2 < cells2.GetLength(1); j2++) {
                            if (cells2[i2, j2] == 0)
                                continue;

                            if (x1 + j1 == x2 + j2 && y1 + i1 == y2 + i2)
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        // zpracování křížení výstřely
        void HitProcessing() {
            // zasah do vetřelce
            for (int i = alienRows - 1; i >= 0; i--) {
                for (int j = 0; j < alienCols; j++) {
                    Alien alien = aliens[i][j];

                    if (alien == null)
                        continue;

                    int[,] cells = alien.state ? aliensCells[alien.index] : aliensCells2[alien.index];

                    for (int k = 0; k < shots.Count; k++) {
                        if (CheckHit(shots[k].x, shots[k].y, shots[k].cells, alien.x, alien.y, cells)) {
                            alien.shooted = 10;
                            scores += alien.score;

                            gameTime -= deltaTime;

                            if (gameTime < endTime)
                                gameTime = endTime;

                            aliens[i][j] = alien;

                            shots.RemoveAt(k);
                            break;
                        }
                    }
                }
            }

            if (ufo != null) {
                for (int i = 0; i < shots.Count; i++) {
                    if (CheckHit(shots[i].x, shots[i].y, shots[i].cells, ufo.x, ufo.y, ufoCells)) {
                        scores += ufoScores; // body za zasah do UFO
                        ufo.shooted = 20;

                        shots.RemoveAt(i);
                        break;
                    }
                }
            }

            // zpracování zasáhu do bunkru
            for (int i = 0; i < bunkersCount; i++) {
                Bunker bunker = bunkers[i];

                for (int j = alienShots.Count - 1; j >= 0; j--) {
                    if (CheckHit(alienShots[j].x, alienShots[j].y, alienShots[j].cells, bunker.x, bunker.y, bunker.cells)) {
                        ShotBunker(ref bunker, alienShots[j]);
                        alienShots.RemoveAt(j);

                        bunkers[i] = bunker;
                    }
                }

                for (int j = shots.Count - 1; j >= 0; j--) {
                    if (CheckHit(shots[j].x, shots[j].y, shots[j].cells, bunker.x, bunker.y, bunker.cells)) {
                        ShotBunker(ref bunker, shots[j]);
                        shots.RemoveAt(j);

                        bunkers[i] = bunker;
                    }
                }
            }

            // zpracování zasahu dvou výstřelu
            for (int i = shots.Count - 1; i >= 0; i--) {
                for (int j = alienShots.Count - 1; j >= 0; j--) {
                    if (CheckHit(shots[i].x, shots[i].y, shots[i].cells, alienShots[j].x, alienShots[j].y, alienShots[j].cells)) {
                        shots.RemoveAt(i);
                        alienShots.RemoveAt(j);
                        break;
                    }
                }
            }

            // zpracování zasáhu do zbraňe
            for (int i = alienShots.Count - 1; i >= 0; i--) {
                if (CheckHit(alienShots[i].x, alienShots[i].y, alienShots[i].cells, gun.x - gunWidth / 2, gun.y, gunCells)) {
                    lifes--;
                    gunShooted = 20;
                    alienShots.RemoveAt(i);
                }
            }
        }

        // pohyb vetřelci
        bool AlienShift(ref int dx) {
            int xmin = gameWidth;
            int xmax = 0;
            int ymax = 0;

            for (int i = 0; i < alienRows; i++) {
                for (int j = 0; j < alienCols; j++) {
                    if (aliens[i][j] == null)
                        continue;

                    xmin = Math.Min(xmin, aliens[i][j].x);
                    xmax = Math.Max(xmax, aliens[i][j].x + alienWidth);
                    ymax = Math.Max(ymax, aliens[i][j].y + alienHeight);
                }
            }

            bool shift = false;

            if (xmax >= gameWidth - alienMoveBorder || xmin < alienMoveBorder) {
                dx = -dx;
                shift = true;
            }

            for (int i = 0; i < alienRows; i++) {
                for (int j = 0; j < alienCols; j++) {
                    if (aliens[i][j] == null)
                        continue;

                    aliens[i][j].x += dx;

                    if (shift)
                        aliens[i][j].y += alienDstVertical + alienHeight;

                    if (random.Next(2) == 1)
                        aliens[i][j].state = !aliens[i][j].state;
                }
            }

            return ymax < gun.y;
        }

        // výstřely vetřelci
        bool AlienShot() {
            List<Alien> shooting = new List<Alien>();

            for (int j = 0; j < alienCols; j++) {
                int i = alienRows - 1;

                while (i >= 0 && aliens[i][j] == null)
                    i--;

                if (i > -1)
                    shooting.Add(aliens[i][j]);
            }

            if (shooting.Count == 0)
                return false;

            int index = random.Next(shooting.Count);

            if (alienShots.Count == 0) {
                alienShots.Add(CreateShot(shooting[index].x + alienWidth / 2, shooting[index].y + alienHeight, true));
            }

            return true;
        }

        // zpracovani stisknutí klávesy
        void KeyProcessing(object sender, EventArgs e) {
            for (int i = 0; i < pressedKeys.Count; i++) {
                Keys key = pressedKeys[i];

                if (key == Keys.Left) {
                    gun.dx -= 3;

                    if (gun.dx < -9)
                        gun.dx = -9;
                }

                if (key == Keys.Right) {
                    gun.dx += 3;

                    if (gun.dx > 9)
                        gun.dx = 9;
                }

                if (key == Keys.Space && shots.Count == 0) {
                    shots.Add(CreateShot(gun.x, gun.y, false));
                }
            }
        }

        // zpracovani uvolněni klávesy
        public void GameKeyUp(object sender, KeyEventArgs e) {
            int index = pressedKeys.IndexOf(e.KeyCode);

            if (index > -1)
                pressedKeys.RemoveAt(index);
        }

        // zpracování stisknutí klávesy
        public void GameKeyDown(object sender, KeyEventArgs e) {
            if (gunShooted > 0)
                return;

            if (e.KeyCode == Keys.Q)
                isQuit = true;

            if (e.KeyCode == Keys.P)
                isPaused = !isPaused;

            if (isPaused)
                return;

            if (pressedKeys.IndexOf(e.KeyCode) == -1) {
                pressedKeys.Add(e.KeyCode);
            }
        }

        public void Game() {
            int dx = 1;
            bool win = false;

            while (lifes > 0) {
                if (isQuit) {
                    if (MessageBox.Show("Opravdu chcete jít ven?", "Jít ven", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        break;

                    isQuit = false;
                }

                if (isPaused || gunShooted > 0)
                    continue;

                if (ufo == null && random.NextDouble() < ufoProbably) {
                    ufo = new UFO {
                        x = 0,
                        y = ufoPosition,
                        shooted = 0
                    };
                }

                if (!AlienShift(ref dx)) {
                    win = false;
                    break;
                }

                if (!AlienShot()) {
                    win = true;
                    break;
                }

                Thread.Sleep((int)gameTime);
            }

            timer.Stop();
            timer.Dispose();

            if (isQuit)
                return;

            if (scores > Properties.Settings.Default.spaceInvadersBest) {
                Properties.Settings.Default.spaceInvadersBest = scores;
                Properties.Settings.Default.Save();
            }

            DrawGameOver(win);
        }
    }
}
