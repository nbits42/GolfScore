namespace TeeScore.Contracts
{
    public enum PlayGameState
    {
        Idle,
        Waiting,
        Started,
        Finished,
        EndScoreCalculating,
        EndScoreCalculated,
        Ready
    }
}