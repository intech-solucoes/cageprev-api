using System;
using System.Collections.Generic;
using System.Text;
using Intech.PrevSystemWeb.Entidades;

namespace Intech.PrevSystemWeb.Cageprev.Negocio.Relatorios
{
    public class ExtratoDeEmprestimo
    {
        public ContratoEmprestimoEntidade _contratoEmprestimo { get; set; }

        public ExtratoDeEmprestimo(ContratoEmprestimoEntidade contratoEmprestimo)
        {
            _contratoEmprestimo = contratoEmprestimo;
        }
    }
}
