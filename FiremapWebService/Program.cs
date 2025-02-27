# region usings
using System.Text.Json;
using FiremapHelper;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
#endregion

var builder = WebApplication.CreateBuilder(args);
// Password: I_am_CS_Admin1. 
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

#region WeatherForecast
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
#endregion

//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;

//using Microsoft.Data.SqlClient;
//using System.Data;

//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//var app = builder.Build();


/*
// web service Get call to get all the data from the user table
app.MapGet("/testdb", async () =>
{
    return await GetData();
});
*/

// Test program/service
app.MapGet("/hello", async () =>
{
    return "Hello World!";
});

// Humidity 
app.MapGet("/GetHumidity", async () =>
{
    string cnStr = "Server=sql8020.site4now.net;Database=db_aab0df_firemap;User Id=db_aab0df_firemap_admin;Password=NNU_Firemap1.;";
    string sSql = "SELECT HumidityId, HumidityDisplay FROM [db_aab0df_firemap].[dbo].[HumidityOptions]";
    string jsonStr = GetJsonFromSqlDataReaderQuery(cnStr, sSql);
    return jsonStr;
}).WithName("GetHumidity");


//tree cover 
app.MapGet("/GetTreeCover", async () =>
{
    string cnStr = "Server=sql8020.site4now.net;Database=db_aab0df_firemap;User Id=db_aab0df_firemap_admin;Password=NNU_Firemap1.;";
    string sSql = "SELECT TreeCoverID, TreeCoverDisplay FROM [db_aab0df_firemap].[dbo].[TreeCoverOptions]";
    string jsonStr = GetJsonFromSqlDataReaderQuery(cnStr, sSql);
    return jsonStr;
}).WithName("GetTreeCover");

//Fuel Models (works)
app.MapGet("/GetFuelModels", async () =>
{ 
    string cnStr = "Server=sql8020.site4now.net;Database=db_aab0df_firemap;User Id=db_aab0df_firemap_admin;Password=NNU_Firemap1.;";
    string sSql = "SELECT FiremapInputCode, FiremapInputId FROM [db_aab0df_firemap].[dbo].[FiremapInputRef]";
    string jsonStr = GetJsonFromSqlDataReaderQuery(cnStr, sSql);
    return jsonStr;

}).WithName("GetFuelModels");


app.MapGet("/GetFiremapDataOptions", () =>
{
    string cnStr = "Server=sql8020.site4now.net;Database=db_aab0df_firemap;User Id=db_aab0df_firemap_admin;Password=NNU_Firemap1.;";
    string sSql = "SELECT TOP (1000) [OutputRefId]" +
        ",[OutputDisplay]" +
        "FROM [db_aab0df_firemap].[dbo].[vwOutputsRef]";
    string jsonStr = GetJsonFromSqlDataReaderQuery(cnStr, sSql);
    return jsonStr;
})
.WithName("GetFiremapDataOptions");



app.MapPost("/GetFiremapDataParms", async (FiremapInputParameters data) =>
{
    try
    {

        // Validate the JSON data
        if (!data.IsValid())
        {
            List<FiremapData> Firemap = new List<FiremapData>();
            FiremapData errorData = new FiremapData();
            errorData.ErrorNumber = -1;
            errorData.ErrorMessage = "Invalid json data";
            Firemap.Add(errorData);


            string FiremapJson = JsonSerializer.Serialize(Firemap, new JsonSerializerOptions
            {
                WriteIndented = true // Makes the JSON pretty-printed
            });
            return FiremapJson;
        }

        string connectionString = "Server=sql8020.site4now.net;Database=db_aab0df_firemap;User Id=db_aab0df_firemap_admin;Password=NNU_Firemap1.;";
        string jsonResult;

        // Fetch data using the helper method
        //try
        //{

        //Change from GetFiremapModelData to GetFiremapModelDataV2
        jsonResult = GetJsonFromSqlDataReaderParms(connectionString, "GetFiremapModelDataV2", data);

        // try this "ExecuteWebsiteInformation"
        //}
        //catch (Exception sqlEx)
        //{
        //    //return Results.Problem("bad call", sqlEx.Message, 500);
        //}

        return jsonResult;
    }
    catch (Exception ex)
    { 
        List<FiremapData> Firemap= new List<FiremapData>();
        FiremapData errorData = new FiremapData();
        errorData.ErrorNumber = -1;
        errorData.ErrorMessage = ex.Message;
        Firemap.Add(errorData);

        
        string FiremapJson = JsonSerializer.Serialize(Firemap, new JsonSerializerOptions
        {
            WriteIndented = true // Makes the JSON pretty-printed
        });
        return FiremapJson;
    }
});


app.Run();


static string GetJsonFromSqlDataReaderParms(string connectionString, string query, FiremapInputParameters input)

{

    var rows = new List<Dictionary<string, object>>();



    using (SqlConnection connection = new SqlConnection(connectionString))

    {

        connection.Open();

        using (SqlCommand command = new SqlCommand(query, connection))

        {

            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@Humidity", input.Humidity));

            command.Parameters.Add(new SqlParameter("@TreeCover", input.TreeCover));

            command.Parameters.Add(new SqlParameter("@Model1", input.Model1));

            command.Parameters.Add(new SqlParameter("@Model2", input.Model2));

            command.Parameters.Add(new SqlParameter("@Model3", input.Model3));

            command.Parameters.Add(new SqlParameter("@Model4", input.Model4));

            command.Parameters.Add(new SqlParameter("@Model5", input.Model5));

            command.Parameters.Add(new SqlParameter("@Model6", input.Model6));

            command.Parameters.Add(new SqlParameter("@Model7", input.Model7));

            command.Parameters.Add(new SqlParameter("@Model8", input.Model8));



            using (SqlDataReader reader = command.ExecuteReader())

            {

                while (reader.Read())

                {

                    var row = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)

                    {

                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);

                    }

                    rows.Add(row);

                }

            }

        }

    }

    // Serialize the list of rows to JSON

    return JsonSerializer.Serialize(rows, new JsonSerializerOptions

    {

        WriteIndented = true // Makes the JSON pretty-printed

    });

}
static string GetJsonFromSqlDataReaderQuery(string connectionString, string query)
{
    var rows = new List<Dictionary<string, object>>();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.CommandType = System.Data.CommandType.Text;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    }
                    rows.Add(row);
                }
            }
        }
    }

    // Serialize the list of rows to JSON
    return JsonSerializer.Serialize(rows, new JsonSerializerOptions
    {
        WriteIndented = true // Makes the JSON pretty-printed
    });
}


/*
app.MapGet("/testdb/{id}", async (int id) =>
{
    return await GetDataById(id);
});

app.MapPost("/testdb", async (Student student) =>
{
    string query = "INSERT INTO [db_aab0df_testdb].[dbo].[Student](StudentId, StudentName, StudentEmail) VALUES("
        + student.Id.ToString() + ", '" + student.Name + "', '" + student.Email + "')";
    List<string> columnData = new List<string>();
    //This is the login info for the database (this is for the test file) 
    using (SqlConnection connection = new SqlConnection("Server=sql8020.site4now.net;Database=db_aab0df_testdb;User=testaccount;Password=testaccount123456.;Encrypt=false;"))
    {
        SqlCommand command = new SqlCommand(query, connection);
        connection.Open();

        command.ExecuteNonQuery();
    }

    return Results.NoContent();
});

//for every list box

*/
/*
app.MapPost("/testing", async ([FromBody] TestOutPut testOutPut) =>
{
    // Need to insert the data into the database
    string query = "TestProc"; // Name of the stored procedure

    using (SqlConnection connection = new SqlConnection("Server=sql8020.site4now.net;Database=db_aab0df_FireMap;User=db_aab0df_firemap_admin;Password=NNU_Firemap1.;Encrypt=false;"))
    {
        SqlCommand command = new SqlCommand(query, connection);
        command.CommandType = CommandType.StoredProcedure;

        // Add parameters
        command.Parameters.AddWithValue("@parm1", testOutPut.TestOutPutID);
        command.Parameters.AddWithValue("@parm2", testOutPut.TestOutPutDisplay);

        connection.Open();
        command.ExecuteNonQuery();
    }

    return Results.NoContent();
});

*/

// Define a POST endpoint

/*
Task<List<Dictionary<string, object>>> GetData()
{
    DataTable dataTable = new DataTable();
    string query = "SELECT OutputRefid, OutputDisplay FROM [db_aab0df_firemap].[dbo].[vwOutputsRef]";
    var columnData = new List<Dictionary<string, object>>();
    using (SqlConnection connection = new SqlConnection("Server=sql8020.site4now.net;Database=db_aab0df_FireMap;User=db_aab0df_firemap_admin;Password=NNU_Firemap1.;Encrypt=false;"))
    {
        SqlCommand command = new SqlCommand(query, connection);
        connection.Open();

        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i); // Get the column name
                    object value = reader.GetValue(i); // Get the column value
                    row[columnName] = value;
                }

                columnData.Add(row);
            }
        }
    }
    return Task.FromResult(columnData);
}
*/

/*
Task<List<Dictionary<string, object>>> GetData()
{
    DataTable dataTable = new DataTable();
    string query = "SELECT StudentId, StudentName, StudentEmail FROM [db_aab0df_testdb].[dbo].[Student]";
    var columnData = new List<Dictionary<string, object>>();
    using (SqlConnection connection = new SqlConnection("Server=sql8020.site4now.net;Database=db_aab0df_testdb;User=testaccount;Password=testaccount123456.;Encrypt=false;"))
    {
        SqlCommand command = new SqlCommand(query, connection);
        connection.Open();

        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i); // Get the column name
                    object value = reader.GetValue(i); // Get the column value
                    row[columnName] = value;
                }

                columnData.Add(row);
            }
        }
    }
    return Task.FromResult(columnData);
}

Task<List<Dictionary<string, object>>> GetDataById(int id)
{
    DataTable dataTable = new DataTable();
    string query = "SELECT StudentId, StudentName, StudentEmail FROM [db_aab0df_testdb].[dbo].[Student] WHERE StudentId = " + id.ToString();
    var columnData = new List<Dictionary<string, object>>();
    using (SqlConnection connection = new SqlConnection("Server=sql8020.site4now.net;Database=db_aab0df_testdb;User=testaccount;Password=testaccount123456.;Encrypt=false;"))
    {
        SqlCommand command = new SqlCommand(query, connection);
        connection.Open();

        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i); // Get the column name
                    object value = reader.GetValue(i); // Get the column value
                    row[columnName] = value;
                }

                columnData.Add(row);
            }
        }
    }
    return Task.FromResult(columnData);
}

app.Run();
*/

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}