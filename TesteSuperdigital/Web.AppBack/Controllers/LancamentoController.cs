using System;
using Microsoft.AspNetCore.Mvc;
using Web.AppBack.Models;
using Repository;

namespace Web.AppBack.Controllers
{
    [Produces("application/json")]
    [Route("api/lancamento")]
    [ApiController]
    public class LancamentoController : Controller
    {
        ContaCorrenteRepository _ContaCorrenteRepository;

        public LancamentoController(ContaCorrenteRepository contaCorrenteRepository)
        {
            _ContaCorrenteRepository = contaCorrenteRepository;
        } 
        
        // POST api/lancamento>
        [HttpPost]
        public ActionResult PostOperacao([FromBody]LancamentoModel Model)
        {
            try
            {
                string message = null;

                ValidateModel(Model);

                if (!ModelState.IsValid)
                {
                    foreach (var item in ModelState[""].Errors)
                    {
                        message += string.Format("{0}\n", item.ErrorMessage);
                    }

                    return BadRequest(message);
                }
                else
                {
                    _ContaCorrenteRepository.Tranferencia(Model.ContaOrigem, Model.ContaDestino, Model.Valor);

                    return Ok();
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        void ValidateModel(LancamentoModel Model)
        {
            try
            {
                if (Model.ContaOrigem == 0)
                {
                    ModelState.AddModelError("", "Conta origem é obrigatório!");
                }
                else if (!_ContaCorrenteRepository.CheckContaExiste(Model.ContaOrigem))
                {
                    ModelState.AddModelError("", "Conta origem inválida!");
                }
                else if (!_ContaCorrenteRepository.CheckSaldoExiste(Model.ContaOrigem, Model.Valor))
                {
                    ModelState.AddModelError("", "Saldo insuficiente!");
                }

                if (Model.ContaDestino == 0)
                {
                    ModelState.AddModelError("", "Conta destino é obrigatório!");
                }
                else if (!_ContaCorrenteRepository.CheckContaExiste(Model.ContaDestino))
                {
                    ModelState.AddModelError("", "Conta destino inválida!");
                }

                if (Model.Valor <= 0)
                {
                    ModelState.AddModelError("", "Valor inválido!");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocorreu erro na validação das informações!");
            }
        }
    }
}
