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

- Add MVC project logic

  - Add conexion string
    ```cs
      {
        "ConnectionStrings": {
          "PruebaSedemi": "Server=(localdb)\\mssqllocaldb;Database=PruebaSedemi_20230703;Trusted_Connection=True;MultipleActiveResultSets=true"
          ....
        },
        ....
      }
    ```
  - Call ConnectionString from **program.cs**
  - Update Database
    ```bash
      dotnet ef database update
    ```
  - Add Entities

    ```cs
      using System.ComponentModel.DataAnnotations;

      namespace PruebaSedemi_00.MVC.Data.Entities
      {
          public class Pokemon
          {
              [Required]
              [Key]
              public string Name { get; set; }
              [Required]
              public string Url { get; set; }
              [Required]
              public bool IsSelected { get; set; }
          }
      }
    ```

    - Create a new DBSet into context
    - Add a new migration for the new DBSet
      ```bash
        dotnet ef migrations add  Pokemons_Table
      ```
    - Update Database.

  - Add ViewModel

    ```cs
      using PruebaSedemi_00.MVC.Data.Entities;

      namespace PruebaSedemi_00.MVC.ViewModels
      {
          public class PokemonViewModel
          {
              public int? Count { get; set; }
              public object? Next { get; set; }
              public object? Previous { get; set; }
              public List<Pokemon>? Results { get;      set; }
          }
      }
    ```

  - Add Repositories

    - Add Interfase

      ```cs
        using PruebaSedemi_00.MVC.Data.Entities;

        namespace PruebaSedemi_00.MVC.Repositories
        {
            public interface IPokemonRepositoryDB
            {
                Task<Pokemon> GetByName(string name);
                Task<int> Update(Pokemon pokemon);
                Task<int> Insert(Pokemon pokemon);
            }
        }
      ```

    - Add Repository

      ```cs
        using Microsoft.EntityFrameworkCore;
        using PruebaSedemi_00.MVC.Data;
        using PruebaSedemi_00.MVC.Data.Entities;
        using System.Net;

        namespace PruebaSedemi_00.MVC.Repositories
        {
            public class PokemonRepositoryDB :        IPokemonRepositoryDB
            {
                private readonly ApplicationDbContext         _context;

                public PokemonRepositoryDB        (ApplicationDbContext context)
                {
                    _context = context;
                }
                public async Task<Pokemon> GetByName        (string name)
                {
                    try
                    {
                        Pokemon? pokemons = await         _context.Pokemons.        FirstOrDefaultAsync(x => x.Name         == name);
                        if (pokemons != null)
                        {
                            return pokemons;
                        }

                        return new Pokemon();

                    }
                    catch (Exception e)
                    {
                        //TODO
                        var a = e;
                        return new Pokemon();
                    }
                }

                public async Task<int> Insert(Pokemon         model)
                {
                    try
                    {
                        Pokemon? pokemon = await        _context.Pokemons.      FirstOrDefaultAsync(x => x.Name       == model.Name);
                        if (pokemon == null)
                        {
                            var result = await _context.        Pokemons.AddAsync(model);
                            await _context.       SaveChangesAsync();
                            //_context.SaveChanges();

                            return 1;
                        }
                        else
                        {
                            return 1;
                        }

                    }
                    catch (Exception e)
                    {
                        //TODO
                        var a = e;
                        return 0;
                    }
                }

                public async Task<int> Update(Pokemon         model)
                {
                    try
                    {
                        Pokemon? pokemon = await        _context.Pokemons.      FirstOrDefaultAsync(x => x.Name       == model.Name);
                        if (pokemon != null)
                        {
                            if (pokemon.Name != model.        Name || pokemon.Url != model.       Url || pokemon.IsSelected !=        model.IsSelected)
                            {
                                pokemon.Name = model.       Name;
                                pokemon.Url = model.Url;
                                pokemon.IsSelected =        model.IsSelected;

                                var pokemonChanged =        _context.Pokemons.Attach      (pokemon);
                                pokemonChanged.State =        EntityState.Modified;
                                _context.SaveChanges();

                                return 1;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    catch (Exception e)
                    {
                        //TODO
                        var a = e;
                        return 0;
                    }
                }

            }
        }
      ```

  - Add Controller

    ```cs
      using Microsoft.AspNetCore.Mvc;
      using PruebaSedemi_00.API.Repositories;
      using PruebaSedemi_00.MVC.Data.Entities;
      using PruebaSedemi_00.MVC.Repositories;
      using PruebaSedemi_00.MVC.ViewModels;

      namespace PruebaSedemi_00.MVC.Controllers
      {
          public class PokemonController : Controller
          {
              private readonly IPokemonRepositoryAPI      _pokemonRepositoryAPI;

              private readonly IPokemonRepositoryDB       _pokemonRepositorDB;

              private IConfiguration _configuration;

              public PokemonController      (IPokemonRepositoryAPI      pokemonRepositoryAPI,     IPokemonRepositoryDB pokemonRepositorDB,      IConfiguration configuration)
              {
                  _pokemonRepositoryAPI =       pokemonRepositoryAPI;
                  _pokemonRepositorDB =       pokemonRepositorDB;
                  _configuration = configuration;
              }

              public async Task Testear()
              {

                  try
                  {
                      var pokemonFromDB = await       _pokemonRepositorDB.GetByName     ("test01");

                  }
                  catch (Exception e)
                  {
                      //TODO
                      var a = e;
                  }
              }
              public async Task<ActionResult> Index     (string url)
              {
                  if(url == null)
                  {
                      url = _configuration["API_URL"];
                  }

                  var pokemonFromAPI =      _pokemonRepositoryAPI.GetItems(url);
                  var pokemons = pokemonFromAPI.      Results;

                  PokemonViewModel pokemonViewModel =       new PokemonViewModel();
                  pokemonViewModel.Count =      pokemonFromAPI.Count;
                  pokemonViewModel.Next =       pokemonFromAPI.Next;
                  pokemonViewModel.Previous =       pokemonFromAPI.Previous;

                  if (pokemons != null)
                  {
                      List<Pokemon> pokemonsTemp = new      List<Pokemon>();

                      foreach (var pokemon in pokemons)
                      {
                          var pokemonFromDB = await       _pokemonRepositorDB.GetByName     (pokemon.Name);

                          if (pokemonFromDB != null &&      pokemonFromDB.Name != null)
                          {
                              pokemonsTemp.Add(
                                  new Pokemon
                                  {
                                      Name = pokemon.     Name,
                                      Url = pokemon.      Url,
                                      IsSelected =      pokemonFromDB.    IsSelected
                                  });
                          }
                          else
                          {
                              pokemonsTemp.Add(
                                  new Pokemon
                                  {
                                      Name = pokemon.     Name,
                                      Url = pokemon.      Url,
                                      IsSelected =      pokemon.    IsSelected
                                  });
                          }
                      }

                      pokemonViewModel.Results =      pokemonsTemp;

                  }

                  ViewData["urlRetorn"] = url;
                  return View(pokemonViewModel);
              }

              [HttpPost]
              [ValidateAntiForgeryToken]
              public async Task<ActionResult> Index     (PokemonViewModel model, string     urlRetorn)
              {
                  if (model.Results != null)
                  {
                      foreach (var pokemon in model.      Results)
                      {
                          Pokemon pok = new Pokemon()
                          {
                              Name = pokemon.Name,
                              Url = pokemon.Url,
                              IsSelected = pokemon.     IsSelected
                          };

                          var resul = await       _pokemonRepositorDB.Update      (pok);

                          if (resul == 2)
                          {
                              //No existe en base hay       que insertarlo.
                              resul = await       _pokemonRepositorDB.      Insert(pok);
                          }

                          if (resul == 0)
                          {
                              Console.WriteLine     ($"Existio un problema      con {pokemon.Name}");
                          }
                      }
                  }

                  return RedirectToAction("index",      "Pokemon", new { url = urlRetorn });
              }
          }
      }
    ```

  - Add Views
    ```cs
      @model PruebaSedemi_00.MVC.ViewModels.PokemonViewModel ;

      @{
          ViewData["Title"] = "Pokemon List Page";
          string uriLastRecord = "";
          string urlRetorn = ViewData["urlRetorn"].ToString();
      }

      <div class="text-center">
          @if (@Model.Results != null)
          {
              Uri myUri = new Uri("https://localhost.com/",         UriKind.Absolute);
              if (Model.Next != null)
              {
                  string str = (string)Model.Next;
                  myUri = new Uri(str, UriKind.Absolute);
              }
              else
              {
                  if (Model.Previous != null)
                  {
                      string str = (string)Model.Previous;
                      myUri = new Uri(str, UriKind.Absolute);
                  } else
                  {
                      var pathValue = $"{Context.Request.Path.        Value}";
                      var queryString = $"{Context.Request.       QueryString}";

                  }
              }


              if (Model.Count != null)
              {
                  string lastRecord = ((Model.Count) - 1).ToString        ();
                  uriLastRecord = myUri.Scheme + "://" + myUri.       Host  + myUri.LocalPath + "?offset=" +        lastRecord + "&limit=20";

              }
              else
              {
                  uriLastRecord = myUri.Scheme + "//" + myUri.Host        +  myUri.LocalPath;
              }

              <form method="post" asp-route-urlRetorn="@urlRetorn">
                  <table>
                      <thead>
                          <tr>

                              <th>Name</th>
                              <th>URL</th>
                          </tr>
                      </thead>
                      <tbody>
                          @for (int i = 0; i < @Model.Results.        Count; i++)
                          {
                              <tr>
                                  <td>
                                      <div class="form-check m-1">
                                          <input type="hidden"        asp-for="@Model.Results       [i].Name" />
                                          <input type="hidden"        asp-for="@Model.Results       [i].Url" />
                                          <input asp-for="@Model.       Results[i].IsSelected"        class="form-check-input"      />
                                          <label        class="form-check-label"        asp-for="@Model.Results     [i].IsSelected">
                                              @Model.Results[i].        Name
                                          </label>
                                      </div>
                                  </td>
                                  <td>
                                      <a href="@Model.Results[i].       Url" target="_blank"        rel="noopener">@Model.Results     [i].Url</a>
                                  </td>
                              </tr>
                          }
                      </tbody>
                  </table>
                  <div>
                      <a asp-controller="Pokemon"         asp-action="Index" class="btn btn-primary"        style="width:auto">First</a>
                      <a asp-controller="Pokemon"         asp-action="Index" asp-route-url="@Model.       Previous" class="btn btn-primary"       style="width:auto">Previous</a>
                      <a asp-controller="Pokemon"         asp-action="Index" asp-route-url="@Model.       Next" class="btn btn-primary"       style="width:auto">Next</a>
                      <a asp-controller="Pokemon"         asp-action="Index"        asp-route-url="@uriLastRecord" class="btn       btn-primary" style="width:auto">Last</a>


                      <input type="submit" value="Guardar"        class="btn btn-primary" style="width:auto" />
                      <a asp-controller="Home" asp-action="Index"         class="btn btn-primary"         style="width:auto">Cancel</a>
                  </div>
              </form>

          }
          else
          {
              <h1 class="display-4">No hay datos</h1>

          }
      </div>
    ```
