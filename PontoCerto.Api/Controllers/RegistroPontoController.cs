using Microsoft.AspNetCore.Mvc;
using PontoCerto.Application.DTOs;
using PontoCerto.Application.Helpers;
using PontoCerto.Application.Interfaces;

namespace PontoCerto.Api.Controllers
{
    public class RegistroPontoController : Controller
    {
        private readonly IValidadorErro _validadorErro;
        private readonly IRegistroPontoService _registroPontoService;

        public RegistroPontoController(IValidadorErro validadorErro, IRegistroPontoService registroPontoService)
        {
            _validadorErro = validadorErro;
            _registroPontoService = registroPontoService;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var pessoaId = PegarValorPessoaId();
                ViewBag.PessoaId = pessoaId;

                var listaRegistrosPontoPessoa = await _registroPontoService.BuscarTodosRegistrosPontoPessoa(pessoaId);
                return View(listaRegistrosPontoPessoa);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("listar todas os registros de ponto da pessoa", ex);
            }
        }

        public IActionResult Cadastrar()
        {
            var pessoaId = PegarValorPessoaId();
            ViewBag.PessoaId = pessoaId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cadastrar([FromForm] RegistroPontoDto registroPontoDto)
        {
            if (!ModelState.IsValid)
                return View(registroPontoDto);

            try
            {
                var pessoaId = PegarValorPessoaId();
                ViewBag.PessoaId = pessoaId;

                registroPontoDto.PessoaId = pessoaId;
                var registroPontoCadastrado = await _registroPontoService.CadastrarRegistroPonto(registroPontoDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("cadastrar registro ponto", ex);
            }
        }

        public async Task<ActionResult> Detalhes(int registroPontoId)
        {
            try
            {
                var pessoaId = PegarValorPessoaId();
                ViewBag.PessoaId = pessoaId;

                var registroPonto = await _registroPontoService.BuscarRegistroPontoPorId(registroPontoId);
                if (registroPonto == null)
                    return NotFound();

                return View(registroPonto);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("buscar registro ponto por Id", ex);
            }
        }

        public async Task<IActionResult> Editar(int registroPontoId)
        {
            try
            {
                var pessoaId = PegarValorPessoaId();
                ViewBag.PessoaId = pessoaId;

                var registroPonto = await _registroPontoService.BuscarRegistroPontoPorId(registroPontoId);
                if (registroPonto == null)
                    return NotFound();

                return View(registroPonto);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("buscar registro ponto por Id", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(RegistroPontoDto registroPontoDto, int registroPontoId)
        {
            if (!ModelState.IsValid || registroPontoId <= 0)
                return View(registroPontoDto);

            try
            {
                var pessoaId = PegarValorPessoaId();
                ViewBag.PessoaId = pessoaId;

                registroPontoDto.Id = registroPontoId;
                registroPontoDto.PessoaId = pessoaId;
                await _registroPontoService.AtualizarRegistroPonto(registroPontoDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("atualizar registro ponto", ex);
            }
        }

        public async Task<IActionResult> Excluir(int registroPontoId)
        {
            try
            {
                var pessoaId = PegarValorPessoaId();
                ViewBag.PessoaId = pessoaId;

                var registroPonto = await _registroPontoService.BuscarRegistroPontoPorId(registroPontoId);
                if (registroPonto == null)
                    return NotFound();

                return View(registroPonto);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("buscar registro ponto por Id", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirPorId(int registroPontoId)
        {
            try
            {
                var pessoaId = PegarValorPessoaId();
                ViewBag.PessoaId = pessoaId;

                await _registroPontoService.ExcluirRegistroPonto(registroPontoId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("excluir registro ponto", ex);
            }
        }

        private int PegarValorPessoaId()
        {
            var pessoaId = HttpContext.Session.GetInt32("ssnPessoaId");
            if (!pessoaId.HasValue)
                throw new Exception("Não foi localizado o id do colaborador, por favor selecione ele novamente.");

            return pessoaId.Value;
        }
    }
}
