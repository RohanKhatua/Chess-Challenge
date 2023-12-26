using System;
using System.Collections.Generic;
using ChessChallenge.API;

public class MyBot : IChessBot
{
    private Dictionary<string, float> pieceValues;

    public MyBot()
    {
        InitializePieceValues();
    }

    public Move Think(Board board, Timer timer)
    {
        string color = board.IsWhiteToMove ? "White" : "Black";
        Move[] moves = board.GetLegalMoves();

        float bestScore = color == "White" ? float.MinValue : float.MaxValue;
        Move bestMove = moves[0];

        foreach (Move move in moves)
        {
            board.MakeMove(move);
            float score = Evaluate(board);
            board.UndoMove(move);

            bestMove = DetermineBestMove(color, move, score, ref bestScore, bestMove);
        }

        return bestMove;
    }

    private Move DetermineBestMove(string color, Move move, float score, ref float bestScore, Move currentBestMove)
    {
        if ((color == "White" && score > bestScore) || (color == "Black" && score < bestScore))
        {
            bestScore = score;
            return move;
        }

        return currentBestMove;
    }

    private void InitializePieceValues()
    {
        pieceValues = new Dictionary<string, float> {
            { "Queen", 9 },
            { "Rook", 5 },
            { "Bishop", 3 },
            { "Knight", 3 },
            { "Pawn", 1 }
        };
    }

    public float Evaluate(Board board)
    {
        float score = 0;

        foreach (KeyValuePair<string, float> entry in pieceValues)
        {
            PieceType pieceType = (PieceType)Enum.Parse(typeof(PieceType), entry.Key);
            PieceList whitePieces = board.GetPieceList(pieceType, true);
            PieceList blackPieces = board.GetPieceList(pieceType, false);

            score += (whitePieces.Count - blackPieces.Count) * entry.Value;
        }

        return score;
    }
}
