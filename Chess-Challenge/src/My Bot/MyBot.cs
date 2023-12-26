using System;
using System.Collections.Generic;
using ChessChallenge.API;

public class MyBot : IChessBot
{
    private Dictionary<string, float> pieceValues;
    private const int MaxDepth = 3;

    private void InitializePieceValues()
    {
        pieceValues = new Dictionary<string, float> {
            {"Queen", 9},
            {"Rook", 5},
            {"Bishop", 3},
            {"Knight", 3},
            {"Pawn", 1}
        };
    }

    public MyBot()
    {
        InitializePieceValues();
    }

    // A variation of the MiniMax algorithm
    // Instead of using 2 separate subroutines for the minimizer and maximizer, we use a single subroutine
    // our static evaluation function must return a score relative to the side being evaluated
    private float NegaMax(Board board, int depth)
    {
        if (depth == 0)
        {
            return Evaluate(board);
        }

        float maxScore = float.MinValue;
        foreach (Move move in board.GetLegalMoves())
        {
            board.MakeMove(move);
            float score = -NegaMax(board, depth - 1);
            // We have to switch the sign of the score because we are evaluating the position from the perspective of the opponent
            board.UndoMove(move);

            if (score > maxScore)
            {
                maxScore = score;
            }
        }

        return maxScore;
    }

    public float Evaluate(Board board)
    {
        float score = 0;

        foreach (KeyValuePair<string, float> entry in pieceValues)
        {
            PieceType pieceType = (PieceType)Enum.Parse(typeof(PieceType), entry.Key);
            score += (board.GetPieceList(pieceType, true).Count - board.GetPieceList(pieceType, false).Count) * entry.Value;
        }

        return board.IsWhiteToMove ? score : -score;
    }

    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        float bestScore = float.MinValue;
        Move bestMove = moves[0];

        foreach (Move move in moves)
        {
            board.MakeMove(move);
            float score = -NegaMax(board, MaxDepth - 1);
            board.UndoMove(move);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }
}