namespace TeeScore.Contracts
{
    public interface IDatabaseConnection
    {
        string DbConnection(string dbVersion);
    }
}
