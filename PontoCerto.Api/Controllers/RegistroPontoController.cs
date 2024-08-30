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
                var listaRegistrosPonto = await _registroPontoService.BuscarTodosRegistrosPonto();
                return View(listaRegistrosPonto);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("listar todas os registros de ponto", ex);
            }
        }

        public async Task<ActionResult> BuscarTodosRegistrosPontoPessoa(int pessoaId)
        {
            try
            {
                var listaRegistrosPontoPessoa = await _registroPontoService.BuscarTodosRegistrosPontoPessoa(pessoaId);
                return View(listaRegistrosPontoPessoa);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("listar todas os registros de ponto da pessoa", ex);
            }
        }

        public IActionResult CadastrarRegistroPonto()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CadastrarRegistroPonto([FromForm] RegistroPontoDto registroPontoDto)
        {
            if (!ModelState.IsValid)
                return View(registroPontoDto);

            try
            {
                var registroPontoCadastrado = await _registroPontoService.CadastrarRegistroPonto(registroPontoDto);
                return RedirectToAction(nameof(registroPontoCadastrado));
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("cadastrar registro ponto", ex);
            }
        }

        public async Task<ActionResult> BuscarRegistroPontoPorId(int registroPontoId)
        {
            try
            {
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
        public async Task<IActionResult> AtualizarRegistroPonto(RegistroPontoDto registroPontoDto, int registroPontoId)
        {
            if (!ModelState.IsValid)
                return View(registroPontoDto);

            try
            {
                await _registroPontoService.AtualizarRegistroPonto(registroPontoDto, registroPontoId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("atualizar registro ponto", ex);
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirRegistroPonto(int registroPontoId)
        {
            try
            {
                await _registroPontoService.ExcluirRegistroPonto(registroPontoId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("excluir registro ponto", ex);
            }
        }
    }
}
