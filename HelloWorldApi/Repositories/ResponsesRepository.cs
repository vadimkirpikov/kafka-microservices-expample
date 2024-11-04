using System.Data;
using HelloWorldApi.Models;
using Npgsql;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HelloWorldApi.Repositories;

public class ResponsesRepository(IDbConnection dbConnection, ILogger<ResponsesRepository> logger) : IAddRepository<HelloWorldResponse>
{
    public void CreateTable()
    {
        dbConnection.Execute("CREATE TABLE IF NOT EXISTS Responses (Text TEXT, DateTime DATE, IsNotified boolean);");
        logger.LogInformation("Created table");
	}
    public async Task AddAsync(HelloWorldResponse response)
    {
        try
        {
            var query = "INSERT INTO Responses (Text, DateTime, IsNotified) VALUES (@Text, @DateTime, @IsNotified)";
            await dbConnection.ExecuteAsync(query, response);
            logger.LogInformation("Request to add response to database");
        }
        catch (Exception ex)
        {
            logger.LogError($"Failed to insert with {ex.Message}");   
        }

    }
}