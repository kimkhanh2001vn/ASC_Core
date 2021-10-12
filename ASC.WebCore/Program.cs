using ASC.DataAccess;
using ASC.WebCore.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ASC.WebCore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            // Azure Storage Account and Table Service Instance
            CloudStorageAccount storageAccount;
            CloudTableClient tableClient;
            // connect to strorege 
            storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            //create the table 'book', if it not exists
            tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Books");
            await table.CreateIfNotExistsAsync();
            //create a book instance
            Book books = new Book()
            {
                Author = "Rami",
                BookName = "ASP.NET Core with Azure",
                publisher = "APress"
            };
            books.BookId = 1;
            books.RowKey = books.BookId.ToString();
            books.PartitionKey = books.publisher;
            books.CreateDate = DateTime.Now;
            books.UpdateDate = DateTime.Now;

            // insert and excute operations

            TableOperation insertOperation = TableOperation.Insert(books);
            await table.ExecuteAsync(insertOperation);
            Console.ReadLine();
            using (var _unitOfWork = new UnitOfWork("UseDevelopmentStorage=true;"))
            {
                var bookRepository = _unitOfWork.Reponsitory<Book>();
                await bookRepository.CreateTableAsync();
                Book book = new Book()
                {
                    Author = "Rami",
                    BookName = "ASP.NET Core With Azure",
                    publisher = "APress"
                };
                book.BookId = 1;
                book.RowKey = book.BookId.ToString();
                book.PartitionKey = book.publisher;
                var data = await bookRepository.AddAsync(book);
                Console.WriteLine(data);
                _unitOfWork.CommitTransaction();
            }
            using (var _unitOfWork = new UnitOfWork("UseDevelopmentStorage=true;"))
            {
                var bookRepository = _unitOfWork.Reponsitory<Book>();
                await bookRepository.CreateTableAsync();
                var data = await bookRepository.FindAsync("APress", "1");
                Console.WriteLine(data);
                data.Author = "Rami Vemula";
                var updatedData = await bookRepository.UpdateAsync(data);
                Console.WriteLine(updatedData);
                _unitOfWork.CommitTransaction();
            }
            using (var _unitOfWork = new UnitOfWork("UseDevelopmentStorage=true;"))
            {
                var bookRepository = _unitOfWork.Reponsitory<Book>();
                await bookRepository.CreateTableAsync();
                var data = await bookRepository.FindAsync("APress", "1");
                Console.WriteLine(data);
                await bookRepository.DeleteAsync(data);
                Console.WriteLine("Deleted");
                // Throw an exception to test rollback actions
                // throw new Exception();
                _unitOfWork.CommitTransaction();
            }
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) 
        {
            var buider = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    if (env.IsDevelopment())
                    {
                        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        if (appAssembly != null)
                        {
                            config.AddUserSecrets(appAssembly, optional: true);
                        }
                    }
                    config.AddEnvironmentVariables();
                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseIISIntegration()
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                });
            return buider;
        }
        public IConfiguration configuration { get; }
           
    }
}
