using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TetrisReborn.Properties;

namespace TetrisReborn {
    public class Form1 : Form {
        public const int WmNclbuttondown = 0xA1;
        public const int HtCaption = 0x2;
        private PictureBox _closeBtn;

        public Form1() {
            InitializeComponent();

            SetUpGame();

            Retrofont.AddFontFile(@"Retro.ttf");
            _aBtnClicked = false;
            var timer = new Timer {Interval = 300};
            timer.Tick += ClockTick;
            timer.Enabled = true;

            _gameTimer.Enabled = true;
            _gameTimer.Interval = _gameSpeed;
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void ClockTick(object sender, EventArgs e) {
            _aBtnClicked = false;
        }

        private void LayoutForm_KeyDown(object sender, KeyEventArgs e) {
            var strKeyPress = e.KeyCode.ToString();
            if (!_isGameOver) {
                switch (strKeyPress.ToUpper()) {
                    case "A":
                        if (_aShape.ShapeMoving) {
                            _rectangleShape = _aShape.MoveShapeLeft(10, _gameGrid.GetGameGrid());
                        }
                        break;
                    case "D":
                        if (_aShape.ShapeMoving) {
                            _rectangleShape = _aShape.MoveShapeRight(10, _gameGrid.GetGameGrid());
                        }
                        break;
                    case "W":
                        if (_aShape.ShapeMoving) {
                            _rectangleShape = _aShape.FlipShape("right", _gameGrid.GetGameGrid());
                        }
                        break;
                    case "Q":
                        _gameTimer.Stop();
                        SetUpGame();
                        DrawStart();
                        break;
                    case "X":
                        Close();
                        break;
                    case "ESCAPE":
                        if (_isGamePaused) {
                            _gameTimer.Start();
                            _isGamePaused = false;
                        }
                        else {
                            _gameTimer.Stop();
                            _isGamePaused = true;
                            var gpause = _startScreen.GetGraphics();
                            _startScreen.Erase();

                            for (var i = 0; i < _numberOfRows; i++) {
                                for (var k = 0; k < _numberOfCols; k++) {
                                    gpause.DrawRectangle(new Pen(Color.White, 0.01f), _rectangleGameGrid[i][k]);
                                }
                            }

                            gpause.FillRectangle(new SolidBrush(Color.Black), 31, 103, 105, 28);
                            gpause.DrawRectangle(new Pen(Color.White, 0.01f), 31, 103, 105, 28);
                            gpause.DrawString("Pause", new Font(Retrofont.Families[0], 18), new SolidBrush(Color.White),
                                30, 100);

                            _startScreen.Flip();
                        }
                        break;
                    case "R":
                        DrawGameOver();
                        break;
                    case "SPACE":
                        if (_aShape.ShapeMoving) {
                            _gameTimer.Interval = _dropRate;
                            IsDropped = true;
                        }
                        break;
                }
            }
            else {
                if (strKeyPress.ToUpper() == "SPACE") {
                    SetUpGame();
                    _shapeType = GetShapeType();
                    _aShape = new Shape(_shapeType, _mainScreen.ScreenWidth, _mainScreen.ScreenHeight, false);
                    _nextShapeType = GetShapeType();

                    _aShape.ShapeMoving = true;
                    _isGameOver = false;
                    _gameTimer.Interval = _gameSpeed;
                    _gameTimer.Enabled = true;
                    _gameTimer.Start();
                }
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof (Form1));
            this._gameTimer = new System.Windows.Forms.Timer(this.components);
            this._lblScore = new System.Windows.Forms.Label();
            this._label1 = new System.Windows.Forms.Label();
            this._label3 = new System.Windows.Forms.Label();
            this._lblRows = new System.Windows.Forms.Label();
            this._drawingAreaCanvas = new System.Windows.Forms.Panel();
            this._label14 = new System.Windows.Forms.Label();
            this._menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this._menuNewGame = new System.Windows.Forms.ToolStripMenuItem();
            this._menuResetGrid = new System.Windows.Forms.ToolStripMenuItem();
            this._menuControls = new System.Windows.Forms.ToolStripMenuItem();
            this._menuStrip1 = new System.Windows.Forms.MenuStrip();
            this._titleBar = new System.Windows.Forms.Panel();
            this._closeBtn = new System.Windows.Forms.PictureBox();
            this._menuStrip1.SuspendLayout();
            this._titleBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this._closeBtn)).BeginInit();
            this.SuspendLayout();
            // 
            // _gameTimer
            // 
            this._gameTimer.Tick += new System.EventHandler(this.GameTimer_Tick);
            // 
            // _lblScore
            // 
            this._lblScore.AutoSize = true;
            this._lblScore.BackColor = System.Drawing.SystemColors.MenuText;
            this._lblScore.ForeColor = System.Drawing.SystemColors.Control;
            this._lblScore.Location = new System.Drawing.Point(50, 60);
            this._lblScore.Name = "_lblScore";
            this._lblScore.Size = new System.Drawing.Size(13, 13);
            this._lblScore.TabIndex = 3;
            this._lblScore.Text = "0";
            // 
            // _label1
            // 
            this._label1.AutoSize = true;
            this._label1.BackColor = System.Drawing.SystemColors.MenuText;
            this._label1.ForeColor = System.Drawing.SystemColors.Control;
            this._label1.Location = new System.Drawing.Point(9, 60);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(39, 13);
            this._label1.TabIndex = 2;
            this._label1.Text = "Points:";
            // 
            // _label3
            // 
            this._label3.AutoSize = true;
            this._label3.BackColor = System.Drawing.SystemColors.MenuText;
            this._label3.ForeColor = System.Drawing.SystemColors.Control;
            this._label3.Location = new System.Drawing.Point(9, 79);
            this._label3.Name = "_label3";
            this._label3.Size = new System.Drawing.Size(35, 13);
            this._label3.TabIndex = 5;
            this._label3.Text = "Lines:";
            // 
            // _lblRows
            // 
            this._lblRows.AutoSize = true;
            this._lblRows.BackColor = System.Drawing.SystemColors.MenuText;
            this._lblRows.ForeColor = System.Drawing.SystemColors.Control;
            this._lblRows.Location = new System.Drawing.Point(50, 79);
            this._lblRows.Name = "_lblRows";
            this._lblRows.Size = new System.Drawing.Size(13, 13);
            this._lblRows.TabIndex = 6;
            this._lblRows.Text = "0";
            // 
            // _drawingAreaCanvas
            // 
            this._drawingAreaCanvas.BackColor = System.Drawing.SystemColors.MenuText;
            this._drawingAreaCanvas.Location = new System.Drawing.Point(12, 106);
            this._drawingAreaCanvas.Name = "_drawingAreaCanvas";
            this._drawingAreaCanvas.Size = new System.Drawing.Size(168, 254);
            this._drawingAreaCanvas.TabIndex = 0;
            // 
            // _label14
            // 
            this._label14.AutoSize = true;
            this._label14.BackColor = System.Drawing.SystemColors.MenuText;
            this._label14.ForeColor = System.Drawing.SystemColors.Control;
            this._label14.Location = new System.Drawing.Point(9, 366);
            this._label14.Name = "_label14";
            this._label14.Size = new System.Drawing.Size(63, 13);
            this._label14.TabIndex = 9;
            this._label14.Text = "Version: 2.0";
            // 
            // _menuFile
            // 
            this._menuFile.BackColor = System.Drawing.SystemColors.MenuText;
            this._menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this._menuNewGame,
                this._menuResetGrid
            });
            this._menuFile.ForeColor = System.Drawing.SystemColors.Control;
            this._menuFile.Name = "_menuFile";
            this._menuFile.Size = new System.Drawing.Size(51, 20);
            this._menuFile.Text = "Menu";
            // 
            // _menuNewGame
            // 
            this._menuNewGame.BackColor = System.Drawing.SystemColors.MenuText;
            this._menuNewGame.ForeColor = System.Drawing.SystemColors.Control;
            this._menuNewGame.Name = "_menuNewGame";
            this._menuNewGame.Size = new System.Drawing.Size(136, 22);
            this._menuNewGame.Text = "New Game";
            this._menuNewGame.Click += new System.EventHandler(this.menuNewGame_Click);
            // 
            // _menuResetGrid
            // 
            this._menuResetGrid.BackColor = System.Drawing.SystemColors.MenuText;
            this._menuResetGrid.ForeColor = System.Drawing.SystemColors.Control;
            this._menuResetGrid.Name = "_menuResetGrid";
            this._menuResetGrid.Size = new System.Drawing.Size(136, 22);
            this._menuResetGrid.Text = "Restart";
            this._menuResetGrid.Click += new System.EventHandler(this.menuResetGrid_Click);
            // 
            // _menuControls
            // 
            this._menuControls.ForeColor = System.Drawing.SystemColors.Control;
            this._menuControls.Name = "_menuControls";
            this._menuControls.Size = new System.Drawing.Size(64, 20);
            this._menuControls.Text = "Controls";
            this._menuControls.Click += new System.EventHandler(this.MenuControls_click);
            // 
            // _menuStrip1
            // 
            this._menuStrip1.BackColor = System.Drawing.SystemColors.MenuText;
            this._menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this._menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this._menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this._menuFile,
                this._menuControls
            });
            this._menuStrip1.Location = new System.Drawing.Point(0, 25);
            this._menuStrip1.Name = "_menuStrip1";
            this._menuStrip1.Size = new System.Drawing.Size(123, 24);
            this._menuStrip1.TabIndex = 8;
            this._menuStrip1.Text = "_menuStrip1";
            // 
            // _titleBar
            // 
            this._titleBar.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))),
                ((int) (((byte) (64)))));
            this._titleBar.Controls.Add(this._closeBtn);
            this._titleBar.Location = new System.Drawing.Point(-9, -9);
            this._titleBar.Name = "_titleBar";
            this._titleBar.Size = new System.Drawing.Size(209, 31);
            this._titleBar.TabIndex = 10;
            this._titleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this._titleBar_MouseDown);
            // 
            // _closeBtn
            // 
            this._closeBtn.Image = ((System.Drawing.Image) (resources.GetObject("_closeBtn.Image")));
            this._closeBtn.Location = new System.Drawing.Point(182, 11);
            this._closeBtn.Name = "_closeBtn";
            this._closeBtn.Size = new System.Drawing.Size(17, 17);
            this._closeBtn.TabIndex = 11;
            this._closeBtn.TabStop = false;
            this._closeBtn.Click += new System.EventHandler(this._closeBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.MenuText;
            this.ClientSize = new System.Drawing.Size(192, 382);
            this.Controls.Add(this._titleBar);
            this.Controls.Add(this._label14);
            this.Controls.Add(this._lblRows);
            this.Controls.Add(this._label3);
            this.Controls.Add(this._lblScore);
            this.Controls.Add(this._label1);
            this.Controls.Add(this._drawingAreaCanvas);
            this.Controls.Add(this._menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this._menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tetris";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.LayoutForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LayoutForm_KeyDown);
            this._menuStrip1.ResumeLayout(false);
            this._menuStrip1.PerformLayout();
            this._titleBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this._closeBtn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private void DrawScreen() {
            var g = _mainScreen.GetGraphics();
            _gameGridBrushes = _gameGrid.GetGameGridBrushes();
            _theBrushColors = _gameGrid.GetShapeColors();
            _rectangleGameGrid = _gameGrid.GetGameGrid();
            _mainScreen.Erase();

            for (var i = 3; i <= _mainScreen.ScreenWidth/16*16; i += _mainScreen.ScreenWidth/16) {
                for (var j = 0; j <= _mainScreen.ScreenHeight/25*24; j += _mainScreen.ScreenHeight/25) {
                    g.DrawRectangle(new Pen(Color.FromArgb(255, 121, 121, 121), 1), i, j, _mainScreen.ScreenWidth/16,
                        _mainScreen.ScreenHeight/25);
                }
            }

            for (var i = 0; i < _numberOfRows; i++) {
                for (var k = 0; k < _numberOfCols; k++) {
                    if (!_gameGrid.IsGridLocationEmpty(i, k)) {
                        g.FillRectangle(_gameGridBrushes[i][k], _rectangleGameGrid[i][k]);
                        g.DrawRectangle(new Pen(Color.FromArgb(255, 0, 0, 0), 1), _rectangleGameGrid[i][k]);
                    }
                }
            }

            foreach (var t in _rectangleShape) {
                g.FillRectangle(_theBrushColors[_shapeType - 1], t);
                g.DrawRectangle(new Pen(Color.White, 1), t);
            }

            _mainScreen.Flip();
            BonusStep--;
        }

        private void LayoutForm_Paint(object sender, PaintEventArgs e) {
            DrawStart();
        }

        private void GameTimer_Tick(object sender, EventArgs e) {
            if (_aBtnClicked) {
            }
            if (_startBtnPressed) {
                SetUpGame();

                _shapeType = GetShapeType();
                _aShape = new Shape(_shapeType, _mainScreen.ScreenWidth, _mainScreen.ScreenHeight, false);
                // _nextShapeType = GetShapeType();
                _nextShapeType = 1;

                _aShape.ShapeMoving = true;
                _isGameOver = false;
                _gameTimer.Interval = _gameSpeed;
                _gameTimer.Enabled = true;
                _gameTimer.Start();
                _startBtnPressed = false;
                return;
            }
            if (_aShape != null) {
                if (_aShape.ShapeMoving) {
                    if (_dropBtnPressed) {
                        _gameTimer.Interval = _dropRate;
                        _dropBtnPressed = false;
                    }
                    _rectangleShape = _aShape.MoveShapeDown(_dropRate, _gameGrid.GetGameGrid());
                    DrawScreen();
                }

                else {
                    int xCoordinate;
                    int yCoordinate;

                    for (var i = 0; i < 4; i++) {
                        if (_rectangle.Contains(_rectangleShape[i])) {
                            continue;
                        }
                        _isGameOver = true;
                        break;
                    }
                    if (!_isGameOver) {
                        var intYCoordinates = new int[4];

                        for (var i = 0; i < 4; i++) {
                            xCoordinate = _rectangleShape[i].X;
                            yCoordinate = _rectangleShape[i].Y;
                            intYCoordinates[i] = yCoordinate/10;

                            _gameGrid.SetShapeLocation(yCoordinate/10, xCoordinate/10, _rectangleShape[i], _shapeType);
                        }

                        Array.Sort(intYCoordinates);
                        for (var i = 0; i < 4; i++) {
                            _isRowFull = true;

                            for (var j = 0; j < _numberOfCols; j++) {
                                if (_gameGrid.IsGridLocationEmpty(intYCoordinates[i], j)) {
                                    _isRowFull = false;
                                    break;
                                }
                            }
                            if (_isRowFull) {
                                for (var k = intYCoordinates[i]; k > 0; k--) {
                                    for (var l = 0; l < _numberOfCols; l++) {
                                        _gameGrid.DropRowsDown(k, l);
                                    }
                                }

                                _gameGrid.SetTopRow();

                                UpdateScore(intYCoordinates[i]);
                                _bonusHeight = _drawingAreaCanvas.Height - 1;
                            }
                        }
                        _shapeType = _nextShapeType;
                        _aShape = new Shape(_shapeType, _mainScreen.ScreenWidth, _mainScreen.ScreenHeight, false);
                        _nextShapeType = GetShapeType();

                        _aShape.ShapeMoving = true;

                        _gameTimer.Interval = _gameSpeed;
                        IsDropped = false;
                    }
                    else {
                        _gameTimer.Stop();

                        DrawGameOver();
                    }
                }
            }
        }

        private void DrawStart() {
            _startScreen.GetGraphics();
            _startScreen.Erase();

            _startScreen.Flip();
        }

        private void UpdateScore(int intRowNum) {
            _levelRowsCompleted++;
            _totalRowsCompleted++;
            var reverseRow = 30 - intRowNum;
            _score += reverseRow*_levelRowsCompleted*_level + _bonusHeight*_level*_levelRowsCompleted;
            _lblScore.Text = _score.ToString();
            if (_levelRowsCompleted == 10) {
                UpdateLevel();
                _levelRowsCompleted = 0;
            }
            _lblRows.Text = _totalRowsCompleted.ToString();
        }

        private void UpdateLevel() {
            _level++;
            _gameSpeed -= 20;
        }

        private void SetUpGame() {

            _gameSpeed = 100;
            _bonusHeight = _drawingAreaCanvas.Height - 1;
            _dropRate = 5;
            _score = 0;
            _level = 10;
            _levelRowsCompleted = 0;
            _totalRowsCompleted = 0;
            _isGameOver = true;
            IsDropped = false;

            _lblScore.Text = _score.ToString();
            _lblRows.Text = _totalRowsCompleted.ToString();

            _rectangle = new Rectangle(0, 0, _drawingAreaCanvas.Width, _drawingAreaCanvas.Height);
            _mainScreen = new Screen(_drawingAreaCanvas, _rectangle);

            _rectangle = new Rectangle(0, 0, _drawingAreaCanvas.Width, _drawingAreaCanvas.Height);
            _startScreen = new Screen(_drawingAreaCanvas, _rectangle);

            _numberOfRows = (_drawingAreaCanvas.Height - 1)/10;
            _numberOfCols = (_drawingAreaCanvas.Width - 1)/10;
            _gameGrid = new GameGrid(_numberOfRows, _numberOfCols);
        }

        private void DrawGameOver() {
            var gOver = _startScreen.GetGraphics();
            _startScreen.Erase();
            gOver.DrawString("GAME\nOVER", new Font(Retrofont.Families[0], 22), new SolidBrush(Color.White), 28, 80);
            _startScreen.Flip();
        }

        private int GetShapeType() {
            int shapeType;
            do {
                shapeType = _randomShapeType.Next(6);
            } while (shapeType == 0);
            return shapeType;
            //return 5;
        }

        private void menuNewGame_Click(object sender, EventArgs e) {
            SetUpGame();
            _shapeType = GetShapeType();
            _aShape = new Shape(_shapeType, _mainScreen.ScreenWidth, _mainScreen.ScreenHeight, false);
            _nextShapeType = GetShapeType();

            _aShape.ShapeMoving = true;
            _isGameOver = false;
            _gameTimer.Interval = _gameSpeed;
            _gameTimer.Enabled = true;
            _gameTimer.Start();
        }

        private void menuResetGrid_Click(object sender, EventArgs e) {
            _gameTimer.Stop();
            SetUpGame();
            DrawStart();
        }

        private void MenuControls_click(object sender, EventArgs e) {
            MessageBox.Show(Resources.controlText);
        }

        private void _titleBar_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
            }
        }

        private void _closeBtn_Click(object sender, EventArgs e) {
            Close();
        }

        #region variables

        private bool _aBtnClicked;
        private Shape _aShape;
        private int _bonusHeight;
        private IContainer components;
        private ToolStripMenuItem _menuControls;
        private Panel _drawingAreaCanvas;
        private bool _dropBtnPressed;
        private int _dropRate;
        private GameGrid _gameGrid;
        private SolidBrush[][] _gameGridBrushes;
        private int _gameSpeed;
        private Timer _gameTimer;
        private bool _isGameOver;
        private bool _isGamePaused;
        private bool _isRowFull;
        private Label _label1;
        private Label _label14;
        private Label _label3;
        private Label _lblRows;
        private Label _lblScore;
        private int _level;
        private int _levelRowsCompleted;
        private Screen _mainScreen;
        private ToolStripMenuItem _menuFile;
        private ToolStripMenuItem _menuNewGame;
        private ToolStripMenuItem _menuResetGrid;
        private MenuStrip _menuStrip1;
        private int _nextShapeType;
        private int _numberOfCols;
        private int _numberOfRows;
        private readonly Random _randomShapeType = new Random();

        private Rectangle _rectangle;
        private Rectangle[][] _rectangleGameGrid;
        private Rectangle[] _rectangleShape;
        private long _score;
        private int _shapeType;

        private bool _startBtnPressed;
        private Screen _startScreen;
        private SolidBrush[] _theBrushColors;
        public static PrivateFontCollection Retrofont = new PrivateFontCollection();
        private Panel _titleBar;
        private int _totalRowsCompleted;

        public bool IsDropped { get; set; }
        public int BonusStep { get; set; }

        #endregion
    }
}