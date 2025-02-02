using System.Runtime.CompilerServices;

namespace EstoqueApi.Entities.Enums
{
    public static class EnumExtension
    {
        public static bool ValidateLocalStatus(this Status status, Local local)
        {
            return status switch
            {
                Status.aguardandoPedido => local == Local.loja,
                Status.aguardandoArmação => local == Local.laboratorio,
                Status.aguardandoLentes => local == Local.laboratorio,
                Status.emProducao => local == Local.montagem,
                Status.entregue => local == Local.loja,
                Status.cancelado => true,
                Status.problema => true,
                _ => false




            };

        }
    }
}
