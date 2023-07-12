<div>
	<div>
		<img src=https://raw.githubusercontent.com/Byron2016/00_forImages/main/images/Logo_01_00.png align=left alt=MyLogo width=200>
	</div>
	&nbsp;
	<div>
		<h1>072_CSharp_PruebaSedemi_00</h1>
	</div>
</div>

&nbsp;

# Table of contents

---

- [Table of contents](#table-of-contents)
- [Project Description](#project-description)
- [Technologies used](#technologies-used)
- [References](#references)
- [Steps](#steps)

  - <details> <summary>List of Steps</summary>

    - [Install & Setup Vite + React + Bootstrap 5](#-artificial-intelligence-and-bots)

   </details>

[⏪(Back to top)](#table-of-contents)

# Project Description

**072_CSharp_PruebaSedemi_00** is a solution for **SEDEMI work's test**.

[⏪(Back to top)](#table-of-contents)
&nbsp;

# Technologies used

---

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

[⏪(Back to top)](#table-of-contents)

# References

---

- Shields.io

  - [Shields.io](https://shields.io/)

  - [Github Ileriayo markdown-badges](https://github.com/Ileriayo/markdown-badges)

  - [Github Ileriayo markdown-badges WebSite](https://ileriayo.github.io/markdown-badges/)

  - [EF6-CodeFirst](https://www.entityframeworktutorial.net/code-first/configure-one-to-one-relationship-in-code-first.aspx)

- API call
  - [Cómo consumir un API Rest como clientes con C#](https://www.luisllamas.es/como-consumir-un-api-rest-como-clientes-con-c/)
  - [WebRequest Class](https://learn.microsoft.com/en-us/dotnet/api/system.net.webrequest?view=net-7.0)

[⏪(Back to top)](#table-of-contents)

# Steps

- Create a new MVC project
  - Create a new MVC project with support for authentication.
- Add a new library to support API comunications.

  - Create a new C# Library
  - Add **Newtonsoft.Json** Nuget package.
  - Enums
    - Create **ErrorsEnumAPI** Enum to manage errors' codes
      ```cs
        namespace PruebaSedemi_00.API.Enums
        {
            public enum ErrorsEnumAPI
            {
                NoUrl = 0,
                NoError = 1,
                NoData = 2,
                CatchError = 3,
                OtherError = 99
            }
        }
      ```
  - Models

    - Create **ErrorAPI** class

      ```cs
        using PruebaSedemi_00.API.Enums;

        namespace PruebaSedemi_00.API.Models
        {
            public class ErrorAPI
            {
                public ErrorsEnumAPI ErrorCode { get; set; } =        ErrorsEnumAPI.NoError;
                public string? ErrorMsg { get; set; }

            }
        }
      ```

    - Create **PokemonAPI** class

      ```cs
        using Newtonsoft.Json;
        using PruebaSedemi_00.API.Enums;
        using PruebaSedemi_00.API.Models;
        using System.Net;
        using System.Reflection.PortableExecutable;

        namespace PruebaSedemi_00.API.Repositories
        {
            public class PokemonRepositoryAPI : IPokemonRepositoryAPI
            {
                public PokemonRecordAPI GetItems(string url)
                {
                    // Create a request for the URL.
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    // If required by the server, set the credentials.
                    request.Method = "GET";
                    request.ContentType = "application/json";
                    request.Accept = "application/json";

                    ErrorAPI errorStatus = new ErrorAPI();
                    PokemonRecordAPI pokemonRecordAPI = new PokemonRecordAPI();

                    try
                    {
                        if (url.Length == 0 || url == null)
                        {
                            return ErrorManger(0);
                        }

                        // Get the response.
                        using (WebResponse response = request.GetResponse())
                        {
                            // Display the status.
                            // Console.WriteLine(response.StatusDescription);
                            // Get the stream containing content returned by the server.
                            using (Stream strReader = response.GetResponseStream())
                            {
                                if (strReader == null)
                                {
                                    return ErrorManger(2);
                                }
                                // Open the stream using a StreamReader for easy access.
                                using (StreamReader objReader = new StreamReader(strReader))
                                {
                                    // Read the content.
                                    string responseBody = objReader.ReadToEnd();

                                    if (responseBody != null && responseBody.Length >= 0)
                                    {
                                        errorStatus.ErrorCode = ErrorsEnumAPI.NoError;
                                        pokemonRecordAPI.ErrorStatus = errorStatus;

                                        pokemonRecordAPI = JsonConvert.       DeserializeObject<PokemonRecordAPI>(responseBody);


                                        // Cleanup the streams and the response.
                                        // objReader.Close();
                                        // strReader.Close();
                                        // strReader.Close();

                                        return pokemonRecordAPI;
                                    }

                                    return ErrorManger(2);

                                }
                            }
                        }

                    }
                    catch (WebException e)
                    {
                        errorStatus.ErrorCode = ErrorsEnumAPI.CatchError;
                        errorStatus.ErrorMsg = e.Message;

                        pokemonRecordAPI.ErrorStatus = errorStatus;

                        return pokemonRecordAPI;
                    }
                }

                private PokemonRecordAPI ErrorManger(int errorCase)
                {
                    ErrorAPI errorStatus = new ErrorAPI();
                    PokemonRecordAPI pokemonRecordAPI = new PokemonRecordAPI();

                    switch (errorCase)
                    {
                        case 0:
                            errorStatus.ErrorCode = ErrorsEnumAPI.NoUrl;
                            errorStatus.ErrorMsg = "Without a valid URL";
                            pokemonRecordAPI.ErrorStatus = errorStatus;
                            break;

                        case 2:
                            errorStatus.ErrorCode = ErrorsEnumAPI.NoData;
                            errorStatus.ErrorMsg = "Without Data";
                            pokemonRecordAPI.ErrorStatus = errorStatus;
                            break;

                        default:
                            errorStatus.ErrorCode = ErrorsEnumAPI.OtherError;
                            errorStatus.ErrorMsg = "Other Error";
                            pokemonRecordAPI.ErrorStatus = errorStatus;
                            break;
                    }

                    return pokemonRecordAPI;

                }
            }
        }
      ```

  - Test call from MVC project

    - Inject **PokemonRepositoryAPI** into **program.cs**

      ```cs
        ....
        using PruebaSedemi_00.API.Repositories;
        ....

        namespace PruebaSedemi_00.MVC
        {
            public class Program
            {
                public static void Main(string[] args)
                {
                    ....
                    builder.Services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(connectionString));

                    builder.Services.AddScoped<IPokemonRepositoryAPI, PokemonRepositoryAPI>       ();

                    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
                    ....
                }
            }
        }
      ```

    - Modify **HomeController** class

      ```cs
        ....
        using PruebaSedemi_00.API.Repositories;
        ....

        namespace PruebaSedemi_00.MVC.Controllers
        {
            public class HomeController : Controller
            {
                ....

                private readonly IPokemonRepositoryAPI _pokemonRepositoryAPI;

                public HomeController(ILogger<HomeController> logger, IPokemonRepositoryAPI         pokemonRepositoryAPI)
                {
                    _logger = logger;
                    _pokemonRepositoryAPI = pokemonRepositoryAPI;
                }

                public IActionResult Index()
                {
                    string url = "https://pokeapi.co/api/v2/pokemon/";

                    var result = _pokemonRepositoryAPI.GetItems(url);

                    return View();
                }
            }
        }
      ```
