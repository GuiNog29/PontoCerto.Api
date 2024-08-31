using Microsoft.AspNetCore.Mvc;
using PontoCerto.Application.DTOs;
using PontoCerto.Application.Helpers;
using PontoCerto.Application.Interfaces;

namespace PontoCerto.Api.Controllers
{
    public class DepartamentoController : Controller
    {
        private readonly IValidadorErro _validadorErro;
        private readonly IDepartamentoService _departamentoService;

        public DepartamentoController(IValidadorErro validadorErro, IDepartamentoService departamentoService)
        {
            _validadorErro = validadorErro;
            _departamentoService = departamentoService;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var listaDepartamentos = await _departamentoService.BuscarTodosDepartamentos();
                return View(listaDepartamentos);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("listar todos os departamentos", ex);
            }
        }

        public IActionResult CadastrarDepartamento()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CadastrarDepartamento([FromForm] DepartamentoDto departamentoDto)
        {
            if (!ModelState.IsValid)
                return View(departamentoDto);

            try
            {
                var departamentoCadastrado = await _departamentoService.CadastrarDepartamento(departamentoDto);
                return RedirectToAction(nameof(BuscarDepartamentoPorId), new { departamentoId = departamentoCadastrado.Id });
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("cadastrar departamento", ex);
            }
        }

        public async Task<ActionResult> BuscarDepartamentoPorId(int departamentoId)
        {
            try
            {
                var departamento = await _departamentoService.BuscarDepartamentoPorId(departamentoId);
                if (departamento == null)
                    return NotFound();

                return View(departamento);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("buscar departamento por Id", ex);
            }
        }

        public async Task<ActionResult> AtualizarDepartamento(int departamentoId)
        {
            try
            {
                var departamento = await _departamentoService.BuscarDepartamentoPorId(departamentoId);
                if (departamento == null)
                    return NotFound();

                return View(departamento);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("atualizar departamento", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarDepartamento([FromForm] DepartamentoDto departamentoDto, int departamentoId)
        {
            if (!ModelState.IsValid)
                return View(departamentoDto);

            try
            {
                await _departamentoService.AtualizarDepartamento(departamentoDto, departamentoId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("atualizar departamento", ex);
            }
        }

        public async Task<ActionResult> BuscarParaExcluirDepartamento(int departamentoId)
        {
            try
            {
                var departamento = await _departamentoService.BuscarDepartamentoPorId(departamentoId);
                if (departamento == null)
                    return NotFound();

                return View(departamento);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("buscar para excluir departamento", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirDepartamento(int departamentoId)
        {
            try
            {
                await _departamentoService.ExcluirDepartamento(departamentoId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("excluir departamento", ex);
            }
        }

        public async Task<ActionResult> GerarResultadoDepartamento(IEnumerable<PessoaDto> pessoas)
        {
            try
            {
                var resultado = await _departamentoService.GerarResultadoDepartamento(pessoas);
                return View(resultado);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("gerar resultado do departamento", ex);
            }
        }

        public async Task<ActionResult> LerArquivos(string caminhoPasta)
        {
            try
            {
                var pessoas = await _departamentoService.LerArquivos(caminhoPasta);
                return View("PessoasImportadas", pessoas);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("ler arquivos CSV", ex);
            }
        }
    }
}
