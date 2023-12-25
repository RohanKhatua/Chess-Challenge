using System;
using ChessChallenge.API;

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        string myColour = board.IsWhiteToMove ? "White" : "Black";
        Console.WriteLine("My colour is " + myColour);
        Move bestMove = moves[0];
        float bestEval = 0;
        foreach (Move move in moves)
        {
            Console.WriteLine("Trying move: " + move);
            board.MakeMove(move);

            // If the move results in checkmate, return it immediately.
            if (board.IsInCheckmate())
            {
                board.UndoMove(move);
                return move;
            }

            float currentEval = Evaluate(board);
            Console.WriteLine("Current eval: " + currentEval);
            if (myColour == "White")
            {
                if (currentEval > bestEval)
                {
                    bestEval = currentEval;
                    bestMove = move;
                }
            }
            else
            {
                if (currentEval < bestEval)
                {
                    bestEval = currentEval;
                    bestMove = move;
                }
            }
            board.UndoMove(move);
        }

        return bestMove;
    }

    // Positive score means white is winning, negative score means black is winning.
    public float Evaluate(Board board)
    {
        float whiteScore = 0;
        float blackScore = 0;
        PieceList[] allPieceLists = board.GetAllPieceLists();
        foreach (PieceList pieceList in allPieceLists)
        {
            if (pieceList.IsWhitePieceList)
            {
                if (pieceList.TypeOfPieceInList == PieceType.Queen)
                {
                    whiteScore += 9;
                }
                else if (pieceList.TypeOfPieceInList == PieceType.Rook)
                {
                    whiteScore += 5;
                }
                else if (pieceList.TypeOfPieceInList == PieceType.Bishop)
                {
                    whiteScore += 3;
                }
                else if (pieceList.TypeOfPieceInList == PieceType.Knight)
                {
                    whiteScore += 3;
                }
                else if (pieceList.TypeOfPieceInList == PieceType.Pawn)
                {
                    whiteScore += 1;
                }
            }
            else
            {
                if (pieceList.TypeOfPieceInList == PieceType.Queen)
                {
                    blackScore += 9;
                }
                else if (pieceList.TypeOfPieceInList == PieceType.Rook)
                {
                    blackScore += 5;
                }
                else if (pieceList.TypeOfPieceInList == PieceType.Bishop)
                {
                    blackScore += 3;
                }
                else if (pieceList.TypeOfPieceInList == PieceType.Knight)
                {
                    blackScore += 3;
                }
                else if (pieceList.TypeOfPieceInList == PieceType.Pawn)
                {
                    blackScore += 1;
                }
            }
        }

        return whiteScore - blackScore;
    }

}