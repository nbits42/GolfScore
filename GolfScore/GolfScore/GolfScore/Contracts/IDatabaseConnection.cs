namespace GolfScore.Contracts
{
    public interface IDatabaseConnection
    {
        string DbConnection(string dbVersion);
    }
}
