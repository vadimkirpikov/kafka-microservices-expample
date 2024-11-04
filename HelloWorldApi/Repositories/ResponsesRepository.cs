using System.Data;
using HelloWorldApi.Models;
using Npgsql;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HelloWorldApi.Repositories;

public class ResponsesRepository: IAddRepository<HelloWorldResponse>
{
    private readonly IDbConnection _dbConnection;
    public ResponsesRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
        _dbConnection.Execute(
            "CREATE TABLE IF NOT EXISTS Responses (Text TEXT, DateTime DATETIME, IsNotified boolean);");
    }
    public async Task AddAsync(HelloWorldResponse response)
    {
        var query = "INSERT INTO Responses (Text, DateTime, IsNotified) VALUES (@Text, @DateTime, @IsNotified)";
        await _dbConnection.ExecuteAsync(query, response);
    }
}