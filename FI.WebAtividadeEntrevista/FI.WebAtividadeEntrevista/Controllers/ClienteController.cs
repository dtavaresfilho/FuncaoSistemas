using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBene = new BoBeneficiario();

            if (!bo.ValidaCpf(model.Cpf))
            {
                var erros = new List<string>();
                erros.Add("CPF Inválido!");

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else if (bo.VerificarExistencia(model.Cpf))
            {
                var erros = new List<string>();
                erros.Add("Já existe um cliente cadastrado com o CPF informado.");

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    Cpf = model.Cpf
                });

                if (model.Beneficiarios != null)
                {
                    foreach (var beneficiario in model.Beneficiarios.Where(w => w.Id == 0))
                    {
                        if (!bo.ValidaCpf(beneficiario.Cpf))
                        {
                            var erros = new List<string>();
                            erros.Add("CPF do Beneficiario " + beneficiario.Nome + " Inválido!");

                            Response.StatusCode = 400;
                            return Json(string.Join(Environment.NewLine, erros));
                        }
                        else
                        {
                            boBene.Incluir(new Beneficiario()
                            {
                                Nome = beneficiario.Nome,
                                Cpf = beneficiario.Cpf,
                                IdCliente = model.Id
                            });
                        }

                    }

                    foreach (var beneficiario in model.Beneficiarios.Where(w => w.Id != 0 && !w.Exclusao))
                    {
                        if (!bo.ValidaCpf(beneficiario.Cpf))
                        {
                            var erros = new List<string>();
                            erros.Add("CPF do Beneficiario " + beneficiario.Nome + " Inválido!");

                            Response.StatusCode = 400;
                            return Json(string.Join(Environment.NewLine, erros));
                        }
                        else
                        {
                            boBene.Alterar(new Beneficiario()
                            {
                                Nome = beneficiario.Nome,
                                Cpf = beneficiario.Cpf,
                                IdCliente = beneficiario.IdCliente,
                                Id = beneficiario.Id
                            });
                        }

                    }

                    foreach (var beneficiario in model.Beneficiarios.Where(w => w.Exclusao))
                    {
                        boBene.Excluir(new Beneficiario()
                        {
                            Id = beneficiario.Id
                        });
                    }
                }

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBene = new BoBeneficiario();

            bo.Alterar(new Cliente()
            {
                Id = model.Id,
                CEP = model.CEP,
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                Telefone = model.Telefone,
                Cpf = model.Cpf
            });

            if (model.Beneficiarios != null)
            {
                foreach (var beneficiario in model.Beneficiarios.Where(w => w.Id == 0))
                {
                    if (!bo.ValidaCpf(beneficiario.Cpf))
                    {
                        var erros = new List<string>();
                        erros.Add("CPF do Beneficiario " + beneficiario.Nome + " Inválido!");

                        Response.StatusCode = 400;
                        return Json(string.Join(Environment.NewLine, erros));
                    }
                    else if (boBene.VerificarExistencia(new Beneficiario() { IdCliente = beneficiario.IdCliente, Cpf = beneficiario.Cpf}))
                    {
                        var erros = new List<string>();
                        erros.Add("Já existe um beneficiario cadastrado com o CPF informado para o cliente.");

                        Response.StatusCode = 400;
                        return Json(string.Join(Environment.NewLine, erros));
                    }
                    else
                    {
                        boBene.Incluir(new Beneficiario()
                        {
                            Nome = beneficiario.Nome,
                            Cpf = beneficiario.Cpf,
                            IdCliente = beneficiario.IdCliente
                        });
                    }

                }

                foreach (var beneficiario in model.Beneficiarios.Where(w => w.Id != 0 && !w.Exclusao))
                {
                    if (!bo.ValidaCpf(beneficiario.Cpf))
                    {
                        var erros = new List<string>();
                        erros.Add("CPF do Beneficiario " + beneficiario.Nome + " Inválido!");

                        Response.StatusCode = 400;
                        return Json(string.Join(Environment.NewLine, erros));
                    }
                    else
                    {
                        boBene.Alterar(new Beneficiario()
                        {
                            Nome = beneficiario.Nome,
                            Cpf = beneficiario.Cpf,
                            IdCliente = beneficiario.IdCliente,
                            Id = beneficiario.Id
                        });
                    }

                }

                foreach (var beneficiario in model.Beneficiarios.Where(w => w.Exclusao))
                {
                    boBene.Excluir(new Beneficiario()
                    {
                        Id = beneficiario.Id
                    });
                }
            }
            return Json("Cadastro alterado com sucesso");

        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    Cpf = cliente.Cpf
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}