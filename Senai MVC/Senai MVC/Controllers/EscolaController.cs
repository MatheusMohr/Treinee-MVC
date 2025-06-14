using Microsoft.AspNetCore.Mvc;
using Senai_MVC.Models.Escola;
using SenaiMvc.Service.Interface;

namespace Senai_MVC.Controllers
{
    public class EscolaController : Controller
    {
        private readonly IApiService _apiService;
        public EscolaController(IApiService apiService)
        { 
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var escolas = await _apiService.GetAsync<List<EscolaModel>>("/Escola/Buscar_Escolas");
            return View(escolas);
        }

        [HttpGet]
        public IActionResult Form()
        {
            var model = new EscolaModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Form(EscolaModel model)
        {
            if (ModelState.IsValid) 
            {
                var retorno = await _apiService.PostAsync<EscolaModel>("/Escola/Salvar", model);
                return Redirect("index");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Editar(long id)
        {
            var model = await _apiService.GetAsync<EscolaModel>($"/Escola/Obter_Por_Id?id={id}");
            return View("Form", model);
        }

        [HttpGet]
        public async Task<IActionResult> Remover(long id)
        {
            var model = await _apiService.DeleteAsync($"/Escola/Remover_Escola?id={id}");
            TempData["SuccessMessage"] = "Escola removida com sucesso!";
            return Redirect("/Escola/Index");
        }
    }
}
