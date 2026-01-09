// See https://aka.ms/new-console-template for more information

using FitnessNetworkApp.UI;
using FitnessNetworkApp1.Services;
using FitnessNetworkApp1.Storage;

var storage = new JsonFileStorage("Data");
var fitness = new FitnessService(storage);
var auth = new AuthService(fitness);

var u = auth.RegisterClient("test", "123");
Console.WriteLine("Creat: " + u.Username);

var logged = auth.Login("test", "123");
Console.WriteLine("Logat: " + logged.Username);




