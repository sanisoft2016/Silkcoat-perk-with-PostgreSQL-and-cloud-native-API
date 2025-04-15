using Microsoft.Extensions.Hosting;
using System.Buffers.Text;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("password", "123456@Abc$", secret: true);
var username = builder.AddParameter("username","postgreadmin", secret: true);

//var password = builder.AddParameter("password", secret: true);
//var sqlServer = builder.AddPostgres("sqlserver", password)
//                       .WithDataVolume();

//var sqlServerDb = sqlServer.AddDatabase("silkcoatDb");

//Host = silkcoatpostgres; Port = 5433; Username = postgreadmin; Password = 123456@Abc; Database = silkcoaatDb
//Host = silkcoaptogres; Port = 5433; Username = postgreadmin; Password = 123456@Abc; Database = silkcoaatDb
//echo - n "Server=tcp:sqlserver-service.default.svc.cluster.local,1433;Database=YourDatabaseName;User Id=your_username;Password=your_password;" | base64
//echo -n "Host = silkcoatpostgres; Port = 5433; Username = postgreadmin; Password = 123456@Abc; Database = silkcoaatDb" | base64




var postgres = builder.AddPostgres("silkcoatpostgres", username, password, 5433)
    .WithImageTag("17.0")
    .WithDataVolume(isReadOnly: false)
    .WithPgAdmin();

var sqlServerDb = postgres.AddDatabase("silkcoatDb");



builder.AddProject<Projects.CustRewardMgtSys_API>("custrewardmgtsys-v3-api")
     .WithReference(sqlServerDb)
     .WaitFor(sqlServerDb);


builder.Build().Run();