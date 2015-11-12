using System.Drawing;

namespace TetrisReborn {
    public class Shape {
        private readonly int[] _blockXPos = new int[4];
        private readonly int[] _blockYPos = new int[4];
        private readonly int _currentShape;
        private readonly bool _isNextShape;
        private readonly int _panelHeight;
        private readonly int _panelWidth;

        private readonly int _startingXCoordinate;
        private readonly int[] _yPositions = new int[4];
        private int _currentShapePosition = 1;

        public Point[] PntShape;
        public Rectangle[] RectangleShape;
        public bool ShapeMoving;

        public Shape(int shapeType, int screenWidth, int screenHeight, bool nextShape) {
            _startingXCoordinate = (screenWidth - 1)/2;
            _panelWidth = screenWidth;
            _panelHeight = screenHeight;
            _isNextShape = nextShape;

            _currentShape = shapeType;

            SetShapeStart();
        }

        private void BuildShape() {
            RectangleShape = new Rectangle[4];
            RectangleShape[0] = new Rectangle(_blockXPos[0], _blockYPos[0], 10, 10);
            RectangleShape[1] = new Rectangle(_blockXPos[1], _blockYPos[1], 10, 10);
            RectangleShape[2] = new Rectangle(_blockXPos[2], _blockYPos[2], 10, 10);
            RectangleShape[3] = new Rectangle(_blockXPos[3], _blockYPos[3], 10, 10);
        }

        public Rectangle[] MoveShapeDown(int movePixels, Rectangle[][] rectangleGameGrid) {
            var canMove = true;

            for (var j = 0; j < 4; j++) {
                if (_blockYPos[j] + 10 + movePixels > _panelHeight - 1) {
                    canMove = false;
                    ShapeMoving = false;
                    break;
                }
            }

            if (canMove) {
                for (var k = 0; k < 4; k++) {
                    if (decimal.Remainder(_blockYPos[k], 10) == 0 && _blockYPos[k] >= 0) {
                        if (_blockYPos[k] == 260) {
                            canMove = false;
                            ShapeMoving = false;
                            break;
                        }
                        if (!rectangleGameGrid[_blockYPos[k]/10 + 1][_blockXPos[k]/10].IsEmpty) {
                            canMove = false;
                            ShapeMoving = false;
                            break;
                        }
                    }
                }
            }
            if (canMove) {
                for (var i = 0; i < 4; i++) {
                    _blockYPos[i] += movePixels;
                }
            }
            BuildShape();
            return RectangleShape;
        }

        public Rectangle[] MoveShapeLeft(int intMovePixels, Rectangle[][] rectangleGameGrid) {
            var canMove = true;
            var yPosition = new int[4];

            var furthestX = _panelWidth;

            for (var j = 0; j < 4; j++) {
                if (_blockXPos[j] <= furthestX) {
                    furthestX = _blockXPos[j];
                    yPosition[j] = _blockYPos[j];
                    if (decimal.Remainder(_blockYPos[j], 10) != 0) {
                        yPosition[j] += 5;
                    }
                }
                if (_blockXPos[j] - intMovePixels < 0) {
                    canMove = false;
                    break;
                }
            }
            if (canMove) {
                for (var i = 0; i < 4; i++) {
                    if (yPosition[i] >= 0) {
                        if (!rectangleGameGrid[yPosition[i]/10][(furthestX - 10)/10].IsEmpty) {
                            canMove = false;
                            break;
                        }
                    }
                }
            }
            if (canMove) {
                for (var i = 0; i < 4; i++) {
                    _blockXPos[i] -= intMovePixels;
                }
            }
            BuildShape();
            return RectangleShape;
        }

        public Rectangle[] MoveShapeRight(int intMovePixels, Rectangle[][] rectangleGameGrid) {
            var canMove = true;
            var yPosition = new int[4];

            var furthestX = 0;

            for (var j = 0; j < 4; j++) {
                if (_blockXPos[j] >= furthestX) {
                    furthestX = _blockXPos[j];
                    yPosition[j] = _blockYPos[j];
                    if (decimal.Remainder(_blockYPos[j], 10) != 0) {
                        yPosition[j] += 5;
                    }
                }
                if (_blockXPos[j] + intMovePixels + 10 >= _panelWidth) {
                    canMove = false;
                    break;
                }
            }
            if (canMove) {
                for (var i = 0; i < 4; i++) {
                    if (yPosition[i] >= 0) {
                        if (!rectangleGameGrid[yPosition[i]/10][(furthestX + 10)/10].IsEmpty) {
                            canMove = false;
                            break;
                        }
                    }
                }
            }
            if (canMove) {
                for (var i = 0; i < 4; i++) {
                    _blockXPos[i] += intMovePixels;
                }
            }
            BuildShape();
            return RectangleShape;
        }

        public Rectangle[] FlipShape(string direction, Rectangle[][] rectangleGameGrid) {
            var canShapeMove = true;
            if (direction == "right") {
                _currentShapePosition++;
                if (_currentShapePosition > 4) {
                    _currentShapePosition = 1;
                }
            }
            if (direction == "left") {
                _currentShapePosition--;
                if (_currentShapePosition < 1) {
                    _currentShapePosition = 4;
                }
            }
            SetShapePosition();
            BuildShape();

            var recGameArea = new Rectangle(0, 0, _panelWidth, _panelHeight);
            var yPosition = new int[4];
            for (var i = 0; i < 4; i++) {
                if (!recGameArea.Contains(RectangleShape[i])) {
                    canShapeMove = false;
                    break;
                }

                yPosition[i] = _blockYPos[i];
                if (decimal.Remainder(_blockYPos[i], 10) != 0) {
                    yPosition[i] += 5;
                }
                if (!rectangleGameGrid[_yPositions[i]/10][_blockXPos[i]/10].IsEmpty) {
                    canShapeMove = false;
                    break;
                }
            }
            if (!canShapeMove) {
                if (direction == "right") {
                    _currentShapePosition--;
                    if (_currentShapePosition < 1) {
                        _currentShapePosition = 4;
                    }
                    SetShapePosition();
                    BuildShape();
                }
                if (direction == "left") {
                    _currentShapePosition++;
                    if (_currentShapePosition > 4) {
                        _currentShapePosition = 1;
                    }
                    SetShapePosition();
                    BuildShape();
                }
            }
            return RectangleShape;
        }

        private void SetShapeStart() {
            if (!_isNextShape) {
                switch (_currentShape) {
                    case 1:
                        _blockXPos[0] = _startingXCoordinate;
                        _blockYPos[0] = -10;
                        _blockXPos[1] = _startingXCoordinate - 10;
                        _blockYPos[1] = -10;
                        _blockXPos[2] = _startingXCoordinate + 10;
                        _blockYPos[2] = -10;
                        _blockXPos[3] = _startingXCoordinate;
                        _blockYPos[3] = -20;
                        break;
                    case 2:
                        _blockXPos[0] = _startingXCoordinate;
                        _blockYPos[0] = -10;
                        _blockXPos[1] = _startingXCoordinate - 10;
                        _blockYPos[1] = -10;
                        _blockXPos[2] = _startingXCoordinate + 10;
                        _blockYPos[2] = -10;
                        _blockXPos[3] = _startingXCoordinate + 10;
                        _blockYPos[3] = -20;
                        break;
                    case 3:
                        _blockXPos[0] = _startingXCoordinate;
                        _blockYPos[0] = -10;
                        _blockXPos[1] = _startingXCoordinate - 10;
                        _blockYPos[1] = -10;
                        _blockXPos[2] = _startingXCoordinate + 10;
                        _blockYPos[2] = -10;
                        _blockXPos[3] = _startingXCoordinate - 10;
                        _blockYPos[3] = -20;
                        break;
                    case 4:
                        _blockXPos[0] = _startingXCoordinate;
                        _blockYPos[0] = -10;
                        _blockXPos[1] = _startingXCoordinate + 10;
                        _blockYPos[1] = -10;
                        _blockXPos[2] = _startingXCoordinate;
                        _blockYPos[2] = -20;
                        _blockXPos[3] = _startingXCoordinate + 10;
                        _blockYPos[3] = -20;
                        break;
                    case 5:
                        _blockXPos[0] = _startingXCoordinate;
                        _blockYPos[0] = -10;
                        _blockXPos[1] = _startingXCoordinate;
                        _blockYPos[1] = -20;
                        _blockXPos[2] = _startingXCoordinate;
                        _blockYPos[2] = -30;
                        _blockXPos[3] = _startingXCoordinate;
                        _blockYPos[3] = -40;
                        break;
                }
            }
            else {
                switch (_currentShape) {
                    case 1:
                        _blockXPos[0] = _startingXCoordinate - 5;
                        _blockYPos[0] = 35;
                        _blockXPos[1] = _startingXCoordinate - 15;
                        _blockYPos[1] = 35;
                        _blockXPos[2] = _startingXCoordinate + 5;
                        _blockYPos[2] = 35;
                        _blockXPos[3] = _startingXCoordinate - 5;
                        _blockYPos[3] = 25;
                        break;
                    case 2:
                        _blockXPos[0] = _startingXCoordinate - 5;
                        _blockYPos[0] = 35;
                        _blockXPos[1] = _startingXCoordinate - 15;
                        _blockYPos[1] = 35;
                        _blockXPos[2] = _startingXCoordinate + 5;
                        _blockYPos[2] = 35;
                        _blockXPos[3] = _startingXCoordinate + 5;
                        _blockYPos[3] = 25;
                        break;
                    case 3:
                        _blockXPos[0] = _startingXCoordinate - 5;
                        _blockYPos[0] = 35;
                        _blockXPos[1] = _startingXCoordinate - 15;
                        _blockYPos[1] = 35;
                        _blockXPos[2] = _startingXCoordinate + 5;
                        _blockYPos[2] = 35;
                        _blockXPos[3] = _startingXCoordinate - 15;
                        _blockYPos[3] = 25;
                        break;
                    case 4:
                        _blockXPos[0] = _startingXCoordinate - 10;
                        _blockYPos[0] = 35;
                        _blockXPos[1] = _startingXCoordinate;
                        _blockYPos[1] = 35;
                        _blockXPos[2] = _startingXCoordinate - 10;
                        _blockYPos[2] = 25;
                        _blockXPos[3] = _startingXCoordinate;
                        _blockYPos[3] = 25;
                        break;
                    case 5:
                        _blockXPos[0] = _startingXCoordinate - 5;
                        _blockYPos[0] = 45;
                        _blockXPos[1] = _startingXCoordinate - 5;
                        _blockYPos[1] = 35;
                        _blockXPos[2] = _startingXCoordinate - 5;
                        _blockYPos[2] = 25;
                        _blockXPos[3] = _startingXCoordinate - 5;
                        _blockYPos[3] = 15;
                        break;
                }
                BuildShape();
            }
        }

        public Rectangle[] GetShape() {
            return RectangleShape;
        }

        private void SetShapePosition() {
            switch (_currentShape) {
                case 1:
                    switch (_currentShapePosition) {
                        case 1:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0] - 10;
                            _blockYPos[1] = _blockYPos[0];
                            _blockXPos[2] = _blockXPos[0] + 10;
                            _blockYPos[2] = _blockYPos[0];
                            _blockXPos[3] = _blockXPos[0];
                            _blockYPos[3] = _blockYPos[0] - 10;
                            break;
                        case 2:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0];
                            _blockYPos[1] = _blockYPos[0] - 10;
                            _blockXPos[2] = _blockXPos[0];
                            _blockYPos[2] = _blockYPos[0] + 10;
                            _blockXPos[3] = _blockXPos[0] + 10;
                            _blockYPos[3] = _blockYPos[0];
                            break;
                        case 3:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0] + 10;
                            _blockYPos[1] = _blockYPos[0];
                            _blockXPos[2] = _blockXPos[0] - 10;
                            _blockYPos[2] = _blockYPos[0];
                            _blockXPos[3] = _blockXPos[0];
                            _blockYPos[3] = _blockYPos[0] + 10;
                            break;
                        case 4:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0];
                            _blockYPos[1] = _blockYPos[0] + 10;
                            _blockXPos[2] = _blockXPos[0];
                            _blockYPos[2] = _blockYPos[0] - 10;
                            _blockXPos[3] = _blockXPos[0] - 10;
                            _blockYPos[3] = _blockYPos[0];
                            break;
                    }
                    break;
                case 2:
                    switch (_currentShapePosition) {
                        case 1:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0] - 10;
                            _blockYPos[1] = _blockYPos[0];
                            _blockXPos[2] = _blockXPos[0] + 10;
                            _blockYPos[2] = _blockYPos[0];
                            _blockXPos[3] = _blockXPos[0] + 10;
                            _blockYPos[3] = _blockYPos[0] - 10;
                            break;
                        case 2:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0];
                            _blockYPos[1] = _blockYPos[0] - 10;
                            _blockXPos[2] = _blockXPos[0];
                            _blockYPos[2] = _blockYPos[0] + 10;
                            _blockXPos[3] = _blockXPos[0] + 10;
                            _blockYPos[3] = _blockYPos[0] + 10;
                            break;
                        case 3:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0] + 10;
                            _blockYPos[1] = _blockYPos[0];
                            _blockXPos[2] = _blockXPos[0] - 10;
                            _blockYPos[2] = _blockYPos[0];
                            _blockXPos[3] = _blockXPos[0] - 10;
                            _blockYPos[3] = _blockYPos[0] + 10;
                            break;
                        case 4:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0];
                            _blockYPos[1] = _blockYPos[0] + 10;
                            _blockXPos[2] = _blockXPos[0];
                            _blockYPos[2] = _blockYPos[0] - 10;
                            _blockXPos[3] = _blockXPos[0] - 10;
                            _blockYPos[3] = _blockYPos[0] - 10;
                            break;
                    }
                    break;
                case 3:
                    switch (_currentShapePosition) {
                        case 1:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0] - 10;
                            _blockYPos[1] = _blockYPos[0];
                            _blockXPos[2] = _blockXPos[0] + 10;
                            _blockYPos[2] = _blockYPos[0];
                            _blockXPos[3] = _blockXPos[0] - 10;
                            _blockYPos[3] = _blockYPos[0] - 10;
                            break;
                        case 2:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0];
                            _blockYPos[1] = _blockYPos[0] - 10;
                            _blockXPos[2] = _blockXPos[0];
                            _blockYPos[2] = _blockYPos[0] + 10;
                            _blockXPos[3] = _blockXPos[0] + 10;
                            _blockYPos[3] = _blockYPos[0] - 10;
                            break;
                        case 3:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0] + 10;
                            _blockYPos[1] = _blockYPos[0];
                            _blockXPos[2] = _blockXPos[0] - 10;
                            _blockYPos[2] = _blockYPos[0];
                            _blockXPos[3] = _blockXPos[0] + 10;
                            _blockYPos[3] = _blockYPos[0] + 10;
                            break;
                        case 4:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0];
                            _blockYPos[1] = _blockYPos[0] + 10;
                            _blockXPos[2] = _blockXPos[0];
                            _blockYPos[2] = _blockYPos[0] - 10;
                            _blockXPos[3] = _blockXPos[0] - 10;
                            _blockYPos[3] = _blockYPos[0] + 10;
                            break;
                    }
                    break;
                case 5:
                    switch (_currentShapePosition) {
                        case 1:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0];
                            _blockYPos[1] = _blockYPos[0] - 10;
                            _blockXPos[2] = _blockXPos[0];
                            _blockYPos[2] = _blockYPos[0] - 20;
                            _blockXPos[3] = _blockXPos[0];
                            _blockYPos[3] = _blockYPos[0] - 30;
                            break;
                        case 2:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0] + 10;
                            _blockYPos[1] = _blockYPos[0];
                            _blockXPos[2] = _blockXPos[0] + 20;
                            _blockYPos[2] = _blockYPos[0];
                            _blockXPos[3] = _blockXPos[0] + 30;
                            _blockYPos[3] = _blockYPos[0];
                            break;
                        case 3:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0];
                            _blockYPos[1] = _blockYPos[0] + 10;
                            _blockXPos[2] = _blockXPos[0];
                            _blockYPos[2] = _blockYPos[0] + 20;
                            _blockXPos[3] = _blockXPos[0];
                            _blockYPos[3] = _blockYPos[0] + 30;
                            break;
                        case 4:
                            _blockXPos[0] = _blockXPos[0];
                            _blockYPos[0] = _blockYPos[0];
                            _blockXPos[1] = _blockXPos[0] - 10;
                            _blockYPos[1] = _blockYPos[0];
                            _blockXPos[2] = _blockXPos[0] - 20;
                            _blockYPos[2] = _blockYPos[0];
                            _blockXPos[3] = _blockXPos[0] - 30;
                            _blockYPos[3] = _blockYPos[0];
                            break;
                    }
                    break;
            }
        }
    }
}