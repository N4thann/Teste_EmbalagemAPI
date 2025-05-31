using SrManoelLoja.SeedWork;

namespace SrManoelLoja.ValueObjects
{
    public class TipoDeCaixa : ValueObject
    {
        public static readonly TipoDeCaixa Caixa1 = new TipoDeCaixa("Caixa1", new Dimensao(30, 40, 80));
        public static readonly TipoDeCaixa Caixa2 = new TipoDeCaixa("Caixa2", new Dimensao(80, 50, 40));
        public static readonly TipoDeCaixa Caixa3 = new TipoDeCaixa("Caixa3", new Dimensao(50, 80, 60));

        public static readonly IReadOnlyList<TipoDeCaixa> TiposDisponiveis = new List<TipoDeCaixa>
        {
            Caixa1, Caixa2, Caixa3
        };

        private TipoDeCaixa() { }

        private TipoDeCaixa(string nome, Dimensao dimensoes)
        {
            Nome = nome;
            Dimensoes = dimensoes;
        }

        public string Nome { get; private set; }
        public Dimensao Dimensoes { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Nome;
            yield return Dimensoes;
        }

        public override string ToString() => $"{Nome} ({Dimensoes})";
    }
}
