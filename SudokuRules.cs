class SudokuSolver {
    public List<ISudokuRule> Rules;
    public Sudoku Board;

    public Tuple<int,int>? FindZero(Sudoku Board){
        for (int i = 0; i < 9; i++){
            for (int j = 0; j < 9; j++){
                if (Board.Board[i,j] == 0){
                    return (i,j);
                }
            }
        }
        return null;
    }

    public boolean AllValid(ISudokuRule[] RuleSet, Sudoku Board, int i, int j, int value){
        foreach (ISudokuRule Rule in RuleSet){
            if (!Rule.IsValid(Board, i, j, value)){
                return false;
            }
        }
        return true;
    }


    public Sudoku? Solve(ISudokuRule[] RuleSet, Sudoku Puzzle){
        Tuple<int,int> (i,j) = FindZero(Puzzle);
        for (int val = 1; val < 10; val++){
            if (AllValid(RuleSet, Puzzle, i, j, val)){
                Puzzle[i,j] = val;
                BestSol = Solve(RuleSet, Puzzle);
                if (BestSol is not null){
                    return BestSol;
                }
            }
            Puzzle[i,j] = 0;
        }
        return null;
    }
}



class Sudoku {
    public int[,] Board;
    public int Size = Board.GetLength(0);

    public Sudoku(int[,] Puzzle){
        this.Board = Puzzle;
    }
}

interface ISudokuRule {
    static boolean IsValid(Sudoku Board, int i, int j, int value);
}

class StandardRule : ISudokuRule {
    public boolean IsValid(Sudoku Board, int i, int j, int value) {
        for (int k = 0; k < board.Size; k++){
            if (Board.Board[i,k] == value || Board.Board[k,j] == value){
                return false;
            }
        }
        for (int x = 0; x < 3; x++){
            for (int y = 0; y < 3; y++){
                if (Board.Board[i + x - (i%3), j + y - (j%3)] == value){
                    return false;
                }
            }
        }
        return true;
    }
}

class KingsMoveRule : ISudokuRule {
    public boolean IsValid(Sudoku Board, int i, int j, int value) {
        for (int x = -1; x < 2; x++){
            for (int y = -1; y < 2; y++){
                if (Board[max(min(i + x,Board.Size - 1),0), max(min(j + y,Board.Size - 1),0)] == value){
                    return false;
                }
            }
        }
        return true;
    }
}

class KnightsMoveRule : ISudokuRule {
    public boolean IsValid(Sudoku Board, int i, int j, int value) {
        if (
            Board[max(i-1,0),max(j-2,0)] == value || 
            Board[max(i-1,0),min(j+2,8)] == value ||
            Board[min(i+1,8),max(j-2,0)] == value ||
            Board[min(i+1,8),min(j+2,8)] == value ||
            Board[max(i-2,0),max(j-1,0)] == value || 
            Board[max(i-2,0),min(j+1,8)] == value ||
            Board[min(i+2,8),max(j-1,0)] == value ||
            Board[min(i+2,8),min(j+1,8)] == value ||
        )
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}


static void Main(string[] args){
    int[,] sample = {
        {0,0,0,0,7,0,0,1,0},
        {1,0,0,0,2,9,7,0,0},
        {3,0,0,5,0,0,9,0,0},
        {0,0,0,0,0,0,0,0,0},
        {0,8,0,7,4,5,0,0,0},
        {7,0,0,0,0,0,2,8,0},
        {0,0,0,0,8,0,6,0,0},
        {6,9,4,0,0,0,0,0,0},
        {0,0,0,0,1,0,0,4,0}};
    
    Sudoku Board = Sudoku(sample);
    ISudokuRule[] Rules = {new StandardRule()};
    SudokuSolver Solver = new SudokuSolver();
    Console.WriteLine(Solver.Solve(Rules, Board));
}