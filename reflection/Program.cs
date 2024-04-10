using reflection.infra;

var prefixes = new string[] { "http://localhost:5341/" };
var webApplication = new WebApplication(prefixes);
webApplication.Open();