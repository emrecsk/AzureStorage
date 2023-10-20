using AzureStrorageLibrary;
using AzureStrorageLibrary.Services;
using System.Text;
// See https://aka.ms/new-console-template for more information
ConnectionString.Con = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";

AzQueue azQueue = new AzQueue("testqueue");

// To send a message:
    //string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("This is for test 1"));
    //azQueue.SendMessageAsync(base64).Wait();
    //Console.WriteLine("Message sent");
 

// To receive a message:
var message = azQueue.RetrieveMessageAsync().Result;
//if (message != null)
//{
//    Console.WriteLine(Encoding.UTF8.GetString(Convert.FromBase64String(message)));
//}
//else
//{
//    Console.WriteLine("No message");
//}

await azQueue.DeleteMessageAsync(message.MessageId, message.PopReceipt);
Console.WriteLine("Message deleted");