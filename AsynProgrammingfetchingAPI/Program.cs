// See https://aka.ms/new-console-template for more information
using System.Data.Common;
using System.Text.Json;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

//Console.WriteLine("Async Programming");
//Example: Fetching JSON Data from a Public API
//We'll fetch random user data from Random User API.


class Program
{
    static async Task Main ()
    {
        await FetchUserDataAsync();
    }

    public static async Task FetchUserDataAsync()
    {
        using HttpClient client = new HttpClient();
        string url = "https://randomuser.me./api/";

        try
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url);


            if ( responseMessage.IsSuccessStatusCode)
            {
                string jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                Console.WriteLine("Raw Json Response");
                Console.WriteLine(jsonResponse);


                //parsejson
                var userData = JsonSerializer.Deserialize<UserResponse>(jsonResponse);
                Console.WriteLine($"\nUser Name: {userData?.Result[0].Name.FirstName} {userData?.Result[0].Name.LastName}");
                Console.WriteLine($"Email: {userData?.Result[0].Email}");
                Console.WriteLine($"Country: {userData?.Result[0].Location.countryName}");

            }
            else
            {
                Console.WriteLine($"Error:{responseMessage.StatusCode}");
            }
        }
        catch ( Exception ex )
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
    //2Making Multiple API Calls in Parallel
    //Instead of waiting for one request at a time, we can fetch multiple users in parallel using
    public static async Task FetchMultipleUsersAsync(int userCount)
    {
        using HttpClient client = new HttpClient();
        string url = "https://randomuser.me/api/";

        Task<string>[] fetchTasks = new Task<string>[userCount];

        // Start multiple async requests
        for (int i = 0; i < userCount; i++)
        {
            fetchTasks[i] = client.GetStringAsync(url);
        }

        // Wait for all tasks to complete
        string[] results = await Task.WhenAll(fetchTasks);

        // Process each result
        for (int i = 0; i < results.Length; i++)
        {
            var userData = JsonSerializer.Deserialize<UserResponse>(results[i]);
            Console.WriteLine($"User {i + 1}: {userData?.Result[0].Name.FirstName} {userData?.Result[0].Name.LastName}");
        }
    }
}
//Json Model Class 
public class UserResponse
{
    public User[] Result { get; set; }
}

public class User
{
    public Name Name  { get; set; }
    public string Email { get; set; }
    public Location Location { get; set; }
}
public class Name
{
    public string  FirstName { get; set; }
    public string  LastName { get; set; }

}
public class Location
{
    public string countryName { get; set; }
}