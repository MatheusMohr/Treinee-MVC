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
                var retorno = await _apiService.PostAsync<EscolaModel>("/Escola/Adicionar_Escola", model);
            }
            return View(model);
        }
    }
}
