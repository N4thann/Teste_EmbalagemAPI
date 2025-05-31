using SrManoelLoja.SeedWork.Validate;
using SrManoelLoja.SeedWork;

namespace SrManoelLoja.ValueObjects
{
    public class Dimensao : ValueObject
    {
        public Dimensao() { }

        public Dimensao(decimal altura, decimal largura, decimal comprimento) 
        {
            Validate.GreaterThanDecimal(altura, 0, nameof(altura));
            Validate.GreaterThanDecimal(largura, 0, nameof(largura));
            Validate.GreaterThanDecimal(comprimento, 0, nameof(comprimento));

            Altura = altura;
            Largura = largura;
            Comprimento = comprimento;
        }

        public decimal Altura { get; private set; }

        public decimal Largura { get; private set; }

        public decimal Comprimento { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Altura;
            yield return Largura;
            yield return Comprimento;
        }

        public override string ToString()
        {
            return $"{Altura}X({Largura}X{Comprimento})";
        }
    }
}
