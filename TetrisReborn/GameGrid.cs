using System.Drawing;

namespace TetrisReborn {
    public class GameGrid {
        private readonly SolidBrush[][] _gameGridBrushes;
        private readonly Rectangle[][] _rectangleGameGrid;
        private readonly SolidBrush[] _theBrushColors;

        public GameGrid(int gameGridRows, int gameGridColumns) {
            _rectangleGameGrid = new Rectangle[gameGridRows][];
            _gameGridBrushes = new SolidBrush[gameGridRows][];
            _theBrushColors = new SolidBrush[5];

            for (var i = 0; i < gameGridRows; i++) {
                _rectangleGameGrid[i] = new Rectangle[gameGridColumns];
                _gameGridBrushes[i] = new SolidBrush[gameGridColumns];
            }

            // #
            //###
            _theBrushColors[0] = new SolidBrush(Color.Magenta);
            //  #
            //###
            _theBrushColors[1] = new SolidBrush(Color.Orange);
            //#
            //###
            _theBrushColors[2] = new SolidBrush(Color.DodgerBlue);
            //##
            //##
            _theBrushColors[3] = new SolidBrush(Color.LawnGreen);
            //####
            _theBrushColors[4] = new SolidBrush(Color.Cyan);
        }

        public Rectangle[][] GetGameGrid() {
            return _rectangleGameGrid;
        }

        public SolidBrush[][] GetGameGridBrushes() {
            return _gameGridBrushes;
        }

        public SolidBrush[] GetShapeColors() {
            return _theBrushColors;
        }

        public bool IsGridLocationEmpty(int rowNumber, int colNumber) {
            return _rectangleGameGrid[rowNumber][colNumber].IsEmpty;
        }

        public void SetShapeLocation(int rowNumber, int colNumber, Rectangle square, int shapeType) {
            _rectangleGameGrid[rowNumber][colNumber] = square;
            SetShapeColorLocation(rowNumber, colNumber, shapeType);
        }

        public void SetShapeColorLocation(int rowNumber, int colNumber, int shapeType) {
            _gameGridBrushes[rowNumber][colNumber] = _theBrushColors[shapeType - 1];
        }

        public void DropRowsDown(int rowNumber, int colNumber) {
            if (!IsGridLocationEmpty(rowNumber - 1, colNumber)) {
                _rectangleGameGrid[rowNumber][colNumber] = new Rectangle(_rectangleGameGrid[rowNumber - 1][colNumber].X,
                    _rectangleGameGrid[rowNumber - 1][colNumber].Y + 10, 10, 10);
                _gameGridBrushes[rowNumber][colNumber] = _gameGridBrushes[rowNumber - 1][colNumber];
            }
            else {
                _rectangleGameGrid[rowNumber][colNumber] = _rectangleGameGrid[rowNumber - 1][colNumber];
            }
        }

        public void SetTopRow() {
            _rectangleGameGrid[0] = new Rectangle[_rectangleGameGrid[1].Length];
        }
    }
}