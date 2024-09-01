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

        public async Task<ActionResult> Index(int departamentoId)
        {
            try
            {
                ViewBag.DepartamentoId = departamentoId;
                var listaPessoas = await _pessoaService.BuscarTodasPessoas();
                return View(listaPessoas);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("listar todas as pessoas", ex);
            }
        }

        public IActionResult CadastrarPessoa(int departamentoId)
        {
            ViewBag.DepartamentoId = departamentoId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CadastrarPessoa(PessoaDto pessoaDto, int departamentoId)
        {
            if (!ModelState.IsValid)
                return View(pessoaDto);

            try
            {
                pessoaDto.DepartamentoId = departamentoId;
                var pessoaCadastrada = await _pessoaService.CadastrarPessoa(pessoaDto);
                return RedirectToAction(nameof(BuscarPessoaPorId), new { departamentoId = pessoaCadastrada.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(pessoaDto);
            }
        }

        public async Task<ActionResult> BuscarPessoaPorId(int pessoaId)
        {
            try
            {
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
        public async Task<IActionResult> AtualizarPessoa([FromForm] PessoaDto pessoaDto, int pessoaId)
        {
            if (!ModelState.IsValid)
                return View(pessoaDto);

            try
            {
                await _pessoaService.AtualizarPessoa(pessoaDto, pessoaId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("atualizar pessoa", ex);
            }
        }

        public async Task<ActionResult> ExcluirPessoaId(int pessoaId)
        {
            try
            {
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

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirPessoa(int pessoaId)
        {
            try
            {
                await _pessoaService.ExcluirPessoa(pessoaId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("excluir pessoa", ex);
            }
        }
    }
}
