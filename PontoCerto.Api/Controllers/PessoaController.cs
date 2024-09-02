using Microsoft.AspNetCore.Mvc;
using PontoCerto.Application.DTOs;
using PontoCerto.Application.Helpers;
using PontoCerto.Application.Interfaces;

namespace PontoCerto.Api.Controllers
{
    public class PessoaController : Controller
    {
        private readonly IValidadorErro _validadorErro;
        private readonly IPessoaService _pessoaService;

        public PessoaController(IValidadorErro validadorErro, IPessoaService pessoaService)
        {
            _validadorErro = validadorErro;
            _pessoaService = pessoaService;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var departamentoId = PegarValorDepartamentoId();
                ViewBag.DepartamentoId = departamentoId;

                var listaPessoas = await _pessoaService.BuscarTodasPessoas(departamentoId);
                return View(listaPessoas);
            }
            catch (Exception ex) 
            {
                return _validadorErro.TratarErro("listar todas as pessoas", ex);
            }
        }

        public IActionResult Cadastrar()
        {
            var departamentoId = PegarValorDepartamentoId();
            ViewBag.DepartamentoId = departamentoId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cadastrar(PessoaDto pessoaDto)
        {
            if (!ModelState.IsValid)
                return View(pessoaDto);

            try
            {
                var departamentoId = PegarValorDepartamentoId();
                ViewBag.DepartamentoId = departamentoId;

                pessoaDto.DepartamentoId = departamentoId;
                var pessoaCadastrada = await _pessoaService.CadastrarPessoa(pessoaDto);
                return RedirectToAction(nameof(Detalhes), new { pessoaId = pessoaCadastrada.Id, departamentoId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(pessoaDto);
            }
        }

        public async Task<ActionResult> Detalhes(int pessoaId)
        {
            try
            {
                var departamentoId = PegarValorDepartamentoId();
                ViewBag.DepartamentoId = departamentoId;

                var pessoa = await _pessoaService.BuscarPessoaPorId(pessoaId);
                if (pessoa == null)
                    return NotFound();

                HttpContext.Session.SetInt32("ssnPessoaId", pessoaId);

                return View(pessoa);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("buscar pessoa por Id", ex);
            }
        }

        public async Task<ActionResult> Editar(int pessoaId)
        {
            try
            {
                var departamentoId = PegarValorDepartamentoId();
                ViewBag.DepartamentoId = departamentoId;

                var pessoa = await _pessoaService.BuscarPessoaPorId(pessoaId);
                if (pessoa == null)
                    return NotFound();

                return View(pessoa);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("buscar pessoa por Id", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar([FromForm] PessoaDto pessoaDto, int pessoaId)
        {
            if (!ModelState.IsValid)
                return View(pessoaDto);

            try
            {
                var departamentoId = PegarValorDepartamentoId();
                ViewBag.DepartamentoId = departamentoId;

                pessoaDto.Id = pessoaId;
                pessoaDto.DepartamentoId = departamentoId;
                ViewBag.DepartamentoId = departamentoId;
                await _pessoaService.AtualizarPessoa(pessoaDto, pessoaId);
                return RedirectToAction(nameof(Detalhes), new { pessoaId, departamentoId });
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("atualizar pessoa", ex);
            }
        }

        public async Task<ActionResult> Excluir(int pessoaId)
        {
            try
            {
                var departamentoId = PegarValorDepartamentoId();
                ViewBag.DepartamentoId = departamentoId;

                var pessoa = await _pessoaService.BuscarPessoaPorId(pessoaId);
                if (pessoa == null)
                    return NotFound();

                return View(pessoa);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("buscar pessoa por Id", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirPorId(int pessoaId)
        {
            try
            {
                var departamentoId = PegarValorDepartamentoId();
                ViewBag.DepartamentoId = departamentoId;

                await _pessoaService.ExcluirPessoa(pessoaId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("excluir pessoa", ex);
            }
        }

        private int PegarValorDepartamentoId()
        {
            var departamentoId = HttpContext.Session.GetInt32("ssnDepartamentoId");
            if (!departamentoId.HasValue)
                throw new Exception("Não foi localizado o id do departamento, por favor selecione ele novamente.");

            return departamentoId.Value;
        }
    }
}
