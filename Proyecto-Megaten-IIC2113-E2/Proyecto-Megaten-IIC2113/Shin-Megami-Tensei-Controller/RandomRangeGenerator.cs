namespace Shin_Megami_Tensei;

public class RandomRangeGenerator
{
    private static Random _random = new Random();
    
    public static int GetRandomFromRange(string range)
    {
        try
        {
            // Dividir el string por el guión
            string[] parts = range.Split('-');
            
            if (parts.Length != 2)
            {
                throw new ArgumentException("El formato del rango debe ser 'min-max'");
            }
            
            // Convertir a enteros
            int min = int.Parse(parts[0].Trim());
            int max = int.Parse(parts[1].Trim());
            
            // Validar que min <= max
            if (min > max)
            {
                throw new ArgumentException("El valor mínimo no puede ser mayor que el máximo");
            }
            
            // Devolver un número aleatorio en el rango (incluyendo ambos extremos)
            return _random.Next(min, max + 1);
        }
        catch (Exception ex)
        {
            // Puedes manejar la excepción de otra forma si lo prefieres
            throw new ArgumentException("Formato de rango inválido. Use 'min-max' (ej. '2-6')", ex);
        }
    }
}