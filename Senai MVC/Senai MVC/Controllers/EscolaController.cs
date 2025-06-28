using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Newtonsoft.Json;
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
        public async Task<IActionResult> Form()
        {
            var model = new EscolaModel();
            await AlimentarEstados(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Form(EscolaModel model)
        {
            if (ModelState.IsValid) 
            {
                if (model.Endereco.Id == null)
                    model.Endereco.Id = 0;
                var retorno = await _apiService.PostAsync<EscolaModel>("/Escola/Salvar", model);
                return Redirect("index");
            }
            await AlimentarEstados(model); 
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Editar(long id)
        {
            var model = await _apiService.GetAsync<EscolaModel>($"/Escola/Obter_Por_Id?id={id}");
            if (model.Endereco == null)
                model.Endereco = new EnderecoModel();
            await AlimentarEstados(model);
            if(!string.IsNullOrEmpty(model.Endereco.Estado))
            {
                await AlimentarCidades(model, model.Endereco.Estado);
            }
            return View("Form", model);
        }

        [HttpGet]
        public async Task<IActionResult> Remover(long id)
        {
            var model = await _apiService.DeleteAsync($"/Escola/Remover_Escola?id={id}");
            TempData["SuccessMessage"] = "Escola removida com sucesso!";
            return Redirect("/Escola/Index");
        }

        private async Task AlimentarEstados(EscolaModel model)
        {
            var estados = await _apiService.PegarEstados<EstadoIBGE>();
            model.Estados = estados.OrderBy(e => e.Nome)
                .Select(e => new SelectListItem
                {
                    Value = e.Sigla,
                    Text = e.Nome
                })
                .ToList(); 
        }

        public async Task AlimentarCidades(EscolaModel model, string uf)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{uf}/distritos");

            if (!response.IsSuccessStatusCode)
            {
                model.Cidades = new List<SelectListItem>();
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var cidades = JsonConvert.DeserializeObject<List<CidadeIBGE>>(json);

            model.Cidades = cidades
                .OrderBy(c => c.Nome)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nome,
                    Selected = c.Id.ToString() == model.Endereco.Cidade.ToString()
                })
                .ToList();
        }



        [HttpGet]

        public async Task<IActionResult> ObterCidadesPorUF(string uf)
        {
            using var httpclient = new HttpClient();
            var response = await httpclient.GetAsync($"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{uf}/distritos");

            if (!response.IsSuccessStatusCode)
                return BadRequest("Erro ao buscar cidades");

            var json = await response.Content.ReadAsStringAsync();
            var cidades = JsonConvert.DeserializeObject<List<CidadeIBGE>>(json);

            var resultado = cidades
                .OrderBy(c => c.Nome)
                .Select(c => new { id = c.Id, nome = c.Nome })
                .ToList();

            return Json(resultado);
        }

    }
}
