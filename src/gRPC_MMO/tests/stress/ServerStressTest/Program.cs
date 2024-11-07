// See https://aka.ms/new-console-template for more information


// -----------------------------------
const int NUM_OF_CLIENTS = 100;
const string ADDRESS = "https://localhost:7197";



Console.WriteLine("Hello, World!");

RunGrpcTest();


// wait
Console.ReadLine();


// -----------------------------------


static void RunGrpcTest()
{
    List<GrpcClient> clients = [];

    for (int i = 0; i < NUM_OF_CLIENTS; i++)
    {
        GrpcClient client = new(ADDRESS, i==0);
        clients.Add(client);
    }
}