﻿using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PontoCerto.Application.DTOs;
using PontoCerto.Application.Helpers;
using System.Text.Json.Serialization;
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

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cadastrar([FromForm] DepartamentoDto departamentoDto)
        {
            if (!ModelState.IsValid)
                return View(departamentoDto);

            try
            {
                var departamentoCadastrado = await _departamentoService.CadastrarDepartamento(departamentoDto);
                return RedirectToAction(nameof(Detalhes), new { departamentoId = departamentoCadastrado.Id });
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("cadastrar departamento", ex);
            }
        }

        public async Task<ActionResult> Detalhes(int departamentoId)
        {
            try
            {
                var departamento = await _departamentoService.BuscarDepartamentoPorId(departamentoId);
                if (departamento == null)
                    return NotFound();

                HttpContext.Session.SetInt32("ssnDepartamentoId", departamentoId);
                return View(departamento);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("buscar departamento por Id", ex);
            }
        }

        public async Task<ActionResult> Editar(int departamentoId)
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
        public async Task<IActionResult> Editar([FromForm] DepartamentoDto departamentoDto, int departamentoId)
        {
            if (!ModelState.IsValid)
                return View(departamentoDto);

            try
            {
                departamentoDto.Id = departamentoId;
                await _departamentoService.AtualizarDepartamento(departamentoDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("atualizar departamento", ex);
            }
        }

        public async Task<ActionResult> Excluir(int departamentoId)
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
        public async Task<IActionResult> ExcluirPorId(int departamentoId)
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

        public async Task<ActionResult> Importar(string caminhoPasta)
        {
            try
            {
                var listaPessoas = await _departamentoService.LerArquivos(caminhoPasta);
                var resultadoDepartamento = await _departamentoService.GerarResultadoDepartamento(listaPessoas);

                // Configurar a serialização para lidar com ciclos de referência
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                // Retornar o resultado em formato JSON
                return new JsonResult(resultadoDepartamento, options);
            }
            catch (Exception ex)
            {
                return _validadorErro.TratarErro("ler arquivos CSV", ex);
            }
        }
    }
}
